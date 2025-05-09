{% import "macros/dotnet" as dotnet %}
using System.Text;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using {{ ProjectName }}.Core.Models;
{% if use-default-service == false %}
{%- for service_key in services -%}
{% set service = services[service_key] %}
using {{ service['ProjectName']}}.API;
{%- endfor %}{% endif %}
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace {{ ProjectName }}.IntegrationTests;

public class {{ ProjectName }}Test(ITestOutputHelper testOutputHelper, WebApplicationFactory<Program> factory)
    : BaseIntegrationTest(testOutputHelper, factory)
{
    private readonly string _id = Guid.NewGuid().ToString();
    private readonly string _name = "name_" + Guid.NewGuid();

    {% if use-default-service == false %}
    {%- for service_key in services -%}
    {% set service = services[service_key] %}
    {%- for entity_key in service.model.entities -%}
    {%- set entity = service.model.entities[entity_key] %}
    {{ dotnet.integration_test_methods(entity_key, service.model.entities[entity_key], service.model) }}
    {% endfor %}
    {% endfor %}
    {% endif %}
}