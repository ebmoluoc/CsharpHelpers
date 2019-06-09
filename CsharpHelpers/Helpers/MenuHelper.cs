using CsharpHelpers.Interops;
using System;
using System.Collections.Generic;

namespace CsharpHelpers.Helpers
{

    public static class MenuHelper
    {

        /// <summary>
        /// To append at the end of the menu, set positionIndex to -1.
        /// </summary>
        public static bool InsertSeparator(IntPtr menuHandle, int positionIndex)
        {
            return NativeMethods.InsertMenu(menuHandle, positionIndex, NativeConstants.MF_BYPOSITION | NativeConstants.MF_SEPARATOR, (IntPtr)0, null);
        }


        /// <summary>
        /// Removes the menu item at positionIndex if it is a separator.
        /// </summary>
        public static bool RemoveSeparator(IntPtr menuHandle, int positionIndex)
        {
            var state = NativeMethods.GetMenuState(menuHandle, positionIndex, NativeConstants.MF_BYPOSITION);
            if (state != uint.MaxValue && (state & NativeConstants.MF_SEPARATOR) != 0)
            {
                return NativeMethods.DeleteMenu(menuHandle, positionIndex, NativeConstants.MF_BYPOSITION);
            }
            return false;
        }


        /// <summary>
        /// To append at the end of the menu, set positionIndex to -1.
        /// </summary>
        public static bool InsertItem(IntPtr menuHandle, int position, bool byPositionIndex, int commandId, string text)
        {
            var byFlag = byPositionIndex ? NativeConstants.MF_BYPOSITION : NativeConstants.MF_BYCOMMAND;
            return NativeMethods.InsertMenu(menuHandle, position, byFlag | NativeConstants.MF_STRING, (IntPtr)commandId, text);
        }


        public static bool RemoveItem(IntPtr menuHandle, int position, bool byPositionIndex)
        {
            var byFlag = byPositionIndex ? NativeConstants.MF_BYPOSITION : NativeConstants.MF_BYCOMMAND;
            return NativeMethods.DeleteMenu(menuHandle, position, byFlag);
        }


        /// <summary>
        /// Gets the position index of a menu item from its command id.
        /// </summary>
        /// <param name="menuHandle">Handle of the menu.</param>
        /// <param name="commandId">Command id to search for.</param>
        /// <param name="searchSubmenus">If the item is not found on the specified menu, search submenus if there is any.</param>
        /// <param name="outMenuHandle">Handle of the menu or submenu where the command id was found.</param>
        /// <returns>Position index if the command id was found or -1 otherwise.</returns>
        public static int GetItemPosition(IntPtr menuHandle, int commandId, bool searchSubmenus, out IntPtr outMenuHandle)
        {
            var handles = new Queue<IntPtr>();
            handles.Enqueue(menuHandle);

            do
            {
                outMenuHandle = handles.Dequeue();
                var count = NativeMethods.GetMenuItemCount(outMenuHandle);

                for (int index = 0; index < count; ++index)
                {
                    if (NativeMethods.GetMenuItemID(outMenuHandle, index) == commandId)
                        return index;

                    if (searchSubmenus)
                    {
                        var submenu = NativeMethods.GetSubMenu(outMenuHandle, index);
                        if (submenu != IntPtr.Zero)
                            handles.Enqueue(submenu);
                    }
                }

            } while (handles.Count > 0);

            return -1;
        }


        public static bool RemoveSystemMenu(IntPtr windowHandle)
        {
            return WindowHelper.RemoveWindowStyle(windowHandle, NativeConstants.GWL_STYLE, NativeConstants.WS_SYSMENU);
        }


        public static bool RemoveSystemMenuIcon(IntPtr windowHandle)
        {
            if (WindowHelper.AddWindowStyle(windowHandle, NativeConstants.GWL_EXSTYLE, NativeConstants.WS_EX_DLGMODALFRAME))
            {
                NativeMethods.SendMessage(windowHandle, NativeConstants.WM_SETICON, (IntPtr)NativeConstants.ICON_SMALL2, IntPtr.Zero);
                NativeMethods.SendMessage(windowHandle, NativeConstants.WM_SETICON, (IntPtr)NativeConstants.ICON_SMALL, IntPtr.Zero);
                NativeMethods.SendMessage(windowHandle, NativeConstants.WM_SETICON, (IntPtr)NativeConstants.ICON_BIG, IntPtr.Zero);
                return NativeMethods.SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, 0, 0, NativeConstants.SWP_NOMOVE | NativeConstants.SWP_NOSIZE | NativeConstants.SWP_NOZORDER | NativeConstants.SWP_FRAMECHANGED);
            }
            return false;
        }


        public static bool RemoveSystemMenuClose(IntPtr systemMenuHandle)
        {
            var index = GetItemPosition(systemMenuHandle, NativeConstants.SC_CLOSE, false, out _);
            if (index != -1)
            {
                if (NativeMethods.DeleteMenu(systemMenuHandle, index, NativeConstants.MF_BYPOSITION))
                {
                    RemoveSeparator(systemMenuHandle, index - 1);
                    return true;
                }
            }
            return false;
        }


        public static bool RemoveSystemMenuMaximize(IntPtr windowHandle, IntPtr systemMenuHandle)
        {
            if (WindowHelper.RemoveWindowStyle(windowHandle, NativeConstants.GWL_STYLE, NativeConstants.WS_MAXIMIZEBOX))
            {
                if (NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_MAXIMIZE, NativeConstants.MF_BYCOMMAND))
                {
                    NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_RESTORE, NativeConstants.MF_BYCOMMAND);
                    return true;
                }
            }
            return false;
        }


        public static bool RemoveSystemMenuMinimize(IntPtr windowHandle, IntPtr systemMenuHandle)
        {
            if (WindowHelper.RemoveWindowStyle(windowHandle, NativeConstants.GWL_STYLE, NativeConstants.WS_MINIMIZEBOX))
            {
                return NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_MINIMIZE, NativeConstants.MF_BYCOMMAND);
            }
            return false;
        }


        public static bool RemoveSystemMenuRestore(IntPtr systemMenuHandle)
        {
            return NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_RESTORE, NativeConstants.MF_BYCOMMAND);
        }


        public static bool RemoveSystemMenuSize(IntPtr systemMenuHandle)
        {
            return NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_SIZE, NativeConstants.MF_BYCOMMAND);
        }


        public static bool RemoveSystemMenuMove(IntPtr systemMenuHandle)
        {
            return NativeMethods.DeleteMenu(systemMenuHandle, NativeConstants.SC_MOVE, NativeConstants.MF_BYCOMMAND);
        }

    }

}
