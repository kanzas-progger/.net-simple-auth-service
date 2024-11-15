using System.ComponentModel.DataAnnotations;
using SimpleAuthAndAuthorization.Core.Abstractions;
using SimpleAuthAndAuthorization.Core.Exceptions;
using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

     public UsersService(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
         IJwtProvider jwtProvider)
     {
         _usersRepository = usersRepository;
         _passwordHasher = passwordHasher;
         _jwtProvider = jwtProvider;
     }

     public async Task<string> Register(string login, string password, string firstname, string surname, int age, string role)
     {
         bool isLoginExists = await VerifyLoginExisting(login);
         if (isLoginExists)
         {
             throw new ValidationException("Login already exists");
         }
         if (role.ToLower() == "admin")
             throw new ValidationException("You cannot create an administrator");
         
         string passwordHash = _passwordHasher.GeneratePasswordHash(password);
         
         var (newUser, error) = User.Create(Guid.NewGuid(), login, passwordHash, firstname, surname, "", age, "", "");
         if (!String.IsNullOrEmpty(error))
         {
             throw new ValidationException(error);
         }

         var newUserId = await _usersRepository.CreateUser(newUser);
         await _usersRepository.AddRoleToUser(newUserId, role);
         string token = await Login(login, password);
         return token;
     }

     public async Task<string> Login(string login, string password)
     {
         var user = await _usersRepository.GetUserByLogin(login);

         if (!(_passwordHasher.VerifyPassword(password, user.PasswordHash)))
         {
             throw new ValidationException("Invalid password");
         }
         
         List<string> roles = await _usersRepository.GetUserRoles(user.Id);

         var token = _jwtProvider.GenerateToken(user.Id, roles);

         return token;
     }
     
    
     public async Task<List<User>> GetAllUsers()
     {
         return await _usersRepository.GetAllUsers();
     }
     
     public async Task<bool> VerifyLoginExisting(string login)
     {
         return await _usersRepository.VerifyLoginExisting(login);
     }

     public async Task<bool> AddAdminRoleToUser(Guid userId)
     {
         return await _usersRepository.AddAdminRoleToUser(userId);
     }

     public async Task<Guid> UpdateUserProfile(Guid userId, string firstname, 
         string surname, string fathername, string email, string phone)
     {
         var userToUpdate = await _usersRepository.GetUserById(userId) ?? throw new CustomExceptions.UserNotFoundException();
   
         User updatedUser = User.Create(userId, userToUpdate.Login, userToUpdate.PasswordHash,
             firstname, surname, fathername, userToUpdate.Age, email, phone).user;
         
         return await _usersRepository.UpdateUserProfile(updatedUser);
     }

     public async Task<bool> ChangeUserPassword(Guid id, string password)
     {
         string passwordHash = _passwordHasher.GeneratePasswordHash(password);

         return await _usersRepository.UpdateUserPassword(id, passwordHash);
     }

     public async Task<Guid> DeleteUser(Guid userId)
     {
         return await _usersRepository.DeleteUser(userId);
     }

     public async Task<User> GetUserByLogin(string login)
     {
         return await _usersRepository.GetUserByLogin(login);
     }
}