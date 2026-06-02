-- ============================================================
-- Proyecto      : ReservaHotelBack
-- Archivo       : 05_verificar_datos.sql
-- Descripcion   : Verifica cantidad de registros cargados
-- ============================================================

SELECT 'paises' AS tabla, COUNT(*) AS cantidad FROM paises
UNION ALL
SELECT 'regiones', COUNT(*) FROM regiones
UNION ALL
SELECT 'ciudades', COUNT(*) FROM ciudades
UNION ALL
SELECT 'comunas', COUNT(*) FROM comunas
UNION ALL
SELECT 'perfiles', COUNT(*) FROM perfiles
UNION ALL
SELECT 'usuarios', COUNT(*) FROM usuarios
UNION ALL
SELECT 'estados_reserva', COUNT(*) FROM estados_reserva
UNION ALL
SELECT 'estados_pago', COUNT(*) FROM estados_pago
UNION ALL
SELECT 'hoteles', COUNT(*) FROM hoteles
UNION ALL
SELECT 'tipos_habitacion', COUNT(*) FROM tipos_habitacion
UNION ALL
SELECT 'habitaciones', COUNT(*) FROM habitaciones
UNION ALL
SELECT 'clientes', COUNT(*) FROM clientes
UNION ALL
SELECT 'reservas', COUNT(*) FROM reservas
UNION ALL
SELECT 'reservas_detalles', COUNT(*) FROM reservas_detalles
UNION ALL
SELECT 'pagos', COUNT(*) FROM pagos
ORDER BY tabla;

-- Vista rapida para revisar reservas con datos relacionados

SELECT
    r.res_codigo,
    c.cli_nombre || ' ' || c.cli_apellido AS cliente,
    h.hot_nombre,
    er.ere_nombre AS estado_reserva,
    r.res_fecha_desde,
    r.res_fecha_hasta,
    r.res_total
FROM reservas r
INNER JOIN clientes c ON c.cli_codigo = r.cli_codigo
INNER JOIN hoteles h ON h.hot_codigo = r.hot_codigo
INNER JOIN estados_reserva er ON er.ere_codigo = r.ere_codigo
ORDER BY r.res_codigo;
