// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.Repository.Example;
using MediatR;

namespace Example.CommandHandler.Example
{
    public class AddExampleCommandHandler : IRequestHandler<AddExampleRequest, AddExampleResponse>
    {
        private readonly IAddExample addExample;
        private readonly IMapper mapper;
        // private readonly IPublishEndpoint publishEndpoint;

        public AddExampleCommandHandler(IAddExample addExample, IMapper mapper)//, IPublishEndpoint publishEndpoint
        {
            this.addExample = addExample;
            this.mapper = mapper;
            // this.publishEndpoint = publishEndpoint;
        }

        public async Task<AddExampleResponse> Handle(AddExampleRequest request, CancellationToken cancellationToken)
        {
            var exampleDto = mapper.Map<ExampleDto>(request.ExampleModel);
            var exampleModel = await AddExample(exampleDto);

            return new AddExampleResponse()
            {
                ExampleModel = exampleModel
            };
        }

        private async Task<ExampleModel> AddExample(ExampleDto exampleDto)
        {
            var result = await this.addExample.ExecuteAsync(exampleDto).ConfigureAwait(false);
            var entity = mapper.Map<ExampleModel>(result);

            //  await PublishMessageExample(result).ConfigureAwait(false);

            return entity;
        }


        private  async Task PublishMessageExample(ExampleDto exampleDto)
        {
            var msgEvent = new
            {
                exampleDto.ExampleId,
                exampleDto.ExampleName
            };

            // await publishEndpoint.Publish(msgEvent).ConfigureAwait(false);
        }
    }
}
