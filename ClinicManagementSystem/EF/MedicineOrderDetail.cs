//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicManagementSystem.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class MedicineOrderDetail
    {
        public int OrderID { get; set; }
        public int MedicineID { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual Medicine Medicine { get; set; }
        public virtual Order Order { get; set; }
    }
}
