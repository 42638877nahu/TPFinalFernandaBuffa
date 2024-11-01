using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TPFinalFernandaBuffa.Models;

namespace TPFinalFernandaBuffa.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Destino> Destinos { get; set; }
        public DbSet<Excursion> Excursiones { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Itinerario> Itinerarios { get; set; }
    }
}
