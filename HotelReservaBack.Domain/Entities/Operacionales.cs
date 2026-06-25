namespace HotelReservaBack.Domain.Entities;

public class Usuario
{
    public int UsuCodigo { get; set; }
    public int PerCodigo { get; set; }
    public string UsuNombre { get; set; } = string.Empty;
    public string UsuEmail { get; set; } = string.Empty;
    public string UsuClaveHash { get; set; } = string.Empty;
    public bool UsuActivo { get; set; }
    public DateTime UsuFechaCrea { get; set; }
}

public class Habitacion
{
    public int HabCodigo { get; set; }
    public int HotCodigo { get; set; }
    public int ThaCodigo { get; set; }
    public string HabNumero { get; set; } = string.Empty;
    public int HabPiso { get; set; }
    public decimal HabPrecioNoche { get; set; }
    public bool HabActiva { get; set; }
}

public class Cliente
{
    public int CliCodigo { get; set; }
    public string? CliRut { get; set; }
    public string CliNombre { get; set; } = string.Empty;
    public string CliApellido { get; set; } = string.Empty;
    public string? CliEmail { get; set; }
    public string? CliTelefono { get; set; }
    public string? CliDireccion { get; set; }
    public bool CliActivo { get; set; }
    public DateTime CliFechaCrea { get; set; }
}

public class Reserva
{
    public int ResCodigo { get; set; }
    public int CliCodigo { get; set; }
    public int HotCodigo { get; set; }
    public int EreCodigo { get; set; }
    public DateOnly ResFechaDesde { get; set; }
    public DateOnly ResFechaHasta { get; set; }
    public int ResCantidadAdulto { get; set; }
    public int ResCantidadNino { get; set; }
    public decimal ResTotal { get; set; }
    public string? ResObservacion { get; set; }
    public DateTime ResFechaCrea { get; set; }
}

public class ReservaDetalle
{
    public int RdeCodigo { get; set; }
    public int ResCodigo { get; set; }
    public int HabCodigo { get; set; }
    public decimal RdePrecioNoche { get; set; }
    public int RdeCantidadNoches { get; set; }
    public decimal RdeSubtotal { get; set; }
}

public class Pago
{
    public int PagCodigo { get; set; }
    public int ResCodigo { get; set; }
    public int EpaCodigo { get; set; }
    public decimal PagMonto { get; set; }
    public string PagMetodo { get; set; } = string.Empty;
    public string? PagReferencia { get; set; }
    public DateTime PagFecha { get; set; }
    public string? PagObservacion { get; set; }
}
