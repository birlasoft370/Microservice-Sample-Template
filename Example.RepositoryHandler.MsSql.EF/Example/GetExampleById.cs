// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.Repository.Examples;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class GetExampleById : IGetExampleById
    {
        private readonly IExampleOperations exampleOperations;

        public GetExampleById(IExampleOperations exampleOperations)
        {
            this.exampleOperations = exampleOperations;
        }

        public async Task<ExampleDto> ExecuteAsync(int exampleId)
        {
            return await exampleOperations.GetByIdAsync(exampleId).ConfigureAwait(false);
        }
    }
}
