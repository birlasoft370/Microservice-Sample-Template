// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.Common.Utility;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.Repository.Example;
using FileHelpers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Example.CommandHandler.Example
{
    public class BulkInsertExampleCommandHandler : IRequestHandler<BulkInsertExampleRequest, BulkInsertExampleResponse>
    {
        private readonly IAddExample addExample;
        private readonly IMapper mapper;
        public BulkInsertExampleCommandHandler(IAddExample addExample, IMapper mapper)
        {
            this.addExample = addExample;
            this.mapper = mapper;
        }

        public async Task<BulkInsertExampleResponse> Handle(BulkInsertExampleRequest request, CancellationToken cancellationToken)
        {
            BulkInsertExampleCsvSampleFormatModel[] records = null;
            List<ExampleDto> exampleDtos = new();

            IFormFile postedFile = request.BulkInsertExampleModel.UploadedFile;

            FileHelperEngine engine = new(typeof(BulkInsertExampleCsvSampleFormatModel));
            using (var ms = new MemoryStream())
            {
                await postedFile.CopyToAsync(ms, cancellationToken);
                ms.Seek(0, SeekOrigin.Begin);

                records = (BulkInsertExampleCsvSampleFormatModel[])engine.ReadStream(new StreamReader(ms), Int32.MaxValue);
            }

            //foreach (var Example in records)
            //{
            //    ExampleDto exampleDto = new ExampleDto()
            //    {
            //        CreatedBy = request.BulkInsertExampleModel.UserID,
            //        LastModifiedBy = request.BulkInsertExampleModel.UserID,
            //        ExampleName = Example.ExampleName,
            //        IsActive = (char)AppConstants.IsActive.Yes
            //    };
            //    var result = await this.addExample.ExecuteAsync(exampleDto).ConfigureAwait(false);
            //    exampleModels.Add(result);
            //}

            //Bulk Insert
            foreach (var Example in records)
            {
                ExampleDto exampleDto = new()
                {
                    CreatedBy = request.BulkInsertExampleModel.UserID,
                    LastModifiedBy = request.BulkInsertExampleModel.UserID,
                    ExampleName = Example.ExampleName,
                    IsActive = (char)AppConstants.IsActive.Yes
                };
                exampleDtos.Add(exampleDto);
            }

            var result = await addExample.ExecuteRangeAsync(exampleDtos).ConfigureAwait(false);

            var response = mapper.Map<List<ExampleModel>>(result);

            return new BulkInsertExampleResponse() { ExampleModels = response };
        }
    }
}
