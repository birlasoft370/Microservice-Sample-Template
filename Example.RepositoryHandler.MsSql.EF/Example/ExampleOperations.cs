using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.CoreSql;
using Microsoft.EntityFrameworkCore;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    public class ExampleOperations : BaseRepositoryOperations<ExampleDto>, IExampleOperations
    {
        public ExampleOperations(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ExampleDto> GetExampleByExampleIdAsync(int exampleId)
        {
            return await (ApplicationDbContext.Example.FirstOrDefaultAsync(o => o.ExampleId == exampleId).ConfigureAwait(false));
        }       

        public async Task<IEnumerable<ExampleDto>> GetExampleByNameAsync(string name)
        {
            var result = await (ApplicationDbContext.Example
                                    .Where(o => o.ExampleName == name)
                                    ?.ToListAsync()).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteExampleByExampleIdAsync(int exampleId)
        {
            var entity = await (ApplicationDbContext.Example
                                   .Where(o => o.ExampleId == exampleId)
                                   ?.ToListAsync()).ConfigureAwait(false);
            if (entity != null)
            {
                ApplicationDbContext.Set<ExampleDto>().RemoveRange(entity);
                await ApplicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
