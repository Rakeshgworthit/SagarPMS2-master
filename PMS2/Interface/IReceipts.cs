using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;

namespace PMS.Interface
{
    public interface IReceipts
    {
        //List<ReceiptsList> QuotationsList(Receipts _Receipts);
        List<ReceiptsList> ReceiptsList(Receipts _Receipts);
        SuccessMessage UpsertCustomerPayments(ReceiptsList _UpsertCustomerPaymentsCriteria, string uid);
        SuccessMessage Delete_Customer_Payments(string Id);
    }
}
