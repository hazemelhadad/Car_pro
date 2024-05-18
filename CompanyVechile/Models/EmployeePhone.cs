using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.Models
{
    public class EmployeePhone
    {
        [ForeignKey("Employees")]
        public string Employee_ID { get; set; }
        public Employees Employees { get; set; }
        public string Employee_PhoneNumber { get; set; }
    }
}
