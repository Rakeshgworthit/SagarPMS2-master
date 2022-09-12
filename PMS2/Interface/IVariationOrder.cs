using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Models;
using PMS.Data_Access;

namespace PMS.Interface
{
    public interface IVariationOrder
    {
        QuotationDetails GetQuotationDetailsByProjectIdForEvo(string ProjectId, string evo_id = null);
        QuotationDetails GetQuotationDetailsByProjectId(string ProjectId);
        List<VariationOrder> GetVO_Details(string vo_det_id);
        SuccessMessage UpsertVO_Details(VariationOrder _VariationOrderDetailCriteria, string uid, string ProjectId);

        List<ProjectTasksItem> GetVOTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria);

        List<VariationOrder> GetVOTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<VariationOrderList> GetVO(SalesmanDropDown salesmen, string ProjectId, VOListCriteria evoListCriteria);
        List<ElectricalVariationOrderList> GetEVO(SalesmanDropDown salesmen, string ProjectId, EvoListCriteria _evoListCriteria);
        SuccessMessage Delete_Vo_Details(string uid, string vo_det_id, string ProjectId, string record_type);
        SuccessMessage Delete_vo(string uid, string vo_id);

        VariationOrderList GetVODetailsByProjectId(string ProjectId, string VO_Id);
        VariationOrderList GetEVODetailsByProjectId(string ProjectId, string VO_Id);
        SalesmanDropDown GetSalesmenIdByUserId(string uid);
        List<ProjectTasksItemList> GetVOTasksListItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        SuccessMessage Validation_VO_Omission(VariationOrder _VariationOrderDetailCriteria, string ProjectId);
        SuccessMessage Update_VOStatus(VOStatusCriteria _VOStatusCriteria, string uid);
        SuccessMessage Update_EVOStatus(VOStatusCriteria _VOStatusCriteria, string uid);
        SuccessMessage UpsertSignature(Signature _Signature, string uid);
        SuccessMessage UpsertEvoSignature(Signature _Signature, string uid);
        SuccessMessage UpsertVO(VariationOrderCriteria _VariationOrderCriteria, string uid);
        SuccessMessage UpsertEVO(VariationOrderCriteria _EVOCriteria, string uid);
        SuccessMessage GetRowsCount(string ProjectId);       
        SuccessMessage GetEVORowsCount(string ProjectId);
        SuccessMessage Get_VODetails_RowsCount(string VO_Id);
        SuccessMessage Get_EVODetails_RowsCount(string EVO_Id);
        SuccessMessage GetDiscountPercentageFromContract(string ProjectId);
        List<VOCategoryDropDown> VOCategoryDropDownList();
        List<ElectricalItemMapping> GetElectricalItemsDetails(int PropertyType_Id, string projectId, string evo_id = null);
        SuccessMessage GetElectricalItemsDetailsCheckedorUnchecked(string projectId, string SelectedData, string UpdatedData, int propertyType_Id, string uid, string evo_det_id, bool IsCheckboxSelected = false);
        SuccessMessage SaveEvoSelectedData(string projectId, string SelectedData, string UpdatedData, int propertyType_Id, string evo_det_id, string uid);
        SuccessMessage SaveNewMethodEvoSelectedData(List<EVOCriteria> _EVOCriteria, EVOTotalCriteria totalCriterias, string Project_id, int PropertyType_Id, string evo_id, string uid, string evo_det_id);
        SuccessMessage InsertHeaderNewEvo(string ProjectId, bool isNew = false);
        List<UserEmailAddress> GetAdminAndSalesmenEmailAddress(string Salesmen_Id, string Project_Id, int Procedure);
        List<ProjectTasksItem> GetEVOTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria);
        List<VariationOrder> GetEVOTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria);
    }
}
