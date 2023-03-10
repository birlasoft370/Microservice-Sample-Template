// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Exceptions;
using Example.Common.Utility;
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.CoreSql;
using Microsoft.Data.SqlClient;
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
            return await
                ApplicationDbContext.Example.FirstOrDefaultAsync(o => o.ExampleId == exampleId).ConfigureAwait(false) ?? await Task.FromResult<ExampleDto>(null);
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
            // var GetSingleExample = ApplicationDbContext.Example.FromSqlRaw($"GetExampleById {exampleId}").AsEnumerable().FirstOrDefault();

            var entity = await (ApplicationDbContext.Example
                                   .Where(o => o.ExampleId == exampleId)
                                   ?.ToListAsync()).ConfigureAwait(false);
            if (entity != null)
            {
                ApplicationDbContext.Set<ExampleDto>().RemoveRange(entity);
                await ApplicationDbContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<ExampleDto>> AddUpdateExample(int exampleId, ExampleDto exampleDto)
        {
            List<ExampleDto> exampleDtos = new();

            //Add Example

            var addedexampleDto = await ApplicationDbContext.Example.AddAsync(exampleDto).ConfigureAwait(false);

            // await ApplicationDbContext.SaveChangesAsync().ConfigureAwait(false);

            //Update Example

            var exampleDetail = await ApplicationDbContext.Example.FirstOrDefaultAsync(ex => ex.ExampleId.Equals(exampleId));

            if (exampleDetail != null)
            {
                exampleDetail.IsActive = exampleDetail.IsActive == (char)AppConstants.IsActive.Yes ? (char)AppConstants.IsActive.No : (char)AppConstants.IsActive.Yes;

                var updatedexampleDto = ApplicationDbContext.Example.Update(exampleDetail);
                exampleDtos.Add(updatedexampleDto.Entity);
            }
            else
            {
                throw new NotFoundException($"exampleId. Record with Id {exampleId} Not Found");
            }

            await ApplicationDbContext.SaveChangesAsync().ConfigureAwait(false);

            exampleDtos.Add(addedexampleDto.Entity);

            return exampleDtos;
        }

        public async Task<bool> DuplicateCheckExample(string exampleName, int exampleId)
        {
            var examples = await ApplicationDbContext.Example.ToListAsync().ConfigureAwait(false);
            bool duplicateRecordFlag = examples.Exists(x => (x.ExampleName.Trim().ToLower() == exampleName.Trim().ToLower() && (x.ExampleId != exampleId)));
            return duplicateRecordFlag;
        }
    }
}
