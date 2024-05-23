using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyVechile.Models
{
    public class CompanyDBContext : IdentityDbContext<applicationUser>
    {
        public DbSet<Branches> Branches { get; set; }
        public DbSet<EmployeePhone> EmployeePhones { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<EmployeesVehicle> EmployeesVehicles { get; set; }

        public CompanyDBContext(DbContextOptions<CompanyDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Arabic_CI_AS");

            modelBuilder.Entity<EmployeePhone>().HasKey(e => new { e.Employee_ID, e.Employee_PhoneNumber });
            modelBuilder.Entity<EmployeesVehicle>().HasKey(e => new { e.VehiclePlateNumber, e.EmployeeId });

            // Configure Vehicle entity
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_PlateNumber)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.License_Registeration)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_BrandName)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Color)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Type)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Insurance)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.License_SerialNumber)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.License_ExpirationDate)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_ChassisNum)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_LastRepair_Date)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_LastAccident_Date)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Vehicle_Owner)
                .HasColumnType("nvarchar(20)");

            // Configure Employees entity
            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_ID)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_Name)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_Birthday)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_Role)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_Nationality)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_Street_Name)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_BuildingNumber)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Employees>()
                .Property(e => e.Employee_City)
                .HasColumnType("nvarchar(20)");

            // Configure Branches entity
            modelBuilder.Entity<Branches>()
                .Property(e => e.Branch_Name)
                .HasColumnType("nvarchar(20)");

            modelBuilder.Entity<Branches>()
               .Property(e => e.Branch_Location)
               .HasColumnType("nvarchar(20)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
