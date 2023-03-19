using Hr.Api.DataAccess.Entities;

namespace Hr.Api.Business.Services;

public interface IPromotionService
{
    Task<bool> PromoteInternalEmployeeAsync(InternalEmployee employee);
}