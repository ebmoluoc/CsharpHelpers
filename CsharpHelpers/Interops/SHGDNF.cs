using System;

namespace CsharpHelpers.Interops
{
    [Flags]
    public enum SHGDNF : uint
    {
        NORMAL = 0,
        INFOLDER = 0x1,
        FOREDITING = 0x1000,
        FORADDRESSBAR = 0x4000,
        FORPARSING = 0x8000
    }
}
