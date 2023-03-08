// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Exceptions;
using Example.DataTransfer.Examples;
using Example.Repository.Example;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class UpdateExample : IUpdateExample
    {
        private readonly IExampleOperations exampleOperations;

        public UpdateExample(IExampleOperations exampleOperatons)
        {
            this.exampleOperations = exampleOperatons;
        }

        public async Task ExecuteAsync(ExampleDto example)
        {
            if (await exampleOperations.DuplicateCheckExample(example.ExampleName, example.ExampleId))
            {
                throw new DuplicateRecordException($"ExampleName.{example.ExampleName}already exist");
            }

            await exampleOperations.UpdateAsync(example).ConfigureAwait(false);
        }
    }
}
