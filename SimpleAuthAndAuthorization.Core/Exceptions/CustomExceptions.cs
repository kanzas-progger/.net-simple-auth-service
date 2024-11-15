namespace SimpleAuthAndAuthorization.Core.Exceptions;

public class CustomExceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }
    }

    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException() : base("User not found") { }
    }

    public class RoleNotFoundException : DomainException
    {
        public RoleNotFoundException() : base("Role not found.") { }
    }
}