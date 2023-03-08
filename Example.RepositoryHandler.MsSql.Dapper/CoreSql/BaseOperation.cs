using Example.DataTransfer;
using Example.Repository;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Example.RepositoryHandler.MsSql.Dapper.CoreSql
{
    public abstract class BaseOperation : IDisposable
    {
        private readonly string connectionString;
        private SqlConnection sqlConnection;
        private bool disposed;

        protected BaseOperation(IConfiguration configuration)
        {
            this.connectionString = configuration.GetSection("ConnectionStrings:SqlConnectionString")?.Value;
        }

        public SqlConnection ConnectionOpen()
        {
            this.sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public void ConnectionClose()
        {
            sqlConnection.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                this.ConnectionClose();
                this.sqlConnection.Dispose();
            }

            disposed = true;
        }
    }
}
