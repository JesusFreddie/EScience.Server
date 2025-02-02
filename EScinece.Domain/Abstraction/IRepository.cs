namespace EScinece.Domain.Abstraction;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetById(Guid id);
    public Task<IEnumerable<T>> GetAll();
    public Task Create(T entity);
    public Task<bool> Delete(Guid id);
    public Task<bool> SoftDelete(Guid id);
}