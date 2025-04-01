using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace Infrastructure.Data;

public class DataContext
{
    private const string connectionString = "Host=localhost;Username=postgres;Password=m1866;Database=My_db";
    public async Task<IDbConnection> GetConnection()
    {
        return new  NpgsqlConnection(connectionString);
    }

}
