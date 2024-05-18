using CompanyVechile.DTO;
using CompanyVechile.Models;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CompanyVechile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<applicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IAdminRepo adminRepo;
        private readonly JwtSettings _jwtSettings;
        //------------------------------------------------------------------------------------------------------
        public AccountController(UserManager<applicationUser>userManager, IConfiguration configuration , IAdminRepo adminRepo,RoleManager<IdentityRole> roleManager, JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.adminRepo = adminRepo;
            _jwtSettings = jwtSettings;
        }
        //------------------------------------------------------------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> registration(registerUserDTO registerDto)
        {
            var applicationUser = new applicationUser();

            if (await userManager.FindByNameAsync(registerDto.EmployeeID) != null) { return BadRequest("اسم المستخدم موجود بالفعل"); }

            try
            {
                applicationUser.UserName = registerDto.EmployeeID;

                IdentityResult user = await userManager.CreateAsync(applicationUser, registerDto.Password);

                if (user.Succeeded)
                {
                    await assignRole(applicationUser);
                    return Ok("تم التسجيل بنجاح");
                }

                else { return BadRequest("فشل التسجيل"); }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        //------------------------------------------------------------------------------------------------------
        private async Task assignRole(applicationUser user)
        {
            IdentityRole role = new IdentityRole("SuperAdmin");

            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await roleManager.CreateAsync(role);
            }
            await userManager.AddToRoleAsync(user, role.Name);
        }
        //------------------------------------------------------------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Logining(loginUserDTO userDTO)
        {
            if (userDTO == null) { return BadRequest("طلب تسجيل الدخول غير صالح."); }

            var user = await userManager.FindByNameAsync(userDTO.employeeID);

            if (user == null) { return BadRequest("اسم المستخدم غير موجود"); }

            if (await userManager.CheckPasswordAsync(user, userDTO.password))
            {
                var employee = adminRepo.GetEmpByID(userDTO.employeeID);

                if (employee == null) { return BadRequest("الموظف غير موجود في قاعدة البيانات."); }

                if (!employee.Branch_ID.HasValue) { return BadRequest("الموظف ليس لديه معرف فرع."); }

                var branchId = employee.Branch_ID.Value;
                var token = await GenerateJwtToken(user, branchId);
                return Ok(new { Token = token });
            }
            else { return BadRequest("كلمة المرور غير صحيحة"); }
        }

        //------------------------------------------------------------------------------------------------------
        private async Task<string> GenerateJwtToken(applicationUser user , int branchId)
        {
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim("BranchId", branchId.ToString()),
                new Claim(ClaimTypes.Role, Guid.NewGuid().ToString())
            };

            if(roles != null)
            {
                foreach(var role in roles) {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)); ;

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
        //------------------------------------------------------------------------------------------------------
    }
    //------------------------------------------------------------------------------------------------------

}





