// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.Common.Exceptions;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.QueryHandler;
using Example.QueryHandler.Example;
using Example.Repository.Examples;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.QueryHandler
{
    public class GetExampleByIdQueryHandlerUnitTests
    {
        private readonly Mock<IGetExampleById> getExampleByIdMoq;
        private readonly IMapper mapper;

        public GetExampleByIdQueryHandlerUnitTests()
        {
            getExampleByIdMoq = new Mock<IGetExampleById>();
            if (mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper _mapper = mappingConfig.CreateMapper();
                mapper = _mapper;
            }
        }

        [Fact]
        public async Task GetExampleById_ValidExampleId_ShouldReturnValidExample()
        {
            /// Arrange
            var request = Builder<GetExampleByIdRequest>.CreateNew().Build();
            var ExampleDtorequest = Builder<ExampleDto>.CreateNew().Build();

            var sut = new GetExampleByIdQueryHandler(getExampleByIdMoq.Object, mapper);
            getExampleByIdMoq.Setup(m => m.ExecuteAsync(request.ExampleId)).ReturnsAsync(ExampleDtorequest);

            /// Act
            var actualResult = await sut.Handle(request, It.IsAny<CancellationToken>());

            /// Assert
            actualResult.ExampleModel.ExampleId.Should().Be(request.ExampleId);
        }

        [Fact]
        public async Task GetExampleById_InValidExampleId_ShouldReturnNullResponse()
        {
            // Arrange
            var request = Builder<GetExampleByIdRequest>.CreateNew().Build();
            var ExampleDtorequest = await Task.FromResult<ExampleDto>(null);

            var sut = new GetExampleByIdQueryHandler(getExampleByIdMoq.Object, mapper);
            getExampleByIdMoq.Setup(m => m.ExecuteAsync(request.ExampleId)).ReturnsAsync(ExampleDtorequest);

            // Act
            async Task act() => await sut.Handle(request, CancellationToken.None);

            //Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
    }
}
