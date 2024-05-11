using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CompanyVechile.Models
{
    public class CompanyDBContext : DbContext
    {
        public DbSet<Branches> Branches { get; set; }
        public DbSet<EmployeePhone> EmployeePhones { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<EmployeesVehicle> EmployeesVehicles { get; set; }

        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Arabic_CI_AS"); 
            modelBuilder.Entity<EmployeePhone>().HasKey(e => new { e.Employee_ID, e.Employee_PhoneNumber });
            modelBuilder.Entity<EmployeesVehicle>().HasKey(e => new { e.VehiclePlateNumber , e.EmployeeId });

            modelBuilder.Entity<EmployeePhone>()
            .HasOne(e => e.Employees) 
            .WithMany(e => e.EmployeePhones) 
            .HasForeignKey(e => e.Employee_ID);

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_PlateNumber)
                .HasColumnType("nvarchar(50)"); 

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_ID)
                .HasColumnType("nvarchar(50)"); 

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.License_Registeration)
                .HasColumnType("nvarchar(100)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_BrandName)
                .HasColumnType("nvarchar(100)"); 

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Color)
                .HasColumnType("nvarchar(50)"); 

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Type)
                .HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Vehicle>()
              .Property(v => v.Vehicle_Insurance)
              .HasColumnType("nvarchar(50)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
