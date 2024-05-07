using System.Collections.Generic;
using System.Linq;
using CompanyVechile.DTO; 
using CompanyVechile.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CompanyVechile.Repositories
{
    public interface IAdminRepo
    {
        List<EmployeeDTO> GetAll();
        EmployeeDTO GetEmpByID(int id);
        List<EmployeeDTO> GetEmpByName(string name);
        void AddEmp(EmployeeDTO empDto);
        void EditEmp(EmployeeDTO empDto, int id);
    }
    //-------------------------------------------------------
    public class AdminRepo : IAdminRepo
    {
        private readonly CompanyDBContext db;
        //-------------------------------------------------------
        public AdminRepo(CompanyDBContext dbContext)
        {
            db = dbContext;
        }
        //-------------------------------------------------------
        public List<EmployeeDTO> GetAll()
        {
            return db.Employees
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
        public EmployeeDTO GetEmpByID(int id)
        {
            var employee = db.Employees
                             .Where(e => e.Employee_ID == id)
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
        public List<EmployeeDTO> GetEmpByName(string name)
        {
            return db.Employees
                     .Where(e => e.Employee_Name.Contains(name))
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
            existingEmployee.Branch_ID = empDto.Branch_ID;

            db.SaveChanges();
        }

        //-------------------------------------------------------
    }
}
