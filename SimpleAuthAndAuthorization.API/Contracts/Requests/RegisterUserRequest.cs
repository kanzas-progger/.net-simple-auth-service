namespace SimpleAuthAndAuthorization.API.Contracts;

public record RegisterUserRequest(string login, string password, string confirmPassword, 
    string firstname, string surname, int age, string role);