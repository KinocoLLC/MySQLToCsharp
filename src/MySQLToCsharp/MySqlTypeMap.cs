using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLToCsharp
{
    public class MySqlTypeMap
    {
        private static readonly MySqlTypeBase[] validMySqlTypes = new MySqlTypeBase[]
        {
            // NUMERIC
            // https://dev.mysql.com/doc/refman/5.6/ja/numeric-type-overview.html
            // https://dev.mysql.com/doc/refman/8.0/en/numeric-type-syntax.html
            // 1-64
            new NumericMySqlType("BIT", 64, "1"),
            // 0: false || none-0: true
            new NumericMySqlType("BOOL"){ Default = "0", Aliases = new [] { "BOOLEAN" } },
            // -128 - 127 || 0 - 255
            new NumericMySqlType("TINYINT", 4, "0") { CanNegative = true},
            //  -32768 - 32767 || 0 - 65535
            new NumericMySqlType("SMALLINT", 6, "0") { CanNegative = true},
            // -8388608 - 8388607 || 0 - 16777215
            new NumericMySqlType("MEDIUMINT", 9, "0") { CanNegative = true},
            // -2147483648 - 2147483647 || 0 - 4294967295
            new NumericMySqlType("INT", 11, "0") { CanNegative = true, Aliases = new [] { "INTEGER" } },
            // -9223372036854775808 - 9223372036854775807 || 0 - 18446744073709551615
            new NumericMySqlType("BIGINT", 20, "0") { CanNegative = true },
            // (M, D); M: 1- 65, D: 0-30
            new NumericMySqlType("DECIMAL", 65, "1") { CanNegative = true, Aliases = new [] { "DEC" } },
            // FLOAT: 0-24; DOUBLE: 25-53
            new NumericMySqlType("FLOAT", 53, "1") { CanNegative = true },
            new NumericMySqlType("DOUBLE", 65, "1") { CanNegative = true, Aliases = new [] { "REAL" } },

            // DATE and TIME
            // https://dev.mysql.com/doc/refman/8.0/en/date-and-time-types.html
            "DATE",
            "TIME",
            "DATETIME",
            "TIMESTAMP",
            "YEAR",
            
            // STRING
            // https://dev.mysql.com/doc/refman/8.0/en/string-types.html
            "CHAR",
            "VARCHAR",
            "BINARY",
            "VARBINARY",
            "BLOB",
            "TINYTEXT",
            "TEXT",
            "MEDIUMTEXT",
            "LONGTEXT",
        };
        public string MySqlType { get; set; }
        public string CsharpType { get; set; }
        public string? MySqlTypeLength { get; set; }
        public bool IsNullable { get; set; }

        public MySqlTypeMap(string mySqlType, string csharpType)
        {
            MySqlType = mySqlType;
            CsharpType = csharpType;
        }

        public bool IsValid()
        {
            //var isValidMysqlType = validMySqlTypes.Contains(MySqlType, StringComparer.OrdinalIgnoreCase);
            //return isValidMysqlType;
            return true;
        }

        private class MySqlTypeBase
        {
            public string TypeName { get; set; } = "";
            public string? Default { get; set; }
            public string[] Aliases { get; set; } = Array.Empty<string>();
        }

        private class NumericMySqlType : MySqlTypeBase
        {
            public byte Digit { get; set; }
            public bool CanNegative { get; set; }

            public NumericMySqlType(string typeName)
            {
                TypeName = typeName;
            }
            public NumericMySqlType(string typeName, byte digit, string? @default)
            {
                TypeName = typeName;
                Digit = digit;
                Default = @default;
            }
        }
    }
}
