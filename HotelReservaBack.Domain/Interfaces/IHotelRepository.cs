using HotelReservaBack.Domain.Entities;

namespace HotelReservaBack.Domain.Interfaces;

public interface IHotelRepository
{
    Task<List<Hotel>> ObtenerHotelesAsync();
}
