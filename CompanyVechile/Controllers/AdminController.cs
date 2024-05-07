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
        [HttpGet]   //Returns all Employees in Database
        public IActionResult GetAllEmployees()
        {
            var model = AdminRepo.GetAll();
            if (model == null) { return NotFound(); }
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("{id}")]   //Returns Employee by searching his ID
        public IActionResult GetEmployeeById(int id) 
        {
            var model = AdminRepo.GetEmpByID(id);
            if (model == null) { return NotFound();};
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpGet("/api/employees/{name}")]   //Returns Employee by searching his Name
        public IActionResult GetEmployeeByName(string name)
        {
            var model = AdminRepo.GetEmpByName(name);
            if (model == null) { return NotFound();}
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult AddEmployee(EmployeeDTO empDto)
        {
            if (empDto == null) { return BadRequest(); }

            AdminRepo.AddEmp(empDto);

            var model= AdminRepo.GetAll();
            return Ok(model);
        }
        //--------------------------------------------------------------------------------
        [HttpPut("{id}")] //id Sent in url
        public IActionResult EditEmployee(EmployeeDTO empDto, int id) //sent in body of Request (empDTO)
        {
            if (empDto == null) { return BadRequest(); }
            if (empDto.Employee_ID != id) { return BadRequest(); } 

            AdminRepo.EditEmp(empDto, id); 

            return Ok(empDto);
        }
        //--------------------------------------------------------------------------------
    }
}
