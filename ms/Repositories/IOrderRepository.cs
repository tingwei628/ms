using System.Threading.Tasks;

namespace ms.Repositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(int id, int amount);
    }
}
