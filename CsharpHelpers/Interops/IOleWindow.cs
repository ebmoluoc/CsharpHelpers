using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("00000114-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleWindow
    {
        [PreserveSig]
        HRESULT GetWindow([Out] out IntPtr phwnd);
        void ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool fEnterMode);
    }
}
