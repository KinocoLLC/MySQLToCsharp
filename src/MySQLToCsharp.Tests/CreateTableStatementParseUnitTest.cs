using FluentAssertions;
using MySQLToCsharp.Listeners;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class CreateTableStatementParseUnitTest
    {
        [Theory]
        [MemberData(nameof(GenerateParseTestData))]
        public void ParseTest(TestData data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            listener.IsTargetStatement.Should().BeTrue();
            listener.IsParseBegin.Should().BeTrue();
            listener.IsParseCompleted.Should().BeTrue();
            listener.TableDefinition.Should().NotBeNull();
        }
        [Theory]
        [MemberData(nameof(GenerateTypeConverterTestData))]
        public void TypeConverterTest(TestData data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            listener.IsTargetStatement.Should().BeTrue();
            listener.IsParseBegin.Should().BeTrue();
            listener.IsParseCompleted.Should().BeTrue();
            listener.TableDefinition.Should().NotBeNull();

            for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
            {
                listener.TableDefinition.Columns[i].Name.Should().Be(data.Expected[i].Name);
                listener.TableDefinition.Columns[i].Order.Should().Be(data.Expected[i].Order);
                listener.TableDefinition.Columns[i].AutoIncrement.Should().Be(data.Expected[i].AutoIncrement);
                listener.TableDefinition.Columns[i].Data.IsNullable.Should().Be(data.Expected[i].Data.IsNullable);
                listener.TableDefinition.Columns[i].Data.IsUnsigned.Should().Be(data.Expected[i].Data.IsUnsigned);
                listener.TableDefinition.Columns[i].Data.Length.Should().Be(data.Expected[i].Data.Length);
                listener.TableDefinition.Columns[i].Data.DataType.Should().Be(data.Expected[i].Data.DataType);
            }
        }

        public static IEnumerable<object[]> GenerateParseTestData()
        {
            var statements = LoadSql("test_data/create_tables.sql");
            foreach (var statement in statements)
            {
                yield return new object[]
                {
                    new TestData
                    {
                        Statement = statement,
                    },
                };
            }
        }

        public static IEnumerable<object[]> GenerateTypeConverterTestData()
        {
            var statements = LoadSql("test_data/Strings.sql");
            foreach (var statement in statements)
            {
                yield return new object[]
                {
                    new TestData
                    {
                        Statement = statement,
                        Expected = new [] {
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "INT",
                                    IsNullable = false,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "Id",
                                Order = 0,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "VARCHAR",
                                    IsNullable = false,
                                    IsUnsigned = false,
                                    Length = 50,
                                },
                                Name = "S",
                                Order = 1,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "VARCHAR",
                                    IsNullable = true,
                                    IsUnsigned = false,
                                    Length = 50,
                                },
                                Name = "NS",
                                Order = 2,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "TEXT",
                                    IsNullable = false,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "T",
                                Order = 3,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "TEXT",
                                    IsNullable = true,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "NT",
                                Order = 4,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "MEDIUMTEXT",
                                    IsNullable = false,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "MT",
                                Order = 5,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "MEDIUMTEXT",
                                    IsNullable = true,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "NMT",
                                Order = 6,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "LONGTEXT",
                                    IsNullable = false,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "LT",
                                Order = 7,
                            },
                            new MySqlColumnDefinition
                            {
                                AutoIncrement = false,
                                Data = new MySqlColumnDataDefinition
                                {
                                    DataType = "LONGTEXT",
                                    IsNullable = true,
                                    IsUnsigned = false,
                                    Length = null,
                                },
                                Name = "NLT",
                                Order = 8,
                            },
                        }
                    },
                };
            }
        }

        private static string[] LoadSql(string path)
        {
            var lines = File.ReadAllLines(path, new UTF8Encoding(false));
            var queries = Parse(lines, new[] { "--", "SET FOREIGN_KEY_CHECKS", "DROP SCHEMA", "CREATE SCHEMA" });
            return queries;
        }

        public static string[] Parse(string[] lines, string[] escapeLines)
        {
            var numLines = escapeLines == null
                ? lines.Select(x => x.RemoveNewLine())
                    .Select(x => x.TrimEnd())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select((x, i) => (index: i, content: x))
                    .ToArray()
                : lines.Select(x => x.RemoveNewLine())
                    .Select(x => x.TrimEnd())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Where(x => !escapeLines.Any(y => x.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                    .Select((x, i) => (index: i, content: x))
                    .ToArray();
            // query should be end with ;
            var ends = numLines.Where(x => x.content.EndsWith(";")).ToArray();
            // current query's begin index should be previous query's end index + 1.
            // 1st query is outof rule, so just prepend to head.
            var begins = Enumerable.Range(0, ends.Length)
                .SelectMany(x => numLines.Where(y => y.index == ends[x].index + 1))
                .Prepend(numLines.First())
                .ToArray();
            // pick up range
            if (begins.Length == 1)
            {
                return new[] {numLines.Select(x => x.content).ToJoinedString("\n")};
            }
            else if (begins.Zip(ends, (b, e) => (begin: b.index, end: e.index)).All(x => x.begin == x.end))
            {
                return numLines.Select(x => x.content).ToArray();
            }
            else
            {
                return Enumerable.Range(0, begins.Length - 1)
                    .Select(x => numLines
                        .Skip(begins[x].index) // CREATE TABLE ....
                        .Take(begins[x + 1].index - begins[x].index)) // .... ;
                    .Select(x => x.Select(y => y.content).ToJoinedString("\n"))
                    .ToArray();
            }
        }

        public class TestData
        {
            public string Statement { get; set; }
            public MySqlColumnDefinition[] Expected { get; set; } 
        }
    }

    public static class StringExtensions
    {
        public static string ToJoinedString(this IEnumerable<string> values, string separator = "")
            => string.Join(separator, values);
        public static string RemoveNewLine(this string value)
            => value?.Replace("\r\n", "")?.Replace("\n", "");
    }
}
