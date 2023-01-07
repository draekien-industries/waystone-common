namespace Waystone.Sample.Domain.Users;

using Common.Domain.Contracts.Primitives;

public class User : Entity<Guid>
{
    /// <inheritdoc />
    protected override IEnumerable<object?> GetSignatureComponents()
    {
        throw new NotImplementedException();
    }
}
