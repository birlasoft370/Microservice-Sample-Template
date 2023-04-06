using Example.DataTransfer.Examples;

namespace Example.Repository.Example
{
    public interface IExampleOperations : IAsyncRepositoryOperations<ExampleDto>
    {
        Task<IEnumerable<ExampleDto>> GetExampleByNameAsync(string name);

        Task<ExampleDto> GetExampleByExampleIdAsync(int exampleId);

        Task DeleteExampleByExampleIdAsync(int exampleId);

        Task<IEnumerable<ExampleDto>> AddUpdateExample(int exampleId,ExampleDto exampleDto);

        Task<bool> DuplicateCheckExample(string exampleName, int exampleId);

        Task<List<ExampleDto>> GetExamplesAsync(int exampleId);

    }
}
