namespace Waystone.Common.Application.Common.Mappings;

using System.Reflection;
using AutoMapper;
using Contracts.Mappings;

/// <summary>
/// The mapping profile which registers all classes implementing <see cref="IMapFrom{T}"/>.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new <see cref="MappingProfile"/>.
    /// </summary>
    public MappingProfile()
    {
        ApplyMappingsFromAssemblies(AssemblyMarkersContainingIMapFrom);
    }

    /// <summary>
    /// The assembly markers containing classes implementing <see cref="IMapFrom{T}"/>.
    /// </summary>
    public static IEnumerable<Type> AssemblyMarkersContainingIMapFrom { get; set; } = new List<Type>();

    private void ApplyMappingsFromAssemblies(IEnumerable<Type> assemblyMarkers)
    {
        const string mappingMethodName = nameof(IMapFrom<object>.Mapping);
        string interfaceName = typeof(IMapFrom<>).Name;

        foreach (Assembly assembly in assemblyMarkers.Select(marker => marker.Assembly))
        {
            Type[] exportedTypes = assembly.GetExportedTypes();
            List<Type> mappingTypes = exportedTypes.Where(TypeImplementsIMapFrom).ToList();

            foreach (Type type in mappingTypes)
            {
                object? instance = Activator.CreateInstance(type);

                MethodInfo? methodInfo = type.GetMethod(mappingMethodName)
                                      ?? type.GetInterface(interfaceName)?.GetMethod(mappingMethodName);

                methodInfo?.Invoke(instance, new object[] { this });
            }
        }

        bool TypeImplementsIMapFrom(Type type) => type.GetInterfaces().Any(TypeIsIMapFrom);
        bool TypeIsIMapFrom(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMapFrom<>);
    }
}
