using PortfolioSite.DataContexts;
using PortfolioSite.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        WebStoreDataEntities storeDb = new WebStoreDataEntities();
        const string PromoCode = "FreeStuff";

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult BillingProfile()
        {
            try
            {
                Order mostRecentOrder = storeDb.Orders.Where(o => o.Username == User.Identity.Name)
                                                      .OrderByDescending(o => o.OrderDate).First();

                Order order = new Order();
                order.FirstName = mostRecentOrder.FirstName;
                order.LastName = mostRecentOrder.LastName;
                order.Address = mostRecentOrder.Address;
                order.City = mostRecentOrder.City;
                order.State = mostRecentOrder.State;
                order.PostalCode = mostRecentOrder.PostalCode;
                order.Country = mostRecentOrder.Country;
                order.Phone = mostRecentOrder.Phone;
                order.Email = mostRecentOrder.Email;

                return View(order);
            }
            catch
            {
                return View();
            }
        }

        //
        // POST: /Checkout/AddressAndPayment

        [HttpPost]
        public ActionResult BillingProfile(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"].Trim(), PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    ViewBag.PromotionCodeAlert = "Use the following promotion code to receive your order without paying: ";
                    ViewBag.PromoCodeValue = PromoCode;
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;
                    storeDb.Orders.Add(order);
                    storeDb.SaveChanges();
                    return RedirectToAction("Finalize", new { id = order.OrderId });
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

        public ActionResult Finalize(int id)
        {
            bool orderSuccess = false;
            Order order = storeDb.Orders.Single(o => o.OrderId == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.CreateOrder(order);
            var inventoryManager = new InventoryManager();

            if (inventoryManager.CheckInventory(order))
            {
                if (inventoryManager.ReduceInventory(order))
                {
                    //Process the order
                    orderSuccess = true;
                }
            }

            if (orderSuccess)
            {
                cart.EmptyCart();
                return RedirectToAction("Confirmation", new { id = order.OrderId });
            }
            else
                return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult Confirmation(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDb.Orders.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}