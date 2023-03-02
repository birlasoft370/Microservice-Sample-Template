using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.Repository.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class GetExamples : IGetExamples
    {
        private IExampleOperations ExamplesSqlOperations { get; }
        public GetExamples(IExampleOperations examplesOperatons)
        {
            this.ExamplesSqlOperations = examplesOperatons;
        }
        public async Task<IEnumerable<ExampleDto>> ExecuteAsync()
        {
            var result = await this.ExamplesSqlOperations.GetAllAsync().ConfigureAwait(false);

            return result;
        }
    }
}
