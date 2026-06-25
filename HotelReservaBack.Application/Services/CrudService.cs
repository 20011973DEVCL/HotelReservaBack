using System.Reflection;
using System.Security.Cryptography;

using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;
using HotelReservaBack.Domain.Entities;
using HotelReservaBack.Domain.Interfaces;

namespace HotelReservaBack.Application.Services;

public class CrudService<TEntity, TDto>(ICrudRepository<TEntity> repository) : ICrudService<TDto>
    where TEntity : class, new()
    where TDto : class, new()
{
    protected ICrudRepository<TEntity> Repository { get; } = repository;

    public virtual async Task<IReadOnlyList<TDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<TEntity> entities = await Repository.ObtenerTodosAsync(cancellationToken);
        return [.. entities.Select(Map<TDto>)];
    }

    public virtual async Task<TDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        TEntity? entity = await Repository.ObtenerPorIdAsync(id, cancellationToken);
        return entity is null ? null : Map<TDto>(entity);
    }

    public virtual async Task<TDto> CrearAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        TEntity entity = Map<TEntity>(dto);
        TEntity created = await Repository.CrearAsync(entity, cancellationToken);
        return Map<TDto>(created);
    }

    public virtual Task<bool> ActualizarAsync(int id, TDto dto, CancellationToken cancellationToken = default)
    {
        return Repository.ActualizarAsync(id, Map<TEntity>(dto), cancellationToken);
    }

    public virtual Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        return Repository.EliminarAsync(id, cancellationToken);
    }

    protected static TDestination Map<TDestination>(object source)
        where TDestination : class, new()
    {
        TDestination destination = new TDestination();
        Dictionary<string, PropertyInfo> sourceProperties = source.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.CanRead)
            .ToDictionary(property => property.Name);

        foreach (PropertyInfo destinationProperty in typeof(TDestination)
                     .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                     .Where(property => property.CanWrite))
        {
            if (sourceProperties.TryGetValue(destinationProperty.Name, out PropertyInfo? sourceProperty)
                && destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
            {
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        return destination;
    }
}

public sealed class UsuarioService(ICrudRepository<Usuario> repository)
    : CrudService<Usuario, UsuarioDto>(repository)
{
    private const int Iteraciones = 100_000;

    public override async Task<UsuarioDto> CrearAsync(
        UsuarioDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.UsuClave))
        {
            throw new ArgumentException("La clave del usuario es obligatoria.", nameof(dto));
        }

        Usuario entity = Map<Usuario>(dto);
        entity.UsuClaveHash = CrearHash(dto.UsuClave);
        Usuario created = await Repository.CrearAsync(entity, cancellationToken);
        return Map<UsuarioDto>(created);
    }

    public override async Task<bool> ActualizarAsync(
        int id,
        UsuarioDto dto,
        CancellationToken cancellationToken = default)
    {
        Usuario? actual = await Repository.ObtenerPorIdAsync(id, cancellationToken);
        if (actual is null)
        {
            return false;
        }

        Usuario entity = Map<Usuario>(dto);
        entity.UsuClaveHash = string.IsNullOrWhiteSpace(dto.UsuClave)
            ? actual.UsuClaveHash
            : CrearHash(dto.UsuClave);

        return await Repository.ActualizarAsync(id, entity, cancellationToken);
    }

    private static string CrearHash(string clave)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            clave,
            salt,
            Iteraciones,
            HashAlgorithmName.SHA256,
            32);

        return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
