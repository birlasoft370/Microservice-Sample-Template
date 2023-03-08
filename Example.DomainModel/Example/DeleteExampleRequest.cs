// Copyright © CompanyName. All Rights Reserved.
using MediatR;


namespace Example.DomainModel.Example
{
    public class DeleteExampleRequest : IRequest<DeleteExampleResponse>
    {
        public int ExampleId { get; set; }
    }
}
