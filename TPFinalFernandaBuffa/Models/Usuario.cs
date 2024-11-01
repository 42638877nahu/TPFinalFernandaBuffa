namespace TPFinalFernandaBuffa.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Contraseña { get; set; }
        public string? Fotografia {  get; set; }

        public List<Comentario>? Comentarios { get; set; }

        public List<Itinerario>? Itinerarios { get; set; }
    }
    
}
