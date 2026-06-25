namespace HotelReservaBack.Application.Interfaces;

public interface ICrudService<TDto>
    where TDto : class
{
    Task<IReadOnlyList<TDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<TDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TDto> CrearAsync(TDto dto, CancellationToken cancellationToken = default);
    Task<bool> ActualizarAsync(int id, TDto dto, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default);
}
