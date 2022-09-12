using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;

namespace PMS.Interface
{
    public interface IDashboard
    {
        List<Top10Sites> GetTop10Sites();
        List<Top10SitesOwing> GetTop10SitesOwing();
        List<Top10SitesCustOwing> GetTop10SitesCustOwing();
        List<Top10OpenProjects> GetTop10OpenProjects();
        List<Top10Salesman> GetTop10Salesman();
        List<Top10SalesmanOwing> GetTop10SalesmanOwing();
        Dashboard GetDetailsBasedonYear(int Year);
        Dashboard GetDetailsBasedonBranch(int Branch);
        List<ProjectsSummaryReport> GetProjectsSummaryReport();
    }
}
