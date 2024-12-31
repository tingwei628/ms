using ms.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ms.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<int>> AddOrderAsync(int id, int amount)
        {
            await _orderRepository.AddOrderAsync(id, amount);
            return new List<int> { id, amount };
        }
    }
}
