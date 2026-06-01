using HotelReservaBack.Application.DTOs;

namespace HotelReservaBack.Application.Interfaces;

public interface IHotelService
{
    Task<List<HotelDto>> ObtenerHotelesAsync();
}