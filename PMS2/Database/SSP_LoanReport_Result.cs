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
    
    public partial class SSP_LoanReport_Result
    {
        public System.DateTime LoanDate { get; set; }
        public string purpose { get; set; }
        public decimal amount { get; set; }
        public int rec_type { get; set; }
        public decimal TotalBalance { get; set; }
        public string supplier_name { get; set; }
        public string mode_of_payment { get; set; }
        public string bank_name { get; set; }
        public int cheque_number { get; set; }
    }
}
