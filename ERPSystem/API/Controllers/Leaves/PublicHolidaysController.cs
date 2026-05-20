using ERPSystem.Application.Features.Leaves.PublicHolidays.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPSystem.API.Controllers.Leaves;

[ApiController]
[Route("[controller]")]
public class PublicHolidaysController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePublicHolidayCommand command)
    {
        return HandleResult(await mediator.Send(command));
    }
}
