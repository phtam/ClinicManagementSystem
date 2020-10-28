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

        public Medicine Get(int? id)
        {
            if (id == null)
                return null;
            else
            {
                return db.Medicines.Find(id);
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
                return db.MedicineFeedbacks.Where(m => m.MedicineID == id).ToList();
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
    }
}