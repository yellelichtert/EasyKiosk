using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Context;

public static class ModelBuilderExtensions
{

    public static void Seed(this ModelBuilder builder)
    {
        var password = "password";
        var passwordHasher = new PasswordHasher<IdentityUser>();

        
        //Seed Roles.
        var adminRole = new IdentityRole("Admin");
        adminRole.NormalizedName = adminRole.Name.ToUpper();

        List<IdentityRole> roles = new()
        {
            adminRole
        };

        builder.Entity<IdentityRole>().HasData(roles);
        
        
        //Seed default user.
        var admin = new IdentityUser()
        {
            UserName = "admin",
            Email = "admin@email.com",
            EmailConfirmed = true
        };

        admin.NormalizedUserName = admin.UserName.ToUpper();
        admin.NormalizedEmail = admin.Email.ToUpper();
        admin.PasswordHash = passwordHasher.HashPassword(admin, password);

        builder.Entity<IdentityUser>().HasData(admin);
        
    }
    
    
}