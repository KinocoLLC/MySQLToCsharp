using System;
using System.Collections.Generic;
using System.Linq;

namespace MySQLToCsharp
{
    public class MySqlTypeMapper
    {
        private static readonly Type type = typeof(IMySqlType);
        private static readonly Type[] implementations;
        private static readonly IMySqlType[] validMySqlTypes = new IMySqlType[]
        {
            // NUMERIC
            // 1-64
            new NumericMySqlType("BIT", 64),
            // -128 - 127 || 0 - 255
            new NumericMySqlType("TINYINT", 4) { CanNegative = true},
            //  -32768 - 32767 || 0 - 65535
            new NumericMySqlType("SMALLINT", 6) { CanNegative = true},
            // -8388608 - 8388607 || 0 - 16777215
            new NumericMySqlType("MEDIUMINT", 9) { CanNegative = true},
            // -2147483648 - 2147483647 || 0 - 4294967295
            new NumericMySqlType("INT", 11) {  CanNegative = true, Aliases = new [] { "INTEGER" } },
            // -9223372036854775808 - 9223372036854775807 || 0 - 18446744073709551615
            new NumericMySqlType("BIGINT", 20) { CanNegative = true },
            // FLOAT: 0-24; DOUBLE: 25-53
            new NumericMySqlType("FLOAT", 53, 65) { CanNegative = true },
            new NumericMySqlType("DOUBLE", 65, 65) { CanNegative = true, Aliases = new [] { "REAL" } },
            // (M, D); M: 1- 65, D: 0-30
            new NumericMySqlType("DECIMAL", 65, 30) { CanNegative = true, Aliases = new [] { "DEC", "NUMERIC" } },

            // DATE and TIME
            new DateMySqlType("DATE"),
            new DateMySqlType("TIME", 6),
            new DateMySqlType("DATETIME", 6),
            new DateMySqlType("TIMESTAMP", 6),
            new DateMySqlType("YEAR"),
            
            // STRING
            new StringMySqlType("CHAR", 255),
            new StringMySqlType("VARCHAR", 65535),
            new StringMySqlType("BINARY", 255),
            new StringMySqlType("VARBINARY", 65535),
            new StringMySqlType("TINYBLOB"),
            new StringMySqlType("BLOB", 65535),
            new StringMySqlType("MEDIUMBLOB"),
            new StringMySqlType("LONGBLOB"),
            new StringMySqlType("TINYTEXT"),
            new StringMySqlType("TEXT", 65535),
            new StringMySqlType("MEDIUMTEXT"),
            new StringMySqlType("LONGTEXT"),

            // Fallback
            new FallbackMySqlType(""),
        };
        private static readonly MySqlTypeInfo[] typeInfos;

        static MySqlTypeMapper()
        {
            implementations = type.Assembly.GetTypes()
                .Where(x => type.IsAssignableFrom(x))
                .Where(x => x.IsClass)
                .Where(x => x.IsPublic)
                .ToArray();

            var aliases = validMySqlTypes.Where(x => x.Aliases.Any())
                .SelectMany(xs => xs.Aliases.Select(x => new MySqlTypeInfo(xs.GetType(), x, xs.TypeName)));
            typeInfos = validMySqlTypes
                .Select(x => new MySqlTypeInfo(x.GetType(), x.TypeName, null))
                .Concat(aliases)
                .ToArray();
        }

        public IMySqlType Map(string typeName)
        {
            var (typeStr, length, @decimal) = ParseTypeInfo(typeName);
            var mysqlType = implementations.FirstOrDefault(x => x.Name == Detect(typeStr));
            var instance = length.HasValue && @decimal.HasValue
                ? Activator.CreateInstance(mysqlType, new object[] { typeStr, length, @decimal })
                : length.HasValue
                    ? Activator.CreateInstance(mysqlType, new object[] { typeStr, length })
                    : Activator.CreateInstance(mysqlType, new object[] { typeStr });
            if (instance == null) throw new NullReferenceException($"{typeName} could not create instance of {mysqlType.Name} for parameters {string.Join(',', length, @decimal)}.");
            var result = (IMySqlType)instance;
            if (!IsValid(result))
            {
                throw new ArgumentOutOfRangeException($"{typeName} was not valid MySQL Type.");
            }
            return result;
        }

        private (string typeStr, ushort? length, byte? @decimal) ParseTypeInfo(string typeName)
        {
            if (!typeName.Contains('('))
            {
                // INT
                return (typeName, null, null);
            }

            // INT(10)
            if (!typeName.Contains(')'))
                throw new ArgumentOutOfRangeException($"{typeName} has '(' but missing ')'.");

            var begin = typeName.IndexOf('(');
            var end = typeName.IndexOf(')');
            var typeStr = typeName.Substring(0, begin);
            var lengthStr = typeName.Substring(begin + 1, end - begin - 1);

            ushort? length = null;
            byte? @decimal = null ;
            if (lengthStr.Contains(','))
            {
                // FLOAT(7,4)
                var lengthSplit = lengthStr.Split(',');
                if (lengthSplit.Length != 2)
                    throw new ArgumentOutOfRangeException($"{typeName} has , but {lengthStr} length {lengthSplit.Length} not match to 2.");
                length = ushort.Parse(lengthSplit[0]);
                @decimal = byte.Parse(lengthSplit[1]);
            }
            else
            {
                // INT(10)
                length = ushort.Parse(lengthStr);
            }

            return (typeStr, length, @decimal);
        }

        private string Detect(string name)
        {
            var known = typeInfos.Where(x => StringEquals(x.typeName, name));
            return known.Any() 
                ? known.First().type.Name 
                : nameof(FallbackMySqlType);
        }

        public bool IsValid(IMySqlType input)
        {
            var type = input.GetType();
            if (type == typeof(FallbackMySqlType)) return true;
            switch (input)
            {
                // INT
                case NumericMySqlType num when num.Length == null:
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, num.TypeName))
                        .Any();
                // INT(100)
                case NumericMySqlType num when num.Decimal == null:
                    // TODO: Decimal Check?
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, num.TypeName))
                        .Where(x => x.Length >= num.Length)
                        .Any();
                // DATETIME
                case DateMySqlType date when date.Length == null:
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, date.TypeName))
                        .Any();
                // DATETIME(6)
                case DateMySqlType date:
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, date.TypeName))
                        .Where(x => x.Length >= date.Length)
                        .Any();
                // VARCHAR
                case StringMySqlType str when str.Length == null:
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, str.TypeName))
                        .Any();
                // VARCHAR(255)
                case StringMySqlType str:
                    return validMySqlTypes
                        .Where(x => StringEquals(x.TypeName, str.TypeName))
                        .Where(x => x.Length >= str.Length)
                        .Any();
                default:
                    break;
            }

            return true;
        }

        private static bool StringEquals(string left, string right)
            => left.Equals(right, StringComparison.OrdinalIgnoreCase);

        private struct MySqlTypeInfo
        {
            public Type type;
            public string typeName;
            public string? aliasOf;

            public MySqlTypeInfo(Type type, string typeName, string? aliasOf)
            {
                this.type = type;
                this.typeName = typeName;
                this.aliasOf = aliasOf;
            }

            public override bool Equals(object? obj)
            {
                return obj is MySqlTypeInfo other &&
                       EqualityComparer<Type>.Default.Equals(type, other.type) &&
                       typeName == other.typeName &&
                       aliasOf == other.aliasOf;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(type, typeName, aliasOf);
            }
        }
    }
}
