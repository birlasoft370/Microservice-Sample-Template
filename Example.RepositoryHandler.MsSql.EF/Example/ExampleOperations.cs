// Copyright © CompanyName. All Rights Reserved.
using Dapper;
using Example.Common.Exceptions;
using Example.Common.Utility;
using Example.DataTransfer;
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.CoreSql;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace Example.RepositoryHandler.MsSql.EF.Example
{
    interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> ExecuteCommandQuery(string command);
    }
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }


        public IEnumerable<TEntity> ExecuteCommandQuery(string command)
            => dbSet.FromSqlRaw(command);

    }

    public class ExampleOperations : BaseRepositoryOperations<ExampleDto>, IExampleOperations
    {
        private readonly DapperUtility dapperObj;

        public ExampleOperations(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
            dapperObj = DapperUtility.CreateInstance(connectionString, configuration);
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

        public async Task<List<ExampleDto>> GetExamplesAsync(int exampleId)
        {

            string spName = "Usp_GetExamples";

            DynamicParameters parameterList = new DynamicParameters();
            parameterList.Add("@ExampleId", 1, System.Data.DbType.Int32);
            return await dapperObj.GetAllRecs<ExampleDto>(spName, parameterList);

            //var query = "";
            //var queryingResult = await ApplicationDbContext.Database.ExecuteSqlRawAsync("SELECT * FROM Example WHERE 1=1");

            //var sqlParameterOut = new SqlParameter
            //{
            //    ParameterName = "@ReturnValue",
            //    DbType = DbType.Int32,
            //    Size = 20,
            //    Direction = ParameterDirection.Output
            //};
            //int queryResult = await ApplicationDbContext.Database.ExecuteSqlRawAsync("SELECT * FROM Example WHERE 1=1", sqlParameterOut);
            //var  name = sqlParameterOut.Value;
            ////  var resultset = ExecuteCommandQuery("SELECT * FROM Example WHERE 1=1");

            var data = ApplicationDbContext.Set<dummyExampleModel>().FromSqlRaw("SELECT * FROM Example WHERE 1=1")
     // .Select(t => new ExampleDto { ExampleId = t.ExampleId, ExampleName = t.ExampleName })
     .ToList();

            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ExampleId", exampleId));

            var data1 = await ApplicationDbContext.Set<dummyExampleModel>().FromSqlRaw($"EXEC Usp_GetExamples @ExampleId", parameters: parameters.ToArray()).ToListAsync();

            //var result12 = await ApplicationDbContext.dummyExamples.FromSqlRaw($"Usp_GetExamples {exampleId}").ToListAsync();

            var GetSingleExample = ApplicationDbContext.Example.FromSqlRaw($"GetExampleById {exampleId}").AsEnumerable().FirstOrDefault();

            var result = await ApplicationDbContext.Example.FromSqlRaw($"Usp_GetExamples {exampleId}").ToListAsync();

            var parameters1 = new List<SqlParameter>();
            parameters1.Add(new SqlParameter("@ExampleId", exampleId));
            var result1 = await ApplicationDbContext.Example.FromSqlRaw($"EXEC Usp_GetExamples @ExampleId", parameters: parameters1.ToArray()).ToListAsync();

            return result;
            //  var Examples = ApplicationDbContext.Database.SqlQuery(procedureName, sqlParameter);
        }

        public async Task<ExampleDto> GetExampleByExampleIdAsync(int exampleId)
        {
            // string procedureName = "EXEC Usp_GetMaxExampleId @ReturnValue OUTPUT";
            string procedureName = "[dbo].[Usp_GetMaxExampleId] @ReturnValue OUT";
            var sqlParameterOut = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                DbType = DbType.Int32,
                Size = 20,
                Direction = ParameterDirection.Output
            };
            var fruit = ApplicationDbContext.Database.ExecuteSqlRaw(procedureName, sqlParameterOut);
            string name = Convert.ToString(sqlParameterOut.Value);

            string sql = "EXEC [Usp_INSExample] @ExampleName,@CreatedBy";
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {

                 new SqlParameter("@ExampleName", "ExampleName"),
                 new SqlParameter("@CreatedBy", 1)
            };

            int lstinserted = await ApplicationDbContext.Database.ExecuteSqlRawAsync(sql, sqlParameters);

            int rowsAffected;

            string sql1 = "EXEC [Usp_INSExample] @ExampleName,@CreatedBy";
            List<SqlParameter> parms = new List<SqlParameter>
                { 
                // Create parameters    
                       new SqlParameter("@ExampleName", "ExampleName"),
                 new SqlParameter("@CreatedBy", 1)
                };

            rowsAffected = ApplicationDbContext.Database.ExecuteSqlRaw(sql1, parms);

            return await
                ApplicationDbContext.Example.FirstOrDefaultAsync(o => o.ExampleId == exampleId).ConfigureAwait(false) ?? await Task.FromResult<ExampleDto>(null);
        }

    }
}
