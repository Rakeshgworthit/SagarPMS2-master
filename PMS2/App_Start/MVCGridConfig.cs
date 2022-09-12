[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PMS.MVCGridConfig), "RegisterGrids")]

namespace PMS
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using Models;
    using PMS.Repository.Infrastructure;
    using PMS.Repository.DataService;
    using Microsoft.AspNet.Identity;

    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {

            MVCGridDefinitionTable.Add("Customer", new MVCGridBuilder<PMS.Database.SSP_Customer_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
                .WithPageParameterNames("customersearch","BranchId")
               .AddColumns(cols =>
               {
                   cols.Add("name1").WithHeaderText("Name1").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.name1);
                   cols.Add().WithColumnName("nric1").WithCellCssClassExpression(p => "col-sm-1").WithHeaderText("NRIC").WithSorting(true).WithValueExpression(i => i.nric1);
                   cols.Add("block_no").WithHeaderText("Block no").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.block_no);
                   cols.Add("job_site").WithHeaderText("Job Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(p => p.job_site);
                   cols.Add().WithColumnName("gst_no").WithCellCssClassExpression(p => "col-sm-1").WithHeaderText("Gst No").WithValueExpression(i => i.gst_no);
                   cols.Add().WithColumnName("branch_name").WithCellCssClassExpression(p => "col-sm-2").WithHeaderText("Branch Name").WithValueExpression(i => i.branch_name);
                   cols.Add().WithColumnName("SourceName").WithCellCssClassExpression(p => "col-sm-1").WithHeaderText("Source Of Inquiry").WithValueExpression(i => i.SourceName);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Customer/_LoadCustomer','id',{Value}); class=btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Customer/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete' title='Delete'><span class='fa fa-trash'></span></a><a onclick=UploadFile({Value}) class='btn-xs' title='Customer Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");



               })
               .WithSorting(true, "name1")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ICustomerRepositor repo = new CustomerService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string vCustomerSearch = "";
                   int iBranchId = 0;
                   if (options.GetPageParameterString("customersearch") != null)
                   {
                       vCustomerSearch = Convert.ToString(options.GetPageParameterString("customersearch"));
                   }
                   if (Convert.ToInt32(options.GetPageParameterString("BranchId")) != 0)
                   {
                       iBranchId = Convert.ToInt32(options.GetPageParameterString("BranchId"));
                   }
                   var items = repo.SearchCustomer(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), vCustomerSearch.ToString(), iBranchId);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Customer_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );


            /* User Grid Start */

            MVCGridDefinitionTable.Add("Users", new MVCGridBuilder<PMS.Database.SSP_Users_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("uname").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-4").WithSorting(true).WithValueExpression(p => p.uname);
                   cols.Add("Email").WithColumnName("Email").WithCellCssClassExpression(p => "col-sm-4").WithSorting(true).WithValueExpression(i => i.email);
                   cols.Add("Country").WithColumnName("Country").WithCellCssClassExpression(p => "col-sm-3").WithValueExpression(i => i.Country);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a href='/Account/Edit?Id={Value}' class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>");
               })
               .WithSorting(true, "uname")
               .WithFiltering(true)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   string sortColumn = options.GetSortColumnData<string>();
                   bool? active = null;
                   string fa = HttpContext.Current.Request["is_active"];
                   if (!String.IsNullOrWhiteSpace(fa))
                   {
                       active = (String.Compare(fa, "active", true) == 0);
                   }
                   List<PMS.Database.SSP_Users_Result> items = new List<Database.SSP_Users_Result>();
                   using (PMS.Database.PMSEntities objDB = new Database.PMSEntities())
                   {
                       items = objDB.SSP_Users(HttpContext.Current.Request["uname"], HttpContext.Current.Request["email"], active, Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString()).ToList();
                   }
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<PMS.Database.SSP_Users_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );

            /*  User Grid End   */

            //********************Start Supplier Grid **********************************************//

            MVCGridDefinitionTable.Add("Supplier", new MVCGridBuilder<PMS.Database.SSP_Supplier_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("Suppliersearch")
               .AddColumns(cols =>
               {
                   cols.Add("supplier_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.supplier_name);
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email);
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                   cols.Add().WithColumnName("gst_no").WithHeaderText("Gst No").WithValueExpression(i => i.gst_no);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2")
                   .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Supplier/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Supplier/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");
               })
               .WithSorting(true, "supplier_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISupplierRepositor repo = new SupplierService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string vSuppliersearch = "";
                   if (options.GetPageParameterString("Suppliersearch") != null)
                   {
                       vSuppliersearch = Convert.ToString(options.GetPageParameterString("Suppliersearch"));
                   }
                   var items = repo.SearchSupplier(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), vSuppliersearch.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Supplier_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Supplier Grid **********************************************//

            // ******************* Start My Project Grid   ***********************************************//
            MVCGridDefinitionTable.Add("MyProjects", new MVCGridBuilder<Database.SSP_Projects_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 10)
               .WithPageParameterNames("hdnUID", "brId", "fromdate", "todate", "ProjectStatusId", "ProjectSalesmenId", "SearchStringmyprojects")
               .AddColumns(cols =>
               {
                   cols.Add("project_number").WithHeaderText("Project Number").WithCellCssClassExpression(p => "col-sm-2")
                   .WithSorting(true).WithValueExpression(p => p.project_number);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("contract_date").WithHeaderText("Contract Date").WithCellCssClassExpression(p => "col-sm-2")
                       .WithSorting(true).WithValueExpression(i => i.contract_date);
                   cols.Add("name1").WithHeaderText("Customer").WithCellCssClassExpression(p => "col-sm-1")
                      .WithSorting(true).WithValueExpression(i => i.name1);
                   cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.salesmen_name);
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                     .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);

                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Edit' href='/Contract/NewContract?id={Value}' class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a><a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='fa fa-usd'></span></a></a><a onclick=UploadFile({Value}) class='btn-xs' title='Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");

                   //cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   // .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Edit' onclick=openModelpop('/Projects/_LoadProject','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a><a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='fa fa-usd'></span></a><a title='Delete' onclick=DeleteConfirm('/Projects/DeleteProjectById','id',{Value}); class='btn-xs'><span class='fa fa-trash'></span></a><a onclick=UploadFile({Value}); href='#' class='btn-xs' title='Contract Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");

                   //cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   // .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='fa fa-usd'></span>");

               })
               .WithSorting(true, "contract_date", SortDirection.Dsc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IProject repo = new ProjectService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   DateTime fromdate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("fromdate"));
                   DateTime todate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("todate"));
                   Int32 projStatus = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectStatusId"));
                   Int32 salesMenId = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId"));
                   string SearchStringmyprojects = context.QueryOptions.GetPageParameterString("SearchStringmyprojects");
                   //string SearchStringmyprojects = "";
                   //if (options.GetPageParameterString("SearchStringmyprojects") != null)
                   //{
                   //    SearchStringmyprojects = Convert.ToString(options.GetPageParameterString("SearchStringmyprojects"));
                   //}

                   var items = repo.GetMyProjects(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), fromdate, todate, projStatus, salesMenId, SearchStringmyprojects);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Projects_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Project Grid **********************************************//

            // ******************* Start My Project Additions Grid   ***********************************************//
            MVCGridDefinitionTable.Add("MyProjectAdditions", new MVCGridBuilder<Database.SSP_Project_additions_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "SearchStringAdditions")
               .AddColumns(cols =>
               {
                   cols.Add("project_number").WithHeaderText("Project Number").WithCellCssClassExpression(p => "col-sm-1")
                   .WithSorting(true).WithValueExpression(p => p.project_number);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("contract_date").WithHeaderText("Contract Date").WithCellCssClassExpression(p => "col-sm-2")
                       .WithSorting(true).WithValueExpression(i => i.contract_date);
                   cols.Add("addition_date").WithHeaderText("Addition Date").WithCellCssClassExpression(p => "col-sm-2")
                      .WithSorting(true).WithValueExpression(i => i.addition_date);
                   cols.Add("VO_Type").WithHeaderText("VO_Type").WithCellCssClassExpression(p => "col-sm-1")
                      .WithSorting(false).WithFiltering(true).WithValueExpression(i => Common.CommonFunction.GetVoType(i.record_type));
                   cols.Add("total_amount").WithHeaderText("Total").WithCellCssClassExpression(p => "col-sm-1")
                      .WithSorting(true).WithValueExpression(i => i.total_amount.ToString());
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);

                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false)
                   //  .WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.id.ToString())
                   .WithCellCssClassExpression(p => "col-sm-1").WithValueExpression(i => i.record_type)
                    .WithValueTemplate("<a onclick=openModelpopup('/Projects/_LoadAdditions','id','{Value}','projectId','0'); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Projects/DeleteAdditionById','id','{Value}'); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a></span></a></a>");

                   cols.Add("Link").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false)
                   //  .WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.id.ToString())
                   .WithCellCssClassExpression(p => "col-sm-1").WithValueExpression(i => i.id.ToString())
                    .WithValueTemplate("<a onclick=UploadFile({Value}) class='btn-xs' title='Documents'><i class='fa fa-upload' aria-hidden='true'></i></a>");

               })
               .WithSorting(true, "contract_date", SortDirection.Dsc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IProjectAdditionRepository repo = new ProjectAdditionService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   var items = repo.GetMyProjects(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), context.QueryOptions.GetPageParameterString("SearchStringAdditions"));
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Project_additions_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Project Additions Grid **********************************************//


            //********************Start Company Grid **********************************************//

            MVCGridDefinitionTable.Add("Company", new MVCGridBuilder<PMS.Database.SSP_Company_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("company_name").WithHeaderText("Name").WithCellCssClassExpression(p => "col-sm-2")
                    .WithSorting(true).WithValueExpression(p => p.company_name);
                   cols.Add("email").WithHeaderText("Email").WithCellCssClassExpression(p => "col-sm-2")
                   .WithSorting(true).WithValueExpression(p => p.email);
                   cols.Add("phone").WithHeaderText("Phone").WithCellCssClassExpression(p => "col-sm-2")
                    .WithSorting(true).WithValueExpression(p => p.phone);
                   cols.Add("reg_no").WithHeaderText("Reg No").WithCellCssClassExpression(p => "col-sm-2")
                  .WithSorting(true).WithValueExpression(p => p.reg_no);
                   // cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   //.WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Admin/CompanyLoadAddEdit','id',{Value}); class='btn btn-primary btn-xs'>Edit</a>&nbsp;<a onclick=DeleteConfirm('/Admin/CompanyDeleteById','id',{Value}); class='btnDelete btn btn-primary btn-xs'>Delete</a>");
               })
               .WithSorting(true, "company_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ICompanyRepositor repo = new CompanyService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchCompany(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Company_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Company Grid **********************************************//


            //********************Start Branches Grid **********************************************//

            MVCGridDefinitionTable.Add("Branches", new MVCGridBuilder<PMS.Database.SSP_Branches_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("branch_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("CompanyName").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.company_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("gst_reg_no").WithHeaderText("Gst Reg No").WithValueExpression(i => i.gst_reg_no).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1")
                   .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<a onclick=openModelpop('/Admin/BranchLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/BranchDeleteById','id',{Value}); class='btnDeletew btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");

               })
               .WithSorting(true, "branch_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBranchesRepositor repo = new BranchesService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBranches(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Branches_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );


            MVCGridDefinitionTable.Add("BranchesView", new MVCGridBuilder<PMS.Database.SSP_Branches_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("branch_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("CompanyName").WithHeaderText("Company Name").WithSorting(true).WithValueExpression(i => i.company_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("gst_reg_no").WithHeaderText("Gst Reg No").WithValueExpression(i => i.gst_reg_no).WithCellCssClassExpression(p => "col-sm-2");

               })
               .WithSorting(true, "branch_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBranchesRepositor repo = new BranchesService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBranches(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Branches_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Branches Grid **********************************************//

            //********************Start Salesmen Grid **********************************************//

            MVCGridDefinitionTable.Add("Salesmen", new MVCGridBuilder<PMS.Database.SSP_Salesmen_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SalesmenSearch")
               .AddColumns(cols =>
               {
                   cols.Add("salesmen_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.salesmen_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add("saleman_commission").WithHeaderText("Commission").WithSorting(true).WithValueExpression(p => p.saleman_commission.ToString()).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Branch Name").WithSorting(true).WithValueExpression(i => i.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add().WithColumnName("zip_code").WithHeaderText("Zip Code").WithValueExpression(i => i.zip_code).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("")
                   .WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1 actions").WithValueExpression((p, c) => p.id.ToString())
                   .WithValueTemplate("<a onclick=openModelpop('/Admin/SalesmenLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/SalesmenDeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");
               })
               .WithSorting(true, "salesmen_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISalesmenRepositor repo = new SalesmenService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string Salesmensearch = Convert.ToString(context.QueryOptions.GetPageParameterString("SalesmenSearch"));
                   if (Salesmensearch == null)
                       Salesmensearch = "";
                   var items = repo.SearchSalesmen(Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Salesmensearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Salesmen_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );

            MVCGridDefinitionTable.Add("SalesmenView", new MVCGridBuilder<PMS.Database.SSP_Salesmen_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SalesmenSearch")
               .AddColumns(cols =>
               {
                   cols.Add("salesmen_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.salesmen_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add("saleman_commission").WithHeaderText("Commission").WithSorting(true).WithValueExpression(p => p.saleman_commission.ToString()).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Branch Name").WithSorting(true).WithValueExpression(i => i.branch_name).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("email").WithHeaderText("Email").WithSorting(true).WithValueExpression(i => i.email).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone).WithCellCssClassExpression(p => "col-sm-2");
                   cols.Add().WithColumnName("zip_code").WithHeaderText("Zip Code").WithValueExpression(i => i.zip_code).WithCellCssClassExpression(p => "col-sm-1");
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("")
                   .WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1 actions").WithValueExpression((p, c) => p.id.ToString())
                   .WithValueTemplate("<a onclick=openModelpop('/Admin/SalesmenLoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>");

               })
               .WithSorting(true, "salesmen_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ISalesmenRepositor repo = new SalesmenService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string Salesmensearch = Convert.ToString(context.QueryOptions.GetPageParameterString("SalesmenSearch"));
                   if (Salesmensearch == null)
                       Salesmensearch = "";
                   var items = repo.SearchSalesmen(Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Salesmensearch);
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Salesmen_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );
            // ******************** End Salesmen Grid **********************************************//

            //********************Start Banks Grid **********************************************//


            MVCGridDefinitionTable.Add("Banks", new MVCGridBuilder<PMS.Database.SSP_Banks_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .AddColumns(cols =>
               {
                   cols.Add("bank_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.bank_name);
                   cols.Add().WithColumnName("account_number").WithHeaderText("Account Number").WithSorting(true).WithValueExpression(i => i.account_number);
                   cols.Add().WithColumnName("branch_code").WithHeaderText("Branch Code").WithSorting(true).WithValueExpression(i => i.branch_code);
                   cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                   cols.Add().WithColumnName("branch_name").WithHeaderText("Branch").WithSorting(true).WithValueExpression(i => i.branch_name);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Admin/LoadAddEditBank','id',{Value}); class='btn-xs' title='Delete'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Admin/DeleteBankById','id',{Value}); class='btnDelete btn-xs'><span class='fa fa-trash'></span></a>");
               })
               .WithSorting(true, "bank_name")
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IBanksRepositor repo = new BanksService();
                   string sortColumn = options.GetSortColumnData<string>();
                   var items = repo.SearchBanks(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }
                   return new QueryResult<PMS.Database.SSP_Banks_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
               );

            MVCGridDefinitionTable.Add("BanksView", new MVCGridBuilder<PMS.Database.SSP_Banks_Result>()
              .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              .WithPaging(true, 20)
              .AddColumns(cols =>
              {
                  cols.Add("bank_name").WithHeaderText("Name").WithSorting(true).WithValueExpression(p => p.bank_name);
                  cols.Add().WithColumnName("account_number").WithHeaderText("Account Number").WithSorting(true).WithValueExpression(i => i.account_number);
                  cols.Add().WithColumnName("branch_code").WithHeaderText("Branch Code").WithSorting(true).WithValueExpression(i => i.branch_code);
                  cols.Add().WithColumnName("Phone").WithHeaderText("Phone").WithSorting(true).WithValueExpression(i => i.phone);
                  cols.Add().WithColumnName("branch_name").WithHeaderText("Branch").WithSorting(true).WithValueExpression(i => i.branch_name);
              })
              .WithSorting(true, "bank_name")
              .WithRetrieveDataMethod((context) =>
              {
                  var options = context.QueryOptions;
                  int totalRecords = 0;
                  IBanksRepositor repo = new BanksService();
                  string sortColumn = options.GetSortColumnData<string>();
                  var items = repo.SearchBanks(Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString());
                  if (items != null && items.Count > 0)
                  {
                      totalRecords = Convert.ToInt32(items[0].TotalRecords);
                  }
                  return new QueryResult<PMS.Database.SSP_Banks_Result>()
                  {
                      Items = items,
                      TotalRecords = totalRecords
                  };
              })
              );
            // ******************** End Banks Grid **********************************************//

            // ******************* Start My Receipts Grid   ***********************************************//

            MVCGridDefinitionTable.Add("Receipts", new MVCGridBuilder<Database.SSP_Receipts_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "SearchString")
               .AddColumns(cols =>
               {
                   cols.Add("receipt_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.receipt_date);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("mode_of_payment").WithHeaderText("Payment Mode").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.mode_of_payment);
                   cols.Add("bank_name").WithHeaderText("Bank").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.bank_name);
                   cols.Add("cheque_details").WithHeaderText("Cheque Details").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.cheque_details);
                   cols.Add("remarks").WithHeaderText("Remarks").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.remarks);
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
             .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2").WithValueExpression((p, c) => p.id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Receipts/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Receipts/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");
               })
               .WithSorting(true, "receipt_date", SortDirection.Dsc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IReceiptsRepositor repo = new ReceiptsService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   var items = repo.SearchReceipts(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), context.QueryOptions.GetPageParameterString("SearchString"));
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Receipts_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Receipts Grid **********************************************//

            // ******************* Start My Payments Grid   ***********************************************//

            MVCGridDefinitionTable.Add("Payments", new MVCGridBuilder<Database.SSP_Payments_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo", "SearchProject", "ProjectSalesmenId", "SearchString")
               .AddColumns(cols =>
               {
                   cols.Add("payment_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(p => p.payment_date);
                   cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.project_name);
                   cols.Add("supplier_name").WithHeaderText("Supplier Name").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.supplier_name);
                   cols.Add("cheque_number").WithHeaderText("Cheque Details").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.cheque_number);
                   cols.Add("remarks").WithHeaderText("Remarks").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.remarks);
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2").WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2 td-class-center")
                    .WithValueExpression((p, c) => p.id.ToString()).WithValueTemplate("<div class='payment-action-links'><a onclick=LoadAddEdit({Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a><a onclick=DeleteConfirm('/Payments/DeleteById','id',{Value});  class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a><a title='Print' onclick=openModelpop('/Payments/PrintPreview','id',{Value}); class='btn-xs'><span class='fa fa-printfa fa-print'></span></a><span class='chkboxprintbatch'><input type='checkbox' name='printbatch'  alt='{Value}' class='clsprintbatch'/><span></div>");
               })
               .WithSorting(true, "payment_date", SortDirection.Dsc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   IPaymentsRepositor repo = new PaymentsService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   var items = repo.SearchPayments(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("ProjectSalesmenId")), context.QueryOptions.GetPageParameterString("SearchString"));
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Payments_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End My Payments Grid **********************************************//


            // ******************* Start Loan Grid   ***********************************************//

            MVCGridDefinitionTable.Add("LoanGrid", new MVCGridBuilder<Database.SSP_Loan_Result>()
               .WithAuthorizationType(AuthorizationType.AllowAnonymous)
               .WithPaging(true, 20)
               .WithPageParameterNames("hdnUID", "brId", "SearchFrom", "SearchTo")
               .AddColumns(cols =>
               {
                   cols.Add("loan_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => i.loan_date);

                   cols.Add("person_name").WithHeaderText("Person Name").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.person_name);
                   cols.Add("rec_type").WithHeaderText("Type").WithCellCssClassExpression(p => "col-sm-1").WithSorting(true).WithValueExpression(i => Common.CommonFunction.GetRecordTypeById(i.rec_type));
                   cols.Add("mode_of_payment").WithHeaderText("Payment Mode").WithCellCssClassExpression(p => "col-sm-2").WithSorting(true).WithValueExpression(i => i.mode_of_payment);

                   cols.Add("amount").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-1").WithSorting(false).WithValueExpression(i => i.amount.ToString());
                  
                   cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2")
                   .WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                   cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.Id.ToString())
                    .WithValueTemplate("<a onclick=openModelpop('/Loan/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/Loan/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");
                   cols.Add("CostLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-1").WithValueExpression((p, c) => p.project_id.ToString())
                    .WithValueTemplate("<a title='Cost' class='btn-xs' href='/Report/ProjectCostingReport?ProjectId={Value}' ><span class='fa fa-usd'></span></a>");

               })
               .WithSorting(true, "loan_date", SortDirection.Dsc)
               .WithRetrieveDataMethod((context) =>
               {
                   var options = context.QueryOptions;
                   int totalRecords = 0;
                   ILoanRepositor repo = new LoanService();
                   string sortColumn = options.GetSortColumnData<string>();
                   string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                   var items = repo.SearchLoans(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchFrom")), Convert.ToDateTime(context.QueryOptions.GetPageParameterString("SearchTo")));
                   if (items != null && items.Count > 0)
                   {
                       totalRecords = Convert.ToInt32(items[0].TotalRecords);
                   }

                   return new QueryResult<Database.SSP_Loan_Result>()
                   {
                       Items = items,
                       TotalRecords = totalRecords
                   };
               })
           );
            //********************End Loan Grid **********************************************//


            // ****************** Start My ProjectsBudget Grid **********************************************//

            MVCGridDefinitionTable.Add("ProjectsBudget", new MVCGridBuilder<Database.SSP_Projects_budget_Result>()
            .WithAuthorizationType(AuthorizationType.AllowAnonymous)
            .WithPaging(true, 20)
            .WithPageParameterNames("hdnUID", "brId", "SearchProject", "SupplierId", "SearchString", "SalesmenId")
            .AddColumns(cols =>
            {
                cols.Add("project_name").WithHeaderText("Address/Site").WithCellCssClassExpression(p => "col-sm-2").WithSorting(false).WithValueExpression(i => i.project_name);
                cols.Add("supplier_names").WithHeaderText("Suppliers").WithCellCssClassExpression(p => "col-sm-3").WithSorting(false).WithValueExpression(i => i.supplier_names);
                cols.Add("budget_amount").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-1").WithSorting(false).WithValueExpression(i => i.budget_amount.ToString());
                cols.Add("CreatedUpdated").WithHeaderText("Created/Updated").WithCellCssClassExpression(p => "col-sm-2").WithHtmlEncoding(false).WithSorting(false).WithValueExpression(i => i.CreatedUpdated);
                cols.Add("ViewLink").WithSorting(false).WithHeaderText("").WithHtmlEncoding(false).WithCellCssClassExpression(p => "col-sm-2").WithValueExpression((p, c) => p.id.ToString())
    .WithValueTemplate("<a onclick=openModelpop('/ProjectsBudget/LoadAddEdit','id',{Value}); class='btn-xs' title='Edit'><span class='fa fa-pencil'></span></a>&nbsp;<a onclick=DeleteConfirm('/ProjectsBudget/DeleteById','id',{Value}); class='btnDelete btn-xs' title='Delete'><span class='fa fa-trash'></span></a>");
            })
            .WithSorting(true, "project_name")
            .WithRetrieveDataMethod((context) =>
            {

                var options = context.QueryOptions;
                int totalRecords = 0;
                IProjectsBudgetRepository repo = new ProjectsBudgetService();
                string sortColumn = options.GetSortColumnData<string>();
                string uid = context.QueryOptions.GetPageParameterString("hdnUID");
                var items = repo.SearchProjectsBudget(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("brId")), Convert.ToInt32(options.GetLimitOffset()) + 1, Convert.ToInt32(options.GetLimitRowcount()), sortColumn, options.SortDirection.ToString(), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SearchProject")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("SupplierId")), context.QueryOptions.GetPageParameterString("SearchString"),
                    Convert.ToInt32(context.QueryOptions.GetPageParameterString("SalesmenId")));
                if (items != null && items.Count > 0)
                {
                    totalRecords = Convert.ToInt32(items[0].TotalRecords);
                }

                return new QueryResult<Database.SSP_Projects_budget_Result>()
                {
                    Items = items,
                    TotalRecords = totalRecords
                };


            })
            );
            //********************End My ProjectsBudget Grid **********************************************//

            // // ******************* Start Receipt Report Grid **********************************//
            // MVCGridDefinitionTable.Add("ReceiptReport", new MVCGridBuilder<PMS.Database.SSP_ReceiptReport_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 2)
            //    .WithPageParameterNames("ReportMonth", "Uid", "ReportYear", "BranchId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("receipt_date").WithHeaderText("Date").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(p => p.receipt_date);
            //        cols.Add("cheque_details").WithHeaderText("Cheque No").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(i => i.cheque_details);
            //        cols.Add("amount").WithHeaderText("Amount").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(p => p.amount.ToString());
            //        cols.Add("branch_name").WithHeaderText("Branch").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(p => p.branch_name);
            //        cols.Add("salesmen_name").WithHeaderText("Salesmen").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(i => i.salesmen_name);
            //        cols.Add("customer_name").WithHeaderText("Client").WithCellCssClassExpression(p => "col-sm-2").WithValueExpression(p => p.customer_name);
            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_ReceiptReport_Result> items = new List<Database.SSP_ReceiptReport_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        int mon = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportMonth"));
            //        int yer = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportYear"));

            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_ReceiptReport(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("BranchId")), mon, yer).ToList();
            //        }

            //        if (items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        if (options.GetLimitOffset().HasValue)
            //        {
            //            items = items.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
            //        }
            //        return new QueryResult<PMS.Database.SSP_ReceiptReport_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };

            //    })
            //);
            // // ******************* End Receipt Report Grid **********************************//

            // // ******************* Start Sales Summary Report Grid **********************************//
            // MVCGridDefinitionTable.Add("SaleSummaryReport", new MVCGridBuilder<PMS.Database.SSP_SaleSummeryReport_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 2)
            //    .WithPageParameterNames("Uid", "ReportYear", "BranchId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("Sale").WithHeaderText("Sale").WithValueExpression(p => p.salesmen_name);
            //        cols.Add("Jan").WithHeaderText("Jan").WithValueExpression(p => p.jan.ToString());
            //        cols.Add("Feb").WithHeaderText("Feb").WithValueExpression(p => p.feb.ToString());
            //        cols.Add("Mar").WithHeaderText("Mar").WithValueExpression(p => p.mar.ToString());
            //        cols.Add("Apr").WithHeaderText("Apr").WithValueExpression(p => p.apr.ToString());
            //        cols.Add("May").WithHeaderText("May").WithValueExpression(p => p.may.ToString());
            //        cols.Add("Jun").WithHeaderText("Jun").WithValueExpression(p => p.jun.ToString());
            //        cols.Add("Jul").WithHeaderText("Jul").WithValueExpression(p => p.jul.ToString());
            //        cols.Add("Aug").WithHeaderText("Aug").WithValueExpression(p => p.aug.ToString());
            //        cols.Add("Sep").WithHeaderText("Sep").WithValueExpression(p => p.sep.ToString());
            //        cols.Add("Oct").WithHeaderText("Oct").WithValueExpression(p => p.oct.ToString());
            //        cols.Add("Nov").WithHeaderText("Nov").WithValueExpression(p => p.nov.ToString());
            //        cols.Add("Dec").WithHeaderText("Dec").WithValueExpression(p => p.dec.ToString());
            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_SaleSummeryReport_Result> items = new List<Database.SSP_SaleSummeryReport_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        int yer = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportYear"));

            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_SaleSummeryReport(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("BranchId")), yer).ToList();
            //        }

            //        if (items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        if (options.GetLimitOffset().HasValue)
            //        {
            //            items = items.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
            //        }
            //        return new QueryResult<PMS.Database.SSP_SaleSummeryReport_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };

            //    })
            //);
            // // ******************* End Sales Summary Report Grid **********************************//


            // // ******************* Start Payment Report Grid **********************************//
            // MVCGridDefinitionTable.Add("PaymentReport", new MVCGridBuilder<PMS.Database.SSP_PaymentReport_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 2)
            //    .WithPageParameterNames("fromdate", "Uid", "todate", "BranchId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("payment_date").WithHeaderText("Date").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.payment_date);
            //        cols.Add("supplier_name").WithHeaderText("Supplier Name").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.supplier_name);
            //        cols.Add("invoice_amount").WithHeaderText("Capital").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.invoice_amount.ToString());
            //        cols.Add("rebate_amount").WithHeaderText("Dis Received").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.rebate_amount.ToString());
            //        cols.Add("mode_payment").WithHeaderText("Mode of").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.mode_payment);
            //        cols.Add("payment_amount").WithHeaderText("Payment ").WithCellCssClassExpression(py => "col-sm-2").WithValueExpression(py => py.payment_amount.ToString());
            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_PaymentReport_Result> items = new List<Database.SSP_PaymentReport_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        DateTime fromdate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("fromdate"));
            //        DateTime todate = Convert.ToDateTime(context.QueryOptions.GetPageParameterString("todate"));
            //        Int32 branchid = Convert.ToInt32(context.QueryOptions.GetPageParameterString("branchid"));
            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_PaymentReport(uid, fromdate, todate, branchid).ToList();
            //        }

            //        if (items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        if (options.GetLimitOffset().HasValue)
            //        {
            //            items = items.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
            //        }
            //        return new QueryResult<PMS.Database.SSP_PaymentReport_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };
            //    })
            //);
            // // ******************* End Payment Report Grid **********************************//

            // // ******************* Start Individual Sales Report Grid **********************************//
            // MVCGridDefinitionTable.Add("IndividualSaleReport", new MVCGridBuilder<PMS.Database.SSP_SaleIndividualReport_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 25)
            //    .WithPageParameterNames("Uid", "ReportYear", "BranchId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("Date").WithHeaderText("Date").WithValueExpression(p => p.receipt_date);
            //        cols.Add("Project Name").WithHeaderText("Project Name").WithValueExpression(p => p.project_name);
            //        cols.Add("Project Number").WithHeaderText("Project Number").WithValueExpression(p => p.project_number);
            //        cols.Add("Date of Contract").WithHeaderText("Date of Contract").WithValueExpression(p => p.contract_date);
            //        cols.Add("Sales Amt").WithHeaderText("Sales Amt").WithValueExpression(p => p.total_amount.ToString());                   
            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_SaleIndividualReport_Result> items = new List<Database.SSP_SaleIndividualReport_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        int yer = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportYear"));

            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_SaleIndividualReport(uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("BranchId")), yer).ToList();
            //        }

            //        if (items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        if (options.GetLimitOffset().HasValue)
            //        {
            //            items = items.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
            //        }
            //        return new QueryResult<PMS.Database.SSP_SaleIndividualReport_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };

            //    })
            //);
            // // ******************* End Individual Sales Report Grid **********************************//

            // // ******************* Start Project Listing Report Grid **********************************//
            // MVCGridDefinitionTable.Add("ProjectListingReport", new MVCGridBuilder<Database.SSP_ProjectsBySalesman_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 25)
            //    .WithPageParameterNames("Uid", "ReportYear", "BranchId", "SalesmenId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("Project Name").WithHeaderText("Project Name").WithValueExpression(p => p.project_name);
            //        cols.Add("Project Number").WithHeaderText("Project Number").WithValueExpression(p => p.project_number);
            //        cols.Add("Date of Contract").WithHeaderText("Date of Contract").WithValueExpression(p => p.contract_date);
            //        cols.Add("Date of Project Closed").WithHeaderText("Date of Project Closed").WithValueExpression(p => p.closing_date);
            //        cols.Add("Salesmen").WithHeaderText("Salesmen").WithValueExpression(p => p.salesmen_name);
            //        cols.Add("Branch").WithHeaderText("Branch").WithValueExpression(p => p.branch_name);
            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_ProjectsBySalesman_Result> items = new List<Database.SSP_ProjectsBySalesman_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        int yer = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportYear"));

            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_ProjectsBySalesman(yer,uid, Convert.ToInt32(context.QueryOptions.GetPageParameterString("SalesmenId")), Convert.ToInt32(context.QueryOptions.GetPageParameterString("BranchId"))).ToList();
            //        }
            //        if(items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        return new QueryResult<Database.SSP_ProjectsBySalesman_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };

            //    })
            //);
            // // ******************* End Project Listing Report Grid **********************************//


            // // ******************* Start Project Status by Branch Report Grid **********************************//
            // MVCGridDefinitionTable.Add("ProjectStatusbyBranchReport", new MVCGridBuilder<PMS.Database.SSP_ProjectListingStatusByBranch_Result>()
            //    .WithAuthorizationType(AuthorizationType.Authorized)
            //    .WithPaging(true, 2)
            //    .WithPageParameterNames("ReportYear", "Uid", "BranchId")
            //    .AddColumns(cols =>
            //    {
            //        cols.Add("job_sites").WithHeaderText("Job Sites").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.job_sites);
            //        cols.Add("contact_no").WithHeaderText("Contact No").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.contact_no);
            //        cols.Add("id_in_charge").WithHeaderText("ID In-Charge").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.id_in_charge);
            //        cols.Add("contract_date").WithHeaderText("Date of Contract").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.contract_date);
            //        cols.Add("tiles").WithHeaderText("Tiles ").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.tiles);
            //        cols.Add("m_bldg_products").WithHeaderText("M. Bldg Products ").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.m_bldg_products);
            //        cols.Add("project_closed").WithHeaderText("Project Closed ").WithCellCssClassExpression(py => "col-sm-2")
            //        .WithValueExpression(py => py.project_closed);

            //    })
            //    .WithRetrieveDataMethod((context) =>
            //    {
            //        var options = context.QueryOptions;
            //        int totalRecords = 0;
            //        string sortColumn = options.GetSortColumnData<string>();
            //        List<Database.SSP_ProjectListingStatusByBranch_Result> items = new List<Database.SSP_ProjectListingStatusByBranch_Result>();
            //        string uid = context.QueryOptions.GetPageParameterString("Uid");
            //        int year = Convert.ToInt32(context.QueryOptions.GetPageParameterString("ReportYear"));
            //        Int32 branchid = Convert.ToInt32(context.QueryOptions.GetPageParameterString("branchid"));
            //        using (Database.PMSEntities objDB = new Database.PMSEntities())
            //        {
            //            items = objDB.SSP_ProjectListingStatusByBranch(uid, year, branchid).ToList();
            //        }

            //        if (items != null)
            //        {
            //            totalRecords = items.Count();
            //        }
            //        if (options.GetLimitOffset().HasValue)
            //        {
            //            items = items.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value).ToList();
            //        }
            //        return new QueryResult<PMS.Database.SSP_ProjectListingStatusByBranch_Result>()
            //        {
            //            Items = items,
            //            TotalRecords = totalRecords
            //        };

            //    })
            //);
            // // ******************* End Project Status by Branch Report Grid **********************************//



        }
    }
}