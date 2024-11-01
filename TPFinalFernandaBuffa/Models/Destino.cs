namespace TPFinalFernandaBuffa.Models
{
    public class Destino
    {
         public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Ubicacion { get; set; }
        public string? Fotografia {  get; set; }

        public List<Excursion>? Excursiones { get; set; }

        public List<Comentario>? Comentarios { get; set; }
    }
}
