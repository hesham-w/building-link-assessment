using Api.Common;

namespace Api.InternalEndpoints;

public class PingEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/ping", Handle)
           .AllowAnonymous();

    public static string Handle() => "pong";
}

