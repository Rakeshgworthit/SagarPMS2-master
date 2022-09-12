using System;
using System.Configuration;
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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using PMS.Interface;
using PMS.Data_Access;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ReceiptsController : Controller
    {
        private readonly IReceiptsRepositor _ReceiptsRepo;
        private readonly IProject _ProjectRepo;
        private IReceipts _IReceipts;

        //public ReceiptsController()
        //{
        //    _IReceipts = new DataLayer();
        //}

        public ReceiptsController(IReceiptsRepositor RecRepo, IProject projRepo)
        {
            _ProjectRepo = projRepo;
            _ReceiptsRepo = RecRepo;
            _IReceipts = new DataLayer();
        }
        public ActionResult Index(ReceiptsViewModel objview)
        {
            DateTime now = DateTime.Now;
            try
            {
                string uid = string.Empty;
                uid = User.Identity.GetUserId();

                var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
                var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
                if (objview.SearchFrom == null)
                    objview.SearchFrom = CurrentstartDate.AddMonths(-1);
                if (objview.SearchTo == null)
                    objview.SearchTo = endDate;
                if (User.IsInRole("Salesman"))
                    ViewBag.IsSalesManLogin = uid;
                else
                    ViewBag.IsSalesManLogin = "NO";
                return View(objview);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Index");
                return null;
            }
            finally
            {
            }
        }

        public ActionResult ReceiptsList(string JsonValues)
        {
            string message = string.Empty;
            var format = "dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            Receipts _Receipts = JsonConvert.DeserializeObject<Receipts>(JsonValues, dateTimeConverter);
            List<ReceiptsList> _ReceiptsList = new List<ReceiptsList>();
            string uid = string.Empty;
            try
            {

                uid = User.Identity.GetUserId();
                _Receipts.UserID = uid;

                if (User.IsInRole("Salesman"))
                {
                    ViewBag.IsSalesManLogin = uid;
                    //objView.SalenmenList = CommonFunction.GetSalesmenByUserIdExistsInUsersTbl(uid);
                }
                else
                    ViewBag.IsSalesManLogin = "NO";

                _ReceiptsList = _IReceipts.ReceiptsList(_Receipts);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ReceiptsList");
                message = ex.Message;
            }
            finally
            {
                _Receipts = null;
            }
            var data = new
            {
                Items = _ReceiptsList,
                TotalCount = _ReceiptsList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpsertCustomerPayments(string JsonCustomerPayments)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            var format = "dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };

            ReceiptsList _UpsertCustomerPaymentsCriteria = JsonConvert.DeserializeObject<ReceiptsList>(JsonCustomerPayments, dateTimeConverter);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IReceipts.UpsertCustomerPayments(_UpsertCustomerPaymentsCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertCustomerPayments");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _UpsertCustomerPaymentsCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_Customer_Payments(string Id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IReceipts.Delete_Customer_Payments(Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Customer_Payments");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteById(Int32 Id)
        {
            try
            {
                if (Id > 0)
                {
                    receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    _ReceiptsRepo.Delete(objRec);
                    _ReceiptsRepo.Save();
                    TempData["Message"] = "Record deleted successfully.";
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
        public ActionResult LoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = Common.SessionManagement.SelectedBranchID;
            ReceiptsViewModel objView = new ReceiptsViewModel();
            Int32 projectId = 0;
            try
            {
                if (Id > 0)
                {

                    if (User.IsInRole("SuperAdmin"))
                    {
                        receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            projectId = Convert.ToInt32(objRec.project_id);
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    else
                    {
                        //receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        receipt objRec = _ReceiptsRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            projectId = Convert.ToInt32(objRec.project_id);
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    if (projectId > 0)
                    {
                        objView.isProjectClosed = false;
                        project objProRec = _ProjectRepo.FindBy(o => o.id == projectId).SingleOrDefault();
                        if (objProRec.status_id == 3)
                        {
                            objView.isProjectClosed = true;
                        }

                    }                   
;                }
                else
                {
                    objView.amount = Convert.ToDecimal(0.00);
                    objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                    objView.gst_amount = Convert.ToDecimal(0.00);
                    objView.total_amount = Convert.ToDecimal(0.00);
                    objView.receipt_date = DateTime.Now;
                    objView.isactive = true;
                    objView.isProjectClosed = false;


                }
                objView.bankList = Common.CommonFunction.BankList();
                objView.GSTList = CommonFunction.GSTList();
                objView.mode_of_paymentList = Common.CommonFunction.ModeofPaymentList();
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.branchList = Common.CommonFunction.BranchList();
                    objView.projectList = Common.CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
                }
                else
                {
                    objView.branchList = Common.CommonFunction.UserBranchList(uid);
                    objView.projectList = Common.CommonFunction.UserProjectListWithID(uid, projectId);
                }
                objView.IsActiveList = Common.CommonFunction.StatusList();
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

        public JsonResult SaveUpdate(ReceiptsViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            receipt objRec = new receipt();
            try
            {
                if (objView.id > 0)
                {
                    objRec = _ReceiptsRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.created_by = objRec.created_by;
                        objView.created_date = objRec.created_date;
                        Common.CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _ReceiptsRepo.Save();
                    }
                    return Json(new { msg = "Receipt updated successfully.", cls = "success" });

                }
                else
                {
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _ReceiptsRepo.Add(objRec);
                    _ReceiptsRepo.Save();
                    return Json(new { msg = "Receipt created successfully.", cls = "success" });

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

        public Int32 CheckReceipt(Int32 projectId)
        {
            Int32 count = 0;
            try
            {
                count = _ReceiptsRepo.FindBy(o => o.project_id == projectId).Count();
                count = count + 1;
                return count;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckReceipt, Parameter : projectId={projectId}");
                return count;
            }
            finally
            {
            }
        }

        public ActionResult LoadSMS(Int32 projectId)
        {
            PaymentsReceived objView = new PaymentsReceived();
            try
            {
                objView.PaymentReceived = PaymentList(projectId);
                Session["payments_received"] = objView.PaymentReceived;
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckReceipt, Parameter : projectId={projectId}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }
        public List<PaymentsReceived> PaymentList(int id)
        {
            List<PaymentsReceived> objList = new List<PaymentsReceived>();
            List<ReceiptsViewModel> ReceiptsViewModelobjList = new List<ReceiptsViewModel>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.SSP_PaymentDetailsForSMS(id).ToList();
                    if (sList.Count() > 0)
                    {
                        foreach (var items in sList)
                        {
                            objList.Add(new PaymentsReceived
                            {
                                date_amount = items.date_amount
                                ,
                                cheque_details = items.cheque_details,
                                mobileno = items.mobile,
                                totalPayment = items.totalPayment,
                                branchAddress = items.branchAddress,
                                accountNo = items.accountNo
                            });
                        }
                    }

                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PaymentList, Parameter : id={id}");
                return null;
            }
            finally
            {
                objList = null;
                ReceiptsViewModelobjList = null;
            }
        }
        [HttpPost]
        public JsonResult SendSMS()
        {
            //PaymentsReceived objView = Session["payments_received"] as PaymentsReceived;
            var pay = Session["payments_received"] as IEnumerable<PaymentsReceived>;
            Session["payments_received"] = null;
            string returnMsg, clsStatus;
            string total = "0.0", mob = "", branch = "", account = "", newbranch = "";
            try
            {
                foreach (var item in pay)
                {
                    newbranch = item.branchAddress;
                }

                string sMsg = "Thank you for the payment made to " + newbranch + "\n PAYMENTS RECEIVED :\n";

                if (pay.Count() > 0)
                {
                    foreach (var x in pay)
                    {
                        sMsg += x.date_amount + "\n" + x.cheque_details + "\n";
                        total = x.totalPayment;
                        mob = x.mobileno;
                        branch = x.branchAddress;
                        account = x.accountNo;
                    }
                }
                sMsg += "\n Total Payment Received as of : " + total;
                sMsg += " \n (*This include payment mode via cheque/transfer, amount which is still pending by bank)";
                sMsg += "\n Kindly contact us at 87499919/63859919 if there is any discrepancy.\n Cash payment MUST be acknowledged with our company official receipt. Bank transfer payment should only be made to our corporate bank account: " + branch + " - " + account;
                sMsg += "\n Accounts \n " + branch + " \n (THIS IS AN AUTO - GENERATED PAYMENT UPDATE) ";
                string MTURL = "http://gateway80.onewaysms.sg/api2.aspx";
                string apiusername = System.Configuration.ConfigurationManager.AppSettings["apiusername"];
                string apipassword = System.Configuration.ConfigurationManager.AppSettings["apipassword"];
                string isStaging = System.Configuration.ConfigurationManager.AppSettings["isStaging"];

                if (String.IsNullOrEmpty(mob))
                {
                    returnMsg = "Mobile number is not provided!";
                    clsStatus = "error";
                    return Json(new { msg = returnMsg, cls = clsStatus });
                }
                if (isStaging == "1")
                {
                    mob = System.Configuration.ConfigurationManager.AppSettings["stagingMobileNumber"];
                    //mob = ConfigurationManager.AppSettings["stagingMobileNumber"];
                }
                //if (ConfigurationManager.AppSettings["isStaging"].ToString() == "1")
                //{
                //    mob = ConfigurationManager.AppSettings["stagingMobileNumber"];
                //}
                else
                {
                    mob = "65" + mob.Replace(" ", string.Empty);
                }

                string sURL = MTURL + "?apiusername=" + apiusername + "&apipassword=" + apipassword + "&mobileno=" + mob + "&senderid=onewaysms&languagetype=1&message=" + sMsg;
                using (var web = new System.Net.WebClient())
                {
                    string result = web.DownloadString(sURL);
                    returnMsg = "SMS sent successfully !";
                    clsStatus = "success";
                }
            }
            catch
            {
                returnMsg = "Some error occurred !";
                clsStatus = "error";
            }
            return Json(new { msg = returnMsg, cls = clsStatus });
        }

    }
}