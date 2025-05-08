using Microsoft.AspNetCore.Authentication;
{% if integrate-services == true %}
{%- for service_key in services -%}
{% set service = services[service_key] %}
using {{ service['ProjectName']}}.API;
{%- endfor %}{% endif %}
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using {{ ProjectName }}.IntegrationTests.Handlers;
using Xunit.Abstractions;

namespace {{ ProjectName }}.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    {% if integrate-services == true %}
    {%- for service_key in services -%}
    {% set service = services[service_key] %}
    protected readonly Mock<I{{ service['ProjectName']}}> {{ service['ProjectName'] }}Mock;
    {%- endfor %}{% endif %}
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly ITestOutputHelper TestOutputHelper;

    public BaseIntegrationTest(ITestOutputHelper testOutputHelper, WebApplicationFactory<Program> factory)
    {
        {% if integrate-services == true %}
        {%- for service_key in services -%}
        {% set service = services[service_key] %}
        {{ service['ProjectName'] }}Mock = new Mock<I{{ service['ProjectName'] }}>();
        {%- endfor %}{% endif %}
        TestOutputHelper = testOutputHelper;
        Factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                {% if integrate-services == true %}
                {%- for service_key in services -%}
                {% set service = services[service_key] %}
                // Remove the real gRPC service
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(I{{ service['ProjectName']}}));
                if (descriptor != null) services.Remove(descriptor);

                // Add the mocked gRPC service
                services.AddSingleton<I{{ service['ProjectName']}}>({{ service['ProjectName']}}Mock.Object);
                {%- endfor %}{% endif %}
                // Add Test Authentication
                services.AddAuthentication("TestAuth")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestAuth", options => { });
            });
        });

        Client = Factory.CreateClient();
    }
}