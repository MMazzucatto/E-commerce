using Microsoft.EntityFrameworkCore;

namespace ProjetoEcommerce.Models // Substitua pelo namespace do seu projeto
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
      
            Produto = Set<Produto>();
            Usuarios = Set<Usuario>();
          
        }

        // Adicione os DbSet para suas tabelas aqui
  
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
