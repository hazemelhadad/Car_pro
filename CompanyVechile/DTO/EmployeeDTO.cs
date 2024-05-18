using System.Collections.Generic;

namespace CompanyVechile.DTO
{
    public class EmployeeDTO
    {
        public string Employee_ID { get; set; }
        public string Employee_Name { get; set; }
        public string Employee_Birthday { get; set; }
        public string Employee_Role { get; set; }
        public string Employee_Nationality { get; set; }
        public string Employee_Street_Name { get; set; }
        public string Employee_BuildingNumber { get; set; }
        public string Employee_City { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public int Branch_ID { get; set; }
        public ICollection<string> EmployeePhones { get; set; }     //string not int to accept 0 at the start of the number
    }
}
