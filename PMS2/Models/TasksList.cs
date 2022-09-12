using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Models
{
    public class TasksList
    {

        public string task_id { get; set; }

        //public string task_cd { get; set; }

        public string task_name { get; set; }

        //public Boolean isactive { get; set; }

        public string task_description { get; set; }

        //public string created_by { get; set; }

        //public string modified_by { get; set; }

        public string userId { get; set; }
        public int Seq_No { get; set; }
    }

    public class TaskDropDown
    {

        public string Task_Id { get; set; }

        public string Task_Name { get; set; }

    }
    public class UOM
    {
        public int? uom_id { get; set; }
        public string uom_cd { get; set; }
        public string uom_description { get; set; }
        public Boolean isactive { get; set; }
        public Boolean IsSystem { get; set; }

    }

    public class UOMDropDown
    {
        public int? uom_id { get; set; }
        public string uom_description { get; set; }

    }
    public class Category
    {
        public int? category_Id { get; set; }
        public string category_cd { get; set; }
        public string category_name { get; set; }
        public string category_description { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
        public int Seq_No { get; set; }
    }

    public class SourceOfInquiry
    {
        public int? SourceOfInquiry_Id { get; set; }
        public string SourceOfInquiry_cd { get; set; }
        public string SourceOfInquiry_name { get; set; }
        public string SourceOfInquiry_description { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
        public int Seq_No { get; set; }
    }

    public class CategoryDropDown
    {
        public int? category_Id { get; set; }
        public string category_name { get; set; }
    }
    public class VOCategoryDropDown
    {
        public int? category_Id { get; set; }
        public string category_name { get; set; }
    }

    public class ElectricalItemMapping
    {
        public int? ElectricalItemMapping_Id { get; set; }
        public int? Item_Id { get; set; }
        public int? PropertyType_Id { get; set; }
        public string Uom_Id { get; set; }
        public string Uom_Description { get; set; }
        public string BillingUom { get; set; }
        public decimal Cost_Price { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal Selling_Price { get; set; }
        public string PropertyType { get; set; }
        public string Item_description { get; set; }
        public bool IsSelected { get; set; }
        public ItemDropDown Item { get; set; }

        public string CheckedIdList { get; set; }
        public string CheckedQTYList { get; set; }
        public string CheckedAmountList { get; set; }

        public decimal totalamount { get; set; }
        public decimal gstamount { get; set; }
        public decimal grandtotal { get; set; }


    }

  
        
    public class Floor
    {
        public int? floor_id { get; set; }
        public string floor_name { get; set; }
        public string floor_description { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
    }

    public class FloorDropdown
    {
        public int? floor_id { get; set; }
        public string floor_name { get; set; }
    }
    public class Plan
    {
        public int? plan_id { get; set; }
        public string plan_cd { get; set; }
        public string plan_name { get; set; }
        public string plan_description { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
    }

    public class PlanDropdown
    {
        public int? plan_id { get; set; }
        public string plan_name { get; set; }
    }
    public class PackageType
    {
        public int? package_type_id { get; set; }
        public string package_name { get; set; }
        public string package_description { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
    }

    public class PackageDropdown
    {
        public int? package_type_id { get; set; }
        public string package_name { get; set; }
    }
    public class Item
    {
        public string item_cd { get; set; }
        public TaskDropDown Task { get; set; }
        public int? item_id { get; set; }
        public string item_description { get; set; }
        //public string task_id { get; set; }
        // public string task_name { get; set; }
        //public Int32 default_qty { get; set; }
        public decimal? default_qty { get; set; }
        public UOMDropDown UOM { get; set; }
        //public Int32 uom_id { get; set; }
        //public string uom_description { get; set; }
        public Decimal price { get; set; }
        public Boolean isactive { get; set; }
        public string userId { get; set; }
        public CategoryDropDown Category { get; set; }
    }

    public class PropertyType
    {
        public string Property_Type { get; set; }
        public string PropertyType_Code { get; set; }
        public int? PropertyType_Id { get; set; }
        public string PropertyType_Desc { get; set; }
        public decimal? Markup_Percentage { get; set; }
        public Boolean Is_Active { get; set; }
    }



    public class ItemDropDown
    {
        public int? item_id { get; set; }
        public string item_description { get; set; }
    }
    public class SelectedData
    {
        public int ElectricalItemMapping_Id { get; set; }
        public decimal Qty { get; set; }
    }
    public class MasterContractTermsCriteria
    {
        public int? master_contract_term_id { get; set; }
        public string contract_desrcription { get; set; }
        public string userId { get; set; }
    }
    public class MasterPaymentTermsCriteria
    {
        public int? master_payment_termid { get; set; }
        public string description { get; set; }
        public string userId { get; set; }
    }
    public class SuccessMessage
    {
        public string Result { get; set; }
        public string Errormessage { get; set; }
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Project_Number { get; set; }
        public decimal Amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal SubTotal { get; set; }

        public decimal addition_Amount { get; set; }
        public decimal omission_Amount { get; set; }
        public decimal TasksTotal_Amount { get; set; }

        public string Vo_Id { get; set; }

        public string Evo_Id { get; set; }
        public string Internal_Evo_Number { get; set; }
        public string evo_det_id { get; set; }
        public decimal Evo_Amount { get; set; }
        public decimal Evo_GstAmount { get; set; }
        public decimal Evo_total_Amount { get; set; }
        public decimal Evo_gstpercentage { get; set; }
        public string Evo_Date { get; set; }
        public string Evo_Status { get; set; }
        public string Property_Type { get; set; }

        public decimal TotalSelectedItemsAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GstAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string project_budget_id { get; set; }
    }
    public class StatusLookup
    {
        public int status_lookup_id { get; set; }
        public string description { get; set; }
    }

    public class Status
    {
        public int status_id { get; set; }
        public string status { get; set; }
    }

    public class Banks
    {
        public string bank_id { get; set; }
        public string bank_name { get; set; }
    }

    public class CustomerDropDown
    {
        public string name1 { get; set; }
        public Int64? Customer_id { get; set; }
    }

    public class SalesmanDropDown
    {
        public string salesmen_name { get; set; }
        public Int32 id { get; set; }
    }

    public class AddressDropDown
    {
        public Int32 status_id { get; set; }
        public string AddressSite { get; set; }
        public Int32 id { get; set; }
    }

    public class ContractTerm
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string ShortName { get; set; }

        //public Guid CreatedBy { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public Guid ModifiedBy { get; set; }
        //public DateTime ModifiedOn { get; set; }
    }

    public class AdditionalDescription
    {
        public string Id { get; set; }
        public string Description { get; set; }

    }
    public class Source
    {
        public int Source_Id { get; set; }

        public string Source_Name { get; set; }

    }
    public class ModeOfPaymentDropDown
    {
        public Int32 MOP_id { get; set; }
        public string MOP_Description { get; set; }
    }
    public class SupplierDropDown
    {
        public Int32 Supplier_id { get; set; }
        public string Supplier_Name { get; set; }
    }

    public class SupplierInvoiceDropDown
    {
        public Int32 id { get; set; }
        public string InvoiceNumber { get; set; }
    }

    public class UserEmailAddress
    {
        public string User_Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

    public class SupplierAddress
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string ZipCode { get; set; }
        
    }

    public class SalesmenAndSupplierDetails
    {
        public int Id { get; set; }
        public string SalesmenId { get; set; }
        public SupplierAddress Supplier { get; set; }
        public string DocPath { get; set; }
        public string ProjectNumber { get; set; }
    }

    public class UnitCodeDropDown
    {
        public string UnitCode_id { get; set; }
        public string UnitCode_Name { get; set; }
    }

    public class ZipCodeDropDown
    {
        public string ZipCode_id { get; set; }
        public string ZipCode_Name { get; set; }
    }

    public class SalesmenDropdown 
    {
        public string Salesmen_id { get; set; }
        public string Salesmen_Name { get; set; }
    }

    public class AddressSiteDropDown
    {
        public string AddressSite_id { get; set; }
        public string AddressSite_Name { get; set; }
    }

    public class BudgetCostTypeDropDown
    {
        public Int32 BudgetCostType_id { get; set; }
        public string BudgetCostType_type { get; set; }
    }

}