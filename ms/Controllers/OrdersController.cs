using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ms.Services;
using Npgsql;

namespace ms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test(int id, int amount)
        {
            /*
               create table orders (
                  id INT,
                  amount NUMERIC
              )
          */
            try
            {
                var result = await _orderService.AddOrderAsync(id, amount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add order");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
