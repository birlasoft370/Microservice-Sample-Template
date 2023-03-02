using AutoMapper;
using Example.DomainModel.Example;
using Example.Repository.Examples;
using MediatR;

namespace Example.QueryHandler.Examples
{
    public class GetExamplesQueryHandler : IRequestHandler<GetExamplesRequest, GetExamplesResponse>
    {
        private readonly IGetExamples getExamples;
        private readonly IMapper mapper;

        public GetExamplesQueryHandler(IGetExamples getExamples, IMapper mapper)
        {
            this.getExamples = getExamples;
            this.mapper = mapper;
        }

        public async Task<GetExamplesResponse> Handle(GetExamplesRequest request, CancellationToken cancellationToken)
        {
           var examplesModel = await GetExamples();

            return new GetExamplesResponse()
            {
                ExamplesModel = examplesModel
            };
        }

        public async Task<IEnumerable<ExampleModel>> GetExamples()
        {
            var exampleDto = await this.getExamples.ExecuteAsync().ConfigureAwait(false);

            var exampleModel = mapper.Map<IEnumerable<ExampleModel>>(exampleDto);

            return exampleModel;
        }
    }
}
