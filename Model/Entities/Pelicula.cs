namespace SalaCine.Api.Model.Entities
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion {get; set;}
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}