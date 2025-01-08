using {{ ProjectName }}.Core;
{%- for service_key in services -%}
{% set service = services[service_key] %}
using {{ service['ProjectName'] }}.API;
using {{ service['ProjectName'] }}.Client;
{% endfor %}
using OpenTelemetry.Logs;
using {{ ProjectName }}.Server.Extensions;
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
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

{%- for service_key in services -%}
{% set service = services[service_key] %}
builder.Services.AddSingleton<I{{ service['ProjectName'] }}>({{ service['ProjectName'] }}Client.Of(builder.Configuration["CoreServices:{{ service['ProjectName'] }}:Url"]));
{% endfor %}
builder.Services.AddSingleton<{{ ProjectName }}Core>();
builder.Services.AddHealthChecks();
builder.Services.AddOpenTelemetryConfig(builder.Configuration);

builder.Services.AddJwtBearerAuth(builder.Configuration);

var app = builder.Build();

app.UseSwaggerUIWithAuth(builder.Configuration);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "{{ ProjectName }}")
    .AllowAnonymous();
app.MapPrometheusScrapingEndpoint("/metrics")
    .AllowAnonymous();
app.MapHealthChecksConfig("/health")
    .AllowAnonymous();

app.Run();

public partial class Program { }