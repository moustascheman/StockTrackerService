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



	}
}