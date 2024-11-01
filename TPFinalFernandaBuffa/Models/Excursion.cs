using System.ComponentModel.DataAnnotations.Schema;

namespace TPFinalFernandaBuffa.Models
{
    public class Excursion
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public string? Fotografia { get; set; }

        public int DestinoId { get; set; }
        public Destino? Destino { get; set; }

    }
}
