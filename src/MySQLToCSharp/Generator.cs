using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySQLToCsharp
{
    class Generator
    {
        private static readonly Encoding encoding = new UTF8Encoding(false);
        private static readonly string extension = ".cs";

        public static void Save(string nameSpace, IEnumerable<MySqlTableDefinition> tables, string outputFolderPath)
        {
            foreach (var table in tables)
            {
                Save(nameSpace, table, outputFolderPath);
            }
        }
        public static void Save(string nameSpace, MySqlTableDefinition table, string outputFolderPath)
        {
            var outputFile = Path.Combine(outputFolderPath, table.Name + extension);
            var generated = Generate(nameSpace, table);

            if (File.Exists(outputFile))
            {
                var current = File.ReadAllText(outputFile, encoding);
                if (generated == current)
                {
                    Console.WriteLine($"{outputFile} already exists, but nothing changed. skip.");
                    return;
                }
            }
            File.WriteAllText(outputFile, generated, encoding);
        }

        public static string Generate(string nameSpace, MySqlTableDefinition table)
        {
            var builder = new StringBuilder();
            builder.Append($@"
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace {nameSpace}
{{
    public partial class {table.Name}
    {{
");
            foreach (var column in table.Columns)
            {
                if (column.PrimaryKeyReference != null)
                {
                    builder.AppendLine($"        [Key]");
                    builder.AppendLine($"        [Column(Order = {column.Order})]");
                }

                //TODO: Attirbute 定義する
                //foreach (var attribute in column.Attribute)
                //{

                //}
                builder.AppendLine($"        public {column.Data.DataType} {column.Name} {{ get; set; }}");
            }
            builder.Append(@"    }
}
");
            return builder.ToString();
        }
    }
}
