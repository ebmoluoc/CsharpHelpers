using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyDownloads : DataDirectory
    {
        public MyDownloads() : base(KNOWNFOLDERID.Downloads)
        {
        }

        public MyDownloads(string directoryName) : base(KNOWNFOLDERID.Downloads, directoryName)
        {
        }
    }

}
