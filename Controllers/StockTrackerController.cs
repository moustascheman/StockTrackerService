using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTrackerService.Data;
using StockTrackerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockTrackerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTrackerController : ControllerBase
    {
        private readonly IStockListingRepo _repo;

        public StockTrackerController(IStockListingRepo repo)
        {
            _repo = repo;
            
        }

        [HttpGet("{symbol}", Name = "GetStockBySymbol")]
        public ActionResult GetStockBySymbol(string symbol)
        {
            //TODO: REPLACE WITH ACTUAL GET ACTION THAT PUSHES TO MESSAGE HUB
            Console.WriteLine("Looking for current value of " + symbol + "\n\n");
            StockListing listing = _repo.getStockListingBySymbol(symbol);
            return Ok(listing);
        }
    }
}
