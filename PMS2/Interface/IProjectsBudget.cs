using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;
using PMS.Database;

namespace PMS.Interface
{
    public interface IProjectsBudget
    {
        List<ProjectsBudget> ProjectsbudgetList(ProjectsBudgetView _ProjectsBudgetView);
        SuccessMessage AddBudgetDetails(AddBudget _AddBudget, bool IsSupplierCreated);
        SuccessMessage Update_ForApproval(ProjectsBudget _StatusCriteria);
        SuccessMessage Update_ForApprovalWithReason(ProjectsBudget _StatusCriteria); 
        List<ProjectsBudget> Get_Project_BudgetById(ProjectsBudgetView _ProjectsBudgetView);
        List<ProjectsBudget> Get_Project_Budget_DetailsById(ProjectsBudgetView _ProjectsBudgetView);

        SuccessMessage UpsertSignature(Signature _Signature, string uid);
        SuccessMessage UpsertSignatureForProjectBudget(Signature _Signature, string uid, SuccessMessage successMessage,string UserName);

        string GetProjectIdById(string ProjectId);

        List<QuotationDetails> Get_DocumentsForProjectBudget(int ProjectBudgetDetailsId);

        string GetFilePathById(Int64 ProjectId);
        SuccessMessage Remove_Project_Budget_Detail(string project_budget_detail_id);
        SuccessMessage Remove_Project_Budget(string project_budget_id);

        SuccessMessage updateBudgetDetails(AddBudget _AddBudget, bool IsSupplierCreated);

        List<ProjectsBudget> Get_Project_Budget_DetailsByIdForEditBudgetCost(ProjectsBudgetView _ProjectsBudgetView);

        List<getBudgetedInvoice_Result> Get_budget_Invoice(int projectId, int supplierId);
        getBudgetedInvoice_Result getBudgetedCost(int Id);

        List<SupplierAddress> GetSupplierAddressById(int supplier_id);

        List<UserEmailAddress> GetAdminAndSalesmenEmailAddress(string Salesmen_Id, string Project_Id, int Procedure);

        SalesmenAndSupplierDetails GetSalesmenAndSupplierDetailsByProjectBudgetDetailsId(int project_budget_details_id);
        string GetProjectNumberIdById(string id, string action);

        string GetProjectNumberIdByBudgetId(long id);
    }
}
