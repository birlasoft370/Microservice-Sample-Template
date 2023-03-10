// Copyright © CompanyName. All Rights Reserved.
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.Example;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Admin.Api.UnitTest.Services
{
    public class GetExampleByIdServiceUnitTests
    {
        private readonly Mock<IExampleOperations> exampleOperationsMoq;

        public GetExampleByIdServiceUnitTests()
        {
            exampleOperationsMoq = new Mock<IExampleOperations>();
        }

        [Fact]
        public async Task GetExampleWithValidId_ShouldCall_GetExampleService_ReturnIdSpecificExample()
        {
            ///Arrange
            int exampleId = 1;

            ExampleDto ExamplebyIdGetModelobj = Builder<ExampleDto>.CreateNew().Build();

            var expectedResult = Task.FromResult(ExamplebyIdGetModelobj);

            exampleOperationsMoq.Setup(x => x.GetByIdAsync(exampleId)).Returns(expectedResult);

            ///Act 
            var GetExampleByIdObj = new GetExampleById(exampleOperationsMoq.Object);
            var actualResult = await GetExampleByIdObj.ExecuteAsync(exampleId);

            /// Assert
            actualResult.Should().BeSameAs(expectedResult.Result, "Invalid Example Id");
        }

        [Fact]
        public async Task GetExampleWithInValidId_ShouldCall_GetExampleService_ReturnEmptyExample()
        {
            ///Arrange
            int ExampleId = 0;

            var expectedResult = Task.FromResult(new ExampleDto());

            exampleOperationsMoq.Setup(x => x.GetByIdAsync(ExampleId)).Returns(expectedResult);

            ///Act 
            var GetExampleByIdObj = new GetExampleById(exampleOperationsMoq.Object);
            var actualResult = await GetExampleByIdObj.ExecuteAsync(ExampleId);

            /// Assert
            actualResult.Should().BeSameAs(expectedResult.Result, "Invalid Example Id");
        }
    }
}
