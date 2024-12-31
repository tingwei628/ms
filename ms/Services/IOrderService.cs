using System.Collections.Generic;
using System.Threading.Tasks;

namespace ms.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<int>> AddOrderAsync(int id, int amount);
    }
}
