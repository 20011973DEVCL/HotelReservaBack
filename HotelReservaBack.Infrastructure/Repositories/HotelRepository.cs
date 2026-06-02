using HotelReservaBack.Domain.Entities;
using HotelReservaBack.Domain.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace HotelReservaBack.Infrastructure.Repositories;

public class HotelRepository(IConfiguration configuration) : IHotelRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("HotelReserva")
            ?? throw new Exception("No existe la cadena de conexión HotelReserva.");

    public async Task<List<Hotel>> ObtenerHotelesAsync()
    {
        List<Hotel> lista = new List<Hotel>();

        await using NpgsqlConnection cn = new NpgsqlConnection(_connectionString);
        await cn.OpenAsync();

        await using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sp_obtener_hoteles()", cn);

        await using NpgsqlDataReader dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new Hotel
            {
                HotCodigo = dr.GetInt32(dr.GetOrdinal("hot_codigo")),
                HotNombre = dr.GetString(dr.GetOrdinal("hot_nombre")),
                HotDireccion = dr.GetString(dr.GetOrdinal("hot_direccion")),
                HotTelefono = dr.IsDBNull(dr.GetOrdinal("hot_telefono")) ? null : dr.GetString(dr.GetOrdinal("hot_telefono")),
                HotEmail = dr.IsDBNull(dr.GetOrdinal("hot_email")) ? null : dr.GetString(dr.GetOrdinal("hot_email")),
                HotEstrellas = dr.GetInt32(dr.GetOrdinal("hot_estrellas")),
                HotActivo = dr.GetBoolean(dr.GetOrdinal("hot_activo")),
                ComCodigo = dr.GetInt32(dr.GetOrdinal("com_codigo")),
                ComNombre = dr.GetString(dr.GetOrdinal("com_nombre"))
            });
        }

        return lista;
    }
}
