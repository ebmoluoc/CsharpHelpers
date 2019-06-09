using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyPictures : DataDirectory
    {
        public MyPictures() : base(KNOWNFOLDERID.Pictures)
        {
        }

        public MyPictures(string directoryName) : base(KNOWNFOLDERID.Pictures, directoryName)
        {
        }
    }

}
