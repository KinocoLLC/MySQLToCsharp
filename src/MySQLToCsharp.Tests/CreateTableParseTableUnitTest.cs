using FluentAssertions;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            listener.IsTargetStatement.Should().BeTrue();
            listener.IsParseBegin.Should().BeTrue();
            listener.IsParseCompleted.Should().BeTrue();
            listener.TableDefinition.Should().NotBeNull();

            definition.Collation.Should().Be(data.Expected.Collation);
            definition.Engine.Should().Be(data.Expected.Engine);
        }
        [Theory]
        [MemberData(nameof(SqlTableCommentTestData))]
        public void SqlTableCommentTest(TestItem data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            listener.IsTargetStatement.Should().BeTrue();
            listener.IsParseBegin.Should().BeTrue();
            listener.IsParseCompleted.Should().BeTrue();
            listener.TableDefinition.Should().NotBeNull();

            definition.Collation.Should().Be(data.Expected.Collation);
            definition.Engine.Should().Be(data.Expected.Engine);
            definition.Comment.Should().Be(data.Expected.Comment);
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
