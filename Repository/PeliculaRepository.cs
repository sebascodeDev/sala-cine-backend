using Microsoft.EntityFrameworkCore;
using SalaCine.Api.Data;
using SalaCine.Api.Model.DTOs;
using SalaCine.Api.Model.Entities;

namespace SalaCine.Api.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _context;

        public PeliculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // CRUD Operations
        public async Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync()
        {
            return await _context.Peliculas
                .Where(p => p.Activo)
                .Select(p => new PeliculaDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descripcion = p.Descripcion,
                    DuracionMinutos = p.DuracionMinutos,
                    FechaEstreno = p.FechaEstreno,
                    Activo = p.Activo
                })
                .ToListAsync();
        }

        public async Task<PeliculaDto?> ObtenerPorIdAsync(int id)
        {
            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (pelicula == null)
                return null;

            return new PeliculaDto
            {
                Id = pelicula.Id,
                Titulo = pelicula.Titulo,
                Descripcion = pelicula.Descripcion,
                DuracionMinutos = pelicula.DuracionMinutos,
                FechaEstreno = pelicula.FechaEstreno,
                Activo = pelicula.Activo
            };
        }

        public async Task<PeliculaDto> CrearAsync(CrearPeliculaDto dto)
        {
            var pelicula = new Pelicula
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                DuracionMinutos = dto.DuracionMinutos,
                FechaEstreno = dto.FechaEstreno,
                Activo = true
            };

            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            return new PeliculaDto
            {
                Id = pelicula.Id,
                Titulo = pelicula.Titulo,
                Descripcion = pelicula.Descripcion,
                DuracionMinutos = pelicula.DuracionMinutos,
                FechaEstreno = pelicula.FechaEstreno,
                Activo = pelicula.Activo
            };
        }

        public async Task<bool> ActualizarAsync(ActualizarPeliculaDto dto)
        {
            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(p => p.Id == dto.Id && p.Activo);

            if (pelicula == null)
                return false;

            pelicula.Titulo = dto.Titulo;
            pelicula.Descripcion = dto.Descripcion;
            pelicula.DuracionMinutos = dto.DuracionMinutos;
            pelicula.FechaEstreno = dto.FechaEstreno;

            _context.Peliculas.Update(pelicula);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (pelicula == null)
                return false;

            // Eliminación lógica
            pelicula.Activo = false;
            _context.Peliculas.Update(pelicula);
            await _context.SaveChangesAsync();

            return true;
        }

        // Procesos de negocio
        public async Task<IEnumerable<PeliculaDto>> BuscarPorNombreAsync(string nombre)
        {
            return await _context.Peliculas
                .Where(p => p.Activo && p.Titulo.Contains(nombre))
                .Select(p => new PeliculaDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descripcion = p.Descripcion,
                    DuracionMinutos = p.DuracionMinutos,
                    FechaEstreno = p.FechaEstreno,
                    Activo = p.Activo
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<PeliculaDto>> ObtenerPorFechaPublicacionAsync(DateTime fecha)
        {
            var fechaSinTiempo = fecha.Date;

            return await _context.Peliculas
                .Where(p => p.Activo && p.FechaEstreno.Date == fechaSinTiempo)
                .Select(p => new PeliculaDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Descripcion = p.Descripcion,
                    DuracionMinutos = p.DuracionMinutos,
                    FechaEstreno = p.FechaEstreno,
                    Activo = p.Activo
                })
                .ToListAsync();
        }

        public async Task<string> ObtenerEstadoSalaPorNombreAsync(string nombreSala)
        {
            var sala = await _context.Salas
                .FirstOrDefaultAsync(s => s.Nombre == nombreSala && s.Activo);

            if (sala == null)
                return "Sala no encontrada";

            // Contar películas activas en la sala
            var cantidadPeliculas = await _context.PeliculasSalas
                .Where(ps => ps.SalaId == sala.Id && ps.Activo)
                .CountAsync();

            if (cantidadPeliculas < 3)
                return "Sala disponible";
            else if (cantidadPeliculas >= 3 && cantidadPeliculas <= 5)
                return $"Sala con {cantidadPeliculas} películas asignadas";
            else
                return "Sala no disponible";
        }
    }
}
