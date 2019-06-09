using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("000214e6-0000-0000-c000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellFolder
    {
        void ParseDisplayName([In] IntPtr hwnd, [In, MarshalAs(UnmanagedType.Interface)] object pbc, [In, MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [Out] out uint pchEaten, [Out] out IntPtr ppidl, [In, Out] ref uint pdwAttributes);
        void EnumObjects([In] IntPtr hwnd, [In] SHCONTF grfFlags, [Out, MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);
        void BindToObject([In] IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);
        void BindToStorage([In] IntPtr pidl, [In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);
        [PreserveSig]
        HRESULT CompareIDs([In] IntPtr lParam, [In] IntPtr pidl1, [In] IntPtr pidl2);
        void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);
        // TODO: GetAttributesOf -> not sure about apidl
        void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In, Out] ref SFGAOF rgfInOut);
        // TODO: GetUIObjectOf -> not sure about apidl (the way MS do it https://referencesource.microsoft.com/#PresentationFramework/src/Framework/MS/Internal/AppModel/ShellProvider.cs,8996fb8bb7c8017f)
        void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.SysInt, SizeParamIndex = 1)] IntPtr apidl, [In] ref Guid riid, [In, Out] ref uint rgfReserved, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);
        void GetDisplayNameOf([In] IntPtr pidl, [In] SHGDNF uFlags, [Out] out IntPtr pName);
        void SetNameOf([In] IntPtr hwnd, [In] IntPtr pidl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] SHGDNF uFlags, [Out] out IntPtr ppidlOut);
    }
}
