using System.Collections.Generic;
using System.Linq;

namespace PortfolioSite.Models
{
    public class InventoryDetailGenerator
    {
        public List<InventoryDetail> GetInventoryDetails(Inventory inventoryItem)
        {
            var stockEnum = inventoryItem.Stocks.Where(s => s.InventoryID == inventoryItem.InventoryID)
                .OrderBy(s => s.Size.DisplayOrder);
            List<InventoryDetail> inventoryDetails = GetInventoryDetailsHelper(stockEnum);
            return inventoryDetails;
        }

        public List<InventoryDetail> GetInventoryDetails(Item saleItem)
        {
            var stockEnum = saleItem.Stocks.AsEnumerable().OrderBy(s => s.Size.DisplayOrder);
            List<InventoryDetail> InventoryDetails = GetInventoryDetailsHelper(stockEnum);
            return InventoryDetails;
        }

        private List<InventoryDetail> GetInventoryDetailsHelper(IEnumerable<Stock> stockEnum)
        {
            List<InventoryDetail> InventoryDetails = new List<InventoryDetail>();
            foreach (var StockRecord in stockEnum)
            {
                int inventoryID = StockRecord.InventoryID; // Groups multiple size types
                int stockID = StockRecord.StockID;
                int qtyAvailable = StockRecord.Inventory.QuantityInStock;
                string sizeType = StockRecord.Size.SizeType.Name;
                string sizeValue = StockRecord.Size.Value;

                bool NoElementFound = true;
                foreach (InventoryDetail CurrentItem in InventoryDetails)
                {
                    if (inventoryID == CurrentItem.getInventoryItemID())
                    {
                        CurrentItem.AddSize(sizeValue, sizeType);
                        NoElementFound = false;
                    }
                }
                if (NoElementFound && qtyAvailable > 0)
                {
                    InventoryDetail newItem = new InventoryDetail(inventoryID, stockID, qtyAvailable);
                    newItem.AddSize(sizeValue, sizeType);
                    InventoryDetails.Add(newItem);
                }
            }
            return InventoryDetails;
        }

        public string GetInventoryDetailString(List<InventoryDetail> InventoryDetails, bool initials)
        {
            int totalElements = InventoryDetails.Count();
            int elementsProcessed = 0;
            bool lastElement = false;
            bool nextToLastElement = false;
            string sizeString = "";
            foreach (InventoryDetail detail in InventoryDetails)
            {
                var sizes = detail.GetSizes();
                if (sizes.Count() > 1)
                {
                    foreach (string[] values in sizes)
                    {
                        if (initials)
                        {
                            string sizeTypeEnding = values[0].Remove(0, 1);
                            string stringInitial = values[0].Replace(sizeTypeEnding, "");
                            sizeString += " " + stringInitial + ": " + values[1];
                        }
                        else
                            sizeString += " " + values[0] + ": " + values[1];
                    }
                }
                else
                {
                    string[] value = sizes.ElementAt(0);
                    sizeString += " " + value[1];
                }

                if (elementsProcessed == (totalElements - 2))
                    nextToLastElement = true;
                if (totalElements > 1)
                {
                    if (!lastElement)
                    {
                        if (nextToLastElement)
                            sizeString += " ";
                        else
                            sizeString += ", ";
                    }
                    else
                        sizeString += ".";
                    elementsProcessed++;
                    if (elementsProcessed == (totalElements - 1))
                    {
                        lastElement = true;
                        sizeString += "and ";
                    }
                }
            }
            return sizeString;
        }
    }
}
