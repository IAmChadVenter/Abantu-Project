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
    [Authorize(Roles = "Admin")]
    public class FilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: File
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Files.ToList());
        }

        // GET: Files/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }
        [AllowAnonymous]
        // GET: Files/DownLoad/5
        public FileResult DownLoad(int? id)
        {
            // Get current File from database
            File file = db.Files.Find(id);
            string fileName = file.FileName;
            string fileType = file.FileType.ToString();
            byte[] contents = file.Content;
            return File(contents, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [AllowAnonymous]
        // GET: Files/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Files/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    File newFile = new File
                    {
                        FileName = System.IO.Path.GetFileName(file.FileName),
                        FileType = FileType.Pdf
                    };
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        newFile.Content = reader.ReadBytes(file.ContentLength);
                    }
                    db.Files.Add(newFile);
                }
                db.SaveChanges();
                TempData["Message"] = "File Uploaded";
                return View();
            }
            TempData["Error"] = "Failed to upload";
            return View(file);
        }

        // GET: Files/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TempData["FileID"] = id;
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    int FileID = Int32.Parse(TempData["FileID"].ToString());
                    File currentFile = db.Files.Find(FileID);
                    if (currentFile == null)
                    {
                        return HttpNotFound();
                    }
                    currentFile.FileName = file.FileName;
                    using (var reader = new System.IO.BinaryReader(file.InputStream))
                    {
                        currentFile.Content = reader.ReadBytes(file.ContentLength);
                    }
                    db.Entry(currentFile).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(file);
        }

        // GET: Files/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: Files/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
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
