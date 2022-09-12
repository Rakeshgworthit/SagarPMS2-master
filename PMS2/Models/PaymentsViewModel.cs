using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PMS.Models
{

    //public class Paymentview
    //{
    //    public string id { get; set; }
    //    public string payment_date { get; set; }
    //    public string supplier_id { get; set; }
    //    public string bank_id { get; set; }
    //    public string cheque_number { get; set; }
    //    public string rebate_amount { get; set; }
    //    public string remarks { get; set; }
    //    public string userid { get; set; }
    //    public string payment_mode { get; set; }
    //    public string isactive { get; set; }
    //    public string collection_date { get; set; }
    //    public string Message { get; set; }
    //}

    public class Payment
    {
        public long id { get; set; }
        public string project_name { get; set; }
        public string supplier_names { get; set; }
        public string status_id { get; set; }
        public decimal cheque_number { get; set; }
        public string CreatedUpdated { get; set; }
        public string remarks { get; set; }
        public string status_name { get; set; }
        public string payment_date { get; set; }

    }

    public class Paymentview
    {
        public string UserID { get; set; }
        public int branchId { get; set; }
        public int salesMenId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SearchString { get; set; }

        public Int64 ProjectId { get; set; }
    }
    public class PaymentChkExistingViewModel
    {
        public long id { get; set; }
        //  public long project_id { get; set; }
        public long supplier_id { get; set; }
        public List<payment_details_chk_Existing> payment_details_chk_Existing { get; set; }
        public static PaymentChkExistingViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);
            PaymentChkExistingViewModel obj = new PaymentChkExistingViewModel();
            obj.payment_details_chk_Existing = new List<payment_details_chk_Existing>();
            obj.id = (int)paymentobj["id"];
            obj.supplier_id = (int)paymentobj["supplier_id"];
            //  obj.project_id = (int)paymentobj["project_id"];
            List<payment_details_chk_Existing> paymentdetails = new List<payment_details_chk_Existing>();
            foreach (JObject pd in paymentobj["payment_details_chk_Existing"])
            {
                payment_details_chk_Existing d = new payment_details_chk_Existing();
                pd["project_id"] = pd["project_id"].HasValues ? pd["project_id"] : "0";
                d.project_id = Convert.ToInt32(pd["project_id"]);
                d.supplier_inv_number = Convert.ToString(pd["supplier_inv_number"]);
                paymentdetails.Add(d);
            }
            obj.payment_details_chk_Existing.AddRange(paymentdetails);
            return obj;
        }

        public override string ToString()
        {
            return $"{{{nameof(id)}={id.ToString()}, {nameof(supplier_id)}={supplier_id.ToString()}, {nameof(payment_details_chk_Existing)}={payment_details_chk_Existing}}}";
        }
    }

    public class PaymentsViewModel
    {
        public string Message { get; set; }
        public string UID { get; set; }
        public string paymentdet_project_id { get; set; }
        public string paymentdet_GstPercentage { get; set; }
        public long id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Enter payment date.")]
        public Nullable<System.DateTime> payment_date { get; set; }

        //[Required(ErrorMessage = "Enter collection date.")]
        public Nullable<System.DateTime> collection_date { get; set; }

        [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select supplier.")]
        [Required(ErrorMessage = "Please select supplier.")]
        public long supplier_id { get; set; }


        public string supplier_inv_number { get; set; }
        public string supplier_inv_number_text { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> invoice_amount { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> gst_amount { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> payment_amount { get; set; }
        public decimal agreed_amount { get; set; }

        //[Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select bank.")]
        //[Required(ErrorMessage = "Please select bank.")]
        public int bank_id { get; set; }

        //[Required(ErrorMessage = "Please enter cheque number.")]
        public string cheque_number { get; set; }

        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> rebate_amount { get; set; }
        public Nullable<decimal> actual_payment_amount { get; set; }
        public Nullable<decimal> total_payment_amount { get; set; }
        public Nullable<decimal> total_invoice_amount_after_gst { get; set; }
        public string remarks { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> created_by { get; set; }
        public System.DateTime modified_date { get; set; }
        public Nullable<System.Guid> modified_by { get; set; }
        public bool isactive { get; set; }
        public List<SelectListItem> projectList { get; set; }
        public List<SelectListItem> GSTList { get; set; }
        public List<SelectListItem> supplierList { get; set; }
        public List<SelectListItem> bankList { get; set; }
        public List<SelectListItem> IsActiveList { get; set; }
        public List<SelectListItem> mode_of_paymentList { get; set; }
        public List<payment_details> payment_details { get; set; }
        public Int32 payment_mode { get; set; }

        public string SearchString { get; set; }

        public List<SelectListItem> SalesmenList { get; set; }
        public Int32 ProjectSalesmenId { get; set; }

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

        public string description { get; set; }
        [RegularExpression("((\\d+)((\\.\\d{1,2})?))$", ErrorMessage = "Please enter valid amount")]
        public Nullable<decimal> amount { get; set; }
        public List<payment_details_descriptions> payment_descriptions { get; set; }
        public static PaymentsViewModel FromJson(string val)
        {
            JObject paymentobj = JObject.Parse(val);

            PaymentsViewModel obj = new PaymentsViewModel();
            obj.payment_details = new List<payment_details>();
            obj.payment_descriptions = new List<payment_details_descriptions>();
            obj.id = (int)paymentobj["id"];
            string paymentdate = Convert.ToString(paymentobj["payment_date"]);
            obj.payment_date = Convert.ToDateTime(paymentdate);
            string collectiondate = Convert.ToString(paymentobj["collection_date"]);
            if (!String.IsNullOrEmpty(collectiondate))
            {
                obj.collection_date = Convert.ToDateTime(collectiondate);
            }
            else
            {
                obj.collection_date = null;
            }

            obj.supplier_id = (int)paymentobj["supplier_id"];
            obj.cheque_number = (string)paymentobj["cheque_number"];
            obj.isactive = (bool)paymentobj["isactive"];
            // obj.project_id = (int)paymentobj["project_id"];
            obj.bank_id = (int)paymentobj["bank_id"];
            obj.rebate_amount = Convert.ToDecimal(paymentobj["rebate_amount"]);
            obj.actual_payment_amount = Convert.ToDecimal(paymentobj["actual_payment_amount"]);
            obj.total_payment_amount = Convert.ToDecimal(paymentobj["total_payment_amount"]);
            obj.total_invoice_amount_after_gst = Convert.ToDecimal(paymentobj["total_invoice_amount_after_gst"]);
            obj.remarks = (string)paymentobj["remarks"];
            obj.payment_mode = (int)paymentobj["payment_mode"];
            obj.Message = (string)paymentobj["Message"];
            List<payment_details> paymentdetails = new List<payment_details>();
            List<payment_details_descriptions> paymentdescription = new List<payment_details_descriptions>();
            if (paymentobj["payment_description"] != null)
            {
                foreach (JObject desc in paymentobj["payment_description"])
                {
                    if (desc["description"].ToString() != "" && desc["amount"].ToString() != "")
                    {
                        payment_details_descriptions pdd = new payment_details_descriptions();
                        pdd.description = Convert.ToString(desc["description"]);
                        pdd.amount = Convert.ToDecimal(desc["amount"]);
                        if (desc["descriptionID"].ToString() != "")
                        {
                            pdd.descriptionID = Convert.ToInt32(desc["descriptionID"]);
                        }

                        paymentdescription.Add(pdd);
                    }

                }
                obj.payment_descriptions.AddRange(paymentdescription);
            }

            foreach (JObject pd in paymentobj["payment_details"])
            {
                //if (Convert.ToString(pd["supplier_inv_number"]) != "")
                //{
                payment_details d = new payment_details();
                d.project_id = Convert.ToInt32(pd["project_id"]);
                d.supplier_inv_number = Convert.ToString(pd["supplier_inv_number"]);
                d.supplier_inv_number_text = Convert.ToString(pd["supplier_inv_number_text"]);
                d.InvRemarks = Convert.ToString(pd["InvRemarks"]);
                if (Convert.ToString(pd["invoice_amount"]) == "")
                {
                    d.invoice_amount = 0;
                }
                else
                {
                    d.invoice_amount = Convert.ToDecimal(pd["invoice_amount"]);
                }
                if (Convert.ToString(pd["gst_percentage"]) == "")
                {
                    d.gst_percentage = 0;
                }
                else
                {
                    d.gst_percentage = Convert.ToDecimal(pd["gst_percentage"]);
                }
                if (Convert.ToString(pd["gst_amount"]) == "")
                {
                    d.gst_amount = 0;
                }
                else
                {
                    d.gst_amount = Convert.ToDecimal(pd["gst_amount"]);
                }
                if (Convert.ToString(pd["payment_amount"]) == "")
                {
                    d.payment_amount = 0;
                }
                else
                {
                    d.payment_amount = Convert.ToDecimal(pd["payment_amount"]);
                }
                if (Convert.ToString(pd["agreed_amount"]) == "")
                {
                    d.agreed_amount = 0;
                }
                else
                {
                    d.agreed_amount = Convert.ToDecimal(pd["agreed_amount"]);
                }



                paymentdetails.Add(d);
                //}
            }
            obj.payment_details.AddRange(paymentdetails);



            return obj;
        }

        public override string ToString()
        {
            return $"{{{nameof(UID)}={UID}, {nameof(paymentdet_project_id)}={paymentdet_project_id}, {nameof(id)}={id.ToString()}, {nameof(payment_date)}={payment_date.ToString()}, {nameof(collection_date)}={collection_date.ToString()}, {nameof(supplier_id)}={supplier_id.ToString()}, {nameof(supplier_inv_number)}={supplier_inv_number}, {nameof(supplier_inv_number_text)}={supplier_inv_number_text}, {nameof(invoice_amount)}={invoice_amount.ToString()}, {nameof(gst_amount)}={gst_amount.ToString()}, {nameof(payment_amount)}={payment_amount.ToString()}, {nameof(agreed_amount)}={agreed_amount.ToString()}, {nameof(bank_id)}={bank_id.ToString()}, {nameof(cheque_number)}={cheque_number}, {nameof(rebate_amount)}={rebate_amount.ToString()} , {nameof(actual_payment_amount)}={actual_payment_amount.ToString()}, {nameof(total_payment_amount)}={total_payment_amount.ToString()}, {nameof(total_invoice_amount_after_gst)}={total_invoice_amount_after_gst.ToString()}, {nameof(remarks)}={remarks}, {nameof(created_date)}={created_date.ToString()}, {nameof(created_by)}={created_by.ToString()}, {nameof(modified_date)}={modified_date.ToString()}, {nameof(modified_by)}={modified_by.ToString()}, {nameof(isactive)}={isactive.ToString()}, {nameof(projectList)}={projectList}, {nameof(supplierList)}={supplierList}, {nameof(bankList)}={bankList}, {nameof(IsActiveList)}={IsActiveList}, {nameof(mode_of_paymentList)}={mode_of_paymentList}, {nameof(payment_details)}={payment_details}, {nameof(payment_mode)}={payment_mode.ToString()}, {nameof(SearchString)}={SearchString}, {nameof(SalesmenList)}={SalesmenList}, {nameof(ProjectSalesmenId)}={ProjectSalesmenId.ToString()}, {nameof(SearchFrom)}={SearchFrom.ToString()}, {nameof(SearchTo)}={SearchTo.ToString()}, {nameof(SearchProject)}={SearchProject.ToString()}, {nameof(isProjectClosed)}={isProjectClosed.ToString()}, {nameof(description)}={description}, {nameof(amount)}={amount.ToString()}, {nameof(payment_descriptions)}={payment_descriptions}}}";
        }
    }

    public class payment_details
    {
        // [Range(minimum: 1, maximum: 9999999, ErrorMessage = "Select select project.")]
        //[Required(ErrorMessage = "Please select project.")]
        public long project_id { get; set; }
        public decimal invoice_amount { get; set; }
        public decimal gst_percentage { get; set; }
        public decimal gst_amount { get; set; }
        public decimal payment_amount { get; set; }
        public string supplier_inv_number { get; set; }
        public string supplier_inv_number_text { get; set; }
        public decimal budgeted_cost { get; set; }

        public string supplier_inv_list { get; set; }
        public decimal total_payment_amount { get; set; }
        public decimal total_invoice_amount_after_gst { get; set; }
        public decimal agreed_amount { get; set; }
        public string InvRemarks { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(project_id)}={project_id.ToString()}, {nameof(invoice_amount)}={invoice_amount.ToString()}, {nameof(agreed_amount)}={agreed_amount.ToString()}, {nameof(gst_percentage)}={gst_percentage.ToString()}, {nameof(gst_amount)}={gst_amount.ToString()}, {nameof(payment_amount)}={payment_amount.ToString()}, {nameof(total_payment_amount)}={total_payment_amount.ToString()}, {nameof(total_invoice_amount_after_gst)}={total_invoice_amount_after_gst.ToString()}, {nameof(supplier_inv_number)}={supplier_inv_number}, {nameof(budgeted_cost)}={budgeted_cost.ToString()}, {nameof(supplier_inv_list)}={supplier_inv_list}}}";
        }
    }
    public class payment_details_chk_Existing
    {
        public string supplier_inv_number { get; set; }
        public long project_id { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(supplier_inv_number)}={supplier_inv_number}, {nameof(project_id)}={project_id.ToString()}}}";
        }
    }
    public class payment_details_descriptions
    {
        public string description { get; set; }
        public decimal amount { get; set; }
        public int descriptionID { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(description)}={description}, {nameof(amount)}={amount.ToString()}, {nameof(descriptionID)}={descriptionID.ToString()}}}";
        }
    }

    public class Proc_GetPaymentDetail_Result
    {
        public long id { get; set; }
        public int payment_id { get; set; }
        public Nullable<decimal> invoice_amount { get; set; }
        public Nullable<decimal> gst_percentage { get; set; }
        public Nullable<decimal> gst_amount { get; set; }
        public Nullable<decimal> payment_amount { get; set; }
        public string supplier_inv_number { get; set; }
        public Nullable<long> project_id { get; set; }
        public Nullable<decimal> budget_amount { get; set; }
        public Nullable<decimal> agreed_amount { get; set; }
        public string message { get; set; }
    }
}