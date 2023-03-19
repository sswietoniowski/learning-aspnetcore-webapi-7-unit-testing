using AutoMapper;
using Hr.Api.Dtos;
using Microsoft.AspNetCore.Http.Features;

namespace Hr.Api.Configurations.Mapper;

public class StatisticProfile : Profile
{
    public StatisticProfile()
    {
        CreateMap<IHttpConnectionFeature, StatisticDto>();
    }
}