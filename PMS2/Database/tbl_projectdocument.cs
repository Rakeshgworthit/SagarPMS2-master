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
    
    public partial class tbl_projectdocument
    {
        public long Id { get; set; }
        public long project_id { get; set; }
        public string file_name { get; set; }
        public System.Guid uploaded_by { get; set; }
        public System.DateTime uploaded_on { get; set; }
        public string file_desc { get; set; }
    }
}
