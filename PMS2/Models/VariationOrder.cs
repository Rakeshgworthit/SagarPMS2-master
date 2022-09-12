using System;

namespace PMS.Models
{
    public class VariationOrder
    {
        public string vo_det_id { get; set; }
        public string vo_id { get; set; }
        public CategoryDropDown Category { get; set; }
        //public TaskDropDown Task { get; set; }
        public string Task_Id { get; set; }
        public string Task_Name { get; set; }
        public ItemDropDown Item { get; set; }
        public StatusLookup BillingUOM { get; set; }//billing uom
        public UOMDropDown UOM { get; set; }
        //public UOMDropDown Uom { get; set; }
        public decimal Price { get; set; }
        public string Qty { get; set; }
        public string item_remarks { get; set; }
        public decimal Amount { get; set; }
        public int record_type { get; set; }
        public string addition_omission_description { get; set; }
        public DateTime currentdate { get; set; }

        public bool Highlight { get; set; }
        public int StatusId { get; set; }
    }

    public class VariationOrderList
    {
        public string vo_id { get; set; }
        public int is_new_record { get; set; }
        public string modified_by { get; set; }
        public string modified_date { get; set; }
        public string created_date { get; set; }
        public string createdBy { get; set; }
        public string isactive { get; set; }
        public decimal total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal gst_amount { get; set; }

        public string salesmen_name { get; set; }

        public string name1 { get; set; }

        public string addressSite { get; set; }

        public string CreatedUpdated { get; set; }

        public string project_number { get; set; }
        public decimal gst_percentage { get; set; }
        public DateTime vo_date { get; set; }
        public string addition_omission_description { get; set; }
        public string project_id { get; set; }
        public string version_no { get; set; }
        public string id { get; set; }
        public int record_type { get; set; }
        public decimal amount { get; set; }
        public string remarks { get; set; }
        public string vo_number { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        //public Int32 id { get; set; }

        //public string salesmen_name { get; set; }

        //public string role { get; set; }
        public decimal addition_Amount { get; set; }
        public decimal omission_Amount { get; set; }
        public string voDate { get; set; }
    }
    public class VariationOrderCriteria
    {
        public string project_id { get; set; }
        public string vo_id { get; set; }
        public int status_id { get; set; }
        public string remarks { get; set; }
        public string userId { get; set; }
        public decimal contract_amount { get; set; }
        public decimal amountBeforeDiscount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal discount { get; set; }
        public decimal discount_percentage { get; set; }
        public decimal total_amount { get; set; }
        public DateTime Vo_date { get; set; }
        public CustomerDropDown Customer { get; set; }

        public SalesmanDropDown Salesmen { get; set; }

        public string ExistingProjectId { get; set; }
        public int version_no { get; set; }
        public bool isactive { get; set; }

    }

    public class ElectricalVariationOrderList
    {
        public string evo_id { get; set; }
        public int is_new_record { get; set; }
        public string modified_by { get; set; }
        public string modified_date { get; set; }
        public string created_date { get; set; }
        public string createdBy { get; set; }
        public string isactive { get; set; }
        public decimal total_amount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal gst_amount { get; set; }

        public string salesmen_name { get; set; }

        public string name1 { get; set; }

        public string addressSite { get; set; }

        public string CreatedUpdated { get; set; }

        public string project_number { get; set; }
        public decimal gst_percentage { get; set; }
        public DateTime? Evo_date { get; set; }
        public string project_id { get; set; }
        public string version_no { get; set; }
        public string id { get; set; }
        public int record_type { get; set; }
        public decimal amount { get; set; }
        public string remarks { get; set; }
        public string evo_number { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }

        public DateTime? evoDate { get; set; }
    }

    public class EvoListCriteria
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

    public class VOListCriteria
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

}