namespace SimpleAuthAndAuthorization.Core.Models;

public class User
{
    public const int MINIMAL_LOGIN_LENGTH = 5;
    public const int MAXIMAL_LOGIN_LENGTH = 20;
    //public const int MINIMAL_PASSWORD_LENGTH = 8;
    //public const int MAXIMAL_PASSWORD_LENGTH = 64;
    public Guid Id { get; }
    public string Login { get; } = string.Empty;
    public string PasswordHash { get; } = string.Empty;
    public string Firstname { get; } = string.Empty;
    public string Surname { get; } = string.Empty;
    public string Fathername { get; } = string.Empty;
    public int Age { get; } = 0;
    public string Email { get; } = string.Empty;
    public string Phone { get; } = string.Empty;

    private User(Guid id, string login, string passwordHash,
        string firstname, string surname, string fathername, int age, string email, string phone)
    {
        Id = id;
        Login = login;
        PasswordHash = passwordHash;
        Firstname = firstname;
        Surname = surname;
        Fathername = fathername;
        Age = age;
        Email = email;
        Phone = phone;
    }

    public static (User user, string error) Create(Guid id, string login, string passwordHash,
        string firstname, string surname, string fathername, int age, string email, string phone)
    {
        string error = string.Empty;
        if (login.Length < MINIMAL_LOGIN_LENGTH)
            error += $"Login length must be at least {MINIMAL_LOGIN_LENGTH}";
        if (login.Length > MAXIMAL_LOGIN_LENGTH)
            error += $"Login length must be less than {MAXIMAL_LOGIN_LENGTH}";
        
        User newUser = new User(id, login, passwordHash, firstname, surname,
            fathername, age, email, phone);
        
        return (newUser, error);
    }
}