using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class PaymentsService : GenericRepository<PMSEntities, payment>, IPaymentsRepositor
    {

        public List<SSP_Payments_Result> SearchPayments(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId, string SearchString)
        {
            List<SSP_Payments_Result> objList = new List<SSP_Payments_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Payments(new Guid(userId), branchid, FromDate, ToDate, ProjectId, StartIndex, PageSize, SortBy, OrderBy, ProjectSalesmenId, SearchString).ToList();
            }
            return objList;
        }

        public SSP_PaymentById_Result PaymentById(int id)
        {
            SSP_PaymentById_Result objList = new SSP_PaymentById_Result();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_PaymentById(id).SingleOrDefault();
            }
            return objList;
        }

        public List<SSP_PaymentsDescription_Result> PaymentsDescription(int id)
        {
            List<SSP_PaymentsDescription_Result> objList = new List<SSP_PaymentsDescription_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_PaymentsDescription(id).ToList();
            }
            return objList;
        }

    }
}