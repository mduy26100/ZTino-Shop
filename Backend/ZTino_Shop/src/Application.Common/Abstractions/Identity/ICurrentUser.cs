namespace Application.Common.Abstractions.Identity
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        IEnumerable<string> Roles { get; }
    }
}
