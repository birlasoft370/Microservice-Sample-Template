using AutoMapper;
using Example.DataTransfer.Examples;
using Example.DomainModel.Example;

namespace Example.QueryHandler
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<ExampleModel, ExampleDto>().ReverseMap();
            CreateMap<ExampleDto, ExampleModel>().ReverseMap();
            // CreateMap<ExampleModel, ExampleDto>().ForMember(dest => dest.ExampleId, opt => opt.MapFrom(src => src.ExampleId));
            // CreateMap<ExampleDto, ExampleModel>().ForMember(dest => dest.ExampleId, opt => opt.MapFrom(src => src.ExampleId));
        }
    }
}
