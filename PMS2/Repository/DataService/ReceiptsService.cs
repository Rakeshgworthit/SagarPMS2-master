﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ReceiptsService : GenericRepository<PMSEntities, receipt>, IReceiptsRepositor
    {
        public List<SSP_Receipts_Result> SearchReceipts(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId, string SearchString)
        {
            List<SSP_Receipts_Result> objList = new List<SSP_Receipts_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Receipts(new Guid(userId), branchid, FromDate, ToDate, ProjectId, StartIndex, PageSize, SortBy, OrderBy, ProjectSalesmenId, SearchString).ToList();
            }
            return objList;
        }
    }
}