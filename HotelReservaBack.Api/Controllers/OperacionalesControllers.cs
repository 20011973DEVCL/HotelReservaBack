using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HotelReservaBack.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class UsuariosController(ICrudService<UsuarioDto> service)
    : CrudControllerBase<UsuarioDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class HotelesController(ICrudService<HotelDto> service)
    : CrudControllerBase<HotelDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class HabitacionesController(ICrudService<HabitacionDto> service)
    : CrudControllerBase<HabitacionDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class ClientesController(ICrudService<ClienteDto> service)
    : CrudControllerBase<ClienteDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class ReservasController(ICrudService<ReservaDto> service)
    : CrudControllerBase<ReservaDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class ReservasDetallesController(ICrudService<ReservaDetalleDto> service)
    : CrudControllerBase<ReservaDetalleDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class PagosController(ICrudService<PagoDto> service)
    : CrudControllerBase<PagoDto>(service);
