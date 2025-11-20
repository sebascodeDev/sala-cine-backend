-- Script de Inicialización de Base de Datos SalaCine
-- Ejecutar como usuario con permisos (ej: postgres)

-- Otorgar permisos al usuario de la aplicación
-- GRANT USAGE ON SCHEMA public TO tu_usuario;
-- GRANT CREATE ON SCHEMA public TO tu_usuario;

-- Tabla Peliculas
CREATE TABLE IF NOT EXISTS "Peliculas" (
    "Id" SERIAL PRIMARY KEY,
    "Titulo" VARCHAR(100) NOT NULL,
    "Descripcion" VARCHAR(500),
    "DuracionMinutos" INT NOT NULL,
    "FechaEstreno" TIMESTAMP NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);

-- Tabla Salas
CREATE TABLE IF NOT EXISTS "Salas" (
    "Id" SERIAL PRIMARY KEY,
    "Nombre" VARCHAR(100) NOT NULL,
    "Capacidad" INT NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);

-- Tabla PeliculasSalas (Relación Many-to-Many)
CREATE TABLE IF NOT EXISTS "PeliculasSalas" (
    "Id" SERIAL PRIMARY KEY,
    "PeliculaId" INT NOT NULL REFERENCES "Peliculas"("Id") ON DELETE CASCADE,
    "SalaId" INT NOT NULL REFERENCES "Salas"("Id") ON DELETE CASCADE,
    "FechaFuncion" TIMESTAMP NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);

-- Datos de ejemplo para Películas
INSERT INTO "Peliculas" ("Titulo", "Descripcion", "DuracionMinutos", "FechaEstreno", "Activo") VALUES
('Avatar', 'Película de ciencia ficción epica', 162, '2022-12-16', true),
('Avatar 2', 'La secuela de Avatar', 192, '2022-12-16', true),
('Inception', 'Thriller de ciencia ficción', 148, '2010-07-16', true),
('Interstellar', 'Película de exploración espacial', 169, '2014-11-07', true),
('Titanic', 'Película de romance y drama', 194, '1997-12-19', true);

-- Datos de ejemplo para Salas
INSERT INTO "Salas" ("Nombre", "Capacidad", "Activo") VALUES
('Sala 1', 100, true),
('Sala 2', 80, true),
('Sala 3', 120, true),
('Sala 4', 90, true);

-- Datos de ejemplo para PeliculasSalas
INSERT INTO "PeliculasSalas" ("PeliculaId", "SalaId", "FechaFuncion", "Activo") VALUES
(1, 1, '2024-11-20 14:00:00', true),
(2, 1, '2024-11-20 17:00:00', true),
(3, 2, '2024-11-20 15:00:00', true),
(4, 3, '2024-11-20 16:00:00', true),
(5, 3, '2024-11-20 19:00:00', true),
(1, 3, '2024-11-20 21:00:00', true);

-- Crear el Stored Procedure para obtener el estado de una sala
CREATE OR REPLACE FUNCTION GetEstadoSala(p_nombreSala VARCHAR)
RETURNS VARCHAR AS $$
DECLARE
    v_salaId INT;
    v_cantidadPeliculas INT;
    v_resultado VARCHAR;
BEGIN
    -- Obtener el ID de la sala
    SELECT id INTO v_salaId FROM "Salas" 
    WHERE "Nombre" = p_nombreSala AND "Activo" = true;

    -- Si la sala no existe, retornar mensaje
    IF v_salaId IS NULL THEN
        RETURN 'Sala no encontrada';
    END IF;

    -- Contar películas activas en la sala
    SELECT COUNT(*) INTO v_cantidadPeliculas 
    FROM "PeliculasSalas" 
    WHERE "SalaId" = v_salaId AND "Activo" = true;

    -- Determinar el estado según la cantidad de películas
    IF v_cantidadPeliculas < 3 THEN
        v_resultado := 'Sala disponible';
    ELSIF v_cantidadPeliculas >= 3 AND v_cantidadPeliculas <= 5 THEN
        v_resultado := 'Sala con ' || v_cantidadPeliculas || ' películas asignadas';
    ELSE
        v_resultado := 'Sala no disponible';
    END IF;

    RETURN v_resultado;
END;
$$ LANGUAGE plpgsql;

-- Consultas de prueba

-- 1. Obtener todas las películas
SELECT * FROM "Peliculas" WHERE "Activo" = true;

-- 2. Obtener todas las salas
SELECT * FROM "Salas" WHERE "Activo" = true;

-- 3. Obtener películas de una sala específica
SELECT p.*, ps."FechaFuncion" 
FROM "Peliculas" p
JOIN "PeliculasSalas" ps ON p."Id" = ps."PeliculaId"
WHERE ps."SalaId" = 1 AND ps."Activo" = true;

-- 4. Probar la función GetEstadoSala
SELECT GetEstadoSala('Sala 1');
SELECT GetEstadoSala('Sala 2');
SELECT GetEstadoSala('Sala 3');
SELECT GetEstadoSala('Sala 4');
