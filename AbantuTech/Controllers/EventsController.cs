using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AbantuTech.Models;
using Abantu_System.Models;
using System.Net.Mail;
using SelectPdf;
using System.IO;

namespace AbantuTech.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            var events = db.Events.Include(p => p.programme);
            return View(events.OrderBy(b => b.start_date));
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Event_ID,text,start_date,end_date,Name,Venue,Programme_ID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();

                foreach (var item in db.Members)
                {
                    try
                    {
                        // create email message
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("abantusystem@gmail.com");
                        message.To.Add(new MailAddress(item.Email));
                        message.Subject = "Event " + @event.Name + " has been Created";
                        message.Body = "You are part of a programme that has a new event " + @event.Name + " Event Details: " + @event.text + " Start Date: " + @event.start_date + " Finish Date: " + @event.end_date + " The Venue is: " + @event.Venue;

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
                    }
                }

                return RedirectToAction("Index");
            }



            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", @event.Programme_ID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            foreach (var item in db.Members)
            {
                try
                {
                    // create email message
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("abantusystem@gmail.com");
                    message.To.Add(new MailAddress(item.Email));
                    message.Subject = "Event " + @event.Name + " has been changed";
                    message.Body = "Your event " + @event.Name + " has been changed. The new Details are, Start Date: " + @event.start_date + " Finish Date: " + @event.end_date + " The Venue is: " + @event.Venue + " Event Details " + @event.text;

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
                }
            }

            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", @event.Programme_ID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Event_ID,text,start_date,end_date,Name,Venue,Programme_ID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", @event.Programme_ID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            foreach (var item in db.Members)
            {
                try
                {
                    // create email message
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("abantusystem@gmail.com");
                    message.To.Add(new MailAddress(item.Email));
                    message.Subject = "Event " + @event.Name + " has been Cancelled";
                    message.Body = "Sorry to inform you that the event " + @event.Name + " has been cancelled.";

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
                }
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult AttendEvent(int id)
        {

            var email = HttpContext.User.Identity.Name;// HttpContext.User.Identity.Name for authorized users
            var @event = db.Events//to get events from database
                .FirstOrDefault(x => x.Event_ID == id);//If the id of event matches the one delclared above
            var member = db.Members//To have mambers from database
                .FirstOrDefault(x => x.Email == email);//Those who received the login credentials
            if (@event != null && member != null)//if there is the event, and a member is a valid member
            {
                var userevent = db.EventMembers//and checking if ther's a member in this class  
                    .FirstOrDefault(p => p.Member_ID == member.Member_ID//if the member id matches the one delclared above
                    && p.Event_ID == @event.Event_ID);//and event id matches the one delclared above
                if (userevent != null) //and checks if there is a member in the event database
                {
                    return Json(new { message = "You already in" });
                }
                else
                {
                    //if ther's no member in the event table, then it is where he's added to the event database
                    var eventMembers = new EventMembers
                    {
                        Event_ID = @event.Event_ID,
                        Member_ID = member.Member_ID,
                        AbantuMember = member,
                        Event = @event

                    };
                    db.EventMembers.Add(eventMembers);
                    db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EventAttendance(int id)
        {
            var @event = db.Events
               .FirstOrDefault(p => p.Event_ID == id);
            if (@event != null)
            {
                var eventMember = db.EventMembers
                    .Include(p => p.AbantuMember)
                    .Where(x => x.Event_ID == @event.Event_ID)
                    .ToList();
                return View(eventMember);
            }
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

