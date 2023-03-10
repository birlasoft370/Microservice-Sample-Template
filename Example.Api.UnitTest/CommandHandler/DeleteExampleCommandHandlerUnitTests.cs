// Copyright © CompanyName. All Rights Reserved.
using Example.CommandHandler.Example;
using Example.DomainModel.Example;
using Example.Repository.Example;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.CommandHandler
{
    public class DeleteExampleCommandHandlerUnitTests
    {
        private readonly Mock<IDeleteExample> deleteExampleMoq;
        private readonly DeleteExampleCommandHandler handler;

        public DeleteExampleCommandHandlerUnitTests()
        {
            deleteExampleMoq = new Mock<IDeleteExample>();

            handler = new DeleteExampleCommandHandler(deleteExampleMoq.Object);
        }

        [Fact]
        public async Task Valid_ExampleId_Deleted_Should_Return_Valid_Response()
        {
            //arrange
            DeleteExampleRequest request = new() { ExampleId = 1 };

            //Service Mock
            deleteExampleMoq.Setup(m => m.ExecuteAsync(request.ExampleId));

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<DeleteExampleResponse>();

            result.ExampleId.Should().Be(request.ExampleId);
        }

        [Fact]
        public async Task InValid_ExampleId_Deleted_Should_Return_InValid_Response()
        {
            //arrange
            int expected = 2;
            DeleteExampleRequest request = new () { ExampleId = 1 };

            deleteExampleMoq.Setup(m => m.ExecuteAsync(request.ExampleId));

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<DeleteExampleResponse>();

            result.ExampleId.Should().NotBe(expected);
        }
    }
}
