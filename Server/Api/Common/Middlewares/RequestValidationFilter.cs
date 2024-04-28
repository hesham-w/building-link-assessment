using FluentValidation;
using Serilog;

namespace Api.Common.Middlewares;

public class RequestValidationFilter<TRequest>(IValidator<TRequest>? validator = null) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
{
    var requestName = typeof(TRequest).FullName;

    if (validator is null)
    {
        Log.Information("{Request}: No validator configured.", requestName);
        return await next(context);
    }

    Log.Information("{Request}: Validating...", requestName);
    var request = context.Arguments.OfType<TRequest>().First();
    var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
    if (!validationResult.IsValid)
    {
        Log.Warning("{Request}: Validation failed.", requestName);
        return TypedResults.ValidationProblem(validationResult.ToDictionary());
    }

    Log.Information("{Request}: Validation succeeded.", requestName);
    return await next(context);
}
}
