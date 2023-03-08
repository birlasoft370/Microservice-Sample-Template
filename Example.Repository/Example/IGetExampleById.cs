// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;

namespace Example.Repository.Examples
{
    public interface IGetExampleById
    {
        Task<ExampleDto> ExecuteAsync(int exampleId);
    }
}
