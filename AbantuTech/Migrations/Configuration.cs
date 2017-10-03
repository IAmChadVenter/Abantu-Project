namespace AbantuTech.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
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
            //var genres = new List<Genre> //category
            //{
            //    new Genre { Name = "T-Shirts", Description = "T-Shirts" },
            //    new Genre { Name = "Caps" },
            //    new Genre { Name = "Pants" },
            //    new Genre { Name = "Socks" }

            //};
            //var artists = new List<Artist> //fabric
            //{
            //    new Artist { Name = "Suede" },
            //    new Artist { Name = "Cotton" },
            //    new Artist { Name = "Polyester" },

            //};
            //new List<Album> // item
            //{
            //    new Album { Title = "Black Tee", Genre = genres.Single(g => g.Name == "T-Shirts"), Price = 80, Artist = artists.Single(a => a.Name == "Cotton"), AlbumArtUrl = "/Content/Images/Store/shirt-b.jpg" },
            //    new Album { Title = "Blue Tee", Genre = genres.Single(g => g.Name == "T-Shirts"), Price = 80, Artist = artists.Single(a => a.Name == "Cotton"), AlbumArtUrl = "/Content/Images/Store/shirt-bl.jpg" },
            //    new Album { Title = "Grey Tee", Genre = genres.Single(g => g.Name == "T-Shirts"), Price = 80, Artist = artists.Single(a => a.Name == "Cotton"), AlbumArtUrl = "/Content/Images/Store/shirt-g.jpg" },
            //}.ForEach(a => context.Albums.Add(a));
        }
    }
}
