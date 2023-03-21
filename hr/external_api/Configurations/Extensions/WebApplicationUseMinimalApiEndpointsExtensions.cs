using Management.Api.Dtos;

namespace Management.Api.Configurations.Extensions;

public static class WebApplicationUseMinimalApiEndpointsExtensions
{
    public static void UseMinimalApiEndpoints(this WebApplication app)
    {
        app.MapGet("api/promotions/{employeeId:guid}", (Guid employeeId) =>
        {
            var promotionEligibility = new PromotionEligibilityDto { EligibleForPromotion = false };

            // For demo purposes, Megan (id = 72f2f5fe-e50c-4966-8420-d50258aefdcb)
            // is eligible for promotion, other employees aren't
            if (employeeId == Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"))
            {
                promotionEligibility.EligibleForPromotion = true;
            }

            return Results.Ok(promotionEligibility);
        })
            .WithName("GetPromotions")
            .Produces<PromotionEligibilityDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}