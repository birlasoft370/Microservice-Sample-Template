using Dapper;
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.Dapper.CoreSql;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Example.RepositoryHandler.MsSql.Dapper.Example
{
    public class ExampleOperations : BaseOperation, IExampleOperations
    {
        public ExampleOperations(IConfiguration configuration) : base(configuration)
        {

        }

        public async Task<IReadOnlyList<ExampleDto>> GetAllAsync()
        {
            using var connection = this.ConnectionOpen();
            string sqlQuery = "SELECT * FROM Example";

            var result = await connection.QueryAsync<ExampleDto>(sqlQuery).ConfigureAwait(false);
            this.ConnectionClose();

            IReadOnlyList<ExampleDto> rdOnly = result.ToList().AsReadOnly();

            return rdOnly;
        }

        public async Task<ExampleDto> GetByIdAsync(int id)
        {
            using var connection = this.ConnectionOpen();

            var example = await connection.QuerySingleOrDefaultAsync<ExampleDto>
            ("SELECT * FROM Example WHERE ExampleId = @ExampleId", new { ExampleId = id });

            this.ConnectionClose();

            return example;
        }

        public async Task<ExampleDto> AddAsync(ExampleDto entity)
        {
            using var connection = this.ConnectionOpen();

            var affectedExampleId =
                await connection.QuerySingleAsync<int>(@"
                    INSERT INTO Example (ExampleName,CreatedBy, CreatedDate,LastModifiedBy,LastModifiedDate)
                    VALUES (@ExampleName,@CreatedBy,@CreatedDate,@LastModifiedBy,@LastModifiedDate);
                    SELECT CAST(SCOPE_IDENTITY() as int)", new
                {
                    entity.ExampleName,
                    entity.CreatedBy,
                    CreatedDate = DateTime.Now,
                    entity.LastModifiedBy,
                    LastModifiedDate = DateTime.Now
                });

            this.ConnectionClose();
            if (affectedExampleId == 0)
                return null;
            var example = await GetByIdAsync(affectedExampleId);
            return example;
        }

        public async Task UpdateAsync(ExampleDto example)
        {
            using var connection = this.ConnectionOpen();

            var affected = await connection.ExecuteAsync
                    ("UPDATE Example SET ExampleName=@ExampleName, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate WHERE ExampleId = @ExampleId",
            new { example.ExampleName, example.LastModifiedBy, LastModifiedDate = DateTime.Now, example.ExampleId });
            this.ConnectionClose();
        }
        public async Task DeleteExampleByExampleIdAsync(int exampleId)
        {
            using var connection = this.ConnectionOpen();

            var affected = await connection.ExecuteAsync("DELETE FROM Example WHERE ExampleId = @ExampleId",
                new { ExampleId = exampleId });
            this.ConnectionClose();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ExampleDto>> GetAsync(Expression<Func<ExampleDto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ExampleDto>> GetAsync(Expression<Func<ExampleDto, bool>> predicate = null, Func<IQueryable<ExampleDto>, IOrderedQueryable<ExampleDto>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ExampleDto>> GetAsync(Expression<Func<ExampleDto, bool>> predicate = null, Func<IQueryable<ExampleDto>, IOrderedQueryable<ExampleDto>> orderBy = null, List<Expression<Func<ExampleDto, object>>> includes = null, bool disableTracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<ExampleDto> GetExampleByExampleIdAsync(int exampleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExampleDto>> GetExampleByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ExampleDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExampleDto>> AddUpdateExample(int exampleId, ExampleDto exampleDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DuplicateCheckExample(string exampleName, int exampleId)
        {
            throw new NotImplementedException();
        }
    }
}
