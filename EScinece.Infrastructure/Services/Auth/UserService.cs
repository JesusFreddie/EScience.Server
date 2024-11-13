using EScinece.Domain.Abstraction;
using EScinece.Domain.DTOs;
using EScinece.Domain.Entities;

namespace EScinece.Infrastructure.Services;
 
public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<User?, string>> Register(UserRegisterDto userRegister)
    {
        var hashedPassword = _passwordHasher.Generate(userRegister.Password);

        var result = User.Create(userRegister.Email, hashedPassword);

        if (!result.IsOk)
            return result.Error;

        return result;
    }
}