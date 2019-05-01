using Microsoft.EntityFrameworkCore;

namespace DotnetCoreWebApiTemplate.Database
{
    public class ServerDbContext : DbContext
    {
        public ServerDbContext()
        {
        }

        public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
        {
        }

        // Models go here as DBsets
        // eg: public DbSet<TableClass> TableClasses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use configuration to set values here
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use here to configure individual model table names etc.
            // eg: modelBuilder.Entity<TableClass>().ToTable("TableClasses");
        }
    }
}