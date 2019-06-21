using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential)]
    public struct VOLUME_DISK_EXTENTS
    {
        public int NumberOfDiskExtents;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public DISK_EXTENT[] Extents;
    }
}
