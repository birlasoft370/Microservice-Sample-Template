﻿// Copyright © CompanyName. All Rights Reserved.
using MediatR;

namespace Example.DomainModel.Example
{
    public class AddUpdateExampleRequest : IRequest<AddUpdateExampleResponse>
    {
        public int UpdateExampleId { get; set; }    
        public ExampleModel ExampleModel { get; set; }
    }
}
