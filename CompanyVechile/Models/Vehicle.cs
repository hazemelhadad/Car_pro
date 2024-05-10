using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.Models
{
    public class Vehicle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Vehicle_PlateNumber { get; set; }
        public string License_SerialNumber { get; set; } 
        public string License_Registeration { get; set; }
        public string License_ExpirationDate { get; set; }
        public string Vehicle_ChassisNum { get; set; }
        public int Vehicle_ManufactureYear { get; set; }
        public string Vehicle_BrandName { get; set; }
        public string Vehicle_Color { get; set; }
        public string Vehicle_Type { get; set; }
        public string Vehicle_Insurance { get; set; }

        [ForeignKey("Branch")]
        public int? Branch_ID { get; set; }
        public Branches Branch { get; set; }
        public ICollection<EmployeesVehicle> EmployeesVehicles { get; set; }

    }
}
