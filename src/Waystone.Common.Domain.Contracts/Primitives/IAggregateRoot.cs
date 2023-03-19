namespace Waystone.Common.Domain.Contracts.Primitives;

/// <summary>
/// The marker interface to use when creating an aggregate root object
/// </summary>
/// <remarks>
/// An aggregate root is the entry point to a cluster of associated objects that are treated as a unit for the
/// purposes of data changes
/// </remarks>
[PublicAPI]
public interface IAggregateRoot
{ }
