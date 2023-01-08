namespace Waystone.Sample.Domain.Users;

using Common.Domain.Contracts.Primitives;

/// <summary>
/// A user of the application
/// </summary>
public class User : Entity<Guid>
{
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
    public string Username { get; set; }

    /// <summary>
    /// The user's hashed password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string EmailAddress { get; set; }

    /// <summary>
    /// A flag indicating whether the email was verified.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetSignatureComponents()
    {
        yield return Username;
        yield return EmailAddress;
    }
}
