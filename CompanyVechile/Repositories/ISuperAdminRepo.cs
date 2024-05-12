using CompanyVechile.DTO;
using CompanyVechile.Models;

namespace CompanyVechile.Repositories
{
    public interface ISuperAdminRepo
    {
        //Branches Region
        List<BranchesDTO> GetAllCompanyBranches();
        void AddNewBranch(NewBranchesDTO brDTO);
        bool EditBranch(BranchesDTO brDTO, int BranchID);
        bool DeleteBranch(int BranchID);

        //Employees Region
        List<EmployeeDTO> GetAllEmps();
        EmployeeDTO GetEmpByID(int id);
        bool EditEmployee(EmployeeDTO empDto, int id);
        void AddEmp(EmployeeDTO empDto);
        bool DeleteEmp(int id);

        //Vehicles Region
        List<VehicleDTO> GetAllVehicles();
        List<VehicleDTO> GetVehicleByPlateNumber(string PltNum);
        List<VehicleDTO> GetVehicleByType(string type);
        bool AddVehicle(VehicleDTO vhc);
        bool EditVhc(VehicleDTO vhc, string PltNum);
        bool DeleteVehicle(string PltNum);

        //-----EmployeeVehicles Region----
        List<EmployeesVehiclesDTO> GetOccupiedVehicles();
    }
}