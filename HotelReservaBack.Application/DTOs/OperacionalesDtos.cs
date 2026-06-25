using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelReservaBack.Application.DTOs;

public class UsuarioDto
{
    public int UsuCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int PerCodigo { get; set; }
    [Required, StringLength(120)]
    public string UsuNombre { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)]
    public string UsuEmail { get; set; } = string.Empty;
    [StringLength(200, MinimumLength = 8)]
    public string? UsuClave { get; set; }
    [JsonIgnore]
    public string UsuClaveHash { get; set; } = string.Empty;
    public bool UsuActivo { get; set; } = true;
    public DateTime UsuFechaCrea { get; set; }
}

public class HabitacionDto
{
    public int HabCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int HotCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int ThaCodigo { get; set; }
    [Required, StringLength(20)]
    public string HabNumero { get; set; } = string.Empty;
    [Range(0, int.MaxValue)]
    public int HabPiso { get; set; } = 1;
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal HabPrecioNoche { get; set; }
    public bool HabActiva { get; set; } = true;
}

public class ClienteDto
{
    public int CliCodigo { get; set; }
    [StringLength(20)]
    public string? CliRut { get; set; }
    [Required, StringLength(120)]
    public string CliNombre { get; set; } = string.Empty;
    [Required, StringLength(120)]
    public string CliApellido { get; set; } = string.Empty;
    [EmailAddress, StringLength(150)]
    public string? CliEmail { get; set; }
    [StringLength(30)]
    public string? CliTelefono { get; set; }
    [StringLength(200)]
    public string? CliDireccion { get; set; }
    public bool CliActivo { get; set; } = true;
    public DateTime CliFechaCrea { get; set; }
}

public class ReservaDto : IValidatableObject
{
    public int ResCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int CliCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int HotCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int EreCodigo { get; set; }
    public DateOnly ResFechaDesde { get; set; }
    public DateOnly ResFechaHasta { get; set; }
    [Range(1, int.MaxValue)]
    public int ResCantidadAdulto { get; set; } = 1;
    [Range(0, int.MaxValue)]
    public int ResCantidadNino { get; set; }
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal ResTotal { get; set; }
    [StringLength(300)]
    public string? ResObservacion { get; set; }
    public DateTime ResFechaCrea { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ResFechaHasta <= ResFechaDesde)
        {
            yield return new ValidationResult(
                "La fecha hasta debe ser posterior a la fecha desde.",
                [nameof(ResFechaHasta)]);
        }
    }
}

public class ReservaDetalleDto
{
    public int RdeCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int ResCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int HabCodigo { get; set; }
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal RdePrecioNoche { get; set; }
    [Range(1, int.MaxValue)]
    public int RdeCantidadNoches { get; set; }
    [Range(typeof(decimal), "0", "9999999999.99")]
    public decimal RdeSubtotal { get; set; }
}

public class PagoDto
{
    public int PagCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int ResCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int EpaCodigo { get; set; }
    [Range(typeof(decimal), "0.01", "9999999999.99")]
    public decimal PagMonto { get; set; }
    [Required, StringLength(80)]
    public string PagMetodo { get; set; } = string.Empty;
    [StringLength(120)]
    public string? PagReferencia { get; set; }
    public DateTime PagFecha { get; set; }
    [StringLength(250)]
    public string? PagObservacion { get; set; }
}
