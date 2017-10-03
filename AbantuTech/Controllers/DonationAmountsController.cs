//using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace AbantuTech.Models
{
    public class DonationAmountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DonationAmounts
        public ActionResult Index()
        {
            return View(db.DonationAmount.ToList());
        }

        // GET: DonationAmounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationAmount donationAmount = db.DonationAmount.Find(id);
            if (donationAmount == null)
            {
                return HttpNotFound();
            }
            return View(donationAmount);
        }

        // GET: DonationAmounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonationAmounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,C")] DonationAmount donationAmount)
        {
            if (ModelState.IsValid)
            {
                db.DonationAmount.Add(donationAmount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donationAmount);
        }

        // GET: DonationAmounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationAmount donationAmount = db.DonationAmount.Find(id);
            if (donationAmount == null)
            {
                return HttpNotFound();
            }
            return View(donationAmount);
        }

        // POST: DonationAmounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,C")] DonationAmount donationAmount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donationAmount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donationAmount);
        }

        // GET: DonationAmounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationAmount donationAmount = db.DonationAmount.Find(id);
            if (donationAmount == null)
            {
                return HttpNotFound();
            }
            return View(donationAmount);
        }

        // POST: DonationAmounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonationAmount donationAmount = db.DonationAmount.Find(id);
            db.DonationAmount.Remove(donationAmount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Report(int? id)
        {
            return View(db.DonationAmount.ToList());
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

