using System.Collections.Generic;
using TPFinalFernandaBuffa.Models; 

namespace TPFinalFernandaBuffa.Models.ViewModels // Este debe ser el mismo que importas en HomeController
{
    public class HomeViewModel
    {
        public required List<Destino> Destinos { get; set; }
        public int TotalUsuarios { get; set; }
        public int TotalDestinos { get; set; }
        public int TotalItinerarios { get; set; }
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
        public string? Search { get; set; }//Busquedad
        public int Page { get; set; }//Paginacion
        public int PageSize { get; set; }
        public int TotalComentarios { get; set; }
        public List<Excursion> Excursiones { get; set; } = new List<Excursion>();
    }
}
