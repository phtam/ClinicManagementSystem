using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class ActivityDAO
    {
        private ClinicSystemData db = new ClinicSystemData();
        public List<Activity> GetAll()
        {
            return db.Activities.ToList();
        }

        public Activity Get(int? id)
        {
            if (id == null)
                return null;
            else
                return db.Activities.Find(id);
        }
    }
}