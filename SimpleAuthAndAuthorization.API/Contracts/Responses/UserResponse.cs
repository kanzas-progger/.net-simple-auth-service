namespace SimpleAuthAndAuthorization.API.Contracts.Responses;

public record UserResponse (Guid userId, string login, string firstname, string surname,
    string fathername, int age, string email, string phone);