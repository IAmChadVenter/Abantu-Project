using AbantuTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AbantuTech.Controllers
{
    public class HelpTicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: HelpTickets
        public ActionResult Index(HelpMessages ? messages)
        {
            ViewBag.StatusMessage =
                    messages == HelpMessages.TicketSuccess ? "Your help request has been sent successfully, you will be emailed shortly" :
                    messages == HelpMessages.TicketFailure ? "Your help request failed, unfortunately. Please try again in a few minutes" :
                    messages == HelpMessages.Error ? "404 An error occurred during your request.": 
                    messages == HelpMessages.UnrestrictedError ? "You are either not signed in or not a registered member, use the non-member help support" :
                    "";
            return View();
        }
        public ActionResult allTickets()
        {
            return View(db.HelpTickets.Include(a=>a.Category).ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpTicket helpTicket = db.HelpTickets.Find(id);
            if (helpTicket == null)
            {
                return HttpNotFound();
            }
            return View(helpTicket);
        }
        [HttpGet]
        public ActionResult Create()
        {
            HelpTicket ticket = new HelpTicket();
            ViewBag.CategoryID = new SelectList(db.HelpCategories, "cID", "cName");
            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,Comment,tCreatedBy,isMember,tCreatedOn,AddFiles")] HelpTicket helpTicket,string emailNonMember)
        {
            if (!ModelState.IsValid)
            {
                return View(helpTicket);
            }
            if (helpTicket != null)
            {
                var email = User.Identity.Name;
                var member = db.Members.FirstOrDefault(x => x.Email == email);
                if (member != null)
                {
                    helpTicket.tCreatedBy = member.Email;
                    helpTicket.tCreatedOn = DateTime.UtcNow;
                    helpTicket.isMember = true;
                    db.HelpTickets.Add(helpTicket);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { helpTicket.TicketId, HelpMessages.TicketSuccess });
                }
                else
                {
                    helpTicket.tCreatedBy = emailNonMember;
                    helpTicket.tCreatedOn = DateTime.UtcNow;
                    helpTicket.isMember = false;
                    db.HelpTickets.Add(helpTicket);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { HelpMessages.TicketSuccess });
                }
            }
            return RedirectToAction("Index", new { HelpMessages.Error });
        }
        public ActionResult UploadAdditionFiles(HttpPostedFileBase file)
        {
            if(file != null && file.ContentLength > 0)
            {

            }
            return View();
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpTicket helpTicket = db.HelpTickets.Find(id);
            if (helpTicket == null)
            {
                return HttpNotFound();
            }
            return View(helpTicket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketId,Comment,tCreatedBy,isMember,tCreatedOn,AddFiles")] HelpTicket helpTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(helpTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(helpTicket);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpTicket helpTicket = db.HelpTickets.Find(id);
            if (helpTicket == null)
            {
                return HttpNotFound();
            }
            return View(helpTicket);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HelpTicket helpTicket = db.HelpTickets.Find(id);
            db.HelpTickets.Remove(helpTicket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public enum HelpMessages
        {
            Error,
            TicketSuccess,
            TicketFailure,
            UnrestrictedError
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}