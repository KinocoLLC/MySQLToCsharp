using System;
using System.Collections.Generic;
using System.Text;
using static MySQLToCSharp.Parsers.MySql.MySqlParser;

namespace MySQLToCsharp
{
    public class MySqlTableDefinition
    {
        public string TableName { get; set; }
        public MySqlColumnDefinition[] ColumnDefinition { get; set; }
        public string[] PrimaryKeyColumns { get; set; }
        // index definitions
        // collate definition
        // engine definition
        // auto increment value definition
    }

    public class MySqlColumnDataDefinition
    {
        //TODO: 数値コンバーターかませる
        public string DataType { get; set; }
        public int? DataLength { get; set; }
        public bool IsUnsigned { get; set; }
    }

    public class MySqlColumnDefinition
    {
        public string Name { get; set; }
        public MySqlColumnDataDefinition Data { get; set; }
        public bool NotNull { get; set; }
        public bool AutoIncrement { get; set; }
        public bool HasDefault { get; set; }
        public string DefaultValue { get; set; }

        public static (bool success, MySqlColumnDefinition columnDefinition) Extract(ColumnDeclarationContext context)
        {
            var columnDefinition = new MySqlColumnDefinition();
            if (context == null) return (false, null);

            // column name
            var name = context.GetChild<UidContext>(0);
            columnDefinition.Name = name.GetText().RemoveBackQuote();

            // check column definitions
            var definitionContext = context.GetChild<ColumnDefinitionContext>(0);
            if (definitionContext == null) return (false, null);

            // BITINT(20): DimensionDataTypeContext
            columnDefinition.Data = new MySqlColumnDataDefinition();
            (columnDefinition.Data.DataType, columnDefinition.Data.DataLength, columnDefinition.Data.IsUnsigned) = definitionContext.GetColumnDataDefinition();

            // NOTNULL: NullColumnConstraintContext
            var notnull = definitionContext.GetChild<NullColumnConstraintContext>(0);
            columnDefinition.NotNull = notnull != null;

            // AUTOINCREMENT: AutoIncrementColumnConstraintContext
            var autoincrement = definitionContext.GetChild<AutoIncrementColumnConstraintContext>(0);
            columnDefinition.AutoIncrement = autoincrement != null;

            // DEFAULt'0': DefaultColumnConstraintContext
            var defaultValue = definitionContext.GetChild<DefaultColumnConstraintContext>(0);
            columnDefinition.HasDefault = defaultValue != null;
            columnDefinition.DefaultValue = defaultValue?.GetText();

            return (true, columnDefinition);
        }
    }
}
