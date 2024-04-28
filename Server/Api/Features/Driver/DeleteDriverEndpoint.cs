using Api.Common;
using Api.Repositories;
using Serilog;

namespace Api.Features.Driver;

public class DeleteDriverEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapDelete("/:id", Handle);

    public static async Task<IResult> Handle(int driverId, IDriverRepository repository)
    {
        var driver = await repository.GetById(driverId);

        if (driver is null)
        {
            Log.Debug("Driver not found for id {id}", driverId);
            return Results.NotFound();
        }

        await repository.Delete(driver.Id);

        return Results.NoContent();
    }
}
