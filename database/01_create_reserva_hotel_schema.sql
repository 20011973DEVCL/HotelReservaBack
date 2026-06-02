-- ============================================================
-- Proyecto      : ReservaHotelBack
-- Archivo       : 01_create_reserva_hotel_schema.sql
-- Base de datos : PostgreSQL
-- Descripcion   : Creacion de tablas principales para sistema
--                 de reservas de hotel.
--
-- Convencion:
--   - Tablas en plural o nombre funcional.
--   - Columnas con prefijo logico de 3 letras.
--   - Nombres fisicos en snake_case.
--   - DTOs en C# podran usar nombres reales tipo PascalCase.
--
-- Ejemplo:
--   hot_codigo      -> HotCodigo
--   hot_nombre      -> HotNombre
--   res_fecha_desde -> ResFechaDesde
-- ============================================================

-- ============================================================
-- LIMPIEZA CONTROLADA
-- OJO:
--   Para recrear todo desde cero, descomenta los DROP TABLE.
--   Mantener comentado si ya tienes datos reales.
-- ============================================================

-- DROP TABLE IF EXISTS pagos CASCADE;
-- DROP TABLE IF EXISTS reservas_detalles CASCADE;
-- DROP TABLE IF EXISTS reservas CASCADE;
-- DROP TABLE IF EXISTS habitaciones CASCADE;
-- DROP TABLE IF EXISTS tipos_habitacion CASCADE;
-- DROP TABLE IF EXISTS clientes CASCADE;
-- DROP TABLE IF EXISTS hoteles CASCADE;
-- DROP TABLE IF EXISTS comunas CASCADE;
-- DROP TABLE IF EXISTS ciudades CASCADE;
-- DROP TABLE IF EXISTS regiones CASCADE;
-- DROP TABLE IF EXISTS paises CASCADE;
-- DROP TABLE IF EXISTS estados_pago CASCADE;
-- DROP TABLE IF EXISTS estados_reserva CASCADE;
-- DROP TABLE IF EXISTS usuarios CASCADE;
-- DROP TABLE IF EXISTS perfiles CASCADE;

-- ============================================================
-- CATALOGOS GEOGRAFICOS
-- ============================================================

CREATE TABLE IF NOT EXISTS paises (
    pai_codigo      SERIAL PRIMARY KEY,
    pai_nombre      VARCHAR(100) NOT NULL,
    pai_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_paises_nombre UNIQUE (pai_nombre)
);

CREATE TABLE IF NOT EXISTS regiones (
    reg_codigo      SERIAL PRIMARY KEY,
    pai_codigo      INTEGER NOT NULL,
    reg_nombre      VARCHAR(120) NOT NULL,
    reg_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT fk_regiones_paises
        FOREIGN KEY (pai_codigo)
        REFERENCES paises (pai_codigo),

    CONSTRAINT uq_regiones_pais_nombre UNIQUE (pai_codigo, reg_nombre)
);

CREATE TABLE IF NOT EXISTS ciudades (
    ciu_codigo      SERIAL PRIMARY KEY,
    reg_codigo      INTEGER NOT NULL,
    ciu_nombre      VARCHAR(120) NOT NULL,
    ciu_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT fk_ciudades_regiones
        FOREIGN KEY (reg_codigo)
        REFERENCES regiones (reg_codigo),

    CONSTRAINT uq_ciudades_region_nombre UNIQUE (reg_codigo, ciu_nombre)
);

CREATE TABLE IF NOT EXISTS comunas (
    com_codigo      SERIAL PRIMARY KEY,
    ciu_codigo      INTEGER NOT NULL,
    com_nombre      VARCHAR(120) NOT NULL,
    com_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT fk_comunas_ciudades
        FOREIGN KEY (ciu_codigo)
        REFERENCES ciudades (ciu_codigo),

    CONSTRAINT uq_comunas_ciudad_nombre UNIQUE (ciu_codigo, com_nombre)
);

-- ============================================================
-- SEGURIDAD BASICA
-- ============================================================

CREATE TABLE IF NOT EXISTS perfiles (
    per_codigo      SERIAL PRIMARY KEY,
    per_nombre      VARCHAR(80) NOT NULL,
    per_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_perfiles_nombre UNIQUE (per_nombre)
);

