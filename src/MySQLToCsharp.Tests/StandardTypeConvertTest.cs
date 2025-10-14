using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using MySQLToCsharp.TypeConverters;
using Xunit;

namespace MySQLToCsharp.Tests;

public class StandardTypeConvertTest
{
    const string converter = "StandardConverter";
    [Theory]
    [MemberData(nameof(GenerateParseTestData))]
    public void QueryParseAndTypeConvertTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        var typeConverter = TypeConverterResolver.Resolve(converter);
        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            var (clrType, _) = typeConverter.Convert(listener.TableDefinition.Columns[i].Data);
            Assert.Equal(data.Expected[i].clr, clrType);
        }

    }

    public class NonNullable
    {
        const bool nullable = false;
        // non nullable
        [Theory]
        [MemberData(nameof(TinyInt_TestData))]
        [MemberData(nameof(SmallInt_TestData))]
        [MemberData(nameof(Int_TestData))]
        [MemberData(nameof(BigInt_TestData))]
        [MemberData(nameof(Float_TestData))]
        [MemberData(nameof(Double_TestData))]
        [MemberData(nameof(Decimal_TestData))]
        [MemberData(nameof(TinyText_TestData))]
        [MemberData(nameof(Text_TestData))]
        [MemberData(nameof(MediumText_TestData))]
        [MemberData(nameof(LongText_TestData))]
        [MemberData(nameof(VarChar_TestData))]
        [MemberData(nameof(TinyBlob_TestData))]
        [MemberData(nameof(Blob_TestData))]
        [MemberData(nameof(MediumBlob_TestData))]
        [MemberData(nameof(LONGBlob_TestData))]
        [MemberData(nameof(Binary_TestData))]
        [MemberData(nameof(VarBinary_TestData))]
        [MemberData(nameof(DateTime_TestData))]
        [MemberData(nameof(TimeStamp_TestData))]
        public void ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            Assert.Equal(data.Expected, t);
        }
        [Theory]
        [MemberData(nameof(Bit_TestData))]
        public void BIT_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            Assert.Throws<NotSupportedException>(() => typeConverter.Convert(data.MySqlColumnData));

        }
        [Theory]
        [MemberData(nameof(Date_TestData))]
        [MemberData(nameof(Time_TestData))]
        [MemberData(nameof(Year_TestData))]
        [MemberData(nameof(Json_TestData))]
        public void Throw_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            Assert.Throws<NotSupportedException>(() => typeConverter.Convert(data.MySqlColumnData));
        }

        // bool/sbyte/byte
        public static IEnumerable<object[]> TinyInt_TestData()
        {
            const string DataType = "TINYINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // bool
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 1,
                        IsNullable = nullable,
                    },
                    Expected = "bool",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // bool
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 1,
                        IsNullable = nullable,
                    },
                    Expected = "bool",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "byte",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "sbyte",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "byte",
                },
            };
        }
        // short/ushort
        public static IEnumerable<object[]> SmallInt_TestData()
        {
            const string DataType = "SMALLINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 6,
                        IsNullable = nullable,
                    },
                    Expected = "ushort",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "short",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "short",
                },
            };
        }
        // int/uint
        public static IEnumerable<object[]> Int_TestData()
        {
            const string DataType = "INT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "uint",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 11,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 6,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int",
                },
            };
        }
        // long/ulong
        public static IEnumerable<object[]> BigInt_TestData()
        {
            const string DataType = "BIGINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "ulong",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 20,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 11,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long",
                },
            };
        }
        // flot
        public static IEnumerable<object[]> Float_TestData()
        {
            const string DataType = "FLOAT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "float",
                },
            };
        }
        // double
        public static IEnumerable<object[]> Double_TestData()
        {
            const string DataType = "DOUBLE";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "double",
                },
            };
        }
        // decimal
        public static IEnumerable<object[]> Decimal_TestData()
        {
            const string DataType = "DECIMAL";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "decimal",
                },
            };
        }
        // not support
        public static IEnumerable<object[]> Bit_TestData()
        {
            const string DataType = "BIT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "System.NotSupportedException",
                },
            };
        }            // string
        public static IEnumerable<object[]> TinyText_TestData()
        {
            const string DataType = "TINYTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> Text_TestData()
        {
            const string DataType = "TEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> MediumText_TestData()
        {
            const string DataType = "MEDIUMTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> LongText_TestData()
        {
            const string DataType = "LONGTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> VarChar_TestData()
        {
            const string DataType = "VARCHAR";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TinyBlob_TestData()
        {
            const string DataType = "TINYBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Blob_TestData()
        {
            const string DataType = "BLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> MediumBlob_TestData()
        {
            const string DataType = "MEDIUMBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> LONGBlob_TestData()
        {
            const string DataType = "LONGBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Binary_TestData()
        {
            const string DataType = "BINARY";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> VarBinary_TestData()
        {
            const string DataType = "VARBINARY";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // DateTime
        public static IEnumerable<object[]> DateTime_TestData()
        {
            const string DataType = "DATETIME";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "DateTime",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TimeStamp_TestData()
        {
            const string DataType = "TIMESTAMP";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // others
        public static IEnumerable<object[]> Date_TestData()
        {
            const string DataType = "DATE";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
        public static IEnumerable<object[]> Time_TestData()
        {
            const string DataType = "TIME";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
        public static IEnumerable<object[]> Year_TestData()
        {
            const string DataType = "YEAR";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
        public static IEnumerable<object[]> Json_TestData()
        {
            const string DataType = "JSON";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
    }

    public class Nullable
    {
        const bool nullable = true;

        // non nullable
        [Theory]
        [MemberData(nameof(TinyInt_TestData))]
        [MemberData(nameof(SmallInt_TestData))]
        [MemberData(nameof(Int_TestData))]
        [MemberData(nameof(BigInt_TestData))]
        [MemberData(nameof(Float_TestData))]
        [MemberData(nameof(Double_TestData))]
        [MemberData(nameof(Decimal_TestData))]
        [MemberData(nameof(TinyText_TestData))]
        [MemberData(nameof(Text_TestData))]
        [MemberData(nameof(MediumText_TestData))]
        [MemberData(nameof(LongText_TestData))]
        [MemberData(nameof(VarChar_TestData))]
        [MemberData(nameof(TinyBlob_TestData))]
        [MemberData(nameof(Blob_TestData))]
        [MemberData(nameof(MediumBlob_TestData))]
        [MemberData(nameof(LONGBlob_TestData))]
        [MemberData(nameof(Binary_TestData))]
        [MemberData(nameof(VarBinary_TestData))]
        [MemberData(nameof(DateTime_TestData))]
        [MemberData(nameof(TimeStamp_TestData))]
        public void ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            Assert.Equal(data.Expected, t);
        }
        [Theory]
        [MemberData(nameof(Bit_TestData))]
        public void BIT_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            Assert.Throws<NotSupportedException>(() => typeConverter.Convert(data.MySqlColumnData));
        }
        [Theory]
        [MemberData(nameof(Date_TestData))]
        [MemberData(nameof(Time_TestData))]
        [MemberData(nameof(Year_TestData))]
        [MemberData(nameof(Json_TestData))]
        public void Throw_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(converter);
            Assert.Throws<NotSupportedException>(() => typeConverter.Convert(data.MySqlColumnData));
        }

        // sbyte/byte
        public static IEnumerable<object[]> TinyInt_TestData()
        {
            const string DataType = "TINYINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // bool
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 1,
                        IsNullable = nullable,
                    },
                    Expected = "bool?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // bool
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 1,
                        IsNullable = nullable,
                    },
                    Expected = "bool?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "byte?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "sbyte?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "byte?",
                },
            };
        }
        // short/ushort
        public static IEnumerable<object[]> SmallInt_TestData()
        {
            const string DataType = "SMALLINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        Length = 6,
                        IsNullable = nullable,
                    },
                    Expected = "ushort?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        Length = 4,
                        IsNullable = nullable,
                    },
                    Expected = "short?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "short?",
                },
            };
        }
        // int/uint
        public static IEnumerable<object[]> Int_TestData()
        {
            const string DataType = "INT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "uint?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 11,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 6,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "int?",
                },
            };
        }
        // long/ulong
        public static IEnumerable<object[]> BigInt_TestData()
        {
            const string DataType = "BIGINT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "ulong?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 20,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long?",
                },
            };
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        Length = 11,
                        IsUnsigned = false,
                        IsNullable = nullable,
                    },
                    Expected = "long?",
                },
            };
        }
        // flot
        public static IEnumerable<object[]> Float_TestData()
        {
            const string DataType = "FLOAT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "float?",
                },
            };
        }
        // double
        public static IEnumerable<object[]> Double_TestData()
        {
            const string DataType = "DOUBLE";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "double?",
                },
            };
        }
        // decimal
        public static IEnumerable<object[]> Decimal_TestData()
        {
            const string DataType = "DECIMAL";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "decimal?",
                },
            };
        }
        // bool
        public static IEnumerable<object[]> Bit_TestData()
        {
            const string DataType = "BIT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "System.NotSupportedException",
                },
            };
        }
        // string
        public static IEnumerable<object[]> TinyText_TestData()
        {
            const string DataType = "TINYTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> Text_TestData()
        {
            const string DataType = "TEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> MediumText_TestData()
        {
            const string DataType = "MEDIUMTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> LongText_TestData()
        {
            const string DataType = "LONGTEXT";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> VarChar_TestData()
        {
            const string DataType = "VARCHAR";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "string",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TinyBlob_TestData()
        {
            const string DataType = "TINYBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Blob_TestData()
        {
            const string DataType = "BLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> MediumBlob_TestData()
        {
            const string DataType = "MEDIUMBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> LONGBlob_TestData()
        {
            const string DataType = "LONGBLOB";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Binary_TestData()
        {
            const string DataType = "BINARY";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> VarBinary_TestData()
        {
            const string DataType = "VARBINARY";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // DateTime
        public static IEnumerable<object[]> DateTime_TestData()
        {
            const string DataType = "DATETIME";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "DateTime?",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TimeStamp_TestData()
        {
            const string DataType = "TIMESTAMP";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // others
        public static IEnumerable<object[]> Date_TestData()
        {
            const string DataType = "DATE";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
        public static IEnumerable<object[]> Time_TestData()
        {
            const string DataType = "TIME";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
        public static IEnumerable<object[]> Year_TestData()
        {
            const string DataType = "YEAR";
            yield return new object[]
            {
            new ColumnDataTestItem
            {
                // sbyte
                MySqlColumnData =new MySqlColumnDataDefinition
                {
                    DataType = DataType,
                    IsUnsigned = true,
                    IsNullable = nullable,
                },
                Expected = "throw",
            },
            };
        }
        public static IEnumerable<object[]> Json_TestData()
        {
            const string DataType = "JSON";
            yield return new object[]
            {
                new ColumnDataTestItem
                {
                    // sbyte
                    MySqlColumnData =new MySqlColumnDataDefinition
                    {
                        DataType = DataType,
                        IsUnsigned = true,
                        IsNullable = nullable,
                    },
                    Expected = "throw",
                },
            };
        }
    }

    public static IEnumerable<object[]> GenerateParseTestData()
    {
        var statements = TestHelper.LoadSql("test_data/create_table.sql");
        foreach (var statement in statements)
        {
            yield return new object[]
            {
                new TestItem
                {
                    Statement = statement,
                    Expected = new []
                    {
                        ("BIGINT", "long"),
                        ("INT", "int"),
                        ("INT", "int"),
                        ("INT", "int"),
                        ("TINYINT", "byte"),
                        ("DATETIME", "DateTime"),
                    }
                },
            };
        }
    }

    public class TestItem
    {
        public required string Statement { get; set; }
        public required (string mysql, string clr)[] Expected { get; set; }
    }

    public class ColumnDataTestItem
    {
        public required MySqlColumnDataDefinition MySqlColumnData { get; set; }
        public required string Expected { get; set; }
    }
}
