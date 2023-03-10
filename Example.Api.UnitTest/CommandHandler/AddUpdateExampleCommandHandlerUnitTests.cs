// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.CommandHandler.Example;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.QueryHandler;
using Example.Repository.Example;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.CommandHandler
{
    public class AddUpdateExampleCommandHandlerUnitTests
    {
        private readonly Mock<IAddUpdateExample> addUpdateExampleMoq;
        private readonly IMapper mapper;
        private readonly AddUpdateExampleCommandHandler handler;
        public AddUpdateExampleCommandHandlerUnitTests()
        {
            addUpdateExampleMoq = new Mock<IAddUpdateExample>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            mapper = mapperConfig.CreateMapper();
            handler = new AddUpdateExampleCommandHandler(addUpdateExampleMoq.Object, mapper);
        }

        [Fact]
        public async Task AddUpdateExample_ValidRequest_ShouldReturn_ValidResponse()
        {
            //Arrange
            int updateExampleId = 1;
            ExampleModel exampleModel = Builder<ExampleModel>.CreateNew().Build();
            AddUpdateExampleRequest request = new() { UpdateExampleId = updateExampleId, ExampleModel = exampleModel };

            ExampleDto exampleDto = Builder<ExampleDto>.CreateNew().Build();
            List<ExampleDto> response = (List<ExampleDto>)Builder<ExampleDto>.CreateListOfSize(2).Build();

            addUpdateExampleMoq.Setup(a => a.ExecuteAsync(updateExampleId, It.IsAny<ExampleDto>())).ReturnsAsync(response);

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeOfType<AddUpdateExampleResponse>();
            result.ExampleModels.Should().HaveCount(response.Count);
        }

        [Fact]
        public async Task AddUpdateExample_InValidRequest_ShouldReturn_EmptyResponse()
        {
            //Arrange
            int updateExampleId = 1;
            ExampleModel exampleModel = Builder<ExampleModel>.CreateNew().Build();
            AddUpdateExampleRequest request = new() { UpdateExampleId = updateExampleId, ExampleModel = exampleModel };
            List<ExampleDto> response = new();
            addUpdateExampleMoq.Setup(a => a.ExecuteAsync(updateExampleId, It.IsAny<ExampleDto>())).ReturnsAsync(response);

            //Act
            var result = await handler.Handle(request, CancellationToken.None);

            //Assert
            result.Should().BeOfType<AddUpdateExampleResponse>();
            result.ExampleModels.Should().HaveCount(response.Count);
        }
    }
}
