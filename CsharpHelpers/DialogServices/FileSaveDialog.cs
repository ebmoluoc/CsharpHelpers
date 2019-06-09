using CsharpHelpers.Helpers;
using CsharpHelpers.Interops;
using System.IO;
using System.Runtime.InteropServices;

namespace CsharpHelpers.DialogServices
{

    public class FileSaveDialog : FileDialog<IFileSaveDialog>
    {

        /// <summary>
        /// Sets a file path to be used as the initial entry in a file Save dialog.
        /// This is used when saving a file that already exists (for new file, use SetFolder and SetFileName).
        /// </summary>
        /// <exception cref="FileNotFoundException">saveAsItem path not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetSaveAsItem(string saveAsItem)
        {
            var shellItem = ComHelper.ShellItemFromPath(saveAsItem);
            Dialog.SetSaveAsItem(shellItem);
            Marshal.ReleaseComObject(shellItem);
        }

    }

}
