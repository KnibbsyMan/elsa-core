using Elsa.Expressions.JavaScript.TypeDefinitions.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Expressions.JavaScript.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds a <see cref="IFunctionDefinitionProvider"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="T">The type of the function definition provider.</typeparam>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddFunctionDefinitionProvider<T>(this IServiceCollection services) where T: class, IFunctionDefinitionProvider => services.AddScoped<IFunctionDefinitionProvider, T>();

    /// <summary>
    /// Adds a <see cref="IFunctionDefinitionProvider"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="factory">A callback to create the function definition provider.</param>
    /// <typeparam name="T">The type of the function definition provider.</typeparam>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddFunctionDefinitionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T: class, IFunctionDefinitionProvider => 
        services.AddScoped<IFunctionDefinitionProvider, T>(factory);
    
    /// <summary>
    /// Adds a <see cref="ITypeDefinitionProvider"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <typeparam name="T">The type of the type definition provider.</typeparam>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddTypeDefinitionProvider<T>(this IServiceCollection services) where T: class, ITypeDefinitionProvider => services.AddScoped<ITypeDefinitionProvider, T>();

    /// <summary>
    /// Adds a <see cref="ITypeDefinitionProvider"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="factory">A callback to create the type definition provider.</param>
    /// <typeparam name="T">The type of the type definition provider.</typeparam>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddTypeDefinitionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T: class, ITypeDefinitionProvider => 
        services.AddScoped<ITypeDefinitionProvider, T>();

    /// <summary>
    /// Adds a <see cref="IVariableDefinitionProvider"/> to the service collection.
    /// </summary>
    public static IServiceCollection AddVariableDefinitionProvider<T>(this IServiceCollection services) where T: class, IVariableDefinitionProvider => services.AddScoped<IVariableDefinitionProvider, T>();
    
    /// <summary>
    /// Adds a <see cref="IVariableDefinitionProvider"/> to the service collection.
    /// </summary>
    public static IServiceCollection AddVariableDefinitionProvider<T>(this IServiceCollection services, Func<IServiceProvider, T> factory) where T: class, IVariableDefinitionProvider => 
        services.AddScoped<IVariableDefinitionProvider, T>(factory);
}