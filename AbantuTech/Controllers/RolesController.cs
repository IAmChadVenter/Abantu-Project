using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using AbantuTech.Models;

namespace AbantuTech.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context = new ApplicationDbContext();
        // GET: Roles
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();
            List<RolesViewModel> rolemodels = new List<RolesViewModel>();
            foreach(var role in roles)
            {
                rolemodels.Add(new RolesViewModel { Name = role.Name });
            }

            return View(rolemodels);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RolesViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }
            if (!context.Roles.Any(x => x.Name == role.Name))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                manager.Create(new IdentityRole { Name = role.Name });
            }
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            var role = context.Roles.FirstOrDefault(x => x.Id.Equals(id));
            if(role!= null)
            {
                context.Roles.Remove(role);
            }
            return RedirectToAction("Index");
        }
    }
}