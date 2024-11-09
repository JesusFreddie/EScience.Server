namespace EScience.Application.Requests;

public record UserRegisterRequest(string Email, string Password, string Name);