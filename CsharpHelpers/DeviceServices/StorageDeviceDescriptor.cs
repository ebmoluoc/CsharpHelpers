using CsharpHelpers.Interops;
using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.DeviceServices
{

    public class StorageDeviceDescriptor : IDisposable
    {

        private IntPtr _pDescriptor;
        private STORAGE_DEVICE_DESCRIPTOR _descriptor;
        private bool _disposed;


        ~StorageDeviceDescriptor()
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
        /// Returns : True if the STORAGE_DEVICE_DESCRIPTOR structure has been loaded successfully.
        /// </summary>
        public bool Load(string pDeviceName)
        {
            var succeeded = false;

            var hDevice = NativeMethods.CreateFile(pDeviceName, 0, NativeConstants.FILE_SHARE_READ | NativeConstants.FILE_SHARE_WRITE, IntPtr.Zero, NativeConstants.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hDevice != NativeConstants.INVALID_HANDLE_VALUE)
            {
                Unload();

                var spq = new STORAGE_PROPERTY_QUERY { PropertyId = STORAGE_PROPERTY_ID.StorageDeviceProperty, QueryType = STORAGE_QUERY_TYPE.PropertyStandardQuery };

                if (NativeMethods.DeviceIoControl(hDevice, NativeConstants.IOCTL_STORAGE_QUERY_PROPERTY, ref spq, Marshal.SizeOf(spq), out var sdh, Marshal.SizeOf(typeof(STORAGE_DESCRIPTOR_HEADER)), out _, IntPtr.Zero))
                {
                    _pDescriptor = Marshal.AllocHGlobal(sdh.Size);

                    succeeded = NativeMethods.DeviceIoControl(hDevice, NativeConstants.IOCTL_STORAGE_QUERY_PROPERTY, ref spq, Marshal.SizeOf(spq), _pDescriptor, sdh.Size, out _, IntPtr.Zero);
                }

                NativeMethods.CloseHandle(hDevice);
            }

            if (succeeded)
                _descriptor = Marshal.PtrToStructure<STORAGE_DEVICE_DESCRIPTOR>(_pDescriptor);
            else
                Unload();

            return succeeded;
        }


        /// <summary>
        /// Free the resources (doesn't need to be called before Load).
        /// </summary>
        public void Unload()
        {
            if (_pDescriptor != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_pDescriptor);
                _pDescriptor = IntPtr.Zero;
            }
        }


        /// <summary>
        /// The SCSI-2 device type.
        /// </summary>
        public byte DeviceType
        {
            get { return _pDescriptor != IntPtr.Zero ? _descriptor.DeviceType : default; }
        }


        /// <summary>
        /// The SCSI-2 device type modifier (if any) - this may be zero.
        /// </summary>
        public byte DeviceTypeModifier
        {
            get { return _pDescriptor != IntPtr.Zero ? _descriptor.DeviceTypeModifier : default; }
        }


        /// <summary>
        /// Flag indicating whether the device's media (if any) is removable.
        /// </summary>
        public bool RemovableMedia
        {
            get { return _pDescriptor != IntPtr.Zero ? _descriptor.RemovableMedia : default; }
        }


        /// <summary>
        /// Flag indicating whether the device can support mulitple outstanding commands.
        /// The actual synchronization in this case is the responsibility of the port driver.
        /// </summary>
        public bool CommandQueueing
        {
            get { return _pDescriptor != IntPtr.Zero ? _descriptor.CommandQueueing : default; }
        }


        /// <summary>
        /// Contains the bus type of the device.
        /// </summary>
        public STORAGE_BUS_TYPE BusType
        {
            get { return _pDescriptor != IntPtr.Zero ? _descriptor.BusType : STORAGE_BUS_TYPE.BusTypeUnknown; }
        }


        /// <summary>
        /// Device's vendor id.
        /// </summary>
        public string VendorId
        {
            get { return GetStringData(_pDescriptor != IntPtr.Zero ? _descriptor.VendorIdOffset : 0); }
        }


        /// <summary>
        /// Device's product id.
        /// </summary>
        public string ProductId
        {
            get { return GetStringData(_pDescriptor != IntPtr.Zero ? _descriptor.ProductIdOffset : 0); }
        }


        /// <summary>
        /// Device's product revision.
        /// </summary>
        public string ProductRevision
        {
            get { return GetStringData(_pDescriptor != IntPtr.Zero ? _descriptor.ProductRevisionOffset : 0); }
        }


        /// <summary>
        /// Device's serial number.
        /// </summary>
        public string SerialNumber
        {
            // TODO: this is not the correct display of the serial number - apparently there is byte swapping to do.
            get { return GetStringData(_pDescriptor != IntPtr.Zero ? _descriptor.SerialNumberOffset : 0); }
        }


        /// <summary>
        /// Bus specific property data.
        /// </summary>
        public byte[] RawDeviceProperties
        {
            get
            {
                if (_pDescriptor != IntPtr.Zero && _descriptor.RawPropertiesLength > 0)
                {
                    var bytes = new byte[_descriptor.RawPropertiesLength];
                    var offset = Marshal.OffsetOf<STORAGE_DEVICE_DESCRIPTOR>("RawDeviceProperties").ToInt32();
                    var ptr = IntPtr.Add(_pDescriptor, offset);
                    Marshal.Copy(ptr, bytes, 0, _descriptor.RawPropertiesLength);
                    return bytes;
                }

                return new byte[0];
            }
        }


        private string GetStringData(int offset)
        {
            if (offset != 0)
            {
                var pStr = IntPtr.Add(_pDescriptor, offset);
                return Marshal.PtrToStringAnsi(pStr);
            }

            return string.Empty;
        }

    }

}
