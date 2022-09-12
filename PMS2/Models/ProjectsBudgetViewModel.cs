using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace PMS.Models
{

    public class ProjectsBudget
    {
        public long id { get; set; }
        public Int64 project_budget_details_id { get; set; }
        public Int64 project_budget_id { get; set; }
        public Int64 project_id { get; set; }
        public AddressDropDown Address { get; set; }
        public string project_name { get; set; }
        public string supplier_names { get; set; }
        public string status_id { get; set; }
        public decimal budget_amount { get; set; }
        public string CreatedUpdated { get; set; }
        public decimal Approved_amount { get; set; }
        public string status_name { get; set; }
        public string remarks { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmtWithGST { get; set; }
        public SelectListItem GSTPercent { get; set; }
        public decimal GSTAmount { get; set; }
        public Int64 branch_Id { get; set; }

        public Int64 supplier_id { get; set; }
        public Int64 SalesMenId { get; set; }
        public string salesman_name { get; set; }
        public SalesmanDropDown Salesman { get; set; }
        public SupplierDropDown supplier { get; set; }

        public string salesmen_name { get; set; }
        public string reason { get; set; }
        public BudgetCostTypeDropDown BudgetCostType { get; set; }
    }

    public class getBudgetedInvoice_Result
    {
        public string remarks { get; set; }
        public long inv { get; set; }
        public Nullable<long> project_id { get; set; }
        public Nullable<decimal> budget_amount { get; set; }
        public string invoiceNumber { get; set; }
        public string PaymentAmount { get; set; }
        public decimal InvoiceAmtWithGST { get; set; }
        public decimal ApprovedAmount { get; set; }
    }

    public class BudgetCostDetails
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public Nullable<long> project_id { get; set; }
        public Nullable<decimal> budget_amount { get; set; }
        public string invoiceNumber { get; set; }
        public string document_path { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal InvoiceAmtWithGST { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal GSTPercent { get; set; }
        public decimal GSTAmount { get; set; }
        public string InvRemarks { get; set; }
    }
    public class AddBudget
    {
        public int project_budget_details_id { get; set; }
        public string project_budget_id { get; set; }
        public SupplierDropDown supplier { get; set; }

        public AddressDropDown Address { get; set; }
        public SalesmanDropDown Salesman { get; set; }
        //public int supplier_id { get; set; }
        public decimal budget_amount { get; set; }
        public string remarks { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmtWithGST { get; set; }
        public SelectListItem GSTPercent { get; set; }
        public decimal GSTAmount { get; set; }
        public int? StatusId { get; set; }
        //public int SalesMenId { get; set; }
        //public Int64 Project_Id { get; set; }
        public Int64 Branch_Id { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public string SalesMenId { get; set; }
        public string SupplierId { get; set; }
        public string Project_Id { get; set; }
        public Nullable<int> Zip_CodeId { get; set; }
        public string Unit_CodeId { get; set; }
        public BudgetCostTypeDropDown BudgetCostType { get; set; }
    }

    public class ProjectsBudgetView
    {
        public string UserID { get; set; }
        public int branchId { get; set; }
        // public string ProjectId { get; set; }
        public int SupplierId { get; set; }

        //public string SearchString { get; set; }
        public int SalesmenId { get; set; }
        public int project_budget_id { get; set; }
        public int Project_Id { get; set; }

        public int ZipCodeId { get; set; }

        public string UnitCode { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime endDate { get; set; }

        public string StatusId { get; set; }

    }

    public class ProjectsBudgetViewModel
    {


        public string UID { get; set; }
        public string projectbudgetdetailsupplierid { get; set; }

        public long id { get; set; }
        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Please select project name")]
        public Nullable<long> project_id { get; set; }
        // [Range(minimum: 0, maximum: 9999999, ErrorMessage = "Please select branch.")]
        public Nullable<long> branch_id { get; set; }
        public Nullable<System.DateTime> created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public Nullable<bool> isactive { get; set; }

        public Nullable<int> Zip_CodeId { get; set; }

        public string Unit_CodeId { get; set; }

        [Required(ErrorMessage = "Enter from date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchFrom { get; set; }
        [Required(ErrorMessage = "Enter to date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? SearchTo { get; set; }

        public List<SelectListItem> supplierList { get; set; }

        public List<SelectListItem> StatusList { get; set; }

        public List<projects_budget_details> projects_budget_details { get; set; }

        public List<SelectListItem> projectList { get; set; }

        public Int32 SearchProject { get; set; }

        public string SearchString { get; set; }
        public string SalesmenName { get; set; }
        public string AddressSitName { get; set; }

        public Int32 supplier_id { get; set; }

        public Int32 Status_Id { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

        public Int32 ProjectSalesmen { get; set; }
        public List<SelectListItem> SalesmenList { get; set; }

        public List<SelectListItem> ZipCodeList { get; set; }
        public List<SelectListItem> UnitCodeList { get; set; }
        public List<SelectListItem> AddressList { get; set; }

        public string BudgetSearch { get; set; }
        public static ProjectsBudgetViewModel FromJson(string val)
        {
            JObject PBobj = JObject.Parse(val);
            ProjectsBudgetViewModel obj = new ProjectsBudgetViewModel();
            obj.projects_budget_details = new List<projects_budget_details>();

            obj.id = (int)PBobj["id"];
            obj.project_id = (int)PBobj["project_id"];


            List<projects_budget_details> projectsbudgetdetail = new List<projects_budget_details>();
            foreach (JObject pbd in PBobj["projects_budget_details"])
            {
                //if (Convert.ToString(pd["supplier_inv_number"]) != "")
                //{
                projects_budget_details d = new projects_budget_details();
                d.project_budget_id = Convert.ToInt32(PBobj["project_id"]);
                d.supplier_id = Convert.ToInt32(pbd["supplier_id"]);

                if (Convert.ToString(pbd["budget_amount"]) == "")
                {
                    d.budget_amount = 0;
                }
                else
                {
                    d.budget_amount = Convert.ToDecimal(pbd["budget_amount"]);
                }

                d.remarks = Convert.ToString(pbd["remarks"]);

                d.id = Convert.ToInt64(pbd["project_budget_details_id"]);



                projectsbudgetdetail.Add(d);
                //}
            }
            obj.projects_budget_details.AddRange(projectsbudgetdetail);

            return obj;
        }

        public override string ToString()
        {
            return $"{{{nameof(UID)}={UID}, {nameof(projectbudgetdetailsupplierid)}={projectbudgetdetailsupplierid}, {nameof(id)}={id.ToString()}, {nameof(project_id)}={project_id.ToString()}, {nameof(branch_id)}={branch_id.ToString()}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(supplierList)}={supplierList}, {nameof(projects_budget_details)}={projects_budget_details}, {nameof(projectList)}={projectList}, {nameof(SearchProject)}={SearchProject.ToString()}, {nameof(SearchString)}={SearchString}, {nameof(supplier_id)}={supplier_id.ToString()}, {nameof(ProjectSalesmenId)}={ProjectSalesmenId.ToString()}, {nameof(SalesmenList)}={SalesmenList}, {nameof(BudgetSearch)}={BudgetSearch}}}";
        }
    }

    public class projects_budget_details
    {
        public long id { get; set; }
        public long project_budget_id { get; set; }
        public long supplier_id { get; set; }
        public decimal budget_amount { get; set; }
        public string remarks { get; set; }

        public long salesmen_id { get; set; }

        public long ProjectId { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(project_budget_id)}={project_budget_id.ToString()}, {nameof(supplier_id)}={supplier_id.ToString()}, {nameof(budget_amount)}={budget_amount.ToString()}, {nameof(remarks)}={remarks}}}";
        }

        public string InvoiceNumber { get; set; }
        public Nullable<decimal> InvoiceAmountWithGST { get; set; }
        public Nullable<decimal> GSTAmount { get; set; }
        public Nullable<decimal> Approved_Amount { get; set; }
        public Nullable<decimal> GSTPercent { get; set; }

        public string supplier_name { get; set; }
    }
}