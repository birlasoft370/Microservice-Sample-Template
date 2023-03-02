using Example.DataTransfer.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Repository.Example
{
    public interface IExampleOperations : IAsyncRepositoryOperations<ExampleDto>
    {
        Task<IEnumerable<ExampleDto>> GetExampleByNameAsync(string name);

        Task<ExampleDto> GetExampleByExampleIdAsync(int exampleId);

        Task DeleteExampleByExampleIdAsync(int exampleId);
    }
}
