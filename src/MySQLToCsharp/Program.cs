using Cocona;
using Microsoft.Extensions.Logging;
using MySQLToCsharp.Listeners;
using MySQLToCsharp.Parsers;
using MySQLToCsharp.TypeConverters;
using System;
using System.Linq;

namespace MySQLToCsharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            CoconaApp.Run<QueryToCSharp>(args);
        }
    }
    public class QueryToCSharp : CoconaConsoleAppBase
    {
        const string defaultConverter = nameof(StandardConverter);

        [Command(Description = "Convert DDL sql query and generate C# class.")]
        public void Query(
            [Option('i', Description = "input mysql ddl query to parse")]string input,
            [Option('o', Description = "output directory path of generated C# class file")]string output,
            [Option('n', Description = "namespace to write")]string @namespace,
            [Option('c', Description = "converter name to use")]string converter = defaultConverter,
            [Option(Description = "true to add bom")]bool addbom = false,
            [Option(Description = "true to dry-run")]bool dry = false)
        {
            PrintDryMessage(dry);
            Console.WriteLine($"quey executed. Output Directory: {output}");

            var listener = new CreateTableStatementDetectListener();
            IParser parser = new Parser();
            parser.Parse(input, listener);
            var table = listener.TableDefinition;
            var resolvedConverter = TypeConverterResolver.Resolve(converter);
            var className = Generator.GetClassName(table.Name);

            var generator = new Generator(addbom, resolvedConverter);
            var generated =generator.Generate(@namespace, className, table, resolvedConverter);
            generator.Save(className, generated, output, dry);

            // TraceLogger
            Context.Logger.LogTrace(generated);
        }

        [Command(Description = "Convert DDL sql file and generate C# class.")]
        public void File(
            [Option('i', Description = "input file path to parse mysql ddl query")]string input,
            [Option('o', Description = "output directory path of generated C# class file")]string output,
            [Option('n', Description = "namespace to write")]string @namespace,
            [Option('c', Description = "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
            bool dry = false)
        {
            PrintDryMessage(dry);
            Console.WriteLine($"file executed. Output Directory: {output}");

            var table = Parser.FromFile(input, false);
            var resolvedConverter = TypeConverterResolver.Resolve(converter);

            var className = Generator.GetClassName(table.Name);
            var generator = new Generator(addbom, resolvedConverter);
            var generated = generator.Generate(@namespace, className, table, resolvedConverter);
            generator.Save(className, generated, output, dry);

            // TraceLogger
            Context.Logger.LogTrace(generated);
        }

        [Command(Description = "Convert DDL sql files in the folder and generate C# class.")]
        public void Dir(
            [Option('i', Description = "input folder path to parse mysql ddl query")]string input,
            [Option('o', Description = "output directory path of generated C# class files")]string output,
            [Option('n', Description = "namespace to write")]string @namespace,
            [Option('c', Description = "converter name to use")]string converter = defaultConverter,
            bool addbom = false,
            bool dry = false)
        {
            PrintDryMessage(dry);
            Console.WriteLine($"dir executed. Output Directory: {output}");

            var tables = Parser.FromFolder(input, false).ToArray();
            var resolvedConverter = TypeConverterResolver.Resolve(converter);
            var generator = new Generator(addbom, resolvedConverter);
            foreach (var table in tables)
            {
                var className = Generator.GetClassName(table.Name);
                var generated = generator.Generate(@namespace, className, table, resolvedConverter);
                generator.Save(className, generated, output, dry);

                // TraceLogger
                Context.Logger.LogTrace(generated);
            }
        }

        private void PrintDryMessage(bool dry)
        {
            if (dry)
            {
                Console.WriteLine($"[NOTE] dry run mode, {nameof(QueryToCSharp)} will not save to file.");
            }
        }
    }
}
