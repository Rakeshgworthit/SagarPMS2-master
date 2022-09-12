using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;



namespace PMS.Models
{
    public class ReportViewModel
    {// public List<System.Web.Mvc.SelectListItem> BranchList { get; set; }
        //public Int32 BranchID { get; set; }
        //// public string ReturnUrl { get; set; }

        public string Uid { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public List<SelectListItem> YearList { get; set; }
        public Int32 BranchId { get; set; }
        public List<SelectListItem> BranchList { get; set; }
        public Int32 SalesmenId { get; set; }
        public List<SelectListItem> SalenmenList { get; set; }
        public Int32 BankId { get; set; }
        public List<SelectListItem> BankList { get; set; }
        public Int32 ProjectId { get; set; }
        public List<SelectListItem> ProjectList { get; set; }
        public Int32 SupplierId { get; set; }
        public List<SelectListItem> SupplierList { get; set; }

        public Int32 ReportMonth { get; set; }
        public Int32 ReportYear { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime? from_date { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public DateTime? to_date { get; set; }
        public string GridData { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Enter number")]
        public Int32? CheckNoFrom { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Enter number")]
        public Int32? CheckNoTo { get; set; }

        public List<SelectListItem> AlphabetList { get; set; }
        public string Alphabet { get; set; }

        public List<SelectListItem> SalesmenAndUserList { get; set; }
        public string SalesmenOrUserId { get; set; }
        public List<SelectListItem> SalesmenStatusList { get; set; }
        public string SearchSalesmenStatus { get; set; }
        public string InvoicesNo { get; set; }
        public string ProjectStatus { get; set; }
        public List<SelectListItem> ProjectStatusList { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Uid)}={Uid}, {nameof(MonthList)}={MonthList}, {nameof(YearList)}={YearList}, {nameof(BranchId)}={BranchId.ToString()}, {nameof(BranchList)}={BranchList}, {nameof(SalesmenId)}={SalesmenId.ToString()}, {nameof(SalenmenList)}={SalenmenList}, {nameof(BankId)}={BankId.ToString()}, {nameof(BankList)}={BankList}, {nameof(ProjectId)}={ProjectId.ToString()}, {nameof(ProjectList)}={ProjectList}, {nameof(SupplierId)}={SupplierId.ToString()}, {nameof(SupplierList)}={SupplierList}, {nameof(ReportMonth)}={ReportMonth.ToString()}, {nameof(ReportYear)}={ReportYear.ToString()}, {nameof(from_date)}={from_date.ToString()}, {nameof(to_date)}={to_date.ToString()}, {nameof(GridData)}={GridData}, {nameof(CheckNoFrom)}={CheckNoFrom.ToString()}, {nameof(CheckNoTo)}={CheckNoTo.ToString()}, {nameof(AlphabetList)}={AlphabetList}, {nameof(Alphabet)}={Alphabet}, {nameof(SalesmenAndUserList)}={SalesmenAndUserList}, {nameof(SalesmenOrUserId)}={SalesmenOrUserId}, {nameof(SalesmenStatusList)}={SalesmenStatusList}, {nameof(SearchSalesmenStatus)}={SearchSalesmenStatus}, {nameof(InvoicesNo)}={InvoicesNo}, {nameof(ProjectStatus)}={ProjectStatus}, {nameof(ProjectStatusList)}={ProjectStatusList}}}";
        }
        public int? ProjectStatusId { get; set; }
        public string SearchStringmyprojects { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

        public long ProjectBudget_DetailId { get; set; }
    }
}