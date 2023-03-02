using Example.DataTransfer.Examples;

namespace Example.Repository.Examples
{
    public interface IGetExamples
    {
        Task<IEnumerable<ExampleDto>> ExecuteAsync();
    }
}
