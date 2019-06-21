using System;
using System.Text;

namespace CsharpHelpers.Helpers
{

    public static class EnvironmentHelper
    {

        /// <summary>
        /// Search for the specified argument prefix in the command-line arguments. Returns
        /// the value after the prefix, returns an empty string if only the prefix was found
        /// or returns null if the prefix was not found.
        /// </summary>
        public static string GetArgument(string prefix)
        {
            foreach (var arg in Environment.GetCommandLineArgs())
                if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return arg.Substring(prefix.Length);

            return null;
        }


        /// <summary>
        /// Search for the specified argument prefix in the supplied command-line arguments.
        /// Returns the value after the prefix, returns an empty string if only the prefix
        /// was found or returns null if the prefix was not found.
        /// </summary>
        public static string GetArgument(string prefix, string[] args)
        {
            foreach (var arg in args)
                if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return arg.Substring(prefix.Length);

            return null;
        }


        /// <summary>
        /// https://docs.microsoft.com/en-us/cpp/cpp/parsing-cpp-command-line-arguments
        /// </summary>
        public static string EscapeArgument(string argument)
        {
            const char Backslash = '\\';
            const char Quote = '\"';

            if (argument.Length == 0)
                return "\"\"";

            if (argument.IndexOfAny(new char[] { ' ', Quote, '\t' }) == -1)
                return argument;

            var escaped = new StringBuilder(argument, argument.Length * 2);
            var index = argument.Length;
            var quote = true;

            while (--index >= 0)
            {
                if (quote && argument[index] == Backslash)
                {
                    escaped.Insert(index, Backslash);
                }
                else if (argument[index] == Quote)
                {
                    escaped.Insert(index, Backslash);
                    quote = true;
                }
                else if (quote == true)
                {
                    quote = false;
                }
            }

            return Quote + escaped.ToString() + Quote;
        }

    }

}
