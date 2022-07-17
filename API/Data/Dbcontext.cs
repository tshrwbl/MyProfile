using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }

        private readonly IConfiguration configuration;

        public DataContext(DbContextOptions options , IConfiguration configuration) : this(options)
        {
            this.configuration = configuration;
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }   

    }
}
