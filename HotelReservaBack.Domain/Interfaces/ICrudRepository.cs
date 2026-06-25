namespace HotelReservaBack.Domain.Interfaces;

public interface ICrudRepository<TEntity>
    where TEntity : class
{
    Task<IReadOnlyList<TEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<TEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity> CrearAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(int id, TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
