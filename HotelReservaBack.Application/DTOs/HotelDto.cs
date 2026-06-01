namespace HotelReservaBack.Application.DTOs;

public class HotelDto
{
    public int HotCodigo { get; set; }
    public string HotNombre { get; set; } = string.Empty;
    public string HotDireccion { get; set; } = string.Empty;
    public bool HotActivo { get; set; }
}