using CompanyVechile.DTO;
using CompanyVechile.Models;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyVechile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminRepo AdminRepo;
        //--------------------------------------------------------------------------------
        public AdminController(IAdminRepo _AdminRepo)
        {
            AdminRepo = _AdminRepo;
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/GetAllEmployeesByBranch")]   //Returns all Employees in Database (for logged in Admin Branch_ID)
        public IActionResult GetAllEmployees()
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetAll(branchId);
            if (model == null) { return NotFound(); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/GetEmployeeByHisNationalID/{id}")]   //Returns Employee by searching his ID
        public IActionResult GetEmployeeById(int id) 
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetEmpByID(id,branchId);
            if (model == null) { return NotFound();};
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/GetEmployeeByHisName/{name}")]   //Returns Employee by searching his Name
        public IActionResult GetEmployeeByName(string name)
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetEmpByName(name,branchId);
            if (model == null) { return NotFound();}
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/AddEmployee")]
        public IActionResult AddEmployee(EmployeeDTO empDto)
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            if (empDto == null) { return BadRequest(); }

            if (empDto.Branch_ID != branchId)
            {
                return BadRequest("Cannot add employee to a different branch");
            }

            AdminRepo.AddEmp(empDto);


            var model= AdminRepo.GetAll(branchId);
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/UpdateEmployeeData{id}")] //id Sent in url
        public IActionResult EditEmployee(EmployeeDTO empDto, int id) //sent in body of Request (empDTO)
        {
            if (empDto == null) { return BadRequest(); }
            if (empDto.Employee_ID != id) { return BadRequest(); } 

            AdminRepo.EditEmp(empDto, id); 

            return Ok(empDto);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var branchId = 1;    //Here,apply method to return actual branch ID from Token
            var model = AdminRepo.GetEmpByID(id, branchId);

            if (model == null) { return NotFound(); }

            AdminRepo.DeleteEmp(id);
            return Ok(model);
        }

        //--------------------------------------------------------------------------------
                    //----------------------VEHICLES----------------------//
        //--------------------------------------------------------------------------------

        [HttpGet("/api/GetAllVehiclesByBranch/")]
        public IActionResult GetAllBranchVehicles()
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetAllVehicles(branchId);
            if (model == null) { return NotFound(); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/GetVehicleByPltNum/{PltNum}")]
        public IActionResult GetVehicleByPlateNumberViaBranch(string PltNum)
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null) { return NotFound(); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/GetVehicleByType/{type}")]
        public IActionResult GetVehicleByType(string type)
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token
            
            var model = AdminRepo.GetVehicleByType(type, branchId);
            if (model == null) { return NotFound();}

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/AddVehicle")]
        public IActionResult AddVehicleForBranch(VehicleDTO vhc)
        {
            var branchId = 1;   //Here,apply method to return actual branch ID from Token

            if (vhc.Branch_ID != branchId)
            {
                return BadRequest("Cannot add employee to a different branch");
            }

            AdminRepo.AddVehicle(vhc);
            return Ok(vhc);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/EditVehicle/{vhc}")]
        public IActionResult EditVehicle(VehicleDTO vhc, string PltNum)
        {
            if (vhc == null) { return BadRequest(); }
            if (vhc.Vehicle_PlateNumber != PltNum) { return BadRequest(); }

            AdminRepo.EditVhc(vhc, PltNum);

            return Ok(vhc);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/DeleteVehice/{PltNum}")]
        public IActionResult DeleteVehicles(string PltNum)
        {
            var branchId = 1;    //Here,apply method to return actual branch ID from Token

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null) { return NotFound(); }

            AdminRepo.DeleteVehicle(PltNum);
            return Ok();
        }
        //--------------------------------------------------------------------------------

    }
}
