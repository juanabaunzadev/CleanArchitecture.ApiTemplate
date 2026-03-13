namespace CleanArchitecture.ApiTemplate.Application.Abstractions.Repositories;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task Delete(T entity);
}