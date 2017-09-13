using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AbantuTech.Models;
using System.Net.Mail;
using Abantu_System.Models;

namespace AbantuTech.Controllers
{
    public class NewslettersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Newsletters
        public ActionResult Index()
        {
            return View(db.Newsletters.ToList());
        }

        // GET: Newsletters/Details/5
        public ActionResult subed(int? id)
        {
            return View();
        }

        // GET: Newsletters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Newsletters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Email_ID,Email")] Newsletter newsletter)
        {
            if (ModelState.IsValid)
            {
                db.Newsletters.Add(newsletter);
                db.SaveChanges();
                return RedirectToAction("subed");
            }

            return View(newsletter);
        }

        // GET: Newsletters/Edit/5
        public ActionResult UnSubed(int? id)
        {
            return View();
        }
        public ActionResult UnSubedNull(string id)
        {
             return View();
        }
        [HttpPost, ActionName("UnsubedNull")]
        [ValidateAntiForgeryToken]
        public ActionResult UnsubedNullre(string id)
        {
            Newsletter newsletter = db.Newsletters.FirstOrDefault(p => p.Email == id);
            if (newsletter == null)
            {
                return RedirectToAction("UnSubedNull");
            }


            if (newsletter != null)
            {
                db.Newsletters.Remove(newsletter);
                db.SaveChanges();
                return RedirectToAction("UnSubed");
            }
            else return View();
        }

        // GET: Newsletters/Delete/5
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Newsletters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Newsletter newsletter = db.Newsletters.FirstOrDefault(p => p.Email == id);
            if (newsletter == null)
                {
                return RedirectToAction("UnSubedNull");
                }


            if (newsletter != null)
            {
                db.Newsletters.Remove(newsletter);
                db.SaveChanges();
                return RedirectToAction("UnSubed");
            }
            else return View();
        
        }
        public ActionResult News()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult News(string subj, string body)
        {
            if (ModelState.IsValid)
            {
                    foreach (var item in db.Newsletters)
                    {
                        try
                        {
                            // create email message
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress("abantusystem@gmail.com");
                            message.To.Add(new MailAddress(item.Email));
                            message.Subject = subj;
                            message.IsBodyHtml = true;
                            message.Body =body;

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
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                
                return RedirectToAction("sentConfirm");
            }
            return View();
        }


        public ActionResult sentConfirm()
        {
            return View();
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
