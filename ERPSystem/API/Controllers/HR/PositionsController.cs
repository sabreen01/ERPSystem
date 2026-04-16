using ERPSystem.Application.Features.HR.Positions.DTOs;
using ERPSystem.Application.Features.HR.Positions.Orchestrators;
using ERPSystem.Application.Features.HR.Positions.Queries;
using ERPSystem.Domain.Entities.HR;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.HR;

[ApiController]
[Route("[controller]")]
public class PositionsController  (IMediator mediator) : BaseController
{

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePositionDto dto)
    {
        return HandleResult(await mediator.Send(new CreatePositionOrchestrator(dto)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await mediator.Send(new GetAllPositionsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return HandleResult(await mediator.Send(new GetPositionByIdQuery(id)));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePositionDto dto)
    {
        return HandleResult(await mediator.Send(new UpdatePositionOrchestrator(id,dto)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return HandleResult(await mediator.Send(new DeletePositionOrchestrator(id)));
    }


}