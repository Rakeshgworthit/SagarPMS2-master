﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Repository.Infrastructure;
using PMS.Database;

namespace PMS.Repository.DataService
{
    public class ProjectsBudgetDetailsService : GenericRepository<PMSEntities, projects_budget_detail>, IProjectsBudgetDetailRepository
    {
        
    }
}