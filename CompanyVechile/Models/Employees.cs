using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyVechile.Models
{
    public class Employees
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_ID { get; set; }
        public string Employee_Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Employee_Birthday { get; set; }
        public string Employee_Role { get; set; }
        public string Employee_Nationality { get; set; }
        public string Employee_Street_Name { get; set; }
        public string Employee_BuildingNumber { get; set; }
        public string Employee_City { get; set;}

        [ForeignKey("Branches")]
        public int Branch_ID { get; set; }
        public Branches Branches { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }  //Many to Many table

    }
}
