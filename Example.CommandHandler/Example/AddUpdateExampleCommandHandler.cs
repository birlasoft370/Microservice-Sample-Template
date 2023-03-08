// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.Repository.Example;
using MediatR;

namespace Example.CommandHandler.Example
{
    public class AddUpdateExampleCommandHandler : IRequestHandler<AddUpdateExampleRequest, AddUpdateExampleResponse>
    {
        private readonly IAddUpdateExample addUpdateExample;
        private readonly IMapper mapper;

        public AddUpdateExampleCommandHandler(IAddUpdateExample addUpdateExample, IMapper mapper)
        {
            this.addUpdateExample = addUpdateExample;
            this.mapper = mapper;
        }

        public async Task<AddUpdateExampleResponse> Handle(AddUpdateExampleRequest request, CancellationToken cancellationToken)
        {
            var exampleDto = mapper.Map<ExampleDto>(request.ExampleModel);
            var exampleModel = await AddUpdateExample(request.UpdateExampleId, exampleDto);

            return new AddUpdateExampleResponse()
            {
                ExampleModels = exampleModel
            };
        }

        private async Task<List<ExampleModel>> AddUpdateExample(int exampleId, ExampleDto exampleDto)
        {
            var result = await this.addUpdateExample.ExecuteAsync(exampleId, exampleDto).ConfigureAwait(false);
            var entity = mapper.Map<List<ExampleModel>>(result);
            return entity;
        }

    }
}
