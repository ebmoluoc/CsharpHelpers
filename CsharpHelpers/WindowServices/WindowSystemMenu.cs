using CsharpHelpers.Helpers;
using CsharpHelpers.Interops;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;

namespace CsharpHelpers.WindowServices
{

    /// <summary>
    /// Window system menu customization.
    /// </summary>
    public sealed class WindowSystemMenu
    {
        private const string ReadOnlyPropertyExceptionMessage = "Read-only property when the SourceInitialized event is raised.";
        private const int AboutDialogId = 0xEFFF;

        private readonly Window _window;
        private IntPtr _windowHandle;

        /// <summary>
        /// Instantiate this class and set the properties before the window SourceInitialized event.
        /// </summary>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public WindowSystemMenu(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _window.SourceInitialized += OnSourceInitialized;

            var noResize = _window.ResizeMode == ResizeMode.NoResize;
            var canMinimize = _window.ResizeMode == ResizeMode.CanMinimize;
            var isFixed = _window.MinWidth == _window.MaxWidth && _window.MinHeight == _window.MaxHeight;
            MaximizeRemoved = noResize || canMinimize || isFixed;
            MinimizeRemoved = noResize || isFixed;
            RestoreRemoved = noResize || canMinimize || isFixed;
            SizeRemoved = noResize || canMinimize || isFixed;
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _menuRemoved;
        public bool MenuRemoved
        {
            get { return _menuRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _menuRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _iconRemoved;
        public bool IconRemoved
        {
            get { return _iconRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _iconRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _closeRemoved;
        public bool CloseRemoved
        {
            get { return _closeRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _closeRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _maximizeRemoved;
        public bool MaximizeRemoved
        {
            get { return _maximizeRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _maximizeRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _minimizeRemoved;
        public bool MinimizeRemoved
        {
            get { return _minimizeRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _minimizeRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _restoreRemoved;
        public bool RestoreRemoved
        {
            get { return _restoreRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _restoreRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _sizeRemoved;
        public bool SizeRemoved
        {
            get { return _sizeRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _sizeRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private bool _moveRemoved;
        public bool MoveRemoved
        {
            get { return _moveRemoved; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _moveRemoved = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private Window _aboutDialog;
        public Window AboutDialog
        {
            get { return _aboutDialog; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                _aboutDialog = value;
            }
        }


        private bool IsSourceInitialized
        {
            get { return _windowHandle != IntPtr.Zero; }
        }


        /// <exception cref="InvalidOperationException">Can't get a system menu from this window.</exception>
        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _windowHandle = WindowHelper.GetWindowHandle(_window);
            var menuHandle = NativeMethods.GetSystemMenu(_windowHandle, false);

            if (menuHandle == IntPtr.Zero)
                throw new InvalidOperationException("Can't get a system nenu handle from this window.");

            if (MenuRemoved)
                MenuHelper.RemoveSystemMenu(_windowHandle);

            if (IconRemoved)
                MenuHelper.RemoveSystemMenuIcon(_windowHandle);

            if (CloseRemoved)
                MenuHelper.RemoveSystemMenuClose(menuHandle);

            if (MaximizeRemoved)
                MenuHelper.RemoveSystemMenuMaximize(_windowHandle, menuHandle);

            if (MinimizeRemoved)
                MenuHelper.RemoveSystemMenuMinimize(_windowHandle, menuHandle);

            if (RestoreRemoved)
                MenuHelper.RemoveSystemMenuRestore(menuHandle);

            if (SizeRemoved)
                MenuHelper.RemoveSystemMenuSize(menuHandle);

            if (MoveRemoved)
                MenuHelper.RemoveSystemMenuMove(menuHandle);

            if (AboutDialog != null)
            {
                MenuHelper.InsertSeparator(menuHandle, -1);
                MenuHelper.InsertItem(menuHandle, -1, true, AboutDialogId, AboutDialog.Title);
                AboutDialog.Closing += OnAboutDialogClosing;
                AboutDialog.Owner = _window;
            }

            MenuHelper.RemoveSeparator(menuHandle, 0);

            if (PresentationSource.FromVisual(_window) is HwndSource hwndSource)
                hwndSource.AddHook(WndProc);
        }


        private void OnAboutDialogClosing(object sender, CancelEventArgs e)
        {
            ((Window)sender).Visibility = Visibility.Hidden;
            e.Cancel = true;
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeConstants.WM_SYSCOMMAND)
            {
                var command = wParam.ToInt32();

                if (command == AboutDialogId)
                {
                    WindowHelper.SetWindowPosition(AboutDialog);
                    AboutDialog.ShowDialog();
                    handled = true;
                }
                else if ((command & 0xFFF0) == NativeConstants.SC_CLOSE && (MenuRemoved || CloseRemoved))
                {
                    handled = true;
                    return (IntPtr)1;
                }
            }

            return IntPtr.Zero;
        }

    }

}
