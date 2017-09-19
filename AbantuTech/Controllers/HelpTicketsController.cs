using AbantuTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AbantuTech.Controllers
{
    public class HelpTicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: HelpTickets
        public ActionResult Index(HelpMessages? messages)
        {
            ViewBag.StatusMessage =
                    messages == HelpMessages.TicketSuccess ? "Your help request has been sent successfully, you will be emailed shortly" :
                    messages == HelpMessages.TicketFailure ? "Your help request failed, unfortunately. Please try again in a few minutes" :
                    messages == HelpMessages.Error ? "404 An error occurred during your request." :
                    messages == HelpMessages.UnrestrictedError ? "You are either not signed in or not a registered member, use the non-member help support" :
                    "";
            return View();
        }
        public ActionResult allTickets()
        {
            return View(db.HelpTickets.Include(a => a.Category).Include(x=>x.Comments).ToList());
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
        public ActionResult Create([Bind(Include = "TicketId,Comment,tCreatedOn")] HelpTicket helpTicket, HttpPostedFileBase file, string comment)
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
                    helpTicket.tCreatedOn = DateTime.UtcNow;
                    db.HelpTickets.Add(helpTicket);
                    member.Tickets.Add(helpTicket);
                    var com = new TicketComment
                    {
                        Comment = comment,
                        HelpTicket = helpTicket,
                        tId = helpTicket.TicketId
                    };
                    helpTicket.Comments.Add(com);
                    if(file!= null && file.ContentLength>0)
                    {
                        helpFileUpload(file, member.Email, helpTicket);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", new { helpTicket.TicketId, HelpMessages.TicketSuccess });
             }
             return RedirectToAction("Index", new { HelpMessages.Error });
        }
        public ActionResult helpFileUpload(HttpPostedFileBase file, string email, HelpTicket ticket)
        {
            email = User.Identity.Name;
            var fname = System.IO.Path.GetFileName(file.FileName) + email;

            if (ModelState.IsValid)
            {
                if (email != null)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        if (fileExt.ToLower().EndsWith(".pdf"))
                        {
                            Models.File newFile = new Models.File
                            {
                                FileName = fname,
                                FileType = FileType.Pdf,
                                HelpTickets = ticket,
                                ticketId = ticket.TicketId
                            };
                            using (var reader = new System.IO.BinaryReader(file.InputStream))
                            {
                                newFile.Content = reader.ReadBytes(file.ContentLength);
                            }
                            db.Files.Add(newFile);
                        }
                        else if (fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".gif"))
                        {
                            Models.File newFile1 = new Models.File
                            {
                                FileName = fname,
                                FileType = FileType.png,
                                HelpTickets = ticket,
                                ticketId = ticket.TicketId
                            };
                            using (var reader = new System.IO.BinaryReader(file.InputStream))
                            {
                                newFile1.Content = reader.ReadBytes(file.ContentLength);
                            }
                            db.Files.Add(newFile1);
                        }
                    }
                    db.SaveChanges();
                    TempData["Message"] = "File Uploaded";
                    return View();
                }
            }
                TempData["Error"] = "Failed to upload";
            return View(file);

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
