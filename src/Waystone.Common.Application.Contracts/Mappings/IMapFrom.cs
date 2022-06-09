namespace Waystone.Common.Application.Contracts.Mappings;

using AutoMapper;

/// <summary>Specifies the contract for a mapping profile from the source type T to the class implementing this interface.</summary>
/// <typeparam name="T">The source type that maps to the class implementing this interface.</typeparam>
public interface IMapFrom<T>
{
    /// <summary>Configures a mapping from the specified type T to the class that implements this interface.</summary>
    /// <param name="profile">The mapping <see cref="Profile" />.</param>
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}
