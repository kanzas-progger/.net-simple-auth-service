namespace SimpleAuthAndAuthorization.API.Contracts;

public record UpdateUserRequest(string firstname, 
    string surname, string fathername, string email, string phone);