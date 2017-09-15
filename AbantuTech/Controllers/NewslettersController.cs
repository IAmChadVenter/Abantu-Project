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
using System.IO;

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
                            message.Body = "<table width='100%' align='center'><tr><th><img src='http://www.africaymca.org/wp-content/uploads/2014/09/AAYMCA-logo.png' /></th></tr><tr><th style='font-family:Garamond; font-size:40px'>YMCA Newsletter</th></tr><tr><th>"+ subj + "</th></tr></table>" + "<br /><div style='font-size:20px; text-align:center'>" + body + "</div><br /><br />" + "<hr style='border: 0; height: 1px; background-image: linear-gradient(to right, rgba(0, 0, 0, 0), rgba(0, 0, 0, 0.75), rgba(0, 0, 0, 0)) ' /><table width='100%' align='center' valign='middle' style='background-color:white'><tr><th>Connect With Us:</th></tr><tr><th ><a href='https://twitter.com/'><img src='https://cdn3.iconfinder.com/data/icons/free-social-icons/67/twitter_circle_color-256.png' height='35' width='35'/></a>&nbsp;&nbsp;<a href='https://facebook.com/'><img src='http://www.old.ittf.com/media/16wttc/subscription/images/facebook.png' height='35' width='35' /></a>&nbsp;&nbsp;<a href='https://youtube.com/'><img src='https://cdn1.iconfinder.com/data/icons/logotypes/32/youtube-512.png' height='35' width='35' /></a>&nbsp;&nbsp;<a href='https://instagram.com/'><img src='https://cdn3.iconfinder.com/data/icons/free-social-icons/67/instagram_circle_color-512.png' height='35' width='35' /></a></th></tr></table>" + "<br>" + "<hr style='border: 0; height: 1px; background-image: linear-gradient(to right, rgba(0, 0, 0, 0), rgba(0, 0, 0, 0.75), rgba(0, 0, 0, 0)) ' /><table width='100%' align='center' valign='middle'><tr><th><a>Contact Us</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a>Unsubscribe</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a>Login</a><th></tr><tr><td align='center'>Mailing Address: Sobantu Technologies (Pty) Ltd, PO Box 194, Durban, 4000</td></tr></table>";

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
