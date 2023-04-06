// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.Repository.Examples;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class GetExamples : IGetExamples
    {
        private readonly IExampleOperations ExamplesSqlOperations;
        public GetExamples(IExampleOperations examplesOperatons)
        {
            this.ExamplesSqlOperations = examplesOperatons;
        }
        public async Task<IEnumerable<ExampleDto>> ExecuteAsync()
        {
            var result = await this.ExamplesSqlOperations.GetAllAsync();

            return result;
        }
    }
}
