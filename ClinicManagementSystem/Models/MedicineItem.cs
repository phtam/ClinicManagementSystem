using ClinicManagementSystem.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicManagementSystem.Models
{
    [Serializable]
    public class MedicineItem
    {
        public Medicine Medicine { get; set; }
        public int Quantity { get; set; }
    }
}