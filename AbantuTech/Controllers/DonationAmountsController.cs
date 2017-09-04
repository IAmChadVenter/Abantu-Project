using SelectPdf;
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
        [HttpPost]
        public ActionResult SubmitAction(FormCollection collection)
        {

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            try
            {
                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertUrl("http://localhost:49965/DonationAmounts/Report/1");

                // create memory stream to save PDF
                MemoryStream pdfStream = new MemoryStream();

                // save pdf document into a MemoryStream
                doc.Save(pdfStream);

                // reset stream position
                pdfStream.Position = 0;

                // create email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("abantusystem@gmail.com");
                message.To.Add(new MailAddress(collection["TxtEmail"]));
                message.Subject = "Abantu System - Budget Report";
                message.Body = "This email should have attached the PDF Budget Report ";
                message.Attachments.Add(new Attachment(pdfStream, "Document.pdf"));

                // close pdf document
                doc.Close();

                ViewData["Message"] = "Email sent";

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "DS3",  // replace with valid value
                        Password = "May2017"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "Mfd.dut.ac.za";
                    //smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                ViewData["Message"] = "An error occurred: " + ex.Message;
            }


            // read parameters from the webpage

            string url = collection["TxtUrl"];


            // instantiate a html to pdf converter object
            HtmlToPdf converter1 = new HtmlToPdf();

            // create a new pdf document converting an url
            PdfDocument doc1 = converter1.ConvertUrl("http://localhost:49965/DonationAmounts/Report/1");

            // save pdf document
            byte[] pdf = doc1.Save();

            // close pdf document
            doc1.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;
        }
    }
}

