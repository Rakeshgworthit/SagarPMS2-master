using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class PaymentsReceived
    {
        public string totalPayment { get; set; }
        public string date_amount { get; set; }
        public string cheque_details { get; set; }
        public string mobileno { get; set; }
        public string branchAddress { get; set; }
        public string accountNo { get; set; }

        public List<PaymentsReceived> PaymentReceived { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(totalPayment)}={totalPayment}, {nameof(date_amount)}={date_amount}, {nameof(cheque_details)}={cheque_details}, {nameof(mobileno)}={mobileno}, {nameof(branchAddress)}={branchAddress}, {nameof(accountNo)}={accountNo}, {nameof(PaymentReceived)}={PaymentReceived}}}";
        }
    }
}