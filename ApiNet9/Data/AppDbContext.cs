using ApiNet9.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ApiNet9.Data
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<UsuarioModel> Usuarios {get; set;}
        public DbSet<AuditoriaModel> Auditorias { get; set; }
    }
}
