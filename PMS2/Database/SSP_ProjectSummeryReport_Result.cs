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
    
    public partial class SSP_ProjectSummeryReport_Result
    {
        public long ID { get; set; }
        public string project_name { get; set; }
        public Nullable<decimal> contract_value { get; set; }
        public string receipt_date { get; set; }
        public Nullable<int> contract_date { get; set; }
        public decimal progress_claim { get; set; }
        public Nullable<decimal> costing_amt { get; set; }
        public Nullable<decimal> budgeted_cost { get; set; }
        public string remarks { get; set; }
        public string salesmen_name { get; set; }
        public Nullable<decimal> pc_bc { get; set; }
    }
}
