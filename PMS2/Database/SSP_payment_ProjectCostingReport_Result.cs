//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PMS.Database
{
    using System;
    
    public partial class SSP_payment_ProjectCostingReport_Result
    {
        public long SupplierId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Description { get; set; }
        public string SupplierName { get; set; }
        public string InvNo { get; set; }
        public Nullable<decimal> NonGst { get; set; }
        public Nullable<decimal> Gst { get; set; }
        public Nullable<decimal> BudgetAmount { get; set; }
    }
}
