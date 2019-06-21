using CsharpHelpers.Interops;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CsharpHelpers.DeviceServices
{

    public class VolumeDiskExtents : IDisposable
    {

        private IntPtr _pVolDiskExt;
        private bool _disposed;


        ~VolumeDiskExtents()
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

                Unload();

                _disposed = true;
            }
        }


        /// <summary>
        /// pDeviceName : Device name.
        /// Returns : True if the VOLUME_DISK_EXTENTS structure has been loaded successfully.
        /// </summary>
        public bool Load(string pDeviceName)
        {
            var succeeded = false;

            var hDevice = NativeMethods.CreateFile(pDeviceName, 0, NativeConstants.FILE_SHARE_READ | NativeConstants.FILE_SHARE_WRITE, IntPtr.Zero, NativeConstants.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hDevice != NativeConstants.INVALID_HANDLE_VALUE)
            {
                Unload();

                var vdeSize = Marshal.SizeOf(typeof(VOLUME_DISK_EXTENTS));
                _pVolDiskExt = Marshal.AllocHGlobal(vdeSize);

                succeeded = NativeMethods.DeviceIoControl(hDevice, NativeConstants.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, _pVolDiskExt, vdeSize, out _, IntPtr.Zero);
                if (!succeeded && Marshal.GetLastWin32Error() == NativeConstants.ERROR_MORE_DATA)
                {
                    var vde = Marshal.PtrToStructure<VOLUME_DISK_EXTENTS>(_pVolDiskExt);
                    vdeSize = Marshal.SizeOf(typeof(VOLUME_DISK_EXTENTS)) + Marshal.SizeOf(typeof(DISK_EXTENT)) * vde.NumberOfDiskExtents;
                    _pVolDiskExt = Marshal.ReAllocHGlobal(_pVolDiskExt, (IntPtr)vdeSize);

                    succeeded = NativeMethods.DeviceIoControl(hDevice, NativeConstants.IOCTL_VOLUME_GET_VOLUME_DISK_EXTENTS, IntPtr.Zero, 0, _pVolDiskExt, vdeSize, out _, IntPtr.Zero);
                }

                NativeMethods.CloseHandle(hDevice);
            }

            if (!succeeded)
                Unload();

            return succeeded;
        }


        /// <summary>
        /// Free the resources (doesn't need to be called before Load).
        /// </summary>
        public void Unload()
        {
            if (_pVolDiskExt != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_pVolDiskExt);
                _pVolDiskExt = IntPtr.Zero;
            }
        }


        /// <summary>
        /// Returns : All the DISK_EXTENT structures from VOLUME_DISK_EXTENTS or an empty list otherwise.
        /// </summary>
        public List<DISK_EXTENT> GetDiskExtents()
        {
            var diskExtents = new List<DISK_EXTENT>();

            if (_pVolDiskExt != IntPtr.Zero)
            {
                var vde = Marshal.PtrToStructure<VOLUME_DISK_EXTENTS>(_pVolDiskExt);
                var nDiskExtents = vde.NumberOfDiskExtents;

                for (int i = 0; i < nDiskExtents; ++i)
                    diskExtents.Add(vde.Extents[i]);
            }

            return diskExtents;
        }

    }

}
