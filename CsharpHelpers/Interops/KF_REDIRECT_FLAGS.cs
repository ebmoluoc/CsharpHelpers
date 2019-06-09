using System;

namespace CsharpHelpers.Interops
{
    [Flags]
    public enum KF_REDIRECT_FLAGS : uint
    {
        USER_EXCLUSIVE = 0x1,
        COPY_SOURCE_DACL = 0x2,
        OWNER_USER = 0x4,
        SET_OWNER_EXPLICIT = 0x8,
        CHECK_ONLY = 0x10,
        WITH_UI = 0x20,
        UNPIN = 0x40,
        PIN = 0x80,
        COPY_CONTENTS = 0x200,
        DEL_SOURCE_CONTENTS = 0x400,
        EXCLUDE_ALL_KNOWN_SUBFOLDERS = 0x800
    }
}
