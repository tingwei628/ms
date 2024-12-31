using Npgsql;
using System.Threading.Tasks;

namespace ms.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly NpgsqlConnection _connection;

        public OrderRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task AddOrderAsync(int id, int amount)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }

            // transation
            using var transaction = await _connection.BeginTransactionAsync();
            try
            {
                var insertCmd = new NpgsqlCommand("INSERT INTO orders (id, amount) VALUES (@id, @amount);", _connection, transaction);
                insertCmd.Parameters.AddWithValue("@id", id);
                insertCmd.Parameters.AddWithValue("@amount", amount);

                await insertCmd.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
