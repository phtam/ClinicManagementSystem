using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClinicManagementSystem.EF;

namespace ClinicManagementSystem.DAO
{
    public class ScientificApparatusTypeDAO
    {
        ClinicSystemData db = new ClinicSystemData();

        public List<ScientificApparatusType> GetAll()
        {
            return db.ScientificApparatusTypes.ToList();
        }

        public ScientificApparatusType Get(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.ScientificApparatusTypes.FirstOrDefault(s=>s.TypeID == id);
            }
            
        }
    }
}