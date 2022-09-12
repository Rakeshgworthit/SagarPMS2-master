using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ProjectsBudgetService : GenericRepository<PMSEntities, projects_budget>, IProjectsBudgetRepository
    {
        public List<SSP_Projects_budget_Result> SearchProjectsBudget(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, Int32 ProjectId, Int32 SupplierId, string SearchString, Int32 SalesmenId)
        {
            List<SSP_Projects_budget_Result> objList = new List<SSP_Projects_budget_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Projects_budget(new Guid(userId), branchid, ProjectId, StartIndex, PageSize, SortBy, OrderBy, SupplierId, SearchString, SalesmenId).ToList();
            }
            return objList;
        }
    }
}