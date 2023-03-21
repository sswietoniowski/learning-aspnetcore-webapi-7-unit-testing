using Hr.Api.Configurations.Extensions;
using Hr.Api.DataAccess;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.AddPersistence();
builder.AddMapper();
builder.AddHttpClient();
builder.AddSwagger();
builder.AddGlobalErrorHandler();

builder.Services.AddControllers();

var app = builder.Build();

app.UseGlobalErrorHandler();
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await UpdateDatabase(app);

app.Run();

static async Task UpdateDatabase(WebApplication app)
{
    const int MAX_RETRIES = 3;
    const int RETRY_DELAY_IN_SECONDS = 5;
    var retryPolicy = Policy.Handle<Exception>()
        .WaitAndRetryAsync(retryCount: MAX_RETRIES,
            sleepDurationProvider: attemptCount => TimeSpan.FromSeconds(RETRY_DELAY_IN_SECONDS));

    await retryPolicy.ExecuteAsync(async () =>
    {
        await app.SeedAsync();
    });
}