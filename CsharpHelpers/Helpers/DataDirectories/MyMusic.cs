using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyMusic : DataDirectory
    {
        public MyMusic() : base(KNOWNFOLDERID.Music)
        {
        }

        public MyMusic(string directoryName) : base(KNOWNFOLDERID.Music, directoryName)
        {
        }
    }

}
