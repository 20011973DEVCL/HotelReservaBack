namespace HotelReservaBack.Domain.Entities;

public class Hotel
{
    public int HotCodigo { get; set; }
    public string HotNombre { get; set; } = string.Empty;
    public string HotDireccion { get; set; } = string.Empty;
    public string? HotTelefono { get; set; }
    public string? HotEmail { get; set; }
    public int HotEstrellas { get; set; }
    public bool HotActivo { get; set; }
    public int ComCodigo { get; set; }
    public string ComNombre { get; set; } = string.Empty;
}