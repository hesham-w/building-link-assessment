using Api.Common;
using Api.Features.Driver.Models;
using Api.Repositories;

namespace Api.Features.Driver;

public class GetAllDriversEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/", Handle);

    public static async Task<IResult> Handle(IDriverRepository repository)
    {
        var drivers = await repository.GetAll();

        var driversList = drivers.Select(
            driver => new DriverModel()
            {
                Id = driver.Id,
                Name = driver.Name,
                AddressLine1 = driver.AddressLine1,
                AddressLine2 = driver.AddressLine2,
                PhoneNumber = driver.PhoneNumber
            });

        return Results.Ok(driversList);
    }
}
