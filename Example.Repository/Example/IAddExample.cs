// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;

namespace Example.Repository.Example
{
    public interface IAddExample
    {
        Task<ExampleDto> ExecuteAsync(ExampleDto example);
    }
}
