CREATE TABLE IF NOT EXISTS paises (
    pais_id SERIAL PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL UNIQUE,
    codigo_iso VARCHAR(3) NOT NULL UNIQUE,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS ciudades (
    ciudad_id SERIAL PRIMARY KEY,
    pais_id INTEGER NOT NULL REFERENCES paises(pais_id),
    nombre VARCHAR(100) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    UNIQUE (pais_id, nombre)
);

CREATE TABLE IF NOT EXISTS hoteles (
    hotel_id SERIAL PRIMARY KEY,
    ciudad_id INTEGER NOT NULL REFERENCES ciudades(ciudad_id),
    nombre VARCHAR(150) NOT NULL,
    direccion VARCHAR(250) NOT NULL,
    telefono VARCHAR(30),
    email VARCHAR(150),
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS roles (
    rol_id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS usuarios (
    usuario_id SERIAL PRIMARY KEY,
    rol_id INTEGER NOT NULL REFERENCES roles(rol_id),
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS tipos_habitacion (
    tipo_habitacion_id SERIAL PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL UNIQUE,
    descripcion VARCHAR(250),
    capacidad INTEGER NOT NULL CHECK (capacidad > 0),
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS estados_habitacion (
    estado_habitacion_id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS habitaciones (
    habitacion_id SERIAL PRIMARY KEY,
    hotel_id INTEGER NOT NULL REFERENCES hoteles(hotel_id),
    tipo_habitacion_id INTEGER NOT NULL REFERENCES tipos_habitacion(tipo_habitacion_id),
    estado_habitacion_id INTEGER NOT NULL REFERENCES estados_habitacion(estado_habitacion_id),
    numero VARCHAR(20) NOT NULL,
    piso INTEGER,
    precio_noche NUMERIC(12,2) NOT NULL CHECK (precio_noche >= 0),
    activa BOOLEAN NOT NULL DEFAULT TRUE,
    UNIQUE (hotel_id, numero)
);

CREATE TABLE IF NOT EXISTS clientes (
    cliente_id SERIAL PRIMARY KEY,
    rut VARCHAR(20),
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    telefono VARCHAR(30),
    direccion VARCHAR(250),
    ciudad_id INTEGER REFERENCES ciudades(ciudad_id),
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS estados_reserva (
    estado_reserva_id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS reservas (
    reserva_id SERIAL PRIMARY KEY,
    cliente_id INTEGER NOT NULL REFERENCES clientes(cliente_id),
    habitacion_id INTEGER NOT NULL REFERENCES habitaciones(habitacion_id),
    estado_reserva_id INTEGER NOT NULL REFERENCES estados_reserva(estado_reserva_id),
    usuario_creacion_id INTEGER REFERENCES usuarios(usuario_id),
    fecha_entrada DATE NOT NULL,
    fecha_salida DATE NOT NULL,
    cantidad_huespedes INTEGER NOT NULL CHECK (cantidad_huespedes > 0),
    total NUMERIC(12,2) NOT NULL DEFAULT 0 CHECK (total >= 0),
    observacion VARCHAR(500),
    fecha_creacion TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CHECK (fecha_salida > fecha_entrada)
);

CREATE TABLE IF NOT EXISTS huespedes (
    huesped_id SERIAL PRIMARY KEY,
    reserva_id INTEGER NOT NULL REFERENCES reservas(reserva_id),
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    documento VARCHAR(30),
    telefono VARCHAR(30),
    email VARCHAR(150)
);

CREATE TABLE IF NOT EXISTS metodos_pago (
    metodo_pago_id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS estados_pago (
    estado_pago_id SERIAL PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS pagos (
    pago_id SERIAL PRIMARY KEY,
    reserva_id INTEGER NOT NULL REFERENCES reservas(reserva_id),
    metodo_pago_id INTEGER NOT NULL REFERENCES metodos_pago(metodo_pago_id),
    estado_pago_id INTEGER NOT NULL REFERENCES estados_pago(estado_pago_id),
    monto NUMERIC(12,2) NOT NULL CHECK (monto > 0),
    fecha_pago TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    referencia VARCHAR(100),
    observacion VARCHAR(300)
);

CREATE TABLE IF NOT EXISTS reservas_historial (
    historial_id SERIAL PRIMARY KEY,
    reserva_id INTEGER NOT NULL REFERENCES reservas(reserva_id),
    estado_reserva_id INTEGER NOT NULL REFERENCES estados_reserva(estado_reserva_id),
    usuario_id INTEGER REFERENCES usuarios(usuario_id),
    observacion VARCHAR(500),
    fecha_cambio TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS auditoria (
    auditoria_id SERIAL PRIMARY KEY,
    usuario_id INTEGER REFERENCES usuarios(usuario_id),
    tabla VARCHAR(100) NOT NULL,
    accion VARCHAR(30) NOT NULL,
    registro_id INTEGER,
    detalle TEXT,
    fecha_evento TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);