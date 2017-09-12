using System;
using System.Linq;
using System.Web.Mvc;
using AbantuTech.Models;
using System.Net.Mail;
using System.Net;

namespace AbantuTech.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        ApplicationDbContext storeDB = new ApplicationDbContext();
        const string PromoCode = "";

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    var car = ShoppingCart.GetCart(this.HttpContext);
                    order.Total = car.GetTotal();

                    //Save Order
                    storeDB.Orders.Add(order);
                    storeDB.SaveChanges();

                    //Process the order
                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }

            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }

        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                Order @order = storeDB.Orders.Find(id);
                try
                {
                    // create email message
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("abantusystem@gmail.com");
                    
                    message.To.Add(new MailAddress(@order.Email));
                    message.Subject = "Abantu System - Budget Report";
                    message.IsBodyHtml = true;
                    message.Body = "Congratulations " + @order.FirstName + " " + @order.LastName + " on your purchase! <br /><br />" + 
                                    "Your Order Total is R " + @order.Total + " <br /><br />"
                                    + "Your details are as follows: <br/>"
                                    + "Email:  " + @order.Email + "<br/>"
                                    + "Country: " + @order.Country + "<br/>"
                                    + "Address: " + @order.Address + "<br />"
                                    + "City: " + @order.City + "<br />"
                                    + "State: " + @order.State + "<br />"
                                    + "Postal Code: " + @order.PostalCode + "<br /><br />"
                                    + "Please reply with proof of purchase! We will dispatch as soon as we recieve it!"+ "<br/>"
                                    + "Banking Details: " + "<br/>"
                                    + "Bank: ABSA " + "<br/>"
                                    + "Account Number: 458965211" + "<br/>"
                                    + "Branch: Durban, SA" + "<br/>"
                                    + "Branch Code 5536297: " + "<br/>";
                    
                    ViewData["Message"] = "Email sent";

                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "abantusystem@gmail.com",  // replace with valid value
                            Password = "Abantu2017"  // replace with valid value
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }

                }
                catch (Exception ex)
                {
                    ViewData["Message"] = "An error occurred: " + ex.Message;
                }
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
