-- ============================================================
-- Proyecto      : ReservaHotelBack
-- Archivo       : 04_seed_clientes_reservas_pagos.sql
-- Descripcion   : Pobla clientes, reservas, detalles y pagos
-- ============================================================

BEGIN;

-- ============================================================
-- CLIENTES
-- ============================================================

INSERT INTO clientes (
    cli_codigo,
    cli_rut,
    cli_nombre,
    cli_apellido,
    cli_email,
    cli_telefono,
    cli_direccion,
    cli_activo
) VALUES
(1, '11111111-1', 'Carlos', 'Munoz', 'carlos.munoz@email.local', '+56 9 1111 1111', 'Los Alerces 123, Santiago', TRUE),
(2, '22222222-2', 'Maria', 'Gonzalez', 'maria.gonzalez@email.local', '+56 9 2222 2222', 'Av. Libertad 456, Vina del Mar', TRUE),
(3, '33333333-3', 'Roberto', 'Silva', 'roberto.silva@email.local', '+56 9 3333 3333', 'Calle Central 789, Puerto Montt', TRUE),
(4, '44444444-4', 'Patricia', 'Rojas', 'patricia.rojas@email.local', '+56 9 4444 4444', 'Pasaje Norte 321, La Serena', TRUE),
(5, '55555555-5', 'Alvaro', 'Fuentes', 'alvaro.fuentes@email.local', '+56 9 5555 5555', 'Camino Sur 654, Santiago', TRUE)
ON CONFLICT (cli_codigo) DO UPDATE SET
    cli_rut = EXCLUDED.cli_rut,
    cli_nombre = EXCLUDED.cli_nombre,
    cli_apellido = EXCLUDED.cli_apellido,
    cli_email = EXCLUDED.cli_email,
    cli_telefono = EXCLUDED.cli_telefono,
    cli_direccion = EXCLUDED.cli_direccion,
    cli_activo = EXCLUDED.cli_activo;

SELECT setval(pg_get_serial_sequence('clientes', 'cli_codigo'), COALESCE(MAX(cli_codigo), 1), TRUE) FROM clientes;

-- ============================================================
-- RESERVAS
-- Estados:
--   1 Pendiente
--   2 Confirmada
--   3 En curso
--   4 Finalizada
--   5 Cancelada
-- ============================================================

INSERT INTO reservas (
    res_codigo,
    cli_codigo,
    hot_codigo,
    ere_codigo,
    res_fecha_desde,
    res_fecha_hasta,
    res_cantidad_adulto,
    res_cantidad_nino,
    res_total,
    res_observacion
) VALUES
(1, 1, 1, 2, '2026-07-10', '2026-07-13', 2, 0, 195000, 'Reserva de prueba confirmada'),
(2, 2, 2, 1, '2026-08-05', '2026-08-09', 2, 1, 356000, 'Pendiente de confirmacion'),
(3, 3, 4, 2, '2026-09-15', '2026-09-18', 2, 2, 297000, 'Viaje familiar al sur'),
(4, 4, 6, 3, '2026-06-01', '2026-06-04', 2, 0, 234000, 'Reserva actualmente en curso'),
(5, 5, 5, 4, '2026-05-10', '2026-05-12', 2, 0, 420000, 'Reserva finalizada'),
(6, 1, 7, 5, '2026-10-20', '2026-10-23', 2, 1, 276000, 'Reserva cancelada por el cliente')
ON CONFLICT (res_codigo) DO UPDATE SET
    cli_codigo = EXCLUDED.cli_codigo,
    hot_codigo = EXCLUDED.hot_codigo,
    ere_codigo = EXCLUDED.ere_codigo,
    res_fecha_desde = EXCLUDED.res_fecha_desde,
    res_fecha_hasta = EXCLUDED.res_fecha_hasta,
    res_cantidad_adulto = EXCLUDED.res_cantidad_adulto,
    res_cantidad_nino = EXCLUDED.res_cantidad_nino,
    res_total = EXCLUDED.res_total,
    res_observacion = EXCLUDED.res_observacion;

SELECT setval(pg_get_serial_sequence('reservas', 'res_codigo'), COALESCE(MAX(res_codigo), 1), TRUE) FROM reservas;

-- ============================================================
-- RESERVAS DETALLES
-- ============================================================

INSERT INTO reservas_detalles (
    rde_codigo,
    res_codigo,
    hab_codigo,
    rde_precio_noche,
    rde_cantidad_noches,
    rde_subtotal
) VALUES
(1, 1, 2, 65000, 3, 195000),
(2, 2, 7, 89000, 4, 356000),
(3, 3, 14, 99000, 3, 297000),
(4, 4, 20, 78000, 3, 234000),
(5, 5, 18, 210000, 2, 420000),
(6, 6, 23, 92000, 3, 276000)
ON CONFLICT (rde_codigo) DO UPDATE SET
    res_codigo = EXCLUDED.res_codigo,
    hab_codigo = EXCLUDED.hab_codigo,
    rde_precio_noche = EXCLUDED.rde_precio_noche,
    rde_cantidad_noches = EXCLUDED.rde_cantidad_noches,
    rde_subtotal = EXCLUDED.rde_subtotal;

SELECT setval(pg_get_serial_sequence('reservas_detalles', 'rde_codigo'), COALESCE(MAX(rde_codigo), 1), TRUE) FROM reservas_detalles;

-- ============================================================
-- PAGOS
-- Estados pago:
--   1 Pendiente
--   2 Pagado
--   3 Parcial
--   4 Anulado
-- ============================================================

INSERT INTO pagos (
    pag_codigo,
    res_codigo,
    epa_codigo,
    pag_monto,
    pag_metodo,
    pag_referencia,
    pag_observacion
) VALUES
(1, 1, 2, 195000, 'Tarjeta credito', 'TRX-RES-0001', 'Pago completo'),
(2, 2, 1, 0.01, 'Pendiente', 'PEND-RES-0002', 'Pago pendiente de regularizacion'),
(3, 3, 3, 150000, 'Transferencia', 'TRF-RES-0003', 'Abono parcial'),
(4, 4, 2, 234000, 'Tarjeta debito', 'TRX-RES-0004', 'Pago completo'),
(5, 5, 2, 420000, 'Transferencia', 'TRF-RES-0005', 'Pago completo'),
(6, 6, 4, 276000, 'Anulado', 'ANU-RES-0006', 'Pago anulado por cancelacion')
ON CONFLICT (pag_codigo) DO UPDATE SET
    res_codigo = EXCLUDED.res_codigo,
    epa_codigo = EXCLUDED.epa_codigo,
    pag_monto = EXCLUDED.pag_monto,
    pag_metodo = EXCLUDED.pag_metodo,
    pag_referencia = EXCLUDED.pag_referencia,
    pag_observacion = EXCLUDED.pag_observacion;

SELECT setval(pg_get_serial_sequence('pagos', 'pag_codigo'), COALESCE(MAX(pag_codigo), 1), TRUE) FROM pagos;

COMMIT;
