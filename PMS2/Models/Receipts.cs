using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class Receipts
    {
        public string UserID { get; set; }
        //public int branchId
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Int64 ProjectId { get; set; }
        public Int64 salesMenId { get; set; }
        public string SearchString { get; set; }
        //,@startRowIndex=1,@pageSize=20,@ColSort='receipt_date',@OrderBy='Dsc'
    }

    public class ReceiptsList
    {
        public int id { get; set; }
        public DateTime receipt_date { get; set; }
        public AddressDropDown project_name { get; set; }
        
        //public string project_name { get; set; }
        public ModeOfPaymentDropDown mode_of_payment { get; set; }
        //public string mode_of_payment { get; set; }
        public Banks Banks { get; set; }
        //public string bank_name { get; set; }
        public string cheque_details { get; set; }
        public string Remarks { get; set; }
        public string CreatedUpdated { get; set; }
        public int TotalRecords { get; set; }
        public Int64 ProjectId { get; set; }
        public int Bank_ID { get; set; }
        public string remarks { get; set; }
        public decimal Amount { get; set; }
        public Int64 salesmen_id { get; set; }
        public Int64 branch_id { get; set; }
        public decimal Gst_Percentage { get; set; }
        public decimal Gst_Amount { get; set; }
        public decimal Total_Amount { get; set; }
        public int mode_of_payment_id { get; set; }
        

    }
}