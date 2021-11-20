using System;
namespace StockTrackerService.Models
{ 
	public class StockListing
	{
        public string Symbol { get; set; }
		public Decimal listingPrice { get; set; }
		public DateTime listingDate { get; set; }
	}
}
