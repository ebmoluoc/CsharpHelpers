using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Interops
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HRESULT
    {
        [FieldOffset(0)]
        private readonly int _value;
        public int Value { get { return _value; } }
        public short Code { get { return (short)(_value & 0x0000FFFF); } }

        public static readonly HRESULT S_OK = new HRESULT(0x00000000);
        public static readonly HRESULT S_FALSE = new HRESULT(0x00000001);
        public static readonly HRESULT E_NOTIMPL = new HRESULT(0x80004001);
        public static readonly HRESULT E_NOINTERFACE = new HRESULT(0x80004002);
        public static readonly HRESULT E_POINTER = new HRESULT(0x80004003);
        public static readonly HRESULT E_ABORT = new HRESULT(0x80004004);
        public static readonly HRESULT E_FAIL = new HRESULT(0x80004005);
        public static readonly HRESULT E_UNEXPECTED = new HRESULT(0x8000FFFF);
        public static readonly HRESULT ERROR_FILE_NOT_FOUND = new HRESULT(0x80070002);
        public static readonly HRESULT E_ACCESSDENIED = new HRESULT(0x80070005);
        public static readonly HRESULT E_HANDLE = new HRESULT(0x80070006);
        public static readonly HRESULT E_OUTOFMEMORY = new HRESULT(0x8007000E);
        public static readonly HRESULT E_INVALIDARG = new HRESULT(0x80070057);
        public static readonly HRESULT ERROR_CANCELLED = new HRESULT(0x800704C7);

        public HRESULT(int value)
        {
            _value = value;
        }

        public HRESULT(uint value)
        {
            _value = unchecked((int)value);
        }

        public bool Succeeded
        {
            get { return (_value >= 0); }
        }

        public bool Failed
        {
            get { return (_value < 0); }
        }

        public void ThrowIfFailed(string message)
        {
            if (Failed)
                throw new COMException(message, _value);
        }

        public override string ToString()
        {
            return $"0x{_value:X8}";
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            try
            {
                return this == (HRESULT)obj;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public bool Equals(HRESULT hr)
        {
            return this == hr;
        }

        public static bool operator ==(HRESULT hrA, HRESULT hrB)
        {
            return hrA._value == hrB._value;
        }

        public static bool operator !=(HRESULT hrA, HRESULT hrB)
        {
            return !(hrA._value == hrB._value);
        }
    }
}
