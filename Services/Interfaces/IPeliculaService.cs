using SalaCine.Api.Model.DTOs;

namespace SalaCine.Api.Services.Interfaces
{
    public interface IPeliculaService
    {
        // CRUD Operations
        Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync();
        Task<PeliculaDto?> ObtenerPorIdAsync(int id);
        Task<PeliculaDto> CrearAsync(CrearPeliculaDto dto);
        Task<bool> ActualizarAsync(ActualizarPeliculaDto dto);
        Task<bool> EliminarAsync(int id);

        // Procesos de negocio
        Task<IEnumerable<PeliculaDto>> BuscarPorNombreAsync(string nombre);
        Task<IEnumerable<PeliculaDto>> ObtenerPorFechaPublicacionAsync(DateTime fecha);
        Task<string> ObtenerEstadoSalaPorNombreAsync(string nombreSala);
    }
}
