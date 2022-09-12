using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;


namespace PMS.Interface
{
    public interface IPayments
    {
        List<Payment> PaymentList(Paymentview _Paymentview);
        List<getBudgetedInvoice_Result> Get_budget_Invoice(int projectId, int supplierId);
        List<Proc_GetPaymentDetail_Result> GetPaymentDetail_Result(int paymentid, int Supplier_id);
        //SuccessMessage UpsertPayments(Paymentview _UpsertPayments);
    }
}
