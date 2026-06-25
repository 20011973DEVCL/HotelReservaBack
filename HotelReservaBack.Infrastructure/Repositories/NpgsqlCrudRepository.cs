using System.Reflection;

using HotelReservaBack.Domain.Entities;
using HotelReservaBack.Domain.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace HotelReservaBack.Infrastructure.Repositories;

public sealed class NpgsqlCrudRepository<TEntity>(IConfiguration configuration) : ICrudRepository<TEntity>
    where TEntity : class, new()
{
    private readonly string _connectionString = configuration.GetConnectionString("HotelReserva")
        ?? throw new InvalidOperationException("No existe la cadena de conexión HotelReserva.");
    private readonly EntityMap _map = EntityMaps.Get<TEntity>();

    public async Task<IReadOnlyList<TEntity>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        List<TEntity> entities = [];
        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        string sql = $"SELECT {_map.SelectColumns} FROM {_map.ReadFrom} ORDER BY {_map.IdColumn}";
        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            entities.Add(MapEntity(reader));
        }

        return entities;
    }

    public async Task<TEntity?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        string sql = $"SELECT {_map.SelectColumns} FROM {_map.ReadFrom} WHERE {_map.QualifiedIdColumn} = @id";
        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        _ = command.Parameters.AddWithValue("id", id);
        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        return await reader.ReadAsync(cancellationToken) ? MapEntity(reader) : null;
    }

    public async Task<TEntity> CrearAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ColumnMap> columns = _map.Columns.Where(column => column.Insertable).ToArray();
        string columnNames = string.Join(", ", columns.Select(column => column.WriteColumn));
        string parameterNames = string.Join(", ", columns.Select((_, index) => $"@p{index}"));
        string sql = $"INSERT INTO {_map.Table} ({columnNames}) VALUES ({parameterNames}) RETURNING {_map.IdColumn}";

        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        AddParameters(command, entity, columns);

        object? result = await command.ExecuteScalarAsync(cancellationToken);
        int id = Convert.ToInt32(result);
        _map.IdProperty.SetValue(entity, id);

        return await ObtenerPorIdAsync(id, cancellationToken) ?? entity;
    }

    public async Task<bool> ActualizarAsync(
        int id,
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ColumnMap> columns = _map.Columns.Where(column => column.Updatable).ToArray();
        string assignments = string.Join(
            ", ",
            columns.Select((column, index) => $"{column.WriteColumn} = @p{index}"));
        string sql = $"UPDATE {_map.Table} SET {assignments} WHERE {_map.IdColumn} = @id";

        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        AddParameters(command, entity, columns);
        _ = command.Parameters.AddWithValue("id", id);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        await using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        string sql = $"DELETE FROM {_map.Table} WHERE {_map.IdColumn} = @id";
        await using NpgsqlCommand command = new NpgsqlCommand(sql, connection);
        _ = command.Parameters.AddWithValue("id", id);

        return await command.ExecuteNonQueryAsync(cancellationToken) > 0;
    }

    private static void AddParameters(
        NpgsqlCommand command,
        TEntity entity,
        IReadOnlyList<ColumnMap> columns)
    {
        for (int index = 0; index < columns.Count; index++)
        {
            object? value = columns[index].Property.GetValue(entity);
            _ = command.Parameters.AddWithValue($"p{index}", value ?? DBNull.Value);
        }
    }

    private TEntity MapEntity(NpgsqlDataReader reader)
    {
        TEntity entity = new TEntity();

        foreach (ColumnMap column in _map.Columns)
        {
            int ordinal = reader.GetOrdinal(column.Property.Name);
            if (!reader.IsDBNull(ordinal))
            {
                column.Property.SetValue(entity, reader.GetValue(ordinal));
            }
        }

        return entity;
    }
}

internal sealed class EntityMap(
    Type entityType,
    string table,
    string idPropertyName,
    string idColumn,
    IReadOnlyList<ColumnMap> columns,
    string? readFrom = null,
    string? qualifiedIdColumn = null)
{
    public Type EntityType { get; } = entityType;
    public string Table { get; } = table;
    public string IdColumn { get; } = idColumn;
    public string ReadFrom { get; } = readFrom ?? table;
    public string QualifiedIdColumn { get; } = qualifiedIdColumn ?? idColumn;
    public PropertyInfo IdProperty { get; } = entityType.GetProperty(idPropertyName)
        ?? throw new InvalidOperationException($"No existe la propiedad {idPropertyName} en {entityType.Name}.");
    public IReadOnlyList<ColumnMap> Columns { get; } = columns;
    public string SelectColumns { get; } = string.Join(
        ", ",
        columns.Select(column => $"{column.ReadColumn} AS \"{column.Property.Name}\""));
}

