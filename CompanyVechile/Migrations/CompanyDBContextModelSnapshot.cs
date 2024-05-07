﻿// <auto-generated />
using System;
using CompanyVechile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CompanyVechile.Migrations
{
    [DbContext(typeof(CompanyDBContext))]
    partial class CompanyDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Arabic_CI_AS")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CompanyVechile.Models.Branches", b =>
                {
                    b.Property<int>("Branch_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Branch_ID"));

                    b.Property<string>("Branch_Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Branch_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Branch_ID");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("CompanyVechile.Models.EmployeePhone", b =>
                {
                    b.Property<string>("Employee_ID")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Employee_PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Employee_ID", "Employee_PhoneNumber");

                    b.ToTable("EmployeePhones");
                });

            modelBuilder.Entity("CompanyVechile.Models.Employees", b =>
                {
                    b.Property<string>("Employee_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Branch_ID")
                        .HasColumnType("int");

                    b.Property<string>("Employee_Birthday")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_BuildingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_Nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Employee_Street_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Employee_ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("CompanyVechile.Models.Vehicle", b =>
                {
                    b.Property<string>("Vehicle_PlateNumber")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Branch_ID")
                        .HasColumnType("int");

                    b.Property<string>("License_ExpirationDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("License_Registeration")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("License_SerialNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vehicle_BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Vehicle_ChassisNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vehicle_Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Vehicle_Insurance")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Vehicle_ManufactureYear")
                        .HasColumnType("int");

                    b.Property<string>("Vehicle_Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Vehicle_PlateNumber");

                    b.HasIndex("Branch_ID");

                    b.ToTable("Vehicle");
                });

            modelBuilder.Entity("EmployeesVehicle", b =>
                {
                    b.Property<string>("EmployeesEmployee_ID")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Vehicle_PlateNumber")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EmployeesEmployee_ID", "Vehicle_PlateNumber");

                    b.HasIndex("Vehicle_PlateNumber");

                    b.ToTable("EmployeesVehicle");
                });

            modelBuilder.Entity("CompanyVechile.Models.EmployeePhone", b =>
                {
                    b.HasOne("CompanyVechile.Models.Employees", "Employees")
                        .WithMany("EmployeePhones")
                        .HasForeignKey("Employee_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("CompanyVechile.Models.Vehicle", b =>
                {
                    b.HasOne("CompanyVechile.Models.Branches", "Branch")
                        .WithMany("Vehicles")
                        .HasForeignKey("Branch_ID");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("EmployeesVehicle", b =>
                {
                    b.HasOne("CompanyVechile.Models.Employees", null)
                        .WithMany()
                        .HasForeignKey("EmployeesEmployee_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CompanyVechile.Models.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("Vehicle_PlateNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompanyVechile.Models.Branches", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("CompanyVechile.Models.Employees", b =>
                {
                    b.Navigation("EmployeePhones");
                });
#pragma warning restore 612, 618
        }
    }
}
