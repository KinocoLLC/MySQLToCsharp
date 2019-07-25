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
        public MySqlIndexDefinition PrimaryKey { get; set; }
        public IList<MySqlIndexDefinition> IndexKeys { get; set; }
        public string CollationName { get; set; }
        public string Engine { get; set; }

        public void AddIndexKey(MySqlIndexDefinition index)
        {
            if (IndexKeys == null)
            {
                IndexKeys = new List<MySqlIndexDefinition>();
            }
            IndexKeys.Add(index);
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
        public bool IsPrimaryKey { get; set; }
        public MySqlIndexDefinition PrimaryKeyReference { get; set; }
        public bool HasIndexKey { get; set; }
        public ISet<MySqlIndexDefinition> IndexKeysReference { get; set; }

        public void AddIndexKeysReference(MySqlIndexDefinition index)
        {
            if (IndexKeysReference == null)
            {
                IndexKeysReference = new HashSet<MySqlIndexDefinition>();
            }
            IndexKeysReference.Add(index);
        }

        //TODO: UniqueKey

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

    public class MySqlIndexDefinition
    {
        public string IndexDeclarationName { get; set; }
        public MySqlIndexDataDefinition[] Indexes { get; set; }

        public static (bool success, MySqlIndexDefinition definition) ExtractPrimaryKey(PrimaryKeyTableConstraintContext context)
        {
            if (context == null) return (false, null);

            var pk = context.GetIndexNames();
            var pkNames = pk.Select(x => x.RemoveBackQuote().RemoveParenthesis()).ToArray();
            var primaryKey = new MySqlIndexDefinition
            {
                IndexDeclarationName = "PRIMARY KEY",
                Indexes = pkNames.Select((x, i) => new MySqlIndexDataDefinition
                {
                    Order = i,
                    IndexKey = x,
                })
                .ToArray(),
            };
            return (true, primaryKey);
        }

        public static (bool success, MySqlIndexDefinition definition) ExtractSecondaryIndex(IndexDeclarationContext context)
        {
            if (context == null) return (false, null);

            var child = context.GetChild<SimpleIndexDeclarationContext>();
            var indexName = child.GetChild<UidContext>().GetText();
            var indexes = child.GetChild<IndexColumnNamesContext>();
            var indexData = Enumerable.Range(0, indexes.ChildCount)
                .Select(x => indexes.GetChild(x) is IndexColumnNameContext columnName
                    ? columnName.GetText()
                    : "")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select((x, i) => new MySqlIndexDataDefinition
                {
                    IndexKey = x.RemoveBackQuote(),
                    Order = i,
                })
                .ToArray();

            var secondaryKey = new MySqlIndexDefinition()
            {
                IndexDeclarationName = indexName.RemoveBackQuote(),
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
            var indexKeyNames = this.Indexes.Select(x => x.IndexKey).ToArray();
            foreach (var column in columnDefinitions)
            {
                if (!indexKeyNames.Contains(column.Name)) continue;

                // column -> pk reference
                column.IsPrimaryKey = true;
                column.PrimaryKeyReference = this;

                // pk -> column reference
                foreach (var item in this.Indexes)
                {
                    item.AddColumnReference(column);
                }
            }
        }

        /// <summary>
        /// map IndexKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddIndexKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            var indexKeyNames = this.Indexes.Select(x => x.IndexKey).ToArray();
            foreach (var column in columnDefinitions)
            {
                if (!indexKeyNames.Contains(column.Name)) continue;

                // column -> indexkey reference
                column.HasIndexKey = true;
                column.AddIndexKeysReference(this);

                // indexkey -> column reference
                foreach (var item in this.Indexes.Where(x => x.IndexKey == column.Name))
                {
                    item.AddColumnReference(column);
                }
            }
        }

    }
    public class MySqlIndexDataDefinition
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
