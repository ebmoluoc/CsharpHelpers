using System;

namespace CsharpHelpers.Interops
{
    [Flags]
    public enum KF_DEFINITION_FLAGS : uint
    {
        LOCAL_REDIRECT_ONLY = 0x2,
        ROAMABLE = 0x4,
        PRECREATE = 0x8,
        STREAM = 0x10,
        PUBLISHEXPANDEDPATH = 0x20,
        NO_REDIRECT_UI = 0x40
    }
}
