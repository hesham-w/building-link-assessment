using Api.Common.Middlewares;
using Serilog;

namespace Api;

public static class ConfigureApp
{
    public static void Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        app.UseMiddleware<PerfomanceLoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapEndpoints();
    }
}
