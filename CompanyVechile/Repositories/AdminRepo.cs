using System.Collections.Generic;
using System.Linq;
using CompanyVechile.DTO; 
using CompanyVechile.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
        public List<AdminEmployeeDTO> GetAll(int branchId)
        {
            return db.Employees
                     .Include(e => e.EmployeePhones)
                     .Where(e => e.Branch_ID == branchId && e.Employee_Role == "Driver")
                     .Select(e => new AdminEmployeeDTO
                     {
                         Employee_ID = e.Employee_ID,
                         Employee_Name = e.Employee_Name,
                         Employee_Birthday = e.Employee_Birthday,
                         Employee_Role = e.Employee_Role,
                         Employee_Nationality = e.Employee_Nationality,
                         Employee_Street_Name = e.Employee_Street_Name,
                         Employee_BuildingNumber = e.Employee_BuildingNumber,
                         Employee_City = e.Employee_City,
                         Branch_ID = e.Branch_ID == null ? 0 : e.Branch_ID.Value,
                         EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                     })
                     .ToList();
        }
        //-------------------------------------------------------
        public List<AdminEmployeeDTO> GetEmpByID(string id, int branchId)
        {
            var employee = db.Employees
                             .Where(e => e.Employee_ID.Contains(id) && e.Branch_ID == branchId && e.Employee_Role == "Driver")
                             .Include(e => e.EmployeePhones)
                             .Select(e => new AdminEmployeeDTO
                             {
                                 Employee_ID = e.Employee_ID,
                                 Employee_Name = e.Employee_Name,
                                 Employee_Birthday = e.Employee_Birthday,
                                 Employee_Role = e.Employee_Role,
                                 Employee_Nationality = e.Employee_Nationality,
                                 Employee_Street_Name = e.Employee_Street_Name,
                                 Employee_BuildingNumber = e.Employee_BuildingNumber,
                                 Employee_City = e.Employee_City,
                                 Branch_ID = e.Branch_ID == null ? 0 : e.Branch_ID.Value,
                                 EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                             })
                             .ToList();

            return employee; 
        }
        //-------------------------------------------------------
        public Employees GetEmpByID(string id)  //Added by Hazem, needed for the Account Controller.
        {
            return db.Employees.FirstOrDefault(u => u.Employee_ID == id);
        }
        //-------------------------------------------------------
        public List<AdminEmployeeDTO> GetEmpByName(string name, int branchId)
        {
            return db.Employees
                     .Where(e => e.Employee_Name.Contains(name) && e.Branch_ID == branchId && e.Employee_Role == "Driver")
                     .Include(e => e.EmployeePhones)
                     .Select(e => new AdminEmployeeDTO
                     {
                         Employee_ID = e.Employee_ID,
                         Employee_Name = e.Employee_Name,
                         Employee_Birthday = e.Employee_Birthday,
                         Employee_Role = e.Employee_Role,
                         Employee_Nationality = e.Employee_Nationality,
                         Employee_Street_Name = e.Employee_Street_Name,
                         Employee_BuildingNumber = e.Employee_BuildingNumber,
                         Employee_City = e.Employee_City,
                         Branch_ID = e.Branch_ID ==null?0:e.Branch_ID.Value,
                         EmployeePhones = e.EmployeePhones.Select(p => p.Employee_PhoneNumber).ToList()
                     })
                     .ToList();
        }
        //-------------------------------------------------------
        public void AddEmp(AdminEmployeeDTO empDto)
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
        public void EditEmp(AdminEmployeeDTO empDto, string id)
        {
            var existingEmployee = db.Employees.FirstOrDefault(e => e.Employee_ID == id);
            if (existingEmployee == null) { return ; } 

            existingEmployee.Employee_Name = empDto.Employee_Name;
            existingEmployee.Employee_Birthday = empDto.Employee_Birthday;
            existingEmployee.Employee_Nationality = empDto.Employee_Nationality;
            existingEmployee.Employee_Street_Name = empDto.Employee_Street_Name;
            existingEmployee.Employee_BuildingNumber = empDto.Employee_BuildingNumber;
            existingEmployee.Employee_City = empDto.Employee_City;

            //No edit for Employee Branch_ID & Employee National ID , only SuperAdmin allowed to do this.
            db.SaveChanges();
        }

        //-------------------------------------------------------
        public void DeleteEmp (string id)
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
            return db.Vehicle.Where(v => v.Vehicle_Type.Contains(type) && v.Branch_ID == branchId).Select(v => new VehicleDTO
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
        public void AddVehicle(VehicleDTO vhc)
        {
            var vehicle = new Vehicle
            {
                Vehicle_PlateNumber = vhc.Vehicle_PlateNumber,
                License_SerialNumber = vhc.License_SerialNumber,
                License_Registeration = vhc.License_Registeration,
                License_ExpirationDate = vhc.License_ExpirationDate,
                Vehicle_ChassisNum = vhc.Vehicle_ChassisNum,
                Vehicle_ManufactureYear = vhc.Vehicle_ManufactureYear,
                Vehicle_BrandName = vhc.Vehicle_BrandName,
                Vehicle_Color = vhc.Vehicle_Color,
                Vehicle_Type = vhc.Vehicle_Type,
                Vehicle_Insurance = vhc.Vehicle_Insurance,
                Branch_ID = vhc.Branch_ID
            };

           db.Vehicle.Add(vehicle);
           db.SaveChanges();
        }
        //-------------------------------------------------------
        public void EditVhc(VehicleDTO vhc, string PltNum)
        {
            var oldVehicle = db.Vehicle.FirstOrDefault(v=>v.Vehicle_PlateNumber == PltNum);
            if (oldVehicle == null) { return; }

            //No Edit for Vehicle PlateNumber. (PK)
            //No Edit for Vehicle's Branch ID.
            oldVehicle.Vehicle_Color = vhc.Vehicle_Color;
            oldVehicle.Vehicle_ChassisNum = vhc.Vehicle_ChassisNum;
            oldVehicle.Vehicle_BrandName = vhc.Vehicle_BrandName;
            oldVehicle.License_SerialNumber = vhc.License_SerialNumber;
            oldVehicle.License_ExpirationDate = vhc.License_ExpirationDate; 
            oldVehicle.License_Registeration = vhc.License_Registeration;
            oldVehicle.License_SerialNumber = vhc.License_SerialNumber;
            oldVehicle.Vehicle_Type = vhc.Vehicle_Type; 
            oldVehicle.Vehicle_ManufactureYear = vhc.Vehicle_ManufactureYear;

            db.SaveChanges();
        }
        //-------------------------------------------------------
        public void DeleteVehicle(string PltNum)
        {
            var model = db.Vehicle.FirstOrDefault(v => v.Vehicle_PlateNumber == PltNum);
            if (model == null) { return; }

            db.Vehicle.Remove(model);
            db.SaveChanges();
        }
        //-------------------------------------------------------
        public List<EmployeesVehiclesDTO> GetOccupiedVehicles(int branchId)
        {
            var vehiclesInBranch = db.EmployeesVehicles.Where(ev => ev.Employee.Branch_ID == branchId && ev.Vehicle.Branch_ID == branchId).ToList();

            var dtoList = vehiclesInBranch.Select(ev => new EmployeesVehiclesDTO
            {
                EmployeeId = ev.EmployeeId,
                VehiclePlateNumber = ev.VehiclePlateNumber
            }).ToList();

            return dtoList;
        }
        //-------------------------------------------------------
        public bool AssignEmpToVehicle(string employeeId, string vehiclePlateNumber, int branchId)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Employee_ID == employeeId);
            var vehicle = db.Vehicle.FirstOrDefault(v => v.Vehicle_PlateNumber == vehiclePlateNumber);

            if (employee != null && vehicle != null && employee.Branch_ID == vehicle.Branch_ID && employee.Employee_Role == "Driver")
            {
                var employeeAssignedToVehicle = new EmployeesVehicle
                {
                    EmployeeId = employeeId,
                    VehiclePlateNumber = vehiclePlateNumber
                };

                db.EmployeesVehicles.Add(employeeAssignedToVehicle);
                db.SaveChanges();

                return true;
            }

            else { return false; }
        }
        //-------------------------------------------------------
        public bool FreeVehicleFromEmployees(string PltNum) 
        {
            var models = db.EmployeesVehicles.Where(ev => ev.VehiclePlateNumber == PltNum).ToList();

            if (models.Count > 1) 
            {
                db.EmployeesVehicles.RemoveRange(models);   //"Remove" removes 1 record, "RemoveRange" removes more than 1 record
                db.SaveChanges();

                return true;
            }
            else { return false; }
        }
        //-------------------------------------------------------
        public bool FreeVehicleFromSingleEmployee(string employeeId, string PltNum)
        {
            var employeeVehicle = db.EmployeesVehicles.FirstOrDefault(ev => ev.EmployeeId == employeeId && ev.VehiclePlateNumber == PltNum);

            if (employeeVehicle != null)
            {
                db.EmployeesVehicles.Remove(employeeVehicle);
                db.SaveChanges();
                return true;
            }

            else { return false; }
        }
        //-------------------------------------------------------
    }
}
