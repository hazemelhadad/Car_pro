using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.Models
{
    public class EmployeePhone
    {
        [ForeignKey("Employees")]
        public int Employee_ID { get; set; }
        public Employees Employees { get; set; }
        public int Employee_PhoneNumber { get; set; }
    }
}
