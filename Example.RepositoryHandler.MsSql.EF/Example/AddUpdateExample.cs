﻿// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;
using Example.Repository.Example;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class AddUpdateExample : IAddUpdateExample
    {
        private readonly IExampleOperations exampleOperations;

        public AddUpdateExample(IExampleOperations exampleOperations)
        {
            this.exampleOperations = exampleOperations;
        }

        public async Task<ExampleDto> ExecuteAsync(ExampleDto example)
        {
            var result = await exampleOperations.AddAsync(example).ConfigureAwait(false);

            return result;
        }

        public async Task<List<ExampleDto>> ExecuteAsync(int exampleId, ExampleDto exampleDto)
        {
            var result = await exampleOperations.AddUpdateExample(exampleId, exampleDto).ConfigureAwait(false);

            return result.ToList();
        }
    }
}
