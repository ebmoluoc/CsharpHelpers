using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_NUMBER
    {
        public DEVICE_TYPE DeviceType;
        public int DeviceNumber;
        public int PartitionNumber;
    }
}
