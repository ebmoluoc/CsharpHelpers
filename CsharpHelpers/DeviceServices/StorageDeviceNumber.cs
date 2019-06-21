using CsharpHelpers.Interops;
using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.DeviceServices
{

    public class StorageDeviceNumber
    {

        private STORAGE_DEVICE_NUMBER _sdn;


        public bool Load(string pDeviceName)
        {
            var succeeded = false;

            var hDevice = NativeMethods.CreateFile(pDeviceName, 0, NativeConstants.FILE_SHARE_READ | NativeConstants.FILE_SHARE_WRITE, IntPtr.Zero, NativeConstants.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hDevice != NativeConstants.INVALID_HANDLE_VALUE)
            {
                succeeded = NativeMethods.DeviceIoControl(hDevice, NativeConstants.IOCTL_STORAGE_GET_DEVICE_NUMBER, IntPtr.Zero, 0, out _sdn, Marshal.SizeOf(_sdn), out _, IntPtr.Zero);

                NativeMethods.CloseHandle(hDevice);
            }

            if (!succeeded)
                Unload();

            return succeeded;
        }


        public void Unload()
        {
            _sdn.DeviceType = 0;
            _sdn.DeviceNumber = 0;
            _sdn.PartitionNumber = 0;
        }


        public DEVICE_TYPE DeviceType
        {
            get { return _sdn.DeviceType; }
        }


        public int DeviceNumber
        {
            get { return _sdn.DeviceNumber; }
        }


        public int PartitionNumber
        {
            get { return _sdn.PartitionNumber; }
        }

    }

}
