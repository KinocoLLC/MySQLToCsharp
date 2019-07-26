using MicroBatchFramework;
using Microsoft.Extensions.Logging;
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
            await BatchHost.CreateDefaultBuilder().RunBatchEngineAsync<QueryToCSharp>(args);
        }
    }
    public class QueryToCSharp : BatchBase
    {
        [Command(new[] { "--query", "-q" }, "read input sql and generate C# class.")]
        public void ParseString(
            [Option("-i", "mysql query to parse")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
            new Generator().Save(@namespace, definition, output);
        }

        [Command(new[] { "--file", "-f" }, "read specified .sql file and generate C# class.")]
        public void ParseFromFile(
            [Option("-i", "file path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace)
        {
            var definition = Parser.FromFile(input, false);
            new Generator().Save(@namespace, definition, output);
        }

        [Command(new[] { "--dir", "-d" }, "list directory's *.sql file and generate C# class.")]
        public void ParseFromFolder(
            [Option("-i", "folder path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace)
        {
            var definitions = Parser.FromFolder(input, false).ToArray();
            Context.Logger.LogInformation($"Output Directory: {output}");
            var generator = new Generator();
            foreach (var definition in definitions)
            {
                generator.Save(@namespace, definition, output);
            }
        }
    }
}
