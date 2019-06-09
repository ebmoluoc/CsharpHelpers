using CsharpHelpers.Interops;

namespace CsharpHelpers.Helpers
{

    public class AppDataRoaming : DataDirectory
    {
        public AppDataRoaming() : base(KNOWNFOLDERID.RoamingAppData)
        {
        }

        public AppDataRoaming(string directoryName) : base(KNOWNFOLDERID.RoamingAppData, directoryName)
        {
        }
    }

}