internal sealed class ColumnMap
{
    public ColumnMap(
        Type entityType,
        string propertyName,
        string readColumn,
        string? writeColumn = null,
        bool insertable = true,
        bool updatable = true)
    {
        Property = entityType.GetProperty(propertyName)
            ?? throw new InvalidOperationException($"No existe la propiedad {propertyName} en {entityType.Name}.");
        ReadColumn = readColumn;
        WriteColumn = writeColumn ?? readColumn;
        Insertable = insertable;
        Updatable = updatable;
    }

    public PropertyInfo Property { get; }
    public string ReadColumn { get; }
    public string WriteColumn { get; }
    public bool Insertable { get; }
    public bool Updatable { get; }
}

internal static class EntityMaps
{
    private static readonly IReadOnlyDictionary<Type, EntityMap> Maps = CreateMaps();

    public static EntityMap Get<TEntity>()
        where TEntity : class
    {
        return Maps.TryGetValue(typeof(TEntity), out EntityMap? map)
            ? map
            : throw new InvalidOperationException($"No existe un mapa SQL para {typeof(TEntity).Name}.");
    }

    private static IReadOnlyDictionary<Type, EntityMap> CreateMaps()
    {
        EntityMap[] maps =
        [
            Map<Pais>("paises", "PaiCodigo", "pai_codigo",
                C<Pais>("PaiCodigo", "pai_codigo", false, false),
                C<Pais>("PaiNombre", "pai_nombre"),
                C<Pais>("PaiActivo", "pai_activo")),
            Map<Region>("regiones", "RegCodigo", "reg_codigo",
                C<Region>("RegCodigo", "reg_codigo", false, false),
                C<Region>("PaiCodigo", "pai_codigo"),
                C<Region>("RegNombre", "reg_nombre"),
                C<Region>("RegActivo", "reg_activo")),
            Map<Ciudad>("ciudades", "CiuCodigo", "ciu_codigo",
                C<Ciudad>("CiuCodigo", "ciu_codigo", false, false),
                C<Ciudad>("RegCodigo", "reg_codigo"),
                C<Ciudad>("CiuNombre", "ciu_nombre"),
                C<Ciudad>("CiuActivo", "ciu_activo")),
            Map<Comuna>("comunas", "ComCodigo", "com_codigo",
                C<Comuna>("ComCodigo", "com_codigo", false, false),
                C<Comuna>("CiuCodigo", "ciu_codigo"),
                C<Comuna>("ComNombre", "com_nombre"),
                C<Comuna>("ComActivo", "com_activo")),
            Map<Perfil>("perfiles", "PerCodigo", "per_codigo",
                C<Perfil>("PerCodigo", "per_codigo", false, false),
                C<Perfil>("PerNombre", "per_nombre"),
                C<Perfil>("PerActivo", "per_activo")),
            Map<Usuario>("usuarios", "UsuCodigo", "usu_codigo",
                C<Usuario>("UsuCodigo", "usu_codigo", false, false),
                C<Usuario>("PerCodigo", "per_codigo"),
                C<Usuario>("UsuNombre", "usu_nombre"),
                C<Usuario>("UsuEmail", "usu_email"),
                C<Usuario>("UsuClaveHash", "usu_clave_hash"),
                C<Usuario>("UsuActivo", "usu_activo"),
                C<Usuario>("UsuFechaCrea", "usu_fecha_crea", false, false)),
            Map<EstadoReserva>("estados_reserva", "EreCodigo", "ere_codigo",
                C<EstadoReserva>("EreCodigo", "ere_codigo", false, false),
                C<EstadoReserva>("EreNombre", "ere_nombre"),
                C<EstadoReserva>("EreActivo", "ere_activo")),
            Map<EstadoPago>("estados_pago", "EpaCodigo", "epa_codigo",
                C<EstadoPago>("EpaCodigo", "epa_codigo", false, false),
                C<EstadoPago>("EpaNombre", "epa_nombre"),
                C<EstadoPago>("EpaActivo", "epa_activo")),
            HotelMap(),
            Map<TipoHabitacion>("tipos_habitacion", "ThaCodigo", "tha_codigo",
                C<TipoHabitacion>("ThaCodigo", "tha_codigo", false, false),
                C<TipoHabitacion>("ThaNombre", "tha_nombre"),
                C<TipoHabitacion>("ThaDescripcion", "tha_descripcion"),
                C<TipoHabitacion>("ThaCapacidad", "tha_capacidad"),
                C<TipoHabitacion>("ThaActivo", "tha_activo")),
            Map<Habitacion>("habitaciones", "HabCodigo", "hab_codigo",
                C<Habitacion>("HabCodigo", "hab_codigo", false, false),
                C<Habitacion>("HotCodigo", "hot_codigo"),
                C<Habitacion>("ThaCodigo", "tha_codigo"),
                C<Habitacion>("HabNumero", "hab_numero"),
                C<Habitacion>("HabPiso", "hab_piso"),
                C<Habitacion>("HabPrecioNoche", "hab_precio_noche"),
                C<Habitacion>("HabActiva", "hab_activa")),
            Map<Cliente>("clientes", "CliCodigo", "cli_codigo",
                C<Cliente>("CliCodigo", "cli_codigo", false, false),
                C<Cliente>("CliRut", "cli_rut"),
                C<Cliente>("CliNombre", "cli_nombre"),
                C<Cliente>("CliApellido", "cli_apellido"),
                C<Cliente>("CliEmail", "cli_email"),
                C<Cliente>("CliTelefono", "cli_telefono"),
                C<Cliente>("CliDireccion", "cli_direccion"),
                C<Cliente>("CliActivo", "cli_activo"),
                C<Cliente>("CliFechaCrea", "cli_fecha_crea", false, false)),
            Map<Reserva>("reservas", "ResCodigo", "res_codigo",
                C<Reserva>("ResCodigo", "res_codigo", false, false),
                C<Reserva>("CliCodigo", "cli_codigo"),
                C<Reserva>("HotCodigo", "hot_codigo"),
                C<Reserva>("EreCodigo", "ere_codigo"),
                C<Reserva>("ResFechaDesde", "res_fecha_desde"),
                C<Reserva>("ResFechaHasta", "res_fecha_hasta"),
                C<Reserva>("ResCantidadAdulto", "res_cantidad_adulto"),
                C<Reserva>("ResCantidadNino", "res_cantidad_nino"),
                C<Reserva>("ResTotal", "res_total"),
                C<Reserva>("ResObservacion", "res_observacion"),
                C<Reserva>("ResFechaCrea", "res_fecha_crea", false, false)),
            Map<ReservaDetalle>("reservas_detalles", "RdeCodigo", "rde_codigo",
                C<ReservaDetalle>("RdeCodigo", "rde_codigo", false, false),
                C<ReservaDetalle>("ResCodigo", "res_codigo"),
                C<ReservaDetalle>("HabCodigo", "hab_codigo"),
                C<ReservaDetalle>("RdePrecioNoche", "rde_precio_noche"),
                C<ReservaDetalle>("RdeCantidadNoches", "rde_cantidad_noches"),
                C<ReservaDetalle>("RdeSubtotal", "rde_subtotal")),
            Map<Pago>("pagos", "PagCodigo", "pag_codigo",
                C<Pago>("PagCodigo", "pag_codigo", false, false),
                C<Pago>("ResCodigo", "res_codigo"),
                C<Pago>("EpaCodigo", "epa_codigo"),
                C<Pago>("PagMonto", "pag_monto"),
                C<Pago>("PagMetodo", "pag_metodo"),
                C<Pago>("PagReferencia", "pag_referencia"),
                C<Pago>("PagFecha", "pag_fecha", false, false),
                C<Pago>("PagObservacion", "pag_observacion"))
        ];

        return maps.ToDictionary(map => map.EntityType);
    }

