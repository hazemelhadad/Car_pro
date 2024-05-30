using CompanyVechile.DTO;
using CompanyVechile.Models;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace CompanyVechile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepo AdminRepo;
        private readonly UserManager<applicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminController(IAdminRepo _AdminRepo, RoleManager<IdentityRole> roleManager, UserManager<applicationUser> userManager)
        {
            AdminRepo = _AdminRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        //----------------------------------------------------------------------------------------
        private int GetBranchIdFromToken()
        {
            var branchIdClaim = User.Claims.FirstOrDefault(c => c.Type == "BranchId");
            if (branchIdClaim != null && int.TryParse(branchIdClaim.Value, out int branchId))
            {
                return branchId;
            }
            return 0;
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetAllEmployeesByBranch")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllEmployees()
        {
            var branchId = GetBranchIdFromToken();
            if(branchId ==0){ return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

          
            var model = AdminRepo.GetAll(branchId);
            if (model == null || model.Count < 1) { return NotFound(new { error = "لا يوجد موظفين في فرعك" }); }
            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetEmployeeByHisNationalID/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployeeById(string id)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetEmpByID(id, branchId);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على الموظف" }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetEmployeeByHisName/{name}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetEmployeeByName(string name)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetEmpByName(name, branchId);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على الموظف" }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AddEmployee")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddEmployee(AdminEmployeeDTO empDto)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            if (empDto == null) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }
            if (empDto.Branch_ID != branchId) { return BadRequest(new { error = "لا يمكن إضافة موظف إلى فرع مختلف." }); }
            if (empDto.Employee_Role != "Driver") { return BadRequest(new { error = "لا يمكن إضافة وظيفة للموظف غير السائق." }); }

            AdminRepo.AddEmp(empDto);
            var model = AdminRepo.GetAll(branchId);
            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpPut("/api/AdminController/UpdateEmployeeData/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditEmployee(AdminEmployeeDTO empDto, string id)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            if (empDto == null) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }
            if (empDto.Employee_ID != id) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }
            if (empDto.Branch_ID != branchId) { return BadRequest(new { error = "لا يمكنك تغيير الفرع للموظف." }); }

            AdminRepo.EditEmp(empDto, id);
            return Ok(empDto);
        }
        //----------------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/DeleteEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteEmployee(string id)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetEmpByID(id, branchId);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على الموظف." }); }

            AdminRepo.DeleteEmp(id);
            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetAllVehiclesByBranch")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBranchVehicles()
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetAllVehicles(branchId);
            if (model.Count < 1) { return NotFound(new { error = "لا يوجد مركبات في فرعك." }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetVehicleByPltNum/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetVehicleByPlateNumberViaBranch(string PltNum)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null || model.Count < 1) { return NotFound(new { error = "لم يتم العثور على المركبة." }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetVehicleByType/{type}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetVehicleByType(string type)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetVehicleByType(type, branchId);
            if (model.Count < 1) { return NotFound(new { error = "لا يوجد مركبات في فرعك بهذا النوع." }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AddVehicle")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddVehicleForBranch(VehicleDTO vhc)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            if (vhc.Branch_ID != branchId) { return BadRequest(new { error = "لا يمكن إضافة مركبة إلى فرع مختلف عن فرعك." }); }

            AdminRepo.AddVehicle(vhc);
            return Ok(vhc);
        }
        //----------------------------------------------------------------------------------------
        [HttpPut("/api/AdminController/UpdateVehicleData/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditVehicle(VehicleDTO vhc, string PltNum)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            if (vhc == null) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }
            if (vhc.Vehicle_PlateNumber != PltNum) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }
            if (vhc.Branch_ID != branchId) { return BadRequest(new { error = "لا يمكنك تغيير الفرع للمركبة." }); }

            AdminRepo.EditVhc(vhc, PltNum);
            return Ok(vhc);
        }
        //----------------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/DeleteVehicle/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteVehicles(string PltNum)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبة." }); }

            AdminRepo.DeleteVehicle(PltNum);
            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpGet("/api/AdminController/GetAllVehiclesInUse")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllVehiclesInUse()
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var model = AdminRepo.GetOccupiedVehicles(branchId);
            if (model.Count < 1) { return NotFound(new { error = "لم يتم العثور على المركبات المستخدمة." }); }

            return Ok(model);
        }
        //----------------------------------------------------------------------------------------
        [HttpPost("/api/AdminController/AssignEmployeeToVehicle")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignEmployeeToVehicle(EmployeesVehiclesDTO evo)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            bool success = AdminRepo.AssignEmpToVehicle(evo.EmployeeId, evo.VehiclePlateNumber, branchId);
            if (!success) { return BadRequest(new { error = "فشل في تعيين الموظف إلى المركبة." }); }

            return Ok(evo);
        }
        //----------------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/FreeTheVehicleFromAllEmployees/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FreeVehicle(string PltNum)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var vehicle = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (vehicle == null) { return NotFound(new { error = "لم يتم العثور على المركبة." }); }

            var bol = AdminRepo.FreeVehicleFromEmployees(PltNum);
            if (bol) { return Ok(new { message = "تم تحرير السياره من جميع الموظفين." }); }

            return BadRequest(new { error = "لا يوجد موظفين/موظف يستعملون تلك المركبة." });
        }
        //----------------------------------------------------------------------------------------
        [HttpDelete("/api/AdminController/FreeTheVehicleFromSingleEmployee/{id}/{PltNum}")]
        [Authorize(Roles = "Admin")]
        public IActionResult FreeVehicleFromOne(string id, string PltNum)
        {
            var branchId = GetBranchIdFromToken();
            if (branchId == 0) { return Unauthorized(new { error = "معرف الفرع غير موجود في الرمز المميز." }); }

            var vehicle = AdminRepo.GetVehicleByPlateNumber(PltNum, branchId);
            if (vehicle == null) { return NotFound(new { error = "لم يتم العثور على المركبة." }); }

            var bol = AdminRepo.FreeVehicleFromSingleEmployee(id, PltNum);
            if (bol) { return Ok(new { message = "تم تحرير السياره من الموظف." }); }

            return BadRequest(new { error = "فشلت العمليه." });
        }
    }
}
