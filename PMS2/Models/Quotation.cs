using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class Quotation
    {
        public string UserID { get; set; }
        public int branchId { get; set; }
        public int startRowIndex { get; set; }
        public int pageSize { get; set; }
        public int ColSort { get; set; }
        public string OrderBy { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string projectStatus { get; set; }
        public Int64 salesMenId { get; set; }
        public string searchText { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public int projectStatusid { get; set; }
        public string statusgroup { get; set; }
        public string isactive { get; set; }

        public Int32 id { get; set; }

        public string salesmen_name { get; set; }

        public string role { get; set; }

    }
    public class QuotationListCriteria
    {
        // public string UserID { get; set; }
        public int branchId { get; set; }
        //public int startRowIndex { get; set; }
        //public int pageSize { get; set; }
        public int ColSort { get; set; }
        public string OrderBy { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public int? projectStatus { get; set; }
        public Int64? salesMenId { get; set; }
        public string searchText { get; set; }
        public int Type { get; set; }

    }
    public class QuotationList
    {
        public string project_id { get; set; }
        //public string id { get; set; }
        public string project_number { get; set; }
        public string project_name { get; set; }
        public DateTime contract_date { get; set; }
        public CustomerDropDown Customer { get; set; }

        public SalesmanDropDown Salesmen { get; set; }
        //public string salesmen_name { get; set; }

        public decimal contract_amount { get; set; }
        public string CreatedUpdated { get; set; }
        //public string branch_Id { get; set; }
        ////public Int64? Customer_id { get; set; }        
        //public string email { get; set; }
        //public string address { get; set; }
        public StatusLookup Status { get; set; }
        public decimal total_amount { get; set; }
        public long Id { get; set; }
    }

    public class CustomerDetailsById
    {
        public Int64? Customer_id { get; set; }
        public string name1 { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public Int32 Source_id { get; set; }
        public string Source_Name { get; set; }
        public string JobSite { get; set; }
    }

    public class Salesmancommission
    {
        public Int64? salesmen_id { get; set; }
        public decimal saleman_commission { get; set; }
    }
    public class CreateQuotationCriteria
    {
        public string project_id { get; set; }
        public string project_number { get; set; }
        public string ReferenceNo { get; set; }
        public string project_name { get; set; }
        public Int64? salesmen_id { get; set; }
        //public Int64 branch_id { get; set; }
        public Int64? Customer_id { get; set; }
        //public int bank_id { get; set; }
        public int status_id { get; set; }
        public DateTime? quotationForwardDate { get; set; }
        //public DateTime? quotationAcceptDate { get; set; }
        public bool is_new_record { get; set; }
        public string reason { get; set; }
        public string remarks { get; set; }
        public int Package_Type_Id { get; set; }
        public int plan_id { get; set; }
        public int floor_id { get; set; }
        public string userId { get; set; }
        public decimal saleman_commission { get; set; }
        public decimal contract_amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal discount { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal total_amount { get; set; }
        //public string name1 { get; set; }
        public string salesmen_name { get; set; }
        //public DateTime? contract_date { get; set; }
        public DateTime contract_date { get; set; }
        public DateTime project_start_date { get; set; }
        public CustomerDropDown Customer { get; set; }

        public SalesmanDropDown Salesmen { get; set; }

        public string ExistingProjectId { get; set; }
        public int version_no { get; set; }
        public int bank_id { get; set; }
    }

    public class QuotationFromPackageCriteria
    {
        public Int64 SalesMenid { get; set; }
        public string PackageId { get; set; }
        public string userId { get; set; }
        public string Project_number { get; set; }
        public string project_name { get; set; }
        public Int64 branch_id { get; set; }
        public Int64 customer_id { get; set; }
        public int bank_id { get; set; }
        public DateTime quotationForwardDate { get; set; }
        public decimal saleman_commission { get; set; }
        public string reason { get; set; }
        
    }
    public class QuotationUpsertProjectDetails
    {
        public string project_id { get; set; }
        public string project_det_Id { get; set; }
        public string Task_Id { get; set; }
        public string Task_Name { get; set; }

        // public int price_type_id { get; set; }

        public CategoryDropDown Category { get; set; }
        public ItemDropDown Item { get; set; }
        public StatusLookup BillingUOM { get; set; }
        //public int item_id { get; set; }

        public UOMDropDown UOM { get; set; }
        public string item_remarks { get; set; }
        //public string item_description { get; set; }
        //public int category_id { get; set; }
        //public int? uom_id { get; set; }
        public decimal price { get; set; }
        //public int? qty { get; set; }
        public decimal? qty { get; set; }
        public decimal? amount { get; set; }
        public decimal? Cost_Amount { get; set; }
        public decimal? Profit_Loss { get; set; }
        public string userId { get; set; }
        //public string category_name { get; set; }
        //public string uom_description { get; set; }
        //public string description { get; set; }
        //public int status_lookup_id { get; set; }
        public string Highlight { get; set; }

        public string AdditionalDescription { get; set; }


    }

    public class ProjectTasksCriteria
    {
        public string project_Id { get; set; }
        public string Task_Id { get; set; }

    }
    public class ProjectTasksList
    {
        public string project_id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string TaskAmount { get; set; }
    }
    public class ProjectAmountList
    {
        public string project_number { get; set; }
        public string salesmen_name { get; set; }
        public string CUstomerName { get; set; }
        public string email { get; set; }
        public string AddressSite { get; set; }
        public string project_status { get; set; }
        public DateTime quotationForwardDate { get; set; }
        public DateTime contract_date { get; set; }
        public string ActualAmount { get; set; }
        public string discount { get; set; }
        public string discount_percentage { get; set; }
        public string Subtotal { get; set; }
        public string gst_amount { get; set; }
        public string GrandTOtal { get; set; }
        //project_number salesmen_name   CUstomerName email   AddressSite project_status  quotationForwardDate
        //contract_date   ActualAmount discount    Subtotal gst_amount  GrandTOtal
    }
    public class ProjectTasksItemList
    {
        public string project_Id { get; set; }
        public string Task_Id { get; set; }
        public string Task_Name { get; set; }
        public string Project_Det_Id { get; set; }
        public CategoryDropDown Category { get; set; }

        public ItemDropDown Item { get; set; }

        public StatusLookup BillingUOM { get; set; }
        //public int category_Id { get; set; }
        //public string category_name { get; set; }

        //public int item_id { get; set; }
        //public string item_description { get; set; }
        //public string description { get; set; }
        public Decimal Price { get; set; }
        public string Qty { get; set; }
        public UOMDropDown UOM { get; set; }
        //public string uom_description { get; set; }
        public string item_remarks { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost_Amount { get; set; }
        public decimal Profit_Loss { get; set; }

        public string Userid { get; set; }
        //public int? uom_id { get; set; }
        //public int status_lookup_id { get; set; }
        //public string ProjectTaskid { get; set; }
        //public string task_id { get; set; }
        public bool Highlight { get; set; }
        public string AdditionalDescription { get; set; }
        public int record_type { get; set; }
        public string Vo_Id { get; set; }

    }

    public class ProjectTasksItem
    {
        public TaskDropDown Task { get; set; }
        public string PackageTask_id { get; set; }
    }
    public class GetDocumentdetails
    {
        public string document_path { get; set; }
    }
    public class QuotationDetails
    {
        public string project_id { get; set; }
        public string project_number { get; set; }
        public string project_name { get; set; }
        public Int64 salesmen_id { get; set; }
        public Int64 branch_id { get; set; }
        public Int64 customer_id { get; set; }
        public int bank_id { get; set; }
        public int status_id { get; set; }
        public DateTime quotationForwardDate { get; set; }
        public DateTime quotationAcceptDate { get; set; }
        public bool is_new_record { get; set; }
        public string reason { get; set; }
        public string remarks { get; set; }
        public int Package_Type_Id { get; set; }
        public int plan_id { get; set; }
        public int floor_id { get; set; }
        public string userId { get; set; }
        public decimal saleman_commission { get; set; }
        public decimal contract_amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal amount_before_discount { get; set; }
        public string id { get; set; }
        public DateTime contract_date { get; set; }
        public DateTime project_start_date { get; set; }
        public DateTime project_end_date { get; set; }
        public decimal amount { get; set; }
        public string Email { get; set; }
        public string status { get; set; }
        public string customer { get; set; }
        public string salesmen { get; set; }

        public string document_path { get; set; }

        public string Customer_document_path { get; set; }
        public string package_id { get; set; }
        public string NRIC { get; set; }
        public string Phone { get; set; }
        public string Jobsite { get; set; }
        public string shortForwardDate { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }

        public string doc_id { get; set; }
        public string id_type { get; set; }
        public string doc_path { get; set; }

        public string EvoNo { get; set; }
        public string Evo_id { get; set; }
        public DateTime? EvoDate { get; set; }
        public DateTime? Evocreated_date { get; set; }
        public string Evocreated_by { get; set; }
        public DateTime? Evomodified_date { get; set; }
        public string Evomodified_by { get; set; }
        public decimal EvoAmount { get; set; }

        public decimal totalamount { get; set; }
        public decimal gstamount { get; set; }
        public decimal grandtotal { get; set; }
        public int PropertyType_id { get; set; }
        public int EvoStatus_Id { get; set; }
        public string EvoStatus { get; set; }

        public EVOTotalCriteria EVOTotalCriteria { get; set; } 

    }

    public class QuotationFromPackageResponse
    {
        public string result { get; set; }
        public string projectId { get; set; }
        public string Errormessage { get; set; }
    }
    public class QuotationStatusCriteria
    {
        public string project_id { get; set; }
        public string Status_Id { get; set; }
        public string Reason { get; set; }
        public string QuoteDate { get; set; }
        public string Customer_id { get; set; }
        public string CustomerName { get; set; }
        public string SalesmenName { get; set; }
        public string CustomerAddress { get; set; }
        public string QuoteNumber { get; set; }
        
    }
    public class VOStatusCriteria
    {
        
        public string project_id { get; set; }
        public string VO_Id { get; set; }
        public string Status_Id { get; set; }
    }

    public class EVOCriteria
    {

        public string Qty { get; set; }
        public string Id { get; set; }
        public string ElectricalMapping { get; set; }
        public string Price { get; set; }
        public string IsSelected { get; set; }
        // public List<EVOCriteriaList> EVOCriteriaList { get; set; }
    }

    public class EVOCriteriaList
    {
        public string Qty { get; set; }
        public string Id { get; set; }
        public string ElectricalMapping { get; set; }
        public string Price { get; set; }
        public string IsSelected { get; set; }
    }

    public class EVOEmailSendCriteria
    {
        public string contract_number { get; set; }
        public string contract_date { get; set; }
        public string ContractYear { get; set; }
        public string VODate { get; set; }
        public string CustomerName { get; set; }
        public string SalesmenName { get; set; }
        public string CustomerAddress { get; set; }
        public string Internal_No { get; set; }
        public string ProjectId { get; set; }
        public string VO_ID { get; set; }
    }

    public class EVOTotalCriteria
    {

        public decimal TotalSelectedItemsAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}