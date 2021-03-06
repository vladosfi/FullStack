using Microsoft.EntityFrameworkCore;
using WorldCities.Data.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace WorldCities.Data
{
    public class ApplicationDbContext
: ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
        {
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    // Map Entity names to DB Table names
        //    modelBuilder.Entity<City>().ToTable("Cities");
        //    modelBuilder.Entity<Country>().ToTable("Countries");
        //}
    }
}