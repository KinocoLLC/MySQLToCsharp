using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
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
        public void ParseString(
            [Option("-i", "input mysql ddl query to parse")]string input,
            [Option("-o", "output directory path of generated C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
        {
            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var definition = listener.TableDefinition;
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            logger.LogInformation($"Output Directory: {output}");
        }

        [Command(new[] { "--file", "-f" }, "read specified .sql file and generate C# class.")]
        public void ParseFromFile(
            [Option("-i", "input file path to parse mysql ddl query")]string input,
            [Option("-o", "output directory path of generated C# class file")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
        {
            var definition = Parser.FromFile(input, false);
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            logger.LogInformation($"Output Directory: {output}");
        }

        [Command(new[] { "--dir", "-d" }, "list directory's *.sql file and generate C# class.")]
        public void ParseFromFolder(
            [Option("-i", "input folder path to parse mysql ddl query")]string input,
            [Option("-o", "output directory path of generated C# class files")]string output,
            [Option("-n", "namespace to write")]string @namespace,
            [Option("-c", "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
        {
            var definitions = Parser.FromFolder(input, false).ToArray();
            var resolvedConverter = TypeConverterResolver.Resolve(converter);
            var generator = new Generator(addbom, resolvedConverter);

            Context.Logger.LogInformation($"Output Directory: {output}");
            logger.LogInformation($"Output Directory: {output}");
            foreach (var definition in definitions)
            {
                generator.Save(@namespace, definition, output);
            }
        }
    }
}
