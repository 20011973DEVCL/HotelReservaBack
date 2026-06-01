using HotelReservaBack.Domain.Entities;
using HotelReservaBack.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace HotelReservaBack.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly string _connectionString;

    public HotelRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("HotelReserva")
            ?? throw new Exception("No existe la cadena de conexión HotelReserva.");
    }

    public async Task<List<Hotel>> ObtenerHotelesAsync()
    {
        var lista = new List<Hotel>();

        await using var cn = new NpgsqlConnection(_connectionString);
        await cn.OpenAsync();

        await using var cmd = new NpgsqlCommand("SELECT * FROM sp_obtener_hoteles()", cn);

        await using var dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new Hotel
            {
                HotCodigo = dr.GetInt32(dr.GetOrdinal("hot_codigo")),
                HotNombre = dr.GetString(dr.GetOrdinal("hot_nombre")),
                HotDireccion = dr.GetString(dr.GetOrdinal("hot_direccion")),
                HotActivo = dr.GetBoolean(dr.GetOrdinal("hot_activo"))
            });
        }

        return lista;
    }
}