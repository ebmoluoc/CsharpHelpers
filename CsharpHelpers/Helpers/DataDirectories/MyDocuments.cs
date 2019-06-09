using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class MyDocuments : DataDirectory
    {
        public MyDocuments() : base(KNOWNFOLDERID.Documents)
        {
        }

        public MyDocuments(string directoryName) : base(KNOWNFOLDERID.Documents, directoryName)
        {
        }
    }

}
