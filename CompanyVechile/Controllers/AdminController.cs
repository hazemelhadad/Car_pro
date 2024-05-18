using CompanyVechile.DTO;
using CompanyVechile.Models;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyVechile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminRepo AdminRepo;
        private readonly UserManager<applicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //--------------------------------------------------------------------------------
        public AdminController( IAdminRepo _AdminRepo,  RoleManager<IdentityRole> roleManager, UserManager<applicationUser> userManager)
        {
            AdminRepo = _AdminRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        //--------------------------------------------------------------------------------
        private int GetBranchIdFromToken()
        {
            var branchIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BranchId");

            if ( branchIdClaim != null && int.TryParse(branchIdClaim.Value, out int branchId) ) { return branchId; }
            return 0;
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetAllEmployeesByBranch")]   //Returns all Employees in Database (for logged in Admin Branch_ID)
        [Authorize(Roles = "Admin")]

        public IActionResult GetAllEmployees()
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetAll(branchId);
            if (model == null) { return NotFound("لم يتم العثور على الموظفين"); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetEmployeeByHisNationalID/{id}")]   //Returns Employee by searching his ID
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployeeById(string id) 
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetEmpByID(id,branchId);
            if (model == null) { return NotFound("لم يتم العثور على الموظف"); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetEmployeeByHisName/{name}")]   //Returns Employee by searching his Name
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployeeByName(string name)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetEmpByName(name,branchId);
            if (model == null) { return NotFound("لم يتم العثور على الموظف"); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AddEmployee")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEmployee(AdminEmployeeDTO empDto)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            if (empDto == null) { return BadRequest(); }

            if (empDto.Branch_ID != branchId) { return BadRequest("لا يمكن إضافة موظف إلى فرع مختلف"); }

            if (empDto.Employee_Role == "Driver") {
                AdminRepo.AddEmp(empDto);
                var model = AdminRepo.GetAll(branchId);
                return Ok(model);
            }
            else {  return BadRequest("لا يمكن اضافه وظيفه للموظف غير السائق"); }
           
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/AdminController/UpdateEmployeeData/{id}")] //id Sent in url
        [Authorize(Roles = "Admin")]
        public IActionResult EditEmployee(AdminEmployeeDTO empDto, string id) //sent in body of Request (empDTO)
        {
            if (empDto == null) { return BadRequest("طلب غير صحيح"); }
            if (empDto.Employee_ID != id) { return BadRequest("طلب غير صحيح"); }

            AdminRepo.EditEmp(empDto, id); 

            return Ok(empDto);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/DeleteEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteEmployee(string id)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetEmpByID(id, branchId);

            if (model == null) { return NotFound("لم يتم العثور على الموظف"); }

            AdminRepo.DeleteEmp(id);
            return Ok(model);
        }

        //--------------------------------------------------------------------------------
                    //----------------------VEHICLES----------------------//
        //--------------------------------------------------------------------------------

        [HttpGet("/api/AdminController/GetAllVehiclesByBranch/")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBranchVehicles()
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetAllVehicles(branchId);
            if (model == null) { return NotFound("لم يتم العثور على المركبات"); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetVehicleByPltNum/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetVehicleByPlateNumberViaBranch(string PltNum)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null) { return NotFound("لم يتم العثور على المركبة"); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetVehicleByType/{type}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetVehicleByType(string type)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetVehicleByType(type, branchId);
            if (model == null) { return NotFound("لم يتم العثور على المركبات بهذا النوع"); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AddVehicle")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVehicleForBranch(VehicleDTO vhc)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            if (vhc.Branch_ID != branchId)
            {
                return BadRequest("لا يمكن إضافة مركبة إلى فرع مختلف عن فرعك.");
            }

            AdminRepo.AddVehicle(vhc);
            return Ok(vhc);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/AdminController/UpdateVehicleData/{PltNum}")]
        [Authorize(Roles = "Admin")]//PltNum sent in URL
        public IActionResult EditVehicle(VehicleDTO vhc, string PltNum) //sent in body of Request (vhc)
        {
            if (vhc == null) { return BadRequest("طلب غير صحيح"); }
            if (vhc.Vehicle_PlateNumber != PltNum) { return BadRequest("طلب غير صحيح"); }

            AdminRepo.EditVhc(vhc, PltNum);

            return Ok(vhc);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/DeleteVehicle/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVehicles(string PltNum)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null) { return NotFound("لم يتم العثور على المركبة"); }

            AdminRepo.DeleteVehicle(PltNum);    
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetAllVehiclesInUse")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllVehiclesInUse()
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var model = AdminRepo.GetOccupiedVehicles(branchId);
            if (model == null) { return NotFound("لم يتم العثور على المركبات المستخدمة"); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AssignEmployeeToVehicle")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignEmployeeToVehicle(EmployeesVehiclesDTO evo)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            bool success = AdminRepo.AssignEmpToVehicle(evo.EmployeeId, evo.VehiclePlateNumber, branchId);

            if (!success)
            {
                return BadRequest("فشل في تعيين الموظف إلى المركبة.");
            }

            return Ok(evo);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/FreeTheVehicleFromAllEmployees/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FreeVehicle(string PltNum)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var vehicle = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (vehicle == null) { return NotFound("لم يتم العثور على المركبة"); }

            AdminRepo.FreeVehicleFromEmployees(PltNum);
            return Ok(vehicle);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/FreeTheVehicleFromSingleEmployee/{id}/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FreeVehicleFromOne(string id, string PltNum)
        {
            var branchId = GetBranchIdFromToken();

            if (branchId == 0) { return Unauthorized("معرف الفرع غير موجود في الرمز المميز."); }

            var vehicle = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (vehicle == null) { return NotFound("لم يتم العثور على المركبة"); }

            AdminRepo.FreeVehicleFromSingleEmployee(id, PltNum);
            return Ok(vehicle);
        }
        //--------------------------------------------------------------------------------
    }
}
