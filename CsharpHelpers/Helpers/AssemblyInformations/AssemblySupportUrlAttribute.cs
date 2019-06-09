using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Helpers
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    [ComVisible(true)]
    public sealed class AssemblySupportUrlAttribute : Attribute
    {
        /// <summary>
        /// URL if support is needed for the assembly.
        /// </summary>
        public AssemblySupportUrlAttribute(string supportUrl)
        {
            SupportUrl = supportUrl;
        }
        public string SupportUrl { get; }
    }
}
