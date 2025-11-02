namespace Application.Common.Interfaces.Shared
{
    public interface ICurrentUserContext
    {
        Guid UserId { get; }
        IEnumerable<string> Roles { get; }
    }
}
