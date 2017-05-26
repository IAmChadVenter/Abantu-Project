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
using PagedList;

namespace AbantuTech.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        //public ActionResult Index()
        //{
        //    var budgets = db.Budgets.Include(b => b.Event).Include(b => b.Programme);
        //    return View(budgets.ToList());
        //}
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            //var budgetExpenses = db.BudgetExpenses.Include(b => b.Budget);
            //var budgetExpenses = db.BudgetExpenses.OrderBy(b => b.Budget_ID);
            //return View(budgetExpenses.ToList());
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.Budgets
                     select s;
            we = db.Budgets.OrderBy(b => b.Name);

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
            //return View(we.ToList());
        }
        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.Event_ID = new SelectList(db.Events, "Event_ID", "Name");
            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Budget_ID,Name,Amount,Event_ID,Programme_ID")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", budget.Programme_ID);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", budget.Programme_ID);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Budget_ID,Name,Amount,Event_ID,Programme_ID")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Programme_ID = new SelectList(db.Programmes, "Programme_ID", "Name", budget.Programme_ID);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
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
