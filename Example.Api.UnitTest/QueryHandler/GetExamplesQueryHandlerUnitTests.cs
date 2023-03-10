// Copyright © CompanyName. All Rights Reserved.
using AutoMapper;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;
using Example.QueryHandler;
using Example.QueryHandler.Examples;
using Example.Repository.Examples;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace Example.Api.UnitTest.QueryHandler
{
    public class GetExamplesQueryHandlerUnitTests
    {
        private readonly Mock<IGetExamples> getExamplesMoq;
        private readonly IMapper mapper;
        public GetExamplesQueryHandlerUnitTests()
        {
            getExamplesMoq = new Mock<IGetExamples>();

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
        public async Task GetAllExamples_ShouldReturnCollectionOfExample()
        {
            ///Arrange
            var request = Builder<GetExamplesRequest>.CreateNew().Build();

            List<ExampleDto> actionDtos = (List<ExampleDto>)Builder<ExampleDto>.CreateListOfSize(2).Build().AsEnumerable();

            Task<IEnumerable<ExampleDto>> exampleDtosEnumerableResult = Task.FromResult(actionDtos.AsEnumerable());

            var expectedResult = await Task.FromResult(exampleDtosEnumerableResult);

            getExamplesMoq.Setup(x => x.ExecuteAsync()).Returns(expectedResult);

            ///Act
            var result = new GetExamplesQueryHandler(getExamplesMoq.Object, mapper);
            var actualResult = await result.Handle(request, It.IsAny<CancellationToken>());

            /// Assert
            actualResult.ExamplesModel.Should().HaveCount(expectedResult.Result.Count(), "Expected Result not matched with Actual Result");
        }

        [Fact]
        public async Task GetAllExamples_ShouldReturnEmptyResult()
        {
            ///Arrange
            var request = Builder<GetExamplesRequest>.CreateNew().Build();
            var expectedResult = Task.FromResult(Enumerable.Empty<ExampleDto>());
            getExamplesMoq.Setup(x => x.ExecuteAsync()).Returns(expectedResult);

            ///Act
            var ExamplesQueryHandlerobj = new GetExamplesQueryHandler(getExamplesMoq.Object, mapper);
            var actualResult = await ExamplesQueryHandlerobj.Handle(request, It.IsAny<CancellationToken>());

            ///Asset
            actualResult.ExamplesModel.Should().HaveCount(expectedResult.Result.Count(), "Expected Result not matched with Actual Result");
        }
    }
}
