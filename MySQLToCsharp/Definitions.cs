using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp
{
    /// <summary>
    /// MySQL table definition which parsed from CreateTable query.
    /// </summary>
    public class MySqlTableDefinition
    {
        public string TableName { get; set; }
        public MySqlColumnDefinition[] ColumnDefinitions { get; set; }
        public MySqlKeyDefinition PrimaryKey { get; set; }
        public IList<MySqlKeyDefinition> UniqueKeys { get; private set; }
        public IList<MySqlKeyDefinition> IndexKeys { get; private set; }
        public string CollationName { get; set; }
        public string Engine { get; set; }

        public void AddIndexKey(MySqlKeyDefinition index)
        {
            if (IndexKeys == null)
            {
                IndexKeys = new List<MySqlKeyDefinition>();
            }
            IndexKeys.Add(index);
        }

        public void AddUniqueKey(MySqlKeyDefinition index)
        {
            if (IndexKeys == null)
            {
                UniqueKeys = new List<MySqlKeyDefinition>();
            }
            UniqueKeys.Add(index);
        }
    }

    public class MySqlColumnDataDefinition
    {
        //TODO: 型コンバーターかませる
        public string DataType { get; set; }
        public int? DataLength { get; set; }
        public bool IsUnsigned { get; set; }
    }

    public class MySqlColumnDefinition
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public MySqlColumnDataDefinition Data { get; set; }
        public bool NotNull { get; set; }
        public bool AutoIncrement { get; set; }
        public bool HasDefault { get; set; }
        public string DefaultValue { get; set; }
        public bool HasPrimaryKey { get; set; }
        public MySqlKeyDefinition PrimaryKeyReference { get; set; }
        public bool HasUniqueKey { get; set; }
        public ISet<MySqlKeyDefinition> UniqueKeysReference { get; set; }
        public bool HasIndexKey { get; set; }
        public ISet<MySqlKeyDefinition> IndexKeysReference { get; set; }

        public void AddUniqueKeysReference(MySqlKeyDefinition index)
        {
            if (UniqueKeysReference == null)
            {
                UniqueKeysReference = new HashSet<MySqlKeyDefinition>();
            }
            UniqueKeysReference.Add(index);
        }

        public void AddIndexKeysReference(MySqlKeyDefinition index)
        {
            if (IndexKeysReference == null)
            {
                IndexKeysReference = new HashSet<MySqlKeyDefinition>();
            }
            IndexKeysReference.Add(index);
        }

        public static (bool success, MySqlColumnDefinition definition) Extract(ColumnDeclarationContext context)
        {
            if (context == null) return (false, null);

            var columnDefinition = new MySqlColumnDefinition();

            // column name
            var name = context.GetChild<UidContext>();
            columnDefinition.Name = name.GetText().RemoveBackQuote();

            // check column definitions
            var definitionContext = context.GetChild<ColumnDefinitionContext>();
            if (definitionContext == null) return (false, null);

            // BITINT(20): DimensionDataTypeContext
            columnDefinition.Data = new MySqlColumnDataDefinition();
            (columnDefinition.Data.DataType, columnDefinition.Data.DataLength, columnDefinition.Data.IsUnsigned) = definitionContext.GetColumnDataDefinition();

            // NOTNULL: NullColumnConstraintContext
            var notnull = definitionContext.GetChild<NullColumnConstraintContext>();
            columnDefinition.NotNull = notnull != null;

            // AUTOINCREMENT: AutoIncrementColumnConstraintContext
            var autoincrement = definitionContext.GetChild<AutoIncrementColumnConstraintContext>();
            columnDefinition.AutoIncrement = autoincrement != null;

            // DEFAULt'0': DefaultColumnConstraintContext
            var defaultValue = definitionContext.GetChild<DefaultColumnConstraintContext>();
            columnDefinition.HasDefault = defaultValue != null;
            columnDefinition.DefaultValue = defaultValue?.GetText()?.RemoveDefaultString();

            return (true, columnDefinition);
        }
    }

    public class MySqlKeyDefinition
    {
        public string KeyName { get; set; }
        public MySqlKeyDetailDefinition[] Indexes { get; set; }

        public static (bool success, MySqlKeyDefinition definition) ExtractPrimaryKey(PrimaryKeyTableConstraintContext context)
        {
            if (context == null) return (false, null);

            var pk = context.GetIndexNames();
            var pkNames = pk.Select(x => x.RemoveBackQuote().RemoveParenthesis()).ToArray();
            var primaryKey = new MySqlKeyDefinition
            {
                KeyName = "PRIMARY KEY",
                Indexes = pkNames.Select((x, i) => new MySqlKeyDetailDefinition
                {
                    Order = i,
                    IndexKey = x,
                })
                .ToArray(),
            };
            return (true, primaryKey);
        }

        public static (bool success, MySqlKeyDefinition definition) ExtractUniqueKey(UniqueKeyTableConstraintContext context)
        {
            if (context == null) return (false, null);
            return ExtractKeyDefinition(context);
        }

        public static (bool success, MySqlKeyDefinition definition) ExtractIndexKey(IndexDeclarationContext context)
        {
            if (context == null) return (false, null);
            var simpleIndexDeclarationContext = context.GetChild<SimpleIndexDeclarationContext>();
            return ExtractKeyDefinition(simpleIndexDeclarationContext);
        }

        private static (bool success, MySqlKeyDefinition definition) ExtractKeyDefinition(ParserRuleContext context)
        {
            var indexName = context.GetChild<UidContext>().GetText();
            var indexes = context.GetChild<IndexColumnNamesContext>();
            var indexData = Enumerable.Range(0, indexes.ChildCount)
                .Select(x => indexes.GetChild(x) is IndexColumnNameContext columnName
                    ? columnName.GetText()
                    : "")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select((x, i) => new MySqlKeyDetailDefinition
                {
                    IndexKey = x.RemoveBackQuote(),
                    Order = i,
                })
                .ToArray();

            var secondaryKey = new MySqlKeyDefinition()
            {
                KeyName = indexName.RemoveBackQuote(),
                Indexes = indexData,
            };
            return (true, secondaryKey);
        }

        /// <summary>
        /// map PrimaryKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddPrimaryKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> primarykey reference mapper
            Action<MySqlColumnDefinition> mapper = column =>
            {
                column.HasPrimaryKey = true;
                column.PrimaryKeyReference = this;
            };
            AddKeyReferenceOnColumnCore(columnDefinitions, mapper);
        }

        /// <summary>
        /// map IndexKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddUniqueKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> uniquekey reference mapper
            Action<MySqlColumnDefinition> mapper = column =>
            {
                column.HasUniqueKey = true;
                column.AddUniqueKeysReference(this);
            };
            AddKeyReferenceOnColumnCore(columnDefinitions, mapper);
        }

        /// <summary>
        /// map IndexKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddIndexKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> indexkey reference mapper
            Action<MySqlColumnDefinition> mapper = column =>
            {
                column.HasIndexKey = true;
                column.AddIndexKeysReference(this);
            };
            AddKeyReferenceOnColumnCore(columnDefinitions, mapper);
        }

        public void AddKeyReferenceOnColumnCore(MySqlColumnDefinition[] columnDefinitions, Action<MySqlColumnDefinition> mapper)
        {
            var indexKeyNames = this.Indexes.Select(x => x.IndexKey).ToArray();
            foreach (var column in columnDefinitions)
            {
                if (!indexKeyNames.Contains(column.Name)) continue;

                // column -> key reference mapper
                mapper(column);

                // indexkey -> column reference
                foreach (var item in this.Indexes.Where(x => x.IndexKey == column.Name))
                {
                    item.AddColumnReference(column);
                }
            }
        }
    }

    public class MySqlKeyDetailDefinition
    {
        public int Order { get; set; }
        public string IndexKey { get; set; }
        public ISet<MySqlColumnDefinition> ColumnReference { get; set; }

        public void AddColumnReference(MySqlColumnDefinition column)
        {
            if (ColumnReference == null)
            {
                ColumnReference = new HashSet<MySqlColumnDefinition>();
            }
            ColumnReference.Add(column);
        }
    }
}
