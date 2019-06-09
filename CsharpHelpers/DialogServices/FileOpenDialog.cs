using CsharpHelpers.Helpers;
using CsharpHelpers.Interops;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CsharpHelpers.DialogServices
{

    public class FileOpenDialog : FileDialog<IFileOpenDialog>
    {

        /// <summary>
        /// Gets the selections made in the dialog. Only call this method if ShowDialog returns true.
        /// </summary>
        /// <returns>The selections made in the dialog.</returns>
        /// <exception cref="InvalidOperationException">Wrong GetResult(s) method was used (AllowMultiSelect).</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public List<string> GetResults()
        {
            if (!AllowMultiSelect)
                throw new InvalidOperationException($"The method {nameof(FileDialog<IFileOpenDialog>.GetResult)} must be used when {nameof(AllowMultiSelect)} is not set.");

            Dialog.GetResults(out var shellItemArray);
            var shellItems = ComHelper.GetShellItems(shellItemArray);
            Marshal.ReleaseComObject(shellItemArray);

            var results = new List<string>(shellItems.Length);

            foreach (var shellItem in shellItems)
            {
                shellItem.GetDisplayName(SIGDN.FILESYSPATH, out var path);
                Marshal.ReleaseComObject(shellItem);
                results.Add(path);
            }

            return results;
        }

    }

}
