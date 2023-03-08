// Copyright © CompanyName. All Rights Reserved.
namespace Example.RepositoryHandler.MsSql.Dapper.CoreSql
{
    public class DapperRetryExtensionSettings
    {
        public int RetryAttempts { get; set; }

        public int TimeOut { get; set; }

        public int[] SqlExceptionErrorCodes { get; set; }

        public string[] Win32ExceptionErrorCodes { get; set; }
    }
}
