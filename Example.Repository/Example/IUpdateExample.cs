﻿// Copyright © CompanyName. All Rights Reserved.

using Example.DataTransfer.Examples;

namespace Example.Repository.Example
{
    public interface IUpdateExample
    {
        Task ExecuteAsync(ExampleDto example);
    }
}
