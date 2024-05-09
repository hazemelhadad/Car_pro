﻿using CompanyVechile.DTO;
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
    }
}