using System.ComponentModel.DataAnnotations;

namespace HotelReservaBack.Application.DTOs;

public class HotelDto
{
    public int HotCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int ComCodigo { get; set; }
    [Required, StringLength(150)]
    public string HotNombre { get; set; } = string.Empty;
    [Required, StringLength(200)]
    public string HotDireccion { get; set; } = string.Empty;
    [StringLength(30)]
    public string? HotTelefono { get; set; }
    [EmailAddress, StringLength(150)]
    public string? HotEmail { get; set; }
    [Range(1, 5)]
    public int HotEstrellas { get; set; } = 3;
    public bool HotActivo { get; set; } = true;
    public string ComNombre { get; set; } = string.Empty;
    public DateTime HotFechaCrea { get; set; }
}
