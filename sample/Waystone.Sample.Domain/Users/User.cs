namespace Waystone.Sample.Domain.Users;

using Common.Domain.Contracts.Primitives;
using Common.Domain.Primitives;
using Common.Domain.Results;
using JetBrains.Annotations;

/// <summary>
/// A user of the application
/// </summary>
public class User : Entity<Guid>, IAggregateRoot
{
    [UsedImplicitly]
    private User()
    { }

    /// <summary>
    /// Creates a new instance of an user.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="emailAddress"></param>
    /// <param name="emailVerified"></param>
    public User(string username, string password, string emailAddress, bool emailVerified = false)
    {
        Username = username;
        Password = password;
        EmailAddress = emailAddress;
        EmailVerified = emailVerified;
    }

    /// <summary>
    /// The user's username.
    /// </summary>
    public string Username { get; init; } = default!;

    /// <summary>
    /// The user's hashed password.
    /// </summary>
    public string Password { get; private set; } = default!;

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string EmailAddress { get; private set; } = default!;

    /// <summary>
    /// A flag indicating whether the email was verified.
    /// </summary>
    public bool EmailVerified { get; private set; }

    public Result<User> UpdatePassword(string password)
    {
        Password = password;

        return this;
    }

    public Result<User> UpdateEmail(string emailAddress)
    {
        EmailAddress = emailAddress;

        return this;
    }

    public Result<User> UpdateEmailVerified(bool verified)
    {
        EmailVerified = verified;

        return this;
    }

    public static Result<User> CreateTransient(string username, string password, string emailAddress)
    {
        return new User(username, password, emailAddress);
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetSignatureComponents()
    {
        yield return Username;
        yield return EmailAddress;
    }
}
