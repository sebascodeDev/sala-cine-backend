using SalaCine.Api.Model.DTOs;
using SalaCine.Api.Model.Entities;

namespace SalaCine.Api.Repository
{
    public interface IPeliculaRepository
    {
        // CRUD Operations
        Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync();
        Task<PeliculaDto?> ObtenerPorIdAsync(int id);
        Task<PeliculaDto> CrearAsync(CrearPeliculaDto dto);
        Task<bool> ActualizarAsync(ActualizarPeliculaDto dto);
        Task<bool> EliminarAsync(int id); // Eliminación lógica

        // Procesos de negocio
        Task<IEnumerable<PeliculaDto>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<PeliculaDto>> ObtenerPorFechaPublicacionAsync(DateTime fecha);
        Task<string> ObtenerEstadoSalaPorNombreAsync(string nombreSala);
    }
}
