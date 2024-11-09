using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;


namespace EScinece.Infrastructure.Service;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
    }
    
    public async Task Register(UserRegusterDto userReguster)
    {
        var hashedPassword = _passwordHasher.Generate(userReguster.Password);
        
        var user = new User();
    }
}