using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace FluentSpring.Context.Resources
{
    public static class ResourceParser
    {
        private static readonly ILog logger;

        static ResourceParser()
        {
            logger = LogManager.GetLogger(typeof(ResourceParser));
        }

        public static string[] GetAssemblyResources(string[] resources)
        {
            IList<string> overridenResources = new List<string>();

            foreach (var resource in resources)
            {
                if (resource.Contains("*"))
                {
                    string assemblyName = GetAssemblyName(resource);
                    string resourcePattern = GetResourcePattern(resource);

                    string resourcesPath = resourcePattern.Replace("/", ".").Replace("*", "");
                    Assembly assembly = Assembly.Load(assemblyName);

                    string[] resourceNames = assembly.GetManifestResourceNames();

                    var resourcesFound =
                        from res in resourceNames
                        where res.Contains(resourcesPath)
                        select res;

                    foreach (var resourceFound in resourcesFound)
                    {
                        logger.Debug(string.Format("Loading up resource {0}", resourceFound));
                        overridenResources.Add(string.Format("registry://{0}/{1}", assemblyName, resourceFound));
                    }
                }
                else
                {
                    overridenResources.Add(resource);
                }
            }

            return overridenResources.ToArray();

        }

        public static string GetAssemblyName(string resource)
        {
            int startIndex = resource.IndexOf("://") + 3;
            return resource.Substring(startIndex, resource.IndexOf("/", startIndex) - startIndex);
        }

        public static string GetResourcePattern(string resource)
        {
            int startIndex = resource.IndexOf("://") + 3;
            int resourcePatternStartIndex = resource.IndexOf("/", startIndex) + 1;
            return resource.Substring(resourcePatternStartIndex);
        }
    }
}
