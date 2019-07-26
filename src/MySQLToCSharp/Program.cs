using MicroBatchFramework;
using MySQLToCsharp.Listeners;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLToCsharp
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            var query = "CREATE TABLE sercol1 (id SERIAL, val INT);";
            args = new[] { "from_query", "-i", query, "-o", "bin/out", "-n", "MyNameSpace.Dazo" };
            var file = @"C:\git\kinocollc\MySQLToCsharp\src\MySQLToCsharp.Tests\test_data\sql\create_table.sql";
            args = new[] { "from_file", "-i", file, "-o", "bin/out", "-n", "MyNameSpace.Dazo" };
            var folder = @"C:\git\kinocollc\MySQLToCsharp\src\MySQLToCsharp.Tests\test_data\sql";
            args = new[] { "from_folder", "-i", folder, "-o", "bin/out", "-n", "MyNameSpace.Dazo" };

            await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<QueryToCSharp>(args);
        }
    }
    public class QueryToCSharp : BatchBase
    {
        [Command("from_query", "parse from mysql query string.")]
        public void ParseString(
            [Option("-i", "mysql query to parse")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string nameSpace)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
            Generator.Save(nameSpace, definition, output);
        }

        [Command("from_file", "parse from mysql query file.")]
        public void ParseFromFile(
            [Option("-i", "file path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string nameSpace)
        {
            var definition = Parser.FromFile(input, false);
            Generator.Save(nameSpace, definition, output);
        }

        [Command("from_folder", "parse from mysql query files in specified folder.")]
        public void ParseFromFolder(
            [Option("-i", "folder path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string nameSpace)
        {
            var definitions = Parser.FromFolder(input, false).ToArray();
            Context.Logger.LogInformation($"Output Directory: {output}");
            foreach (var definition in definitions)
            {
                Generator.Save(nameSpace, definition, output);
            }
        }
    }
}
