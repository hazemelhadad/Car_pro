using System.ComponentModel.DataAnnotations.Schema;

namespace CompanyVechile.DTO
{
    public class VehicleDTO
    {
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
        public int? Branch_ID { get; set; }
    }
}