CREATE TABLE IF NOT EXISTS usuarios (
    usu_codigo      SERIAL PRIMARY KEY,
    per_codigo      INTEGER NOT NULL,
    usu_nombre      VARCHAR(120) NOT NULL,
    usu_email       VARCHAR(150) NOT NULL,
    usu_clave_hash  TEXT NOT NULL,
    usu_activo      BOOLEAN NOT NULL DEFAULT TRUE,
    usu_fecha_crea  TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_usuarios_perfiles
        FOREIGN KEY (per_codigo)
        REFERENCES perfiles (per_codigo),

    CONSTRAINT uq_usuarios_email UNIQUE (usu_email)
);

-- ============================================================
-- CATALOGOS DE RESERVA Y PAGO
-- ============================================================

CREATE TABLE IF NOT EXISTS estados_reserva (
    ere_codigo      SERIAL PRIMARY KEY,
    ere_nombre      VARCHAR(80) NOT NULL,
    ere_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_estados_reserva_nombre UNIQUE (ere_nombre)
);

CREATE TABLE IF NOT EXISTS estados_pago (
    epa_codigo      SERIAL PRIMARY KEY,
    epa_nombre      VARCHAR(80) NOT NULL,
    epa_activo      BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_estados_pago_nombre UNIQUE (epa_nombre)
);

-- ============================================================
-- HOTELES
-- ============================================================

CREATE TABLE IF NOT EXISTS hoteles (
    hot_codigo      SERIAL PRIMARY KEY,
    com_codigo      INTEGER NOT NULL,
    hot_nombre      VARCHAR(150) NOT NULL,
    hot_direccion   VARCHAR(200) NOT NULL,
    hot_telefono    VARCHAR(30),
    hot_email       VARCHAR(150),
    hot_estrellas   INTEGER NOT NULL DEFAULT 3,
    hot_activo      BOOLEAN NOT NULL DEFAULT TRUE,
    hot_fecha_crea  TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_hoteles_comunas
        FOREIGN KEY (com_codigo)
        REFERENCES comunas (com_codigo),

    CONSTRAINT chk_hoteles_estrellas
        CHECK (hot_estrellas BETWEEN 1 AND 5)
);

CREATE TABLE IF NOT EXISTS tipos_habitacion (
    tha_codigo       SERIAL PRIMARY KEY,
    tha_nombre       VARCHAR(100) NOT NULL,
    tha_descripcion  VARCHAR(250),
    tha_capacidad    INTEGER NOT NULL,
    tha_activo       BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT uq_tipos_habitacion_nombre UNIQUE (tha_nombre),

    CONSTRAINT chk_tipos_habitacion_capacidad
        CHECK (tha_capacidad > 0)
);

CREATE TABLE IF NOT EXISTS habitaciones (
    hab_codigo       SERIAL PRIMARY KEY,
    hot_codigo       INTEGER NOT NULL,
    tha_codigo       INTEGER NOT NULL,
    hab_numero       VARCHAR(20) NOT NULL,
    hab_piso         INTEGER NOT NULL DEFAULT 1,
    hab_precio_noche NUMERIC(12,2) NOT NULL,
    hab_activa       BOOLEAN NOT NULL DEFAULT TRUE,

    CONSTRAINT fk_habitaciones_hoteles
        FOREIGN KEY (hot_codigo)
        REFERENCES hoteles (hot_codigo),

    CONSTRAINT fk_habitaciones_tipos_habitacion
        FOREIGN KEY (tha_codigo)
        REFERENCES tipos_habitacion (tha_codigo),

    CONSTRAINT uq_habitaciones_hotel_numero UNIQUE (hot_codigo, hab_numero),

    CONSTRAINT chk_habitaciones_precio_noche
        CHECK (hab_precio_noche >= 0)
);

-- ============================================================
-- CLIENTES
-- ============================================================

CREATE TABLE IF NOT EXISTS clientes (
    cli_codigo       SERIAL PRIMARY KEY,
    cli_rut          VARCHAR(20),
    cli_nombre       VARCHAR(120) NOT NULL,
    cli_apellido     VARCHAR(120) NOT NULL,
    cli_email        VARCHAR(150),
    cli_telefono     VARCHAR(30),
    cli_direccion    VARCHAR(200),
    cli_activo       BOOLEAN NOT NULL DEFAULT TRUE,
    cli_fecha_crea   TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT uq_clientes_rut UNIQUE (cli_rut)
);

-- ============================================================
-- RESERVAS
-- ============================================================

