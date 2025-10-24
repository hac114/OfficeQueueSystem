using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OfficeQueueSystem.Controllers
{
    [ApiController]
    //[Authorize] // Protegge TUTTI i controller che ereditano da questo
    public class SecureBaseController : ControllerBase
    {
        // Metodi utility comuni per tutti i controller
        protected string? GetUserId()
        {
            return User.FindFirst("sub")?.Value;
        }

        protected string? GetUserEmail()
        {
            return User.FindFirst("email")?.Value;
        }

        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }
    }
}