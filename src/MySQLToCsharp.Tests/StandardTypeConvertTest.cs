using FluentAssertions;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.TypeConverters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class StandardTypeConvertTest
    {
        [Theory]
        [MemberData(nameof(GenerateParseTestData))]
        public void QueryParseAndTypeConvertTest(TestItem data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            listener.IsTargetStatement.Should().BeTrue();
            listener.IsParseBegin.Should().BeTrue();
            listener.IsParseCompleted.Should().BeTrue();
            listener.TableDefinition.Should().NotBeNull();

            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
            {
                var (clrType, _) = typeConverter.Convert(listener.TableDefinition.Columns[i].Data);
                clrType.Should().Be(data.Expected[i].clr);
            }

        }

        [Theory]
        [MemberData(nameof(TinyInt_NonNullableTestData))]
        public void TINYINT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(SmallInt_NonNullableTestData))]
        public void SMALLINT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Int_NonNullableTestData))]
        public void INT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(BigInt_NonNullableTestData))]
        public void BIGINT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Float_NonNullableTestData))]
        public void FLOAT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Double_NonNullableTestData))]
        public void DOUBLE_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Decimal_NonNullableTestData))]
        public void DECIMAL_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Bit_NonNullableTestData))]
        public void BIT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(TinyText_NonNullableTestData))]
        public void TINYTEXT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Text_NonNullableTestData))]
        public void TEXT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(MediumText_NonNullableTestData))]
        public void MEDIUMTEXT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(LongText_NonNullableTestData))]
        public void LONGTEXT_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(VarChar_NonNullableTestData))]
        public void VARCHAR_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(TinyBlob_NonNullableTestData))]
        public void TINYBLOB_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Blob_NonNullableTestData))]
        public void BLOB_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(MediumBlob_NonNullableTestData))]
        public void MEDIUMBLOB_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(LONGBlob_NonNullableTestData))]
        public void LONGBLOB_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(Binary_NonNullableTestData))]
        public void BINARY_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(VarBinary_NonNullableTestData))]
        public void VARBINARY_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(DateTime_NonNullableTestData))]
        public void DATETIME_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
        }
        [Theory]
        [MemberData(nameof(TimeStamp_NonNullableTestData))]
        public void TIMESTAMP_NonNullable_ConvertTest(ColumnDataTestItem data)
        {
            var typeConverter = TypeConverterResolver.Resolve(typeof(StandardConverter));
            var (t, _) = typeConverter.Convert(data.MySqlColumnData);
            t.Should().Be(data.Expected);
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
                            ("DATETIME", "DateTimeOffset"),
                        }
                    },
                };
            }
        }

        // sbyte/byte
        public static IEnumerable<object[]> TinyInt_NonNullableTestData()
        {
            const string DataType = "TINYINT";
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
                    },
                    Expected = "byte",
                },
            };
        }
        // short/ushort
        public static IEnumerable<object[]> SmallInt_NonNullableTestData()
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
                    },
                    Expected = "short",
                },
            };
        }
        // int/uint
        public static IEnumerable<object[]> Int_NonNullableTestData()
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
                    },
                    Expected = "int",
                },
            };
        }
        // long/ulong
        public static IEnumerable<object[]> BigInt_NonNullableTestData()
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
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
                        IsNullable = false,
                    },
                    Expected = "long",
                },
            };
        }
        // flot
        public static IEnumerable<object[]> Float_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "float",
                },
            };
        }
        // double
        public static IEnumerable<object[]> Double_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "double",
                },
            };
        }
        // decimal
        public static IEnumerable<object[]> Decimal_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "decimal",
                },
            };
        }
        // bool
        public static IEnumerable<object[]> Bit_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "bool",
                },
            };
        }
        // string
        public static IEnumerable<object[]> TinyText_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> Text_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> MediumText_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> LongText_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "string",
                },
            };
        }
        public static IEnumerable<object[]> VarChar_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "string",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TinyBlob_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Blob_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> MediumBlob_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> LONGBlob_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> Binary_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        public static IEnumerable<object[]> VarBinary_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // DateTimeOffset
        public static IEnumerable<object[]> DateTime_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "DateTimeOffset",
                },
            };
        }
        // byte[]
        public static IEnumerable<object[]> TimeStamp_NonNullableTestData()
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
                        IsNullable = false,
                    },
                    Expected = "byte[]",
                },
            };
        }
        // others


        public class TestItem
        {
            public string Statement { get; set; }
            public (string mysql, string clr)[] Expected { get; set; }
        }

        public class ColumnDataTestItem
        {
            public MySqlColumnDataDefinition MySqlColumnData { get; set; }
            public string Expected { get; set; }
        }
    }
}
