using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CsharpHelpers.Helpers
{

    public static class ExceptionHelper
    {

        /// <summary>
        /// Throws an ArgumentNullException if the value is null.
        /// </summary>
        public static void ThrowIfNull(object value, [CallerMemberName] string propertyName = "")
        {
            if (value == null)
                throw new ArgumentNullException(propertyName);
        }


        /// <summary>
        /// Throws an ArgumentOutOfRangeException if type T value is out of range.
        /// </summary>
        public static void ThrowIfOutOfRange<T>(T min, T max, T value, [CallerMemberName] string propertyName = "") where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;
            if (comparer.Compare(value, min) < 0 || comparer.Compare(value, max) > 0)
                throw new ArgumentOutOfRangeException(propertyName, $"The expected range is \"{min}\" to \"{max}\" but the value is \"{value}\".");
        }


        /// <summary>
        /// Throws an ArgumentException if the IntPtr is Zero.
        /// </summary>
        public static void ThrowIfIntPtrZero(IntPtr value, [CallerMemberName] string propertyName = "")
        {
            if (value == IntPtr.Zero)
                throw new ArgumentException("The IntPtr cannot be Zero.", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the Guid is Empty.
        /// </summary>
        public static void ThrowIfGuidEmpty(Guid value, [CallerMemberName] string propertyName = "")
        {
            if (value == Guid.Empty)
                throw new ArgumentException("The Guid cannot be Empty.", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the Enum value is not valid.
        /// </summary>
        public static void ThrowIfEnumNotValid<T>(T value, [CallerMemberName] string propertyName = "") where T : Enum
        {
            if (!Enum.IsDefined(value.GetType(), value))
                throw new ArgumentException($"The Enum value is not valid ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the collection is null or empty.
        /// </summary>
        public static void ThrowIfCollectionNullOrEmpty(ICollection value, [CallerMemberName] string propertyName = "")
        {
            if (value == null || value.Count == 0)
                throw new ArgumentException("The collection cannot be null or empty.", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the string is null or empty.
        /// </summary>
        public static void ThrowIfStringNullOrEmpty(string value, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The string cannot be null or empty.", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the string is null, empty or only white-space characters.
        /// </summary>
        public static void ThrowIfStringNullOrWhiteSpace(string value, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The string cannot be null, empty or only white-space characters.", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the file path is not found.
        /// </summary>
        public static void ThrowIfFilePathNotFound(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.FileExists(value))
                throw new ArgumentException($"The file path is not found ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the directory path is not found.
        /// </summary>
        public static void ThrowIfDirectoryPathNotFound(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.DirectoryExists(value))
                throw new ArgumentException($"The directory path is not found ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the file path is not valid.
        /// </summary>
        public static void ThrowIfFilePathNotValid(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.IsValidFilePath(value))
                throw new ArgumentException($"The file path is not valid ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the directory path is not valid.
        /// </summary>
        public static void ThrowIfDirectoryPathNotValid(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.IsValidDirectoryPath(value))
                throw new ArgumentException($"The directory path is not valid ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the file name is not valid.
        /// </summary>
        public static void ThrowIfFileNameNotValid(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.IsValidFileName(value))
                throw new ArgumentException($"The file name is not valid ({value}).", propertyName);
        }


        /// <summary>
        /// Throws an ArgumentException if the file extension is not valid.
        /// </summary>
        public static void ThrowIfFileExtensionNotValid(string value, [CallerMemberName] string propertyName = "")
        {
            if (!PathHelper.IsValidFileExtension(value))
                throw new ArgumentException($"The file extension is not valid ({value}).", propertyName);
        }

    }

}
