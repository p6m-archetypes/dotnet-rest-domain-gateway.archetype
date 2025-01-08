using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace {{ ProjectName}}.Server.Controllers;

[ApiController]
[Route("api")]
[SwaggerTag("User Info")]
public class UserInfoController() : ControllerBase
{
    
    [SwaggerOperation(Summary = "User Info", Description = "Information about currently logged in user")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(401, "Unauthenticated")]
    [HttpGet("/me")]
    public IActionResult me()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value; 
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value); 
            
            return Ok(new
            {
                UserId = userId,
                Email = email,
                Roles = roles
            });
        }

        return Unauthorized("User is not authenticated.");
    }
}
