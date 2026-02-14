using CleanArchitecture.ApiTemplate.Domain.Enums;
using CleanArchitecture.ApiTemplate.Domain.Exceptions;
using CleanArchitecture.ApiTemplate.Domain.ValueObject;

namespace CleanArchitecture.ApiTemplate.Domain.Entities;

public class ToDo
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateRange? Timeline { get; private set; }
    public ToDoStatus Status { get; private set; }

    public ToDo(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"{nameof(name)} cannot be null or empty.");
        }

        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        CreatedDate = DateTime.UtcNow;
        Status = ToDoStatus.NotStarted;
    }

    public void MarkAsInProgress()
    {
        if (Status != ToDoStatus.NotStarted)
        {
            throw new BusinessRuleException("Only ToDos that are Not Started can be marked as In Progress.");
        }

        Status = ToDoStatus.InProgress;
        Timeline = new DateRange(DateTime.UtcNow);
    }

    public void MarkAsCompleted()
    {
        if (Status != ToDoStatus.InProgress)
        {
            throw new BusinessRuleException("Only ToDos that are In Progress can be marked as Completed.");
        }

        if (Timeline == null)
        {
            throw new BusinessRuleException("Timeline must be initialized before completing a ToDo.");
        }

        Status = ToDoStatus.Completed;
        Timeline.Complete();
    }
}