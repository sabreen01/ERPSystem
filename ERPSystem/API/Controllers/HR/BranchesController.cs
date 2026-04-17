using ERPSystem.Application.Features.HR.Branches.Commands;
using ERPSystem.Application.Features.HR.Branches.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.HR;
[ApiController]
[Route("[controller]")]
public class BranchesController(IMediator mediator):BaseController
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
    {
        return HandleResult(await mediator.Send(new CreateBranchCommand(dto)));
    }

}