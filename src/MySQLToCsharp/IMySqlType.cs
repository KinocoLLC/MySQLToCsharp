using System;
using System.Linq;

namespace MySQLToCsharp
{
    public interface IMySqlType
    {
        string TypeName { get; }
        public ushort? Length { get; }
        string[] Aliases { get; set; }
    }

    /// <summary>
    /// MySql Numeric Data Type
    /// https://dev.mysql.com/doc/refman/5.6/ja/numeric-type-overview.html
    /// https://dev.mysql.com/doc/refman/8.0/en/numeric-type-syntax.html
    /// </summary>
    public class NumericMySqlType : IMySqlType
    {
        public string TypeName { get; }
        public ushort? Length { get; }
        public byte? Decimal { get; }
        public string[] Aliases { get; set; } = Array.Empty<string>();

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
    }

    /// <summary>
    /// MySql Date Data Type
    /// https://dev.mysql.com/doc/refman/8.0/en/date-and-time-types.html
    /// </summary>
    public class DateMySqlType : IMySqlType
    {
        public string TypeName { get; }
        public ushort? Length { get; }
        public string[] Aliases { get; set; } = Array.Empty<string>();

        public DateMySqlType(string typeName)
        {
            TypeName = typeName;
        }
        public DateMySqlType(string typeName, ushort? length) : this(typeName)
        {
            Length = length;
        }
    }

    /// <summary>
    /// MySql String Data Type
    /// https://dev.mysql.com/doc/refman/8.0/en/string-types.html
    /// https://dev.mysql.com/doc/refman/8.0/en/char.html
    /// https://dev.mysql.com/doc/refman/8.0/en/binary-varbinary.html
    /// https://dev.mysql.com/doc/refman/8.0/en/blob.html
    /// </summary>
    public class StringMySqlType : IMySqlType
    {
        public string TypeName { get; }
        public ushort? Length { get; }
        public string[] Aliases { get; set; } = Array.Empty<string>();

        public StringMySqlType(string typeName)
        {
            TypeName = typeName;
        }
        public StringMySqlType(string typeName, ushort? length) : this(typeName)
        {
            Length = length;
        }
    }

    /// <summary>
    /// Fallback MySql Data Type
    /// </summary>
    public class FallbackMySqlType : IMySqlType
    {
        public string TypeName { get; }
        public ushort? Length { get; }
        public string[] Aliases { get; set; } = Array.Empty<string>();

        public FallbackMySqlType(string typeName)
        {
            TypeName = typeName;
        }
    }
}
