namespace AbantuTech.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AbantuTech.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

        protected override void Seed(AbantuTech.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var PasswordHash = new PasswordHasher();

                if (!context.Users.Any(u => u.UserName == "admin@abantutech.com"))
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin@abantutech.com",
                        Email = "admin@abantutech.com",
                        PasswordHash = PasswordHash.HashPassword("AbantuTech01"),
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = "0854319987"
                    };

                    UserManager.Create(user);
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
