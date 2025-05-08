using Microsoft.AspNetCore.Identity;

namespace EasyKiosk.Server.Auth;

public sealed class IdentityUserAccesor(
    UserManager<IdentityUser> userManager)
{
    public async Task<IdentityUser> GetRequiredUserAsync(HttpContext context)
    {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null)
        {
            throw new NullReferenceException("USER NOT FOUND");
        }

        return user;
    }
}