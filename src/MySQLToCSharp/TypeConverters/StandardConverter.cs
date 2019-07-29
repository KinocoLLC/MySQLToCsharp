using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp.TypeConverters
{
    /// <summary>
    /// Convert MySQL Type to CLR Type.
    /// * Follow to the Fized MySQL Type.
    /// EF to MySQL Type | CLR            | Fixed MySQL Type    | nochange | Comment
    /// -----            | ----           | ----                | ----     | ----
    /// SMALLINT(6)      | sbyte          | TINYINT(4)          | X        |
    /// TINYINT(4)       | byte           | TINYINT(4) UNSIGNED | X        |
    /// SMALLINT(6)      | short          | SMALLINT(6)         | O        |
    /// INT(11)          | ushort         | SMALLINT(6) UNSIGNED| X        |
    /// INT(11)          | int            | INT(11)             | O        |
    /// BIGINT(20)       | uint           | INT(11) UNSIGNED    | X        |
    /// BIGINT(20)       | long           | BIGINT(20)          | O        |
    /// BIGINT(20)       | Ulong          | BIGINT(20) UNSIGNED | X        | 
    /// FLOAT            | float          | FLOAT               | O        |
    /// DOUBLE           | double         | DOUBLE              | O        |
    /// DECIMAL(18,2)    | decimal        | DECIMAL(18,2)       | O        | NUMERIC is DECIMAL base
    /// TINYINT(1)       | bool           | BIT                 | X        | [Column(TypeName = "BIT(1)")]
    /// INT(11)          | char           | <throw>             | X        | not handle on DB
    /// TEXT             | string         | VARCHAR(N)          | X        | [Column[TypeName = "VARCHAR(255)"]
    /// TINYTEXT         | string         | TINYTEXT            | O        | 2^8 should not use TEXT, try use VARCHAR
    /// TEXT             | string         | TEXT                | O        | 2^8 should not use TEXT, try use VARCHAR
    /// MEDIUMTEXT       | string         | MEDIUMTEXT          | O        | 2^24 should not use TEXT, try use VARCHAR
    /// LONGTEXT         | string         | LONGTEXT            | O        | 2^32 should not use TEXT, try use VARCHAR
    /// VARBINARY(3000)  | byte[]         | VARBINARY(3000)     | O        |
    /// VARBINARY(65535) | byte[]         | TINYBLOB            | O        | 2^8
    /// VARBINARY(65535) | byte[]         | BLOB                | O        | 2^16
    /// VARBINARY(65535) | byte[]         | MEDIUMBLOB          | O        | 2^24
    /// VARBINARY(65535) | byte[]         | LONGBLOB            | O        | 2^32
    /// DATETIME         | DateTime       | n/a                 | X        | should not use TIMESTAMP. try use DATETIME + TIMEZONE + OFFSET column
    /// DATETIME         | DateTimeOffset | DATETIME            | X        | Converter on C#
    /// TIMESTAMP        | DATETIME       | n/a                 | O        | DB TIMESTAMP should not use for DATETIME.
    /// TIMESTAMP        | byte[]         | TIMESTAMP           | X        | Converter on C# (Equivalant to MSSQL RowVersion)
    /// </summary>
    /// <remarks>
    /// mysql type length:
    /// number: https://dev.mysql.com/doc/refman/5.6/ja/integer-types.html
    /// numeric: https://dev.mysql.com/doc/refman/5.6/ja/fixed-point-types.html
    /// float: https://dev.mysql.com/doc/refman/5.6/ja/floating-point-types.html
    /// </remarks>
    public class StandardConverter : ITypeConverter
    {
        private static readonly string[] none = Array.Empty<string>();

        // TODO: ROWVERSION handling is missing
        // TOOD: Required handling is not correct
        private const string Required = "Required";
        private const string Timestamp = "Timestamp";
        private static string StringLength(MySqlColumnDataDefinition data)
            => data.Length.HasValue && data.Length != 0
                ? $"StringLength({data.Length})"
                : null;
        private static string ArrayLength(MySqlColumnDataDefinition data)
            => data.Length.HasValue && data.Length != 0
                ? $"MaxLength({data.Length})"
                : null;

        public (string typeName, string[] attributes) Convert(MySqlColumnDataDefinition data)
        {
            if (data.IsNullable)
            {
                return NullableConverter(data);
            }
            else
            {
                return NonNullableConverter(data);
            }
        }

        private (string typeName, string[] attributes) NonNullableConverter(MySqlColumnDataDefinition data)
        {
            switch (data.DataType)
            {
                // sbyte/byte
                case "TINYINT" when data.IsUnsigned && data.Length == 4: return ("byte", none);
                case "TINYINT" when data.Length == 4: return ("sbyte", none);
                case "TINYINT": return ("byte", none);
                // short/ushort
                case "SMALLINT" when data.IsUnsigned && data.Length == 6: return ("ushort", none);
                case "SMALLINT" when data.Length == 6: return ("short", none);
                case "SMALLINT": return ("short", none);
                // int/uint
                case "INT" when data.IsUnsigned: return ("uint", none);
                case "INT": return ("int", none);
                // long/ulong
                case "BIGINT" when data.IsUnsigned: return ("ulong", none);
                case "BIGINT": return ("long", none);
                // float/double
                case "FLOAT": return ("float", none);
                case "DOUBLE": return ("double", none);
                // decimal
                case "DECIMAL": return ("decimal", none);
                // bool
                case "BIT": return ("bool", none);
                // clr char: no hanlding
                // string
                case "TINYTEXT": // fallthrough
                case "TEXT": // fallthrough
                case "MEDIUMTEXT": // fallthrough
                case "LONGTEXT": // fallthrough
                case "VARCHAR": return ("string", StringLength(data) is string sl ? new[] { Required, sl } : new[] { Required });
                // byte[]
                case "TINYBLOB": // fallthrough
                case "BLOB": // fallthrough
                case "MEDIUMBLOB": // fallthrough
                case "LONGBLOB": // fallthrough
                case "BINARY": // fallthrough
                case "VARBINARY": return ("byte[]", ArrayLength(data) is string al ? new[] { Required, al } : new[] { Required });
                // DateTimeOffset
                case "DATETIME": return ("DateTimeOffset", none);
                // byte[]
                // mysql TIMESTAMP should handle as RowVersion in MSSQL, means CLR byte[] and [Timestamp] attribute
                case "TIMESTAMP": return ("byte[]", new[] { Timestamp });
                default: throw new NotSupportedException(data.DataType);
            }
        }

        private (string typeName, string[] attributes) NullableConverter(MySqlColumnDataDefinition data)
        {
            switch (data.DataType)
            {
                // sbyte/byte
                case "TINYINT" when data.IsUnsigned && data.Length == 4: return ("byte?", none);
                case "TINYINT" when data.Length == 4: return ("sbyte?", none);
                case "TINYINT": return ("sbyte?", none);
                // short/ushort
                case "SMALLINT" when data.IsUnsigned && data.Length == 6: return ("ushort?", none);
                case "SMALLINT" when data.Length == 6: return ("short?", none);
                case "SMALLINT": return ("short?", none);
                // int/uint
                case "INT" when data.IsUnsigned: return ("uint?", none);
                case "INT": return ("int?", none);
                // long/ulong
                case "BIGINT" when data.IsUnsigned: return ("ulong?", none);
                case "BIGINT": return ("long?", none);
                // float/double
                case "FLOAT": return ("float?", none);
                case "DOUBLE": return ("double?", none);
                // decimal
                case "DECIMAL": return ("decimal?", none);
                // bool
                case "BIT": return ("bool?", none);
                // clr char: no hanlding
                // string
                case "TINYTEXT": // fallthrough
                case "TEXT": // fallthrough
                case "MEDIUMTEXT": // fallthrough
                case "LONGTEXT": // fallthrough
                case "VARCHAR": return ("string", StringLength(data) is string sl ? new[] { sl } : none);
                // byte[]
                case "TINYBLOB": // fallthrough
                case "BLOB": // fallthrough
                case "MEDIUMBLOB": // fallthrough
                case "LONGBLOB": // fallthrough
                case "BINARY": // fallthrough
                case "VARBINARY": return ("byte[]", ArrayLength(data) is string al ? new[] { al } : none);
                // DateTimeOffset
                case "DATETIME": return ("DateTimeOffset?", none);
                // byte[]
                // mysql TIMESTAMP should handle as RowVersion in MSSQL, means CLR byte[] and [Timestamp] attribute
                case "TIMESTAMP": return ("byte[]", new[] { Timestamp });
                default: throw new NotSupportedException(data.DataType);
            }
        }
    }
}
