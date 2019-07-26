using FluentAssertions;
using MySQLToCsharp.Listeners;
using MySQLToCSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Xunit;

namespace MySQLToCsharp.Tests
{
    public class CreateTableStatementParseUnitTest
    {
        [Theory]
        [MemberData(nameof(GenerateTestData))]
        public void ParseTest(TestData data)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(data.Statement, listener);
            var definition = listener.TableDefinition;
            listener.IsParseBegin.Should().BeTrue();
        }

        public static IEnumerable<object[]> GenerateTestData()
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

        private static string[] LoadSql(string path)
        {
            var lines = File.ReadAllLines(path, new UTF8Encoding(false));

            var tableBeginKeyword = "CREATE TABLE";
            var numLines = lines.Select(x => x.TrimEnd())
                .Where(x => !string.IsNullOrEmpty(x))
                .Where(x => !x.StartsWith("--"))
                .Where(x => !x.StartsWith("SET FOREIGN_KEY_CHECKS"))
                .Where(x => !x.StartsWith("DROP SCHEMA"))
                .Where(x => !x.StartsWith("CREATE SCHEMA"))
                .Select((x, i) => (index: i, content: x))
                .ToArray();
            var queryRanges = numLines
                .Where(x => x.content.StartsWith(tableBeginKeyword))
                .Zip(numLines.Where(x => x.content.EndsWith(";")), (title, end) => (title, end))
                .ToArray();
            var queries = queryRanges
                .Select(x => x.title.content)
                .ToArray();
            return queries;
        }

        public class TestData
        {
            public string Statement { get; set; }
        }
    }

    public static class StringExtensions
    {
        public static string ToJoinedString(this IEnumerable<string> values, string separator = "")
            => string.Join(separator, values);
    }
}
