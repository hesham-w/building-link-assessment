using Api.Authentication.Services;
using Api.Common;
using Api.Common.Extensions;
using Api.Domain;
using Api.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Authentication.Endpoints;

public class SignupEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
    .MapPost("/signup", Handle)
    .WithSummary("Creates a new user account")
    .WithRequestValidation<Request>();

    public record Request(string Email, string Password, string Name);
    public record Response(string Token);
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, ValidationProblem>> Handle(Request request, IUserRepository repository, Jwt jwt, CancellationToken ct)
    {
        var isUsernameTaken = await repository.ExistsAsync(request.Email);

        if (isUsernameTaken)
        {
            return TypedResults.Extensions.ValidationProblem(nameof(request.Email), "An account already exists for this email address.");
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };

        var _ = await repository.InsertAsync(user);

        var token = jwt.GenerateToken(user);
        var response = new Response(token);
        return TypedResults.Ok(response);
    }
}