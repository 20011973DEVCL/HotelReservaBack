namespace HotelReservaBack.Domain.Entities;

public class Pais
{
    public int PaiCodigo { get; set; }
    public string PaiNombre { get; set; } = string.Empty;
    public bool PaiActivo { get; set; }
}

public class Region
{
    public int RegCodigo { get; set; }
    public int PaiCodigo { get; set; }
    public string RegNombre { get; set; } = string.Empty;
    public bool RegActivo { get; set; }
}

public class Ciudad
{
    public int CiuCodigo { get; set; }
    public int RegCodigo { get; set; }
    public string CiuNombre { get; set; } = string.Empty;
    public bool CiuActivo { get; set; }
}

public class Comuna
{
    public int ComCodigo { get; set; }
    public int CiuCodigo { get; set; }
    public string ComNombre { get; set; } = string.Empty;
    public bool ComActivo { get; set; }
}

public class Perfil
{
    public int PerCodigo { get; set; }
    public string PerNombre { get; set; } = string.Empty;
    public bool PerActivo { get; set; }
}

public class EstadoReserva
{
    public int EreCodigo { get; set; }
    public string EreNombre { get; set; } = string.Empty;
    public bool EreActivo { get; set; }
}

public class EstadoPago
{
    public int EpaCodigo { get; set; }
    public string EpaNombre { get; set; } = string.Empty;
    public bool EpaActivo { get; set; }
}

public class TipoHabitacion
{
    public int ThaCodigo { get; set; }
    public string ThaNombre { get; set; } = string.Empty;
    public string? ThaDescripcion { get; set; }
    public int ThaCapacidad { get; set; }
    public bool ThaActivo { get; set; }
}
