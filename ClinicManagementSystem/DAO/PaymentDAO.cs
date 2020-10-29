using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.DAO
{
    public class PaymentDAO
    {
        private ClinicSystemData db = new ClinicSystemData();
        public List<Payment> GetAll()
        {
            return db.Payments.ToList();
        }
    }
}