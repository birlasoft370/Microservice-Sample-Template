// Copyright © CompanyName. All Rights Reserved.

namespace Example.DomainModel.Example
{
    public class BulkInsertExampleResponse
    {
        public BulkInsertExampleResponse()
        {
            ExampleModels = new List<ExampleModel>();

        }
        public List<ExampleModel> ExampleModels { get; set; }
    }
}
