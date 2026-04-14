using ERPSystem.Application.Features.HR.Departments.Commands;
using ERPSystem.Application.Features.HR.Departments.Queries;
using ERPSystem.Application.Helper.models;
using MediatR;

namespace ERPSystem.Application.Features.HR.Departments.Orchestrator;




public record CreateDepartmentOrchestrator(string Name 
    , string Code 
    ,Guid? ParentId)
    :IRequest<RequestResult<Guid>>;

public class CreateDepartmentOrchestratorHandler(IMediator mediator) 
    : IRequestHandler<CreateDepartmentOrchestrator, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateDepartmentOrchestrator request, CancellationToken cancellationToken)
    {
        if (await mediator.Send(new CheckDepartmentCodeQuery(request.Code)))
        {
            return RequestResult<Guid>.Failure("Already Code Exists");
        }

        if (request.ParentId.HasValue)
        {
            var level = await mediator.Send(new GetDepartmentLevelQuery(request.ParentId.Value));
            if (level >= 3)
            {
                return RequestResult<Guid>.Failure("It is forbidden to exceed 3 levels");
            }
        }

        var id = await mediator.Send(new CreateDepartmentCommand(
            request.Name, 
            request.Code, 
            request.ParentId
            ));
        return RequestResult<Guid>.Success(id , "created successfully");
    }
}

