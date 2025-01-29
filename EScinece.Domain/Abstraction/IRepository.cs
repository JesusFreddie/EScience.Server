namespace EScinece.Domain.Abstraction;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetById(Guid id);
    public Task<List<T>> GetAll();
    public Task<T> Create(T entity);
    public Task<T> Update(T entity);
    public Task<bool> Delete(Guid id);
}