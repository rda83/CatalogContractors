using CatalogContractors.Models;
using Microsoft.EntityFrameworkCore;


namespace CatalogContractors.Database
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options): base(options){}

        public DbSet<TypeContact> TypeContact { get; set; }
        public DbSet<Contractor> Contractor { get; set; }
        public DbSet<Contact> Contact { get; set; }
    }
}
