// Copyright © CompanyName. All Rights Reserved.
using MediatR;

namespace Example.DomainModel.Example
{
    public class AddExampleRequest:IRequest<AddExampleResponse>
    {
        public ExampleModel ExampleModel { get; set; }
    }
}
