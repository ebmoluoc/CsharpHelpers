using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential)]
    public struct STORAGE_DEVICE_DESCRIPTOR
    {
        public int Version;
        public int Size;
        public byte DeviceType;
        public byte DeviceTypeModifier;
        [MarshalAs(UnmanagedType.U1)]
        public bool RemovableMedia;
        [MarshalAs(UnmanagedType.U1)]
        public bool CommandQueueing;
        public int VendorIdOffset;
        public int ProductIdOffset;
        public int ProductRevisionOffset;
        public int SerialNumberOffset;
        public STORAGE_BUS_TYPE BusType;
        public int RawPropertiesLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] RawDeviceProperties;
    }
}
