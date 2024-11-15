using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.Core.Abstractions;

public interface IUsersRepository
{
    Task<List<User>> GetAllUsers();
    Task<User?> GetUserById(Guid id);
    Task<Guid> CreateUser(User user);
    Task<Guid> UpdateUserProfile(User user);
    Task<bool> UpdateUserPassword(Guid id, string passwordHash);
    Task<Guid> DeleteUser(Guid id);
    Task<bool> VerifyLoginExisting(string login);
    Task<User> GetUserByLogin(string login);
    Task<List<string>> GetUserRoles(Guid userId);
    Task AddRoleToUser(Guid userId, string role);
    Task<bool> AddAdminRoleToUser(Guid userId);
}