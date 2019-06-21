using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DISK_EXTENT
    {
        public int DiskNumber;
        public long StartingOffset;
        public long ExtentLength;
    }
}
