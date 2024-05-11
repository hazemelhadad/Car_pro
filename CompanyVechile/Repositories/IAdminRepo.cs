using CompanyVechile.DTO;
using CompanyVechile.Models;

namespace CompanyVechile.Repositories
{
    public interface IAdminRepo
    {
        //----Employees Region----
        List<EmployeeDTO> GetAll(int branchId);
        EmployeeDTO GetEmpByID(int id, int branchId);
        List<EmployeeDTO> GetEmpByName(string name, int branchId);
        void AddEmp(EmployeeDTO empDto);
        void EditEmp(EmployeeDTO empDto, int id);
        void DeleteEmp(int id);

        //----Vehicles Region----
        List<VehicleDTO> GetAllVehicles(int branchId);
        List<VehicleDTO> GetVehicleByPlateNumber(string PltNum, int branchId);
        List<VehicleDTO> GetVehicleByType(string type, int branchId);
        void AddVehicle(VehicleDTO vhc);
        void EditVhc(VehicleDTO vhc, string PltNum);
        void DeleteVehicle(string PltNum);

        //-----EmployeeVehicles Region----
        List<EmployeesVehiclesDTO> GetOccupiedVehicles(int branchId);
        bool AssignEmpToVehicle(int employeeId, string vehiclePlateNumber, int branchId);

        void FreeVehicleFromEmployees(string PltNum);
        void FreeVehicleFromSingleEmployee(int employeeId, string PltNum);
    }
}
