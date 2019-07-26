using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static MySQLToCsharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp
{
    /// <summary>
    /// MySQL table definition which parsed from CreateTable query.
    /// </summary>
    public class MySqlTableDefinition
    {
        public string SchemaName { get; set; }
        public string Name { get; set; }
        public MySqlColumnDefinition[] Columns { get; set; }
        public MySqlKeyDefinition PrimaryKey { get; set; }
        public ISet<MySqlKeyDefinition> UniqueKeys { get; private set; }
        public ISet<MySqlKeyDefinition> IndexKeys { get; private set; }
        public string Collation { get; set; }
        public string Engine { get; set; }

        public void AddIndexKey(MySqlKeyDefinition index)
        {
            if (IndexKeys == null)
            {
                IndexKeys = new HashSet<MySqlKeyDefinition>();
            }
            IndexKeys.Add(index);
        }

        public void AddUniqueKey(MySqlKeyDefinition index)
        {
            if (IndexKeys == null)
            {
                UniqueKeys = new HashSet<MySqlKeyDefinition>();
            }
            UniqueKeys.Add(index);
        }

        public MySqlColumnDefinition LookupColumnDefinition(ParserRuleContext context)
        {
            var columnDeclaretionContext = context.Ascendant<ColumnDeclarationContext>();
            var columnName = columnDeclaretionContext.GetColumnName();
            var column = this.Columns.Where(x => x.Name == columnName).FirstOrDefault();
            return column;
        }

        public static (string schemaName, string tableName) ExtractTableName(TableNameContext context)
        {
            var fullIdContext = context.GetChild<FullIdContext>();
            var schemaName = "";
            var tableName = "";

            //MEMO: TableNameContext.tablename() contains schema name if query style is `schema`.`table`.
            // 3     = schema.table (expr + . + expr)
            // other = table
            if (fullIdContext.ChildCount == 3)
            {
                schemaName = fullIdContext.GetChild<UidContext>().GetText()?.RemoveBackQuote();
                tableName = fullIdContext.GetChild<UidContext>(1).GetText()?.RemoveBackQuote();
            }
            else
            { 
                tableName = fullIdContext.GetChild<UidContext>().GetText()?.RemoveBackQuote();
            }
            return (schemaName, tableName);
        }
    }

    public class MySqlColumnDefinition
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public MySqlColumnDataDefinition Data { get; set; }
        public bool AutoIncrement { get; set; }
        public bool HasDefault { get; set; }
        public string DefaultValue { get; set; }
        public GeneratedColumnDefinition GeneratedColumn { get; set; }
        public ReferenceColumnDefinition ReferenceColumn { get; set; }

        // key reference
        public MySqlKeyDefinition PrimaryKeyReference { get; set; }
        public ISet<MySqlKeyDefinition> UniqueKeysReferences { get; private set; }
        public ISet<MySqlKeyDefinition> IndexKeysReferences { get; private set; }

        public static MySqlColumnDefinition Extract(ColumnDeclarationContext context)
        {
            if (context == null) return null;

            var column = new MySqlColumnDefinition();

            // column name
            column.Name = context.GetColumnName();

            // check column definitions
            var definitionContext = context.GetChild<ColumnDefinitionContext>();
            if (definitionContext == null) return null;

            // BITINT(20): DimensionDataTypeContext
            column.Data = new MySqlColumnDataDefinition();
            (column.Data.DataType, column.Data.Length, column.Data.IsUnsigned) = definitionContext.ExtractColumnDataDefinition();

            // NOTNULL: NullColumnConstraintContext
            var notnull = definitionContext.GetChild<NullColumnConstraintContext>();
            column.Data.IsNullable = notnull == null;

            // AUTOINCREMENT: AutoIncrementColumnConstraintContext
            var autoincrement = definitionContext.GetChild<AutoIncrementColumnConstraintContext>();
            column.AutoIncrement = autoincrement != null;

            // DEFAULt'0': DefaultColumnConstraintContext
            var defaultValue = definitionContext.GetChild<DefaultColumnConstraintContext>();
            column.HasDefault = defaultValue != null;
            column.DefaultValue = defaultValue?.GetText()?.RemoveDefaultString();

            //MEMO: GeneratedColumnContext will done via Listener
            //var generatedColumnConstraintContext = definitionContext.GetChild<GeneratedColumnConstraintContext>();
            //columnDefinition.GeneratedColumn = GeneratedColumnDefinition.ExtractGeneratedColumnDefinition(generatedColumnConstraintContext);

            //MEMO: ReferenceColumnContext will done via Listener

            // Special overwrite for SERIAL DataType.
            // SERIAL is an alias for BIGINT UNSIGNED NOT NULL AUTO_INCREMENT UNIQUE.
            if (column.Data.DataType == "SERIAL")
            {
                column.Data.IsUnsigned = true;
                column.Data.IsNullable = false;
                column.AutoIncrement = true;
                //MEMO: No action for UniqueKey. It's too much special. (should add unique key clause on sql)
            }

            return column;
        }

        public void AddUniqueKeysReference(MySqlKeyDefinition index)
        {
            if (UniqueKeysReferences == null)
            {
                UniqueKeysReferences = new HashSet<MySqlKeyDefinition>();
            }
            UniqueKeysReferences.Add(index);
        }

        public void AddIndexKeysReference(MySqlKeyDefinition index)
        {
            if (IndexKeysReferences == null)
            {
                IndexKeysReferences = new HashSet<MySqlKeyDefinition>();
            }
            IndexKeysReferences.Add(index);
        }
    }

    public class MySqlColumnDataDefinition
    {
        public string DataType { get; set; }
        public int? Length { get; set; }
        public bool IsUnsigned { get; set; }
        public bool IsNullable { get; set; }
    }

    /// <summary>
    /// Generated Column info https://dev.mysql.com/doc/refman/5.7/en/create-table.html#create-table-generated-columns
    /// </summary>
    public class GeneratedColumnDefinition
    {
        public bool Always { get; set; }
        public string Statement { get; set; }
        /// <summary>
        /// STORED or VIRTUAL
        /// </summary>
        public bool IsStored { get; set; }

        /// <summary>
        /// Extract Generated Column from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static GeneratedColumnDefinition Extract(GeneratedColumnConstraintContext context)
        {
            // Get Generated Column detail: "GENERATEDALWAYSAS(hex(id))STORED"
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");

            var predicateExpressionContext = context.GetChild<PredicateExpressionContext>();
            var generatedElements = Enumerable.Range(0, context.ChildCount)
                .Select(x => context.GetChild(x))
                .Select(x => x.GetText())
                .ToArray();
            var isAlways = generatedElements.Contains("ALWAYS");
            var isStored = generatedElements.Contains("STORED");

            var statement = predicateExpressionContext.GetText();
            var generatedColumn = new GeneratedColumnDefinition()
            {
                Always = isAlways,
                IsStored = isStored,
                Statement = statement,
            };
            return generatedColumn;
        }
    }

    /// <summary>
    /// Extract Reference Column from context
    /// </summary>
    public class ReferenceColumnDefinition
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }

        public static ReferenceColumnDefinition Extract(ReferenceDefinitionContext context)
        {
            // Get Generated Column detail: "GENERATEDALWAYSAS(hex(id))STORED"
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");

            // tableName: hoge_table (TableNameContext)
            var table = context.GetChild<TableNameContext>();
            var tableName = table.GetText();

            // columnName:  (IndexColumnNamesContext -> IndexColumnNameContext -> UidContext)
            var indexColumnNamesContext = context.GetChild<IndexColumnNamesContext>();
            var columnName = Enumerable.Range(0, indexColumnNamesContext.ChildCount)
                .Select(x => indexColumnNamesContext.GetChild<IndexColumnNameContext>(x))
                .Where(x => x != null)
                .First()
                .GetChild<UidContext>()
                .GetText();
            var definition = new ReferenceColumnDefinition()
            {
                TableName = tableName,
                ColumnName = columnName,
            };
            return definition;
        }
    }

    public class MySqlKeyDefinition
    {
        public string KeyName { get; set; }
        public MySqlKeyDetailDefinition[] Indexes { get; set; }

        /// <summary>
        /// Extract PrimaryKey Definition from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MySqlKeyDefinition ExtractPrimaryKey(PrimaryKeyTableConstraintContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");

            var pk = context.GetIndexNames();
            var pkNames = pk.Select(x => x.RemoveBackQuote().RemoveParenthesis()).ToArray();
            var definition = new MySqlKeyDefinition
            {
                KeyName = "PRIMARY KEY",
                Indexes = pkNames.Select((x, i) => new MySqlKeyDetailDefinition
                {
                    Order = i,
                    IndexKey = x,
                })
                .ToArray(),
            };
            return definition;
        }

        /// <summary>
        /// Extract UniqueKey Definition from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MySqlKeyDefinition ExtractUniqueKey(UniqueKeyTableConstraintContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");
            return ExtractKeyDefinition(context);
        }

        /// <summary>
        /// Extract IndexKey Definition from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static MySqlKeyDefinition ExtractIndexKey(IndexDeclarationContext context)
        {
            if (context == null) throw new ArgumentOutOfRangeException($"{nameof(context)} is null");
            var simpleIndexDeclarationContext = context.GetChild<SimpleIndexDeclarationContext>();
            return ExtractKeyDefinition(simpleIndexDeclarationContext);
        }

        /// <summary>
        /// Extract Key Definition from context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static MySqlKeyDefinition ExtractKeyDefinition(ParserRuleContext context)
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

            var definition = new MySqlKeyDefinition()
            {
                KeyName = indexName.RemoveBackQuote(),
                Indexes = indexData,
            };
            return definition;
        }

        /// <summary>
        /// map PrimaryKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddPrimaryKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> primarykey reference mapper
            AddKeyReferenceOnColumnCore(columnDefinitions, column => column.PrimaryKeyReference = this);
        }

        /// <summary>
        /// map IndexKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddUniqueKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> uniquekey reference mapper
            AddKeyReferenceOnColumnCore(columnDefinitions, column => column.AddUniqueKeysReference(this));
        }

        /// <summary>
        /// map IndexKey and existing Column reference
        /// </summary>
        /// <param name="columnDefinitions"></param>
        public void AddIndexKeyReferenceOnColumn(MySqlColumnDefinition[] columnDefinitions)
        {
            // column -> indexkey reference mapper
            AddKeyReferenceOnColumnCore(columnDefinitions, column => column.AddIndexKeysReference(this));
        }

        /// <summary>
        /// Add Reference between Key -> Column.
        /// </summary>
        /// <param name="columnDefinitions"></param>
        /// <param name="mapper"></param>
        private void AddKeyReferenceOnColumnCore(MySqlColumnDefinition[] columnDefinitions, Action<MySqlColumnDefinition> mapper)
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
        // column reference
        public ISet<MySqlColumnDefinition> ColumnReference { get; private set; }

        /// <summary>
        /// Add Reference between Column -> Key.
        /// </summary>
        /// <param name="column"></param>
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
