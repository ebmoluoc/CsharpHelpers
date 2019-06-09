using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Helpers
{

    /// <summary>
    /// This class encapsulates the informations about the assembly.
    /// </summary>
    public class AssemblyInfo
    {

        private readonly Assembly _assembly;
        private readonly AssemblyName _assemblyName;
        private readonly string _licenseResourceName;


        /// <exception cref="ArgumentNullException">assembly cannot be null.</exception>
        public AssemblyInfo(Assembly assembly)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _assemblyName = _assembly.GetName();
        }


        /// <exception cref="ArgumentException">licenseResourceName embedded resource not found.</exception>
        /// <exception cref="ArgumentNullException">assembly cannot be null.</exception>
        public AssemblyInfo(Assembly assembly, string licenseResourceName) : this(assembly)
        {
            if (!ResourceHelper.EmbeddedResourceExists(_assembly, licenseResourceName))
                throw new ArgumentException("Embedded resource not found.", nameof(licenseResourceName));

            _licenseResourceName = licenseResourceName;
        }


        /// <summary>
        /// License of the assembly.
        /// </summary>
        public string License
        {
            get { return ResourceHelper.StringFromEmbeddedResource(_assembly, _licenseResourceName) ?? ""; }
        }


        /// <summary>
        /// Icon associated with the assembly.
        /// </summary>
        public Icon Icon
        {
            get { return Icon.ExtractAssociatedIcon(_assembly.Location); }
        }


        /// <summary>
        /// Full path of the assembly.
        /// </summary>
        public string FullPath
        {
            get { return _assembly.Location; }
        }


        /// <summary>
        /// Directory path of the assembly.
        /// </summary>
        public string DirectoryPath
        {
            get { return Path.GetDirectoryName(_assembly.Location); }
        }


        /// <summary>
        /// File name (without extension) of the assembly.
        /// </summary>
        public string FileName
        {
            get { return Path.GetFileNameWithoutExtension(_assembly.Location); }
        }


        /// <summary>
        /// File extension (without leading period) of the assembly.
        /// </summary>
        public string FileExtension
        {
            get { return Path.GetExtension(_assembly.Location).Substring(1); }
        }


        /// <summary>
        /// Name of the assembly.
        /// </summary>
        public string Name
        {
            get { return _assemblyName.Name; }
        }


        /// <summary>
        /// Product name of the assembly.
        /// </summary>
        public string Product
        {
            get { return _assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? ""; }
        }


        /// <summary>
        /// Title of the assembly.
        /// </summary>
        public string Title
        {
            get { return _assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? ""; }
        }


        /// <summary>
        /// Description of the assembly.
        /// </summary>
        public string Description
        {
            get { return _assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? ""; }
        }


        /// <summary>
        /// Company name of the assembly.
        /// </summary>
        public string Company
        {
            get { return _assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? ""; }
        }


        /// <summary>
        /// Copyright of the assembly.
        /// </summary>
        public string Copyright
        {
            get { return _assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright ?? ""; }
        }


        /// <summary>
        /// Trademark of the assembly.
        /// </summary>
        public string Trademark
        {
            get { return _assembly.GetCustomAttribute<AssemblyTrademarkAttribute>()?.Trademark ?? ""; }
        }


        /// <summary>
        /// GUID of the assembly.
        /// </summary>
        public string Guid
        {
            get { return _assembly.GetCustomAttribute<GuidAttribute>()?.Value ?? ""; }
        }


        /// <summary>
        /// Version of the assembly.
        /// </summary>
        public string Version
        {
            get { return _assemblyName.Version.ToString(); }
        }


        /// <summary>
        /// Win32 file version of the assembly.
        /// </summary>
        public string FileVersion
        {
            get { return _assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version ?? ""; }
        }


        /// <summary>
        /// Culture that the assembly's neutral resources were written in.
        /// </summary>
        public string NeutralResourcesLanguage
        {
            get { return _assembly.GetCustomAttribute<NeutralResourcesLanguageAttribute>()?.CultureName ?? ""; }
        }


        /// <summary>
        /// Culture the assembly supports.
        /// </summary>
        public string Culture
        {
            get { return _assembly.GetCustomAttribute<AssemblyCultureAttribute>()?.Culture ?? ""; }
        }


        /// <summary>
        /// Build configuration, such as retail or debug, of the assembly.
        /// </summary>
        public string Configuration
        {
            get { return _assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration ?? ""; }
        }


        /// <summary>
        /// Indicates if the types in the assembly are visible to COM.
        /// </summary>
        public bool ComVisible
        {
            get { return _assembly.GetCustomAttribute<ComVisibleAttribute>()?.Value == true; }
        }


        /// <summary>
        /// URL if support is needed for the assembly.
        /// </summary>
        public string SupportUrl
        {
            get { return _assembly.GetCustomAttribute<AssemblySupportUrlAttribute>()?.SupportUrl ?? ""; }
        }

    }

}
