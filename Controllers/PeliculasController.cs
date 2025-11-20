using Microsoft.AspNetCore.Mvc;
using SalaCine.Api.Model.DTOs;
using SalaCine.Api.Services.Interfaces;

namespace SalaCine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaService _service;
        private readonly ILogger<PeliculasController> _logger;

        public PeliculasController(IPeliculaService service, ILogger<PeliculasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las películas activas
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PeliculaDto>>> ObtenerTodas()
        {
            try
            {
                var peliculas = await _service.ObtenerTodasAsync();
                return Ok(peliculas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las películas");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene una película por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PeliculaDto>> ObtenerPorId(int id)
        {
            try
            {
                var pelicula = await _service.ObtenerPorIdAsync(id);
                if (pelicula == null)
                    return NotFound("Película no encontrada");

                return Ok(pelicula);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener película por ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Crea una nueva película
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PeliculaDto>> Crear([FromBody] CrearPeliculaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var pelicula = await _service.CrearAsync(dto);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = pelicula.Id }, pelicula);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear película");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Actualiza una película existente
        /// </summary>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarPeliculaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.ActualizarAsync(dto);
                if (!resultado)
                    return NotFound("Película no encontrada");

                return Ok("Película actualizada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar película");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Elimina (lógicamente) una película
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var resultado = await _service.EliminarAsync(id);
                if (!resultado)
                    return NotFound("Película no encontrada");

                return Ok("Película eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar película");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Busca películas por nombre
        /// </summary>
        [HttpGet("buscar/nombre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PeliculaDto>>> BuscarPorNombre([FromQuery] string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return BadRequest("El nombre no puede estar vacío");

                var peliculas = await _service.BuscarPorNombreAsync(nombre);
                return Ok(peliculas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar película por nombre");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene películas por fecha de publicación
        /// </summary>
        [HttpGet("buscar/fecha-publicacion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PeliculaDto>>> ObtenerPorFechaPublicacion([FromQuery] DateTime fecha)
        {
            try
            {
                if (fecha == default)
                    return BadRequest("La fecha no es válida");

                var peliculas = await _service.ObtenerPorFechaPublicacionAsync(fecha);
                return Ok(peliculas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener películas por fecha");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Obtiene el estado de una sala de cine
        /// </summary>
        [HttpGet("sala/estado")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ObtenerEstadoSala([FromQuery] string nombreSala)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreSala))
                    return BadRequest("El nombre de la sala no puede estar vacío");

                var estado = await _service.ObtenerEstadoSalaPorNombreAsync(nombreSala);
                return Ok(new { mensaje = estado });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estado de sala");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor");
            }
        }
    }
}
