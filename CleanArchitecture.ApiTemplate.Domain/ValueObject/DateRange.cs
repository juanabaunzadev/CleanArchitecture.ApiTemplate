using CleanArchitecture.ApiTemplate.Domain.Exceptions;

namespace CleanArchitecture.ApiTemplate.Domain.ValueObject;

public record DateRange
{
    public DateTime StartDate { get; }
    public DateTime? EndDate { get; private set; }

    public DateRange(DateTime startDate, DateTime? endDate = null)
    {
        if (endDate.HasValue && startDate > endDate)
        {
            throw new BusinessRuleException("Start date must be earlier than end date.");
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    public void Complete()
    {
        if (EndDate.HasValue)
        {
            throw new BusinessRuleException("End date is already set.");
        }

        EndDate = DateTime.UtcNow;
    }
}