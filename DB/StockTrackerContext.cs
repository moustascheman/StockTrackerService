using System;
using IBM.Data.Db2;
using Microsoft.EntityFrameworkCore;

namespace StockTrackerService.Models
{
	public class StockTrackerContext : DbContext
	{
        private readonly DB2Connection _connection;
    
        public StockTrackerContext()
        {
            String connString = "";
			_connection = new DB2Connection(connString);
            _connection.Open();
        }


	}
}