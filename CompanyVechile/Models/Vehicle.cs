using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.Models
{
    public class Vehicle
    {
        [Key]
        public int Vehicle_PlateNumber { get; set; }
        public string License_SerialNumber { get; set; } 
        public string License_Registeration { get; set; }

        [DataType(DataType.Date)]
        public DateTime License_ExpirationDate { get; set; }
        public string Vehicle_PlateNum { get; set; }
        public string Vehicle_ChassisNum { get; set; }
        public int Vehicle_ManufactureYear { get; set; }
        public string Vehicle_BrandName { get; set; }
        public string Vehicle_Color { get; set; }
        public string Vehicle_Type { get; set; }

        [ForeignKey("Branches")]
        public int? Branch_ID { get; set; }
        public Branches Branches { get; set; }  //Not sure List or single object
        public  ICollection<Employees> Employees { get; set; }   //Many to Many table

    }
}
