using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockTrackerService.DTOs
{
    public class StockDTO
    {
        [Key]
        public String Code { get; set; }

        public double price { get; set; }
          
        public long timestamp { get; set; }

    }
}
