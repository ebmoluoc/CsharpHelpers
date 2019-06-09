using System;
using System.IO;

namespace CsharpHelpers.Helpers
{

    public class DataDirectory : IDataDirectory
    {

        /// <exception cref="ArgumentNullException">directoryPath cannot be null.</exception>
        public DataDirectory(string directoryPath) : this(directoryPath, "")
        {
        }


        /// <exception cref="FileNotFoundException">knownFolder KNOWNFOLDERID not found.</exception>
        public DataDirectory(Guid knownFolder) : this(ComHelper.GetKnownFolderPath(knownFolder), "")
        {
        }


        /// <exception cref="ArgumentNullException">directoryName cannot be null.</exception>
        /// <exception cref="FileNotFoundException">knownFolder KNOWNFOLDERID not found.</exception>
        public DataDirectory(Guid knownFolder, string directoryName) : this(ComHelper.GetKnownFolderPath(knownFolder), directoryName)
        {
        }


        /// <exception cref="ArgumentNullException">Both directoryPath and directoryName cannot be null.</exception>
        public DataDirectory(string directoryPath, string directoryName)
        {
            directoryPath = Path.Combine(directoryPath, directoryName);
            Directory.CreateDirectory(directoryPath);
            DirectoryPath = directoryPath;
        }


        public string DirectoryPath
        {
            get;
        }


        public string GetFilePath(string fileName)
        {
            return Path.Combine(DirectoryPath, fileName);
        }

    }

}
