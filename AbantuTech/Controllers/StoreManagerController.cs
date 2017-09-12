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
using SelectPdf;
using System.Net.Mail;
using System.IO;
using System.Data.Entity.Infrastructure;

namespace AbantuTech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoreManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /StoreManager/
        
        public ViewResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            var albums = db.Albums.Include(a => a.Genre).Include(a => a.Artist);
            ViewBag.CurrentSort = sortOrder;
            var we = from s in albums
                     select s;
            we = db.Albums.OrderBy(b => b.Genre.Name);
            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.Title.Contains(searchString));
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

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(we.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /StoreManager/Details/5

        public ViewResult Details(int id)
        {
            Album album = db.Albums.Find(id);
            return View(album);
        }

        //
        // GET: /StoreManager/Create

        public ActionResult Create()
        {
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Create(Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Edit/5

        public ActionResult Edit(int id)
        {
            Album album = db.Albums.Find(id);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // POST: /StoreManager/Edit/5

        [HttpPost]
        public ActionResult Edit(Album album, int id)
        {
            if (ModelState.IsValid)
            {
                Album album1 = db.Albums.Find(id);
                db.Albums.Remove(album1);
                db.SaveChanges();

                db.Albums.Add(album);
                db.SaveChanges();



                return RedirectToAction("Index");
            }
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            return View(album);
        }

        //
        // GET: /StoreManager/Delete/5

        public ActionResult Delete(int id)
        {
            Album album = db.Albums.Find(id);
            return View(album);
        }

        //
        // POST: /StoreManager/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Artist()
        {
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult Artist(Artist art)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(art);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(art);
        }
        public ActionResult category()
        {
            return View();
        }

        //
        // POST: /StoreManager/Create

        [HttpPost]
        public ActionResult category(Genre alb)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(alb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alb);
        }
        

        public ActionResult dets(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            var we = from s in db.OrderDetails.Include(p => p.Order)
                     select s;

            we = db.OrderDetails.OrderBy(b => b.Order.OrderDate);

            if (!String.IsNullOrEmpty(searchString))
            {
                we = we.Where(s => s.Order.FirstName.Contains(searchString));
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

            int pageSize = 100;
            //var oder = db.OrderDetails.Include(p => p.Order);
            int pageNumber = (page ?? 1);
            return View(we.ToPagedList(pageNumber, pageSize));
            //return View(oder.ToList());
        }

        public ActionResult detsEdit(int id)
        {
            OrderDetail op = db.OrderDetails.Find(id);
            return View(op);
        }
        [HttpPost]
        public ActionResult detsEdit([Bind(Include = "OrderId, AlbumId, Quantity, UnitPrice, isSent")] OrderDetail or, int id)
        {
            if (ModelState.IsValid)
            {
                OrderDetail album = db.OrderDetails.Find(id);
                db.OrderDetails.Remove(album);
                db.SaveChanges();

                db.OrderDetails.Add(or);
                db.SaveChanges();
                return RedirectToAction("dets");
            }
            return View(or);
        }




    }
}