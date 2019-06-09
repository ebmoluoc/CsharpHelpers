using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyVideos : DataDirectory
    {
        public MyVideos() : base(KNOWNFOLDERID.Videos)
        {
        }

        public MyVideos(string directoryName) : base(KNOWNFOLDERID.Videos, directoryName)
        {
        }
    }

}
