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
    using System.Collections.Generic;
    
    public partial class projects_budget_detail
    {
        public long id { get; set; }
        public Nullable<long> project_budget_id { get; set; }
        public Nullable<long> supplier_id { get; set; }
        public Nullable<decimal> budget_amount { get; set; }
        public string remarks { get; set; }
        public long Project_id { get; set; }
    }
}
