using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ProjectAdditionService : GenericRepository<PMSEntities, additions_omissions>, IProjectAdditionRepository
    {
        public List<SSP_Project_additions_Result> GetMyProjects(string userId, Int32 BranchId, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate, Int32 ProjectId, Int32 ProjectSalesmenId, string SearchStringAdditions)
        {
            List<SSP_Project_additions_Result> objList = new List<SSP_Project_additions_Result>();
            using (PMSEntities objDB = new PMSEntities())
            {
                objList = objDB.SSP_Project_additions(new Guid(userId), BranchId, FromDate, ToDate,ProjectId, StartIndex, PageSize, SortBy, OrderBy, ProjectSalesmenId, SearchStringAdditions).ToList();
            }
            return objList;
        }
    }    
}