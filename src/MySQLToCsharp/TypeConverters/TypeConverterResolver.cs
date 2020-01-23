using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MySQLToCsharp.TypeConverters
{
    public static class TypeConverterResolver
    {
        static readonly Dictionary<string, ITypeConverter> converterDict;
        static readonly Type type;

        static TypeConverterResolver()
        {
            type = typeof(ITypeConverter);
            converterDict = new Dictionary<string, ITypeConverter>();

            AddResolver(nameof(StandardConverter));
            AddResolver(nameof(StandardBitAsBoolConverter));
            AddResolver(nameof(StandardDateTimeAsOffsetConverter));
        }

        public static void AddResolver(string resolverType)
        {
            if (!converterDict.ContainsKey(resolverType))
            {
                var instance = (ITypeConverter)type.Assembly.CreateInstance($"{type.Namespace}.{resolverType}");
                converterDict.Add(resolverType, instance);
            }
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
