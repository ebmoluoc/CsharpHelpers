using CsharpHelpers.Helpers;
using CsharpHelpers.Interops;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace CsharpHelpers.DialogServices
{

    public abstract class FileDialog<T> : IDisposable where T : class
    {

        private const string NullOrWhiteSpaceExceptionMessage = "Cannot be null, empty or only white-space characters.";

        private readonly IFileDialog _dialog;
        private bool _disposed;


        /// <exception cref="InvalidOperationException">Type <T> not valid.</exception>
        protected FileDialog()
        {
            if (typeof(T) == typeof(IFileOpenDialog))
            {
                _dialog = (IFileDialog)new FileOpenDialogRcw();
                _dialog.SetOptions(FILEOPENDIALOGOPTIONS.FORCEFILESYSTEM | FILEOPENDIALOGOPTIONS.PATHMUSTEXIST | FILEOPENDIALOGOPTIONS.FILEMUSTEXIST | FILEOPENDIALOGOPTIONS.NOCHANGEDIR);
            }
            else if (typeof(T) == typeof(IFileSaveDialog))
            {
                _dialog = (IFileDialog)new FileSaveDialogRcw();
                _dialog.SetOptions(FILEOPENDIALOGOPTIONS.FORCEFILESYSTEM | FILEOPENDIALOGOPTIONS.PATHMUSTEXIST | FILEOPENDIALOGOPTIONS.OVERWRITEPROMPT | FILEOPENDIALOGOPTIONS.NOREADONLYRETURN | FILEOPENDIALOGOPTIONS.NOCHANGEDIR);
            }
            else
            {
                throw new InvalidOperationException("Type <T> not valid.");
            }
        }


        ~FileDialog()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }

                if (_dialog != null)
                    Marshal.FinalReleaseComObject(_dialog);

                _disposed = true;
            }
        }


        /// <summary>
        /// The instance of the file Open/Save dialog.
        /// </summary>
        protected T Dialog
        {
            get { return (T)_dialog; }
        }


        /// <summary>
        /// When saving a file, prompt before overwriting an existing file of the same name.
        /// This is a default value for the file Save dialog.
        /// </summary>
        public bool OverwritePrompt
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.OVERWRITEPROMPT); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.OVERWRITEPROMPT, value); }
        }


        /// <summary>
        /// In the file Save dialog, only allow the user to choose a file that has one of
        /// the file name extensions specified through FileTypes.
        /// </summary>
        public bool StrictFileTypes
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.STRICTFILETYPES); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.STRICTFILETYPES, value); }
        }


        /// <summary>
        /// Don't change the current working directory. This is a default value.
        /// </summary>
        public bool NoChangeDir
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.NOCHANGEDIR); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.NOCHANGEDIR, value); }
        }


        /// <summary>
        /// Present a file Open dialog that offers a choice of folders rather than files.
        /// Using this flag on a file Save dialog will raise an exception.
        /// </summary>
        /// <exception cref="ArgumentException">The value cannot be true on a file save dialog.</exception>
        public bool PickFolders
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.PICKFOLDERS); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.PICKFOLDERS, value); }
        }


        /// <summary>
        /// Do not check for situations that would prevent an application from opening the
        /// selected file, such as sharing violations or access denied errors.
        /// </summary>
        public bool NoValidate
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.NOVALIDATE); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.NOVALIDATE, value); }
        }


        /// <summary>
        /// Enables the user to select multiple items in the file Open dialog.
        /// Using this flag on a file Save dialog will raise an exception.
        /// </summary>
        /// <exception cref="ArgumentException">The value cannot be true on a file save dialog.</exception>
        public bool AllowMultiSelect
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.ALLOWMULTISELECT); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.ALLOWMULTISELECT, value); }
        }


        /// <summary>
        /// The item returned must be in an existing folder. This is a default value.
        /// </summary>
        public bool PathMustExist
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.PATHMUSTEXIST); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.PATHMUSTEXIST, value); }
        }


        /// <summary>
        /// The item returned must exist. This is a default value for the file Open dialog.
        /// </summary>
        public bool FileMustExist
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.FILEMUSTEXIST); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.FILEMUSTEXIST, value); }
        }


        /// <summary>
        /// Prompt for creation if the item returned in the file Save dialog does not exist.
        /// Note that this does not actually create the item.
        /// </summary>
        public bool CreatePrompt
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.CREATEPROMPT); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.CREATEPROMPT, value); }
        }


        /// <summary>
        /// In the case of a sharing violation when an application is opening a file,
        /// call the application back through OnShareViolation for guidance.
        /// This flag is overridden by NOVALIDATE.
        /// </summary>
        public bool ShareAware
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.SHAREAWARE); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.SHAREAWARE, value); }
        }


        /// <summary>
        /// Do not return read-only items. This is a default value for the file Save dialog.
        /// </summary>
        public bool NoReadOnlyReturn
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.NOREADONLYRETURN); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.NOREADONLYRETURN, value); }
        }


        /// <summary>
        /// Do not test whether creation of the item as specified in the file Save dialog will
        /// be successful. If this flag is not set, the calling application must handle errors,
        /// such as denial of access, discovered when the item is created.
        /// </summary>
        public bool NoTestFileCreate
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.NOTESTFILECREATE); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.NOTESTFILECREATE, value); }
        }


        /// <summary>
        /// Hide all of the standard namespace locations (such as Favorites, Libraries, Computer,
        /// and Network) shown in the navigation pane. This flag is often used in conjunction with
        /// the AddPlace methods, to hide standard locations and replace them with custom locations. 
        /// </summary>
        public bool HidePinnedPlaces
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.HIDEPINNEDPLACES); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.HIDEPINNEDPLACES, value); }
        }


        /// <summary>
        /// Shortcuts should not be treated as their target items. This allows an application to
        /// open a .lnk file rather than what that file is a shortcut to.
        /// </summary>
        public bool NoDereferenceLinks
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.NODEREFERENCELINKS); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.NODEREFERENCELINKS, value); }
        }


        /// <summary>
        /// Do not add the item being opened or saved to the recent documents list.
        /// </summary>
        public bool DontAddToRecent
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.DONTADDTORECENT); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.DONTADDTORECENT, value); }
        }


        /// <summary>
        /// Include hidden and system items.
        /// </summary>
        public bool ForceShowHidden
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.FORCESHOWHIDDEN); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.FORCESHOWHIDDEN, value); }
        }


        /// <summary>
        /// Indicates to the file Open dialog box that the preview pane should always be displayed.
        /// </summary>
        public bool ForcePreviewPaneOn
        {
            get { return GetFosOption(FILEOPENDIALOGOPTIONS.FORCEPREVIEWPANEON); }
            set { SetFosOption(FILEOPENDIALOGOPTIONS.FORCEPREVIEWPANEON, value); }
        }


        /// <summary>
        /// Sets the title of the dialog.
        /// </summary>
        /// <exception cref="ArgumentException">title cannot be null, empty or only white-space characters.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(NullOrWhiteSpaceExceptionMessage, nameof(title));

            _dialog.SetTitle(title);
        }


        /// <summary>
        /// Sets the text of the file name label.
        /// </summary>
        /// <exception cref="ArgumentException">fileNameLabel cannot be null, empty or only white-space characters.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetFileNameLabel(string fileNameLabel)
        {
            if (string.IsNullOrWhiteSpace(fileNameLabel))
                throw new ArgumentException(NullOrWhiteSpaceExceptionMessage, nameof(fileNameLabel));

            _dialog.SetFileNameLabel(fileNameLabel);
        }


        /// <summary>
        /// Sets the text of the Open/Save button.
        /// </summary>
        /// <exception cref="ArgumentException">okButtonLabel cannot be null, empty or only white-space characters.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetOkButtonLabel(string okButtonLabel)
        {
            if (string.IsNullOrWhiteSpace(okButtonLabel))
                throw new ArgumentException(NullOrWhiteSpaceExceptionMessage, nameof(okButtonLabel));

            _dialog.SetOkButtonLabel(okButtonLabel);
        }


        /// <summary>
        /// Sets the text of the Cancel button.
        /// </summary>
        /// <exception cref="ArgumentException">cancelButtonLabel cannot be null, empty or only white-space characters.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetCancelButtonLabel(string cancelButtonLabel)
        {
            if (string.IsNullOrWhiteSpace(cancelButtonLabel))
                throw new ArgumentException(NullOrWhiteSpaceExceptionMessage, nameof(cancelButtonLabel));

            var fileDialog2 = MarshalHelper.QueryInterface<IFileDialog2>(_dialog);
            fileDialog2.SetCancelButtonLabel(cancelButtonLabel);
            Marshal.ReleaseComObject(fileDialog2);
        }


        /// <summary>
        /// Sets the location (KNOWNFOLDERID) from which to begin browsing a namespace in the navigation pane.
        /// </summary>
        /// <exception cref="FileNotFoundException">navigationRoot KNOWNFOLDERID not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetNavigationRoot(Guid navigationRoot)
        {
            var shellItem = ComHelper.ShellItemFromKnownFolder(navigationRoot);
            var fileDialog2 = MarshalHelper.QueryInterface<IFileDialog2>(_dialog);
            fileDialog2.SetNavigationRoot(shellItem);
            Marshal.ReleaseComObject(fileDialog2);
            Marshal.ReleaseComObject(shellItem);
        }


        /// <summary>
        /// Sets the folder that is selected in the dialog.
        /// </summary>
        /// <exception cref="FileNotFoundException">folder path not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetFolder(string folder)
        {
            var shellItem = ComHelper.ShellItemFromPath(folder);
            _dialog.SetFolder(shellItem);
            Marshal.ReleaseComObject(shellItem);
        }


        /// <summary>
        /// Sets the folder used as a default if there is not a recently used folder value available.
        /// </summary>
        /// <exception cref="FileNotFoundException">defaultFolder path not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetDefaultFolder(string defaultFolder)
        {
            var shellItem = ComHelper.ShellItemFromPath(defaultFolder);
            _dialog.SetDefaultFolder(shellItem);
            Marshal.ReleaseComObject(shellItem);
        }


        /// <summary>
        /// Sets the default extension to be added to the file name.
        /// This extension should not contain a leading period.
        /// </summary>
        /// <exception cref="ArgumentException">defaultExtension not valid or has leading period.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetDefaultExtension(string defaultExtension)
        {
            if (!PathHelper.IsValidFileExtension("." + defaultExtension))
                throw new ArgumentException("Not valid or has leading period.", nameof(defaultExtension));

            _dialog.SetDefaultExtension(defaultExtension);
        }


        /// <summary>
        /// Sets the file name selected in the dialog.
        /// </summary>
        /// <exception cref="ArgumentException">fileName is not valid.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetFileName(string fileName)
        {
            if (!PathHelper.IsValidFileName(fileName))
                throw new ArgumentException("Not valid.", nameof(fileName));

            _dialog.SetFileName(fileName);
        }


        /// <summary>
        /// Sets the file types filters in the dialog.
        /// The expected format is "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All files (*.*)|*.*".
        /// </summary>
        /// <exception cref="ArgumentException">fileTypes format not valid.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetFileTypes(string fileTypes)
        {
            var filterSpecs = GetFilterSpecs(fileTypes) ?? throw new ArgumentException("Format not valid.", nameof(fileTypes));

            _dialog.SetFileTypes((uint)filterSpecs.Length, filterSpecs);
        }


        /// <summary>
        /// Sets the selected file types filter in the dialog. This is a base 1 index.
        /// </summary>
        /// <exception cref="ArgumentException">fileTypeIndex must be a base 1 index.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetFileTypeIndex(int fileTypeIndex)
        {
            if (fileTypeIndex < 1)
                throw new ArgumentException("Must be a base 1 index.", nameof(fileTypeIndex));

            _dialog.SetFileTypeIndex((uint)fileTypeIndex);
        }


        /// <summary>
        /// Associates a GUID with a dialog persisted state. This way, an application
        /// can have different persisted states for different versions of the dialog.
        /// </summary>
        /// <exception cref="ArgumentException">clientGuid Empty.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void SetClientGuid(Guid clientGuid)
        {
            if (clientGuid == Guid.Empty)
                throw new ArgumentException("Empty.", nameof(clientGuid));

            _dialog.SetClientGuid(clientGuid);
        }


        /// <summary>
        /// Instructs the dialog to clear all persisted state information. Persisted information can be
        /// associated with an application or a GUID. If a GUID was set by using ClientGuid, that GUID
        /// is used to clear persisted information.
        /// </summary>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void ClearClientData()
        {
            _dialog.ClearClientData();
        }


        /// <exception cref="FileNotFoundException">path not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void AddPlaceFromPath(string path, bool top = false)
        {
            var shellItem = ComHelper.ShellItemFromPath(path);
            _dialog.AddPlace(shellItem, top ? FDAP.TOP : FDAP.BOTTOM);
            Marshal.ReleaseComObject(shellItem);
        }


        /// <exception cref="FileNotFoundException">knownFolder not found.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public void AddPlaceFromKnownFolder(Guid knownFolder, bool top = false)
        {
            var shellItem = ComHelper.ShellItemFromKnownFolder(knownFolder);
            _dialog.AddPlace(shellItem, top ? FDAP.TOP : FDAP.BOTTOM);
            Marshal.ReleaseComObject(shellItem);
        }


        /// <summary>
        /// Gets the selection made in the dialog. Only call this method if ShowDialog returns true.
        /// </summary>
        /// <returns>The selection made in the dialog.</returns>
        /// <exception cref="InvalidOperationException">Wrong GetResult(s) method was used (AllowMultiSelect).</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public string GetResult()
        {
            if (AllowMultiSelect)
                throw new InvalidOperationException($"The method {nameof(FileOpenDialog.GetResults)} must be used when {nameof(AllowMultiSelect)} is set.");

            _dialog.GetResult(out var shellItem);
            shellItem.GetDisplayName(SIGDN.FILESYSPATH, out var result);
            Marshal.ReleaseComObject(shellItem);

            return result;
        }


        /// <summary>
        /// Displays the file dialog. This method is virtual in case you would want to implement IFileDialogEvents.
        /// </summary>
        /// <param name="owner">Owner window or null for non-modal dialog.</param>
        /// <returns>True if a selection was made.</returns>
        /// <exception cref="InvalidOperationException">owner window's SourceInitialized event not raised.</exception>
        /// <exception cref="COMException">See HRESULT value for more details.</exception>
        public virtual bool ShowDialog(Window owner = null)
        {
            var hwnd = owner != null ? WindowHelper.GetWindowHandle(owner) : IntPtr.Zero;
            var hresult = _dialog.Show(hwnd);

            if (hresult != HRESULT.S_OK && hresult != HRESULT.ERROR_CANCELLED)
                throw new COMException("Unexpected error from IModalWindow::Show.", hresult.Value);

            return hresult.Succeeded;
        }


        private bool GetFosOption(FILEOPENDIALOGOPTIONS option)
        {
            _dialog.GetOptions(out var fos);
            return fos.HasFlag(option);
        }


        private void SetFosOption(FILEOPENDIALOGOPTIONS option, bool value)
        {
            _dialog.GetOptions(out var fos);

            if (value)
                fos |= option;
            else
                fos &= ~option;

            _dialog.SetOptions(fos);
        }


        private static COMDLG_FILTERSPEC[] GetFilterSpecs(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var filters = value.Split('|');
            if (filters.Length % 2 != 0)
                return null;

            var filterSpecs = new COMDLG_FILTERSPEC[filters.Length / 2];

            for (int i = 0, j = 0; i < filters.Length; i += 2)
            {
                var name = filters[i].Trim();
                var spec = filters[i + 1].Trim();

                if (name.Length == 0 || spec.Length < 3)
                    return null;

                filterSpecs[j++] = new COMDLG_FILTERSPEC { pszName = name, pszSpec = spec };
            }

            return filterSpecs;
        }

    }

}
