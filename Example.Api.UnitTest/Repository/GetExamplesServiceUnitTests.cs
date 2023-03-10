// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.Example;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.Repository
{
    public class GetExamplesServiceUnitTests
    {

        private readonly Mock<IExampleOperations> exampleOperationsMoq;

        public GetExamplesServiceUnitTests()
        {
            exampleOperationsMoq = new Mock<IExampleOperations>();
        }

        [Fact]
        public async Task GetExamples_ShouldCall_GetExamplesService_ReturnCollectionOfExample()
        {
            //Arrange
            var getServicesObj = new GetExamples(exampleOperationsMoq.Object);
            List<ExampleDto> exampleDtos = (List<ExampleDto>)Builder<ExampleDto>.CreateListOfSize(2).Build();
            exampleOperationsMoq.Setup(x => x.GetAllAsync()).ReturnsAsync(exampleDtos);

            //Act
            var actualResult = await getServicesObj.ExecuteAsync();

            // Assert
            actualResult.Should().HaveCount(exampleDtos.Count, "Expected Result not matched with Actual Result");
        }

        [Fact]
        public async Task GetExamples_ShouldCall_GetExamplesService_ReturnEmpty()
        {
            //Arrange
            var getServicesObj = new GetExamples(exampleOperationsMoq.Object);
            List<ExampleDto> exampleDtos = new();
            exampleOperationsMoq.Setup(x => x.GetAllAsync()).ReturnsAsync(exampleDtos);

            //Act
            var actualResult = await getServicesObj.ExecuteAsync();

            // Assert
            actualResult.Should().HaveCount(exampleDtos.Count, "Expected Result not matched with Actual Result");
        }
    }
}
