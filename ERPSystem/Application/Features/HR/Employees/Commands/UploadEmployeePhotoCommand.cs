using ERPSystem.Application.Helper.models;
using ERPSystem.Application.Interfaces;
using ERPSystem.Domain.Entities.HR;
using ERPSystem.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ERPSystem.Application.Features.HR.Employees.Commands;

public record UploadEmployeePhotoCommand(Guid EmployeeId, IFormFile Photo) : IRequest<RequestResult<string>>;

public class UploadEmployeePhotoCommandHandler(
    IRepository<Employee> repository, 
    IFileService fileService) 
    : IRequestHandler<UploadEmployeePhotoCommand, RequestResult<string>>
{
    public async Task<RequestResult<string>> Handle(UploadEmployeePhotoCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetById(request.EmployeeId);
        if (employee == null)
            return RequestResult<string>.Failure("Employee not found.");

       
        if (!string.IsNullOrEmpty(employee.PhotoPath))
        {
            fileService.DeleteFile(employee.PhotoPath);
        }

        
        var newPhotoPath = await fileService.UploadFileAsync(request.Photo, "employees");
        
        if (string.IsNullOrEmpty(newPhotoPath))
            return RequestResult<string>.Failure("Failed to upload photo.");

        employee.PhotoPath = newPhotoPath;
        repository.Update(employee);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<string>.Success(newPhotoPath, "Photo uploaded successfully.");
    }
}
