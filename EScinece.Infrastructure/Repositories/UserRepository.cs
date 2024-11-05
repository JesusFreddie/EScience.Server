using EScinece.Domain.Abstraction;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Repositories;

public class UserRepository: IRepository<User>
{
    public List<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public User Create(User entity)
    {
        throw new NotImplementedException();
    }

    public User Update(User entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(User entity)
    {
        throw new NotImplementedException();
    }
}