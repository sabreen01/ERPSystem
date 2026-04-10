using ERPSystem.Application.Helper.models;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected ActionResult HandleResult<T>(RequestResult<T> result)
    {
        if (result is null) return NotFound();

        if (result.IsSuccess)
            return Ok(result); 

        return BadRequest(result);
    }
    
}