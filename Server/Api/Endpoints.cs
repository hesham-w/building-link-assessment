using Api.Authentication.Endpoints;
using Api.Common;
using Api.Features.Driver;
using Api.InternalEndpoints;
using Chirper.Authentication.Endpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Api;

public static class Endpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var securityScheme = new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Name = JwtBearerDefaults.AuthenticationScheme,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new()
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        };

        var endpoints = app.MapGroup("")
            .MapEndpoint<PingEndpoint>();

        endpoints.MapGroup("/auth")
             .WithTags("Authentication")
             .AllowAnonymous()
             .MapEndpoint<SignupEndpoint>()
             .MapEndpoint<LoginEndpoint>();

        endpoints.MapGroup("/driver")
            .RequireAuthorization()
            .WithOpenApi(x => new(x)
            {
                Security = [new() { [securityScheme] = [] }],
            })
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
