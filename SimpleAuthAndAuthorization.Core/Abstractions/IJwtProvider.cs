using SimpleAuthAndAuthorization.Core.Models;

namespace SimpleAuthAndAuthorization.Core.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, List<string> roles);
}