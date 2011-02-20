using System.Globalization;
using System.IO;
using System.Reflection;
using Common.Logging;
using Spring.Core.IO;

namespace FluentSpring.Context.Resources
{
    public class ExtendedAssemblyResource : AbstractResource
    {
        private readonly string resourcePath;
        private readonly Assembly assembly;
        private static readonly ILog log;
        private readonly string fullResourceName;

        static ExtendedAssemblyResource()
        {
            log = LogManager.GetLogger(typeof(ExtendedAssemblyResource));
        }

        public ExtendedAssemblyResource(string resourceName)
            : base(resourceName)
        {
            fullResourceName = resourceName;
            string[] resourcePaths = GetResourceNameWithoutProtocol(resourceName).Split('/');
            assembly = Assembly.Load(resourcePaths[0]);
            resourcePath = resourcePaths[1];
        }

        public override Stream InputStream
        {
            get
            {
                Stream manifestResourceStream = this.assembly.GetManifestResourceStream(resourcePath);
                if (manifestResourceStream == null)
                {
                    log.Error(string.Concat(new object[] { "Could not load resource with name = [", fullResourceName, "] from assembly + ", assembly }));
                    log.Error("URI specified = [" + fullResourceName + "] Spring.NET URI syntax is 'assembly://assemblyName/namespace/resourceName'.");
                    log.Error("Resource name often has the default namespace prefixed, e.g. 'assembly://MyAssembly/MyNamespace/MyNamespace.MyResource.txt'.");
                }
                return manifestResourceStream;
            }
        }
        public override string Description
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "assembly [{0}], resource [{1}]", new object[] { assembly.FullName, fullResourceName });
            }
        }
    }
}
