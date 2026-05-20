namespace ERPSystem.Application.Interfaces;

public interface ILeaveCalculatorService
{
    Task<int> CalculateActualLeaveDaysAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<List<DateTime>> FilterValidLeaveDaysAsync(List<DateTime> requestedDates, CancellationToken cancellationToken);
}
