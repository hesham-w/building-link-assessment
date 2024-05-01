using Api.Common.Extensions;
using Api.Repositories;
using FluentValidation;

namespace Api;

public class ActivateDriver
{
    public record Request(int DriverId);

    public record Response(int DriverId, string Name, string AddressLine1, string AddressLine2, string PhoneNumber);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/driver/activate", Handle)
        .WithRequestValidation<Request>();

    public class ActivateDriverRequestValidator : AbstractValidator<Request>
    {
        public ActivateDriverRequestValidator()
        {
            RuleFor(x => x.DriverId)
                .NotEmpty();
        }
    }

    public static async Task<IResult> Handle(Request request, IDriverRepository repository)
    {
        var driver = await repository.GetById(request.DriverId);

        if (driver == null)
        {
            return TypedResults.NotFound("Driver not found");
        }

        driver.Activate();

        var savedDriver = await repository.Update(driver);

        return TypedResults.Ok(new Response(savedDriver.Id, savedDriver.Name, savedDriver.AddressLine1, savedDriver.AddressLine2, savedDriver.PhoneNumber));
    }
}
