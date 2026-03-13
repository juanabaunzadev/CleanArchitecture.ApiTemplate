using CleanArchitecture.ApiTemplate.Application.Abstractions.Persistence;

namespace CleanArchitecture.ApiTemplate.Persistence.UnitOfWork;

public class UnitOfWorkEFCore : IUnitOfWork
{
    private readonly ApiTemplateDbContext _context;

    public UnitOfWorkEFCore(ApiTemplateDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        return Task.CompletedTask;
    }
}
