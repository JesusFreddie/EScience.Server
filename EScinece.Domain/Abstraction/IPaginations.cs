namespace EScinece.Domain.Abstraction;

public interface IPaginations<T> where T : BaseEntity
{
    public Task<List<T>> GetByPage(int pageNumber, int pageSize);
}