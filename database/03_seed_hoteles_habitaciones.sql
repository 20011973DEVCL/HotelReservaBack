-- ============================================================
-- Proyecto      : ReservaHotelBack
-- Archivo       : 03_seed_hoteles_habitaciones.sql
-- Descripcion   : Pobla hoteles, tipos de habitacion y habitaciones
-- ============================================================

BEGIN;

-- ============================================================
-- HOTELES
-- ============================================================

INSERT INTO hoteles (
    hot_codigo,
    com_codigo,
    hot_nombre,
    hot_direccion,
    hot_telefono,
    hot_email,
    hot_estrellas,
    hot_activo
) VALUES
(1, 2, 'Hotel Providencia Plaza', 'Av. Providencia 1234', '+56 2 2222 1000', 'contacto@providenciaplaza.local', 4, TRUE),
(2, 5, 'Hotel Costa Pacifico', 'Av. San Martin 850', '+56 32 222 2000', 'reservas@costapacifico.local', 4, TRUE),
(3, 6, 'Hotel Serena Mar', 'Av. del Mar 1500', '+56 51 222 3000', 'contacto@serenamar.local', 3, TRUE),
(4, 7, 'Hotel Puerto Sur', 'Costanera 450', '+56 65 222 4000', 'reservas@puertosur.local', 4, TRUE),
(5, 8, 'Hotel Austral Patagonia', 'Av. Colon 900', '+56 61 222 5000', 'contacto@australpatagonia.local', 5, TRUE),
(6, 9, 'Hotel Desierto Andino', 'Calle Toconao 220', '+56 55 222 6000', 'reservas@desiertoandino.local', 4, TRUE),
(7, 10, 'Hotel Lago y Volcan', 'Camino al Volcan 700', '+56 45 222 7000', 'contacto@lagoyvolcan.local', 4, TRUE)
ON CONFLICT (hot_codigo) DO UPDATE SET
    com_codigo = EXCLUDED.com_codigo,
    hot_nombre = EXCLUDED.hot_nombre,
    hot_direccion = EXCLUDED.hot_direccion,
    hot_telefono = EXCLUDED.hot_telefono,
    hot_email = EXCLUDED.hot_email,
    hot_estrellas = EXCLUDED.hot_estrellas,
    hot_activo = EXCLUDED.hot_activo;

SELECT setval(pg_get_serial_sequence('hoteles', 'hot_codigo'), COALESCE(MAX(hot_codigo), 1), TRUE) FROM hoteles;

-- ============================================================
-- TIPOS DE HABITACION
-- ============================================================

INSERT INTO tipos_habitacion (
    tha_codigo,
    tha_nombre,
    tha_descripcion,
    tha_capacidad,
    tha_activo
) VALUES
(1, 'Single', 'Habitacion individual para una persona', 1, TRUE),
(2, 'Doble', 'Habitacion para dos personas', 2, TRUE),
(3, 'Triple', 'Habitacion para tres personas', 3, TRUE),
(4, 'Familiar', 'Habitacion amplia para grupo familiar', 4, TRUE),
(5, 'Suite', 'Habitacion superior con mayor comodidad', 2, TRUE)
ON CONFLICT (tha_codigo) DO UPDATE SET
    tha_nombre = EXCLUDED.tha_nombre,
    tha_descripcion = EXCLUDED.tha_descripcion,
    tha_capacidad = EXCLUDED.tha_capacidad,
    tha_activo = EXCLUDED.tha_activo;

SELECT setval(pg_get_serial_sequence('tipos_habitacion', 'tha_codigo'), COALESCE(MAX(tha_codigo), 1), TRUE) FROM tipos_habitacion;

-- ============================================================
-- HABITACIONES
-- ============================================================

INSERT INTO habitaciones (
    hab_codigo,
    hot_codigo,
    tha_codigo,
    hab_numero,
    hab_piso,
    hab_precio_noche,
    hab_activa
) VALUES
-- Hotel Providencia Plaza
(1, 1, 1, '101', 1, 45000, TRUE),
(2, 1, 2, '102', 1, 65000, TRUE),
(3, 1, 4, '201', 2, 95000, TRUE),
(4, 1, 5, '301', 3, 135000, TRUE),

-- Hotel Costa Pacifico
(5, 2, 1, '101', 1, 48000, TRUE),
(6, 2, 2, '102', 1, 72000, TRUE),
(7, 2, 3, '201', 2, 89000, TRUE),
(8, 2, 5, '501', 5, 155000, TRUE),

-- Hotel Serena Mar
(9, 3, 1, '101', 1, 39000, TRUE),
(10, 3, 2, '102', 1, 59000, TRUE),
(11, 3, 4, '202', 2, 87000, TRUE),

-- Hotel Puerto Sur
(12, 4, 1, '101', 1, 42000, TRUE),
(13, 4, 2, '102', 1, 68000, TRUE),
(14, 4, 4, '203', 2, 99000, TRUE),
(15, 4, 5, '401', 4, 145000, TRUE),

-- Hotel Austral Patagonia
(16, 5, 2, '201', 2, 85000, TRUE),
(17, 5, 4, '301', 3, 125000, TRUE),
(18, 5, 5, '501', 5, 210000, TRUE),

-- Hotel Desierto Andino
(19, 6, 1, '101', 1, 52000, TRUE),
(20, 6, 2, '102', 1, 78000, TRUE),
(21, 6, 5, '301', 3, 165000, TRUE),

-- Hotel Lago y Volcan
(22, 7, 2, '101', 1, 76000, TRUE),
(23, 7, 3, '102', 1, 92000, TRUE),
(24, 7, 4, '201', 2, 118000, TRUE),
(25, 7, 5, '301', 3, 175000, TRUE)
ON CONFLICT (hab_codigo) DO UPDATE SET
    hot_codigo = EXCLUDED.hot_codigo,
    tha_codigo = EXCLUDED.tha_codigo,
    hab_numero = EXCLUDED.hab_numero,
    hab_piso = EXCLUDED.hab_piso,
    hab_precio_noche = EXCLUDED.hab_precio_noche,
    hab_activa = EXCLUDED.hab_activa;

SELECT setval(pg_get_serial_sequence('habitaciones', 'hab_codigo'), COALESCE(MAX(hab_codigo), 1), TRUE) FROM habitaciones;

COMMIT;
