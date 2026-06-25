using HotelReservaBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HotelReservaBack.Api.Controllers;

public abstract class CrudControllerBase<TDto>(ICrudService<TDto> service) : ControllerBase
    where TDto : class
{
    private readonly ICrudService<TDto> _service = service;

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos(CancellationToken cancellationToken)
    {
        IReadOnlyList<TDto> items = await _service.ObtenerTodosAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        TDto? item = await _service.ObtenerPorIdAsync(id, cancellationToken);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] TDto dto, CancellationToken cancellationToken)
    {
        TDto created = await _service.CrearAsync(dto, cancellationToken);
        return Created(string.Empty, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] TDto dto,
        CancellationToken cancellationToken)
    {
        bool updated = await _service.ActualizarAsync(id, dto, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        bool deleted = await _service.EliminarAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
