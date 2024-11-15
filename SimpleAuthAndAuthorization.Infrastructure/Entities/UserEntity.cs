namespace SimpleAuthAndAuthorization.Infrastructure.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Fathername { get; set; } = string.Empty;
    public int Age { get; set; } = 0;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public ICollection<RoleEntity> Roles { get; set; } = [];
}