CREATE TABLE IF NOT EXISTS reservas (
    res_codigo          SERIAL PRIMARY KEY,
    cli_codigo          INTEGER NOT NULL,
    hot_codigo          INTEGER NOT NULL,
    ere_codigo          INTEGER NOT NULL,
    res_fecha_desde     DATE NOT NULL,
    res_fecha_hasta     DATE NOT NULL,
    res_cantidad_adulto INTEGER NOT NULL DEFAULT 1,
    res_cantidad_nino   INTEGER NOT NULL DEFAULT 0,
    res_total           NUMERIC(12,2) NOT NULL DEFAULT 0,
    res_observacion     VARCHAR(300),
    res_fecha_crea      TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_reservas_clientes
        FOREIGN KEY (cli_codigo)
        REFERENCES clientes (cli_codigo),

    CONSTRAINT fk_reservas_hoteles
        FOREIGN KEY (hot_codigo)
        REFERENCES hoteles (hot_codigo),

    CONSTRAINT fk_reservas_estados_reserva
        FOREIGN KEY (ere_codigo)
        REFERENCES estados_reserva (ere_codigo),

    CONSTRAINT chk_reservas_fechas
        CHECK (res_fecha_hasta > res_fecha_desde),

    CONSTRAINT chk_reservas_adultos
        CHECK (res_cantidad_adulto > 0),

    CONSTRAINT chk_reservas_ninos
        CHECK (res_cantidad_nino >= 0),

    CONSTRAINT chk_reservas_total
        CHECK (res_total >= 0)
);

CREATE TABLE IF NOT EXISTS reservas_detalles (
    rde_codigo       SERIAL PRIMARY KEY,
    res_codigo       INTEGER NOT NULL,
    hab_codigo       INTEGER NOT NULL,
    rde_precio_noche NUMERIC(12,2) NOT NULL,
    rde_cantidad_noches INTEGER NOT NULL,
    rde_subtotal     NUMERIC(12,2) NOT NULL,

    CONSTRAINT fk_reservas_detalles_reservas
        FOREIGN KEY (res_codigo)
        REFERENCES reservas (res_codigo),

    CONSTRAINT fk_reservas_detalles_habitaciones
        FOREIGN KEY (hab_codigo)
        REFERENCES habitaciones (hab_codigo),

    CONSTRAINT uq_reservas_detalles_reserva_habitacion
        UNIQUE (res_codigo, hab_codigo),

    CONSTRAINT chk_reservas_detalles_precio
        CHECK (rde_precio_noche >= 0),

    CONSTRAINT chk_reservas_detalles_noches
        CHECK (rde_cantidad_noches > 0),

    CONSTRAINT chk_reservas_detalles_subtotal
        CHECK (rde_subtotal >= 0)
);

-- ============================================================
-- PAGOS
-- ============================================================

CREATE TABLE IF NOT EXISTS pagos (
    pag_codigo       SERIAL PRIMARY KEY,
    res_codigo       INTEGER NOT NULL,
    epa_codigo       INTEGER NOT NULL,
    pag_monto        NUMERIC(12,2) NOT NULL,
    pag_metodo       VARCHAR(80) NOT NULL,
    pag_referencia   VARCHAR(120),
    pag_fecha        TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    pag_observacion  VARCHAR(250),

    CONSTRAINT fk_pagos_reservas
        FOREIGN KEY (res_codigo)
        REFERENCES reservas (res_codigo),

    CONSTRAINT fk_pagos_estados_pago
        FOREIGN KEY (epa_codigo)
        REFERENCES estados_pago (epa_codigo),

    CONSTRAINT chk_pagos_monto
        CHECK (pag_monto > 0)
);

-- ============================================================
-- INDICES RECOMENDADOS
-- ============================================================

CREATE INDEX IF NOT EXISTS idx_hoteles_com_codigo
    ON hoteles (com_codigo);

CREATE INDEX IF NOT EXISTS idx_habitaciones_hot_codigo
    ON habitaciones (hot_codigo);

CREATE INDEX IF NOT EXISTS idx_habitaciones_tha_codigo
    ON habitaciones (tha_codigo);

CREATE INDEX IF NOT EXISTS idx_reservas_cli_codigo
    ON reservas (cli_codigo);

CREATE INDEX IF NOT EXISTS idx_reservas_hot_codigo
    ON reservas (hot_codigo);

CREATE INDEX IF NOT EXISTS idx_reservas_fechas
    ON reservas (res_fecha_desde, res_fecha_hasta);

CREATE INDEX IF NOT EXISTS idx_reservas_detalles_res_codigo
    ON reservas_detalles (res_codigo);

CREATE INDEX IF NOT EXISTS idx_pagos_res_codigo
    ON pagos (res_codigo);

-- ============================================================
-- FIN DEL SCRIPT
-- ============================================================
