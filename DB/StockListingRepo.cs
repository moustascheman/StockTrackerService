using StockTrackerService.Models;

namespace StockTrackerService.Data
{
    public class StockListingRepo : IStockListingRepo
    {
        
        public StockListing getStockListingBySymbol(string Sym){
            //temporary value
            //PLEASE REPLACE WITH ACTUAL IMPLEMENTATION WHEN DB CONNECTED
            StockListing listing = new()
            {
                Symbol = Sym,
                listingPrice = 012.43M,
                listingDate = System.DateTime.Now
            };
            return listing;
        }

    }

}