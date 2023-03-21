using Management.Api.Configurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.AddSwagger();
builder.AddGlobalErrorHandler();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseGlobalErrorHandler();
app.UseSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMinimalApiEndpoints();

app.Run();
