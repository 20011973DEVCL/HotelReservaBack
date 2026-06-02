CREATE OR REPLACE FUNCTION sp_obtener_hoteles()
RETURNS TABLE (
    hot_codigo INTEGER,
    hot_nombre VARCHAR,
    hot_direccion VARCHAR,
    hot_telefono VARCHAR,
    hot_email VARCHAR,
    hot_estrellas INTEGER,
    hot_activo BOOLEAN,
    com_codigo INTEGER,
    com_nombre VARCHAR
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        h.hot_codigo,
        h.hot_nombre,
        h.hot_direccion,
        h.hot_telefono,
        h.hot_email,
        h.hot_estrellas,
        h.hot_activo,
        c.com_codigo,
        c.com_nombre
    FROM hoteles h
    INNER JOIN comunas c ON c.com_codigo = h.com_codigo
    ORDER BY h.hot_codigo;
END;
$$;