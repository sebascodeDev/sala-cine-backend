using System.ComponentModel.DataAnnotations;

namespace SalaCine.Api.Model.DTOs
{
    public class CrearPeliculaDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La duración en minutos es requerida")]
        [Range(1, 500, ErrorMessage = "La duración debe estar entre 1 y 500 minutos")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "La fecha de estreno es requerida")]
        public DateTime FechaEstreno { get; set; }
    }
}
