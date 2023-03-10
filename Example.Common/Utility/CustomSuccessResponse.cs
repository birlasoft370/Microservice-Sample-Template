// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Messages;

namespace Example.Common.Utility
{
    public static class CustomSuccessResponse
    {
        public static T GetSuccessResponse<T, X, Y>(X entity, Y message)
        {
            var response = new CommonResponse
            {
                Status = AppConstants.Status.Success.ToString(),
                TimeStamp = System.DateTime.Now,
                DeveloperMessage = message.ToString()
            };
            var customResponseDetails = new CommonResponse.Details()
            {
                Code = AppConstants.SuccessCode,
                Message = message.ToString(),
            };
            response.DetailsList.Add(customResponseDetails);
            response.Data = entity;
            return (T)Convert.ChangeType(response, typeof(T));
        }
    }
}
