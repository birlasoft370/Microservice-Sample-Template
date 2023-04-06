// Copyright © CompanyName. All Rights Reserved.
using Dapper;
using Example.Common.Utility;
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
            string spName = "Usp_GetExamples";
            DynamicParameters parameterList = null;

            parameterList = new DynamicParameters();
            parameterList.Add("@ExampleId", 1, System.Data.DbType.Int32);
            return GetAllRecs<ExampleDto>(spName, parameterList);



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
                    INSERT INTO Example (ExampleName,CreatedBy, CreatedDate,LastModifiedBy,LastModifiedDate,IsActive)
                    VALUES (@ExampleName,@CreatedBy,@CreatedDate,@LastModifiedBy,@LastModifiedDate,@IsActive);
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

        public async Task<IEnumerable<ExampleDto>> AddUpdateExample(int exampleId, ExampleDto exampleDto)
        {
            List<ExampleDto> exampleList = new();

            int affectedExampleId;

            var existExample = await GetByIdAsync(exampleId) ?? throw new Exception($"ExampleId. Record with Id : {exampleId} not found");

            using (var connection = this.ConnectionOpen())
            using (var transaction = connection.BeginTransaction())
            {
                affectedExampleId = await connection.QuerySingleAsync<int>(
                       $"INSERT INTO Example (ExampleName, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate, IsActive)" +
                       $"VALUES(@ExampleName, @CreatedBy, @CreatedDate, @LastModifiedBy, @LastModifiedDate, @IsActive)" +
                       $"SELECT CAST(SCOPE_IDENTITY() as int)", new
                       {
                           exampleDto.ExampleName,
                           exampleDto.CreatedBy,
                           CreatedDate = DateTime.Now,
                           exampleDto.LastModifiedBy,
                           LastModifiedDate = DateTime.Now,
                           exampleDto.IsActive
                       }, transaction);

                existExample.IsActive = existExample.IsActive == (char)AppConstants.IsActive.Yes ? (char)AppConstants.IsActive.No : (char)AppConstants.IsActive.Yes;
                await connection.ExecuteAsync
                    ("UPDATE Example SET IsActive=@IsActive, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate WHERE ExampleId = @ExampleId",
                new { existExample.IsActive, exampleDto.LastModifiedBy, LastModifiedDate = DateTime.Now, ExampleId = exampleId }, transaction);

                transaction.Commit();

                var insertedExample = await GetByIdAsync(affectedExampleId);
                exampleList.Add(insertedExample);
                var updatedExample = await GetByIdAsync(exampleId);
                exampleList.Add(updatedExample);
            }

            return exampleList;

        }

        public async Task<bool> DuplicateCheckExample(string exampleName, int exampleId)
        {
            var examples = await GetAllAsync();
            bool duplicateRecordFlag = examples.ToList().Exists(x => x.ExampleName.Trim().ToLower() == exampleName.Trim().ToLower() && (x.ExampleId != exampleId));
            return duplicateRecordFlag;
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

        public Task<int> UpdateRangeAsync(IList<ExampleDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddRangeAsync(IList<ExampleDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteRangeAsync(IList<ExampleDto> entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExampleDto>> GetExamplesAsync(int exampleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExampleDto> ExecuteCommandQuery(string command)
        {
            throw new NotImplementedException();
        }
    }
}
