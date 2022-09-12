using PMS.Models;
using PMS.Data_Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Interface
{
    public interface IMaster
    {
        List<TasksList> BindTasksList();

        SuccessMessage CreateTask(TasksList _TasksList, string uid);

        string DeleteTask(List<TasksList> _TasksList, string uid);


        List<UOM> UOMList();

        SuccessMessage CreateUOM(UOM _UOMList, string uid);

        string DeleteUOM(List<UOM> _UOMList, string uid);


        List<Category> CategoryList();

        SuccessMessage CreateCategory(Category _CategoryList, string uid);

        string DeleteCategory(List<Category> _CategoryList, string uid);


        List<Floor> FloorList();

        SuccessMessage CreateFloor(Floor _FloorList, string uid);

        string DeleteFloor(List<Floor> _FloorList, string uid);

        List<Plan> PlanList();

        SuccessMessage CreatePlan(Plan _PlanList, string uid);

        string DeletePlan(List<Plan> _PlanList, string uid);


        List<PackageType> PackageTypeList();

        SuccessMessage CreatePackageType(PackageType _PackageTypeList, string uid);

        string DeletePackageType(List<PackageType> _PackageTypeList, string uid);

        List<Item> ItemList(string userId);

        List<PropertyType> PropertyTypeList(string userId);

        SuccessMessage CreateItem(Item _ItemList, string uid);
        SuccessMessage CreatePropertyType(PropertyType _PropertyList, string uid,string mode);
        SuccessMessage CreateElectricalItemType(ElectricalItemMapping _ElectricalItemsList, string uid, string mode);
        List<ElectricalItemMapping> GetElectricalItemsMappingDetails(int PropertyType_Id, string userId);


        string DeleteItem(List<Item> _ItemList, string uid);

        List<UOMDropDown> UOMDropDownList();

        List<TaskDropDown> TaskDropDownList();

        List<FloorDropdown> FloorDropDownList();

        List<CategoryDropDown> CategoryDropDownList();

        List<ItemDropDown> ElectricalItemDropDownList(int PropertyType_Id);

        List<ItemDropDown> ItemDropDownList(string TaskId);

        List<PackageDropdown> PackageDropDownList();

        List<PlanDropdown> PlanDropDownList();
        string UpsertMasterContractTerms(MasterContractTermsCriteria _MasterContractTermsCriteria, string uid);
        string UpsertMasterPaymentTerms(MasterPaymentTermsCriteria _MasterPaymentTermsCriteria, string uid);
        List<TasksList> GetMasterTasksList();
        List<TasksList> GetTasksbytaskid(TasksList _GetTasksByTaskId);
        List<Item> GetItemByItemId(Int32 ItemId);
        List<Plan> GetPlansByPlanId(Plan _GetPlansByPlanId);
        List<UOM> GetUomByUomId(UOM _GetUomByUomId);
        List<Floor> GetFloorByFloorId(Floor _GetFloorByFloorId);
        List<PackageType> GetPackageTypeByPackageId(PackageType _GetPackageTypeByPackageId);
        //List<Package> GetPackagesByPackageId(Package _GetPackagesByPackageId);
        SuccessMessage DeleteTasksByTaskId(string TaskId);
        SuccessMessage DeleteItemByItemId(string ItemId);
        SuccessMessage DeleteUOMByUomId(string _DeleteUOMById);
        SuccessMessage DeletePlansByPlanId(string _DeletePlanById);
        SuccessMessage DeleteFloorByFloorId(string _DeleteFloorById);
        SuccessMessage DeletePackageTypeByPackageTypeId(string _DeletePackageTypeById);
        List<SuccessMessage> DeletePackagesByPackageTypeId(PackageDetail _DeletePackageById);
        List<StatusLookup> Bind_StatusLookUp(string LookUpColumnId);
        List<PaymentDescription> BindMasterpaymentterms();
        List<PaymentDescription> BindPackageMasterPaymentTerms();
        List<Banks> BindBanks();
        List<CustomerDropDown> BindCustomer();

        List<SalesmanDropDown> BindSalesmen();

        List<SalesmanDropDown> GetSalesmenListByProject(int ProjectId);

        List<AddressDropDown> BindAddressSite(string Project_Id, string Salesmen_Id);

        List<AddressDropDown> BindAddressSite(Int64 Supplier_Id);

        List<SupplierInvoiceDropDown> BindSuppliernvoiceNo(Int64 id);

        List<ContractTerm> ContractTermsList();
        SuccessMessage CreateContractTerm(ContractTerm contractTerms, Guid uid);
        SuccessMessage DeleteCategoryByCategoryId(string Category_Id);
        SuccessMessage DeleteMasterContractTermsByID(string ContractTermId);
        List<ModeOfPaymentDropDown> BindMode_of_Payments();
        List<SupplierDropDown> BindSupplier();
        List<SupplierDropDown> BindSupplier(string userId);
        SalesmanDropDown GetSalesmenIdByUserId(string uid);
        List<AddressDropDown> BindAddressSiteByStatus(string Project_Id, string Salesmen_Id);

        List<SourceOfInquiry> SourceOfInquiryList();
        SuccessMessage CreateSourceOfInquiry(SourceOfInquiry _SourceOfInquiryList, string uid);
        SuccessMessage DeleteSourceOfInquiryBySourceOfInquiryId(string SourceOfInquiry_Id);

        List<UnitCodeDropDown> BindUnitCode(string ZipCodeId);

        List<ZipCodeDropDown> BindZipCode();
        List<SalesmanDropDown> GetSalesmenList(string ZipCodeId, string UnitCodeId, string BranchId);

        List<AddressSiteDropDown> GetAddressSiteList(string ZipCodeId, string UnitCodeId,string BranchId,string SalesmanId);

        List<BudgetCostTypeDropDown> BindBudgetCostType();
    }
}
