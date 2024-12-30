using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PgController : ControllerBase
    {
        private readonly ILogger<PgController> _logger;
        private readonly IConfiguration _configuration;

        public PgController(ILogger<PgController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<int>> Get(int id, int amount)
        {
            /*
             create table orders (
	            id INT,
	            amount NUMERIC
            )
            */
            var connectionString = _configuration.GetConnectionString("PgCluster");
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var insertCmd = new NpgsqlCommand("INSERT INTO orders (id, amount) VALUES (@id, @amount);", connection, transaction);
                insertCmd.Parameters.AddWithValue("@id", id);
                insertCmd.Parameters.AddWithValue("@amount", amount);

                await insertCmd.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Transaction failed: {ex.Message}");
                await transaction.RollbackAsync();
            }

            return new List<int> { id, amount };
        }
    }
}
