using AutoMapper;
using Hr.Api.DataAccess.Entities;
using Hr.Api.Dtos;

namespace Hr.Api.Configurations.Mapper;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<InternalEmployee, InternalEmployeeDto>();
    }
}