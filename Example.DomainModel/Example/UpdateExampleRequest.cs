// Copyright © CompanyName. All Rights Reserved.
using MediatR;

namespace Example.DomainModel.Example
{
    public class UpdateExampleRequest : IRequest<UpdateExampleResponse>
    {
        public ExampleModel ExampleModel { get; set; }
    }
}
