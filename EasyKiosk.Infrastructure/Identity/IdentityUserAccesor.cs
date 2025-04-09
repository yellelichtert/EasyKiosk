using EasyKiosk.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace EasyKiosk.Infrastructure.Identity;

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