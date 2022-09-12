﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace PMS.Models
{
    public class BranchViewModel
    {
       


        public long id { get; set; }
        [Required(ErrorMessage = "Please select company")]
        public long company_id { get; set; }

        [Required(ErrorMessage = "Enter branch name")]
        public string branch_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string contact_person { get; set; }
        [Required(ErrorMessage = "Please select country")]
        public Nullable<long> country_id { get; set; }
        public string company_reg_no { get; set; }
        public string gst_reg_no { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> CountryList { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(company_id)}={company_id.ToString()}, {nameof(branch_name)}={branch_name}, {nameof(address1)}={address1}, {nameof(address2)}={address2}, {nameof(address3)}={address3}, {nameof(address4)}={address4}, {nameof(zip_code)}={zip_code}, {nameof(email)}={email}, {nameof(website)}={website}, {nameof(phone)}={phone}, {nameof(mobile)}={mobile}, {nameof(contact_person)}={contact_person}, {nameof(country_id)}={country_id.ToString()}, {nameof(company_reg_no)}={company_reg_no}, {nameof(gst_reg_no)}={gst_reg_no}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(CompanyList)}={CompanyList}, {nameof(CountryList)}={CountryList}}}";
        }
    }
}