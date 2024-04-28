using Api.Common;
using Api.Features.Driver.Models;
using Api.Repositories;
using Serilog;

namespace Api.Features.Driver;

public class GetDriverByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", Handle);

    public static async Task<IResult> Handle(int id, IDriverRepository repository)
    {
        var driver = await repository.GetById(id);

        if (driver is null)
        {
            Log.Debug("Driver not found for id {id}", id);
            return Results.NotFound();
        }

        var driverModel = new DriverModel()
        {
            Id = driver.Id,
            Name = driver.Name,
            AddressLine1 = driver.AddressLine1,
            AddressLine2 = driver.AddressLine2,
            PhoneNumber = driver.PhoneNumber
        };

        return Results.Ok(driverModel);
    }
}
