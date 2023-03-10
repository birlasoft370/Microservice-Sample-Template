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
    public class UpdateExampleServiceUnitTests
    {
        private readonly Mock<IExampleOperations> exampleOperationsMoq;
        public UpdateExampleServiceUnitTests()
        {
            exampleOperationsMoq = new Mock<IExampleOperations>();
        }

        [Fact]
        public async Task ValidExample_Update_ShouldReturn_Valid_Response()
        {
            /// Arrange
            var sut = new UpdateExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            var response = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.GetByIdAsync(request.ExampleId)).ReturnsAsync(response);
            exampleOperationsMoq.Setup(m => m.UpdateAsync(request));

            /// Act
            await sut.ExecuteAsync(request);

            /// Assert
            request.LastModifiedBy.Should().Be(response.LastModifiedBy);
        }

        [Fact]
        public async Task InValidExample_Update_ShouldReturn_Null_Response()
        {
            /// Arrange
            var sut = new UpdateExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.GetExampleByExampleIdAsync(request.ExampleId)).Returns(Task.FromResult<ExampleDto>(null));

            /// Act
            await sut.ExecuteAsync(request);

            /// Assert
            //ExampleOperationsMoq.Verify(x => x.UpdateAsync(request));
            request.ExampleId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ValidExample_UpdateDuplicate_ShouldThrow_DuplicateException()
        {
            /// Arrange
            var sut = new UpdateExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            var response = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.GetExampleByExampleIdAsync(request.ExampleId)).ReturnsAsync(response);
            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName,request.ExampleId)).Throws(new DuplicateRecordException("Examplename.DuplicateKey"));

            /// Act & Assert
            await Assert.ThrowsAsync<DuplicateRecordException>(() => sut.ExecuteAsync(request));
        }

        [Fact]
        public async Task ValidExample_UpdateDuplicate_ShouldThrow_DuplicateException_WithFrameworkDefinedMessage()
        {

            /// Arrange
            var sut = new UpdateExample(exampleOperationsMoq.Object);
            var request = Builder<ExampleDto>.CreateNew().Build();
            var response = Builder<ExampleDto>.CreateNew().Build();
            exampleOperationsMoq.Setup(m => m.GetExampleByExampleIdAsync(request.ExampleId)).ReturnsAsync(response);

            exampleOperationsMoq.Setup(m => m.DuplicateCheckExample(request.ExampleName,request.ExampleId)).Throws(new DuplicateRecordException("Examplename.DuplicateKey"));

            // Act & Assert
            // DuplicateRecordException ex = await Assert.ThrowsAsync<DuplicateRecordException>(() => sut.AddAsync(request));

            //Act
            Func<Task> act = (async () => await sut.ExecuteAsync(request));

            // Assert
            await act.Should().ThrowExactlyAsync<DuplicateRecordException>().WithMessage("Examplename.DuplicateKey");
        }
    }
}
