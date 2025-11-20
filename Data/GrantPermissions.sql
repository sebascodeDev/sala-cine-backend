-- Script para otorgar permisos en PostgreSQL
-- Ejecutar ANTES de las migraciones de Entity Framework
-- 
-- Instrucciones:
-- 1. Conectar como superusuario (postgres):
--    psql -U postgres -d SalaCineDb
--
-- 2. Ejecutar los siguientes comandos:

-- Asegurarse que existe el usuario (cambiar 'postgres' por tu usuario real)
-- CREATE USER app_user WITH PASSWORD 'tu_contraseña';

-- Otorgar permisos en la base de datos
GRANT ALL PRIVILEGES ON DATABASE "SalaCineDb" TO postgres;

-- Otorgar permisos en el schema public
GRANT USAGE ON SCHEMA public TO postgres;
GRANT CREATE ON SCHEMA public TO postgres;

-- Otorgar permisos en todas las tablas (actual y futuras)
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO postgres;

-- Hacer que los permisos se apliquen automáticamente a nuevas tablas
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON FUNCTIONS TO postgres;

-- Verificar que los permisos están correctos
-- \dp+ (en psql para ver privilegios de tablas)
-- \dn+ (en psql para ver privilegios de schemas)
