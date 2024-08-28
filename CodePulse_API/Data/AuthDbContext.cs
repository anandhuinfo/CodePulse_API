using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse_API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Create reader & writer roles
            var readerRoleId = "eeb00605-7b36-4a7f-8f19-ca890de1d46a";
            var writerRoleId = "d188d2bd-9696-48b7-b309-949b9438640c";

            var roles = new List<IdentityRole> {
                new IdentityRole(){
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },

                new IdentityRole(){
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seed the roles to DB
            builder.Entity<IdentityRole>().HasData(roles);
            var adminUserId = "f3ead830 - a200 - 4b5b - 9997 - 58a424f7dcef";

            // Create an Admin User
            var admin = new IdentityUser() { 
                Id = adminUserId,
                UserName = "admin@codePulse.com",
                Email = "admin@codePulse.com",
                NormalizedEmail = "admin@codePulse.com".ToUpper(),
                NormalizedUserName = "admin@codePulse.com".ToUpper(),
                
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            
            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>() {
                new(){
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },

                new(){
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }           
            };
            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
