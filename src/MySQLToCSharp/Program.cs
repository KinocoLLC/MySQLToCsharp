using MicroBatchFramework;
using MySQLToCsharp.Listeners;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLToCSharp
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            var query = "CREATE TABLE sercol1 (id SERIAL, val INT);";
            args = new[] { "from_query", "-i", query, "-o", "bin/out" };
            var file = @"C:\git\kinocollc\MySQLToCsharp\MySQLToCsharp.Tests\test_data\sql\create_table.sql";
            args = new[] { "from_file", "-i", file, "-o", "bin/out" };
            var folder = @"C:\git\kinocollc\MySQLToCsharp\MySQLToCsharp.Tests\test_data\sql";
            args = new[] { "from_folder", "-i", folder, "-o", "bin/out" };

            await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<QueryToCSharp>(args);
        }
    }
    public class QueryToCSharp : BatchBase
    {
        [Command("from_query", "parse from mysql query string.")]
        public void ParseString(
            [Option("-i", "mysql query to parse")]string input,
            [Option("-o", "directory path to output C# class file")]string output)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
        }

        [Command("from_file", "parse from mysql query file.")]
        public void ParseFromFile(
            [Option("-i", "file path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output)
        {
            var definition = Parser.FromFile(input, false);
        }

        [Command("from_folder", "parse from mysql query files in specified folder.")]
        public void ParseFromFolder(
            [Option("-i", "folder path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output)
        {
            var definitions = Parser.FromFolder(input, false);
            foreach (var def in definitions)
            {

            }
        }
    }
}
