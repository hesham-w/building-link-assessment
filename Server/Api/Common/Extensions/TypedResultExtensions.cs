﻿using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Common.Extensions;

public static class TypedResultExtensions
{
    public static ValidationProblem ValidationProblem(this IResultExtensions _, string property, string message)
        => TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            [property] = [message]
        });
}
