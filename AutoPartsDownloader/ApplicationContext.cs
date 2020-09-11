using AutoPartsDownloader.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoPartsDownloader
{
    public class ApplicationContext:DbContext
    {
        public DbSet<AutoParts> AutoParts { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=autopartsdb;Username=postgres;Password=postgres");
        }
    }
}
