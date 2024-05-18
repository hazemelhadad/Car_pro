using System.ComponentModel.DataAnnotations;

namespace CompanyVechile.DTO
{
    public class registerUserDTO
    {
        [Required]

        public string EmployeeID { get; set; }
        public string Password{get; set;}

        //public string UserName { get; set;}

        }
}
