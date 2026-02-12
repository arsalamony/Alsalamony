using System.Security.Claims;

namespace Alsalamony.Utilities;

public static class Utitlity
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return ((string)user.FindFirstValue(ClaimTypes.Role)!) == "Admin";
    }
}
