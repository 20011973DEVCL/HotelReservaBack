using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;
using HotelReservaBack.Domain.Interfaces;

namespace HotelReservaBack.Application.Services;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;

    public HotelService(IHotelRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    public async Task<List<HotelDto>> ObtenerHotelesAsync()
    {
        var hoteles = await _hotelRepository.ObtenerHotelesAsync();

        return hoteles.Select(h => new HotelDto
        {
            HotCodigo = h.HotCodigo,
            HotNombre = h.HotNombre,
            HotDireccion = h.HotDireccion,
            HotTelefono = h.HotTelefono,
            HotEmail = h.HotEmail,
            HotEstrellas = h.HotEstrellas,
            HotActivo = h.HotActivo,
            ComCodigo = h.ComCodigo,
            ComNombre = h.ComNombre
        }).ToList();
    }
}