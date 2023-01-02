namespace Waystone.Common.Domain.Contracts.Primitives;

/// <summary>
/// Represents an object that has an identity that is specified by a unique ID.
/// </summary>
/// <remarks>Commonly used types for <c>TId</c> are <c>int</c>, <c>Guid</c>.</remarks>
/// <typeparam name="TId">The type of the unique identifier.</typeparam>
public interface IEntity<out TId> where TId : IEquatable<TId>
{
    /// <summary>
    /// The ID that is used to uniquely identify the current instance of <see cref="IEntity{TId}" />.
    /// </summary>
    TId? Id { get; }

    /// <summary>
    /// A computed flag that indicates whether the current instance of
    /// <see cref="IEntity{TId}" /> is transient.
    /// </summary>
    /// <remarks>An entity is considered transient if it's ID has not yet been set.</remarks>
    /// <returns>
    /// <c>true</c> when <see cref="Id" /> is <c>null</c> or <c>default</c>; otherwise <c>false</c>.
    /// </returns>
    bool IsTransient();
}
