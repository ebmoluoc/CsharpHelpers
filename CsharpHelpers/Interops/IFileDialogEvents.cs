using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("973510db-7d7f-452b-8975-74a85828d354")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileDialogEvents
    {
        [PreserveSig]
        HRESULT OnFileOk([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
        [PreserveSig]
        HRESULT OnFolderChanging([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psiFolder);
        void OnFolderChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
        void OnSelectionChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
        void OnShareViolation([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [Out] out FDE_SHAREVIOLATION_RESPONSE pResponse);
        void OnTypeChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);
        void OnOverwrite([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [Out] out FDE_OVERWRITE_RESPONSE pResponse);
    }
}
