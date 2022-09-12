using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class SupplierViewModel
    {
        public long id { get; set; }
        [Required(ErrorMessage = "Please enter supplier name.")]
        public string supplier_name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        //[Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string contact_person { get; set; }
        public string nric_no { get; set; }
        public Nullable<bool> gst_registered { get; set; }
        public string gst_no { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> GSTRegisteredList { get; set; }

        public string Suppliersearch { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(supplier_name)}={supplier_name}, {nameof(address1)}={address1}, {nameof(address2)}={address2}, {nameof(address3)}={address3}, {nameof(address4)}={address4}, {nameof(zip_code)}={zip_code}, {nameof(email)}={email}, {nameof(website)}={website}, {nameof(phone)}={phone}, {nameof(mobile)}={mobile}, {nameof(contact_person)}={contact_person}, {nameof(nric_no)}={nric_no}, {nameof(gst_registered)}={gst_registered.ToString()}, {nameof(gst_no)}={gst_no}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(StatusList)}={StatusList}, {nameof(GSTRegisteredList)}={GSTRegisteredList}}}";
        }
    }
}
