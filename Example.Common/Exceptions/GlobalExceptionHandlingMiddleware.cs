// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Messages;
using Example.Common.Utility;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace Example.Common.Exceptions
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            CommonResponse response = new();

            response.Status = AppConstants.Status.Error.ToString();
            response.TimeStamp = System.DateTime.Now;
            response.DeveloperMessage = (ex.Message.Contains("See inner exception for details") ? ex.InnerException.Message : ex.Message);

            var exceptionType = ex.GetType();

            if (exceptionType == typeof(DuplicateRecordException))
            {
                // throw new DuplicateRecordException("ColumnName.Duplicate Error Message");

                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = JsonNamingPolicy.CamelCase.ConvertName(ex.Message.Split(".")[0].ToString()),
                    Code = AppConstants.ValidationCode,
                    Message = ex.Message.Split(".")[1].ToString(),
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                // throw new NotFoundException("Column Name.Not Found Message);
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = JsonNamingPolicy.CamelCase.ConvertName(ex.Message.Split(".")[0].ToString()),
                    Code = AppConstants.DatabaseErrorCode,
                    Message = ex.Message.Split(".")[1].ToString()
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(SqlException))
            {
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = "SqlException",
                    Code = AppConstants.DatabaseErrorCode,
                    Message = AppConstants.ExceptionType.DatabaseException.ToString()
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(Microsoft.Data.SqlClient.SqlException))
            {
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = "Microsoft.Data.SqlClient.SqlException",
                    Code = AppConstants.DatabaseErrorCode,
                    Message = AppConstants.ExceptionType.DatabaseException.ToString()
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = "NotImplemented",
                    Code = AppConstants.DatabaseErrorCode,
                    Message = AppConstants.ExceptionType.NotImplemented.ToString()
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException) || exceptionType == typeof(KeyNotFoundException))
            {
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = "Unauthorized",
                    Code = AppConstants.DatabaseErrorCode,
                    Message = AppConstants.ExceptionType.Unauthorized.ToString()
                };
                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                var customResponseDetails = new CommonResponse.Details()
                {
                    FieldName = "Unhandled Exception",
                    Code = AppConstants.RuntimeErrorCode,
                    Message = AppConstants.ExceptionType.UnexpectedError.ToString(),
                };

                response.DetailsList.Add(customResponseDetails);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            await context.Response.WriteJson(response, "application/json");
        }
    }
}
