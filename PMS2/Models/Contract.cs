using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class Contract
    {
        public string task_description { get; set; }
        public string item_description { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class ProjectContractTermsCriteria
    {
        public int? contract_term_id { get; set; }
        public string contract_desrcription { get; set; }
        public int master_contract_term_id { get; set; }
        public string userId { get; set; }
    }
    public class ProjectPaymentTermsCriteria
    {
        public int? payment_term_id { get; set; }
        public string description { get; set; }
        public int master_payment_termid { get; set; }
        public string userId { get; set; }
    }

    public class Signature
    {
        public string Customer { get; set; }
        public string Salesmen { get; set; }
        public int SalesmenId { get; set; }
        public string contract_number { get; set; }
        public DateTime contract_date { get; set; }
        public Int64 ContractYear { get; set; }
        //public string FileName { get; set; }
        public string DOCUMENT_NAME { get; set; }
        public string FILE_TYPE { get; set; }
        //public string DOCUMENT_PATH { get; set; }
        public string SalesmanimageData { get; set; }
        public string CustomerimageData { get; set; }

        public string CustomerFileName { get; set; }
        public string SalesmenFileName { get; set; }

        public string CustomerImage_PATH { get; set; }
        public string SalesmenImage_PATH { get; set; }
        public string project_id { get; set; }
        //public string contentType { get; set; }

        //public string base64 { get; set; }
        public string ID_TYPE { get; set; }
        public string ID { get; set; }
        public string SuperId { get; set; }
        public string CustomerId { get; set; }
        public string VODate { get; set; }
        public string CustomerName { get; set; }
        public string SalesmenName { get; set; }
        public string CustomerAddress { get; set; }
        public string ProjectId { get; set; }
        public string ContractDate { get; set; }
        public string Internal_No { get; set; }
    }

    public class contractSupplierMapping
    {
        public string project_number { get; set; }
        public string customer { get; set; }
        public string project_name { get; set; }
        public string salesmen { get; set; }
        public SupplierDropDown Supplier { get; set; }
        public List<ContractSuppliersMappingData> _contractSupplierMapping { get; set; }
    }

    public class ContractSuppliersMappingData
    {
        public string supplierID { get; set; }
        public string suppliername { get; set; }
        public string suppaddress { get; set; }
    }

}