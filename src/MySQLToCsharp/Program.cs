using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using MySQLToCsharp.TypeConverters;
using System.Linq;
using System.Threading.Tasks;

namespace MySQLToCsharp
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                .ConfigureLogging(logging => logging.ReplaceToSimpleConsole())
                .RunConsoleAppFrameworkAsync<QueryToCSharp>(args);
        }
    }
    public class QueryToCSharp : ConsoleAppBase
    {
        const string defaultConverter = nameof(StandardConverter);
        readonly ILogger<QueryToCSharp> logger;

        public QueryToCSharp(ILogger<QueryToCSharp> logger) => (this.logger) = logger;

        [Command(new[] { "query" }, "Convert DDL sql query and generate C# class.")]
        public void ParseString(
            [Option("-i", "input mysql ddl query to parse")]string input,
            [Option("-o", "output directory path of generated C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
            bool dry = false)
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            PrintDryMessage(dry);
            logger.LogInformation($"Output Directory: {output}");
            new Generator(addbom, resolvedConverter).Save(@namespace, definition, output, dry);
        }

        [Command(new[] { "file" }, "Convert DDL sql file and generate C# class.")]
        public void ParseFromFile(
            [Option("-i", "input file path to parse mysql ddl query")]string input,
            [Option("-o", "output directory path of generated C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
            bool dry = false)
        {
            var definition = Parser.FromFile(input, false);
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            PrintDryMessage(dry);
            logger.LogInformation($"Output Directory: {output}");
            new Generator(addbom, resolvedConverter).Save(@namespace, definition, output, dry);
        }

        [Command(new[] { "dir" }, "Convert DDL sql files in the folder and generate C# class.")]
        public void ParseFromFolder(
            [Option("-i", "input folder path to parse mysql ddl query")]string input,
            [Option("-o", "output directory path of generated C# class files")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
            bool dry = false)
        {
            var definitions = Parser.FromFolder(input, false).ToArray();
            var resolvedConverter = TypeConverterResolver.Resolve(converter);
            var generator = new Generator(addbom, resolvedConverter);

            PrintDryMessage(dry);
            logger.LogInformation($"Output Directory: {output}");
            foreach (var definition in definitions)
            {
                generator.Save(@namespace, definition, output, dry);
            }
        }

        private void PrintDryMessage(bool dry)
        {
            if (dry) logger.LogInformation($"[NOTE] dry run mode, {nameof(QueryToCSharp)} will not save to file.");
        }
    }
}
