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
    
    public partial class projects_budget
    {
        public long id { get; set; }
        public Nullable<long> project_id { get; set; }
        public Nullable<long> branch_id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public Nullable<bool> isactive { get; set; }
    }
}
