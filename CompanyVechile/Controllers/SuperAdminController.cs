using CompanyVechile.DTO;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyVechile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        ISuperAdminRepo SuperAdminRepo;
        //--------------------------------------------------------------------------------
        public SuperAdminController(ISuperAdminRepo _SuperAdminRepo)
        {
            SuperAdminRepo = _SuperAdminRepo;
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllBranches")]
        public IActionResult GetAllBranches()
        {
            var model = SuperAdminRepo.GetAllCompanyBranches();
            if (model == null) { return NotFound("لا توجد فروع متاحة."); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/SuperAdminController/AddBranch")]
        public IActionResult AddBranch(NewBranchesDTO brDTO)
        {
            if (brDTO == null) { return BadRequest(); }

            SuperAdminRepo.AddNewBranch(brDTO);
            return Ok(brDTO);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/SuperAdminController/EditBranch/{BranchID}")]
        public IActionResult EditBranch(BranchesDTO brDTO, int BranchID)
        {
            if (brDTO == null) { return BadRequest("لا يمكن إضافة فرع جديد. البيانات المقدمة غير صالحة."); }
            if (brDTO.Branch_ID != BranchID) { return BadRequest(); }

            var bol = SuperAdminRepo.EditBranch(brDTO, BranchID);
            if (bol == true) { return Ok(brDTO); }

            else { return BadRequest("لا يمكن إضافة فرع جديد. البيانات المقدمة غير صالحة."); }
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/SuperAdminController/DeleteBranch")]
        public IActionResult DeleteBranch(int? BranchID)
        {
            if (BranchID == null || BranchID <= 0) { return BadRequest("يرجى توفير معرف فرع صحيح."); }

            var deleted = SuperAdminRepo.DeleteBranch(BranchID.Value);

            if (!deleted) { return NotFound($"الفرع ذو الرقم التعريفي {BranchID} غير موجود."); }

            return Ok("تم حذف الفرع بنجاح.");
        }

        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetAllCompanyEmployees")]
        public IActionResult GetAllCompanyEmployees()
        {
            var model = SuperAdminRepo.GetAllEmps();
            if (model == null) { return NotFound("لا يوجد موظفين في الشركة."); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/SuperAdminController/GetEmployeeByHisNationalID/{id}")]   //Returns Employee by searching his ID
        public IActionResult GetEmployeeById(int id)
        {
            var model = SuperAdminRepo.GetEmpByID(id);

            if (model == null) { return NotFound("لا يوجد موظف برقم الهوية هذا في الشركة."); }

            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("/api/SuperAdminController/UpdateEmployees/{id}")]
        public IActionResult UpdateEmployees(EmployeeDTO empDto, int id)
        {
            if (empDto == null) { return BadRequest(); }
            if (empDto.Employee_ID != id) { return BadRequest(); }

            var boolean = SuperAdminRepo.EditEmployee(empDto, id);
            if (boolean == true)
            {
                return Ok(empDto);
            }
            else { return BadRequest($"لديه مركبه في عهدته, برجاء أن يتم اخلاء المركبه أولا. {id} :الموظف الذي لديه رقم قومي"); }
        }
        //--------------------------------------------------------------------------------
        [HttpPost("/api/SuperAdminController/AddEmployee")]
        public IActionResult AddEmployee(EmployeeDTO empDto)
        {
            if (empDto == null) { return BadRequest(); }
            SuperAdminRepo.AddEmp(empDto);

            var model = SuperAdminRepo.GetAllEmps();
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpDelete("/api/SuperAdminController/DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var model = SuperAdminRepo.GetEmpByID(id);

            if (model == null) { return NotFound(); }

            var boolean = SuperAdminRepo.DeleteEmp(id);
            if (boolean == true)
            {
                return Ok(model);
            }
            else { return BadRequest($"لديه مركبه في عهدته, برجاء أن يتم اخلاء المركبه أولا. {id} :الموظف الذي لديه رقم قومي"); }
        }
        //--------------------------------------------------------------------------------
    }
}
