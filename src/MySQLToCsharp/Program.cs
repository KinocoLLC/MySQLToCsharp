using MicroBatchFramework;
using Microsoft.Extensions.Logging;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.TypeConverters;
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
        const string defaultConverter = nameof(StandardConverter);

        [Command(new[] { "--query", "-q" }, "read input sql and generate C# class.")]
        public void ParseString(
            [Option("-i", "mysql query to parse")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "add bom or not")]string converter = defaultConverter,
            [Option("--addbom", "add bom or not")]bool addbom = false)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            new Generator(addbom, resolvedConverter).Save(@namespace, definition, output);
        }

        [Command(new[] { "--file", "-f" }, "read specified .sql file and generate C# class.")]
        public void ParseFromFile(
            [Option("-i", "file path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "add bom or not")]string converter = defaultConverter,
            [Option("--addbom", "add bom or not")]bool addbom = false)
        {
            var definition = Parser.FromFile(input, false);
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            new Generator(addbom, resolvedConverter).Save(@namespace, definition, output);
        }

        [Command(new[] { "--dir", "-d" }, "list directory's *.sql file and generate C# class.")]
        public void ParseFromFolder(
            [Option("-i", "folder path to parse mysql query")]string input,
            [Option("-o", "directory path to output C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "add bom or not")]string converter = defaultConverter,
            [Option("--addbom", "add bom or not")]bool addbom = false)
        {
            var definitions = Parser.FromFolder(input, false).ToArray();
            var resolvedConverter = TypeConverterResolver.Resolve(converter);
            var generator = new Generator(addbom, resolvedConverter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            foreach (var definition in definitions)
            {
                generator.Save(@namespace, definition, output);
            }
        }
    }
}
