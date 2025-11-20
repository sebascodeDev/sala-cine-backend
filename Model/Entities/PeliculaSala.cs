namespace SalaCine.Api.Model.Entities
{
    public class PeliculaSala
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public int SalaId { get; set; }
        public DateTime FechaFuncion { get; set; }
        public bool Activo { get; set; }

        // Relaciones
        public Pelicula? Pelicula { get; set; }
        public Sala? Sala { get; set; }
    }
}
