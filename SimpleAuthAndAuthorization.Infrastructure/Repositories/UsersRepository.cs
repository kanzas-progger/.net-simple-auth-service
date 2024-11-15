using Microsoft.EntityFrameworkCore;
using SimpleAuthAndAuthorization.Core.Abstractions;
using SimpleAuthAndAuthorization.Core.Enums;
using SimpleAuthAndAuthorization.Core.Exceptions;
using SimpleAuthAndAuthorization.Core.Models;
using SimpleAuthAndAuthorization.Infrastructure.Entities;

namespace SimpleAuthAndAuthorization.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly SimpleAuthAndAuthorizationDbContext _context;

    public UsersRepository(SimpleAuthAndAuthorizationDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var userEntities = await _context.Users.AsNoTracking().ToListAsync();
        var users = userEntities.Select(u => User.Create(u.Id, u.Login, u.PasswordHash,
            u.Firstname, u.Surname, u.Fathername, u.Age, u.Email, u.Phone).user).ToList();
        
        return users;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var userEntity = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) ?? throw new CustomExceptions.UserNotFoundException();;
        
        var user = User.Create(userEntity.Id, userEntity.Login, userEntity.PasswordHash,
            userEntity.Firstname, userEntity.Surname, userEntity.Fathername, userEntity.Age, 
            userEntity.Email, userEntity.Phone).user;

        return user;
    }

    public async Task<Guid> CreateUser(User user)
    {
        var newUser = new UserEntity
        {
            Id = user.Id,
            Login = user.Login,
            PasswordHash = user.PasswordHash,
            Firstname = user.Firstname,
            Surname = user.Surname,
            Age = user.Age,
        };
        
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        
        return newUser.Id;
    }

    public async Task AddRoleToUser(Guid userId, string role)
    {
        var userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name.ToLower() == role.ToLower()) ?? throw new CustomExceptions.RoleNotFoundException();;
        
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new CustomExceptions.UserNotFoundException();

        user.Roles.Add(userRole);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> AddAdminRoleToUser(Guid userId)
    {
        var adminRole = await _context.Roles.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Name == Role.Admin.ToString()) ?? throw new CustomExceptions.RoleNotFoundException();
        
        var user = await _context.Users.AsNoTracking().
            FirstOrDefaultAsync(u => u.Id == userId) ?? throw new CustomExceptions.UserNotFoundException();
        
        user.Roles.Add(adminRole);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<Guid> UpdateUserProfile(User user)
    {
        await _context.Users.Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.Firstname, user.Firstname)
                .SetProperty(u => u.Surname, user.Surname)
                .SetProperty(u => u.Fathername, user.Fathername)
                .SetProperty(u => u.Email, user.Email)
                .SetProperty(u => u.Phone, user.Phone));

        return user.Id;
    }

    public async Task<bool> UpdateUserPassword(Guid id, string passwordHash)
    {
        await _context.Users.Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.PasswordHash, passwordHash));

        return true;
    }

    public async Task<Guid> DeleteUser(Guid id)
    {
        await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
        return id;
    }

    public async Task<bool> VerifyLoginExisting(string login)
    {
        return await _context.Users.AsNoTracking().AnyAsync(u => u.Login.ToLower() == login.ToLower());
    }

    public async Task<User> GetUserByLogin(string login)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == login) ?? throw new CustomExceptions.UserNotFoundException();
        
        var user = User.Create(userEntity.Id, userEntity.Login, userEntity.PasswordHash,
            userEntity.Firstname, userEntity.Surname, userEntity.Fathername, userEntity.Age, 
            userEntity.Email, userEntity.Phone).user;

        return user;
    }

    public async Task<List<string>> GetUserRoles(Guid userId)
    {
        var userRoles = await _context.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new CustomExceptions.UserNotFoundException();

        List<string> roles = userRoles.Roles.Select(r => r.Name).ToList();

        return roles;
    }
    
}