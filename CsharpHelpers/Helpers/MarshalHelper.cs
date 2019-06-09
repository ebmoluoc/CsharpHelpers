using System;
using System.Runtime.InteropServices;

namespace CsharpHelpers.Helpers
{

    public static class MarshalHelper
    {

        /// <summary>Request an interface instance from a COM object.</summary>
        /// <typeparam name="T">Interface requested.</typeparam>
        /// <param name="comObject">COM object that provides the interface.</param>
        /// <returns>An instance of the interface.</returns>
        /// <exception cref="ArgumentNullException">comObject cannot be null.</exception>
        /// <exception cref="ArgumentException">T is not attributed with ComImport attribute or T is a Windows Runtime type.</exception>
        /// <exception cref="InvalidCastException">COM object can't provides the requested interface.</exception>
        public static T QueryInterface<T>(object comObject) where T : class
        {
            var unk = Marshal.GetIUnknownForObject(comObject);
            var obj = Marshal.GetTypedObjectForIUnknown(unk, typeof(T));

            Marshal.Release(unk);

            return (T)obj;
        }


        /// <summary>Copy a type T array from unmanaged memory to a managed object.</summary>
        /// <typeparam name="T">Type of array to be created.</typeparam>
        /// <param name="arrayPtr">Memory pointer of the array.</param>
        /// <param name="count">Count of items of type T in the array.</param>
        /// <returns>A new managed array of type T.</returns>
        /// <exception cref="NullReferenceException">arrayPtr cannot be Zero.</exception>
        /// <exception cref="ArgumentException">Layout of T is not sequential or explicit.</exception>
        /// <exception cref="MissingMethodException">Type T does not have an accessible default constructor.</exception>
        public static T[] PtrToStructureArray<T>(IntPtr arrayPtr, int count) where T : struct
        {
            var array = new T[count];
            var size = Marshal.SizeOf(typeof(T));

            for (int i = 0; i < count; ++i)
            {
                array[i] = Marshal.PtrToStructure<T>(arrayPtr);
                arrayPtr = IntPtr.Add(arrayPtr, size);
            }

            return array;
        }

    }

}
