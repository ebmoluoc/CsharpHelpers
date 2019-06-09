using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("3aa7af7e-9b36-420c-a8e3-f77d4674a488")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IKnownFolder
    {
        void GetId([Out] out Guid pkfid);
        void GetCategory([Out] out KF_CATEGORY pCategory);
        void GetShellItem([In] KNOWN_FOLDER_FLAG dwFlags, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);
        void GetPath([In] KNOWN_FOLDER_FLAG dwFlags, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);
        void SetPath([In] KNOWN_FOLDER_FLAG dwFlags, [In, MarshalAs(UnmanagedType.LPWStr)] string pszPath);
        void GetIDList([In] KNOWN_FOLDER_FLAG dwFlags, [Out] out IntPtr ppidl);
        // TODO: GetFolderType -> Does it need a different signature because it doesn't work (or I don't know what is a FOLDERTYPEID).
        void GetFolderType([Out] out Guid pftid);
        void GetRedirectionCapabilities([Out] out KF_REDIRECTION_CAPABILITIES pCapabilities);
        void GetFolderDefinition([Out] out KNOWNFOLDER_DEFINITION pKFD);
    }
}
