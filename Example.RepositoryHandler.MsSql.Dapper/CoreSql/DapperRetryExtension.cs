using System.ComponentModel;
using System.Data;
using Dapper;
using Polly;
using Polly.Retry;
using System.Data.SqlClient;

namespace Example.RepositoryHandler.MsSql.Dapper.CoreSql
{
    public static class DapperRetryExtension
    {
        private static AsyncRetryPolicy retryPolicy;

        public static DapperRetryExtensionSettings Settings { get; set; }

        private static AsyncRetryPolicy RetryPolicyAsync
        {
            get
            {
                if (retryPolicy == null && Settings.RetryAttempts > 0)
                {
                    List<TimeSpan> retryTimes = new();
                    for (int retryAttempt = 1; retryAttempt <= Settings.RetryAttempts; retryAttempt++)
                    {
                        retryTimes.Add(TimeSpan.FromSeconds(retryAttempt));
                    }

                    retryPolicy = Policy
                                  .Handle<SqlException>(ShouldRetryOnSqlException)
                                  .OrInner<Win32Exception>(ShouldRetryOnWin32Exception)
                                  .Or<TimeoutException>()
                                  .Or<InvalidOperationException>()
                                  .WaitAndRetryAsync(
                                                     retryTimes, (exception, timeSpan, retryCount, context) =>
                                                     {
                                                         //// Log.Warning(exception, "WARNING: Error retry attempt {RetryCount}", retryCount);
                                                     });
                }

                return retryPolicy;
            }
        }

        public static bool ShouldRetryOnSqlException(SqlException exception)
        {
            foreach (SqlError error in exception.Errors)
            {
                if (Settings.SqlExceptionErrorCodes.Contains(error.Number))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldRetryOnWin32Exception(Win32Exception exception)
        {
            if (Settings.Win32ExceptionErrorCodes.Equals(exception.NativeErrorCode))
            {
                return true;
            }

            return false;
        }

        public static async Task<int> ExecuteWithRetryAsync(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.ExecuteAsync(sql,
                    param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.ExecuteAsync(sql,
                param, transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<T>> QueryWithRetryAsync<T>(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync<T>(sql,
                    param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync<T>(sql,
                param, transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<dynamic>> QueryWithRetryAsync(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync(sql, param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync(sql, param, transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<TReturn>> QueryWithRetryAsync<TFirst, TSecond, TReturn>(this IDbConnection dbConnection,
            string sql, Func<TFirst, TSecond, TReturn> map, object param = null, IDbTransaction transaction = null, bool buffered = true,
            string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync(sql, map,
                    param, transaction, buffered, splitOn, commandTimeout, commandType)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryAsync(sql, map, param, transaction, buffered, splitOn, Settings.TimeOut, commandType)).ConfigureAwait(false);
        }

        public static async Task<T> ExecuteScalarWithRetryAsync<T>(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.ExecuteScalarAsync<T>(sql,
                    param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.ExecuteScalarAsync<T>(sql,
                param, transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task<T> QueryFirstWithRetryAsync<T>(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryFirstAsync<T>(sql,
                    param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryFirstAsync<T>(sql, param,
                transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }

        public static async Task<T> QueryFirstOrDefaultWithRetryAsync<T>(
               this IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            if (commandTimeout != null)
            {
                return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryFirstOrDefaultAsync<T>(sql,
                    param, transaction, commandTimeout, commandType).ConfigureAwait(false)).ConfigureAwait(false);
            }

            return await RetryPolicyAsync.ExecuteAsync(async () => await dbConnection.QueryFirstOrDefaultAsync<T>(sql,
                param, transaction, Settings.TimeOut, commandType).ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}
