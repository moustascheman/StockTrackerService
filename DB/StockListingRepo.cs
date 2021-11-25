using Microsoft.EntityFrameworkCore;
using StockTrackerService.Models;
using System.Linq;

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
            return _context.stocks.FromSqlRaw("SELECT * From stockcsv.stocks where Code = {0} AND Timestamp IN (SELECT MAX(Timestamp) from stockcsv.stocks where code = {0})", Sym).FirstOrDefault();
        }

    }

}