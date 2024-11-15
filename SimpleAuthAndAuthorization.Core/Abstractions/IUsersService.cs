using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.Core.Abstractions;

public interface IUsersService
{
    Task<string> Register(string login, string password, string firstname, string surname, int age, string role);
    Task<string> Login(string login, string password);
    Task<List<User>> GetAllUsers();
    Task<bool> VerifyLoginExisting(string login);
    Task<bool> AddAdminRoleToUser(Guid userId);
    Task<Guid> UpdateUserProfile(Guid userId, string firstname, 
        string surname, string fathername, string email, string phone);

    Task<bool> ChangeUserPassword(Guid id, string password);
    Task<Guid> DeleteUser(Guid userId);
    Task<User> GetUserByLogin(string login);
}