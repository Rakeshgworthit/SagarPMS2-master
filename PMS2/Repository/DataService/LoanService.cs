﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class LoanService : GenericRepository<PMSEntities, loan>, ILoanRepositor 
    {
        public List<SSP_Loan_Result> SearchLoans(string userId,Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate)
        {
            List<SSP_Loan_Result> objList = new List<SSP_Loan_Result>();
            //using (PMSEntities objDB = new PMSEntities())
            //{
            //    objList = objDB.SSP_Loan(new Guid(userId), branchid, FromDate, ToDate, StartIndex, PageSize, SortBy, OrderBy).ToList();
            //}
            objList = Common.CommonFunction.GetLoan_Results(new Guid(userId), branchid, FromDate, ToDate, StartIndex, PageSize, SortBy, OrderBy);
            return objList;
        }
    }
}