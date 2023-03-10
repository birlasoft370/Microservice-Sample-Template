// Copyright © CompanyName. All Rights Reserved.
using Example.Repository.Example;
using Example.RepositoryHandler.MsSql.EF.Example;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.Repository
{
    public class DeleteExampleServiceUnitTests
    {
        private readonly Mock<IExampleOperations> exampleOperationsMoq;
        public DeleteExampleServiceUnitTests()
        {
            exampleOperationsMoq = new Mock<IExampleOperations>();
        }


        [Fact]
        public async Task ValidExampleId_ShouldCall_DeleteExamplService_Return_ValidResponse()
        {
            //Arrange
            int exampleId = 2;
            DeleteExample example = new DeleteExample(exampleOperationsMoq.Object);
            exampleOperationsMoq.Setup(x => x.DeleteExampleByExampleIdAsync(exampleId)).Returns(Task.FromResult(exampleId));
            //act
            await example.ExecuteAsync(exampleId);

            Assert.True(exampleId > 0);
        }

    }
}
