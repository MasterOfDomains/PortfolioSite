using PortfolioSite.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioSite.Models
{
    public partial class ShoppingCart
    {
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";
        WebStoreDataEntities storeDb = new WebStoreDataEntities();

        public void EmptyCart()
        {
            var cartItems = storeDb.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                storeDb.Carts.Remove(cartItem);
            }
            storeDb.SaveChanges();
        }

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }


        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        
        public void AddToCart(int inventoryId, int itemId, decimal price)
        {
            // Get the matching cart and album instances

            var cartItem = storeDb.Carts.SingleOrDefault(
                            c => c.CartId == ShoppingCartId &&
                            c.InventoryId == inventoryId
                        );

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    InventoryId = inventoryId,
                    CartId = ShoppingCartId,
                    ItemId = itemId,
                    Price = price,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                storeDb.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }

            // Save changes
            storeDb.SaveChanges();
        }

        public void RemoveFromCart(int id)
        {
            var cartItem = storeDb.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);

            if (cartItem != null)
            {
                storeDb.Carts.Remove(cartItem);
                storeDb.SaveChanges();
            }
        }

        public void ChangeCartQty(int id, int qty)
        {
            var cartItem = storeDb.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.RecordId == id);

            if (cartItem != null)
            {
                cartItem.Count = qty;
                storeDb.SaveChanges();
            }
        }

        public List<Cart> GetCartItems()
        {
            return storeDb.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {

            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in storeDb.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in storeDb.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Price).Sum();
                              //select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            var cartItems = GetCartItems();

            order.OrderDetails = new List<OrderDetail>();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    RecordId = item.RecordId,
                    InventoryId = item.InventoryId,
                    ItemId = item.ItemId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Price,
                    Quantity = item.Count
                };

                order.OrderDetails.Add(orderDetail);

                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Price);
            }

            order.Total = orderTotal;
            storeDb.SaveChanges();
            //EmptyCart();
            return order.OrderId;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = storeDb.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            storeDb.SaveChanges();
        }
    }
}