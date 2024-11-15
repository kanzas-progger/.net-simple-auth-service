namespace SimpleAuthAndAuthorization.API.Contracts;

public record UpdateUserPasswordRequest (string password, string confirmPassword);