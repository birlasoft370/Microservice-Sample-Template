// Copyright © CompanyName. All Rights Reserved.
using Example.Api.Controllers;
using Example.DomainModel.Example;
using FizzWare.NBuilder;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using Xunit;

namespace Example.Api.UnitTest.Controller
{
    public class ExampleControllerUnitTests
    {
        private readonly Mock<IMediator> mediatorMoq;
        private readonly ExampleController controller;
        public ExampleControllerUnitTests()
        {
            mediatorMoq = new Mock<IMediator>();
            controller = new ExampleController(mediatorMoq.Object);
        }

        [Fact]
        public async Task GetExamples_ShouldReturn200Status()
        {
            //Act
            var result = (OkObjectResult)await controller.GetExamples();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetExampleById_ValidExampleId_ShouldReturn200Status()
        {
            //Arrange
            mediatorMoq.Setup(m => m.Send(It.IsAny<GetExampleByIdRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetExampleByIdResponse() { ExampleModel = new ExampleModel() { ExampleId = 1, ExampleName = "test" } });

            //Act
            var actualresult = (OkObjectResult)await controller.GetExampleById(new GetExampleByIdRequest() { ExampleId = 1 });

            //Assert
            actualresult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddExample_ValidRequest_ShouldReturn200Status()
        {
            //Arrange
            ExampleModel exampleModel = Builder<ExampleModel>.CreateNew().Build();
            AddExampleRequest request = new() { ExampleModel = exampleModel };
            AddExampleResponse response = new() { ExampleModel = exampleModel };
            mediatorMoq.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);
            //mediatorMoq.Setup(m => m.Send(It.IsAny<AddExampleRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actualresult = (OkObjectResult)await controller.AddExample(request);

            //Assert
            actualresult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task UpdateExample_ValidRequest_ShouldReturn200Status()
        {
            //Arrange
            ExampleModel exampleModel = Builder<ExampleModel>.CreateNew().Build();
            UpdateExampleRequest request = new() { ExampleModel = exampleModel };
            UpdateExampleResponse response = new() { ExampleModel = exampleModel };
            mediatorMoq.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actualresult = (OkObjectResult)await controller.UpdateExample(request);

            //Assert
            actualresult.StatusCode.Should().Be(200);

        }

        [Fact]
        public async Task AddUpdateExample_ValidRequest_ShouldReturn200Status()
        {
            //Arrange
            int UpdateExampleId = 1;
            ExampleModel exampleModel = Builder<ExampleModel>.CreateNew().Build();
            AddUpdateExampleRequest request = new() { UpdateExampleId = UpdateExampleId, ExampleModel = exampleModel };

            List<ExampleModel> exampleModels = (List<ExampleModel>)Builder<ExampleModel>.CreateListOfSize(2).Build();
            AddUpdateExampleResponse response = new() { ExampleModels = exampleModels };
            mediatorMoq.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actualresult = (OkObjectResult)await controller.AddUpdateExample(request);

            //Assert
            actualresult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeleteExampleRequest_ValidExampleId_ShouldReturn200Status()
        {
            //Arrange
            DeleteExampleRequest request = Builder<DeleteExampleRequest>.CreateNew().Build();
            DeleteExampleResponse response = Builder<DeleteExampleResponse>.CreateNew().Build();
            mediatorMoq.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actualresult = (OkObjectResult)await controller.DeleteExample(request);

            //Assert
            actualresult.StatusCode.Should().Be(200);
        }


        [Fact]
        public async Task BulkInsertExample_ValidRequest_ShouldReturn200Status()
        {
            //Arrange
            BulkInsertExampleModel bulkInsert = Builder<BulkInsertExampleModel>.CreateNew().Build();
            bulkInsert.UploadedFile = GetFileMock("text/csv", "test;test;");
            BulkInsertExampleRequest request = new() { BulkInsertExampleModel = bulkInsert };

            List<ExampleModel> exampleModels = (List<ExampleModel>)Builder<ExampleModel>.CreateListOfSize(2).Build();
            BulkInsertExampleResponse response = new() { ExampleModels = exampleModels };
            mediatorMoq.Setup(x => x.Send(request, It.IsAny<CancellationToken>())).ReturnsAsync(response);

            //Act
            var actualresult = (OkObjectResult)await controller.BulkInsertExample(request);

            //Assert
            actualresult.StatusCode.Should().Be(200);
        }

        private static IFormFile GetFileMock(string contentType, string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: "dummy.csv"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return file;
        }
    }
}
