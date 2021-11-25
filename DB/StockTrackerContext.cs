using System;
using IBM.Data.Db2;
using Microsoft.EntityFrameworkCore;

namespace StockTrackerService.Models
{
	public class StockTrackerContext : DbContext
	{    

        public StockTrackerContext(DbContextOptions<StockTrackerContext> opt) : base(opt)
        {

        }

        public DbSet<StockListing> stocks { get; set; }


        public void testQuery()
        {
            string query = "SELECT * FROM stockcsv.stocks WHERE Code = 'a' AND timestamp = (SELECT MAX(timestamp) from stockcsv.stocks where Code='a')";
            bool existing = base.Database.CanConnect();
            Console.WriteLine(existing);
        }


	}
}