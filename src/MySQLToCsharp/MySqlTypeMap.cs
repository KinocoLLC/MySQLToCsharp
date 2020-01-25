using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySQLToCsharp
{
    public class MySqlTypeMap
    {
        private static readonly string[] validMySqlTypes = new []
        {
            // https://dev.mysql.com/doc/refman/8.0/en/numeric-type-syntax.html
            "BIT",
            "BOOL",
            "TINYINT",
            "SMALLINT",
            "MEDIUMINT",
            "INT",
            "BIGINT",
            "DECIMAL",
            "FLOAT",
            "DOUBLE",
            // https://dev.mysql.com/doc/refman/8.0/en/date-and-time-types.html
            "DATE",
            "TIME",
            "DATETIME",
            "TIMESTAMP",
            "YEAR",
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
        public bool IsNullable { get; set; }
        public string MySqlType { get; set; }
        public int MySqlTypeLength { get; set; }
        public string CsharpType { get; set; }

        public bool IsValid()
        {
            //var isValidMysqlType = validMySqlTypes.Contains(MySqlType, StringComparer.OrdinalIgnoreCase);
            return true;
        }
    }
}
