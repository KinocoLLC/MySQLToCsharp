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
        // primary key definition
        // index definitions
        // collate definition
        // engine definition
        // auto increment value definition
    }

    public class MySqlColumnDefinition
    {
        public string Name { get; set; }
        //TODO: 数値コンバーターかませる
        public string DataType { get; set; }
        public int? DataLength { get; set; }
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
            Console.WriteLine(name.GetText());
            columnDefinition.Name = name.GetText().RemoveBackQuote();

            // check column definitions
            var declares = context.GetChild<ColumnDefinitionContext>(0);
            if (declares == null) return (false, null);

            // BITINT(20): DimensionDataTypeContext
            (columnDefinition.DataType, columnDefinition.DataLength) = declares.GetColumnDataDefinition();

            // NOTNULL: NullColumnConstraintContext
            var notnull = declares.GetChild<NullColumnConstraintContext>(0);
            columnDefinition.NotNull = notnull != null;

            // AUTOINCREMENT: AutoIncrementColumnConstraintContext
            var autoincrement = declares.GetChild<AutoIncrementColumnConstraintContext>(0);
            columnDefinition.AutoIncrement = autoincrement != null;

            // DEFAULt'0': DefaultColumnConstraintContext
            var defaultValue = declares.GetChild<DefaultColumnConstraintContext>(0);
            columnDefinition.HasDefault = defaultValue != null;
            columnDefinition.DefaultValue = defaultValue?.GetText();

            return (true, columnDefinition);
        }
    }
}
