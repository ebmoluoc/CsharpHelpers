using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CsharpHelpers.Helpers
{

    public static class PathHelper
    {

        private static readonly char[] _InvalidFatVolumeLabelChars = { '*', '?', '/', '\\', '|', ',', ';', ':', '+', '=', '<', '>', '[', ']', '"', '.' };
        private static readonly string[] _PathReservedNames = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
        private static readonly int _PathReservedNamesLength = _PathReservedNames.Length;


        public static char[] GetInvalidFatVolumeLabelChars()
        {
            return (char[])_InvalidFatVolumeLabelChars.Clone();
        }


        public static string[] GetPathReservedNames()
        {
            return (string[])_PathReservedNames.Clone();
        }


        /// <summary>
        /// Get the first drive available from the bit mask.
        /// Bit position 0 is drive A:, bit position 1 is drive B:, bit position 2 is drive C:, and so on.
        /// </summary>
        public static string DriveFromMask(uint bitmask, bool trailingBackslash)
        {
            for (var i = 0; i < 26; ++i)
            {
                if ((bitmask & 1) != 0)
                {
                    var drive = $"{(char)(i + 'A')}:";

                    if (trailingBackslash)
                        drive += "\\";

                    return drive;
                }

                bitmask >>= 1;
            }

            return null;
        }


        /// <summary>
        /// Checks whether FileInfo or DirectoryInfo has the specified FileSystemRights.
        /// </summary>
        /// <exception cref="ArgumentNullException">fileSystemInfo cannot be null.</exception>
        /// <exception cref="ArgumentException">Unknown FileSystemInfo object.</exception>
        // TODO: internal access for now, it needs more testing to make sure it's reliable.
        internal static bool HasFileSystemRights(FileSystemInfo fileSystemInfo, FileSystemRights flags)
        {
            FileSystemSecurity fileSystemSecurity;

            if (fileSystemInfo is FileInfo)
                fileSystemSecurity = ((FileInfo)fileSystemInfo).GetAccessControl(AccessControlSections.Access);
            else if (fileSystemInfo is DirectoryInfo)
                fileSystemSecurity = ((DirectoryInfo)fileSystemInfo).GetAccessControl(AccessControlSections.Access);
            else if (fileSystemInfo == null)
                throw new ArgumentNullException(nameof(fileSystemInfo));
            else
                throw new ArgumentException("Unknown FileSystemInfo object.", nameof(fileSystemInfo));

            var authorizationRuleCollection = fileSystemSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
            var windowsIdentity = WindowsIdentity.GetCurrent();

            foreach (FileSystemAccessRule rule in authorizationRuleCollection)
            {
                if (windowsIdentity.Groups.Contains(rule.IdentityReference) || windowsIdentity.User == rule.IdentityReference)
                {
                    if ((rule.FileSystemRights & flags) != 0 && rule.AccessControlType == AccessControlType.Deny)
                        return false;

                    if ((rule.FileSystemRights & flags) == flags && rule.AccessControlType == AccessControlType.Allow)
                        return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Gets the paths defined in the PATH environment variable with the possibility to add few
        /// more if needed through the optional extraPaths parameter. This function returns an empty
        /// array if the PATH environment variable couldn't be read and no extraPaths were specified.
        /// </summary>
        /// <exception cref="ArgumentNullException">extraPaths cannot be null.</exception>
        public static string[] GetEnvironmentPaths(params string[] extraPaths)
        {
            if (extraPaths == null)
                throw new ArgumentNullException(nameof(extraPaths));

            var envPath = Environment.GetEnvironmentVariable("PATH");
            if (envPath == null)
                return extraPaths;

            var envPaths = envPath.Split(new char[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries);
            var paths = new string[extraPaths.Length + envPaths.Length];

            extraPaths.CopyTo(paths, 0);
            envPaths.CopyTo(paths, extraPaths.Length);

            return paths;
        }


        /// <summary>
        /// If the file is found in one of the paths specified, the file path is
        /// returned, or null otherwise.
        /// </summary>
        public static string GetFilePath(string fileName, IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var filePath = Path.Combine(path, fileName);

                if (FileExists(filePath))
                    return filePath;
            }

            return null;
        }


        /// <summary>
        /// Determines whether the specified file exists (fully qualified path expected).
        /// </summary>
        public static bool FileExists(string path)
        {
            return IsRootedPath(path) && File.Exists(path);
        }


        /// <summary>
        /// Determines whether the specified directory exists (fully qualified path expected).
        /// </summary>
        public static bool DirectoryExists(string path)
        {
            return IsRootedPath(path) && Directory.Exists(path);
        }


        /// <summary>
        /// Fully qualified path expected.
        /// </summary>
        public static bool IsValidFilePath(string path)
        {
            return IsValidPath(path, 260);
        }


        /// <summary>
        /// Fully qualified path expected.
        /// </summary>
        public static bool IsValidDirectoryPath(string path)
        {
            return IsValidPath(path, 248);
        }


        /// <summary>
        /// This method assumes that a rooted path begins with a letter (a-z or A-Z) followed by
        /// a volume separator (:) and finally a directory separator (\). Subsequent path elements
        /// are not validated.
        /// </summary>
        public static bool IsRootedPath(string path)
        {
            if (path == null || path.Length < 3)
                return false;

            if (path[1] != Path.VolumeSeparatorChar || path[2] != Path.DirectorySeparatorChar)
                return false;

            return IsValidDriveLetter(path[0]);
        }


        public static bool IsValidDriveLetter(char drive)
        {
            return (drive >= 'a' && drive <= 'z') || (drive >= 'A' && drive <= 'Z');
        }


        public static bool IsValidFileName(string name)
        {
            if (name == null)
                return false;

            var length = name.Length;

            if (length == 0 || length > 244)
                return false;

            if (name[0] == ' ' || name[0] == '.' || name[length - 1] == ' ' || name[length - 1] == '.')
                return false;

            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                return false;

            var index = name.IndexOf('.');
            if (index == -1)
                index = length;

            name = name.Substring(0, index).ToUpper();

            for (int i = 0; i < _PathReservedNamesLength; ++i)
                if (_PathReservedNames[i] == name)
                    return false;

            return true;
        }


        /// <summary>
        /// This method expects a leading period.
        /// </summary>
        public static bool IsValidFileExtension(string extension)
        {
            if (extension == null)
                return false;

            var length = extension.Length;

            if (length < 2 || length >= 244)
                return false;

            if (extension[0] != '.')
                return false;

            if (extension[0] == '.' && extension[1] == '.')
                return false;

            if (extension[length - 1] == ' ' || extension[length - 1] == '.')
                return false;

            return extension.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        }


        private static bool IsValidPath(string path, int maxPath)
        {
            if (!IsRootedPath(path))
                return false;

            if (path.Length >= maxPath)
                return false;

            foreach (var name in path.Substring(3).Split(Path.DirectorySeparatorChar))
                if (!IsValidFileName(name))
                    return false;

            return true;
        }

    }

}
