namespace CsharpHelpers.Interops
{
    public enum SIGDN
    {
        NORMALDISPLAY = 0x00000000,
        PARENTRELATIVEPARSING = unchecked((int)0x80018001),
        DESKTOPABSOLUTEPARSING = unchecked((int)0x80028000),
        PARENTRELATIVEEDITING = unchecked((int)0x80031001),
        DESKTOPABSOLUTEEDITING = unchecked((int)0x8004c000),
        FILESYSPATH = unchecked((int)0x80058000),
        URL = unchecked((int)0x80068000),
        PARENTRELATIVEFORADDRESSBAR = unchecked((int)0x8007c001),
        PARENTRELATIVE = unchecked((int)0x80080001),
        PARENTRELATIVEFORUI = unchecked((int)0x80094001)
    }
}
