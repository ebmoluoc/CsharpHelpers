﻿using System;

namespace CsharpHelpers.Interops
{
    [Flags]
    public enum KNOWN_FOLDER_FLAG : uint
    {
        DEFAULT = 0x00000000,
        SIMPLE_IDLIST = 0x00000100,
        NOT_PARENT_RELATIVE = 0x00000200,
        DEFAULT_PATH = 0x00000400,
        INIT = 0x00000800,
        NO_ALIAS = 0x00001000,
        DONT_UNEXPAND = 0x00002000,
        DONT_VERIFY = 0x00004000,
        CREATE = 0x00008000,
        NO_PACKAGE_REDIRECTION = 0x00010000,
        NO_APPCONTAINER_REDIRECTION = 0x00010000,
        FORCE_PACKAGE_REDIRECTION = 0x00020000,
        FORCE_APPCONTAINER_REDIRECTION = 0x00020000,
        RETURN_FILTER_REDIRECTION_TARGET = 0x00040000,
        FORCE_APP_DATA_REDIRECTION = 0x00080000,
        ALIAS_ONLY = 0x80000000
    }
}
