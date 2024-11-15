using SimpleAuthAndAuthorization.Core.Abstractions;

namespace SimpleAuthAndAuthorization.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string GeneratePasswordHash(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}