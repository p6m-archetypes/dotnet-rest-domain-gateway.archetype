using System.Text.Json;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using {{ ProjectName }}.Core;

{%- for service_key in services -%}
{% set service = services[service_key] %}
using {{ service['ProjectName'] }}.Client;
{%- endfor %}
using Path = System.IO.Path;


namespace {{ ProjectName }}.Server;

public class Startup
{
    public Startup(IConfigurationRoot configuration)
    {
        Configuration = configuration;
    }
    public IConfigurationRoot Configuration { get; }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => { c.EnableAnnotations(); });

        {%- for service_key in services -%}
        {% set service = services[service_key] %}
        services.AddSingleton({{ service['ProjectName'] }}Client.Of(Configuration["CoreServices:{{ service['ProjectName'] }}:Url"]));
        {% endfor %}
        services.AddSingleton<{{ ProjectName }}Core>();

        services.AddHealthChecks();
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService("{{ project-name}}"))
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
                metrics.AddOtlpExporter();
            })
            .WithTracing(tracing =>
            {
                tracing
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter();
            })
            ;
    }
        
        
    public void Configure(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", () => "{{ ProjectName }}");

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
    }
}