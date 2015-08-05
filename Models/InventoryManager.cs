using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioSite.Models
{
    public class InventoryManager
    {
        ClothingDataEntities db = new ClothingDataEntities();

        public int CheckInventory(int inventoryId)
        {
            Inventory item = db.Inventories.Single(i => i.InventoryID == inventoryId);
            return item.QuantityInStock;
        }

        public bool ReduceInventory(Order order)
        {
            order.AllItemsRetreived = true;
            foreach (OrderDetail orderDetail in order.OrderDetails)
            {
                int inventoryID = orderDetail.InventoryId;
                int count = orderDetail.Quantity;

                Inventory inventoryItem = db.Inventories.Single(i => i.InventoryID == inventoryID);

                if (inventoryItem.QuantityInStock >= orderDetail.Quantity)
                {
                    inventoryItem.QuantityInStock = inventoryItem.QuantityInStock - orderDetail.Quantity;
                    orderDetail.RetreivedFromInventory = true;
                }
                else
                {
                    orderDetail.QuantityShort = orderDetail.Quantity - inventoryItem.QuantityInStock;
                    order.AllItemsRetreived = false;
                    orderDetail.RetreivedFromInventory = false;
                }
            }

            if (order.AllItemsRetreived)
                db.SaveChanges();
            return order.AllItemsRetreived;
        }

        public bool CheckInventory(Order order)
        {
            order.AllItemsAvailable = true;
            foreach (OrderDetail orderDetail in order.OrderDetails)
            {
                int inventoryID = orderDetail.InventoryId;
                int count = orderDetail.Quantity;

                Inventory inventoryItem = db.Inventories.Single(i => i.InventoryID == inventoryID);

                if (inventoryItem.QuantityInStock < orderDetail.Quantity)
                {
                    orderDetail.QuantityShort = orderDetail.Quantity - inventoryItem.QuantityInStock;
                    order.AllItemsAvailable = false;
                }
                else
                    orderDetail.QuantityShort = 0;
            }
            return order.AllItemsAvailable;
        }
    }
}
