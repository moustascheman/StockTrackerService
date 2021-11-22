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
        

        public StockTrackerController()
        {
            
            
        }

        [HttpGet("{symbol}", Name = "GetStockBySymbol")]
        public ActionResult GetStockBySymbol(string symbol)
        {
            //TODO: REPLACE WITH ACTUAL GET ACTION THAT PUSHES TO MESSAGE HUB
            Console.WriteLine("Looking for current value of " + symbol + "\n\n");
            return Ok();
        }
    }
}
