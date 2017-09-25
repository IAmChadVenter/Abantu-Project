using AbantuTech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            return View(db.HelpTickets.Include(a => a.Category).ToList());
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult viewTicket(int id)
        {
            if (ModelState.IsValid)
            {
                TempData["Message"] = "Your helpdesk inquiry has been submitted, a response will be email to you shortly";
                var ticket = db.HelpTickets.Include(s => s.Category).FirstOrDefault(x => x.TicketId == id);
                if(ticket == null)
                {
                    return HttpNotFound();
                }
                return View(ticket);
            }
            return View();
        }
        public FileResult downloadTicketFile(HelpTicket ticket, string fileExt)
        {
                var filePath = "~/App_Data/Upload/helpticketfiles/" + ticket.tCreatedOn + "/" + ticket.tCreatedBy;
  
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            
                string fileName = "HelpDeskTicket_" + ticket.tCreatedBy + ".pdf";

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult replyToTicket(int id, string reply)
        {
            if (ModelState.IsValid)
            {
                var ticket = db.HelpTickets.Include(c => c.Category).FirstOrDefault(x => x.TicketId == id);
                if (ticket != null)
                {
                    ticket.AdminResponse = reply;
                    try
                    {
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("inquiries.abantu@gmail.com");
                        message.To.Add(new MailAddress(ticket.Members.Email));
                        message.Subject = "Help desk support for your inquiry";
                        message.Body = reply;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "abantusystem@gmail.com",  // replace with valid value
                                Password = "Abantu2017"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.Send(message);

                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    }
            }
            return View();
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
        public ActionResult Create(FileMessages? fmessages)
        {

            HelpTicket ticket = new HelpTicket();
            ViewBag.cID = new SelectList(db.HelpCategories, "cID", "cName");
            ViewBag.FileMessage = fmessages == FileMessages.FileUploadSuccess ? "File has successfully been included" :
                fmessages == FileMessages.FileUploadFailure ? "File has failed to be included" :
                fmessages == FileMessages.FileError ? "An error occurred while uploading file, try again later" :
                "";
            return View(ticket);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,Comment,tCreatedOn,tCreatedBy")] HelpTicket helpTicket, HttpPostedFileBase file, string comment)
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
                    helpTicket.tCreatedBy = member.Email;
                    db.HelpTickets.Add(helpTicket);
                    member.Tickets.Add(helpTicket);
                    comment = helpTicket.Comment;
                }
                db.SaveChanges();
                ViewBag.cID = new SelectList(db.HelpCategories, "cID", "cName");
                return RedirectToAction("viewTicket", new { helpTicket.TicketId, HelpMessages.TicketSuccess });
            }
            return RedirectToAction("Index", new { HelpMessages.Error });
        }
        [HttpGet]
        public ActionResult helpFileUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult helpFileUpload(HttpPostedFileBase file, HelpTicket ticket)
        {
            if (ModelState.IsValid)
            {
                if (ticket.tCreatedBy != null)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        if (fileExt.ToLower().EndsWith(".pdf"))
                        {
                            var filePath = HostingEnvironment.MapPath("~/App_Data/Upload/helpticketfiles/" + ticket.tCreatedOn.Date + "/" + ticket.tCreatedBy + ".pdf");
                            var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/Upload/helpticketfiles/" + ticket.tCreatedOn.Date));
                            if (directory.Exists == false)
                            {
                                directory.Create();
                            }
                            ViewBag.FilePath = filePath.ToString();
                            file.SaveAs(filePath);
                            return RedirectToAction("Create", new { FileMessages.FileUploadSuccess });
                        }
                        else if (fileExt.ToLower().EndsWith(".jpg") || fileExt.ToLower().EndsWith(".png") || fileExt.ToLower().EndsWith(".gif"))
                        {
                            var filePath = HostingEnvironment.MapPath("~/App_Data/Upload/helpticketfiles/" + ticket.tCreatedOn.Date + "/" + ticket.tCreatedBy + ".png");
                            var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Data/Upload/helpticketfiles/" + ticket.tCreatedOn.Date));
                            if (directory.Exists == false)
                            {
                                directory.Create();
                            }
                            ViewBag.FilePath = filePath.ToString();
                            file.SaveAs(filePath);
                            return RedirectToAction("Create", new { FileMessages.FileUploadSuccess });
                        }
                        else
                        {
                            return RedirectToAction("Create", new { FileMessages.FileUploadFailure });
                        }
                    }
                    
                }
            }
            return RedirectToAction("Create", new { FileMessages.FileError });
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
        public enum FileMessages
        {
            FileUploadSuccess,
            FileUploadFailure,
            FileError
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
