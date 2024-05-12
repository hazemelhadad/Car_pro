using CompanyVechile.DTO;
using CompanyVechile.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyVechile.Repositories
{
    public class SuperAdminRepo : ISuperAdminRepo
    {
        private readonly CompanyDBContext db;
        public SuperAdminRepo(CompanyDBContext dbContext)
        {
            db = dbContext;
        }
        //-------------------------------------------------------
        public List<BranchesDTO> GetAllCompanyBranches()
        {
            return db.Branches.Select(b => new BranchesDTO
            {
                Branch_ID= b.Branch_ID,
                Branch_Location=b.Branch_Location,
                Branch_Name=b.Branch_Name,
            }).ToList();
        }
        //-------------------------------------------------------
        public void AddNewBranch(NewBranchesDTO brDTO)
        {
            var branch = new Branches
            {
                Branch_Name = brDTO.Branch_Name,
                Branch_Location = brDTO.Branch_Location,
            };

            db.Branches.Add(branch);
            db.SaveChanges();
        }
        //-------------------------------------------------------
        public bool EditBranch(BranchesDTO brDTO,int BranchID)
        {
            var oldBranch = db.Branches.FirstOrDefault(b => b.Branch_ID == BranchID);
            if (oldBranch == null) { return false; }

            oldBranch.Branch_Name = brDTO.Branch_Name;
            oldBranch.Branch_Location = brDTO.Branch_Location;

            db.SaveChanges();
            return true;
        }
        //-------------------------------------------------------
        public bool DeleteBranch(int BranchID)
        {
            var branch = db.Branches.FirstOrDefault(b => b.Branch_ID == BranchID);
            if (branch == null) { return false; }

            db.Branches.Remove(branch);
            db.SaveChanges();

            return true;
        }
        //-------------------------------------------------------
        public List<EmployeeDTO> GetAllEmps()
        {
            return db.Employees.Include(e => e.EmployeePhones).Select(e => new EmployeeDTO
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
            }).ToList();
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
        public bool EditEmployee(EmployeeDTO empDto, int id)
        {
            var existingEmployee = db.Employees.FirstOrDefault(e => e.Employee_ID == id);
            if (existingEmployee == null) { return false; }

            //These 2 lines are used to ensure that before transferring an Employee from a branch to another, he MUST NOT have a vehicle occupied to him.
            var employeeHasVehicles = db.EmployeesVehicles.Any(ev => ev.EmployeeId == id);
            if (employeeHasVehicles) { return false; }

            existingEmployee.Employee_Name = empDto.Employee_Name;
            existingEmployee.Employee_Birthday = empDto.Employee_Birthday;
            existingEmployee.Employee_Role = empDto.Employee_Role;
            existingEmployee.Employee_Nationality = empDto.Employee_Nationality;
            existingEmployee.Employee_Street_Name = empDto.Employee_Street_Name;
            existingEmployee.Employee_BuildingNumber = empDto.Employee_BuildingNumber;
            existingEmployee.Employee_City = empDto.Employee_City;
            existingEmployee.Branch_ID = empDto.Branch_ID;

            db.SaveChanges();
            return true;
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
        public bool DeleteEmp(int id)
        {
            var model = db.Employees.FirstOrDefault(p => p.Employee_ID == id);
            if (model == null) { return false; }

            var EmpHasVehicle = db.EmployeesVehicles.Any(e => e.EmployeeId == id);
            if (EmpHasVehicle) { return false; }

            db.Employees.Remove(model);
            db.SaveChanges();
            return true;
        }
        //-------------------------------------------------------
    }
}
