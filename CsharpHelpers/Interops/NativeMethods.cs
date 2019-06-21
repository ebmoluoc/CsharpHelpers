using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    internal static class NativeMethods
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void SHCreateItemFromParsingName([In] string pszPath, [In] IntPtr pbc, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void SHCreateItemInKnownFolder([In] ref Guid kfid, [In] KNOWN_FOLDER_FLAG dwKFFlags, [In, MarshalAs(UnmanagedType.LPWStr)] string pszItem, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void SHGetKnownFolderPath([In] ref Guid rfid, [In] KNOWN_FOLDER_FLAG dwFlags, [In] IntPtr hToken, [Out, MarshalAs(UnmanagedType.LPWStr)] out string ppszPath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateFile([In] string lpFileName, [In] uint dwDesiredAccess, [In] uint dwShareMode, [In] IntPtr lpSecurityAttributes, [In] uint dwCreationDisposition, [In] uint dwFlagsAndAttributes, [In] IntPtr hTemplateFile);

        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle([In] IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool DeviceIoControl([In] IntPtr hDevice, [In] uint dwIoControlCode, [In] IntPtr lpInBuffer, [In] int nInBufferSize, [Out] IntPtr lpOutBuffer, [In] int nOutBufferSize, [Out] out int lpBytesReturned, [In, Out] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool DeviceIoControl([In] IntPtr hDevice, [In] uint dwIoControlCode, [In] ref STORAGE_PROPERTY_QUERY lpInBuffer, [In] int nInBufferSize, [Out] out STORAGE_DESCRIPTOR_HEADER lpOutBuffer, [In] int nOutBufferSize, [Out] out int lpBytesReturned, [In, Out] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool DeviceIoControl([In] IntPtr hDevice, [In] uint dwIoControlCode, [In] ref STORAGE_PROPERTY_QUERY lpInBuffer, [In] int nInBufferSize, [Out] IntPtr lpOutBuffer, [In] int nOutBufferSize, [Out] out int lpBytesReturned, [In, Out] IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool DeviceIoControl([In] IntPtr hDevice, [In] uint dwIoControlCode, [In] IntPtr lpInBuffer, [In] int nInBufferSize, [Out] out STORAGE_DEVICE_NUMBER lpOutBuffer, [In] int nOutBufferSize, [Out] out int lpBytesReturned, [In, Out] IntPtr lpOverlapped);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSubMenu([In] IntPtr hMenu, [In] int nPos);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

        [DllImport("user32.dll")]
        internal static extern uint GetMenuState([In] IntPtr hMenu, [In] int uId, [In] uint uFlags);

        [DllImport("user32.dll")]
        internal static extern int GetMenuItemCount([In] IntPtr hMenu);

        [DllImport("user32.dll")]
        internal static extern int GetMenuItemID([In] IntPtr hMenu, [In] int nPos);

        [DllImport("user32.dll")]
        internal static extern bool DeleteMenu([In] IntPtr hMenu, [In] int uPosition, [In] uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool InsertMenu([In] IntPtr hMenu, [In] int uPosition, [In] uint uFlags, [In] IntPtr uIDNewItem, [In] string lpNewItem);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos([In] IntPtr hWnd, [In] IntPtr hWndInsertAfter, [In] int X, [In] int Y, [In] int cx, [In] int cy, [In] uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr DefWindowProc([In] IntPtr hWnd, [In] int Msg, [In] IntPtr wParam, [In] IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool PostMessage([In] IntPtr hWnd, [In] int Msg, [In] IntPtr wParam, [In] IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage([In] IntPtr hWnd, [In] int Msg, [In] IntPtr wParam, [In] IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int RegisterWindowMessage([In] string lpString);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GetWindowLongW([In] IntPtr hWnd, [In] int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr GetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLongW([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

        internal static IntPtr GetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex)
        {
            return IntPtr.Size == 8 ? GetWindowLongPtrW(hWnd, nIndex) : GetWindowLongW(hWnd, nIndex);
        }

        internal static IntPtr SetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong)
        {
            return IntPtr.Size == 8 ? SetWindowLongPtrW(hWnd, nIndex, dwNewLong) : SetWindowLongW(hWnd, nIndex, dwNewLong);
        }
    }
}
