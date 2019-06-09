using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyDesktop : DataDirectory
    {
        public MyDesktop() : base(KNOWNFOLDERID.Desktop)
        {
        }

        public MyDesktop(string directoryName) : base(KNOWNFOLDERID.Desktop, directoryName)
        {
        }
    }

}
