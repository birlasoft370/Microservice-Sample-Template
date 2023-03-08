// Copyright © CompanyName. All Rights Reserved.x
using AutoMapper;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.Repository.Example;
using MediatR;

namespace Example.CommandHandler.Example
{
    public class UpdateExampleCommandHandler : IRequestHandler<UpdateExampleRequest, UpdateExampleResponse>
    {
        private readonly IUpdateExample updateExample;
        private readonly IMapper mapper;
        public UpdateExampleCommandHandler(IUpdateExample updateExample, IMapper mapper)
        {
            this.updateExample = updateExample;
            this.mapper = mapper;
        }
        public async Task<UpdateExampleResponse> Handle(UpdateExampleRequest request, CancellationToken cancellationToken)
        {
            var exampleDto = mapper.Map<ExampleDto>(request.ExampleModel);
            await UpdateExample(exampleDto);

            return new UpdateExampleResponse()
            {
                ExampleModel = request.ExampleModel
            };
        }

        private async Task UpdateExample(ExampleDto exampleDto)
        {
            await this.updateExample.ExecuteAsync(exampleDto).ConfigureAwait(false);

            //  await PublishMessageExample(exampleDto).ConfigureAwait(false);
        }
    }
}
