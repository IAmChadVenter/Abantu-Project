using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AbantuTech.Models;

namespace AbantuTech.Controllers
{
    public class HelpCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HelpCategories
        public ActionResult Index()
        {
            return View(db.HelpCategories.ToList());
        }

        // GET: HelpCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpCategory helpCategory = db.HelpCategories.Find(id);
            if (helpCategory == null)
            {
                return HttpNotFound();
            }
            return View(helpCategory);
        }

        // GET: HelpCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HelpCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult Create([Bind(Include = "cID,cName")] HelpCategory helpCategory)
        {
            if (ModelState.IsValid)
            {
                db.HelpCategories.Add(helpCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(helpCategory);
        }

        // GET: HelpCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpCategory helpCategory = db.HelpCategories.Find(id);
            if (helpCategory == null)
            {
                return HttpNotFound();
            }
            return View(helpCategory);
        }

        // POST: HelpCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cID,cName")] HelpCategory helpCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(helpCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(helpCategory);
        }

        // GET: HelpCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HelpCategory helpCategory = db.HelpCategories.Find(id);
            if (helpCategory == null)
            {
                return HttpNotFound();
            }
            return View(helpCategory);
        }

        // POST: HelpCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HelpCategory helpCategory = db.HelpCategories.Find(id);
            db.HelpCategories.Remove(helpCategory);
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
