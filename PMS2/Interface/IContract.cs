using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;

namespace PMS.Interface
{
    public interface IContract
    {
        List<QuotationList> QuotationsList(QuotationListCriteria _QuotationsList);
        SuccessMessage UpsertProjectContractTerms(ProjectContractTermsCriteria _ProjectContractTermsCriteria, string uid, string ProjectId);
        SuccessMessage UpsertProjectPaymentTerms(PaymentTerms _ProjectPaymentTermsCriteria, string uid, string ProjectId);
        QuotationDetails GetQuotationDetailsByProjectId(string ProjectId);
        List<ProjectTasksItem> GetProjectTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<ProjectTasksItem> GetProjectTasksItemPackage(ProjectTasksItemList _ProjectTasksItemCriteria,bool IsFromPackage);
        List<ProjectTasksItemList> GetProjectTasksListItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<ProjectTasksItemList> GetProjectTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<ProjectTasksItemList> GetProjectTasksQuotationItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria,bool IsFromPackage);
        List<PaymentTerms> GetProjectpaymentterms(string ProjectId);
        SuccessMessage UpsertProjectDetails(QuotationUpsertProjectDetails _QuotationUpsertProjectDetails);
        SuccessMessage UpsertProjectQuotation(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        string UpsertProjectTasks(List<ProjectTasksItemList> _ProjectTasksCriteria, string ProjectId);
        List<Contract> GetContractTerms(string project_id);
        List<ProjectContractTermsCriteria> GetProjectContractTermsList(string ProjectId);
        SuccessMessage Insert_Projects(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        SuccessMessage DeleteProjectQuotation(string uid, string ProjectId);
        SuccessMessage DeleteProjectDetails(string uid, string Project_det_id);
        SuccessMessage DeleteProjectPaymentTermsByID(string uid, string Payment_term_id);
        List<QuotationDetails> GetQuotationDetailsListByProjectId(string ProjectId);
        SalesmanDropDown GetSalesmenIdByUserId(string uid);
        SuccessMessage UpsertSignature(Signature _Signature, string uid);

        SuccessMessage Update_ContractStatus(QuotationStatusCriteria _QuotationStatusCriteria, string uid);
        //string SavePath(string Id, string path);
        //string DeletePath(string Id, string path);
        List<QuotationDetails> Get_Documents(string ProjectId, int ID_TYPE);

        SuccessMessage UpsertProjectContractQuotation(CreateQuotationCriteria _CreateQuotationCriteria, string uid);
        List<UserEmailAddress> GetAdminAndSalesmenEmailAddress(string Salesmen_Id,string Project_Id,int Procedure);
        List<CustomerDetailsById> GetCustomerDetailsById(string customer_id);

        SuccessMessage UpsertContractSupplierMapping(string suppID, string ProjectID);

        contractSupplierMapping GetContractSupplierMappingForProject(string ProjectID);
        SuccessMessage GetAdditionOmissionRowsCount(long ProjectId);
    }
}
