using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class EducationDAO
    {
        private ClinicSystemData db = new ClinicSystemData();

        public List<Education> GetAll()
        {
            return db.Educations.ToList();
        }

        public Education Get(int? id)
        {
            if (id == null)
                return null;
            else
                return db.Educations.Find(id);
        }

        public List<Education> SortBySubject(int? id)
        {
            if (id == null)
                return null;
            else
                return db.Educations.Where(e=>e.SubjectID == id).ToList();
        }

        public List<Education> SortByActivity(int? id)
        {
            if (id == null)
                return null;
            else
                return db.Educations.Where(e => e.ActivityID == id).ToList();
        }

        public List<EducationFeedback> GetFeedbacks(int? id)
        {
            if (id == null)
                return null;
            else
                return db.EducationFeedbacks.Where(e => e.EducationID == id).ToList();
        }
    }
}