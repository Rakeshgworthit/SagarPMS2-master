using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public partial class PrintPaymentViewModel
    {

        public List<a> all { get; set; }

        public List<b> allll { get; set; }


        public List<Database.payment_detail> payment_details { get; set; }
        public Database.SSP_PaymentById_Result ssp_paymentById_result { get; set; }
        public List<Database.payment_details_description> payment_details_description { get; set; }
        public List<Database.SSP_PaymentsDescription_Result> SSP_PaymentsDescription_Result { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(payment_details)}={payment_details}, {nameof(ssp_paymentById_result)}={ssp_paymentById_result}, {nameof(payment_details_description)}={payment_details_description}, {nameof(SSP_PaymentsDescription_Result)}={SSP_PaymentsDescription_Result}}}";
        }


    }

    public partial class PrintBatchPaymentViewModel
    {



        public List<PrintPaymentViewModel> printpaymentviewmodel { get; set; }

        public List<payment_details_descriptions> payment_descriptions { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(printpaymentviewmodel)}={printpaymentviewmodel}, {nameof(payment_descriptions)}={payment_descriptions}}}";
        }
    }

    public partial class a
    {
        public string no { get; set; }
        public string no2 { get; set; }
    }

    public partial class b
    {
        public string No1 { get; set; }
    }

}