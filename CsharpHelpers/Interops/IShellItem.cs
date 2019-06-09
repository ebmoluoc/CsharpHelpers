using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] ref Guid rbhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppvOut);
        void GetParent([Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void GetDisplayName([In] SIGDN sigdnName, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        void GetAttributes([In] SFGAOF sfgaoMask, [Out] out SFGAOF psfgaoAttribs);
        void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] SICHINTF hint, [Out] out int piOrder);
    }
}
