// Copyright © CompanyName. All Rights Reserved.
using MediatR;

namespace Example.DomainModel.Example
{
    public class GetExampleByIdRequest : IRequest<GetExampleByIdResponse>
    {
        public int ExampleId { get; set; }
    }
}
