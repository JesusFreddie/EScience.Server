namespace EScinece.Domain.Abstraction;

public interface IRepository<T> where T: BaseEntity
{
    public List<T> GetAll();
    public T Create(T entity);
    public T Update(T entity);
    public void Delete(T entity);
}