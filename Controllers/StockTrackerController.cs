using Confluent.Kafka;
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
        private readonly ProducerConfig _proConfig;

        public StockTrackerController(IStockListingRepo repo, ProducerConfig config)
        {
            _repo = repo;
            _proConfig = config;
            
            
        }

        [HttpGet("{symbol}", Name = "GetStockBySymbol")]
        public ActionResult GetStockBySymbol(string symbol)
        {
            if(symbol == null)
            {
                return BadRequest();
            }
            Console.WriteLine("Looking for current value of " + symbol + "\n\n");
            try
            {

                string lowerSym = symbol.Trim().ToLower();
                StockListing listing = _repo.getStockListingBySymbol(lowerSym);
                if(listing == null)
                {
                    throw new Exception();
                }
                PublishToMessageBus(listing);
                return Ok(listing);
            }
            catch(Exception e)
            {
                return NotFound();
            }
            
        }

        private void PublishToMessageBus(StockListing listing)
        {
            IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_proConfig).Build();
            string MessageString = listing.Code + ";" + listing.Price.ToString() + ";"+listing.Timestamp.ToString();
            Message<Null, string> messTest = new Message<Null, string> { Value = MessageString };
            string topicName = getTopicName(listing.Code);
            producer.Produce(topicName, messTest);
        }

        private String getTopicName(string symbol)
        {
            string topicName ="";
            char firstLetter = symbol[0];
            if (char.IsLetter(firstLetter))
            {
                topicName = "StockTrackerTopic_" + firstLetter.ToString().ToUpper();
            }
            else
            {
                topicName = "StockTrackerTopic_Other";
            }
            return topicName;
        }

    }
}
