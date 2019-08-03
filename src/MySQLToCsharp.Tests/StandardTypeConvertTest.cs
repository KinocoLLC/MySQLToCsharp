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
        const string converter = "StandardConverter";
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

            var typeConverter = TypeConverterResolver.Resolve(converter);
            for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
            {
                var (clrType, _) = typeConverter.Convert(listener.TableDefinition.Columns[i].Data);
                clrType.Should().Be(data.Expected[i].clr);
            }

        }

        public class NonNullable
        {
            const bool nullable = false;
            // non nullable
            [Theory]
            [MemberData(nameof(TinyInt_TestData))]
            public void TINYINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(SmallInt_TestData))]
            public void SMALLINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Int_TestData))]
            public void INT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(BigInt_TestData))]
            public void BIGINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Float_TestData))]
            public void FLOAT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Double_TestData))]
            public void DOUBLE_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Decimal_TestData))]
            public void DECIMAL_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Bit_TestData))]
            public void BIT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TinyText_TestData))]
            public void TINYTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Text_TestData))]
            public void TEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(MediumText_TestData))]
            public void MEDIUMTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(LongText_TestData))]
            public void LONGTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(VarChar_TestData))]
            public void VARCHAR_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TinyBlob_TestData))]
            public void TINYBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Blob_TestData))]
            public void BLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(MediumBlob_TestData))]
            public void MEDIUMBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(LONGBlob_TestData))]
            public void LONGBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Binary_TestData))]
            public void BINARY_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(VarBinary_TestData))]
            public void VARBINARY_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(DateTime_TestData))]
            public void DATETIME_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TimeStamp_TestData))]
            public void TIMESTAMP_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
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
                    Expected = "bool",
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
            // DateTimeOffset
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
                    Expected = "DateTimeOffset",
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
            public void TINYINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(SmallInt_TestData))]
            public void SMALLINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Int_TestData))]
            public void INT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(BigInt_TestData))]
            public void BIGINT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Float_TestData))]
            public void FLOAT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Double_TestData))]
            public void DOUBLE_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Decimal_TestData))]
            public void DECIMAL_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Bit_TestData))]
            public void BIT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TinyText_TestData))]
            public void TINYTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Text_TestData))]
            public void TEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(MediumText_TestData))]
            public void MEDIUMTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(LongText_TestData))]
            public void LONGTEXT_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(VarChar_TestData))]
            public void VARCHAR_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TinyBlob_TestData))]
            public void TINYBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Blob_TestData))]
            public void BLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(MediumBlob_TestData))]
            public void MEDIUMBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(LONGBlob_TestData))]
            public void LONGBLOB_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(Binary_TestData))]
            public void BINARY_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(VarBinary_TestData))]
            public void VARBINARY_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(DateTime_TestData))]
            public void DATETIME_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
            }
            [Theory]
            [MemberData(nameof(TimeStamp_TestData))]
            public void TIMESTAMP_ConvertTest(ColumnDataTestItem data)
            {
                var typeConverter = TypeConverterResolver.Resolve(converter);
                var (t, _) = typeConverter.Convert(data.MySqlColumnData);
                t.Should().Be(data.Expected);
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
                    Expected = "bool?",
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
            // DateTimeOffset
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
                    Expected = "DateTimeOffset?",
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
                            ("DATETIME", "DateTimeOffset"),
                        }
                    },
                };
            }
        }

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
