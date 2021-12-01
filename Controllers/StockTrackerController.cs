using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockTrackerService.Data;
using StockTrackerService.DTOs;
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
        private readonly ProducerConfig _proConfig;
        private readonly ILogger<StockTrackerController> _logger;

        public StockTrackerController(IStockListingRepo repo, ProducerConfig config, ILogger<StockTrackerController> logger)
        {
            _repo = repo;
            _proConfig = config;
            _logger = logger;
            
            
        }

        [HttpGet("{symbol}", Name = "GetStockBySymbol")]
        public ActionResult GetStockBySymbol(string symbol)
        {
            //LOGGING
            var dt1 = DateTimeOffset.UtcNow;
            _logger.LogInformation("Starting request at: {t}", dt1);
            ////
            if(symbol == null)
            {
                return BadRequest();
            }
            Console.WriteLine("Looking for current value of " + symbol + "\n\n");
            try
            {

                string lowerSym = symbol.Trim().ToLower();
                StockListing listing = _repo.getStockListingBySymbol(lowerSym);
                if (listing == null)
                {
                    throw new Exception();
                }
                StockDTO listingDTO = mapListingToDTO(listing);
                ///
                _logger.LogInformation("Retrieval completed, publishing to message bus at {t}", DateTimeOffset.UtcNow);
                ///
                PublishToMessageBus(listingDTO);
                ///
                var dt2 = DateTimeOffset.UtcNow;
                var dt3 = (dt2 - dt1).TotalMilliseconds.ToString();
                _logger.LogInformation("Published at {t}", dt2);
                _logger.LogInformation("Total time for request: {t}", dt3);
                ///
                return Ok(listingDTO);
            }
            catch(Exception e)
            {
                _logger.LogWarning("Stock Listing was not found");
                return NotFound();
            }
            
        }

        private void PublishToMessageBus(StockDTO listing)
        {
            IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_proConfig).Build();
            string MessageString = listing.Code + ";" + listing.price.ToString() + ";"+listing.timestamp.ToString();
            Message<Null, string> myMessage = new Message<Null, string> { Value = MessageString };
            myMessage.Headers = getSortingHeaders(listing.Code);
            
            producer.Produce("4471ProjectTopic", myMessage);
        }

        private Headers getSortingHeaders(string symbol)
        {
            Headers headers = new Headers();
            headers.Add(new Header("StockTrackerService", null));
            headers.Add(new Header("1", null));
            headers.Add(new Header(symbol, null));
            return headers;
        }

        private StockDTO mapListingToDTO(StockListing listing)
        {
            StockDTO myDTO = new StockDTO();
            myDTO.Code = listing.Code;
            myDTO.price = listing.Price;
            myDTO.timestamp = listing.Timestamp;
            return myDTO;
        }
    }

}
