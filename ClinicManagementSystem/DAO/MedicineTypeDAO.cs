using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClinicManagementSystem.EF;

namespace ClinicManagementSystem.DAO
{
    public class MedicineTypeDAO
    {
        ClinicSystemData db = new ClinicSystemData();

        public List<MedicineType> GetAll()
        {
            return db.MedicineTypes.ToList();
        }

        
        public MedicineType Get(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.MedicineTypes.FirstOrDefault(s => s.TypeID == id);
            }

        }
    }
}