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
    
    public partial class discount
    {
        public long id { get; set; }
        public System.DateTime date { get; set; }
        public long project_id { get; set; }
        public Nullable<int> record_type { get; set; }
        public string addition_omissioni_description { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<decimal> gst_percentage { get; set; }
        public Nullable<decimal> gst_amount { get; set; }
        public Nullable<decimal> total_amount { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public System.DateTime modified_date { get; set; }
        public System.Guid created_by { get; set; }
        public System.Guid modified_by { get; set; }
        public bool isactive { get; set; }
    }
}