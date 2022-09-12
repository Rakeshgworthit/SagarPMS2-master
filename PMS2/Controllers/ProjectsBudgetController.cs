using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMS.Models;
using PMS.Repository.DataService;
using PMS.Repository.Infrastructure;
using PMS.Database;
using PMS.Common;
using PMS.Data_Access;
using PMS.Interface;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ProjectsBudgetController : Controller
    {
        private IProjectsBudget _IProjectsBudget;
        private readonly IProjectsBudgetRepository _ProjectsBudgetRepo;
        private readonly IProjectsBudgetDetailRepository _ProjectsBudgetDetailRepo;
        public ProjectsBudgetController(IProjectsBudgetRepository ProjectsBudgetRepo, IProjectsBudgetDetailRepository ProjectsBudgetDetailRepo)
        {
            _ProjectsBudgetRepo = ProjectsBudgetRepo;
            _ProjectsBudgetDetailRepo = ProjectsBudgetDetailRepo;
            _IProjectsBudget = new DataLayer();

        }
        public ActionResult Index(ProjectsBudgetViewModel objView)
        {
            DateTime now = DateTime.Now;
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.UID = "00000000-0000-0000-0000-000000000000";
                    ViewBag.IsAdminLogin = "YES";
                }
                else
                {
                    objView.UID = User.Identity.GetUserId();
                    ViewBag.IsAdminLogin = "NO";
                }
                if (objView.SearchString == null)
                {
                    objView.SearchString = "";
                }
                else
                {
                    objView.SearchString = objView.SearchString;
                }
                var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
                var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
                if (objView.SearchFrom == null)
                    objView.SearchFrom = CurrentstartDate.AddMonths(-1);
                if (objView.SearchTo == null)
                    objView.SearchTo = endDate;
                objView.SalesmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                objView.StatusList = CommonFunction.ProjectBudgetStatusList();
                if (User.IsInRole("SuperAdmin"))
                    objView.supplierList = CommonFunction.UserSupplierList(objView.UID);
                else
                {
                    int salesMenId = CommonFunction.SalesmenIDByUserID(objView.UID);
                    objView.supplierList = CommonFunction.GetSupplierListBySalesManBudgetCost(salesMenId);
                    if (objView.Status_Id == 0)
                        objView.Status_Id = 7;
                }
                objView.projectList = CommonFunction.UserProjectList(objView.UID);
                if (TempData["MsgDelete"] != null)
                    ViewBag.MsgDelete = TempData["MsgDelete"].ToString();
                
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Index, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }
        public ActionResult DeleteById(Int32 Id)
        {
            try
            {
                if (Id > 0)
                {
                    projects_budget objRec = _ProjectsBudgetRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    _ProjectsBudgetRepo.Delete(objRec);
                    _ProjectsBudgetRepo.Save();
                    TempData["MsgDelete"] = "Record deleted successfully.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DeleteById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
            }
        }

        #region old  code 
        //public string getBudgetedInvoice(Int32 projectId, Int32 supplierId)
        //{


        //    using (Database.PMSEntities obj = new PMSEntities())
        //    {
        //        string ddlhtml = "";
        //        List<SSP_getBudgetedInvoice_Result> objPBList = obj.SSP_getBudgetedInvoice(projectId, supplierId).ToList();

        //        ddlhtml = ddlhtml + "<option value =''>Please Select</option>";
        //        foreach (var items in objPBList)
        //        {
        //            ddlhtml = ddlhtml + "<option value ='" + items.inv.ToString() + "'>" + items.remarks.ToString() + "</option >";
        //        }
        //        return ddlhtml;
        //    }
        //}
        #endregion

        public string getBudgetedInvoice(Int32 projectId, Int32 supplierId)
        {
            try
            {
                using (Database.PMSEntities obj = new PMSEntities())
                {
                    string ddlhtml = "";
                    List<SSP_getBudgetedInvoice_Result> _list = obj.SSP_getBudgetedInvoice(projectId, supplierId).ToList();

                    if (_list.Count() > 0)
                    {
                        ddlhtml = ddlhtml + "<option value =''>Please Select</option>";
                        foreach (var item in _list)
                        {
                            if (string.IsNullOrEmpty(item.PaymentAmount) || item.budget_amount > Convert.ToDecimal(CalcualteInvoiceAmount(item.PaymentAmount, false, false)))
                            {
                                ddlhtml = ddlhtml + "<option value ='" + item.inv.ToString() + "'>" + item.remarks.ToString() + "</option >";
                            }
                        }
                    }
                    return ddlhtml;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: getBudgetedInvoice, Parameter : projectId={projectId} , supplierId={supplierId}");
                return null;
            }
            finally
            {
            }
        }

        public string getBudgetedInvoiceNew(Int32 projectId, Int32 supplierId)
        {
            try
            {
                using (Database.PMSEntities obj = new PMSEntities())
                {
                    string ddlhtml = "";
                    

                   // List<SSP_getBudgetedInvoice_Result> _list = obj.SSP_getBudgetedInvoice(projectId, supplierId).ToList();
                    var _list = _IProjectsBudget.Get_budget_Invoice(projectId, supplierId);
                    if (_list.Count() > 0)
                    {
                        ddlhtml = ddlhtml + "<option value ='0'>Please Select</option>";
                        foreach (var item in _list)
                        {
                            if (string.IsNullOrEmpty(item.PaymentAmount) || item.budget_amount > Convert.ToDecimal(CalcualteInvoiceAmount(item.PaymentAmount, false, false)))
                            {
                                ddlhtml = ddlhtml + "<option value ='" + item.inv.ToString() + "'>" + item.invoiceNumber.ToString() + "</option >";
                            }
                        }
                    }
                    return ddlhtml;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: getBudgetedInvoice, Parameter : projectId={projectId} , supplierId={supplierId}");
                return null;
            }
            finally
            {
            }
        }

        internal static string CalcualteInvoiceAmount(string total, bool calcutedAmount = false, bool isDefaultDollerIncluded = true)
        {
            decimal TotalAmount = 0;
            var list = total.Split(',').ToList();
            decimal itemTotal = list.Count() > 0 ? list.Sum(o => Convert.ToDecimal(o)) : 0;

            TotalAmount = calcutedAmount ? itemTotal : itemTotal;
            return isDefaultDollerIncluded ? string.Format("${0}", TotalAmount) : string.Format("{0}", TotalAmount);
        }

        public JsonResult getBudgetedCost(Int32 Id)
        {
            try
            {
                var _list = _IProjectsBudget.getBudgetedCost(Id);

                var data = new
                {
                    Items = _list
                   
                };
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            //  decimal budgetedCostAmt = Convert.ToDecimal(obj.SSP_getBudgetedCostByid(Convert.ToInt32(Id)).SingleOrDefault());
             catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: getBudgetedCost, Parameter");
                return null;
            }
        }

        public ActionResult LoadApprovalReason(Int32 Id, decimal Approved_Amount)
        {
            ProjectsBudget _ProjectsBudgetView = new ProjectsBudget();
            SuccessMessage _successMessage = new SuccessMessage();
            try
            {
                _ProjectsBudgetView.project_budget_details_id = Id;
                //_ProjectsBudgetView.status_id = Status_Id;
                _ProjectsBudgetView.Approved_amount = Approved_Amount;
                return View(_ProjectsBudgetView);
            }
            catch(Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadApprovalReason, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                _ProjectsBudgetView = null;
            }
        }

        public JsonResult CheckApprovedAmount(Int32 Id)
        {
            try
            {
                var _list = _IProjectsBudget.getBudgetedCost(Id);
                var data = new
                {
                    Items = _list
                };
                return Json(new { data = data }, JsonRequestBehavior.AllowGet);
            }
            //  decimal budgetedCostAmt = Convert.ToDecimal(obj.SSP_getBudgetedCostByid(Convert.ToInt32(Id)).SingleOrDefault());
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckApprovedAmount, Parameter");
                return null;
            }
        }

        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = Common.SessionManagement.SelectedBranchID;
            ProjectsBudgetViewModel objView = new ProjectsBudgetViewModel();
            Int32 projectId = 0;
            try
            {
                if (Id > 0)
                {
                    ViewBag.IsEdit = "EDIT";
                    if (User.IsInRole("SuperAdmin"))
                    {
                        projects_budget objRec = _ProjectsBudgetRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            projectId = Convert.ToInt32(objRec.project_id);
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    else
                    {
                        //receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();

                        projects_budget objRec = _ProjectsBudgetRepo.FindBy(o => o.id == Id).SingleOrDefault();

                        if (objRec != null)
                        {
                            projectId = Convert.ToInt32(objRec.project_id);
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    List<ProjectsBudget> objPBListItems = new List<ProjectsBudget>();
                    ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
                    _ProjectsBudgetView = Common.CommonFunction.GetProjectBudgetUnitandZipCode(Id);
                    _ProjectsBudgetView.UserID = uid;
                    _ProjectsBudgetView.project_budget_id = Id;
                    _ProjectsBudgetView.Project_Id = projectId;

                    objPBListItems = _IProjectsBudget.Get_Project_Budget_DetailsByIdForEditBudgetCost(_ProjectsBudgetView);
                    ViewData["ProjectID"] = projectId;
                    ViewData["project_budget_id"] = _ProjectsBudgetView.project_budget_id;
                    List<projects_budget_details> objPBList = new List<projects_budget_details>();
                    //List<projects_budget_detail> objPBRec = _ProjectsBudgetDetailRepo.FindBy(o => o.project_budget_id == Id).ToList();
                    foreach (var items in objPBListItems)
                    {
                        objPBList.Add(new projects_budget_details
                        {
                            id = Convert.ToInt64(items.id),
                            supplier_id = Convert.ToInt64(items.supplier.Supplier_id),
                            supplier_name = items.supplier.Supplier_Name,
                            project_budget_id = Convert.ToInt64(items.project_budget_id),
                            budget_amount = Convert.ToDecimal(items.budget_amount),
                            remarks = Convert.ToString(items.remarks),
                            Approved_Amount = items.Approved_amount,
                            GSTAmount = items.GSTAmount,
                            GSTPercent = Convert.ToDecimal( items.GSTPercent.Value),
                            InvoiceNumber = items.InvoiceNumber,
                            InvoiceAmountWithGST = items.InvoiceAmtWithGST
                            //salesmen_id = Convert.ToInt64(items.salesmen_id),
                        });
                        objView.ProjectSalesmenId = Convert.ToInt32(items.SalesMenId);
                    }                    
                    objView.Zip_CodeId = _ProjectsBudgetView.ZipCodeId;
                    objView.Unit_CodeId = _ProjectsBudgetView.UnitCode;                    
                    objView.projects_budget_details = objPBList;

                }
                else
                {
                    objView.project_id = 0;
                    objView.projects_budget_details = new List<Models.projects_budget_details>();
                    ViewData["ProjectID"] = 0;
                    ViewData["project_budget_id"] = 0;
                    ViewBag.IsEdit = "";
                }

                if (User.IsInRole("SuperAdmin"))
                {
                    objView.supplierList = CommonFunction.UserSupplierList("00000000-0000-0000-0000-000000000000");
                    objView.projectList = Common.CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
                }
                else
                {
                    objView.supplierList = CommonFunction.UserSupplierList(uid);
                    objView.projectList = Common.CommonFunction.UserProjectListWithID(uid, projectId);
                }
                objView.SalesmenName = CommonFunction.GetSalesmanNameBySalesmanId(objView.ProjectSalesmenId);
                objView.AddressSitName = CommonFunction.GetProjectNameByProjectId(objView.project_id);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEdit, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public ActionResult LoadAddEditNew(Int32 Id,Int32 ProjectId)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = Common.SessionManagement.SelectedBranchID;
            ProjectsBudgetViewModel objView = new ProjectsBudgetViewModel();
            try
            {
                if (Id > 0)
                {
                    ViewBag.IsEdit = "EDIT";
                    //if (User.IsInRole("SuperAdmin"))
                    //{
                    //    projects_budget_detail objRec = _ProjectsBudgetDetailRepo.FindBy(o => o.project_budget_id == Id).SingleOrDefault();
                    //    if (objRec != null)
                    //    {
                    //        projectId = Convert.ToInt32(objRec.Project_id);
                    //        Common.CommonFunction.MergeObjects(objView, objRec, true);
                    //    }
                    //}
                    //else
                    //{
                    //    //receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();

                    //    projects_budget_detail objRec = _ProjectsBudgetDetailRepo.FindBy(o => o.project_budget_id == Id).SingleOrDefault();

                    //    if (objRec != null)
                    //    {
                    //        projectId = Convert.ToInt32(objRec.Project_id);
                    //        Common.CommonFunction.MergeObjects(objView, objRec, true);
                    //    }
                    //}

                    List<ProjectsBudget> objPBListItems = new List<ProjectsBudget>();
                    ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
                    _ProjectsBudgetView = Common.CommonFunction.GetSupplierBudget(Id);
                    _ProjectsBudgetView.UserID = uid;
                    _ProjectsBudgetView.project_budget_id = Id;
                    _ProjectsBudgetView.Project_Id = ProjectId;
                    objPBListItems = _IProjectsBudget.Get_Project_Budget_DetailsByIdForEditBudgetCost(_ProjectsBudgetView);
                    ViewData["ProjectID"] = ProjectId;
                    ViewData["project_budget_id"] = _ProjectsBudgetView.project_budget_id;
                    List<projects_budget_details> objPBList = new List<projects_budget_details>();
                    //List<projects_budget_detail> objPBRec = _ProjectsBudgetDetailRepo.FindBy(o => o.project_budget_id == Id).ToList();
                    foreach (var items in objPBListItems)
                    {
                        objPBList.Add(new projects_budget_details
                        {
                            id = Convert.ToInt64(items.project_budget_details_id),
                            ProjectId = Convert.ToInt64(items.project_id),
                            salesmen_id = items.SalesMenId,
                            project_budget_id = Convert.ToInt64(items.project_budget_id),
                            budget_amount = Convert.ToDecimal(items.budget_amount),
                            remarks = Convert.ToString(items.remarks),
                            Approved_Amount = items.Approved_amount,
                            GSTAmount = items.GSTAmount,
                            GSTPercent = Convert.ToDecimal( items.GSTPercent.Value),
                            InvoiceNumber = items.InvoiceNumber,
                            InvoiceAmountWithGST = items.InvoiceAmtWithGST
                            //salesmen_id = Convert.ToInt64(items.salesmen_id),
                        });
                        //objView.ProjectSalesmenId = Convert.ToInt32(items.SalesMenId);
                    }
                    objView.Zip_CodeId = _ProjectsBudgetView.ZipCodeId;
                    objView.Unit_CodeId = _ProjectsBudgetView.UnitCode;
                    objView.supplier_id = _ProjectsBudgetView.SupplierId;
                    objView.projects_budget_details = objPBList;
                    //objView.id = Id;

                }
                else
                {
                    objView.project_id = 0;
                    objView.projects_budget_details = new List<Models.projects_budget_details>();
                    ViewData["ProjectID"] = 0;
                    ViewData["project_budget_id"] = 0;
                    ViewBag.IsEdit = "";
                }

                if (User.IsInRole("SuperAdmin"))
                {
                    objView.supplierList = CommonFunction.UserSupplierList("00000000-0000-0000-0000-000000000000");
                    //objView.projectList = Common.CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", ProjectId);
                }
                else
                {
                    objView.supplierList = CommonFunction.UserSupplierList(uid);
                    //objView.projectList = Common.CommonFunction.UserProjectListWithID(uid, ProjectId);
                }
                //objView.SalesmenName = CommonFunction.GetSalesmanNameBySalesmanId(objView.ProjectSalesmenId);
                //objView.AddressSitName = CommonFunction.GetProjectNameByProjectId(objView.project_id);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEdit, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public JsonResult SaveUpdate(ProjectsBudgetViewModel objView)
        {
            string Result = "";
            try
            {
                if (!string.IsNullOrEmpty(Request.Params["objProjectBudget"]))
                {
                    objView = ProjectsBudgetViewModel.FromJson(Request.Params["objProjectBudget"]);
                }

                string uid = User.Identity.GetUserId();
                projects_budget objProjectBudget = new projects_budget();

                if (objView.id > 0)
                {
                    objProjectBudget = _ProjectsBudgetRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objProjectBudget != null)
                    {
                        objView.created_by = objProjectBudget.created_by;
                        objView.created_date = objProjectBudget.created_date;
                        Common.CommonFunction.MergeObjects(objProjectBudget, objView, true);
                        objProjectBudget.modified_date = DateTime.Now;
                        objProjectBudget.modified_by = new Guid(uid);
                        _ProjectsBudgetRepo.Save();

                        using (Database.PMSEntities obj = new PMSEntities())
                        {
                            obj.SSP_Remove_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id));

                            foreach (var item in objView.projects_budget_details)
                            {
                                Result = obj.SSP_Add_Project_Budget_Detail(Convert.ToInt64(item.id), Convert.ToInt64(objProjectBudget.id), Convert.ToInt64(item.supplier_id), Convert.ToDecimal(item.budget_amount), Convert.ToString(item.remarks)).ToList().FirstOrDefault().Result;

                            }
                        }
                    }

                    if (Result == "2" || Result == "1")
                    {
                        return Json(new { msg = "Project budget updated successfully.", cls = "success" });
                    }
                    else if (Result == "-2")
                    {
                        return Json(new { msg = "Project budget not updated please try again.", cls = "error" });
                    }
                    else if (Result == "-3")
                    {
                        return Json(new { msg = "Supplier and Invoice Number already exists.", cls = "error" });
                    }
                    else
                    {
                        return Json(new { msg = Result, cls = "error" });
                    }
                }
                else
                {
                    Common.CommonFunction.MergeObjects(objProjectBudget, objView, true);
                    objProjectBudget.created_date = DateTime.Now;
                    objProjectBudget.created_by = new Guid(uid);
                    objProjectBudget.modified_date = DateTime.Now;
                    objProjectBudget.modified_by = new Guid(uid);
                    _ProjectsBudgetRepo.Add(objProjectBudget);
                    _ProjectsBudgetRepo.Save();

                    using (Database.PMSEntities obj = new PMSEntities())
                    {

                        obj.SSP_Remove_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id));

                        foreach (var item in objView.projects_budget_details)
                        {
                            Result = obj.SSP_Add_Project_Budget_Detail(0, Convert.ToInt64(objProjectBudget.id), Convert.ToInt64(item.supplier_id),
                                 Convert.ToDecimal(item.budget_amount), Convert.ToString(item.remarks)).ToList().FirstOrDefault().Result;
                        }

                    }


                    //return Json(new { msg = "Project budget created successfully.", cls = "success" });

                    if (Result == "1")
                    {
                        return Json(new { msg = "Project budget created successfully.", cls = "success" });
                    }
                    else if (Result == "-1")
                    {
                        return Json(new { msg = "Project budget not created please try again.", cls = "error" });
                    }
                    else if (Result == "-3")
                    {
                        return Json(new { msg = "Supplier and Invoice Number already exists.", cls = "error" });
                    }
                    else
                    {
                        return Json(new { msg = Result, cls = "error" });
                    }


                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUpdate, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }

        //public JsonResult SaveUpdate(ProjectsBudgetViewModel objView)
        //{
        //    int Result = 0;

        //    if (!string.IsNullOrEmpty(Request.Params["objProjectBudget"]))
        //    {
        //        objView = ProjectsBudgetViewModel.FromJson(Request.Params["objProjectBudget"]);
        //    }

        //    string uid = User.Identity.GetUserId();
        //    projects_budget objProjectBudget = new projects_budget();

        //    if (objView.id > 0)
        //    {
        //        objProjectBudget = _ProjectsBudgetRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
        //        if (objProjectBudget != null)
        //        {
        //            objView.created_by = objProjectBudget.created_by;
        //            objView.created_date = objProjectBudget.created_date;
        //            Common.CommonFunction.MergeObjects(objProjectBudget, objView, true);
        //            objProjectBudget.modified_date = DateTime.Now;
        //            objProjectBudget.modified_by = new Guid(uid);
        //            _ProjectsBudgetRepo.Save();

        //            using (Database.PMSEntities obj = new PMSEntities())
        //            {

        //                obj.SSP_Remove_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id));

        //                foreach (var item in objView.projects_budget_details)
        //                {
        //                    Result = obj.SSP_Add_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id), Convert.ToInt32(item.supplier_id), Convert.ToDecimal(item.budget_amount), Convert.ToString(item.remarks));
        //                }

        //            }

        //        }
        //        if (Result == 1)
        //        {
        //            return Json(new { msg = "Project budget updated successfully.", cls = "success" });
        //        }
        //        else if (Result == -1)
        //        {
        //            return Json(new { msg = "Project budget not updated please try again.", cls = "error" });
        //        }
        //        else
        //        {
        //            return Json(new { msg = "Supplier and Invoice Number already exists.", cls = "error" });
        //        }

        //    }
        //    else
        //    {
        //        Common.CommonFunction.MergeObjects(objProjectBudget, objView, true);
        //        objProjectBudget.created_date = DateTime.Now;
        //        objProjectBudget.created_by = new Guid(uid);
        //        objProjectBudget.modified_date = DateTime.Now;
        //        objProjectBudget.modified_by = new Guid(uid);
        //        _ProjectsBudgetRepo.Add(objProjectBudget);
        //        _ProjectsBudgetRepo.Save();

        //        using (Database.PMSEntities obj = new PMSEntities())
        //        {

        //            //obj.SSP_Remove_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id));

        //            foreach (var item in objView.projects_budget_details)
        //            {
        //                Result = obj.SSP_Add_Project_Budget_Detail(Convert.ToInt32(objProjectBudget.id), Convert.ToInt32(item.supplier_id),
        //                     Convert.ToDecimal(item.budget_amount), Convert.ToString(item.remarks));
        //            }

        //        }


        //        //return Json(new { msg = "Project budget created successfully.", cls = "success" });

        //        if (Result == 1)
        //        {
        //            return Json(new { msg = "Project budget created successfully.", cls = "success" });
        //        }
        //        else if (Result == -1)
        //        {
        //            return Json(new { msg = "Project budget not created please try again.", cls = "error" });
        //        }
        //        else
        //        {
        //            return Json(new { msg = "Supplier and Invoice Number already exists.", cls = "error" });
        //        }

        //    }
        //}

        //public Int32 CheckReceipt(Int32 projectId)
        //{
        //    Int32 count = 0;
        //    count = _ReceiptsRepo.FindBy(o => o.project_id == projectId).Count();
        //    count = count + 1;
        //    return count;
        //}

        public JsonResult GetAddressSiteBySaleman(Int32 salemanId)
        {
            try
            {
                List<SelectListItem> supplierList = null;
                string uid = User.Identity.GetUserId();
                Int32 projectId = 0;
                if (User.IsInRole("SuperAdmin"))
                {
                    supplierList = Common.CommonFunction.UserProjectListWithSaleman("00000000-0000-0000-0000-000000000000", projectId, salemanId).ToList();
                }
                else
                {

                    supplierList = Common.CommonFunction.UserProjectListWithSaleman(uid, projectId, salemanId).ToList();

                }

                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }

        public JsonResult FilterSupplierByAddress(Int32 AddressId)
        {
            try
            {
                List<SelectListItem> supplierList = null;
                //string uid = User.Identity.GetUserId();
                //if (User.IsInRole("SuperAdmin"))
                //{
                //    supplierList = Common.CommonFunction.UserProjectListWithSaleman(AddressId).ToList();
                //}
                //else
                //{

                supplierList = Common.CommonFunction.FilterSupplierByAddress(AddressId).ToList();

                // }

                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                return null;
            }
        }


        //public JsonResult ProjectsBudgetList(int supplier_id, string SearchString, DateTime fromdate, DateTime enddate)
        public JsonResult ProjectsBudgetList(string JsonValues)
        {
            string message = string.Empty;
            ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
            List<ProjectsBudget> _ProjectsBudgetList = new List<ProjectsBudget>();
            string uid = string.Empty;
            Int32 StatusId = 0;
            Int32 salesmanId = 0;
            Int32 SupplierId = 0;
            try
            {
                uid = User.Identity.GetUserId();
                var format = "dd/MM/yyyy";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                _ProjectsBudgetView = JsonConvert.DeserializeObject<ProjectsBudgetView>(JsonValues, dateTimeConverter);
                //_ProjectsBudgetView.SearchString = SearchString;
                _ProjectsBudgetView.branchId = Common.SessionManagement.SelectedBranchID;              
                _ProjectsBudgetView.UserID = uid;
                //_ProjectsBudgetView.SupplierId = supplier_id;
                //_ProjectsBudgetView.ProjectId = '';
               
                if (User.IsInRole("SuperAdmin") || User.IsInRole("User"))
                {
                    if (_ProjectsBudgetView.SalesmenId == 0)
                    {
                        _ProjectsBudgetView.SalesmenId = salesmanId;
                    }
                }
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = CommonFunction.GetSalesmenIdByUserId(uid);
                        for (int i = 0; i < salesManDtls.Count; i++)
                        {
                            salesmanId = Convert.ToInt32(salesManDtls[0].Value);
                        }
                        _ProjectsBudgetView.SalesmenId = salesmanId;
                    }
                }
                if (User.IsInRole("Supplier"))
                {
                    List<SelectListItem> SupplierList = new List<SelectListItem>();
                    SupplierList = CommonFunction.GetSupplierIdByUserId(uid);
                    for (int i = 0; i < SupplierList.Count; i++)
                    {
                        SupplierId = Convert.ToInt32(SupplierList[0].Value);
                    }
                    _ProjectsBudgetView.SupplierId = SupplierId;
                    _ProjectsBudgetView.branchId = 0;

                }
                StatusId = Convert.ToInt32(_ProjectsBudgetView.StatusId);
                _ProjectsBudgetList = _IProjectsBudget.ProjectsbudgetList(_ProjectsBudgetView).ToList();               
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ProjectsBudgetList");
                message = ex.Message;
            }
            finally
            {
                _ProjectsBudgetView = null;
            }
            var data = new
            {
                Items = _ProjectsBudgetList,
                StatusId = StatusId,
                SupplierId = SupplierId,
                TotalCount = _ProjectsBudgetList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddBudgetDetails(string JsonBudget, string SupplierId , string project_id ,int ZipCodeId,string UnitCodeId)
        {
            var user = User.Identity.Name.ToString();
            SuccessMessage _SuccessMessage = new SuccessMessage();
            Signature List = new Signature();
            AddBudget _AddBudget = JsonConvert.DeserializeObject<AddBudget>(JsonBudget);
            string uid = string.Empty;
            String path = string.Empty;
            string extension = string.Empty;
            string ProjectId = string.Empty;
            string fileNameWithExt = string.Empty;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();

            
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    fileNameWithExt = fileName;
                    string doc_Path = "/UploadDocuments/" + "/ProjectBudget/" + project_id + "/";
                    string dirPath = PMS.Common.Constants.PhysicalPath + doc_Path;

                    string UploadPath = "~/UploadDocuments/ProjectBudget/" + project_id;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string addTimeStampToFileName = string.Empty;
                    //if (file.ContentLength == 0)
                    //   continue;
                    if (file.ContentLength > 0)
                    {
                        addTimeStampToFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName),DateTime.Now.ToString("yyyyMMddHHmmssfff"),Path.GetExtension(file.FileName));
                        path = Path.Combine(HttpContext.Request.MapPath(UploadPath), addTimeStampToFileName);
                        extension = Path.GetExtension(file.FileName);

                        file.SaveAs(path);
                    }

                    ProjectId = _IProjectsBudget.GetProjectIdById(project_id);

                    if (!string.IsNullOrEmpty(ProjectId))
                    {
                        List.SuperId = ProjectId;
                        List.DOCUMENT_NAME = "UploadFile";
                        List.ID = "11";
                        List.ID_TYPE = "10";
                        List.SalesmenImage_PATH = doc_Path + addTimeStampToFileName;
                        List.FILE_TYPE = extension;


                        if (_AddBudget.project_budget_details_id > 0)
                        {
                            bool IsSupplierCreated = false;
                            if (User.IsInRole("Supplier"))
                                IsSupplierCreated = true;
                            uid = User.Identity.GetUserId();
                            _AddBudget.SupplierId = SupplierId;
                            _AddBudget.UserId = uid;
                            _AddBudget.Project_Id = project_id;
                            _SuccessMessage = _IProjectsBudget.AddBudgetDetails(_AddBudget, IsSupplierCreated);
                            ViewData["ProjectID"] = project_id;
                            ViewData["project_budget_id"] = _SuccessMessage.project_budget_id;
                            ViewData["Project_Budget_detail_Id"] = _SuccessMessage.Id;
                            ViewBag.Project_Budget_Id = _SuccessMessage.project_budget_id;
                            ViewBag.Project_Budget_detail_Id = _SuccessMessage.Id;
                        }
                        else
                        {
                            //if (_SuccessMessage.Result == "1")
                            //{
                            bool IsSupplierCreated = false;
                            if (User.IsInRole("Supplier"))
                                IsSupplierCreated = true;
                            uid = User.Identity.GetUserId();
                            _AddBudget.SupplierId = SupplierId;
                            _AddBudget.UserId = uid;
                            _AddBudget.Project_Id = project_id;
                            _AddBudget.Zip_CodeId = ZipCodeId;
                            _AddBudget.Unit_CodeId = UnitCodeId;
                            _SuccessMessage = _IProjectsBudget.AddBudgetDetails(_AddBudget, IsSupplierCreated);
                            ViewData["ProjectID"] = project_id;
                            ViewData["project_budget_id"] = _SuccessMessage.project_budget_id;
                            ViewData["Project_Budget_detail_Id"] = _SuccessMessage.Id;
                            ViewBag.Project_Budget_Id = _SuccessMessage.project_budget_id;
                            ViewBag.Project_Budget_detail_Id = _SuccessMessage.Id;
                            //}
                        }
                        _SuccessMessage = _IProjectsBudget.UpsertSignatureForProjectBudget(List, uid, _SuccessMessage, User.Identity.GetUserName());
                        string projectNumber = _IProjectsBudget.GetProjectNumberIdById(project_id, "projectId");
                        string Project_Id = ProjectId.ToString();
                        _SuccessMessage.project_budget_id = ViewBag.Project_Budget_Id;
                        _SuccessMessage.Id = ViewBag.Project_Budget_detail_Id;
                        List<SupplierAddress> addresses = _IProjectsBudget.GetSupplierAddressById(Convert.ToInt32(_AddBudget.SupplierId));
                        if (User.IsInRole("Salesman"))
                        {
                            int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                            emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(_AddBudget.SalesMenId, Project_Id, Common.Constants.GetTransaction.CreateBudgetCost);
                        }
                        else
                            emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(_AddBudget.Salesman.id.ToString(), Project_Id, Common.Constants.GetTransaction.CreateBudgetCost);
                        string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                        string supplierAddress = string.Join(",",
                            new[] { addresses[0].Address1, addresses[0].Address2, addresses[0].Address3, addresses[0].Address4, addresses[0].ZipCode }.Where(s => !string.IsNullOrWhiteSpace(s)));
                        SendEmail(emailIds, "[Test]-Budget cost has been created for ", _AddBudget.SupplierId, path, fileNameWithExt,  addresses[0].Address1, projectNumber, "created");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: AddBudgetDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                //  _AddBudget = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_ForApproval(int id, string Status_Id, decimal Approved_Amount)
        {
            ProjectsBudget _ProjectsBudgetView = new ProjectsBudget();
            SuccessMessage _successMessage = new SuccessMessage();
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string ProjectId = _IProjectsBudget.GetProjectNumberIdByBudgetId(id);
            int Procedure = 0;
            try
            {
                _ProjectsBudgetView.project_budget_details_id = id;
                _ProjectsBudgetView.status_id = Status_Id;
                _ProjectsBudgetView.Approved_amount = Approved_Amount;
                _successMessage = _IProjectsBudget.Update_ForApproval(_ProjectsBudgetView);

                if (Convert.ToInt32(Status_Id) == 7)
                    Procedure = Common.Constants.GetTransaction.VerifyBudgetCost;
                else if (Convert.ToInt32(Status_Id) == 2)
                    Procedure = Common.Constants.GetTransaction.ApproveBudgetCost;
                else
                    Procedure = Common.Constants.GetTransaction.CreateBudgetCost;
                //Email to Admin and Salesmen
                SalesmenAndSupplierDetails salesmenAndSupplierDetails = _IProjectsBudget.GetSalesmenAndSupplierDetailsByProjectBudgetDetailsId(id);


                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(salesmenAndSupplierDetails.SalesmenId,ProjectId, Procedure);
                }
                else
                    emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(salesmenAndSupplierDetails.SalesmenId, ProjectId, Procedure);
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                string supplierAddress = string.Join(",",
                    new[] { salesmenAndSupplierDetails.Supplier.Address1, salesmenAndSupplierDetails.Supplier.Address2, salesmenAndSupplierDetails.Supplier.Address3, salesmenAndSupplierDetails.Supplier.Address4, salesmenAndSupplierDetails.Supplier.ZipCode }.Where(s => !string.IsNullOrWhiteSpace(s)));
                string[] splitPath = salesmenAndSupplierDetails.DocPath != null ? salesmenAndSupplierDetails.DocPath.Split('/') : null;
                string fileNameWithExt = splitPath != null ? splitPath.Last() : null;
                if (emailIds.Length > 0)
                {
                    SendEmail(emailIds, "[Test]-Budget cost has been approved for ", salesmenAndSupplierDetails.Supplier.SupplierName, salesmenAndSupplierDetails.DocPath, fileNameWithExt, BuildFullAddress(salesmenAndSupplierDetails.Supplier), salesmenAndSupplierDetails.ProjectNumber, "approved");
                }
                return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                _successMessage.Result = "0";
                _successMessage.Errormessage = "Unhandled Exception at Update Process";
                ExceptionLog.WriteLog(ex, "Method Name: Update_ForApproval");                
                return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                _ProjectsBudgetView = null;
            }

           
        }

        public JsonResult Update_ForApprovalWithReason(int id, string Status_Id, decimal Approved_Amount, string reason)
        {
            ProjectsBudget _ProjectsBudgetView = new ProjectsBudget();
            SuccessMessage _successMessage = new SuccessMessage();
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string project_Id = _IProjectsBudget.GetProjectNumberIdByBudgetId(id);
            int Procedure = 0;
            try
            {
                _ProjectsBudgetView.project_budget_details_id = id;
                _ProjectsBudgetView.status_id = Status_Id;
                _ProjectsBudgetView.Approved_amount = Approved_Amount;
                _ProjectsBudgetView.reason = reason;
                _successMessage = _IProjectsBudget.Update_ForApprovalWithReason(_ProjectsBudgetView);

                if (Convert.ToInt32(Status_Id) == 7)
                    Procedure = Common.Constants.GetTransaction.VerifyBudgetCost;
                else if (Convert.ToInt32(Status_Id) == 2)
                    Procedure = Common.Constants.GetTransaction.ApproveBudgetCost;
                else
                    Procedure = Common.Constants.GetTransaction.CreateBudgetCost;
                //Email to Admin and Salesmen
                SalesmenAndSupplierDetails salesmenAndSupplierDetails = _IProjectsBudget.GetSalesmenAndSupplierDetailsByProjectBudgetDetailsId(id);
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(salesmenAndSupplierDetails.SalesmenId, project_Id, Procedure);
                }
                else
                    emailAddress = _IProjectsBudget.GetAdminAndSalesmenEmailAddress(salesmenAndSupplierDetails.SalesmenId, project_Id, Procedure);
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                string supplierAddress = string.Join(",",
                    new[] { salesmenAndSupplierDetails.Supplier.Address1, salesmenAndSupplierDetails.Supplier.Address2, salesmenAndSupplierDetails.Supplier.Address3, salesmenAndSupplierDetails.Supplier.Address4, salesmenAndSupplierDetails.Supplier.ZipCode }.Where(s => !string.IsNullOrWhiteSpace(s)));
                string[] splitPath = salesmenAndSupplierDetails.DocPath != null ? salesmenAndSupplierDetails.DocPath.Split('/') : null;
                string fileNameWithExt = splitPath != null ? splitPath.Last() : null;
                if (emailIds.Length > 0)
                {
                    SendEmail(emailIds, "[Test]-Budget cost has been approved with Reason for ", salesmenAndSupplierDetails.Supplier.SupplierName, salesmenAndSupplierDetails.DocPath, fileNameWithExt, BuildFullAddress(salesmenAndSupplierDetails.Supplier), salesmenAndSupplierDetails.ProjectNumber, "approved");
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_ForApprovalWithReason");
            }
            finally
            {
                _ProjectsBudgetView = null;
            }

            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Project_BudgetById(int project_budget_id)
        {
            ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
            List<ProjectsBudget> _ProjectsBudgetList = new List<ProjectsBudget>();
            string uid = string.Empty;
            try
            {
                uid = User.Identity.GetUserId();
                _ProjectsBudgetView.UserID = uid;
                _ProjectsBudgetView.project_budget_id = project_budget_id;
                _ProjectsBudgetList = _IProjectsBudget.Get_Project_BudgetById(_ProjectsBudgetView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_BudgetById");
            }
            finally
            {
                _ProjectsBudgetView = null;
            }

            return Json(new { data = _ProjectsBudgetList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_Project_Budget_DetailsById(int project_budget_id, int project_id)
        {
            ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
            List<ProjectsBudget> _ProjectsBudgetList = new List<ProjectsBudget>();
            string uid = string.Empty;
            try
            {
                uid = User.Identity.GetUserId();
                _ProjectsBudgetView.UserID = uid;
                _ProjectsBudgetView.project_budget_id = project_budget_id;
                _ProjectsBudgetView.Project_Id = project_id;
                if(project_budget_id !=0 && project_id !=0)
                {
                    _ProjectsBudgetList = _IProjectsBudget.Get_Project_Budget_DetailsByIdForEditBudgetCost(_ProjectsBudgetView);
                }
                else
                {
                    _ProjectsBudgetList = _IProjectsBudget.Get_Project_Budget_DetailsById(_ProjectsBudgetView);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsById");
            }
            finally
            {
                _ProjectsBudgetView = null;
            }
            var data = new
            {
                Items = _ProjectsBudgetList,
                TotalCount = _ProjectsBudgetList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Get_Project_Budget_DetailsByIdNew(int project_budget_id, int project_id)
        {
            ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
            List<ProjectsBudget> _ProjectsBudgetList = new List<ProjectsBudget>();
            string uid = string.Empty;
            try
            {
                uid = User.Identity.GetUserId();
                _ProjectsBudgetView = Common.CommonFunction.GetSupplierBudget(project_budget_id);
                _ProjectsBudgetView.UserID = uid;
                _ProjectsBudgetView.project_budget_id = project_budget_id;
                _ProjectsBudgetView.Project_Id = project_id;

                _ProjectsBudgetList = _IProjectsBudget.Get_Project_Budget_DetailsByIdForEditBudgetCost(_ProjectsBudgetView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsByIdNew");
            }
            finally
            {
                _ProjectsBudgetView = null;
            }
            var data = new
            {
                Items = _ProjectsBudgetList,
                TotalCount = _ProjectsBudgetList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult LoadAddEdit(Int32 Id)
        //{
        //    string uid = User.Identity.GetUserId();
        //    Int32 branchid = Common.SessionManagement.SelectedBranchID;
        //    ProjectsBudget objView = new ProjectsBudget();
        //    ProjectsBudgetView _ProjectsBudgetView = new ProjectsBudgetView();
        //    List<ProjectsBudget> _ProjectsBudgetList = new List<ProjectsBudget>();
        //    try
        //    {
        //        if (Id > 0)
        //        {
        //            uid = User.Identity.GetUserId();
        //            _ProjectsBudgetView.UserID = uid;
        //            //_ProjectsBudgetView.project_budget_id = Id;
        //            //_ProjectsBudgetList = _IProjectsBudget.Get_Project_Budget_DetailsById(_ProjectsBudgetView);

        //            _ProjectsBudgetView.project_budget_id = Id;
        //            _ProjectsBudgetList = _IProjectsBudget.Get_Project_BudgetById(_ProjectsBudgetView);
        //            objView.project_id = _ProjectsBudgetList[0].project_id;
        //            objView.SalesMenId = _ProjectsBudgetList[0].SalesMenId;
        //            objView.salesmen_name = _ProjectsBudgetList[0].salesmen_name;
        //            objView.project_budget_details_id = _ProjectsBudgetList[0].project_budget_details_id;
        //            //objView.project_budget_id = Id;
        //        }
        //        else
        //        {
        //            //_ProjectsBudgetList[0].project_budget_id = Id;
        //        }
        //        return View(objView);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEdit, Parameter : Id={Id}");
        //        return null;
        //    }
        //    finally
        //    {
        //        _ProjectsBudgetList = null;
        //    }
        //}
        [HttpPost]
        public JsonResult GetFilePathByProjectBudgetDetailsId(Int64 ProjectId, int ProjectBudgetDetailsId)
        {
            string path = string.Empty;
            try
            {
                string projectId = _IProjectsBudget.GetProjectIdById(ProjectId.ToString());
                List<GetDocumentdetails> getDocs = Get_DocumentsForProjectBudget(ProjectBudgetDetailsId);
                if(getDocs.Count > 0)
                {
                    foreach(var item in getDocs)
                    {
                        path = item.document_path;
                    }
                }
                else
                {
                    path = string.Empty;
                }

                //path = _IProjectsBudget.GetFilePathById(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetFilePathById");
            }
            finally
            {

            }
            return Json(new { data = path }, JsonRequestBehavior.AllowGet);
        }

        public List<GetDocumentdetails> Get_DocumentsForProjectBudget(int ProjectBudgetDetailsId)
        {
            List<QuotationDetails> _GetDocumentList;
            List<GetDocumentdetails> _GetDocumentdetails = new List<GetDocumentdetails>();

            try
            {

                _GetDocumentList = new List<QuotationDetails>();
                _GetDocumentList = _IProjectsBudget.Get_DocumentsForProjectBudget(ProjectBudgetDetailsId);
                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "11")
                        {
                            GetDocumentdetails _GetDocumentpath = new GetDocumentdetails();
                            _GetDocumentpath.document_path = _GetDocumentList[i].document_path;
                            _GetDocumentdetails.Add(_GetDocumentpath);
                        }

                    }

                }
                else
                {
                }

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Documents");
            }
            finally
            {
                _GetDocumentList = null;
            }
            return _GetDocumentdetails;
        }

        [HttpPost]
        public JsonResult Remove_Project_Budget(string project_budget_id)
        {
            string message = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IProjectsBudget.Remove_Project_Budget(project_budget_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Remove_Project_Budget_Detail");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Remove_Project_Budget_Detail(string project_budget_detail_Id)
        {
            string message = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IProjectsBudget.Remove_Project_Budget_Detail(project_budget_detail_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Remove_Project_Budget_Detail");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public void SendEmail(string emailID, string subject, string supplierName, string path, string fileNameWithExt, string fullAddress, string projectNumber, string mailType)
        {
            try
            {
                EMailInfo emaildtls = new EMailInfo();
                emaildtls.ToMail = emailID;
                emaildtls.CCMail = "ismsqateam@gmail.com";
                //emaildtls.ToMail = "ismsqateam@gmail.com";
                emaildtls.Subject = subject + supplierName;
                emaildtls.Body = PrepareEmailBody(supplierName, fileNameWithExt, fullAddress, projectNumber, mailType);
                emaildtls.DisplayName = "Administrator";
                emaildtls.AttachmentPath = path;
                emaildtls.FileName = fileNameWithExt;
                Mail.SendMail(emaildtls);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string PrepareEmailBody(string supplierName, string fileNameWithExt, string fullAddress, string projectNumber, string mailType)
        {
            return "<table style='margin-right: calc(15%); width: 85%;'><tbody>" 
                +"<tr> <td style='width: 25%;'>Date</td><td style='width: 75%;'>: "+ DateTime.Now.ToString("d") +"</td></tr>"
                +"<tr> <td style='width: 25%;'>To</td><td style='width: 75%;'>: "+ supplierName.ToString() + "</td></tr>"
                +"<tr> <td style='width: 25%;'>Address</td><td style='width: 75%;'>: "+ fullAddress + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Project Number</td><td style='width: 75%;'>: " + projectNumber + "</td></tr>"
                + "<tr> <td style='width: 25%;'>RE</td><td style='width: 75%;'>: New Budgeted Cost is " + mailType + " for Supplier Invoice.</td></tr></tbody></table>"
                + "<br><div>Login and Approve using the Below Link:</div><div>"
                + "<a href='http://118.201.113.216:38080/ProjectsBudget/#'>http://118.201.113.216:38080/ProjectsBudget/#</a></div>"
                + "<br><div>Budget Cost PDF Attachment :</div><div>"+ fileNameWithExt + "</div><div><br></div><div>Thank you</div><br>"
                + "<div>REGARDS,</div><div> Design 4 Space, "+ @PMS.Common.SessionManagement.SelectedBranchName + " </div>";
        }

        public string BuildFullAddress(SupplierAddress supplierAddress)
        {
            string fullAddress = string.Empty;
            if (!string.IsNullOrEmpty(supplierAddress.Address1))
            {
                fullAddress = supplierAddress.Address1.ToString();
            }
            else if (!string.IsNullOrEmpty(supplierAddress.Address2))
            {
                fullAddress = ", " + supplierAddress.Address2.ToString();
            }
            else if (!string.IsNullOrEmpty(supplierAddress.Address3))
            {
                fullAddress = ", " + supplierAddress.Address3.ToString();
            }
            else if (!string.IsNullOrEmpty(supplierAddress.Address4))
            {
                fullAddress = ", " + supplierAddress.Address4.ToString();
            }
            else if (!string.IsNullOrEmpty(supplierAddress.ZipCode))
            {
                fullAddress = ", " + supplierAddress.ZipCode.ToString();
            }
            return fullAddress;
        }
    }

    public class suppPaymentsTest
    {
        public int project_budget_details_id { get; set; }
        public int Address { get; set; }
        public int Invoice { get; set; }
        public int InvoiceAmtWithGST { get; set; }
        public int GSTPercent { get; set; }
        public int GSTAmount { get; set; }
        public int budget_amount { get; set; }
        public int StatusId { get; set; }
    }
}

