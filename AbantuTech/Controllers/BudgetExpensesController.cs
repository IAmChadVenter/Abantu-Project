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
    public class BudgetExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetExpenses
        //public ActionResult Index()
        //{
        //    var budgetExpenses = db.BudgetExpenses.Include(b => b.Budget);
        //    return View(budgetExpenses.ToList());
        //}

        // GET: BudgetExpens
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            //var budgetExpenses = db.BudgetExpenses.Include(b => b.Budget);
            //var budgetExpenses = db.BudgetExpenses.OrderBy(b => b.Budget_ID);
            //return View(budgetExpenses.ToList());
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.BudgetExpenses
                     select s;
            we = db.BudgetExpenses.OrderBy(b => b.Budget_ID);

            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.ExpenseName.Contains(searchString));
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

        // GET: BudgetExpenses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetExpenses budgetExpenses = db.BudgetExpenses.Find(id);
            if (budgetExpenses == null)
            {
                return HttpNotFound();
            }
            return View(budgetExpenses);
        }

        // GET: BudgetExpenses/Create
        public ActionResult Create()
        {
            ViewBag.Budget_ID = new SelectList(db.Budgets, "Budget_ID", "Name");
            return View();
        }

        // POST: BudgetExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseName,Amount,Budget_ID")] BudgetExpenses budgetExpenses)
        {
            if (ModelState.IsValid)
            {
                db.BudgetExpenses.Add(budgetExpenses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Budget_ID = new SelectList(db.Budgets, "Budget_ID", "Name", budgetExpenses.Budget_ID);
            return View(budgetExpenses);
        }

        // GET: BudgetExpenses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetExpenses budgetExpenses = db.BudgetExpenses.Find(id);
            if (budgetExpenses == null)
            {
                return HttpNotFound();
            }
            ViewBag.Budget_ID = new SelectList(db.Budgets, "Budget_ID", "Name", budgetExpenses.Budget_ID);
            return View(budgetExpenses);
        }

        // POST: BudgetExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseName,Amount,Budget_ID")] BudgetExpenses budgetExpenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetExpenses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Budget_ID = new SelectList(db.Budgets, "Budget_ID", "Name", budgetExpenses.Budget_ID);
            return View(budgetExpenses);
        }

        // GET: BudgetExpenses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetExpenses budgetExpenses = db.BudgetExpenses.Find(id);
            if (budgetExpenses == null)
            {
                return HttpNotFound();
            }
            return View(budgetExpenses);
        }

        // POST: BudgetExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BudgetExpenses budgetExpenses = db.BudgetExpenses.Find(id);
            db.BudgetExpenses.Remove(budgetExpenses);
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
