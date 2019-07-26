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
        private static readonly ITypeConverter defaultTypeConverter = new StandardConverter();
        public ITypeConverter TypeConverter { get; }

        public Generator() : this(defaultTypeConverter)
        {
        }
        public Generator(ITypeConverter typeConverter)
        {
            TypeConverter = typeConverter;
        }

        public void Save(string @namespace, IEnumerable<MySqlTableDefinition> tables, string outputFolderPath)
        {
            foreach (var table in tables)
            {
                Save(@namespace, table, outputFolderPath);
            }
        }
        public void Save(string @namespace, MySqlTableDefinition table, string outputFolderPath)
        {
            var fileName = table.Name + extension;
            var outputFile = Path.Combine(outputFolderPath, fileName);
            var generated = Generate(@namespace, table, TypeConverter);

            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            if (File.Exists(outputFile))
            {
                var current = File.ReadAllText(outputFile, encoding);
                if (generated == current)
                {
                    Console.WriteLine($"[-] skipped: {fileName} (no change)");
                    return;
                }
            }
            Console.WriteLine($"[o] generate: {fileName}");
            File.WriteAllText(outputFile, generated, encoding);
        }

        public static string Generate(string @namespace, MySqlTableDefinition table, ITypeConverter typeConverter)
        {
            var builder = new StringBuilder();
            builder.Append($@"
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace {@namespace}
{{
    public partial class {table.Name}
    {{
");
            foreach (var column in table.Columns)
            {
                var (clrType, attributes) = typeConverter.Convert(column.Data);
                if (column.PrimaryKeyReference != null)
                {
                    builder.AppendLine($"        [Key]");
                    builder.AppendLine($"        [Column(Order = {column.Order})]");
                }
                foreach (var attribute in attributes)
                {
                    builder.AppendLine($"        [{attribute}]");
                }
                builder.AppendLine($"        public {clrType} {column.Name} {{ get; set; }}");
            }
            builder.Append(@"    }
}
");
            return builder.ToString();
        }
    }
}
