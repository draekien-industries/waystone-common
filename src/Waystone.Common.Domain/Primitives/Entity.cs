namespace Waystone.Common.Domain.Primitives;

using Contracts.Primitives;

/// <inheritdoc cref="IEntity{TId}" />
[PublicAPI]
public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// Creates a new instance of an Entity with an optional ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    protected Entity(TId? id = default)
    {
        Id = id;
    }

    /// <inheritdoc />
    public TId? Id { get; private set; }

    /// <inheritdoc />
    public bool IsTransient()
    {
        return Id == null || Id.Equals(default);
    }

    /// <inheritdoc />
    public virtual bool Equals(Entity<TId>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;

        if (!IsTransient()) return Id!.Equals(other.Id);

        return other.IsTransient() && OtherHasSameSignatureComponents();

        bool OtherHasSameSignatureComponents()
        {
            return GetSignatureComponents().SequenceEqual(other.GetSignatureComponents());
        }
    }


    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((Entity<TId>)obj);
    }

    /// <summary>
    /// Assigns an ID to the current entity.
    /// </summary>
    /// <param name="id">The ID.</param>
    public void AssignId(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets the Entity's signature components so that an entity's signature
    /// can be compared with another entity if it is considered transient.
    /// </summary>
    /// <remarks>
    /// Use <c>yield return</c> to return each signature that uniquely identifies
    /// a transient entity.
    /// </remarks>
    /// <returns>THe set of signature components.</returns>
    protected abstract IEnumerable<object?> GetSignatureComponents();

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        if (!IsTransient()) return Id!.GetHashCode();

        return GetSignatureComponents()
              .Select(component => component is not null ? component.GetHashCode() : 0)
              .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}
