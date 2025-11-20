# SalaCine API - Documentación Completa

## 📋 Descripción del Proyecto

API REST desarrollada en .NET 9.0 para gestionar películas y salas de cine. Implementa un CRUD completo con funcionalidades de búsqueda avanzada y validación de disponibilidad de salas.

## 🏗️ Estructura del Proyecto

```
SalaCine.Api/
├── Controllers/          # Endpoints de la API
│   └── PeliculasController.cs
├── Model/
│   ├── DTOs/            # Data Transfer Objects
│   │   ├── CrearPeliculaDto.cs
│   │   ├── ActualizarPeliculaDto.cs
│   │   └── PeliculaDto.cs
│   └── Entities/        # Modelos de base de datos
│       ├── Pelicula.cs
│       ├── Sala.cs
│       └── PeliculaSala.cs
├── Repository/          # Acceso a datos
│   ├── IPeliculaRepository.cs
│   └── PeliculaRepository.cs
├── Services/            # Lógica de negocio
│   ├── Interfaces/
│   │   └── IPeliculaService.cs
│   └── Implementations/
│       └── PeliculaService.cs
├── Data/                # Context y base de datos
│   ├── ApplicationDbContext.cs
│   └── StoredProcedures.sql
├── Program.cs           # Configuración de la aplicación
└── SalaCine-Postman-Collection.json
```

## 🔧 Tecnologías Utilizadas

- **.NET 9.0** - Framework principal
- **Entity Framework Core 9.0.1** - ORM
- **PostgreSQL** - Base de datos
- **Npgsql** - Driver PostgreSQL
- **Swagger/OpenAPI** - Documentación de API
- **C# 13** - Lenguaje de programación

## 📦 Instalación y Configuración

### Requisitos Previos

- .NET 9.0 SDK
- PostgreSQL 12 o superior
- Visual Studio Code o Visual Studio 2022

### Pasos de Instalación

1. **Clonar el repositorio**

```bash
git clone https://github.com/sebascodeDev/sala-cine-backend.git
cd SalaCine.Api
```

2. **Restaurar dependencias**

```bash
dotnet restore
```

3. **Configurar la cadena de conexión**

Editar `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=SalaCineDb;Username=postgres;Password=tu_password"
  }
}
```

4. **Otorgar permisos en PostgreSQL** (IMPORTANTE)

Si reciben error `permission denied for schema public`, ejecutar primero:

```bash
# Conectar como superusuario
psql -U postgres -d SalaCineDb

# Ejecutar en la consola psql:
GRANT ALL PRIVILEGES ON DATABASE "SalaCineDb" TO postgres;
GRANT USAGE ON SCHEMA public TO postgres;
GRANT CREATE ON SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO postgres;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
```

O utilizar el script proporcionado:
```bash
psql -U postgres -d SalaCineDb -f Data/GrantPermissions.sql
```

5. **Crear la base de datos y ejecutar migraciones**

```bash
dotnet ef database update
```

6. **Ejecutar datos iniciales (opcional)**

```bash
psql -U postgres -d SalaCineDb -f Data/InitializeDatabase.sql
```

7. **Ejecutar la aplicación**

```bash
dotnet run
```

La API estará disponible en: `http://localhost:5233`

## 📚 Endpoints de la API

### 1. CRUD de Películas

#### Obtener todas las películas

```
GET /api/peliculas
```

**Respuesta exitosa (200):**

```json
[
  {
    "id": 1,
    "titulo": "Avatar",
    "descripcion": "Una película de ciencia ficción",
    "duracionMinutos": 162,
    "fechaEstreno": "2022-12-16T00:00:00",
    "activo": true
  }
]
```

#### Obtener película por ID

```
GET /api/peliculas/{id}
```

**Parámetros:**

- `id` (int, requerido): ID de la película

#### Crear película

```
POST /api/peliculas
Content-Type: application/json

{
  "titulo": "Avatar",
  "descripcion": "Una película de ciencia ficción",
  "duracionMinutos": 162,
  "fechaEstreno": "2022-12-16T00:00:00"
}
```

**Respuesta exitosa (201):** Devuelve el objeto creado con ID

#### Actualizar película

```
PUT /api/peliculas
Content-Type: application/json

{
  "id": 1,
  "titulo": "Avatar 2",
  "descripcion": "La secuela de Avatar",
  "duracionMinutos": 192,
  "fechaEstreno": "2022-12-16T00:00:00"
}
```

#### Eliminar película (Eliminación lógica)

```
DELETE /api/peliculas/{id}
```

**Nota:** La película se marca como inactiva, no se elimina físicamente

### 2. Procesos de Negocio

#### Buscar película por nombre

```
GET /api/peliculas/buscar/nombre?nombre=Avatar
```

**Parámetros:**

- `nombre` (string, requerido): Nombre o parte del nombre de la película

**Respuesta:**