    private static EntityMap HotelMap()
    {
        Type type = typeof(Hotel);
        ColumnMap[] columns =
        [
            C<Hotel>("HotCodigo", "h.hot_codigo", false, false, "hot_codigo"),
            C<Hotel>("ComCodigo", "h.com_codigo", true, true, "com_codigo"),
            C<Hotel>("HotNombre", "h.hot_nombre", true, true, "hot_nombre"),
            C<Hotel>("HotDireccion", "h.hot_direccion", true, true, "hot_direccion"),
            C<Hotel>("HotTelefono", "h.hot_telefono", true, true, "hot_telefono"),
            C<Hotel>("HotEmail", "h.hot_email", true, true, "hot_email"),
            C<Hotel>("HotEstrellas", "h.hot_estrellas", true, true, "hot_estrellas"),
            C<Hotel>("HotActivo", "h.hot_activo", true, true, "hot_activo"),
            C<Hotel>("ComNombre", "c.com_nombre", false, false, "com_nombre"),
            C<Hotel>("HotFechaCrea", "h.hot_fecha_crea", false, false, "hot_fecha_crea")
        ];

        return new EntityMap(
            type,
            "hoteles",
            "HotCodigo",
            "hot_codigo",
            columns,
            "hoteles h INNER JOIN comunas c ON c.com_codigo = h.com_codigo",
            "h.hot_codigo");
    }

    private static EntityMap Map<TEntity>(
        string table,
        string idProperty,
        string idColumn,
        params ColumnMap[] columns)
    {
        return new EntityMap(typeof(TEntity), table, idProperty, idColumn, columns);
    }

    private static ColumnMap C<TEntity>(
        string property,
        string column,
        bool insertable = true,
        bool updatable = true,
        string? writeColumn = null)
    {
        return new ColumnMap(typeof(TEntity), property, column, writeColumn, insertable, updatable);
    }
}
