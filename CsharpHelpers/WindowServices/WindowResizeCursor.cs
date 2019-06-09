using CsharpHelpers.Interops;
using System;
using System.Windows;
using System.Windows.Interop;

namespace CsharpHelpers.WindowServices
{

    /// <summary>
    /// The purpose of this class is to remove the resizing cursor when the
    /// window MinWidth/MaxWidth or MinHeight/MaxHeight are the same, which
    /// result in the impossibility to resize horizontally or vertically.
    /// </summary>
    public sealed class WindowResizeCursor
    {

        private readonly Window _window;


        /// <summary>
        /// Instantiate this class before the window SourceInitialized event.
        /// </summary>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public WindowResizeCursor(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));

            if (_window.ResizeMode != ResizeMode.NoResize && _window.ResizeMode != ResizeMode.CanMinimize)
                _window.SourceInitialized += OnSourceInitialized;
        }


        private void OnSourceInitialized(object sender, EventArgs e)
        {
            if (PresentationSource.FromVisual(_window) is HwndSource hwndSource)
                hwndSource.AddHook(WndProc);
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeConstants.WM_NCHITTEST)
            {
                var result = NativeMethods.DefWindowProc(hwnd, msg, wParam, lParam).ToInt32();

                if (_window.MinHeight == _window.MaxHeight)
                {
                    switch (result)
                    {
                        case NativeConstants.HTTOP:
                        case NativeConstants.HTTOPLEFT:
                        case NativeConstants.HTTOPRIGHT:
                            handled = true;
                            return (IntPtr)NativeConstants.HTCAPTION;
                        case NativeConstants.HTBOTTOM:
                        case NativeConstants.HTBOTTOMLEFT:
                        case NativeConstants.HTBOTTOMRIGHT:
                            handled = true;
                            return (IntPtr)NativeConstants.HTBORDER;
                    }
                }

                if (_window.MinWidth == _window.MaxWidth)
                {
                    switch (result)
                    {
                        case NativeConstants.HTTOPLEFT:
                        case NativeConstants.HTTOPRIGHT:
                            handled = true;
                            return (IntPtr)NativeConstants.HTCAPTION;
                        case NativeConstants.HTLEFT:
                        case NativeConstants.HTRIGHT:
                        case NativeConstants.HTBOTTOMLEFT:
                        case NativeConstants.HTBOTTOMRIGHT:
                            handled = true;
                            return (IntPtr)NativeConstants.HTBORDER;
                    }
                }
            }

            return IntPtr.Zero;
        }

    }

}
