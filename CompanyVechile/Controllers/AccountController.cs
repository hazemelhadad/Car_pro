using CompanyVechile.DTO;
using CompanyVechile.Models;
using CompanyVechile.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        public AccountController(
            UserManager<applicationUser> userManager,
            IConfiguration configuration,
            IAdminRepo adminRepo,
            RoleManager<IdentityRole> roleManager,
            JwtSettings jwtSettings)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.adminRepo = adminRepo;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(registerUserDTO registerDto)
        {
            if (await userManager.FindByNameAsync(registerDto.EmployeeID) != null)
            {
                return BadRequest(new { error = "اسم المستخدم موجود بالفعل" });
            }

            var applicationUser = new applicationUser
            {
                UserName = registerDto.EmployeeID
            };

            try
            {
                var result = await userManager.CreateAsync(applicationUser, registerDto.Password);

                if (result.Succeeded)
                {
                    await AssignRole(applicationUser);
                    return Ok(new { message = "تم التسجيل بنجاح" });
                }

                return BadRequest(new { error = "فشل التسجيل" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task AssignRole(applicationUser user)
        {
            const string roleName = "SuperAdmin";

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await userManager.AddToRoleAsync(user, roleName);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(loginUserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest(new { error = "طلب تسجيل الدخول غير صالح." });
            }

            var user = await userManager.FindByNameAsync(userDTO.employeeID);

            if (user == null)
            {
                return BadRequest(new { error = "اسم المستخدم غير موجود" });
            }

            if (await userManager.CheckPasswordAsync(user, userDTO.password))
            {
                var employee = adminRepo.GetEmpByID(userDTO.employeeID);

                if (employee == null)
                {
                    return BadRequest(new { error = "الموظف غير موجود في قاعدة البيانات." });
                }

                if (!employee.Branch_ID.HasValue)
                {
                    return BadRequest(new { error = "الموظف ليس لديه معرف فرع." });
                }

                var branchId = employee.Branch_ID.Value;
                var token = await GenerateJwtToken(user, branchId,userDTO);
                return Ok(new { message = "تم تسجيل الدخول بنجاح", Token = token });
            }

            return BadRequest(new { error = "كلمة المرور غير صحيحة" });
        }

        private async Task<string> GenerateJwtToken(applicationUser user, int branchId, loginUserDTO loginUserDTO)
        {
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
     {
         new Claim(ClaimTypes.NameIdentifier, user.UserName),
         new Claim("BranchId", branchId.ToString()),
         new Claim("EmplyeeName", adminRepo.getEmployeeNameById(loginUserDTO.employeeID).ToString()),//add name to taken
         new Claim(ClaimTypes.Role, Guid.NewGuid().ToString())
     };
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}