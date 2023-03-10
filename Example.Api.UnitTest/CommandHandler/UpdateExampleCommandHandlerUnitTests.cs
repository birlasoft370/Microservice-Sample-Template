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
    public class UpdateExampleCommandHandlerUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IUpdateExample> updateExample;
        private readonly UpdateExampleCommandHandler _handler;

        public UpdateExampleCommandHandlerUnitTests()
        {
            updateExample = new Mock<IUpdateExample>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new UpdateExampleCommandHandler(updateExample.Object, _mapper);
        }

        [Fact]
        public async Task Valid_Example_Update_Should_Return_Valid_Response()
        {
            //arrange
            var exampleModel = Builder<ExampleModel>.CreateNew().Build();
            UpdateExampleRequest request = new() { ExampleModel = exampleModel };

            //Service Mock
            var response = Builder<ExampleDto>.CreateNew().Build();
            updateExample.Setup(m => m.ExecuteAsync(It.IsAny<ExampleDto>()));

            //act
            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<UpdateExampleResponse>();

            result.ExampleModel.ExampleId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task InValid_Example_Update_Should_Return_InValid_Response()
        {
            //arrange
            var exampleModel = Builder<ExampleModel>.CreateNew().Build();
            exampleModel.ExampleId = 0;

            UpdateExampleRequest request = new() { ExampleModel = exampleModel };

            var response = Builder<ExampleDto>.CreateNew().Build();
            response.ExampleId = 0;
            updateExample.Setup(m => m.ExecuteAsync(It.IsAny<ExampleDto>()));

            //act
            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<UpdateExampleResponse>();

            result.ExampleModel.ExampleId.Should().Be(0);
        }
    }
}
