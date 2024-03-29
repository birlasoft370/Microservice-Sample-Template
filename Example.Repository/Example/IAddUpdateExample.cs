﻿// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;

namespace Example.Repository.Example
{
    public interface IAddUpdateExample
    {
        Task<IEnumerable<ExampleDto>> ExecuteAsync(int exampleId, ExampleDto exampleDto);
    }
}
