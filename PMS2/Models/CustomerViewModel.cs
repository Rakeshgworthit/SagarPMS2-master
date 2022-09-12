using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace PMS.Models
{
    public class CustomerViewModel
    {
        public long id { get; set; }
        [Required(ErrorMessage = "Enter name")]
        public string name1 { get; set; }
        public string name2 { get; set; }
        //[Required(ErrorMessage = "Enter NRIC1")]
        public string nric1 { get; set; }
        public string nric2 { get; set; }
        //[Required(ErrorMessage = "Enter block number")]
        public string block_no { get; set; }
        //[Required(ErrorMessage = "Enter job site")]
        public string job_site { get; set; }
        public string address { get; set; }
        //[Required(ErrorMessage = "Enter Contact Person")]
        public string contact_person { get; set; }
        //[Required(ErrorMessage = "Enter email address")]
        //[EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string phone { get; set; }
        [Required(ErrorMessage = "Enter Mobile number")]
        public string mobile { get; set; }
        public Nullable<bool> gst_registered { get; set; }
        public int SourceOfEnquiry { get; set; }
        public string gst_no { get; set; }
        //[Required(ErrorMessage = "Enter Unit Code")]
        public string unit_code { get; set; }
        //[Required(ErrorMessage = "Enter Zip Code")]
        public Nullable<int> zip_code { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> StatusList { get; set; }

        public List<SelectListItem> BranchList { get; set; }

        public List<SelectListItem> CountryList { get; set; }
        public int BranchId { get; set; }
        public long CountryId { get; set; }
        public List<SelectListItem> GSTRegisterList { get; set; }

        public List<SelectListItem> SourceList { get; set; }

        public string customersearch { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(name1)}={name1}, {nameof(name2)}={name2}, {nameof(nric1)}={nric1}, {nameof(nric2)}={nric2}, {nameof(block_no)}={block_no}, {nameof(job_site)}={job_site}, {nameof(address)}={address}, {nameof(contact_person)}={contact_person}, {nameof(email)}={email}, {nameof(phone)}={phone}, {nameof(mobile)}={mobile}, {nameof(gst_registered)}={gst_registered.ToString()}, {nameof(gst_no)}={gst_no}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(StatusList)}={StatusList}, {nameof(GSTRegisterList)}={GSTRegisterList}, {nameof(customersearch)}={customersearch},{nameof(SourceOfEnquiry)}={SourceOfEnquiry}}}";
        }
    }
}