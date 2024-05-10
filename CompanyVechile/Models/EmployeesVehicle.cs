using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.Models
{
    public class EmployeesVehicle
    {
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employees Employee { get; set; }

        [ForeignKey("Vehicle")]
        public string VehiclePlateNumber { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
