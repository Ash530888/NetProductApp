using Microsoft.EntityFrameworkCore;

namespace NetProductApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){ }

        public DbSet<Product> Products {get; set;}
        public DbSet<Login> logins {get; set;}
    }
}