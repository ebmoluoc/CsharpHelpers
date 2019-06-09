using CsharpHelpers.Interops;
using System;
using System.Windows;
using System.Windows.Interop;

namespace CsharpHelpers.Helpers
{

    public static class WindowHelper
    {

        /// <summary>
        /// Gets the handle of a window. Don't call this method before the SourceInitialized event or
        /// when the window is closing.
        /// </summary>
        /// <exception cref="ArgumentNullException">Window cannot be null.</exception>
        /// <exception cref="InvalidOperationException">SourceInitialized event not raised or the window is closing.</exception>
        public static IntPtr GetWindowHandle(Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;

            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("SourceInitialized event not raised or the window is closing.");

            return handle;
        }


        /// <summary>
        /// This method is based on GetWindowLong/SetWindowLong
        /// </summary>
        public static bool AddWindowStyle(IntPtr window, int index, uint style)
        {
            var value = (uint)NativeMethods.GetWindowLongPtr(window, index);
            if (value != 0)
            {
                value |= style;
                return NativeMethods.SetWindowLongPtr(window, index, (IntPtr)value) != IntPtr.Zero;
            }

            return false;
        }


        /// <summary>
        /// This method is based on GetWindowLong/SetWindowLong
        /// </summary>
        public static bool RemoveWindowStyle(IntPtr window, int index, uint style)
        {
            var value = (uint)NativeMethods.GetWindowLongPtr(window, index);
            if (value != 0)
            {
                value &= ~style;
                return NativeMethods.SetWindowLongPtr(window, index, (IntPtr)value) != IntPtr.Zero;
            }

            return false;
        }


        public static bool ForceActivate(Window window)
        {
            if (window.WindowState == WindowState.Minimized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                var state = window.WindowState;
                window.WindowState = WindowState.Minimized;
                window.WindowState = state;
            }

            return window.Activate();
        }


        /// <summary>
        /// The window position is based on WindowStartupLocation and Owner properties.
        /// </summary>
        public static void SetWindowPosition(Window window)
        {
            if (window.WindowStartupLocation == WindowStartupLocation.CenterOwner)
            {
                if (window.Owner == null || window.Owner.WindowState != WindowState.Normal)
                    CenterWindow(window, SystemParameters.WorkArea);
                else
                    CenterWindow(window, window.Owner.RestoreBounds);
            }
            else if (window.WindowStartupLocation == WindowStartupLocation.CenterScreen)
            {
                CenterWindow(window, SystemParameters.WorkArea);
            }
            else if (window.WindowStartupLocation == WindowStartupLocation.Manual)
            {
                AdjustWindowPosition(window, SystemParameters.WorkArea);
            }
        }


        /// <summary>
        /// Center a child window into its owner window or desktop rectangle
        /// (Window.RestoreBounds or SystemParameters.WorkArea).
        /// </summary>
        public static void CenterWindow(Window child, Rect parent)
        {
            child.Left = parent.Left + (parent.Width - child.Width) / 2;
            child.Top = parent.Top + (parent.Height - child.Height) / 2;

            AdjustWindowPosition(child, SystemParameters.WorkArea);
        }


        /// <summary>
        /// Adjusts the left and top positions to ensure that the window won't be displayed
        /// outside the desktop area of the display.
        /// </summary>
        public static void AdjustWindowPosition(Window window, Rect desktop)
        {
            var right = window.Left + window.Width;
            var bottom = window.Top + window.Height;

            if (right > desktop.Right)
                window.Left -= right - desktop.Right;

            if (bottom > desktop.Bottom)
                window.Top -= bottom - desktop.Bottom;

            if (window.Left < 0 && window.Width < desktop.Width)
                window.Left = 0;

            if (window.Top < 0)
                window.Top = 0;
        }

    }

}
