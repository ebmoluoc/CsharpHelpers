using System;

namespace CsharpHelpers.Helpers
{

    public static class StringHelper
    {

        /// <summary>
        /// Creates hexadecimal pairs separated by hyphens.
        /// </summary>
        public static string HexadecimalFromBytes(byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }


        /// <summary>
        /// Creates hexadecimal pairs separated by the specified string.
        /// </summary>
        public static string HexadecimalFromBytes(byte[] bytes, string separator)
        {
            return BitConverter.ToString(bytes).Replace("-", separator);
        }

    }

}
