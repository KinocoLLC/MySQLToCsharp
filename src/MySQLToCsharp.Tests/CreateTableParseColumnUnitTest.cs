using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using System.Text;
using Xunit;

namespace MySQLToCsharp.Tests;

public class CreateTableParseColumnUnitTest
{
    [Theory]
    [MemberData(nameof(GenerateParseTestData))]
    public void ParsableTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);
    }
    [Theory]
    [MemberData(nameof(SqlColumnCommentTestData))]
    public void SqlColumnCommentTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            Assert.Equal(data.Expected[i].Name, definition.Columns[i].Name);
            Assert.Equal(data.Expected[i].Order, definition.Columns[i].Order);
            Assert.Equal(data.Expected[i].AutoIncrement, definition.Columns[i].AutoIncrement);
            Assert.Equal(data.Expected[i].Data.IsNullable, definition.Columns[i].Data.IsNullable);
            Assert.Equal(data.Expected[i].Data.IsUnsigned, definition.Columns[i].Data.IsUnsigned);
            Assert.Equal(data.Expected[i].Data.Length, definition.Columns[i].Data.Length);
            Assert.Equal(data.Expected[i].Data.DataType, definition.Columns[i].Data.DataType);
            Assert.Equal(data.Expected[i].HasDefault, definition.Columns[i].HasDefault);
            Assert.Equal(data.Expected[i].Comment, definition.Columns[i].Comment);
            Assert.Equal(data.Expected[i].DefaultValue, definition.Columns[i].DefaultValue);
        }
    }
    [Theory]
    [MemberData(nameof(GenerateMultiplePkData))]
    public void MultiplePrimaryKeyColumnTests(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            Assert.Equal(data.Expected[i].Name, definition.Columns[i].Name);
            Assert.Equal(data.Expected[i].Order, definition.Columns[i].Order);
            Assert.Equal(data.Expected[i].AutoIncrement, definition.Columns[i].AutoIncrement);
            Assert.Equal(data.Expected[i].Data.IsNullable, definition.Columns[i].Data.IsNullable);
            Assert.Equal(data.Expected[i].Data.IsUnsigned, definition.Columns[i].Data.IsUnsigned);
            Assert.Equal(data.Expected[i].Data.Length, definition.Columns[i].Data.Length);
            Assert.Equal(data.Expected[i].Data.DataType, definition.Columns[i].Data.DataType);
            Assert.Equal(data.Expected[i].HasDefault, definition.Columns[i].HasDefault);
            Assert.Equal(data.Expected[i].DefaultValue, definition.Columns[i].DefaultValue);
        }
    }
    [Theory]
    [MemberData(nameof(GenerateMultiplePkData))]
    public void MultiplePrimaryKeyReferenceTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            if (data.Expected[i].PrimaryKeyReference != null)
            {
                Assert.Equal(data.Expected[i].PrimaryKeyReference.KeyName, definition.Columns[i].PrimaryKeyReference.KeyName);
                Assert.NotNull(definition.Columns[i].PrimaryKeyReference);
                Assert.NotNull(definition.PrimaryKey);
                Assert.Equal(definition.Columns[i].PrimaryKeyReference.KeyName, definition.PrimaryKey.KeyName);
                Assert.NotNull(definition.PrimaryKey.Indexes);
                Assert.Equal(definition.Columns[i].PrimaryKeyReference.Indexes.Length, definition.PrimaryKey.Indexes.Length);

                // index name must match to referenced column name
                foreach (var index in definition.PrimaryKey.Indexes)
                {
                    foreach (var reference in index.ColumnReference)
                    {
                        Assert.Equal(reference.Name, index.IndexKey);
                    }
                }
            }
        }
    }
    [Theory]
    [MemberData(nameof(GenerateMultiplePkData))]
    public void MultipleIndexKeyReferenceTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            if (data.Expected[i].IndexKeysReferences != null)
            {
                var j = 0;
                foreach (var key in definition.Columns[i].IndexKeysReferences)
                {
                    Assert.Equal(data.Expected[i].IndexKeysReferences.Skip(j).First().KeyName, key.KeyName);
                    Assert.NotNull(key.Indexes);
                    j++;
                }
                Assert.NotNull(definition.IndexKeys);
                foreach (var key in definition.IndexKeys)
                {
                    Assert.NotNull(key.Indexes);
                    Assert.NotNull(key.Indexes.Where(x => x.IndexKey == definition.Columns[i].Name));

                    // index name must match to referenced column name
                    foreach (var index in key.Indexes)
                    {
                        foreach (var reference in index.ColumnReference)
                        {
                            Assert.Equal(reference.Name, index.IndexKey);
                        }
                    }
                }
            }
        }
    }
    [Theory]
    [MemberData(nameof(GenerateMultiplePkData))]
    public void MultipleUniqueKeyReferenceTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            if (data.Expected[i].UniqueKeysReferences != null)
            {
                var j = 0;
                foreach (var key in definition.Columns[i].UniqueKeysReferences)
                {
                    Assert.Equal(data.Expected[i].UniqueKeysReferences.Skip(j).First().KeyName, key.KeyName);
                    Assert.NotNull(key.Indexes);
                    j++;
                }
                Assert.NotNull(definition.UniqueKeys);
                foreach (var key in definition.UniqueKeys)
                {
                    Assert.NotNull(key.Indexes);
                    Assert.NotNull(key.Indexes.Where(x => x.IndexKey == definition.Columns[i].Name));
                    // index name must match to referenced column name
                    foreach (var index in key.Indexes)
                    {
                        foreach (var reference in index.ColumnReference)
                        {
                            Assert.Equal(reference.Name, index.IndexKey);
                        }
                    }
                }
            }
        }
    }
    [Theory]
    [MemberData(nameof(GenerateTypeConverterTestData))]
    public void ParseColumnTypeTest(TestItem data)
    {
        var listener = new CreateTableStatementDetectListener();
        IParser parser = new Parser();
        parser.Parse(data.Statement, listener);
        var definition = listener.TableDefinition;
        Assert.True(listener.IsTargetStatement);
        Assert.True(listener.IsParseBegin);
        Assert.True(listener.IsParseCompleted);
        Assert.NotNull(listener.TableDefinition);

        for (var i = 0; i < listener.TableDefinition.Columns.Length; i++)
        {
            Assert.Equal(data.Expected[i].Name, definition.Columns[i].Name);
            Assert.Equal(data.Expected[i].Order, definition.Columns[i].Order);
            Assert.Equal(data.Expected[i].AutoIncrement, definition.Columns[i].AutoIncrement);
            Assert.Equal(data.Expected[i].Data.IsNullable, definition.Columns[i].Data.IsNullable);
            Assert.Equal(data.Expected[i].Data.IsUnsigned, definition.Columns[i].Data.IsUnsigned);
            Assert.Equal(data.Expected[i].Data.Length, definition.Columns[i].Data.Length);
            Assert.Equal(data.Expected[i].Data.DataType, definition.Columns[i].Data.DataType);
        }
    }

    public static IEnumerable<object[]> GenerateParseTestData()
    {
        var statements = TestHelper.LoadSql("test_data/create_tables.sql");
        foreach (var statement in statements)
        {
            yield return new object[]
            {
                new TestItem
                {
                    Statement = statement,
                },
            };
        }
    }

    public static IEnumerable<object[]> SqlColumnCommentTestData()
    {
        var statements = TestHelper.LoadSql("test_data/create_table_column_comment.sql");
        foreach (var statement in statements)
        {
            yield return new object[]
            {
                new TestItem
                {
                    Statement = statement,
                    Expected = new [] {
                        new MySqlColumnDefinition
                        {
                            AutoIncrement = true,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "BIGINT",
                                IsNullable = false,
                                IsUnsigned = false,
                                Length = 20,
                            },
                            Name = "Id",
                            Order = 0,
                        },
                        new MySqlColumnDefinition
                        {
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "INT",
                                IsNullable = false,
                                IsUnsigned = false,
                                Length = 11,
                            },
                            Name = "SampleId",
                            Order = 1,
                            Comment = "this is comment",
                        },
                    }
                },
            };
        }
    }

    public static IEnumerable<object[]> GenerateTypeConverterTestData()
    {
        var statements = TestHelper.LoadSql("test_data/Strings.sql");
        foreach (var statement in statements)
        {
            yield return new object[]
            {
                new TestItem
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

    public static IEnumerable<object[]> GenerateMultiplePkData()
    {
        var statements = TestHelper.LoadSql("test_data/create_table_multiplepk.sql");
        foreach (var statement in statements)
        {
            yield return new object[]
            {
                new TestItem
                {
                    Statement = statement,
                    Expected = new [] {
                        new MySqlColumnDefinition
                        {
                            Name = "Id",
                            AutoIncrement = true,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "BIGINT",
                                Length = 20,
                                IsNullable = false,
                                IsUnsigned = false,
                            },
                            HasDefault = false,
                            DefaultValue = null,
                            Order = 0,
                            PrimaryKeyReference = new MySqlKeyDefinition
                            {
                                KeyName = "Id,SampleId"
                            },
                        },
                        new MySqlColumnDefinition
                        {
                            Name = "SampleId",
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "INT",
                                Length = 11,
                                IsNullable = false,
                                IsUnsigned = false,
                            },
                            HasDefault = false,
                            DefaultValue = null,
                            Order = 1,
                            IndexKeysReferences = new HashSet<MySqlKeyDefinition>
                            {
                                new MySqlKeyDefinition
                                {
                                    KeyName = "SampleId_Status"
                                },
                            },
                            UniqueKeysReferences = new HashSet<MySqlKeyDefinition>
                            {
                                new MySqlKeyDefinition
                                {
                                    KeyName = "UQ_SampleId_MasterId"
                                },
                            }
                        },
                        new MySqlColumnDefinition
                        {
                            Name = "MasterId",
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "INT",
                                Length = 11,
                                IsNullable = false,
                                IsUnsigned = false,
                            },
                            HasDefault = false,
                            DefaultValue = null,
                            Order = 2,
                            IndexKeysReferences = new HashSet<MySqlKeyDefinition>
                            {
                                new MySqlKeyDefinition
                                {
                                    KeyName = "MasterId_Status"
                                },
                            },
                            UniqueKeysReferences = new HashSet<MySqlKeyDefinition>
                            {
                                new MySqlKeyDefinition
                                {
                                    KeyName = "UQ_SampleId_MasterId"
                                },
                            }
                        },
                        new MySqlColumnDefinition
                        {
                            Name = "Value",
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "INT",
                                Length = 11,
                                IsNullable = false,
                                IsUnsigned = false,
                            },
                            HasDefault = true,
                            DefaultValue = "0",
                            Order = 3,
                        },
                        new MySqlColumnDefinition
                        {
                            Name = "Status",
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "TINYINT",
                                Length = 3,
                                IsNullable = false,
                                IsUnsigned = true,
                            },
                            HasDefault = true,
                            DefaultValue = "1",
                            Order = 4,
                            IndexKeysReferences = new HashSet<MySqlKeyDefinition>
                            {
                                new MySqlKeyDefinition
                                {
                                    KeyName = "SampleId_Status"
                                },
                                new MySqlKeyDefinition
                                {
                                    KeyName = "MasterId_Status"
                                },
                            },
                        },
                        new MySqlColumnDefinition
                        {
                            Name = "Created",
                            AutoIncrement = false,
                            Data = new MySqlColumnDataDefinition
                            {
                                DataType = "DATETIME",
                                Length = 6,
                                IsNullable = false,
                                IsUnsigned = false,
                            },
                            HasDefault = false,
                            DefaultValue = null,
                            Order = 5,
                        },
                    }
                },
            };
        }
    }

    public class TestItem
    {
        public string Statement { get; set; }
        public MySqlColumnDefinition[] Expected { get; set; }
    }
}

public static class TestHelper
{
    public static string[] LoadSql(string path)
    {
        var lines = File.ReadAllLines(path, new UTF8Encoding(false));
        var queries = DivideQuery(lines, new[] { "--", "SET FOREIGN_KEY_CHECKS", "DROP SCHEMA", "CREATE SCHEMA" });
        return queries;
    }

    public static string[] DivideQuery(string[] lines, string[] escapeLines)
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
            return new[] { numLines.Select(x => x.content).ToJoinedString("\n") };
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

}

public static class StringExtensions
{
    public static string ToJoinedString(this IEnumerable<string> values, string separator = "")
        => string.Join(separator, values);
    public static string RemoveNewLine(this string value)
        => value?.Replace("\r\n", "")?.Replace("\n", "");
}
