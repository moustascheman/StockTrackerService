using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockTrackerService.Models
{ 
    [Table("stockcsv.stocks")]
	public class StockListing
	{
        [Key]
        public string Code { get; set; }
        public long Timestamp { get; set; }
        public double Price { get; set; }
        public int Size { get; set; }
        public string Exchange { get; set; }
        public string Condition { get; set; }
        public bool Suspicious { get; set; }
    }
}
