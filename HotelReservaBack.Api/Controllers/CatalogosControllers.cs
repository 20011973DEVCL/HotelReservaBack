using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HotelReservaBack.Api.Controllers;

[ApiController, Route("api/[controller]")]
public sealed class PaisesController(ICrudService<PaisDto> service) : CrudControllerBase<PaisDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class RegionesController(ICrudService<RegionDto> service) : CrudControllerBase<RegionDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class CiudadesController(ICrudService<CiudadDto> service) : CrudControllerBase<CiudadDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class ComunasController(ICrudService<ComunaDto> service) : CrudControllerBase<ComunaDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class PerfilesController(ICrudService<PerfilDto> service) : CrudControllerBase<PerfilDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class EstadosReservaController(ICrudService<EstadoReservaDto> service)
    : CrudControllerBase<EstadoReservaDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class EstadosPagoController(ICrudService<EstadoPagoDto> service)
    : CrudControllerBase<EstadoPagoDto>(service);

[ApiController, Route("api/[controller]")]
public sealed class TiposHabitacionController(ICrudService<TipoHabitacionDto> service)
    : CrudControllerBase<TipoHabitacionDto>(service);
