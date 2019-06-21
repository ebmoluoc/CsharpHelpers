using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DESCRIPTOR_HEADER
    {
        public int Version;
        public int Size;
    }
}
