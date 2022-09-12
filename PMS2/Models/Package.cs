using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class PackageList
    {
        public string package_id { get; set; }
        public string package_cd { get; set; }
        public PackageDropdown package { get; set; }
        public PlanDropdown plan { get; set; }
        public FloorDropdown floor { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime valid_to { get; set; }
        public Decimal total_amount { get; set; }
        public bool isactive { get; set; }
        public string userid { get; set; }
        public string ExistingPackageId { get; set; }
        public string ExistingProjectId { get; set; }
        public bool isGlobalpkg { get; set; }
    }

    public class PackageTasksItem
    {
        public TaskDropDown Task { get; set; }
        public string PackageTask_id { get; set; }
    }

    public class PackageTasksItemList
    {
        public string Package_Id { get; set; }

        public string Task_Id { get; set; }

        public string Task_Name { get; set; }
        //public TaskDropDown Task { get; set; }
        public string Package_Det_Id { get; set; }
        public CategoryDropDown Category { get; set; }
        public ItemDropDown Item { get; set; }
        public StatusLookup BillingUOM { get; set; }
        public Decimal Price { get; set; }
        //public Int32? Qty { get; set; }
        public Decimal? Qty { get; set; }
        public UOMDropDown UOM { get; set; }
        public string item_remarks { get; set; }
        public decimal Amount { get; set; }

        public string userid { get; set; }
    }

    //public class UpsertPackage
    //{
    //    public string package_id { get; set; }
    //    public string package_cd { get; set; }
    //    public PackageDropdown package_name { get; set; }
    //    public PlanDropdown plan_name { get; set; }
    //    public FloorDropdown floor_name { get; set; }
    //    public DateTime valid_from { get; set; }
    //    public DateTime valid_to { get; set; }
    //    public Decimal total_amount { get; set; }
    //    public bool isactive { get; set; }
    //    public string userid { get; set; }
    //}

    public class PackageDetail
    {
        public string package_id { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime valid_to { get; set; }

        public Int32 plan_id { get; set; }
        public Int32 floor_id { get; set; }
        public Int32 package_type_id { get; set; }

        public bool isactive { get; set; }
        public Decimal gst_percentage { get; set; }
        public Decimal gst_amount { get; set; }
        public Decimal amount { get; set; }
        public Decimal discount_amount { get; set; }
        public string package_cd { get; set; }
        public string package_name { get; set; }
        public string plan_name { get; set; }
        public string floor_name { get; set; }
        public string payment_term_id { get; set; }
        public string payment_description { get; set; }
        public Decimal total_amount { get; set; }
        public Decimal minamount { get; set; }
        public Decimal maxamount { get; set; }
        public string search { get; set; }

        public DateTime created_date { get; set; }
        public string created_by { get; set; }
        public DateTime modified_date { get; set; }
        public string modified_by { get; set; }
        public bool isGlobalpkg { get; set; }
    }



    public class UpsertPackageCriteria
    {
        public string package_id { get; set; }
        public string package_cd { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime valid_to { get; set; }
        public Decimal discount_amount { get; set; }
        public Int32 floor_id { get; set; }
        public bool isactive { get; set; }
        public string userid { get; set; }
        public Int32 plan_id { get; set; }
        public Int32 package_type_id { get; set; }

    }
    public class UpsertPackageDetailsCriteria
    {
        public string package_det_id { get; set; }
        public string package_id { get; set; }
        public string item_description { get; set; }
        public string task_id { get; set; }
        public int item_id { get; set; }
        public int category_name { get; set; }
        public int category_id { get; set; }
        public int price_type_id { get; set; }
        public int uom_id { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
        public decimal amount { get; set; }
        public string Userid { get; set; }
        public string item_remarks { get; set; }
        public string task_description { get; set; }
    }
    public class PackageTasksCriteria
    {
        public string Package_Id { get; set; }
        public string Task_Id { get; set; }

    }
    public class PackageTasksList
    {
        public string package_id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string TaskAmount { get; set; }
    }

    public class PackageAmountList
    {
        public string Package_Id { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal discount_amount { get; set; }
        public decimal Subtotal { get; set; }
        public decimal gst_amount { get; set; }
        public decimal GrandTotal { get; set; }

    }
    public class PaymentTerms
    {
        public string payment_term_id { get; set; }
        public string payment_description { get; set; }
        public PaymentDescription paymentdescription { get; set; }

        //public string package_id { get; set; }
    }

    public class PaymentDescription
    {
        public Int32? Master_payment_term_id { get; set; }
        public string Master_payment_description { get; set; }
    }
    public class NewPackageDetailTasksItem
    {
        public string package_id { get; set; }
        public string package_cd { get; set; }
        public PackageDropdown package { get; set; }
        public PlanDropdown plan { get; set; }
        public FloorDropdown floor { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime valid_to { get; set; }
        public Decimal total_amount { get; set; }
        public bool isactive { get; set; }

        public string Task_Id { get; set; }

        public string Task_Name { get; set; }
        public string Package_Det_Id { get; set; }
        public CategoryDropDown Category { get; set; }
        public ItemDropDown Item { get; set; }
        public StatusLookup BillingUOM { get; set; }
        public Decimal Price { get; set; }
        public Int32 Qty { get; set; }
        public UOMDropDown UOM { get; set; }
        public string item_remarks { get; set; }
        public decimal Amount { get; set; }

        public string userid { get; set; }
    }
}