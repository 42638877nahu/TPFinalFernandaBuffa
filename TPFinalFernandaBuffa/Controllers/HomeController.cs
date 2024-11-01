using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TPFinalFernandaBuffa.Data;
using TPFinalFernandaBuffa.Models;
using TPFinalFernandaBuffa.Models.ViewModels;
using TPFinalFernandaBuffa.Data;
using TPFinalFernandaBuffa.Models.ViewModels;
using TPFinalFernandaBuffa.Models;

namespace TPFinalFernandaBuffa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? search, int page = 1, int pageSize = 3)
        {
            // Obtiene los comentarios y los incluye con el usuario relacionado
            var query = _context.Comentarios.Include(c => c.Usuario).AsQueryable();

            // Filtrar por texto de comentario o nombre de usuario si se proporciona un criterio de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Texto.Contains(search) || c.Usuario.Nombre.Contains(search));
            }

            // Obtener el total de comentarios después del filtro para la paginación
            var totalComentarios = query.Count();

            // Aplicar paginación
            var comentarios = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var destinos = _context.Destinos.Take(3).ToList();
            var totalUsuarios = _context.Usuarios.Count();
            var totalDestinos = _context.Destinos.Count();
            var totalItinerarios = _context.Itinerarios.Count();

            // Obtener las próximas excursiones (cambiar la lógica según tu criterio)
            var proximasExcursiones = _context.Excursiones
                .OrderBy(e => e.Id) // Cambiar el orden según lo que consideres "próximas"
                .Take(5)
                .ToList();

            var viewModel = new HomeViewModel
            {
                Destinos = destinos,
                TotalUsuarios = totalUsuarios,
                TotalDestinos = totalDestinos,
                TotalItinerarios = totalItinerarios,
                Comentarios = comentarios,
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalComentarios = totalComentarios,
                Excursiones = proximasExcursiones // Agregar las excursiones al modelo
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
