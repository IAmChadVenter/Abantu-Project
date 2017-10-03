using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AbantuTech.Models;
using AbantuTech.ViewModel;

namespace AbantuTech.Controllers
{
    public class StoreController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();

        //
        // GET: /Store/

        public ActionResult Index()
        {
            var genres = storeDB.Genres.ToList();

            return View(genres);
        }
        public ActionResult Collect()
        {
            return View();
        }
        public ActionResult collecEdit(int id)
        {
            OrderDetail op = storeDB.OrderDetails.Find(id);
            return View(op);
        }
        [HttpPost]
        public ActionResult collecEdit([Bind(Include = "OrderId, AlbumId, Quantity, UnitPrice, isSent, iscollected")] OrderDetail or, int id)
        {
            if (ModelState.IsValid)
            {
                OrderDetail album = storeDB.OrderDetails.Find(id);
                storeDB.OrderDetails.Remove(album);
                storeDB.SaveChanges();

                storeDB.OrderDetails.Add(or);
                storeDB.SaveChanges();
                return RedirectToAction("Collect");
            }
            return View(or);
        }
        //
        // GET: /Store/Browse?genre=Disco

        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            var genreModel = storeDB.Genres.Include("Albums")
                .Single(g => g.Name == genre);

            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(int id)
        {
            var album = storeDB.Albums.Find(id);

            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var genres = storeDB.Genres.ToList();

            return PartialView(genres);
        }

    }
}