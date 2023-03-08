using AutoMapper;
using Example.Common.Exceptions;
using Example.DomainModel.Example;
using Example.Repository.Examples;
using MediatR;

namespace Example.QueryHandler.Example
{
    public class GetExampleByIdQueryHandler : IRequestHandler<GetExampleByIdRequest, GetExampleByIdResponse>
    {
        private readonly IGetExampleById getExampleById;
        private readonly IMapper mapper;

        public GetExampleByIdQueryHandler(IGetExampleById getExampleById, IMapper mapper)
        {
            this.getExampleById = getExampleById;
            this.mapper = mapper;
        }

        public async Task<GetExampleByIdResponse> Handle(GetExampleByIdRequest request, CancellationToken cancellationToken)
        {
            var exampleModel = await GetExampleById(request.ExampleId);

            return new GetExampleByIdResponse()
            {
                ExampleModel = exampleModel
            };
        }

        private async Task<ExampleModel> GetExampleById(int exampleId)
        {
            var exampleDto = await this.getExampleById.ExecuteAsync(exampleId).ConfigureAwait(false);

            if (exampleDto == null)
            {
                throw new NotFoundException($"exampleId. Record with Id {exampleId} Not Found");
            }

            var exampleModel = mapper.Map<ExampleModel>(exampleDto);

            return exampleModel;
        }
    }
}
