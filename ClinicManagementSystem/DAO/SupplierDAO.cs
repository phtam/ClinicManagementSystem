using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class SupplierDAO
    {
        private ClinicSystemData db = new ClinicSystemData();

        public List<Supplier> GetAll()
        {
            return db.Suppliers.ToList();
        }

        public Supplier Get(int? id)
        {
            return db.Suppliers.Find(id);
        }

    }
}