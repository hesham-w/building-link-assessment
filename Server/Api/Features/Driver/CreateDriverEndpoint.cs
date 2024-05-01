using Api.Common;
using Api.Common.Extensions;
using Api.Repositories;
using FluentValidation;

namespace Api.Features.Driver;

public class CreateDriverEndpoint : IEndpoint
{
    public record Request(string Name, string AddressLine1, string AddressLine2, string PhoneNumber);

    public record Response(int DriverId, string Name, string AddressLine1, string AddressLine2, string PhoneNumber);

    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", Handle)
        .WithRequestValidation<Request>();

    public class CreateDriverRequestValidator : AbstractValidator<Request>
    {
        public CreateDriverRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
               .NotEmpty()
               .MaximumLength(50);
        }
    }

    public static async Task<Response> Handle(Request request, IDriverRepository repository)
    {
        var driver = new Domain.Driver()
        {
            Name = request.Name,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            PhoneNumber = request.PhoneNumber
        };

        var savedDriver = await repository.Insert(driver);

        return new Response(savedDriver.Id, savedDriver.Name, savedDriver.AddressLine1, savedDriver.AddressLine2, savedDriver.PhoneNumber);
    }
}