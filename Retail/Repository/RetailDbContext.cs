using Microsoft.EntityFrameworkCore;
using Retail.Repository.Entity;

namespace Retail.Repository
{
    public sealed class RetailDbContext : DbContext
    {

        public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }


        public DbSet<ProductEntity> Products { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    //if (!optionsBuilder.IsConfigured)
        //    //{
        //    //    var builder = new ConfigurationBuilder();
        //    //    builder.AddJsonFile("appsettings.json", false, false);
        //    //    var config = builder.Build();
        //    //    optionsBuilder.UseSqlServer(config.GetConnectionString("DBConnectionString")); 
        //    //}
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RetailDbContext).Assembly);
            foreach (  var property in modelBuilder.Model
                    .GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?))
            )
            {
                property.SetColumnType("decimal(10, 2)");
            }
            //     modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
        }
         
    }
}
