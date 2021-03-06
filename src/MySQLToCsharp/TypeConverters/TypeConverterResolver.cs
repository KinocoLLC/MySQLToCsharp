﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            var implementations = GetImplementations(type.Assembly);
            foreach (var implementation in implementations)
            {
                AddResolver(implementation);
            }
        }

        public static Type[] GetImplementations(Assembly assembly)
        {
            var implementations = assembly.GetTypes()
                .Where(x => type.IsAssignableFrom(x))
                .Where(x => x.IsClass)
                .Where(x => x.IsPublic)
                .ToArray();
            return implementations;
        }

        public static void AddResolver(string resolverType)
        {
            if (!converterDict.ContainsKey(resolverType))
            {
                var instance = (ITypeConverter)type.Assembly.CreateInstance($"{type.Namespace}.{resolverType}")!;
                converterDict.Add(resolverType, instance);
            }
        }
        public static void AddResolver(Type resolverType)
        {
            if (!converterDict.ContainsKey(resolverType.Name))
            {
                var instance = (ITypeConverter)Activator.CreateInstance(resolverType)!;
                converterDict.Add(resolverType.Name, instance);
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
