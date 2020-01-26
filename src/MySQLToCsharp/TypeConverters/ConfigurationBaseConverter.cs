using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp.TypeConverters
{
    public class ConfigurationBaseConverter : ITypeConverter
    {
        public (string typeName, string[] attributes) Convert(MySqlColumnDataDefinition data)
        {
            throw new NotImplementedException();
        }
    }
}
