using StockTrackerService.Models;

namespace StockTrackerService.Data
{
    public interface IStockListingRepo
    {
        StockListing getStockListingBySymbol(string sym);
           
    }
    
}