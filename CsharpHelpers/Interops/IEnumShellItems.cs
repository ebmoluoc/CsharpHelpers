using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("70629033-e363-4a28-a567-0db78006e6d7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumShellItems
    {
        [PreserveSig]
        HRESULT Next([In] uint celt, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface)] IShellItem[] rgelt, [Out] out uint pceltFetched);
        void Skip([In] uint celt);
        void Reset();
        void Clone([Out] out IEnumShellItems ppenum);
    }
}
