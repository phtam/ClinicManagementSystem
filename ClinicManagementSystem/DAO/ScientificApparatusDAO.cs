using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class ScientificApparatusDAO
    {
        ClinicSystemData db = new ClinicSystemData();
        private ScientificApparatusTypeDAO scientificApparatusTypeDAO = new ScientificApparatusTypeDAO();

        public List<ScientificApparatu> GetAll()
        {
            return db.ScientificApparatus.ToList();
        }

        public ScientificApparatu Get(int? id)
        {
            if (id == null)
                return null;
            else
            {
                return db.ScientificApparatus.Find(id);
            }
        }

        public List<ScientificApparatusImage> GetImages(int? id)
        {
            if (id == null)
                return null;
            else
                return db.ScientificApparatusImages.Where(m => m.ScientificApparatusID == id).ToList();
        }

        public List<ScientificApparatusFeedback> GetFeedbacks(int? id)
        {
            if (id == null)
                return null;
            else
                return db.ScientificApparatusFeedbacks.Where(m => m.ScientificApparatusID == id).ToList();
        }

        public List<ScientificApparatu> SortByType(int? typeId)
        {
            if (typeId == null)
            {
                return null;
            }
            var list = db.ScientificApparatus.Where(p => p.TypeID == typeId);
            if (list != null)
            {
                return list.ToList();
            }
            else
            {
                return null;
            }
        }

        public List<ScientificApparatu> GetNewScientificApparatus(int typeId)
        {
            var list = db.ScientificApparatus.Where(p => p.TypeID == typeId);
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