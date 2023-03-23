using AutoMapper;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Http.Features;

namespace Hr.Api.Configurations.Mapper;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<IHttpConnectionFeature, StatisticsDto>();
    }
}