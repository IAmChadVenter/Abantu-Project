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
    [Authorize(Roles = "Admin")]
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Members
        public ActionResult Index()
        {
            var members = db.Members.Include(m => m.Branch).Include(m => m.Committee)
                .Where(m => m.isAccepted == true);
            return View(members);
        }
        [HttpPost]
        public ActionResult Index(string Name)
        {
            var members = db.Members.Include(m => m.Branch).Include(m => m.Committee)
                .Where(m => m.FirstName.Equals(Name));
            return View(members);

        }
        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbantuMember member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.Branch_ID = new SelectList(db.Branches, "Branch_ID", "Branch_Name");
            //ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Member_ID,Surname,FirstName,DateOfBirth,Gender,Email,PhoneNumber,Type,Branch_ID,Committee_ID")] AbantuMember member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Branch_ID = new SelectList(db.Branches, "Branch_ID", "Branch_Name", member.Branch_ID);
            //ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Name", member.Committee_ID);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbantuMember member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.Branch_ID = new SelectList(db.Branches, "Branch_ID", "Branch_Name", member.Branch_ID);
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name", member.Committee_ID);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Member_ID,Surname,FirstName,DateOfBirth,Gender,Email,PhoneNumber,Type,City,Province,ZipCode,Branch_ID,Committee_ID")] AbantuMember member)
        {
            if (ModelState.IsValid)
            {
                member.isAccepted = true;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Branch_ID = new SelectList(db.Branches, "Branch_ID", "Branch_Name", member.Branch_ID);
            ViewBag.Committee_ID = new SelectList(db.Committtes, "Committee_ID", "Committee_Name", member.Committee_ID);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AbantuMember member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AbantuMember member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
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
