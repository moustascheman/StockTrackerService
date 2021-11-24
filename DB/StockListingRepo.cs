using Microsoft.EntityFrameworkCore;
using StockTrackerService.Models;

namespace StockTrackerService.Data
{
    public class StockListingRepo : IStockListingRepo
    {
        private readonly StockTrackerContext _context;

        public StockListingRepo(StockTrackerContext db)
        {
            _context = db;
        }
        
        public StockListing getStockListingBySymbol(string Sym){
            //temporary value
            //PLEASE REPLACE WITH ACTUAL IMPLEMENTATION WHEN DB CONNECTED
            _context.testQuery();

            StockListing listing = new StockListing();
            listing.Code = "IBM";
            listing.Price = 12.40f;
            listing.Timestamp = 124;
            return listing;
        }

    }

}