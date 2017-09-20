using System.Linq;
using System.Web.Mvc;
using AbantuTech.Models;
using AbantuTech.ViewModel;

namespace AbantuTech.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /Store/AddToCart/5

        public ActionResult AddToCart(int id)
        {

            // Retrieve the album from the database
            var addedAlbum = storeDB.Albums
                .Single(album => album.AlbumId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedAlbum);

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string albumName = storeDB.Carts
                .Single(item => item.RecordId == id).Album.Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();

            return PartialView("CartSummary");
        }


        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                // Get the cart 
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Get the name of the album to display confirmation 
                string albumName = storeDB.Carts
                    .Single(item => item.RecordId == id).Album.Title;

                // Update the cart count 
                int itemCount = cart.UpdateCartCount(id, cartCount);

                //Prepare messages
                string msg = "The quantity of " + Server.HtmlEncode(albumName) +
                        " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(albumName) +
                        " has been removed from your shopping cart.";
                //
                // Display the confirmation message 
                results = new ShoppingCartRemoveViewModel
                {
                    Message = msg,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
            }
            catch
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input...",
                    CartTotal = -1,
                    CartCount = -1,
                    ItemCount = -1,
                    DeleteId = id
                };
            }
            return Json(results);
        }




    }
}