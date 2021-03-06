﻿using System;
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
//using SelectPdf;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using AbantuTech.Helpers;
using PagedList;

namespace AbantuTech.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserManager<ApplicationUser> UserManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

        // GET: Events
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.Events
                     select s;
            we = db.Events.Include(b=> b.programme).OrderBy(b => b.Name);

            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.Name.Contains(searchString));
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(we.ToPagedList(pageNumber, pageSize));
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

                var progmember = db.ProgrammeMembers.Where(x => x.Programme_ID == @event.Programme_ID);
                if (progmember != null)
                {

                    foreach (var item in progmember)
                    {
                        try
                        {
                            // create email message
                            MailMessage message = new MailMessage();
                            message.From = new MailAddress("abantusystem@gmail.com");
                            message.To.Add(new MailAddress(item.email_));
                            message.Subject = "Event " + @event.Name + " has been Created";
                            message.Body = "A Programme you are a part of has a new event - " + @event.Name + " Event Details: " + @event.text + " Start Date: " + @event.start_date + " Finish Date: " + @event.end_date + " The Venue is: " + @event.Venue;

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

            var eventmember = db.EventMembers.Where(x => x.Event_ID == id).ToList();
            if (eventmember != null)
            {

                foreach (var item in eventmember)
                {
                    try
                    {
                        // create email message
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("abantusystem@gmail.com");
                        message.To.Add(new MailAddress(item.email_));
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
                        Console.WriteLine(ex.Message);
                    }
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

            var eventmember = db.EventMembers.Where(x => x.Event_ID == id).ToList();
            if (eventmember != null)
            {
                foreach (var item in eventmember)
                {
                    try
                    {
                        // create email message
                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("abantusystem@gmail.com");
                        message.To.Add(new MailAddress(item.email_));
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
                        Console.WriteLine(ex.Message);
                    }
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
                        email_ = member.Email,
                        Name = member.FirstName,
                        LName = member.Surname,
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
        [HttpGet]
        public ActionResult PastE(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.Events.Where(x => x.end_date <= DateTime.Today).ToList()
                     select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.Name.Contains(searchString));
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(we.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public async Task<ActionResult> photoGallery(int id, string filter = null, int page = 1, int pageSize = 20)
        {
            var @event = db.Events.FirstOrDefault(x => x.Event_ID == id);
            if (@event != null)
            {
                var records = new Models.PagedList<EventPhoto>();
                ViewBag.filter = filter;

                records.Content = db.Photos
                            .Where(x => filter == null || (x.Description.Contains(filter)) && x.eventid == @event.Event_ID)
                            .OrderByDescending(x => x.PhotoId)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                // Count
                records.TotalRecords = await db.Photos
                                .Where(x => filter == null || (x.Description.Contains(filter))).CountAsync();

                records.CurrentPage = page;
                records.PageSize = pageSize;

                return View(records);
            }
            return View();
        }
        [HttpGet]
        public ActionResult PhotoUpload()
        {
            var ephoto = new EventPhoto();
            return View(ephoto);
        }
        [HttpPost]
        public ActionResult PhotoUpload(EventPhoto photo, IEnumerable<HttpPostedFileBase> files, int id)
        {
            if (!ModelState.IsValid)
                return View(photo);
            if (files.Count() == 0 || files.FirstOrDefault() == null)
            {
                ViewBag.Error = "Please select images";
                return View(photo);
            }
            var evt = db.Events.FirstOrDefault(x => x.Event_ID == id);
            if (evt != null)
            {
                var model = new EventPhoto();
                var count = 0;
                foreach (var file in files)
                {
                    count++;
                    if (file.ContentLength == 0) continue;
                    model.Description = photo.Description;
                    model.eventid = evt.Event_ID;
                    var filename = evt.Name + " photo #" + count;
                    var extension = Path.GetExtension(file.FileName).ToLower();

                    using (var img = Image.FromStream(file.InputStream))
                    {
                        model.ThumbPath = String.Format("~/Content/eventphotos" + evt.Name + "/thumbs/{0}{1}", filename, extension);
                        model.ImagePath = String.Format("~/Content/eventphotos" + evt.Name + "/{0}{1}", filename, extension);

                        SaveToFolder(img, filename, extension, new Size(100, 100), model.ThumbPath);

                        SaveToFolder(img, filename, extension, new Size(600, 600), model.ImagePath);
                    }
                    model.CreatedOn = DateTime.Now;
                    db.Photos.Add(model);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("photoGallery", new { id = evt.Event_ID });
        }
        public Size NewImageSize(Size imageSize, Size newSize)
        {
            Size finalSize;
            double tempval;
            if (imageSize.Height > newSize.Height || imageSize.Width > newSize.Width)
            {
                if (imageSize.Height > imageSize.Width)
                    tempval = newSize.Height / (imageSize.Height * 1);
                else
                    tempval = newSize.Width / (imageSize.Width * 1);

                finalSize = new Size((int)(tempval * imageSize.Width), (int)(tempval * imageSize.Height));
            }
            else
                finalSize = imageSize;
            return finalSize;
        }
        private void SaveToFolder(Image img, string fileName, string extension, Size newSize, string pathToSave)
        {
            Size imgSize = NewImageSize(img.Size, newSize);
            using (Image newImg = new Bitmap(img, imgSize.Width, imgSize.Height))
            {
                newImg.Save(Server.MapPath(pathToSave), img.RawFormat);
            }
        }
        public JsonResult AttendanceConfirmed(int id)
        {
            var @event = db.Events
                .FirstOrDefault(x => x.Event_ID == id);
            var member = db.Members
                .ToList();
            if (@event != null && member != null)
            {
                foreach (var m in member)
                {
                    var attendee = db.EventMembers
                        .FirstOrDefault(p => p.Member_ID == m.Member_ID
                        && p.Event_ID == @event.Event_ID && p.attendanceConfirmed == true);
                    if (attendee != null)
                    {
                        return Json(new { message = "Already checked in" });
                    }
                    else
                    {
                        attendee.attendanceConfirmed = true;
                        attendee.arrivalTime = DateTime.UtcNow;
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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

