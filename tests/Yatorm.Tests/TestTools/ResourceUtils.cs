﻿using System.Reflection;

namespace Yatorm.Tests.TestTools
{
    public class ResourceUtils
    {
        public static string GetEmbeddedResource(string key)
        {
            var resourceStream = GetEmbeddedResourceStream(key);

            if (resourceStream == null)
                return null;

            using var reader = new StreamReader(resourceStream);
            return reader.ReadToEnd();
        }

        public static Stream? GetEmbeddedResourceStream(string key, Assembly? assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            var resourceKey = assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(key));
            return resourceKey == null ? null : assembly.GetManifestResourceStream(resourceKey);
        }

        public static bool ResourceExists(string key, Assembly? assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetCallingAssembly();
            }

            return assembly.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith(key)) != null;
        }
    }
}
