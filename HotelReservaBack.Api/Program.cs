using HotelReservaBack.Api.Infrastructure;
using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;
using HotelReservaBack.Application.Services;
using HotelReservaBack.Domain.Entities;
using HotelReservaBack.Domain.Interfaces;
using HotelReservaBack.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

builder.Services.AddScoped(typeof(ICrudRepository<>), typeof(NpgsqlCrudRepository<>));
builder.Services.AddScoped<ICrudService<PaisDto>, CrudService<Pais, PaisDto>>();
builder.Services.AddScoped<ICrudService<RegionDto>, CrudService<Region, RegionDto>>();
builder.Services.AddScoped<ICrudService<CiudadDto>, CrudService<Ciudad, CiudadDto>>();
builder.Services.AddScoped<ICrudService<ComunaDto>, CrudService<Comuna, ComunaDto>>();
builder.Services.AddScoped<ICrudService<PerfilDto>, CrudService<Perfil, PerfilDto>>();
builder.Services.AddScoped<ICrudService<UsuarioDto>, UsuarioService>();
builder.Services.AddScoped<ICrudService<EstadoReservaDto>, CrudService<EstadoReserva, EstadoReservaDto>>();
builder.Services.AddScoped<ICrudService<EstadoPagoDto>, CrudService<EstadoPago, EstadoPagoDto>>();
builder.Services.AddScoped<ICrudService<HotelDto>, CrudService<Hotel, HotelDto>>();
builder.Services.AddScoped<ICrudService<TipoHabitacionDto>, CrudService<TipoHabitacion, TipoHabitacionDto>>();
builder.Services.AddScoped<ICrudService<HabitacionDto>, CrudService<Habitacion, HabitacionDto>>();
builder.Services.AddScoped<ICrudService<ClienteDto>, CrudService<Cliente, ClienteDto>>();
builder.Services.AddScoped<ICrudService<ReservaDto>, CrudService<Reserva, ReservaDto>>();
builder.Services.AddScoped<ICrudService<ReservaDetalleDto>, CrudService<ReservaDetalle, ReservaDetalleDto>>();
builder.Services.AddScoped<ICrudService<PagoDto>, CrudService<Pago, PagoDto>>();

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
