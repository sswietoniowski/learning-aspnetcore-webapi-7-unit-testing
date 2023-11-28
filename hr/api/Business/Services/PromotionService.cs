using System.Net.Http.Headers;
using System.Text.Json;
using Hr.Api.DataAccess.Entities;
using Hr.Api.DataAccess.Repositories;
using Hr.Api.Dtos;

namespace Hr.Api.Business.Services;

public class PromotionService : IPromotionService
{
    private readonly IHrRepository _repository;
    private readonly HttpClient _httpClient;

    public PromotionService(IHrRepository repository, IHttpClientFactory httpClientFactory)
        : this(repository, httpClientFactory.CreateClient("MyCustomClient"))
    {
    }

    private PromotionService(IHrRepository repository, HttpClient httpClient)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Promote an internal employee if eligible for promotion
    /// </summary>
    /// <param name="employee"></param>
    /// <returns></returns>
    public async Task<bool> PromoteInternalEmployeeAsync(InternalEmployee employee)
    {
        if (await CheckIfInternalEmployeeIsEligibleForPromotion(employee.Id))
        {
            employee.JobLevel++;
            await _repository.SaveChangesAsync();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Calls into external API (containing a data source only
    /// the top level managers can manage) to check whether
    /// an internal employee is eligible for promotion
    /// </summary> 
    private async Task<bool> CheckIfInternalEmployeeIsEligibleForPromotion(Guid employeeId)
    {
        // call into API
        var request = new HttpRequestMessage(HttpMethod.Get,
            $"api/promotions/{employeeId}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // deserialize content
        var content = await response.Content.ReadAsStringAsync();
        var promotionEligibilityDto = JsonSerializer.Deserialize<PromotionEligibilityDto>(content,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        // return value
        return promotionEligibilityDto == null ? false : promotionEligibilityDto.EligibleForPromotion;
    }
}