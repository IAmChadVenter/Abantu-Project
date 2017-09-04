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
using System.Collections;

namespace AbantuTech.Controllers
{
    public class TasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Committee);
            return View(tasks.ToList());
        }
        public ActionResult ViewTasks()
        {
            ApplicationDbContext pp = new ApplicationDbContext();
            List<Tasks> tastAssigned = pp.Tasks.ToList();
            return View(tastAssigned);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var notify = db.Notifications.FirstOrDefault(x => x.ID == id
                && x.userName == User.Identity.Name);
                if(notify != null)
                {
                    notify.seen = true;
                    db.SaveChanges();
                }
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,StartDate,EndDate,status,Committee_ID")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(tasks);
                db.SaveChanges();
                // create notifications
                var members = getMembers(tasks.Committee_ID);
                if (members != null)
                {
                    foreach (var m in members)
                    {
                        var notify = new Notification
                        {
                            userName = m.Email,
                            Task = tasks,
                            ID = tasks.ID,
                            message = "You have a new " + tasks.Name + " task",
                            seen = false
                        };
                        db.Notifications.Add(notify);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name", tasks.Committee_ID);
            return View(tasks);
        }

        // GET: Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name", tasks.Committee_ID);
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,StartDate,EndDate,status,Committee_ID")] Tasks tasks)
        {
            if (ModelState.IsValid)
            {

                db.Entry(tasks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name", tasks.Committee_ID);
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tasks tasks = db.Tasks.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tasks tasks = db.Tasks.Find(id);
            db.Tasks.Remove(tasks);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult userNotifications()
        {
            if (User.Identity.IsAuthenticated)
            {
                var notifs = db.Notifications
                    .Where(x => x.userName == User.Identity.Name
                    && x.seen == false);
                if(notifs != null)
                    return Json(notifs, JsonRequestBehavior.AllowGet);
                return Json(new { message = "You have not new notifications" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { message = "Log in to see your notifications" }, JsonRequestBehavior.AllowGet);
        }
        public IEnumerable<AbantuMember> getMembers(int committeId)
        {
            var members = db.Members
                .Where(x => x.Committee_ID == committeId);
            return members;
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
