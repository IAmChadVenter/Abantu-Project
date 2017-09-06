using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AbantuTech.Models;

namespace AbantuTech.Models
{
    public class RolesLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> UserManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        public bool AddUserToRoleTwo(string rolename, string username)
        {
            try
            {
                ApplicationUser user = UserManger.FindByName(username);
                IdentityRole role = roleManager.FindByName(rolename);

                if (!UserManger.IsInRole(user.Id, rolename))
                {
                    UserManger.AddToRole(user.Id, rolename);

                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveUserFromRole(string rolename, string username)
        {
            try
            {
                ApplicationUser user = UserManger.FindByName(username);

                IdentityRole role = roleManager.FindByName(rolename);
                if (!UserManger.IsInRole(user.Id, rolename))
                {
                    UserManger.RemoveFromRoleAsync(user.Id, rolename);

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
