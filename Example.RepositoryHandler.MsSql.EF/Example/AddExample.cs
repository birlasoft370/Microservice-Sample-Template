// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Exceptions;
using Example.DataTransfer.Examples;
using Example.Repository.Example;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class AddExample : IAddExample
    {
        private readonly IExampleOperations exampleOperations;

        public AddExample(IExampleOperations exampleOperations)
        {
            this.exampleOperations = exampleOperations;
        }

        public async Task<ExampleDto> ExecuteAsync(ExampleDto example)
        {
            if (await exampleOperations.DuplicateCheckExample(example.ExampleName, example.ExampleId))
            {
                throw new DuplicateRecordException($"ExampleName.{example.ExampleName}already exist");
            }

            var result = await exampleOperations.AddAsync(example).ConfigureAwait(false);

            return result;
        }

        public async Task<List<ExampleDto>> ExecuteRangeAsync(List<ExampleDto> examples)
        {
            var result = await exampleOperations.AddRangeAsync(examples).ConfigureAwait(false);

            return examples;
        }
    }
}
