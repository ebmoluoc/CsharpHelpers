namespace CsharpHelpers.Interops
{
    public enum KF_REDIRECTION_CAPABILITIES : uint
    {
        ALLOW_ALL = 0xff,
        REDIRECTABLE = 0x1,
        DENY_ALL = 0xfff00,
        DENY_POLICY_REDIRECTED = 0x100,
        DENY_POLICY = 0x200,
        DENY_PERMISSIONS = 0x400
    }
}
