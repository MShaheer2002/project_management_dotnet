using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagementBackend.Api.Controller;

[ApiController]
[Route("api/health")]
[Authorize]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Perfect!");
    }
}