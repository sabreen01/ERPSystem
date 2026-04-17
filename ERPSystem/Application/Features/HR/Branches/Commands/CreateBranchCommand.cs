using ERPSystem.Application.Features.HR.Branches.DTOs;
using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.HR.Branches.Commands;

public record CreateBranchCommand(CreateBranchDto Data) : IRequest<RequestResult<Guid>>;
public class CreateBranchHandler(IRepository<Branch> repository) 
    : IRequestHandler<CreateBranchCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = new Branch
        {
            Name = request.Data.Name,
            Location = request.Data.Location,
            IsActive = true
        };
        
         repository.Add(branch);
         await repository.SaveChangesAsync(cancellationToken);
         return RequestResult<Guid>.Success(branch.Id, "Branch created successfully");
        
        
    }
}