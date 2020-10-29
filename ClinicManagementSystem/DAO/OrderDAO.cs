using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class OrderDAO
    {
        private ClinicSystemData db = new ClinicSystemData();

        public int Create(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.OrderID;
        }

        public bool AddMedicine(MedicineOrderDetail medicineOrder)
        {
            db.MedicineOrderDetails.Add(medicineOrder);
            if (db.SaveChanges() > 0)
                return true;
            else 
                return false;
        }

        public bool AddApparatus (ScientificApparatusOrderDetail scientificApparatusOrder)
        {
            db.ScientificApparatusOrderDetails.Add(scientificApparatusOrder);
            if (db.SaveChanges() > 0)
                return true;
            else 
                return false;
        }

        public bool UpdateMedicineUnitOnOrder(int medicineId, int quantity)
        {
            var medicine = db.Medicines.Find(medicineId);
            medicine.UnitOnOrder += quantity;
            if (db.SaveChanges() > 0)
                return true;
            else
                return false;
        }

        public bool UpdateApparatusUnitOnOrder(int apparatusId, int quantity)
        {
            var apparatu = db.ScientificApparatus.Find(apparatusId);
            apparatu.UnitOnOrder += quantity;
            if (db.SaveChanges() > 0)
                return true;
            else
                return false;
        }

    }
}