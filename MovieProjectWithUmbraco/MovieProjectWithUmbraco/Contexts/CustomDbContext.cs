using MovieProjectWithUmbraco.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MovieProjectWithUmbraco.Contexts
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext() : base("umbracoDbDSN") { }

        public DbSet<FilmRating> FilmRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}