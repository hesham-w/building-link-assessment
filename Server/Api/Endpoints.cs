using Api.Common;
using Api.Features.Driver;
using Api.InternalEndpoints;

namespace Api;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpoints = app.MapGroup("")
            .MapEndpoint<PingEndpoint>();

        endpoints.MapGroup("/driver")
            .WithTags("Driver Endpoints")
            .MapEndpoint<CreateDriverEndpoint>()
            .MapEndpoint<GetDriverByIdEndpoint>()
            .MapEndpoint<GetAllDriversEndpoint>()
            .MapEndpoint<UpdateDriverEndpoint>()
            .MapEndpoint<DeleteDriverEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app) where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}
