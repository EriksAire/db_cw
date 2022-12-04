using cw_db.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace cw_db.Models
{
    public class DBSeeder
    {

        public static async void SeedAdminUser(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var _context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var user = new Customer
            {
                UserName = "eriks.aire@gmail.com",
                NormalizedEmail = "eriks.aire@gmail.com",
                Email = "eriks.aire@gmail.com",
                NormalizedUserName = "eriks.aire@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var roleStore = new RoleStore<IdentityRole>(_context);
            if (!_context.Roles.Any(r => r.Name == "admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "admin", NormalizedName = "admin" });

            }

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<Customer>();
                var hashed = password.HashPassword(user, "password@123");
                user.PasswordHash = hashed;
                var userStore = new UserStore<Customer>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "admin");
            }

            await _context.SaveChangesAsync();
        }
    }
}
