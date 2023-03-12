FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5001

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_HTTPS_PORT=5001

RUN mkdir /https
COPY ["./https/Hr.pfx", "/https"]

ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Hr.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=P@ssw0rd
ENV ASPNETCORE_Kestrel__Certificates__Development__Path=/https/Hr.pfx
ENV ASPNETCORE_Kestrel__Certificates__Development__Password=P@ssw0rd

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app && chown -R appuser /https
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Hr.Api.csproj", "./"]
RUN dotnet restore "./Hr.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Hr.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hr.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hr.Api.dll"]
