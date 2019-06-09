using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SHITEMID
    {
        public ushort cb;
        public byte[] abID; //BYTE abID[1];
    }
}
