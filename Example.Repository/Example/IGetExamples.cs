using Example.DataTransfer.Examples;
using Example.Repository.Example;

namespace Example.Repository.Examples
{
    public interface IGetExamples
    {
        Task<IEnumerable<ExampleDto>> ExecuteAsync();
    }

    /*
    public interface IExamples
    {
        Task<IEnumerable<ExampleDto>> GetExamples();
        Task<ExampleDto> GetExampleById(int exampleId);
        Task<ExampleDto> AddExample();
        Task<ExampleDto> UpdateExample();
    }

    public class Examples : IExamples
    {
        private readonly IExampleOperations exampleOperations;
        public Examples(IExampleOperations exampleOperations)
        {
            exampleOperations = exampleOperations;
        }
        public Task<ExampleDto> AddExample(int exampleId)
        {
            throw new NotImplementedException();
        }

        public Task<ExampleDto> AddExample()
        {
            throw new NotImplementedException();
        }

        public async Task<ExampleDto> GetExampleById(int exampleId)
        {
            return await exampleOperations.GetExampleByExampleIdAsync(exampleId);
        }

        public async Task<IEnumerable<ExampleDto>> GetExamples()
        {
            var result = await exampleOperations.GetExamplesAsync(1).ConfigureAwait(false);
            return result;
        }

        public Task<ExampleDto> UpdateExample()
        {
            throw new NotImplementedException();
        }
    }*/
}
