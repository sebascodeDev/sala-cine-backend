namespace SalaCine.Api.Model.Entities
{
    public class Sala
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Capacidad { get; set; }
        public bool Activo { get; set; }

        // Relación
        public ICollection<PeliculaSala>? PeliculasSalas { get; set; }
    }
}
