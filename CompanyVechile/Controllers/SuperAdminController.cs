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
    public class SuperAdminController : ControllerBase
    {
        ISuperAdminRepo SuperAdminRepo;
        private readonly UserManager<applicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //--------------------------------------------------------------------------------
        public SuperAdminController(ISuperAdminRepo _SuperAdminRepo, RoleManager<IdentityRole> roleManager, UserManager<applicationUser> userManager)
        {
            SuperAdminRepo = _SuperAdminRepo;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllBranches")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllBranches()
        {
            var model = SuperAdminRepo.GetAllCompanyBranches();
            if (model == null) { return NotFound(new { error = "لا توجد فروع متاحة." }); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/SuperAdminController/AddBranch")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddBranch(NewBranchesDTO brDTO)
        {
            if (brDTO == null) { return BadRequest(new { error = "طلب غير صالح." }); }

            SuperAdminRepo.AddNewBranch(brDTO);
            return Ok(brDTO);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/SuperAdminController/EditBranch/{BranchID}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult EditBranch(BranchesDTO brDTO, int BranchID)
        {
            if (brDTO == null) { return BadRequest(new { error = "لا يمكن إضافة فرع جديد. البيانات المقدمة غير صالحة." }); }
            if (brDTO.Branch_ID != BranchID) { return BadRequest(new { error = "لا يمكن تعديل الفرع. البيانات المقدمة غير صالحة." }); }

            var bol = SuperAdminRepo.EditBranch(brDTO, BranchID);
            if (bol == true) { return Ok(brDTO); }

            else { return BadRequest(new { error = "لا يمكن إضافة فرع جديد. البيانات المقدمة غير صالحة." }); }
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/SuperAdminController/DeleteBranch/{BranchID}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteBranch(int? BranchID)
        {
            if (BranchID == null || BranchID <= 0) { return BadRequest(new { error = "يرجى توفير معرف فرع صحيح." }); }

            var deleted = SuperAdminRepo.DeleteBranch(BranchID.Value);

            if (!deleted) { return NotFound(new { error = $"الفرع ذو الرقم التعريفي {BranchID} غير موجود." }); }

            return Ok(new { message = "تم حذف الفرع بنجاح." });
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllCompanyEmployees")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllCompanyEmployees()
        {
            var model = SuperAdminRepo.GetAllEmps();
            if (model == null) { return NotFound(new { error = "لا يوجد موظفين في الشركة." }); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetEmployeeByHisNationalID/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        //Returns Employee by searching his ID
        public IActionResult GetEmployeeById(string id)
        {
            var model = SuperAdminRepo.GetEmpByID(id);

            if (model == null) { return NotFound(new { error = "لا يوجد موظف برقم الهوية هذا في الشركة." }); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/SuperAdminController/UpdateEmployees/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult UpdateEmployees(EmployeeDTO empDto, string id)
        {
            if (empDto == null) return BadRequest(new { error = $"الموظف غير موجود. لا يمكن حذف موظف غير موجود." });
            if (empDto.Employee_ID != id) return BadRequest(new { error = $"الموظف غير موجود. لا يمكن حذف موظف غير موجود." });

            var boolean = SuperAdminRepo.EditEmployee(empDto, id);

            if (boolean == true) { return Ok(empDto); }
            else { return BadRequest(new { error = $"لديه مركبه في عهدته, برجاء اخلاء المركبه أولا. {id} :الموظف الذي لديه رقم قومي" }); }
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/SuperAdminController/AddEmployee")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AddEmployeeAsync(EmployeeDTO empDto)
        {
            if (empDto == null) { return BadRequest(new { error = "البيانات المقدمة غير صالحة." }); }

            var res = await addLoginToEmployee(empDto);
            if (res == "تم التسجيل بنجاح")
            {
                var userId = userManager.FindByNameAsync(empDto.Employee_ID).Result.Id;
                empDto.UserId = userId;
                SuperAdminRepo.AddEmp(empDto);
                var model = SuperAdminRepo.GetAllEmps();
                return Ok(model);
            }

            else { return BadRequest(new { error = res }); }
        }
        //--------------------------------------------------------------------------------
        private async Task<string> addLoginToEmployee(EmployeeDTO empDto)
        {
            var createdResult = "";

            if (await userManager.FindByNameAsync(empDto.Employee_ID) != null) { createdResult = "اسم المستخدم موجود بالفعل"; }

            try
            {
                var applicationUser = new applicationUser();
                applicationUser.UserName = empDto.Employee_ID;
                IdentityResult user = await userManager.CreateAsync(applicationUser, empDto.Password);

                if (user.Succeeded)
                {
                    if (assignRole(applicationUser).Result.Succeeded) { createdResult = "تم التسجيل بنجاح"; }
                    else { createdResult = "فشل التسجيل"; }
                }

                else { createdResult = "فشل التسجيل"; }
            }

            catch (Exception ex) { createdResult = ex.Message; }
            return createdResult;
        }
        //--------------------------------------------------------------------------------
        private async Task<IdentityResult> assignRole(applicationUser user)
        {
            IdentityRole role = new IdentityRole("Admin");

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(role);
            }
            return await userManager.AddToRoleAsync(user, role.Name);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/SuperAdminController/DeleteEmployee/{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteEmployee(string id)
        {
            var model = SuperAdminRepo.GetEmpByID(id);

            if (model == null) return BadRequest(new { error = $"الموظف غير موجود. لا يمكن حذف موظف غير موجود." });

            var boolean = SuperAdminRepo.DeleteEmp(id);
            if (boolean == true)
            {
                return Ok(model);
            }
            else { return BadRequest(new { error = $"لديه مركبه في عهدته, برجاء اخلاء المركبه أولا {id} :الموظف الذي لديه رقم قومي" }); }
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllVehicles")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllCompanyVehicles()
        {
            var model = SuperAdminRepo.GetAllVehicles();
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبات" }); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetVehicleByPltNum/{PltNum}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetVehicleByPlateNumberViaBranch(string PltNum)
        {
            var model = SuperAdminRepo.GetVehicleByPlateNumber(PltNum);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبة" }); };

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetVehicleByType/{type}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetVehicleByType(string type)
        {
            var model = SuperAdminRepo.GetVehicleByType(type);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبات" }); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/SuperAdminController/AddVehicle")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult AddVehicle(VehicleDTO vhc)
        {
            var boolean = SuperAdminRepo.AddVehicle(vhc);
            if (boolean == true) { return Ok(vhc); }
            else { return BadRequest(new { error = "لا يوجد في الشؤكه فرع بهذا الرقم" }); }
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/SuperAdminController/UpdateVehicleData/{PltNum}")]
        [Authorize(Roles = "SuperAdmin")]//PltNum sent in URL
        public IActionResult EditVehicle(VehicleDTO vhc, string PltNum) //sent in body of Request (vhc)
        {
            if (vhc == null) { return BadRequest(new { error = "طلب غير صحيح" }); }
            if (vhc.Vehicle_PlateNumber != PltNum) { return BadRequest(new { error = "طلب غير صحيح" }); }

            var boolean = SuperAdminRepo.EditVhc(vhc, PltNum);
            if (boolean == true) { return Ok(vhc); }
            else { return BadRequest(new { error = "لا يمكن نقل المركبة إلى فرع مختلف قبل إزالة الموظفين منها" }); }

        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/SuperAdminController/DeleteVehicle/{PltNum}")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteVehicles(string PltNum)
        {
            var model = SuperAdminRepo.GetVehicleByPlateNumber(PltNum);
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبة" }); }

            var boolean = SuperAdminRepo.DeleteVehicle(PltNum);
            if (boolean == true)
            {
                return Ok(new { message = "تم حذف المركبة" });
            }
            else { return BadRequest(new { error = "تعذر حذف المركبة" }); }
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllVehiclesInUse")]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAllVehiclesInUse()
        {
            var model = SuperAdminRepo.GetOccupiedVehicles();
            if (model == null) { return NotFound(new { error = "لم يتم العثور على المركبات المستخدمة" }); }
            if (model.Count < 1) { return Ok(new { message = "لم يتم العثور على أي مركبه مستخدمه" }); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
    }
}
