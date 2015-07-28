using System.Collections.Generic;
using System.Linq;

namespace PortfolioSite.Models
{
    public class InventoryDetail
    {
        public InventoryDetail(int InventoryID, int StockID, int QuantityAvailable)
        {
            InventoryItemID = InventoryID;
            StockItemID = StockID;
            Qty = QuantityAvailable;
            Sizes = new List<string>();
            SizeTypes = new List<string>();
        }

        public int InventoryItemID;
        public int StockItemID;
        public List<string> Sizes;
        public List<string> SizeTypes;
        public int Qty;

        public long getInventoryItemID()
        {
            return InventoryItemID;
        }

        public void AddSize(string newSize, string newSizeType)
        {
            Sizes.Add(newSize);
            SizeTypes.Add(newSizeType);
        }

        // Every Item with a size contains an IEnumerable<string[]> with count of 1 or more
        // holding array of length 2. Item types with no size (etc. hats) return zero
        public IEnumerable<string[]> GetSizes()
        {
            var returnVals = Sizes.Zip(SizeTypes, (value, size) =>
                new string[]
                {
                    size,
                    value
                }
            );
            return returnVals;
        }
    }
}