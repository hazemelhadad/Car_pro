using System.Collections.Generic;
using System.Linq;
using CompanyVechile.DTO; 
using CompanyVechile.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CompanyVechile.Repositories
{   
    public class AdminRepo : IAdminRepo
    {
        private readonly CompanyDBContext db;
        public AdminRepo(CompanyDBContext dbContext)
        {
            db = dbContext;
        }
        //-------------------------------------------------------
        public List<EmployeeDTO> GetAll(int branchId)
        {
            return db.Employees
                     .Include(e => e.EmployeePhones)
                     .Where(e => e.Branch_ID == branchId)
                     .Select(e => new EmployeeDTO
                     {
                         Employee_ID = e.Employee_ID,
                         Employee_Name = e.Employee_Name,
                         Employee_Birthday = e.Employee_Birthday,
                         Employee_Role = e.Employee_Role,
                         Employee_Nationality = e.Employee_Nationality,
                         Employee_Street_Name = e.Employee_Street_Name,
                         Employee_BuildingNumber = e.Employee_BuildingNumber,
                         Employee_City = e.Employee_City,
                         Branch_ID = e.Branch_ID,
                         EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                     })
                     .ToList();
        }
        //-------------------------------------------------------
        public EmployeeDTO GetEmpByID(int id, int branchId)
        {
            var employee = db.Employees
                             .Where(e => e.Employee_ID == id && e.Branch_ID == branchId)
                             .Include(e => e.EmployeePhones)
                             .Select(e => new EmployeeDTO
                             {
                                 Employee_ID = e.Employee_ID,
                                 Employee_Name = e.Employee_Name,
                                 Employee_Birthday = e.Employee_Birthday,
                                 Employee_Role = e.Employee_Role,
                                 Employee_Nationality = e.Employee_Nationality,
                                 Employee_Street_Name = e.Employee_Street_Name,
                                 Employee_BuildingNumber = e.Employee_BuildingNumber,
                                 Employee_City = e.Employee_City,
                                 Branch_ID = e.Branch_ID,
                                 EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                             })
                             .FirstOrDefault();

            return employee; 
        }

        //-------------------------------------------------------
        public List<EmployeeDTO> GetEmpByName(string name, int branchId)
        {
            return db.Employees
                     .Where(e => e.Employee_Name.Contains(name) && e.Branch_ID == branchId )
                     .Include(e => e.EmployeePhones)
                     .Select(e => new EmployeeDTO
                     {
                         Employee_ID = e.Employee_ID,
                         Employee_Name = e.Employee_Name,
                         Employee_Birthday = e.Employee_Birthday,
                         Employee_Role = e.Employee_Role,
                         Employee_Nationality = e.Employee_Nationality,
                         Employee_Street_Name = e.Employee_Street_Name,
                         Employee_BuildingNumber = e.Employee_BuildingNumber,
                         Employee_City = e.Employee_City,
                         Branch_ID = e.Branch_ID,
                         EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                     })
                     .ToList();
        }
        //-------------------------------------------------------
        public void AddEmp(EmployeeDTO empDto)
        {
            var emp = new Employees
            {
                Employee_ID = empDto.Employee_ID,
                Employee_Name = empDto.Employee_Name,
                Employee_Birthday = empDto.Employee_Birthday,
                Employee_Role = empDto.Employee_Role,
                Employee_Nationality = empDto.Employee_Nationality,
                Employee_Street_Name = empDto.Employee_Street_Name,
                Employee_BuildingNumber = empDto.Employee_BuildingNumber,
                Employee_City = empDto.Employee_City,
                Branch_ID = empDto.Branch_ID,
                EmployeePhones = empDto.EmployeePhones.Select(p => new EmployeePhone
                {
                    Employee_PhoneNumber = p
                }).ToList()
            };

            db.Employees.Add(emp);
            db.SaveChanges();
        }
        //-------------------------------------------------------
        public void EditEmp(EmployeeDTO empDto, int id)
        {
            var existingEmployee = db.Employees.FirstOrDefault(e => e.Employee_ID == id);
            if (existingEmployee == null) { return ; } 

            existingEmployee.Employee_Name = empDto.Employee_Name;
            existingEmployee.Employee_Birthday = empDto.Employee_Birthday;
            existingEmployee.Employee_Role = empDto.Employee_Role;
            existingEmployee.Employee_Nationality = empDto.Employee_Nationality;
            existingEmployee.Employee_Street_Name = empDto.Employee_Street_Name;
            existingEmployee.Employee_BuildingNumber = empDto.Employee_BuildingNumber;
            existingEmployee.Employee_City = empDto.Employee_City;

            //No edit for Employee Branch_ID , only SuperAdmin allowed to do this
            db.SaveChanges();
        }

        //-------------------------------------------------------
        public void DeleteEmp (int id)
        {
            var model = db.Employees.FirstOrDefault(p => p.Employee_ID == id);
            if (model == null) { return ; }

            db.Employees.Remove(model);
            db.SaveChanges();
        }
        //-------------------------------------------------------
        public List<VehicleDTO> GetAllVehicles(int branchId)
        {
            return db.Vehicle.Where(e => e.Branch_ID == branchId).Select(v => new VehicleDTO
            {
                Vehicle_PlateNumber = v.Vehicle_PlateNumber,
                License_SerialNumber = v.License_SerialNumber,
                License_Registeration = v.License_Registeration,
                License_ExpirationDate = v.License_ExpirationDate,
                Vehicle_ChassisNum = v.Vehicle_ChassisNum,
                Vehicle_ManufactureYear = v.Vehicle_ManufactureYear,
                Vehicle_BrandName = v.Vehicle_BrandName,
                Vehicle_Color = v.Vehicle_Color,
                Vehicle_Type = v.Vehicle_Type,
                Vehicle_Insurance = v.Vehicle_Insurance,
                Branch_ID=v.Branch_ID
            }).ToList();

        }
        //-------------------------------------------------------
        public List<VehicleDTO> GetVehicleByPlateNumber(string PltNum, int branchId)
        {
            return db.Vehicle.Where(v => v.Vehicle_PlateNumber.Contains(PltNum) && v.Branch_ID == branchId).Select(v => new VehicleDTO
            {
                Vehicle_PlateNumber = v.Vehicle_PlateNumber,
                License_SerialNumber = v.License_SerialNumber,
                License_Registeration = v.License_Registeration,
                License_ExpirationDate = v.License_ExpirationDate,
                Vehicle_ChassisNum = v.Vehicle_ChassisNum,
                Vehicle_ManufactureYear = v.Vehicle_ManufactureYear,
                Vehicle_BrandName = v.Vehicle_BrandName,
                Vehicle_Color = v.Vehicle_Color,
                Vehicle_Type = v.Vehicle_Type,
                Vehicle_Insurance = v.Vehicle_Insurance,
                Branch_ID = v.Branch_ID
            }).ToList();
        }
        //-------------------------------------------------------
        public List<VehicleDTO> GetVehicleByType(string type, int branchId)
        {
            return db.Vehicle.Where(v => v.Vehicle_Type == type && v.Branch_ID == branchId).Select(v => new VehicleDTO
            {
                Vehicle_PlateNumber = v.Vehicle_PlateNumber,
                License_SerialNumber = v.License_SerialNumber,
                License_Registeration = v.License_Registeration,
                License_ExpirationDate = v.License_ExpirationDate,
                Vehicle_ChassisNum = v.Vehicle_ChassisNum,
                Vehicle_ManufactureYear = v.Vehicle_ManufactureYear,
                Vehicle_BrandName = v.Vehicle_BrandName,
                Vehicle_Color = v.Vehicle_Color,
                Vehicle_Type = v.Vehicle_Type,
                Vehicle_Insurance = v.Vehicle_Insurance,
                Branch_ID = v.Branch_ID
            }).ToList();
        }
        //-------------------------------------------------------
    }
}
