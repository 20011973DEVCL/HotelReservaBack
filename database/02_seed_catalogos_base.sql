-- ============================================================
-- Proyecto      : ReservaHotelBack
-- Archivo       : 02_seed_catalogos_base.sql
-- Descripcion   : Pobla catalogos base para Reserva Hotel
-- ============================================================

BEGIN;

-- ============================================================
-- PAISES
-- ============================================================

INSERT INTO paises (pai_codigo, pai_nombre, pai_activo) VALUES
(1, 'Chile', TRUE),
(2, 'Argentina', TRUE),
(3, 'Brasil', TRUE),
(4, 'Peru', TRUE),
(5, 'Uruguay', TRUE)
ON CONFLICT (pai_codigo) DO UPDATE SET
    pai_nombre = EXCLUDED.pai_nombre,
    pai_activo = EXCLUDED.pai_activo;

SELECT setval(pg_get_serial_sequence('paises', 'pai_codigo'), COALESCE(MAX(pai_codigo), 1), TRUE) FROM paises;

-- ============================================================
-- REGIONES
-- ============================================================

INSERT INTO regiones (reg_codigo, pai_codigo, reg_nombre, reg_activo) VALUES
(1, 1, 'Region Metropolitana de Santiago', TRUE),
(2, 1, 'Region de Valparaiso', TRUE),
(3, 1, 'Region de Coquimbo', TRUE),
(4, 1, 'Region de Los Lagos', TRUE),
(5, 1, 'Region de Magallanes y de la Antartica Chilena', TRUE),
(6, 1, 'Region de Antofagasta', TRUE),
(7, 1, 'Region de La Araucania', TRUE)
ON CONFLICT (reg_codigo) DO UPDATE SET
    pai_codigo = EXCLUDED.pai_codigo,
    reg_nombre = EXCLUDED.reg_nombre,
    reg_activo = EXCLUDED.reg_activo;

SELECT setval(pg_get_serial_sequence('regiones', 'reg_codigo'), COALESCE(MAX(reg_codigo), 1), TRUE) FROM regiones;

-- ============================================================
-- CIUDADES
-- ============================================================

INSERT INTO ciudades (ciu_codigo, reg_codigo, ciu_nombre, ciu_activo) VALUES
(1, 1, 'Santiago', TRUE),
(2, 2, 'Valparaiso', TRUE),
(3, 2, 'Vina del Mar', TRUE),
(4, 3, 'La Serena', TRUE),
(5, 4, 'Puerto Montt', TRUE),
(6, 5, 'Punta Arenas', TRUE),
(7, 6, 'San Pedro de Atacama', TRUE),
(8, 7, 'Pucon', TRUE)
ON CONFLICT (ciu_codigo) DO UPDATE SET
    reg_codigo = EXCLUDED.reg_codigo,
    ciu_nombre = EXCLUDED.ciu_nombre,
    ciu_activo = EXCLUDED.ciu_activo;

SELECT setval(pg_get_serial_sequence('ciudades', 'ciu_codigo'), COALESCE(MAX(ciu_codigo), 1), TRUE) FROM ciudades;

-- ============================================================
-- COMUNAS
-- ============================================================

INSERT INTO comunas (com_codigo, ciu_codigo, com_nombre, com_activo) VALUES
(1, 1, 'Santiago', TRUE),
(2, 1, 'Providencia', TRUE),
(3, 1, 'Las Condes', TRUE),
(4, 2, 'Valparaiso', TRUE),
(5, 3, 'Vina del Mar', TRUE),
(6, 4, 'La Serena', TRUE),
(7, 5, 'Puerto Montt', TRUE),
(8, 6, 'Punta Arenas', TRUE),
(9, 7, 'San Pedro de Atacama', TRUE),
(10, 8, 'Pucon', TRUE)
ON CONFLICT (com_codigo) DO UPDATE SET
    ciu_codigo = EXCLUDED.ciu_codigo,
    com_nombre = EXCLUDED.com_nombre,
    com_activo = EXCLUDED.com_activo;

SELECT setval(pg_get_serial_sequence('comunas', 'com_codigo'), COALESCE(MAX(com_codigo), 1), TRUE) FROM comunas;

-- ============================================================
-- PERFILES
-- ============================================================

INSERT INTO perfiles (per_codigo, per_nombre, per_activo) VALUES
(1, 'Administrador', TRUE),
(2, 'Recepcionista', TRUE),
(3, 'Supervisor', TRUE),
(4, 'Consulta', TRUE)
ON CONFLICT (per_codigo) DO UPDATE SET
    per_nombre = EXCLUDED.per_nombre,
    per_activo = EXCLUDED.per_activo;

SELECT setval(pg_get_serial_sequence('perfiles', 'per_codigo'), COALESCE(MAX(per_codigo), 1), TRUE) FROM perfiles;

-- ============================================================
-- USUARIOS
-- Nota:
--   usu_clave_hash contiene valores de prueba.
--   Mas adelante podemos reemplazarlo por hash real.
-- ============================================================

INSERT INTO usuarios (usu_codigo, per_codigo, usu_nombre, usu_email, usu_clave_hash, usu_activo) VALUES
(1, 1, 'Administrador Sistema', 'admin@reservahotel.local', 'HASH_DE_PRUEBA_ADMIN', TRUE),
(2, 2, 'Recepcion Hotel', 'recepcion@reservahotel.local', 'HASH_DE_PRUEBA_RECEPCION', TRUE),
(3, 3, 'Supervisor Hotel', 'supervisor@reservahotel.local', 'HASH_DE_PRUEBA_SUPERVISOR', TRUE)
ON CONFLICT (usu_codigo) DO UPDATE SET
    per_codigo = EXCLUDED.per_codigo,
    usu_nombre = EXCLUDED.usu_nombre,
    usu_email = EXCLUDED.usu_email,
    usu_clave_hash = EXCLUDED.usu_clave_hash,
    usu_activo = EXCLUDED.usu_activo;

SELECT setval(pg_get_serial_sequence('usuarios', 'usu_codigo'), COALESCE(MAX(usu_codigo), 1), TRUE) FROM usuarios;

-- ============================================================
-- ESTADOS DE RESERVA
-- ============================================================

INSERT INTO estados_reserva (ere_codigo, ere_nombre, ere_activo) VALUES
(1, 'Pendiente', TRUE),
(2, 'Confirmada', TRUE),
(3, 'En curso', TRUE),
(4, 'Finalizada', TRUE),
(5, 'Cancelada', TRUE)
ON CONFLICT (ere_codigo) DO UPDATE SET
    ere_nombre = EXCLUDED.ere_nombre,
    ere_activo = EXCLUDED.ere_activo;

SELECT setval(pg_get_serial_sequence('estados_reserva', 'ere_codigo'), COALESCE(MAX(ere_codigo), 1), TRUE) FROM estados_reserva;

-- ============================================================
-- ESTADOS DE PAGO
-- ============================================================

INSERT INTO estados_pago (epa_codigo, epa_nombre, epa_activo) VALUES
(1, 'Pendiente', TRUE),
(2, 'Pagado', TRUE),
(3, 'Parcial', TRUE),
(4, 'Anulado', TRUE)
ON CONFLICT (epa_codigo) DO UPDATE SET
    epa_nombre = EXCLUDED.epa_nombre,
    epa_activo = EXCLUDED.epa_activo;

SELECT setval(pg_get_serial_sequence('estados_pago', 'epa_codigo'), COALESCE(MAX(epa_codigo), 1), TRUE) FROM estados_pago;

COMMIT;
