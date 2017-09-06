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

namespace AbantuTech.Controllers
{
    [Authorize]
    public class ProgrammesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programmes
        public ActionResult Index()
        {
            var programmes = db.Programmes.Include(p => p.Committee);
            return View(programmes.ToList());
        }
        [HttpGet]
        public ActionResult ProgrammeMembers(int id)
        {
            var programme = db.Programmes
                .FirstOrDefault(p => p.Programme_ID == id);
            if(programme != null)
            {
                var programmeMembers = db.ProgrammeMembers
                    .Include(p => p.Member)
                    .Where(x => x.Programme_ID == programme.Programme_ID)
                    .ToList();
                return View(programmeMembers);
            }
            return View();
        }
        // GET: Programmes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // GET: Programmes/Create
        public ActionResult Create()
        {
            //ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name");
            return View();
        }

        // POST: Programmes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Programme_ID,Name,Description,Committee_ID")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                db.Programmes.Add(programme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name", programme.Committee_ID);
            return View(programme);
        }

        // GET: Programmes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name", programme.Committee_ID);
            return View(programme);
        }

        // POST: Programmes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Programme_ID,Name,Description,Committee_ID")] Programme programme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name", programme.Committee_ID);
            return View(programme);
        }

        // GET: Programmes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programme programme = db.Programmes.Find(id);
            if (programme == null)
            {
                return HttpNotFound();
            }
            return View(programme);
        }

        // POST: Programmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Programme programme = db.Programmes.Find(id);
            db.Programmes.Remove(programme);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ADD YOURSELF TO PROGRAMME
        [HttpPost]
        public JsonResult AddToProgramme(int id)
        {
            var email = HttpContext.User.Identity.Name;
            var programme = db.Programmes
                .FirstOrDefault(x => x.Programme_ID == id);
            var member = db.Members
                .FirstOrDefault(x => x.Email == email);
            if(programme != null && member != null)
            {
                var userprogramme = db.ProgrammeMembers
                    .FirstOrDefault(p => p.Member_ID == member.Member_ID 
                    && p.Programme_ID == programme.Programme_ID);
                if(userprogramme != null)
                {
                    return Json(new { message = "You already in" });
                }
                else
                {
                    var memberprograme = new ProgrammeMember
                    {
                        Member_ID = member.Member_ID,
                        Member = member,
                        Programme_ID = programme.Programme_ID,
                        Programme = programme
                    };
                    db.ProgrammeMembers.Add(memberprograme);
                    db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpDelete]
        public JsonResult RemoveToProgramme(int id)
        {
            var email = HttpContext.User.Identity.Name;
            var programme = db.Programmes
                .FirstOrDefault(x => x.Programme_ID == id);
            var member = db.Members
                .FirstOrDefault(x => x.Email == email);
            if (programme != null && member != null)
            {
                var userprogramme = db.ProgrammeMembers
                    .FirstOrDefault(p => p.Member_ID == member.Member_ID
                    && p.Programme_ID == programme.Programme_ID);
                if (userprogramme == null)
                {
                    return Json(new { message = "Not in programme" });
                }
                else
                {
                    db.ProgrammeMembers.Remove(userprogramme);
                    db.SaveChanges();
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
