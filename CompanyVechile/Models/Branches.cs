using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CompanyVechile.Models
{
    public class Branches
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Branch_ID { get; set; }  
        public string Branch_Name { get; set; }
        public string Branch_Location { get; set; }
        public  ICollection<Vehicle> Vehicles { get; set; }      
    }
}
