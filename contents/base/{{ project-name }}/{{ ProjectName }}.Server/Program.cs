using System.Text.Json;
using {{ ProjectName }}.Core;
{%- for service_key in services -%}
{% set service = services[service_key] %}
using {{ service['ProjectName'] }}.API;
using {{ service['ProjectName'] }}.Client;
{% endfor %}
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog configuration
builder.Host.UseSerilog((content, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(builder.Configuration)
);

builder.Logging.AddOpenTelemetry(logging => logging.AddOtlpExporter());

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

{%- for service_key in services -%}
{% set service = services[service_key] %}
builder.Services.AddSingleton<I{{ service['ProjectName'] }}>({{ service['ProjectName'] }}Client.Of(builder.Configuration["CoreServices:{{ service['ProjectName'] }}:Url"]));
{% endfor %}
builder.Services.AddSingleton<{{ ProjectName }}Core>();

builder.Services.AddHealthChecks();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("{{ project-name}}"))
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();
        metrics.AddOtlpExporter();
        metrics.AddPrometheusExporter();
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter();
    })
    ;

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "{{ ProjectName }}");
app.MapPrometheusScrapingEndpoint("/metrics");
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description ?? "No description",
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();

public partial class Program { }