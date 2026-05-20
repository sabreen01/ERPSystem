using ERPSystem.Domain.Entities.Leaves;
using ERPSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ERPSystem.Application.Interfaces;

namespace ERPSystem.Application.Services;


public class LeaveCalculatorService(IRepository<PublicHoliday> repository) : ILeaveCalculatorService
{
    //range dayes
    public async Task<int> CalculateActualLeaveDaysAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var start = startDate.Date;
        var end = endDate.Date;

        if (start > end) return 0;

        var publicHolidays = await repository
            .GetAll(h => h.IsActive && h.Date >= start && h.Date <= end)
            .Select(h => h.Date.Date)
            .ToListAsync(cancellationToken);

        int actualDaysCount = 0;

        for (var date = start; date <= end; date = date.AddDays(1))
        {
           
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                continue;

          
            if (publicHolidays.Contains(date))
                continue;

            actualDaysCount++;
        }

        return actualDaysCount;
    }

    //date - date
    public async Task<List<DateTime>> FilterValidLeaveDaysAsync(List<DateTime> requestedDates, CancellationToken cancellationToken)
    {
        if (requestedDates == null || !requestedDates.Any()) 
            return new List<DateTime>();

        var dates = requestedDates.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
        var start = dates.First();
        var end = dates.Last();

        var publicHolidays = await repository
            .GetAll(h => h.IsActive && h.Date >= start && h.Date <= end)
            .Select(h => h.Date.Date)
            .ToListAsync(cancellationToken);

        var validDates = new List<DateTime>();

        foreach (var date in dates)
        {
            if (date.DayOfWeek == DayOfWeek.Friday || date.DayOfWeek == DayOfWeek.Saturday)
                continue;

            if (publicHolidays.Contains(date))
                continue;

            validDates.Add(date);
        }

        return validDates;
    }
}
