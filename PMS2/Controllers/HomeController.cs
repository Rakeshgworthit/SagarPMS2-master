using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMS.Common;
using PMS.Data_Access;
using PMS.Database;
using PMS.Models;

namespace PMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private List<SelectListItem> branchList;
        public ActionResult Index(string ReturnUrl)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                string uid = User.Identity.GetUserId();

                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    branchList = Common.CommonFunction.BranchList();
                }
                else
                {
                    branchList = Common.CommonFunction.UserBranchListNew(uid);
                }

                HomeViewModel objView = new HomeViewModel()
                {
                    BranchList = branchList,
                    ReturnUrl = ReturnUrl
                };

                if (branchList.Count == 2)
                {
                    objView.BranchID = Convert.ToInt32(branchList[1].Value);
                }

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Index , Parameter : ReturnUrl={ReturnUrl}");
                return null;
            }
            finally
            {
                branchList = null;
            }

        }

        //public JsonResult GetTop10Salesman()
        //{
        //    string message = string.Empty;
        //    DataLayer obj = new DataLayer();
        //    List<Top10Salesman> _top10salesman = new List<Top10Salesman>();
        //    try
        //    {
        //        _top10salesman = obj.GetTop10Salesman();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: GetTop10Salesman");
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //        //ObjDB = null;
        //    }
        //    return Json(_top10salesman, JsonRequestBehavior.AllowGet);
        //}    

        public ActionResult SetBranch(HomeViewModel objView)
        {
            try
            {
                string uid = User.Identity.GetUserId();
                Common.SessionManagement.SelectedBranchID = objView.BranchID;

                //using (Database.PMSEntities objDB = new Database.PMSEntities())
                //{
                //    Common.SessionManagement.SelectedBranchName = (from b in objDB.branches
                //                                                   join c in objDB.companies on b.company_id equals c.id
                //                                                   where b.id == objView.BranchID
                //                                                   select b.branch_name + ", " + c.company_name 
                //                                                  ).SingleOrDefault();
                //}

                ////Repository.Infrastructure.IBranchesRepositor _objRepo = new Repository.DataService.BranchesService();
                ////Common.SessionManagement.SelectedBranchName = _objRepo.FindBy(o => o.id == objView.BranchID).Select(o => o.branch_name).SingleOrDefault();
                Repository.Infrastructure.IBranchesRepositor _objRepo = new Repository.DataService.BranchesService();
                var objBranch = _objRepo.FindBy(o => o.id == objView.BranchID).SingleOrDefault();
                Common.SessionManagement.SelectedBranchName = Convert.ToString(objBranch.branch_name);
                Common.SessionManagement.IsBranchAdmin = CommonFunction.GetIsBranchAdmin(uid, objView.BranchID);

                //if (Convert.ToString(objBranch.gst_reg_no) == "" || objBranch.gst_reg_no == null)
                //{
                //    Common.SessionManagement.BranchGST = Common.Constants.DefaultZeroGstBranchGST;
                //}
                //else
                //{
                //    Common.SessionManagement.BranchGST = Common.Constants.DefaultBranchGST;
                //}

                Common.SessionManagement.BranchGST = Common.Constants.DefaultBranchGST;

                if (!string.IsNullOrEmpty(objView.ReturnUrl))
                {
                    return RedirectToAction("Index", "Projects");
                }
                else
                {
                    //return RedirectToAction("Index", "Projects");
                    if (User.IsInRole("SuperAdmin") || User.IsInRole("User"))
                    {
                        return RedirectToAction("AccountSalesDashboard", "DashBoard");
                    }
                    else if (User.IsInRole("Salesman"))
                    {
                        return RedirectToAction("SalesDashBoard", "Home");
                        //return RedirectToAction("AccountSalesDashboard", "DashBoard");
                    }
                    else
                    {
                        return RedirectToAction("SalesDashBoard", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SetBranch, Parameter : objView={objView}");
                return null;
            }
            finally
            {

            }
        }


        public void test2()
        {
            try
            {
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=Print.xls");
                Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                Response.Write("<head>");
                Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                Response.Write("<!--[if gte mso 9]><xml>");
                Response.Write("<x:ExcelWorkbook>");
                Response.Write("<x:ExcelWorksheets>");
                Response.Write("<x:ExcelWorksheet>");
                Response.Write("<x:Name>Report Data</x:Name>");
                Response.Write("<x:WorksheetOptions>");
                Response.Write("<x:Print>");
                Response.Write("<x:ValidPrinterInfo/>");
                Response.Write("</x:Print>");
                Response.Write("</x:WorksheetOptions>");
                Response.Write("</x:ExcelWorksheet>");
                Response.Write("</x:ExcelWorksheets>");
                Response.Write("</x:ExcelWorkbook>");
                Response.Write("</xml>");
                Response.Write("<![endif]--> ");
                Response.Write("<html><body><table><tr><td><table><tr><td style='background-color:#d2d2d2;'>Date</td><td>Date 2</td></tr><tr><td>Row 1</td><td>Row 2</td></tr><tr><td>Row 3</td><td>Row 4</td></tr></table></td><td><table><tr><td>Date 4</td><td>Date 5</td></tr><tr><td>Row 1</td><td>Row 2</td></tr><tr><td>Row 3</td><td>Row 4</td></tr></table></td></tr></table></body></html>"); // give ur html string here
                Response.Write("</head>");
                Response.Flush();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: test2");
            }
            finally
            {

            }
        }

        public JsonResult GetBranchList(string code)
        {
            List<SelectListItem> searchList = Common.CommonFunction.BranchListByCode(code == null ? "" : code);
            List<test> branchList = new List<test>();
            try
            {

                foreach (var items in searchList)
                {
                    branchList.Add(new test { id = items.Value, label = items.Text, value = items.Text });
                }
                return Json(branchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                ExceptionLog.WriteLog(ex, $"Method Name: GetBranchList, Parameter : code={code}");
                return null;
            }
            finally
            {
                searchList = null;
                branchList = null;
            }
        }

        public class test
        {
            public string id { get; set; }
            public string label { get; set; }
            public string value { get; set; }
        }


        public JsonResult GetCustomerList()
        {
            List<SelectListItem> customerList = Common.CommonFunction.CustomerListByBranchId();

            try
            {
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                ExceptionLog.WriteLog(ex, $"Method Name: GetCustomerList");
                return null;
            }
            finally
            {
                customerList = null;
            }
        }

        public JsonResult GetSupplierList()
        {
            List<SelectListItem> customerList = Common.CommonFunction.UserSupplierList("");

            try
            {
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                ExceptionLog.WriteLog(ex, $"Method Name: GetSupplierList");
                return null;
            }
            finally
            {
                customerList = null;
            }
        }

        public JsonResult GetProjectListByYear(Int32 year, Int32 salesmenId)
        {
            List<SelectListItem> customerList = new List<SelectListItem>();

            try
            {
                string uid = User.Identity.GetUserId();
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "00000000-0000-0000-0000-000000000000";
                }
                customerList = Common.CommonFunction.UserProjectListByYear(uid.ToString(), year, salesmenId);
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectListByYear, Parameter : year={year},salesmenId={salesmenId}");
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                customerList = null;
            }
        }

        public JsonResult GetSupplierListByCode(string str)
        {
            List<SelectListItem> supplierList = Common.CommonFunction.UserSupplierList("").ToList();

            try
            {
                if (str != "0")
                {
                    if (str == "1")
                    {
                        supplierList = supplierList.Where(o => (o.Text.StartsWith("0") || o.Text.StartsWith("1") || o.Text.StartsWith("2")
                        || o.Text.StartsWith("3") || o.Text.StartsWith("4") || o.Text.StartsWith("5") || o.Text.StartsWith("6") || o.Text.StartsWith("7")
                        || o.Text.StartsWith("8") || o.Text.StartsWith("9"))).ToList();


                    }
                    else
                    {
                        supplierList = supplierList.Where(o => o.Text.StartsWith(str)).ToList();
                    }


                    supplierList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                }
                return Json(supplierList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //LogException(ex, "");
                ExceptionLog.WriteLog(ex, $"Method Name: GetSupplierListByCode, Parameter : str={str}");
                return null;
            }
            finally
            {
                supplierList = null;
            }

        }

        public ActionResult SalesDashBoard(ReportViewModel objView)
        {
            if (objView.BranchId == 0)
            {
                objView.BranchId = SessionManagement.SelectedBranchID;
            }
            if (User.IsInRole("SuperAdmin"))
            {
                objView.Uid = "00000000-0000-0000-0000-000000000000";
                objView.SalenmenList = CommonFunction.SalesmenListByBranch(objView.BranchId);
            }
            else
            {

                objView.Uid = User.Identity.GetUserId();
                if (!CommonFunction.GetIsBranchAdmin(objView.Uid,objView.BranchId))
                {
                    objView.SalesmenId = Common.CommonFunction.SalesmenIDByUserID(objView.Uid);
                    objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                    objView.ProjectSalesmenId = objView.SalesmenId;
                }
                else
                {
                    objView.SalenmenList = CommonFunction.SalesmenListByBranch(objView.BranchId);
                }
                branchList = Common.CommonFunction.UserBranchList(objView.Uid);
                
            }
            ViewBag.IsBranchAdmin  = CommonFunction.GetIsBranchAdmin(objView.Uid, objView.BranchId);           
            objView.BranchList = branchList;
            if (objView.ProjectStatusId == null)
            {
                objView.ProjectStatusId = 2;
            }
           
            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.from_date == null)
            {
                objView.from_date = CurrentstartDate.AddMonths(-6);
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
            if (!User.IsInRole("Supplier"))
                return View(objView);
            else
                return RedirectToAction("Index", "ProjectsBudget");
        }

        public ActionResult AdminDashBoard()
        {
            return View();
        }
        public ActionResult SalesMenGenerateLogin()
        {
            SalesMenUserMapping slmusrMap = new SalesMenUserMapping();
            List<SelectListItem> objusrsList = new List<SelectListItem>();

          
            using (PMSEntities objDB = new PMSEntities())
            {
                var usrsList = (from u in objDB.user_detail 
                                join a in objDB.user_access_rights on u.user_id equals a.user_id
                                where u.is_active == true && a.branch_id == SessionManagement.SelectedBranchID
                                select new SelectListItem
                                {
                                    Text = u.Name,
                                    Value = u.did.ToString()
                                }).ToList();
                foreach (var items in usrsList.OrderBy(o => o.Text))
                {
                    objusrsList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                }
            }
            objusrsList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
            slmusrMap.SalesmenList = CommonFunction.SalesmenListwithoutlogin();
            slmusrMap.UsersList = objusrsList;
            return View(slmusrMap);
        }
    }

    public class SalesMenUserMapping
    {
        public List<SelectListItem> SalesmenList { get; set; }

        public List<SelectListItem> UsersList { get; set; }

        public string slsId { get; set; }

        public string usrId { get; set; }
    }
}