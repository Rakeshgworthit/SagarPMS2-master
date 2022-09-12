using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Models;
using PMS.Data_Access;

namespace PMS.Interface
{
    public interface IPackage
    {
        List<PackageList> PackagesList(string userId);

        SuccessMessage CreatePackage(PackageList _PackagesList);
        string DeletePackage(List<PackageList> _PackagesList, string uid);
        SuccessMessage UpsertPackageDetails(PackageTasksItemList _UpsertPackageDetailsCriteria);
        List<PackageTasksList> GetPackageTasks(PackageTasksCriteria _ProjectTasksCriteria);
        string UpsertPackage(UpsertPackageCriteria _UpsertPackageCriteria, string uid);
        List<PackageTasksItem> GetPackageTasksItem(string PackageId, string TaskId);

        List<TaskDropDown> GetPackageTasksListItem(string PackageId, string TaskId);
        List<PackageTasksItemList> GetPackageTasksItemDetails(string PackageId, string TaskId);
        List<PackageAmountList> GetPackageAmount(PackageAmountList _PackageAmountCriteria);
        PackageDetail GetPackagesByPackageId(string PackageId);
        List<PaymentTerms> GetPackagepaymentterms(string PackageId);
        SuccessMessage UpsertPackagePaymentTerms(PaymentTerms _PackagePaymentTermsCriteria, string uid, string PackageId);
        string UpsertPackageTasks(List<PackageTasksItemList> _ProjectTasksCriteria, string ProjectId);
        List<PackageDetail> GetQuotationForSearchPackage(PackageDetail _PackagesSearchList);
        SuccessMessage UpsertPackage_PackageDetails(NewPackageDetailTasksItem _UpsertPackageDetailsCriteria, NewPackageDetailTasksItem _HeaderDetails, string uid);
        SuccessMessage DeletePackages(string uid, string package_id);
        SuccessMessage deletepackageDetails(string uid, string package_det_id);
        SuccessMessage DeletePackagePaymentTermsByID(string uid, string Payment_term_id);
        SuccessMessage Update_ActDeactPackage(string PackageId, Boolean IsActive);
        SuccessMessage Upsert_Package_For_Clone(PackageList _PackagesList);
        SuccessMessage Upsert_Package_For_Clone_InProject(PackageList _PackagesList);
    }
}