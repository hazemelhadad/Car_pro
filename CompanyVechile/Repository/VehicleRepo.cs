using CompanyVechile.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyVechile.Repository
{
    public class VehicleRepo
    {
        CompanyDBContext db=new CompanyDBContext();

        public List<Vehicle> getAllVehicles()
        {
            return db.Vehicle.ToList();
        }
        public Vehicle getVehicleById(int vehiclePlateNum)
        {
            return db.Vehicle.FirstOrDefault(x => x.Vehicle_PlateNumber == vehiclePlateNum);
        }
        public void insert(Vehicle vehicle)
        {
            db.Vehicle.Add(vehicle);
            db.SaveChanges();

        }
        public void update(Vehicle vehicle, int vehiclePlateNum) 
        {
            var oldOne = getVehicleById( vehiclePlateNum);
            oldOne.Vehicle_PlateNumber = vehicle.Vehicle_PlateNumber;
            db.SaveChanges();


        }
        public void delete(int vehiclePlateNum) 
        {
            var oldOne = getVehicleById(vehiclePlateNum);
            db.Vehicle.Remove(oldOne);
            db.SaveChanges();
        }

    }
}
