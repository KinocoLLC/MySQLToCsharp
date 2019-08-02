namespace MySQLToCsharp.TypeConverters
{
    public interface ITypeConverter
    {
        /// <summary>
        /// Convert MySQL SQL Query DataType to C# CLR Class and Attirbutes
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        (string typeName, string[] attributes) Convert(MySqlColumnDataDefinition data);
    }
}
