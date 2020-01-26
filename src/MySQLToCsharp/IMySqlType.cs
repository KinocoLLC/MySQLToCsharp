using System;
using System.Linq;

namespace MySQLToCsharp
{
    public interface IMySqlType
    {
        string TypeName { get; }
        public ushort? Length { get; }
        string[]? Aliases { get; set; }
    }

    /// <summary>
    /// https://dev.mysql.com/doc/refman/5.6/ja/numeric-type-overview.html
    /// https://dev.mysql.com/doc/refman/8.0/en/numeric-type-syntax.html
    /// </summary>
    public class NumericMySqlType : IMySqlType
    {
        private static readonly string[] _typeNames = new[] { "BIT", "TINYINT", "SMALLINT", "MEDIUMINT", "INT", "BIGINT", "FLOAT", "DOUBLE", "DECIMAL" };
        public string TypeName { get; protected set; }
        public ushort? Length { get; protected set; }
        public byte? Decimal { get; set; }
        public string[]? Aliases { get; set; }

        public bool CanNegative { get; set; }

        public NumericMySqlType(string typeName)
        {
            TypeName = typeName;
        }
        public NumericMySqlType(string typeName, ushort? length) : this(typeName)
        {
            Length = length;
        }

        public NumericMySqlType(string typeName, ushort? length, byte? @decimal) : this(typeName, length)
        {
            Decimal = @decimal;
        }

        public static bool IsType(string typeName) => _typeNames.Contains(typeName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// https://dev.mysql.com/doc/refman/8.0/en/date-and-time-types.html
    /// </summary>
    public class DateMySqlType : IMySqlType
    {
        private static readonly string[] _typeNames = new[] { "DATE", "TIME", "DATETIME", "TIMESTAMP", "YEAR" };
        public string TypeName { get; protected set; }
        public ushort? Length { get; }
        public string[]? Aliases { get; set; }

        public DateMySqlType(string typeName)
        {
            TypeName = typeName;
        }
        public DateMySqlType(string typeName, ushort? length) : this(typeName)
        {
            Length = length;
        }

        public static bool IsType(string typeName) => _typeNames.Contains(typeName, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// https://dev.mysql.com/doc/refman/8.0/en/string-types.html
    /// https://dev.mysql.com/doc/refman/8.0/en/char.html
    /// https://dev.mysql.com/doc/refman/8.0/en/binary-varbinary.html
    /// https://dev.mysql.com/doc/refman/8.0/en/blob.html
    /// </summary>
    public class StringMySqlType : IMySqlType
    {
        private static readonly string[] _typeNames = new[] 
        { 
            "CHAR", "VARCHAR", 
            "BINARY", "VARBINARY", 
            "TINYBLOB", "MEDIUMBLOB", "BLOB", "LONGBLOB", 
            "TINYTEXT", "MEDIUMTEXT", "TEXT", "LONGTEXT",
        };
        public string TypeName { get; protected set; }
        public ushort? Length { get; set; }
        public string[]? Aliases { get; set; }

        public StringMySqlType(string typeName)
        {
            TypeName = typeName;
        }
        public StringMySqlType(string typeName, ushort? length) : this(typeName)
        {
            Length = length;
        }

        public static bool IsType(string typeName) => _typeNames.Contains(typeName, StringComparer.OrdinalIgnoreCase);
    }

    public class FollbackMySqlType : IMySqlType
    {
        public string TypeName { get; protected set; }
        public ushort? Length { get; set; }
        public string[]? Aliases { get; set; }

        public FollbackMySqlType(string typeName)
        {
            TypeName = typeName;
        }

        public static bool IsType(string typeName) => true;
    }
}
