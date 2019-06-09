using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("8be2d872-86aa-4d47-b776-32cca40c7018")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKnownFolderManager
    {
        void FolderIdFromCsidl([In] CSIDL nCsidl, [Out] out Guid pfid);
        void FolderIdToCsidl([In] ref Guid rfid, [Out] out CSIDL pnCsidl);
        void GetFolderIds([Out] out IntPtr ppKFId, [Out] out uint pCount);
        void GetFolder([In] ref Guid rfid, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);
        void GetFolderByName([In, MarshalAs(UnmanagedType.LPWStr)] string pszCanonicalName, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);
        void RegisterFolder([In] ref Guid rfid, [In] ref KNOWNFOLDER_DEFINITION pKFD);
        void UnregisterFolder([In] ref Guid rfid);
        void FindFolderFromPath([In, MarshalAs(UnmanagedType.LPWStr)] string pszPath, [In] FFFP_MODE mode, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);
        void FindFolderFromIDList([In] IntPtr pidl, [Out, MarshalAs(UnmanagedType.Interface)] out IKnownFolder ppkf);
        void Redirect([In] ref Guid rfid, [In] IntPtr hwnd, [In] KF_REDIRECT_FLAGS flags, [In, MarshalAs(UnmanagedType.LPWStr)] string pszTargetPath, [In] uint cFolders, [In] ref Guid pExclusion, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszError);
    }
}
