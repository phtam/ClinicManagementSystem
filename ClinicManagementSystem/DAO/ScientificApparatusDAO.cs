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

        public List<ScientificApparatu> Show()
        {
            return db.ScientificApparatus.OrderByDescending(s=>s.ScientificApparatusID).Take(6).ToList();
        }

        public List<ScientificApparatu> ShowTopPick()
        {
            return db.ScientificApparatus.OrderByDescending(s => s.UnitOnOrder).Take(6).ToList();
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

        public bool CheckApparatusUnitInStock(int? id, int quantity)
        {
            var apparatu = db.ScientificApparatus.Find(id);
            if (apparatu.UnitInStock < quantity)
            {
                return false;
            }
            else
            {
                return true;
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
                return db.ScientificApparatusFeedbacks.Where(m => m.ScientificApparatusID == id).OrderByDescending(m=>m.FeedbackID).ToList();
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