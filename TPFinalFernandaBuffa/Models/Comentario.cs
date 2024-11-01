namespace TPFinalFernandaBuffa.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public string? Texto { get; set; }
        public DateTime Fecha { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int DestinoId { get; set; }
        public Destino? Destino { get; set; }
    }
}
