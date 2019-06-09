using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFileOpenDialog
    {
        // IModalWindow
        [PreserveSig]
        HRESULT Show([In] IntPtr hwndOwner);
        // IFileDialog
        void SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] COMDLG_FILTERSPEC[] rgFilterSpec);
        void SetFileTypeIndex([In] uint iFileType);
        void GetFileTypeIndex([Out] out uint piFileType);
        void Advise([In, MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, [Out] out uint pdwCookie);
        void Unadvise([In] uint dwCookie);
        void SetOptions([In] FILEOPENDIALOGOPTIONS fos);
        void GetOptions([Out] out FILEOPENDIALOGOPTIONS fos);
        void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);
        void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);
        void GetFolder([Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void GetCurrentSelection([Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);
        void GetFileName([Out, MarshalAs(UnmanagedType.LPWStr)] out string pszName);
        void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
        void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
        void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
        void GetResult([Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] FDAP fdap);
        void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
        void Close([In, MarshalAs(UnmanagedType.Error)] uint hr);
        void SetClientGuid([In] ref Guid guid);
        void ClearClientData();
        [Obsolete("Deprecated in Windows 7.", true)]
        void SetFilter([In, MarshalAs(UnmanagedType.Interface)] object pFilter);
        //IFileOpenDialog
        void GetResults([Out, MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppenum);
        void GetSelectedItems([Out, MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppsai);
    }
}
