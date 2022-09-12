﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class LoanViewModel
    {
        public bool Verified { get; set; }

        public string UID { get; set; }

        public int Id { get; set; }
        public Nullable<long> branch_Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter loan date.")]
        public System.DateTime LoanDate { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please select type.")]
        public int rec_type { get; set; }

        [Required(ErrorMessage = "Please select person name.")]
        public string person_id { get; set; }
        public string person_type { get; set; }
        public string purpose { get; set; }

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Please select payment mode.")]
        public int payment_mode { get; set; }
        public Nullable<int> bank_id { get; set; }
        public Nullable<int> cheque_number { get; set; }


        [RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount.")]
        public decimal amount { get; set; }
        public System.Guid created_by { get; set; }
        public System.DateTime created_on { get; set; }
        public System.Guid updated_by { get; set; }
        public System.DateTime updated_on { get; set; }
        public bool isactive { get; set; }
        public Int32 project_id { get; set; }
        public List<SelectListItem> projectList { get; set; }

        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> branchList{ get; set; }
        
        public List<SelectListItem> SalesmenAndUserList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<SelectListItem> RecordTypeList { get; set; }
        public Int32 brId { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(UID)}={UID}, {nameof(Id)}={Id.ToString()}, {nameof(branch_Id)}={branch_Id.ToString()}, {nameof(LoanDate)}={LoanDate.ToString()}, {nameof(rec_type)}={rec_type.ToString()}, {nameof(person_id)}={person_id}, {nameof(person_type)}={person_type}, {nameof(purpose)}={purpose}, {nameof(payment_mode)}={payment_mode.ToString()}, {nameof(bank_id)}={bank_id.ToString()}, {nameof(cheque_number)}={cheque_number.ToString()}, {nameof(amount)}={amount.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(created_on)}={created_on.ToString()}, {nameof(updated_by)}={updated_by.ToString()}, {nameof(updated_on)}={updated_on.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(project_id)}={project_id.ToString()}, {nameof(projectList)}={projectList}, {nameof(bankList)}={bankList}, {nameof(branchList)}={branchList}, {nameof(SalesmenAndUserList)}={SalesmenAndUserList}, {nameof(mode_of_paymentList)}={mode_of_paymentList}, {nameof(IsActiveList)}={IsActiveList}, {nameof(RecordTypeList)}={RecordTypeList}, {nameof(brId)}={brId.ToString()}, {nameof(SearchFrom)}={SearchFrom.ToString()}, {nameof(SearchTo)}={SearchTo.ToString()}}}";
        }
    }
}