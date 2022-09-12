using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PMS.Models
{
    public class ReceiptsViewModel
    {
        public string UID { get; set; }

        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter receipt date.")]
        public Nullable<System.DateTime> receipt_date { get; set; }
        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select project name")]
        public Nullable<long> project_id { get; set; }
        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select mode of payment")]
        public Nullable<int> mode_of_payment_id { get; set; }
        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select a bank")]
        public Nullable<int> bank_id { get; set; }

        public string cheque_details { get; set; }


        //[Range(minimum: 1, maximum:int.MaxValue, ErrorMessage = "The amount must be greater than 0")]

        [RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> amount { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid value")]
        [Required(ErrorMessage = "Please enter gst percentage")]
        public Nullable<decimal> gst_percentage { get; set; }

        //[RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [RegularExpression("^-?[0-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter gst amount")]
        public Nullable<decimal> gst_amount { get; set; }

        //[RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        [RegularExpression("^-?[1-9]\\d*(\\.\\d+)?$", ErrorMessage = "Please enter valid amount")]
        [Required(ErrorMessage = "Please enter total amount")]
        public Nullable<decimal> total_amount { get; set; }


        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select salesman.")]
        public Nullable<long> salesman_id { get; set; }

        [Range(minimum: 0, maximum: 9999999, ErrorMessage = "Please select branch.")]
        public Nullable<long> branch_id { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }

        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> branchList { get; set; }
        public List<SelectListItem> projectList { get; set; }
        public List<SelectListItem> GSTList { get; set; }
        public List<SelectListItem> salesmanList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public string SearchString { get; set; }

        public Int32 brId { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }
        public Int32 SearchProject { get; set; }
        public bool isProjectClosed { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(UID)}={UID}, {nameof(id)}={id.ToString()}, {nameof(receipt_date)}={receipt_date.ToString()}, {nameof(project_id)}={project_id.ToString()}, {nameof(mode_of_payment_id)}={mode_of_payment_id.ToString()}, {nameof(bank_id)}={bank_id.ToString()}, {nameof(cheque_details)}={cheque_details}, {nameof(amount)}={amount.ToString()}, {nameof(gst_percentage)}={gst_percentage.ToString()}, {nameof(gst_amount)}={gst_amount.ToString()}, {nameof(total_amount)}={total_amount.ToString()}, {nameof(salesman_id)}={salesman_id.ToString()}, {nameof(branch_id)}={branch_id.ToString()}, {nameof(remarks)}={remarks}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(bankList)}={bankList}, {nameof(branchList)}={branchList}, {nameof(projectList)}={projectList}, {nameof(salesmanList)}={salesmanList}, {nameof(mode_of_paymentList)}={mode_of_paymentList}, {nameof(IsActiveList)}={IsActiveList}, {nameof(SearchString)}={SearchString}, {nameof(brId)}={brId.ToString()}, {nameof(SearchFrom)}={SearchFrom.ToString()}, {nameof(SearchTo)}={SearchTo.ToString()}, {nameof(SearchProject)}={SearchProject.ToString()}, {nameof(isProjectClosed)}={isProjectClosed.ToString()}, {nameof(ProjectSalesmenId)}={ProjectSalesmenId.ToString()}}}";
        }
    }

}