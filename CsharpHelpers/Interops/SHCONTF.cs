﻿using System;

namespace CsharpHelpers.Interops
{
    [Flags]
    public enum SHCONTF : uint
    {
        CHECKING_FOR_CHILDREN = 0x10,
        FOLDERS = 0x20,
        NONFOLDERS = 0x40,
        INCLUDEHIDDEN = 0x80,
        INIT_ON_FIRST_NEXT = 0x100,
        NETPRINTERSRCH = 0x200,
        SHAREABLE = 0x400,
        STORAGE = 0x800,
        NAVIGATION_ENUM = 0x1000,
        FASTITEMS = 0x2000,
        FLATLIST = 0x4000,
        ENABLE_ASYNC = 0x8000,
        INCLUDESUPERHIDDEN = 0x10000
    }
}
