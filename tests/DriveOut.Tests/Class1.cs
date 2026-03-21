using DriveOut.Core;
namespace DriveOut.Tests;

public static class SmokeTests
{
    public static void Run()
    {
        VerifyEnumsDefined();
    }

    private static void VerifyEnumsDefined()
    {
        if (!Enum.IsDefined(RoleType.Driver) || !Enum.IsDefined(HazardType.BananaPeel))
        {
            throw new InvalidOperationException("Core enums should include base gameplay values.");
        }
    }
}
