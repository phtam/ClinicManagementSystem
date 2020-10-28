using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class SubjectDAO
    {
        ClinicSystemData db = new ClinicSystemData();

        public List<Subject> GetAll()
        {
            return db.Subjects.ToList();
        }

        public Subject Get(int? id)
        {
            if (id == null)
                return null;
            else
                return db.Subjects.Find(id);
        }
    }
}