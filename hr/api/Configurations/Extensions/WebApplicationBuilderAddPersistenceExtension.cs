namespace Hr.Api.Configurations.Extensions;

using Hr.Api.Business.Services;
using Hr.Api.DataAccess;
using Hr.Api.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderAddPersistenceExtension
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<HrDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("HrDb")));

        builder.Services.AddScoped<IHrRepository, HrRepository>();
        builder.Services.AddScoped<EmployeeFactory>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();
        builder.Services.AddScoped<IPromotionService>(x =>
        {
            var hrRepository = x.GetRequiredService<IHrRepository>();
            var httpClientFactory = x.GetRequiredService<IHttpClientFactory>();

            var promotionService = new PromotionService(hrRepository, httpClientFactory);
            return promotionService;
        });

        return builder;
    }
}
