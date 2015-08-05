using PortfolioSite.DataContexts;
using PortfolioSite.Helpers;
using PortfolioSite.Models;
using PortfolioSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PortfolioSite.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : Controller
    {
        ClothingDataEntities mainDb = new ClothingDataEntities();
        WebStoreDataEntities webStoreDb = new WebStoreDataEntities();

        public ActionResult ClearCart()
        {
            var shoppingCart = ShoppingCart.GetCart(this.HttpContext);
            shoppingCart.EmptyCart();
            return RedirectToAction("Index", "ShoppingCart");
        }

        public ActionResult Index()
        {
            var shoppingCart = ShoppingCart.GetCart(this.HttpContext);
            string cartId = shoppingCart.GetCartId(this.HttpContext);

            var viewModel = new ShoppingCartViewModel();
            var carts = webStoreDb.Carts.Where(c => c.CartId == cartId);
            List<CartDetail> details = new List<CartDetail>();
            decimal cartTotal = 0;
            //bool insufficientInventory = false;
            var alertMessages = new List<string>();
            var displayImage = new DisplayImage(DisplayImage.ImageCategory.Clothing);

            foreach (Cart cart in carts)
            {
                Item item = mainDb.Items.Single(i => i.ItemID == cart.ItemId);
                Inventory inventory = mainDb.Inventories.Single(i => i.InventoryID == cart.InventoryId);
                InventoryDetailGenerator inventoryDetailGenerator = new InventoryDetailGenerator();
                List<InventoryDetail> inventoryDetails = inventoryDetailGenerator.GetInventoryDetails(inventory);
                decimal cartDetailTotal = cart.Price * cart.Count;
                bool insufficientInventoryItem = false;
                if (inventory.QuantityInStock < cart.Count)
                {
                    insufficientInventoryItem = true;
                    //insufficientInventory = true;
                    alertMessages.Add("Choose new quantity for " + item.Name + ".");
                }
                details.Add(new CartDetail
                    {
                        RecordId = cart.RecordId,
                        InventoryId = cart.InventoryId,
                        ItemId = cart.ItemId,
                        SubTotal = cart.Price,
                        Quantity = cart.Count,
                        QuantityInStock = inventory.QuantityInStock,
                        Name = item.Name,
                        Size = inventoryDetailGenerator.GetInventoryDetailString(inventoryDetails, false),
                        Picture = displayImage.GetPath(item, true),
                        Total = cartDetailTotal,
                        InventoryFlag = insufficientInventoryItem
                    }
                );
                cartTotal += cartDetailTotal;
            }
            viewModel.Details = details;
            viewModel.CartId = cartId;
            viewModel.Total = cartTotal;
            viewModel.NumItems = shoppingCart.GetCount();
            ViewBag.AlertMessages = alertMessages;

            return View(viewModel);
        }

        //
        // POST: /ShoppingCart/AddToCart/
        [HttpPost]
        public ActionResult AddToCart(int? id)
        {
            if (!id.HasValue)
                throw new ArgumentNullException("GET argument not found.");
            int itemId = (int)id;
            Item item = mainDb.Items.Single(i => i.ItemID == id);
            string strInventoryId = Request.Form.Get("InventoryId");
            int inventoryId;
            if (!Int32.TryParse(strInventoryId, out inventoryId))
                throw new ArgumentException("Invalid Inventory ID.");

            // Add it to the shopping cart
            var shoppingCart = ShoppingCart.GetCart(this.HttpContext);
            shoppingCart.AddToCart(inventoryId, item.ItemID, item.Price);

            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the item to display confirmation
            int itemId = webStoreDb.Carts
                .Single(item => item.RecordId == id).ItemId;
            string itemName = mainDb.Items.Single(i => i.ItemID == itemId).Name;
            decimal itemPrice = mainDb.Items.Single(p => p.ItemID == itemId).Price;

            // Remove from cart
            cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartUpdateViewModel
            {
                Success = true,
                Message = itemName +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                UpdateId = id
            };

            return Json(results);
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult ChangeQuantity(int id, int qty)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the item to display confirmation
            int itemId = webStoreDb.Carts
                .Single(item => item.RecordId == id).ItemId;
            string itemName = mainDb.Items.Single(i => i.ItemID == itemId).Name;
            decimal itemPrice = mainDb.Items.Single(p => p.ItemID == itemId).Price;

            // Find out if new quantity is available
            string cartId = cart.GetCartId(this.HttpContext);
            var cartItem = webStoreDb.Carts.Single(c => c.CartId == cartId && c.RecordId == id);
            int inventoryID = cartItem.InventoryId;
            Inventory inventory = mainDb.Inventories.Single(i => i.InventoryID == inventoryID);
            int itemAvailableQty = inventory.QuantityInStock;

            var results = new ShoppingCartUpdateViewModel();
            if (itemAvailableQty >= qty)
            {
                results.Success = true;
                cart.ChangeCartQty(id, qty);

                results.Message = itemName +
                    " has been changed to " + qty + " unit";
                if (qty == 1)
                    results.Message += ".";
                else
                    results.Message += "s.";
                results.CartTotal = cart.GetTotal();
                results.CartCount = cart.GetCount();
                results.ItemCount = qty;
                results.ItemTotal = itemPrice * qty;
                results.UpdateId = id;
            }
            else
            {
                results.Success = false;
                results.Message = "There are only " + itemAvailableQty + " of " + Server.HtmlEncode(itemName) +
                    " available in that size.";
            }

            return Json(results);
        }

        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            int cartCount = cart.GetCount();

            if (cartCount > 0)
            {
                ViewData["CartCount"] = cartCount;
                return PartialView("CartSummary");
            }
            else
                return null;
        }

        protected override void Dispose(bool disposing)
        {
            mainDb.Dispose();
            webStoreDb.Dispose();
            base.Dispose(disposing);
        }
    }
}