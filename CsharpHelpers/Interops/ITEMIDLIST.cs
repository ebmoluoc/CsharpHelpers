using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ITEMIDLIST
    {
        public SHITEMID mkid;
    }
}
