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

        //New Edits
        public float? Vehicle_Price { get; set; } = 0.0f;
        public int? Vehicle_Mileage { get; set; } = 0;
        public string? Vehicle_LastRepair_Date { get; set; } = "غير معلوم";
        public float? Vehicle_LastRepair_Price { get; set; } = 0.0f;
        public string? Vehicle_LastAccident_Date { get; set; } = "لم يحدث أي حوادث";
        public string? Vehicle_Owner { get; set; } = "غير معلوم";

        //end of new Edits
    }
}
