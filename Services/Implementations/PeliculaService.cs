using SalaCine.Api.Model.DTOs;
using SalaCine.Api.Repository;
using SalaCine.Api.Services.Interfaces;

namespace SalaCine.Api.Services.Implementations
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IPeliculaRepository _repository;

        public PeliculaService(IPeliculaRepository repository)
        {
            _repository = repository;
        }

        // CRUD Operations
        public async Task<IEnumerable<PeliculaDto>> ObtenerTodasAsync()
        {
            return await _repository.ObtenerTodasAsync();
        }

        public async Task<PeliculaDto?> ObtenerPorIdAsync(int id)
        {
            return await _repository.ObtenerPorIdAsync(id);
        }

        public async Task<PeliculaDto> CrearAsync(CrearPeliculaDto dto)
        {
            return await _repository.CrearAsync(dto);
        }

        public async Task<bool> ActualizarAsync(ActualizarPeliculaDto dto)
        {
            return await _repository.ActualizarAsync(dto);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            return await _repository.EliminarAsync(id);
        }

        // Procesos de negocio
        public async Task<IEnumerable<PeliculaDto>> BuscarPorNombreAsync(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre no puede estar vacío");

            return await _repository.BuscarPorNombreAsync(nombre.Trim());
        }

        public async Task<IEnumerable<PeliculaDto>> ObtenerPorFechaPublicacionAsync(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("La fecha no es válida");

            return await _repository.ObtenerPorFechaPublicacionAsync(fecha);
        }

        public async Task<string> ObtenerEstadoSalaPorNombreAsync(string nombreSala)
        {
            if (string.IsNullOrWhiteSpace(nombreSala))
                throw new ArgumentException("El nombre de la sala no puede estar vacío");

            return await _repository.ObtenerEstadoSalaPorNombreAsync(nombreSala.Trim());
        }
    }
}
