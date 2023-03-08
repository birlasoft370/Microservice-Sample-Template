// Copyright © CompanyName. All Rights Reserved.
namespace Example.DomainModel.Example
{
    public class AddUpdateExampleResponse
    {
        public AddUpdateExampleResponse()
        {
            ExampleModels = new List<ExampleModel>();
        }
        public List<ExampleModel> ExampleModels { get; set; }
    }
}
