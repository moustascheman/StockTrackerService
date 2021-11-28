using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                if (listing == null)
                {
                    throw new Exception();
                }
                StockDTO listingDTO = mapListingToDTO(listing); 
                PublishToMessageBus(listingDTO);
                return Ok(listingDTO);
            }
            catch(Exception e)
            {
                return NotFound();
            }
            
        }

        private void PublishToMessageBus(StockDTO listing)
        {
            IProducer<Null, string> producer = new ProducerBuilder<Null, string>(_proConfig).Build();
            string MessageString = listing.Code + ";" + listing.price.ToString() + ";"+listing.timestamp.ToString();
            Message<Null, string> myMessage = new Message<Null, string> { Value = MessageString };
            myMessage.Headers = new Headers();
            myMessage.Headers.Add(new Header("StockTrackerService", null));
            
            producer.Produce("4471ProjectTopic", myMessage);
        }

        private String getSortingHeader(string symbol)
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
