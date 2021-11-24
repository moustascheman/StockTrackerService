using System;
namespace StockTrackerService.Models
{ 
	public class StockListing
	{
        public string Code { get; set; }
		public float Price { get; set; }
		public int Timestamp { get; set; }
        public int Size { get; set; }
        public string Exchange { get; set; }
        public string Condition { get; set; }
        public bool Suspicious { get; set; }
    }
}
