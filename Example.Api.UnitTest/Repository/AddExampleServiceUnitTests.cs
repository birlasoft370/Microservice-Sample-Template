// Copyright © CompanyName. All Rights Reserved.
using Example.Common.Exceptions;
using Example.DataTransfer.Examples;
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.Example;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.Repository
{
    public class AddExampleServiceUnitTests
    {
        private readonly Mock<IExampleOperations> exampleOperationsMoq;
        public AddExampleServiceUnitTests()
        {
            exampleOperationsMoq = new Mock<IExampleOperations>();
        }

        [Fact]
        public async Task ValidExample_Add_ShouldReturn_Valid_Response()
        {
            /// Arrange
            var sut = new AddExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            request.ExampleId = 0;

            var response = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName, request.ExampleId)).ReturnsAsync(false);
            exampleOperationsMoq.Setup(m => m.AddAsync(request)).ReturnsAsync(response);

            /// Act
            var actualresult = await sut.ExecuteAsync(request);

            /// Assert
            actualresult.Should().BeOfType<ExampleDto>();

            actualresult.ExampleId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task InValidExample_Add_ShouldReturn_InValid_Response()
        {
            /// Arrange
            var sut = new AddExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            request.ExampleId = 0;

            var response = Builder<ExampleDto>.CreateNew().Build();
            response.ExampleId = 0;
            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName, request.ExampleId)).ReturnsAsync(false);
            exampleOperationsMoq.Setup(m => m.AddAsync(request)).ReturnsAsync(response);

            /// Act
            var actualresult = await sut.ExecuteAsync(request);

            /// Assert
            actualresult.Should().BeOfType<ExampleDto>();

            actualresult.ExampleId.Should().Be(request.ExampleId);
        }

        [Fact]
        public async Task ValidExample_AddDuplicate_ShouldThrow_DuplicateException()
        {
            /// Arrange
            var sut = new AddExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName, request.ExampleId)).Throws(new DuplicateRecordException("ExampleName.DuplicateKey"));

            /// Act 
            Func<Task> act = (async () => await sut.ExecuteAsync(request));

            // Assert
            // await Assert.ThrowsAsync<DuplicateRecordException>(async () => await sut.AddAsync(request));
            await act.Should().ThrowExactlyAsync<DuplicateRecordException>();
        }

        [Fact]
        public async Task ValidExample_AddDuplicate_ShouldThrow_DuplicateException_WithFrameworkDefinedMessage()
        {

            /// Arrange
            var sut = new AddExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName, request.ExampleId)).Throws(new DuplicateRecordException($"ExampleName.{request.ExampleName} already exist"));

            // Act & Assert
            // DuplicateRecordException ex = await Assert.ThrowsAsync<DuplicateRecordException>(() => sut.AddAsync(request));

            //Act
            Func<Task> act = (async () => await sut.ExecuteAsync(request));

            // Assert
            await act.Should().ThrowExactlyAsync<DuplicateRecordException>().WithMessage($"ExampleName.{request.ExampleName} already exist");
        }
    }
}
