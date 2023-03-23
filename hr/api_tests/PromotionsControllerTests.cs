﻿using Hr.Api.Business.Services;
using Hr.Api.Controllers;
using Hr.Api.DataAccess.Entities;
using Hr.Api.Dtos;
using Hr.Api.Tests.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Text;
using System.Text.Json;

namespace Hr.Api.Tests;

public class PromotionsControllerTests
{
    [Fact]
    public async Task CreatePromotion_RequestPromotionForEligibleEmployee_MustPromoteEmployee()
    {
        // Arrange 
        var expectedEmployeeId = Guid.NewGuid();
        var currentJobLevel = 1;
        var promotionForCreationDto = new PromotionForCreationDto()
        {
            EmployeeId = expectedEmployeeId
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock
            .Setup(m => m.GetInternalEmployeeByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(
                new InternalEmployee(
                    "Anna", "Johnson", 3, 3400, true, currentJobLevel)
                {
                    Id = expectedEmployeeId,
                    SuggestedBonus = 500
                });

        var eligibleForPromotionHandlerMock = new Mock<HttpMessageHandler>();
        eligibleForPromotionHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(
                        new PromotionEligibilityDto() { EligibleForPromotion = true },
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        }),
                    Encoding.ASCII,
                    "application/json")
            });

        var httpClient = new HttpClient(eligibleForPromotionHandlerMock.Object);
        httpClient.BaseAddress = new Uri("http://localhost:5003");
        var promotionService = new PromotionService(
            new HrTestDataRepository(), httpClient);

        var promotionsController = new PromotionsController(
            employeeServiceMock.Object, promotionService, default!);

        // Act
        var result = await promotionsController
            .CreatePromotion(promotionForCreationDto);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var promotionResultDto = Assert.IsType<PromotionDto>(
            okObjectResult.Value);
        Assert.Equal(expectedEmployeeId, promotionResultDto.EmployeeId);
        Assert.Equal(++currentJobLevel, promotionResultDto.JobLevel);
    }
}