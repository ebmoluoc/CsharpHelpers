using CsharpHelpers.Helpers;
using System;
using System.Windows;
using System.Windows.Interop;

namespace CsharpHelpers.WindowServices
{

    /// <summary>
    /// The purpose of this class is to respond to the WM_SHOW_MAIN message which bring
    /// the running application to the foreground in case of single instance application.
    /// </summary>
    public sealed class WindowShowMain
    {

        private readonly Window _window;


        /// <summary>
        /// Instantiate this class before the window SourceInitialized event.
        /// </summary>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public WindowShowMain(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _window.SourceInitialized += OnSourceInitialized;
        }


        private void OnSourceInitialized(object sender, EventArgs e)
        {
            if (PresentationSource.FromVisual(_window) is HwndSource hwndSource)
                hwndSource.AddHook(WndProc);
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == AppHelper.WM_SHOW_MAIN)
            {
                WindowHelper.ForceActivate(_window);
                handled = true;
            }

            return IntPtr.Zero;
        }

    }

}
