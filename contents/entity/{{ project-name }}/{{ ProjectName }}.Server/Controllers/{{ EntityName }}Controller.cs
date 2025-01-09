{% import "macros/dotnet" as dotnet %}
using Microsoft.AspNetCore.Mvc;
using {{ ProjectName }}.Core;
using {{ ProjectName }}.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace {{ ProjectName }}.Server.Controllers;

[ApiController]
[Route("api/{{ entity_name | pluralize }}")]
[SwaggerTag("{{ EntityName }} API")]
public class {{ EntityName }}Controller({{ ProjectName }}Core {{ projectName}}) : ControllerBase
{

    [HttpPost]
    [SwaggerOperation(Summary = "Create a {{ entityName }}", Description = "Creates a {{ entityName }}")]
    [SwaggerResponse(200, "{{ EntityName }} was created successfully", typeof({{ EntityName }}))]
    public {{ entity_name | pascal_case }} Create{{ entity_name | pascal_case }}(Create{{ entity_name | pascal_case }}RequestDto request)
    {
        return {{ projectName }}.Create{{ entity_name | pascal_case }}(request);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get {{ entityName }} by id", Description = "Get {{ entityName }} by id")]
    [SwaggerResponse(200, "{{ EntityName }} was found", typeof({{ EntityName }}))]
    [SwaggerResponse(404, "{{ EntityName }} not found by given id")]
    public {{ entity_name | pascal_case }}? {{ entity_name | pascal_case }}(Guid id)
    {
        return {{ projectName }}.Get{{ entity_name | pascal_case }}(id);
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Get all {{ entityName | pluralize }}", Description = "Get all {{ entityName | pluralize }}")]
    [SwaggerResponse(200, "{{ entityName | pluralize }} was found", typeof({{ EntityName }}))]
    public List<{{ entity_name | pascal_case }}> {{ entity_name | pascal_case | pluralize }}()
    {
        return {{ projectName }}.Get{{ entity_name | pascal_case | pluralize }}();
    }


    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update {{ entityName }}", Description = "Updates {{ entityName }} by id")]
    [SwaggerResponse(200, "{{ EntityName }} was updated successfully", typeof({{ EntityName }}))]
    [SwaggerResponse(404, "{{ EntityName }} not found by given id")]
    public {{ entity_name | pascal_case }}? Update{{ entity_name | pascal_case }}(Guid id, [FromBody] Update{{ entity_name | pascal_case }}RequestDto request)
    {
        return {{ projectName }}.Update{{ entity_name | pascal_case }}(id, request);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete {{ entityName }}", Description = "Deletes {{ entityName }} by id")]
    [SwaggerResponse(200, "{{ EntityName }} was deleted successfully")]
    [SwaggerResponse(404, "{{ EntityName }} not found by given id")]
    public bool Delete{{ entity_name | pascal_case }}(Guid id)
    {
        return {{ projectName }}.Delete{{ entity_name | pascal_case }}(id);
    }
}

