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
    public class AddExampleCommandHandlerUnitTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IAddExample> addExample;
        private readonly AddExampleCommandHandler _handler;

        public AddExampleCommandHandlerUnitTests()
        {
            addExample = new Mock<IAddExample>();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
            _handler = new AddExampleCommandHandler(addExample.Object, _mapper);
        }

        [Fact]
        public async Task Valid_Example_Added_Should_Return_Valid_Response()
        {
            //arrange
            var exampleModel = Builder<ExampleModel>.CreateNew().Build();
            AddExampleRequest request = new() { ExampleModel = exampleModel };

            //Service Mock
            var response = Builder<ExampleDto>.CreateNew().Build();
            addExample.Setup(m => m.ExecuteAsync(It.IsAny<ExampleDto>())).ReturnsAsync(response);

            //act
            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<AddExampleResponse>();

            result.ExampleModel.ExampleId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task InValid_Example_Added_Should_Return_InValid_Response()
        {
            //arrange
            var exampleModel = Builder<ExampleModel>.CreateNew().Build();
            exampleModel.ExampleId = 0;

            AddExampleRequest request = new() { ExampleModel = exampleModel };

            var response = Builder<ExampleDto>.CreateNew().Build();
            response.ExampleId = 0;
            addExample.Setup(m => m.ExecuteAsync(It.IsAny<ExampleDto>())).ReturnsAsync(response);

            //act
            var result = await _handler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeOfType<AddExampleResponse>();

            result.ExampleModel.ExampleId.Should().Be(0);
        }

    }
}
