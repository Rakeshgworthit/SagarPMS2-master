using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace PMS.Models
{
    public class SalesmenViewModel
    {
   
        public long id { get; set; }
        [Required(ErrorMessage = "Please select branch")]
        public long branch_Id { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string salesmen_name { get; set; }
        public string project_number { get; set; }
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "Please enter valid value")]
        public Nullable<decimal> saleman_commission { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }

        [Required(ErrorMessage = "Please enter Email Address")]
        [EmailAddress(ErrorMessage = "Enter valid e-mail address")]
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
   
        public Guid User_Id { get; set; }

        public List<SelectListItem> BranchList { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(branch_Id)}={branch_Id.ToString()}, {nameof(salesmen_name)}={salesmen_name}, {nameof(saleman_commission)}={saleman_commission.ToString()}, {nameof(address1)}={address1}, {nameof(address2)}={address2}, {nameof(address3)}={address3}, {nameof(address4)}={address4}, {nameof(zip_code)}={zip_code}, {nameof(email)}={email}, {nameof(website)}={website}, {nameof(phone)}={phone}, {nameof(mobile)}={mobile}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(BranchList)}={BranchList}}}";
        }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,})", ErrorMessage = "Password must contains 6 or more characters. Passwords must have at least one number, one lowercase ('a'-'z'), one uppercase ('A'-'Z') and one special character(@#$%).")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        // [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.@!]{6,15}$", ErrorMessage = "Enter alphanumeric user name between 6-15 characters in length")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter ICNumber")]
        [StringLength(10)]
        [RegularExpression("^[a-zA-Z0-9_.@!]{0,10}$", ErrorMessage = "Enter alphanumeric ICNumber not exceeding 10 characters")]
        [Display(Name = "ICNumber")]
        public string ICNumber { get; set; }

        public Boolean IsCreateLogin { get; set; }
        public string SalesmenSearch { get; set; }
    }
}