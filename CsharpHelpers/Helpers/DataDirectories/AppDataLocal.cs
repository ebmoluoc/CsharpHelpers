using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class AppDataLocal : DataDirectory
    {
        public AppDataLocal() : base(KNOWNFOLDERID.LocalAppData)
        {
        }

        public AppDataLocal(string directoryName) : base(KNOWNFOLDERID.LocalAppData, directoryName)
        {
        }
    }

}
