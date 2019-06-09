using CsharpHelpers.Interops;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Helpers
{

    public static class ComHelper
    {

        /// <summary>
        /// Creates a ShellItem from an existing file or folder path.
        /// </summary>
        /// <exception cref="FileNotFoundException">The file or folder path does not exist.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public static IShellItem ShellItemFromPath(string path)
        {
            var iidShellItem = typeof(IShellItem).GUID;
            NativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, ref iidShellItem, out var obj);
            return (IShellItem)obj;
        }


        /// <summary>
        /// Creates a ShellItem from an existing KNOWNFOLDERID.
        /// </summary>
        /// <exception cref="FileNotFoundException">The specified KNOWNFOLDERID was not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public static IShellItem ShellItemFromKnownFolder(Guid knownFolder)
        {
            var iidShellItem = typeof(IShellItem).GUID;
            NativeMethods.SHCreateItemInKnownFolder(ref knownFolder, KNOWN_FOLDER_FLAG.DEFAULT, null, ref iidShellItem, out var obj);
            return (IShellItem)obj;
        }


        /// <summary>
        /// Gets the KNOWNFOLDERID path.
        /// </summary>
        /// <exception cref="FileNotFoundException">The specified KNOWNFOLDERID was not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public static string GetKnownFolderPath(Guid knownFolder)
        {
            NativeMethods.SHGetKnownFolderPath(ref knownFolder, KNOWN_FOLDER_FLAG.DEFAULT, IntPtr.Zero, out var path);
            return path;
        }


        /// <summary>
        /// Gets all the ShellItems from ShellItemArray.
        /// </summary>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public static IShellItem[] GetShellItems(IShellItemArray shellItemArray)
        {
            shellItemArray.GetCount(out var count);
            var shellItems = new IShellItem[count];

            // TODO: getting all the items in one go might not be a good idea - but when could it fails?
            shellItemArray.EnumItems(out var enumShellItems);
            var hresult = enumShellItems.Next(count, shellItems, out var fetched);

            Marshal.ReleaseComObject(enumShellItems);

            if (hresult != HRESULT.S_OK || count != fetched)
                throw new COMException("Unexpected error from IEnumShellItems::Next.", hresult.Value);

            return shellItems;
        }

    }

}
