using System;
using System.IO;

namespace CsharpHelpers.Helpers
{

    public class TextFileLogger : ILogger
    {
        private readonly FileInfo _logFile;

        /// <exception cref="ArgumentNullException">The filePath cannot be null.</exception>
        /// <exception cref="IOException">The directory cannot be created or the file is already open.</exception>
        /// <exception cref="UnauthorizedAccessException">The path is read-only.</exception>
        public TextFileLogger(string filePath)
        {
            _logFile = new FileInfo(filePath);
            _logFile.Directory.Create();

            using (var stream = _logFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                // Just to make sure the file can be created and edited.
            }
        }

        public void Write(string log)
        {
            if (!_logFile.Exists)
                _logFile.Directory.Create();

            using (var stream = _logFile.AppendText())
                stream.WriteLine($"{DateTime.Now.ToString()}  :  {log}");
        }
    }

}
