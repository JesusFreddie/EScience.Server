using EScinece.Domain.Entities;

namespace EScinece.Domain.Abstraction.ErrorMessages;

public static class AuthErrorMessage
{
    public const string InvalidEmail = "Invalid email";
    public const string PasswordIsRequired = "Password is required";
    public const string EmailIsRequired = "Email is required";
    public const string NameIsRequired = "Name is required";
    public const string InvalidDataLogin = "Invalid data login";
}