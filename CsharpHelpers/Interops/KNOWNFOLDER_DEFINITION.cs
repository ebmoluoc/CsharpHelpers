using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct KNOWNFOLDER_DEFINITION
    {
        public KF_CATEGORY category;
        public string pszName;
        public string pszDescription;
        public Guid fidParent;
        public string pszRelativePath;
        public string pszParsingName;
        public string pszTooltip;
        public string pszLocalizedName;
        public string pszIcon;
        public string pszSecurity;
        public uint dwAttributes;
        public KF_DEFINITION_FLAGS kfdFlags;
        public Guid ftidType;
    }
}
