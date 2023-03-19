namespace Hr.Api.Configurations.Extensions;

using Hr.Api.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderAddPersistenceExtension
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<HrDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("HrDb")));

        // TODO: add proper repositories & services
        // builder.Services.AddScoped<IBooksRepository, BooksRepository>();
        // builder.Services.AddScoped<IBooksService, BooksService>();

        return builder;
    }
}
