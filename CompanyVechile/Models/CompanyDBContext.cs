using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CompanyVechile.Models
{
    public class CompanyDBContext:DbContext
    {
        public DbSet<Branches> Branches { get; set; }
        public DbSet<EmployeePhone> EmployeePhones { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = .; Initial Catalog = AramoonDatabase; Integrated Security = True; Trust Server Certificate = True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeePhone>().HasKey(e => new {e.Employee_ID, e.Employee_PhoneNumber }); //MultiValued Table Composite primary key
            base.OnModelCreating(modelBuilder);
        }
    }
}
