// Copyright © CompanyName. All Rights Reserved.
using Example.DomainModel.Example;
using Example.Repository.Example;
using MediatR;

namespace Example.CommandHandler.Example
{
    public class DeleteExampleCommandHandler : IRequestHandler<DeleteExampleRequest, DeleteExampleResponse>
    {
        private readonly IDeleteExample deleteExample;

        public DeleteExampleCommandHandler(IDeleteExample deleteExample)
        {
            this.deleteExample = deleteExample;
        }

        public async Task<DeleteExampleResponse> Handle(DeleteExampleRequest request, CancellationToken cancellationToken)
        {
            await DeleteExample(request.ExampleId);

            return new DeleteExampleResponse() { ExampleId = request.ExampleId };
        }

        private async Task DeleteExample(int id)
        {
            await this.deleteExample.ExecuteAsync(id).ConfigureAwait(false);
        }
    }
}
