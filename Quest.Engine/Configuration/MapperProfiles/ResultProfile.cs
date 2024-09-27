using AutoMapper;
using Quest.Infrastructure.Helper;
using Quest.Infrastructure.Models;

namespace Quest.Engine.Configuration.MapperProfiles;
public class ResultProfile : Profile
{
    public ResultProfile()
    {
        CreateMap<Result, ResultDto>()
            .ForMember(dest => dest.NumberOfComponents, opt => opt.MapFrom(src => src.NumberOfComponents))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName));

    }
}
