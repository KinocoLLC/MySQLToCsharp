using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using System.Collections.Generic;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class CreateTableParseTableUnitTest
    {
        [Theory]
        [MemberData(nameof(GenerateParseTestData))]
        public void ParsableTest(TestItem data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            Assert.True(listener.IsTargetStatement);
            Assert.True(listener.IsParseBegin);
            Assert.True(listener.IsParseCompleted);
            Assert.NotNull(listener.TableDefinition);

            Assert.Equal(data.Expected.Collation, definition.Collation);
            Assert.Equal(data.Expected.Engine, definition.Engine);
        }
        [Theory]
        [MemberData(nameof(SqlTableCommentTestData))]
        public void SqlTableCommentTest(TestItem data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            Assert.True(listener.IsTargetStatement);
            Assert.True(listener.IsParseBegin);
            Assert.True(listener.IsParseCompleted);
            Assert.NotNull(listener.TableDefinition);

            Assert.Equal(data.Expected.Collation, definition.Collation);
            Assert.Equal(data.Expected.Engine, definition.Engine);
            Assert.Equal(data.Expected.Comment, definition.Comment);
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
                        Expected = new MySqlTableDefinition
                        {
                            Collation = "utf8mb4_general_ci",
                            Engine = "InnoDB",
                        }
                    },
                };
            }
        }

        public static IEnumerable<object[]> SqlTableCommentTestData()
        {
            var statements = TestHelper.LoadSql("test_data/create_table_comment.sql");
            foreach (var statement in statements)
            {
                yield return new object[]
                {
                    new TestItem
                    {
                        Statement = statement,
                        Expected =new MySqlTableDefinition
                        {
                            Collation = "utf8mb4_general_ci",
                            Engine = "InnoDB",
                            Comment = "this is comment",
                        }
                    }
                };
            }
        }
        public class TestItem
        {
            public string Statement { get; set; }
            public MySqlTableDefinition Expected { get; set; }
        }
    }
}
