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
    
    public partial class user_detail
    {
        public long did { get; set; }
        public System.Guid user_id { get; set; }
        public string Name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        public string website { get; set; }
        public string mobile { get; set; }
        public string phone { get; set; }
        public Nullable<int> country_id { get; set; }
        public Nullable<bool> is_active { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
    }
}