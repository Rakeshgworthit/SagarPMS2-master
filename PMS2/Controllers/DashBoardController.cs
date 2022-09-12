using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using PMS.Common;
using PMS.Data_Access;
using PMS.Interface;
using PMS.Models;

namespace PMS.Controllers
{
    [Authorize]
    public class DashBoardController  : Controller
    {
        // GET: DashBoard
        private IDashboard _IDashboard;
        private List<SelectListItem> branchList;

        public ActionResult Index(ReportViewModel objView)
        {

            return View();
        }

        public ActionResult SaleSummaryReport(ReportViewModel objView)
        {

            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    //   branchList = Common.CommonFunction.BranchList();
                }
                else
                {
                    //  branchList = Common.CommonFunction.UserBranchList(uid);
                }
                objView.Uid = uid;

                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                //objView.BranchList = branchList;

                objView.YearList = Common.CommonFunction.YearList();

                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                }
                //ExportSaleSummaryReport(objView, true);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaleSummaryReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public JsonResult GetTop10Salesman(int year, int BranchID,int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            List<Top10Salesman> _top10salesman = new List<Top10Salesman>();
            try
            {
                if (User.IsInRole("Salesman"))
                {                   
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        string uid = User.Identity.GetUserId();
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }                
                _top10salesman = obj.GetTop10Salesman(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10Salesman");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_top10salesman, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopTenCustomer(int year, int BranchID, int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            string uid = User.Identity.GetUserId();
            List<Top10Customer> _top10customer = new List<Top10Customer>();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    if (!CommonFunction.GetIsBranchAdmin(uid,BranchID))
                    {                        
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }
                _top10customer = obj.GetTopTenCustomer(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10customer");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_top10customer, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopTenProjects(int year, int BranchID, int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            string uid = User.Identity.GetUserId();
            List<Top10Projects> _top10projects = new List<Top10Projects>();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    if (!CommonFunction.GetIsBranchAdmin(uid, BranchID))
                    {
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }
                _top10projects = obj.GetTopTenProjects(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10projects");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_top10projects, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopTenSupplier(int year, int BranchID, int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            List<Top10Supplier> _top10supplier = new List<Top10Supplier>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    if (!CommonFunction.GetIsBranchAdmin(uid, BranchID))
                    {
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }
                _top10supplier = obj.GetTopTenSupplier(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10supplier");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_top10supplier, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailsBasedonYear(int year, int BranchID, int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            List<Dashboard> _Dashboard = new List<Dashboard>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    if (!CommonFunction.GetIsBranchAdmin(uid, BranchID))
                    {
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }
                _Dashboard = obj.GetDetailsBasedonYear(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10supplier");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_Dashboard, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTopTenLoan(int year, int BranchID, int SalesmanId = 0)
        {
            string message = string.Empty;
            DataLayer obj = new DataLayer();
            List<GetTopTenLoan> _top10loans = new List<GetTopTenLoan>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    if (!CommonFunction.GetIsBranchAdmin(uid, BranchID))
                    {
                        List<SelectListItem> salesManDtls = new List<SelectListItem>();
                        salesManDtls = Common.CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmanId = Convert.ToInt32(salesManDtls[0].Value);
                    }
                    else
                    {
                        SalesmanId = 0;
                    }
                }
                _top10loans = obj.GetTopTenLoan(year, SalesmanId, BranchID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTop10loan");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            return Json(_top10loans, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AccountPurchaseDashboard()
        {
            return View();
        }

        public ActionResult AccountSalesDashboard(ReportViewModel objView)
        {          
            //ReportViewModel objView = new ReportViewModel();          
            if (User.IsInRole("SuperAdmin"))
            {                
                objView.Uid = "00000000-0000-0000-0000-000000000000";
                branchList = Common.CommonFunction.BranchList();
            }
            else
            {
                objView.Uid = User.Identity.GetUserId();
                branchList = Common.CommonFunction.UserBranchList(objView.Uid);


            }

            objView.BranchList = branchList;
            if (objView.ProjectStatusId == null)
            {
                objView.ProjectStatusId = 2;
            }
            if (objView.BranchId == 0)
            {
                objView.BranchId = SessionManagement.SelectedBranchID;
            }
            DateTime now = DateTime.Now;
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-5);
            }
            else
            {
                objView.from_date = objView.from_date;
            }
            if (objView.to_date == null)
            {
                objView.to_date = endDate;
            }
            else
            {
                objView.to_date = objView.to_date;
            }
            objView.SalenmenList = CommonFunction.SalesmenListByBranch(objView.BranchId);           
            return View(objView);
        }

        public JsonResult ProjectsSummaryReport()
        {
            var data = _IDashboard.GetProjectsSummaryReport();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetailsBasedonBranch(int branch)
        {
            Dashboard dashboard = _IDashboard.GetDetailsBasedonBranch(branch);
            return Json(dashboard, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSalesmanListByBranch(int BranchId)
        {
            string message = string.Empty;
            List<SelectListItem> _BindSalesmen = new List<SelectListItem>();
            int SalesmenId = 0;
            bool bIsBranchAdmin = false;
            try
            {
                string uid = User.Identity.GetUserId();
                bIsBranchAdmin = CommonFunction.GetIsBranchAdmin(uid, BranchId);
                
                if (!bIsBranchAdmin)
                {
                    if (User.IsInRole("Salesman"))
                    {
                        _BindSalesmen = CommonFunction.GetSalesmenIdByUserId(uid);
                        SalesmenId = Common.CommonFunction.SalesmenIDByUserID(uid);
                    }
                    else
                    {
                        _BindSalesmen = CommonFunction.SalesmenListByBranch(BranchId);
                    }
                }
                else
                {
                    _BindSalesmen = CommonFunction.SalesmenListByBranch(BranchId);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmanListByBranch");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindSalesmen , id = SalesmenId ,IsBranchAdmin = bIsBranchAdmin }, JsonRequestBehavior.AllowGet);
        }
    }
}