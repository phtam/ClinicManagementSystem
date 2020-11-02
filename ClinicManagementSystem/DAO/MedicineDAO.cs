using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class MedicineDAO
    {
        ClinicSystemData db = new ClinicSystemData();

        public List<Medicine> GetAll()
        {
            return db.Medicines.ToList();
        }

        public List<Medicine> Show()
        {
            return db.Medicines.OrderByDescending(m=>m.MedicineID).Take(6).ToList();
        }

        public Medicine Get(int? id)
        {
            if (id == null)
                return null;
            else
            {
                return db.Medicines.Find(id);
            }
        }

        public bool CheckMedicineUnitInStock(int? id, int quantity)
        {
            var medicine = db.Medicines.Find(id);
            if (medicine.UnitInStock < quantity)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<MedicineImage> GetImages(int? id)
        {
            if (id == null)
                return null;
            else
                return db.MedicineImages.Where(m=>m.MedicineID == id).ToList();
        }

        public List<MedicineFeedback> GetFeedbacks(int? id)
        {
            if (id == null)
                return null;
            else
                return db.MedicineFeedbacks.Where(m => m.MedicineID == id).OrderByDescending(m=>m.FeedbackID).ToList();
        }

        public List<Medicine> SortByType(int? typeId)
        {
            if (typeId == null)
            {
                return null;
            }
            var list = db.Medicines.Where(p => p.TypeID == typeId);
            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<Medicine> SortBySupplier(int? supId)
        {
            if (supId == null)
            {
                return null;
            }
            var list = db.Medicines.Where(p => p.SupplierID == supId);
            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<Medicine> GetNewMedicines(int typeId)
        {
            var list = db.Medicines.Where(p => p.TypeID == typeId);
            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }

        public int CountProductByCategoryInStock(int cateID)
        {
            int num = 0;
            foreach (var item in db.Medicines.Where(pro => pro.TypeID == cateID))
            {
                num += item.UnitInStock.GetValueOrDefault(0);
            }
            return num;
        }

        public int CountProductByCategoryOnOrder(int cateID)
        {
            int num = 0;
            foreach (var item in db.Medicines.Where(pro => pro.TypeID == cateID))
            {
                num += item.UnitOnOrder.GetValueOrDefault(0);
            }
            return num;
        }
    }
}