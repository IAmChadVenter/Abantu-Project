using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsTicket.API;
using AbantuTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

namespace AbantuTech.Controllers
{
    public class HelpController : Controller
    {
        //    private ApplicationDbContext db = new ApplicationDbContext();
        //    UserManager<ApplicationUser> UserManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        //    RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        //    // GET: Help
        //    public ActionResult Index(HelpMessages? messages)
        //    {
        //        ViewBag.StatusMessage =
        //            messages == HelpMessages.TicketSuccess ? "Your help request has been sent successfully, you will be emailed shortly" :
        //            messages == HelpMessages.TicketFailure ? "Your help request failed, unfortunately. Please try again in a few minutes" :
        //            messages == HelpMessages.Error ? "404 An error occurred during your request.": 
        //            "";
        //        return View();
        //    }
        //    public ActionResult CreateMemberTicket()
        //    {

        //        return View();
        //    }
        //    public ActionResult CreateMemberTicket(int id)
        //    {
        //        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //        ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
        //        var e = db.Members.Where(m => m.Email == currentUser.Email).FirstOrDefault();
        //        if (e!=null)
        //        {
        //            HelpTicket ticket = new HelpTicket()
        //            {
        //                tCreatedBy = e.Email,
        //                isMember = true,
        //            };
        //            return View(ticket);
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        return View();
        //    }
        //    public enum HelpMessages
        //    {
        //        Error,
        //        TicketSuccess,
        //        TicketFailure
        //    }
        //    }
    }
}