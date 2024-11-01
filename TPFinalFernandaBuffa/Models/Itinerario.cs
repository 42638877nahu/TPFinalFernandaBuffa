namespace TPFinalFernandaBuffa.Models
{
    public class Itinerario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string? Descripcion { get; set; }

        public List<Destino>? Destinos { get; set; }

        public List<Excursion>? Excursiones { get; set; }
    }
}
