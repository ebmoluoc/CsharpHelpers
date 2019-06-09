using System.IO;
using System.Reflection;

namespace CsharpHelpers.Helpers
{

    public static class ResourceHelper
    {

        /// <summary>
        /// The resource name must not include the assembly part which will be
        /// taken from the assembly itself. The resource name is case sensitive.
        /// </summary>
        public static bool EmbeddedResourceExists(Assembly assembly, string resourceName)
        {
            var startIndex = assembly.GetName().Name.Length + 1;

            foreach (var name in assembly.GetManifestResourceNames())
            {
                if (name.Substring(startIndex) == resourceName)
                    return true;
            }

            return false;
        }


        public static string StringFromEmbeddedResource(Assembly assembly, string resourceName)
        {
            var name = $"{assembly.GetName().Name}.{resourceName}";
            var stream = assembly.GetManifestResourceStream(name);

            if (stream == null)
                return null;

            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }

    }

}
