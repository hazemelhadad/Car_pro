using CompanyVechile.DTO;
using CompanyVechile.Models;

namespace CompanyVechile.Repositories
{
    public interface IAdminRepo
    {
        //----Employees Region----
        List<AdminEmployeeDTO> GetAll(int branchId);
        AdminEmployeeDTO GetEmpByID(string id, int branchId);
        Employees GetEmpByID(string id);
        List<AdminEmployeeDTO> GetEmpByName(string name, int branchId);
        void AddEmp(AdminEmployeeDTO empDto);
        void EditEmp(AdminEmployeeDTO empDto, string id);
        void DeleteEmp(string id);

        //----Vehicles Region----
        List<VehicleDTO> GetAllVehicles(int branchId);
        List<VehicleDTO> GetVehicleByPlateNumber(string PltNum, int branchId);
        List<VehicleDTO> GetVehicleByType(string type, int branchId);
        void AddVehicle(VehicleDTO vhc);
        void EditVhc(VehicleDTO vhc, string PltNum);
        void DeleteVehicle(string PltNum);

        //-----EmployeeVehicles Region----
        List<EmployeesVehiclesDTO> GetOccupiedVehicles(int branchId);
        bool AssignEmpToVehicle(string employeeId, string vehiclePlateNumber, int branchId);

        void FreeVehicleFromEmployees(string PltNum);
        void FreeVehicleFromSingleEmployee(string employeeId, string PltNum);
    }
}
