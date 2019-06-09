using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [ComImport]
    [Guid("b63ea76d-1f85-456f-a19c-48159efa858b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemArray
    {
        void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] object pbc, [In] ref Guid rbhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppvOut);
        void GetPropertyStore([In] GETPROPERTYSTOREFLAGS Flags, [In] ref Guid riid, [Out] out object ppv);
        void GetPropertyDescriptionList([In] ref PROPERTYKEY keyType, [In] ref Guid riid, [Out] out object ppv);
        void GetAttributes([In] SIATTRIBFLAGS dwAttribFlags, [In] SFGAOF sfgaoMask, [Out] out SFGAOF psfgaoAttribs);
        void GetCount([Out] out uint pdwNumItems);
        void GetItemAt([In] uint dwIndex, [Out, MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
        void EnumItems([Out, MarshalAs(UnmanagedType.Interface)] out IEnumShellItems ppenumShellItems);
    }
}
