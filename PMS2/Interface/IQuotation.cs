using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;
using System.Data;
using PMS.Controllers;

namespace PMS.Interface
{
    public interface IQuotation
    {
        List<QuotationList> QuotationsList(QuotationListCriteria _QuotationsList);
        SuccessMessage UpsertProjectQuotation(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        QuotationFromPackageResponse UpsertQuotationFromPackage(QuotationFromPackageCriteria _QuotationFromPackageCriteria, string uid);
        SuccessMessage UpsertProjectDetails(QuotationUpsertProjectDetails _QuotationUpsertProjectDetails);
        bool UpsertAdditionDescription(string description, string Id);
        string GetAdditionDescription( string Id);
        List<ProjectTasksList> GetProjectTasks(ProjectTasksCriteria _ProjectTasksCriteria);
        List<ProjectAmountList> ProjectAmountList(ProjectTasksCriteria _QuotationsList);
        List<ProjectTasksItem> GetProjectTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<ProjectTasksItem> GetProjectTasksItemPackage(ProjectTasksItemList _ProjectTasksItemCriteria, bool bIsFromPackage);
        List<ProjectTasksItem> GetProjectTasksQuotationItem(ProjectTasksItemList _ProjectTasksItemCriteria,bool IsFromPackage);
        List<ProjectTasksItemList> GetProjectTasksListItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<ProjectTasksItemList> GetProjectTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria);

        List<ProjectTasksItemList> GetProjectTasksQuotationItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria, bool IsFromPackage);

        List<Quotation> BindProjectStatus(string Type);

        QuotationDetails GetQuotationDetailsByProjectId(string ProjectId);
        List<PaymentTerms> GetProjectpaymentterms(string ProjectId);
        string UpsertProjectTasks(List<ProjectTasksItemList> _ProjectTasksCriteria, string ProjectId);
        List<Salesmancommission> GetSalesmenDetailsById(string salesmen_id);
        List<CustomerDetailsById> GetCustomerDetailsById(string customer_id);
        SuccessMessage UpsertProjectPaymentTerms(PaymentTerms _ProjectPaymentTermsCriteria, string uid, string ProjectId);
        SuccessMessage Update_ProjectStatus(QuotationStatusCriteria _QuotationStatusCriteria, string uid);
        SuccessMessage DeleteProjectQuotation(string uid, string ProjectId);
        SuccessMessage DeleteProjectDetails(string uid, string Project_det_id);
        SuccessMessage DeleteProjectPaymentTermsByID(string uid, string Payment_term_id);
        List<QuotationDetails> GetQuotationDetailsListByProjectId(string ProjectId);
        SalesmanDropDown GetSalesmenIdByUserId(string uid);
        SuccessMessage UpsertProjectQuotation_Clone(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        SuccessMessage UpsertProjectQuotation_History(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        SuccessMessage UpsertProjectDetails_History(QuotationUpsertProjectDetails _QuotationUpsertProjectDetails);
        List<UserEmailAddress> GetAdminAndSalesmenEmailAddress(string Salesmen_Id, string Project_Id,int Procedure);
        QuotationImportExcelResult ImportFromExelFile(DataTable dtHeader, DataTable dtDetails);
        SuccessMessage CheckCustomerAndSalesmenDetails(string salesmenName, string customerName);
    }
}
