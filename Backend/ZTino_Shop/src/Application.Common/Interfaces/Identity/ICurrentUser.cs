namespace Application.Common.Interfaces.Identity
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        IEnumerable<string> Roles { get; }
    }
}
