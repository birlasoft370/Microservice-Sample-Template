using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Example.RepositoryHandler.MsSql.EF
{
    public class DapperUtility
    {
        static Lazy<DapperUtility> obj_Dapper = null;
        private static string connectionString;
        public static DapperUtility CreateInstance(string connString, IConfiguration configuration)
        {
            connectionString = connString;// configuration.GetSection("ConnectionStrings:SqlConnectionString")?.Value;
            obj_Dapper = new Lazy<DapperUtility>();// obj_Dapper = new DapperUtility();
            return obj_Dapper.Value;
        }

        internal IDbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public async Task<List<T>> GetAllRecs<T>(string storedProcedure, DynamicParameters param) where T : class
        {
            using (var connection = GetOpenConnection())
            {
                var result = await connection.QueryAsync<T>(storedProcedure, param, commandType: CommandType.StoredProcedure);

                return result.ToList();
            }
        }
    }
}
