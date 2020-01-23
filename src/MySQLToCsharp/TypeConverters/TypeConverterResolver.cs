using System;
using System.Collections.Generic;
using System.Text;

namespace MySQLToCsharp.TypeConverters
{
    public static class TypeConverterResolver
    {
        static readonly Dictionary<string, ITypeConverter> converterDict;

        static TypeConverterResolver()
        {
            converterDict = new Dictionary<string, ITypeConverter>();
            converterDict.Add(nameof(StandardConverter), new StandardConverter());
            converterDict.Add(nameof(StandardBitAsBoolConverter), new StandardBitAsBoolConverter());
            converterDict.Add(nameof(StandardDateTimeAsOffsetConverter), new StandardDateTimeAsOffsetConverter());
        }
        public static ITypeConverter Resolve(string key)
        {
            if (converterDict.TryGetValue(key, out var converter))
            {
                return converter;
            }

            throw new ArgumentOutOfRangeException($"Could not resolve converter '{key}'.");
        }
    }
}
