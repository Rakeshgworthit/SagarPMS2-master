using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Database;
namespace PMS.Repository.Infrastructure
{
    public interface IProjectsBudgetRepository : IGenericRepository<projects_budget>
    {
        List<SSP_Projects_budget_Result> SearchProjectsBudget(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, Int32 ProjectId, Int32 SupplierId, string SearchString, int SalesmenId);

    }
}