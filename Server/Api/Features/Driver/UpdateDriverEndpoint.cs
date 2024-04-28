using Api.Common;
using Api.Common.Extensions;
using Api.Repositories;
using FluentValidation;

namespace Api.Features.Driver;

public class UpdateDriverEndpoint : IEndpoint
{
    public record Request(int Id, string Name, string AddressLine1, string AddressLine2, string PhoneNumber);

    public record Response(int Id, string Name, string AddressLine1, string AddressLine2, string PhoneNumber);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/", Handle)
        .WithRequestValidation<Request>();

    public class CreateDriverRequestValidator : AbstractValidator<Request>
    {
        public CreateDriverRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.AddressLine2)
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
               .NotEmpty()
               .MaximumLength(50);
        }
    }

    public static async Task<IResult> Handle(Request request, IDriverRepository repository)
    {
        var driver = await repository.GetById(request.Id);

        if (driver is null)
        {
            return Results.NotFound();
        }

        // Update the driver's properties.
        driver.Name = request.Name;
        driver.AddressLine1 = request.AddressLine1;
        driver.AddressLine2 = request.AddressLine2;
        driver.PhoneNumber = request.PhoneNumber;

        var savedDriver = await repository.Update(driver);

        return Results.Ok(new Response(savedDriver.Id, savedDriver.Name, savedDriver.AddressLine1, savedDriver.AddressLine2, savedDriver.PhoneNumber));
    }
}
