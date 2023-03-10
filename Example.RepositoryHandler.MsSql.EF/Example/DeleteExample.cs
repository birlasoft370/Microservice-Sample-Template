// Copyright © CompanyName. All Rights Reserved.
using Example.Repository.Example;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class DeleteExample : IDeleteExample
    {
        private readonly IExampleOperations exampleOperations;

        public DeleteExample(IExampleOperations exampleOperatons)
        {
            this.exampleOperations = exampleOperatons;
        }

        public async Task ExecuteAsync(int exampleId)
        {
            //Stored Procedure
            //await exampleOperations.DeleteExampleByExampleIdAsync(exampleId).ConfigureAwait(false);
            await exampleOperations.DeleteExampleByExampleIdAsync(exampleId).ConfigureAwait(false);
        }
    }
}