```json
[
  {
    "id": 1,
    "titulo": "Avatar",
    "descripcion": "Una película de ciencia ficción",
    "duracionMinutos": 162,
    "fechaEstreno": "2022-12-16T00:00:00",
    "activo": true
  }
]
```

#### Obtener películas por fecha de publicación

```
GET /api/peliculas/buscar/fecha-publicacion?fecha=2022-12-16
```

**Parámetros:**

- `fecha` (date, requerido): Fecha de publicación (formato: YYYY-MM-DD)

**Validaciones:**

- La fecha debe ser válida
- Se buscan películas exactas a esa fecha

#### Obtener estado de sala

```
GET /api/peliculas/sala/estado?nombreSala=Sala 1
```

**Parámetros:**

- `nombreSala` (string, requerido): Nombre de la sala de cine

**Respuestas posibles:**

- `"Sala disponible"` - Si tiene menos de 3 películas
- `"Sala con [n] películas asignadas"` - Si tiene entre 3 y 5 películas
- `"Sala no disponible"` - Si tiene más de 5 películas
- `"Sala no encontrada"` - Si la sala no existe

**Respuesta del endpoint:**

```json
{
  "mensaje": "Sala disponible"
}
```

## 🗄️ Modelo de Base de Datos

### Tabla: Peliculas

```sql
CREATE TABLE "Peliculas" (
    "Id" SERIAL PRIMARY KEY,
    "Titulo" VARCHAR(100) NOT NULL,
    "Descripcion" VARCHAR(500),
    "DuracionMinutos" INT NOT NULL,
    "FechaEstreno" TIMESTAMP NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);
```

### Tabla: Salas

```sql
CREATE TABLE "Salas" (
    "Id" SERIAL PRIMARY KEY,
    "Nombre" VARCHAR(100) NOT NULL,
    "Capacidad" INT NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);
```

### Tabla: PeliculasSalas (Relación Many-to-Many)

```sql
CREATE TABLE "PeliculasSalas" (
    "Id" SERIAL PRIMARY KEY,
    "PeliculaId" INT NOT NULL REFERENCES "Peliculas"("Id"),
    "SalaId" INT NOT NULL REFERENCES "Salas"("Id"),
    "FechaFuncion" TIMESTAMP NOT NULL,
    "Activo" BOOLEAN NOT NULL DEFAULT true
);
```

## 🔐 Características de Seguridad

- ✅ Eliminación lógica de datos (no se eliminan físicamente)
- ✅ Validación de entrada en DTOs
- ✅ Manejo de excepciones
- ✅ Logging de errores
- ✅ HTTPS en producción

## 📖 Swagger / OpenAPI

La documentación interactiva de la API está disponible en:

```
http://localhost:5233/swagger
```

## 📮 Colección Postman

La colección `SalaCine-Postman-Collection.json` incluye:

- ✅ Todos los endpoints CRUD
- ✅ Búsquedas por nombre y fecha
- ✅ Validación de estado de sala
- ✅ Ejemplos de solicitud/respuesta

**Para importar:**

1. Abrir Postman
2. Click en "Import"
3. Seleccionar `SalaCine-Postman-Collection.json`

## 🧪 Ejemplos de Uso

### Crear una película

```bash
curl -X POST http://localhost:5233/api/peliculas \
  -H "Content-Type: application/json" \
  -d '{
    "titulo": "Avatar",
    "descripcion": "Película de ciencia ficción",
    "duracionMinutos": 162,
    "fechaEstreno": "2022-12-16"
  }'
```

### Buscar película por nombre

```bash
curl http://localhost:5233/api/peliculas/buscar/nombre?nombre=Avatar
```

### Obtener películas por fecha

```bash
curl http://localhost:5233/api/peliculas/buscar/fecha-publicacion?fecha=2022-12-16
```

### Verificar disponibilidad de sala

```bash
curl http://localhost:5233/api/peliculas/sala/estado?nombreSala=Sala%201
```

## 🚀 Deployment

### Publicar para producción

```bash
dotnet publish -c Release -o ./publish
```

### Variables de entorno necesarias

```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=<cadena_conexion>
```

## 📝 Notas Importantes

1. **Eliminaciones lógicas:** Todas las eliminaciones son lógicas. Los datos se marcan como inactivos pero permanecen en la base de datos.

2. **Validación de fechas:** Las fechas se validan en el servicio antes de procesarlas.

3. **Entity Framework:** Se utiliza EF Core con Npgsql para PostgreSQL.

4. **Stored Procedure:** Implementado `GetEstadoSala` que verifica el estado de disponibilidad de una sala.

## 📞 Soporte

Para reportar problemas o sugerencias, crear un issue en el repositorio.

## 📄 Licencia

Proyecto académico - Gestión de Películas y Salas de Cine

---

**Última actualización:** 20 de noviembre de 2025
**Versión:** 1.0.0
**Autor:** Sebastian Valarezo
