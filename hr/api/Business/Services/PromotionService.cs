using System.Net.Http.Headers;
using System.Text.Json;
using Hr.Api.DataAccess.Entities;
using Hr.Api.DataAccess.Repositories;

namespace Hr.Api.Business.Services;

public class PromotionService : IPromotionService
{
    private readonly IHrRepository _repository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PromotionService> _logger;

    public PromotionService(IHrRepository repository, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<PromotionService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger;
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
        var httpClient = _httpClientFactory.CreateClient();
        var externalApiBaseUrl = _configuration["ExternalApiBaseUrl"];

        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{externalApiBaseUrl}/api/promotions/{employeeId}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        // deserialize content
        var content = await response.Content.ReadAsStringAsync();
        var promotionEligibility = JsonSerializer.Deserialize<PromotionEligibility>(content,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        // return value
        return promotionEligibility == null ? false : promotionEligibility.EligibleForPromotion;
    }
}