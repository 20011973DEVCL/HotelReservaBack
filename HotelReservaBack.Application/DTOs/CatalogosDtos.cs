using System.ComponentModel.DataAnnotations;

namespace HotelReservaBack.Application.DTOs;

public class PaisDto
{
    public int PaiCodigo { get; set; }
    [Required, StringLength(100)]
    public string PaiNombre { get; set; } = string.Empty;
    public bool PaiActivo { get; set; } = true;
}

public class RegionDto
{
    public int RegCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int PaiCodigo { get; set; }
    [Required, StringLength(120)]
    public string RegNombre { get; set; } = string.Empty;
    public bool RegActivo { get; set; } = true;
}

public class CiudadDto
{
    public int CiuCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int RegCodigo { get; set; }
    [Required, StringLength(120)]
    public string CiuNombre { get; set; } = string.Empty;
    public bool CiuActivo { get; set; } = true;
}

public class ComunaDto
{
    public int ComCodigo { get; set; }
    [Range(1, int.MaxValue)]
    public int CiuCodigo { get; set; }
    [Required, StringLength(120)]
    public string ComNombre { get; set; } = string.Empty;
    public bool ComActivo { get; set; } = true;
}

public class PerfilDto
{
    public int PerCodigo { get; set; }
    [Required, StringLength(80)]
    public string PerNombre { get; set; } = string.Empty;
    public bool PerActivo { get; set; } = true;
}

public class EstadoReservaDto
{
    public int EreCodigo { get; set; }
    [Required, StringLength(80)]
    public string EreNombre { get; set; } = string.Empty;
    public bool EreActivo { get; set; } = true;
}

public class EstadoPagoDto
{
    public int EpaCodigo { get; set; }
    [Required, StringLength(80)]
    public string EpaNombre { get; set; } = string.Empty;
    public bool EpaActivo { get; set; } = true;
}

public class TipoHabitacionDto
{
    public int ThaCodigo { get; set; }
    [Required, StringLength(100)]
    public string ThaNombre { get; set; } = string.Empty;
    [StringLength(250)]
    public string? ThaDescripcion { get; set; }
    [Range(1, int.MaxValue)]
    public int ThaCapacidad { get; set; }
    public bool ThaActivo { get; set; } = true;
}
