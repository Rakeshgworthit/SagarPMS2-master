﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class BankViewModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter bank name.")]
        public string bank_name { get; set; }
        [Required(ErrorMessage = "Please enter account number.")]
        public string account_number { get; set; }
        [Required(ErrorMessage = "Please enter branch code.")]
        public string branch_code { get; set; }
        public string payment_instructions { get; set; }
        public string additional_remarks { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string zip_code { get; set; }
        [EmailAddress]
        public string email { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string contact_person { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please select a branch")]
        public Nullable<Int32> branch_id { get; set; }

        public List<SelectListItem> BranchList { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(bank_name)}={bank_name}, {nameof(account_number)}={account_number}, {nameof(branch_code)}={branch_code}, {nameof(payment_instructions)}={payment_instructions}, {nameof(additional_remarks)}={additional_remarks}, {nameof(address1)}={address1}, {nameof(address2)}={address2}, {nameof(address3)}={address3}, {nameof(address4)}={address4}, {nameof(zip_code)}={zip_code}, {nameof(email)}={email}, {nameof(website)}={website}, {nameof(phone)}={phone}, {nameof(mobile)}={mobile}, {nameof(contact_person)}={contact_person}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(branch_id)}={branch_id.ToString()}, {nameof(BranchList)}={BranchList}}}";
        }
    }
}