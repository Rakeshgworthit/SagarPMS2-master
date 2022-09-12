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

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class LoanController : Controller
    {
        private readonly ILoanRepositor _LoanRepo;
        public LoanController(ILoanRepositor LoanRepo)
        {
            _LoanRepo = LoanRepo;
        }
        public ActionResult Index(LoanViewModel objView)
        {
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.UID = "00000000-0000-0000-0000-000000000000";
                }
                else
                {
                    objView.UID = User.Identity.GetUserId();
                }

                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                if (objView.SearchFrom == null)
                {
                    objView.SearchFrom = startDate;
                }
                if (objView.SearchTo == null)
                {
                    objView.SearchTo = endDate;
                }
                objView.RecordTypeList = CommonFunction.GetRecordTypeList();
                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"].ToString();
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

        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = Common.SessionManagement.SelectedBranchID;
            LoanViewModel objView = new LoanViewModel();
            try
            {
                if (Id > 0)
                {
                    if (User.IsInRole("SuperAdmin"))
                    {
                        loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            objView.LoanDate = objRec.LoanDate;
                            objView.Id = objRec.Id;
                            objView.rec_type = objRec.rec_type;
                            objView.person_id = Convert.ToString(objRec.person_type + "_" + objRec.person_id);
                            objView.person_type = objRec.person_type;
                            objView.purpose = objRec.purpose;
                            objView.payment_mode = objRec.payment_mode;
                            objView.bank_id = objRec.bank_id;
                            objView.cheque_number = objRec.cheque_number;
                            objView.amount = objRec.amount;
                            objView.created_by = objRec.created_by;
                            objView.created_on = objRec.created_on;
                            objView.updated_by = objRec.updated_by;
                            objView.updated_on = objRec.updated_on;
                            objView.isactive = objRec.isactive;
                            objView.project_id = objRec.project_id == null ? 0 : Convert.ToInt32(objRec.project_id);
                        }
                    }
                    else
                    {
                        //loan objRec = _LoanRepo.FindBy(o => o.Id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            objView.Id = objRec.Id;
                            objView.LoanDate = objRec.LoanDate;
                            objView.rec_type = objRec.rec_type;
                            objView.person_id = Convert.ToString(objRec.person_type + "_" + objRec.person_id);
                            objView.person_type = objRec.person_type;
                            objView.purpose = objRec.purpose;
                            objView.payment_mode = objRec.payment_mode;
                            objView.bank_id = objRec.bank_id;
                            objView.cheque_number = objRec.cheque_number;
                            objView.amount = objRec.amount;
                            objView.created_by = objRec.created_by;
                            objView.created_on = objRec.created_on;
                            objView.updated_by = objRec.updated_by;
                            objView.updated_on = objRec.updated_on;
                            objView.isactive = objRec.isactive;
                            objView.project_id = objRec.project_id == null ? 0 : Convert.ToInt32(objRec.project_id);
                        }
                    }
                }
                else
                {
                    objView.LoanDate = Convert.ToDateTime(DateTime.Now);
                    objView.amount = Convert.ToDecimal(0.00);
                    objView.isactive = true;
                }
                objView.branch_Id = SessionManagement.SelectedBranchID;
                objView.RecordTypeList = Common.CommonFunction.GetRecordTypeList();
                objView.SalesmenAndUserList = Common.CommonFunction.GetSalesmenAndUser();
                objView.bankList = Common.CommonFunction.BankList();
                objView.mode_of_paymentList = Common.CommonFunction.ModeofPaymentList();
                objView.IsActiveList = Common.CommonFunction.StatusList();
                objView.projectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
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


        public JsonResult SaveUpdate(LoanViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            loan objRec = new loan();
            try
            {
                if (objView.Id > 0)
                {
                    objRec = _LoanRepo.FindBy(o => o.Id == objView.Id).SingleOrDefault();
                    if (objRec != null)
                    {

                        objRec.branch_Id = SessionManagement.SelectedBranchID;
                        objRec.LoanDate = objView.LoanDate;
                        objRec.rec_type = objView.rec_type;

                        string PersonType = Convert.ToString(objView.person_id).Split('_')[0];
                        string PersonID = Convert.ToString(objView.person_id).Split('_')[1];
                        objRec.person_id = Convert.ToInt32(PersonID);
                        objRec.person_type = Convert.ToString(PersonType);


                        objRec.purpose = objView.purpose;
                        objRec.payment_mode = objView.payment_mode;

                        objRec.bank_id = objView.bank_id;
                        objRec.cheque_number = objView.cheque_number;
                        objRec.amount = objView.amount;

                        objRec.created_by = objView.created_by;
                        objRec.created_on = objView.created_on;
                        objRec.updated_on = DateTime.Now;
                        objRec.updated_by = new Guid(uid);
                        objRec.isactive = objView.isactive;
                        objRec.project_id = objView.project_id;
                        _LoanRepo.Save();
                    }
                    return Json(new { msg = "Loan successfully updated.", cls = "success" });

                }
                else
                {

                    objRec.branch_Id = SessionManagement.SelectedBranchID;
                    objRec.LoanDate = objView.LoanDate;
                    objRec.rec_type = objView.rec_type;

                    string PersonType = Convert.ToString(objView.person_id).Split('_')[0];
                    string PersonID = Convert.ToString(objView.person_id).Split('_')[1];
                    objRec.person_id = Convert.ToInt32(PersonID);
                    objRec.person_type = Convert.ToString(PersonType);


                    objRec.purpose = objView.purpose;
                    objRec.payment_mode = objView.payment_mode;

                    objRec.bank_id = objView.bank_id;
                    objRec.cheque_number = objView.cheque_number;
                    objRec.amount = objView.amount;

                    //   Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.isactive = objView.isactive;
                    objRec.project_id = objView.project_id;

                    objRec.created_on = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.updated_on = DateTime.Now;
                    objRec.updated_by = new Guid(uid);
                    _LoanRepo.Add(objRec);
                    _LoanRepo.Save();
                    return Json(new { msg = "Loan successfully saved.", cls = "success" });

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUpdate, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }


        public ActionResult DeleteById(Int32 Id)
        {
            try
            {
                if (Id > 0)
                {
                    loan objRec = _LoanRepo.FindBy(o => o.Id == Id).SingleOrDefault();
                    _LoanRepo.Delete(objRec);
                    _LoanRepo.Save();
                    TempData["Message"] = "Loan deleted successfully.";
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
    }
}