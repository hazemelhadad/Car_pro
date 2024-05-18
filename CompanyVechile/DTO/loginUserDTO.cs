using System.ComponentModel.DataAnnotations;

namespace CompanyVechile.DTO
{
    public class loginUserDTO
    {
        [Required]
        public string employeeID {  get; set; }
        [Required]
        public string password { get; set; }
       
        

    }
}
