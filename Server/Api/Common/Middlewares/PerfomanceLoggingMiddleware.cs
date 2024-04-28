using Serilog;
using System.Diagnostics;

namespace Api.Common.Middlewares;

public class PerfomanceLoggingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
{
    var stopwatch = Stopwatch.StartNew();

    await next(context);

    stopwatch.Stop();

    var route = context.Request.Path.Value;

    if (context.Response.StatusCode == 200)
    {
        Log.Information($"Route: {route}, Time: {stopwatch.ElapsedMilliseconds}ms");
    }
}

}
