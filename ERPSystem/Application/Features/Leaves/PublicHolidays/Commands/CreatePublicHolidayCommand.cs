using ERPSystem.Application.Helper.models;
using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using MediatR;

namespace ERPSystem.Application.Features.Leaves.PublicHolidays.Commands;

public record CreatePublicHolidayCommand(string Name, DateTime Date) : IRequest<RequestResult<Guid>>;

public class CreatePublicHolidayCommandHandler(IRepository<PublicHoliday> repository)
    : IRequestHandler<CreatePublicHolidayCommand, RequestResult<Guid>>
{
    public async Task<RequestResult<Guid>> Handle(CreatePublicHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = new PublicHoliday
        {
            Name = request.Name,
            Date = DateTime.SpecifyKind(request.Date.Date, DateTimeKind.Utc)
        };

        repository.Add(holiday);
        await repository.SaveChangesAsync(cancellationToken);

        return RequestResult<Guid>.Success(holiday.Id, "Public holiday created successfully.");
    }
}
