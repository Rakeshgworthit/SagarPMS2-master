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
using PMS.Interface;
using PMS.Data_Access;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Data;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]

    public class PaymentsController : Controller
    {
        private readonly IProject _ProjectRepo;
        private readonly IPaymentsRepositor _PaymentRepo;
        private readonly IPaymentDetailRepository _PaymentDetailRepo;
        private readonly IPaymentDetailsDescription _PaymentsDescription;
        private IPayments _IPayments;
        public PaymentsController(IPaymentsRepositor PayRepo, IPaymentDetailRepository PayDetRepo, IProject projRepo, IPaymentDetailsDescription paymentsDescription)
        {
            _ProjectRepo = projRepo;
            _PaymentRepo = PayRepo;
            _PaymentDetailRepo = PayDetRepo;
            _PaymentsDescription = paymentsDescription;
            _IPayments = new DataLayer();
        }

        public ActionResult Index(PaymentsViewModel objView)
        {


            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            if (objView.SearchFrom == null)
            {
                objView.SearchFrom = CurrentstartDate.AddMonths(-5);
            }
            if (objView.SearchTo == null)
            {
                objView.SearchTo = endDate;
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("User"))
            {
                objView.UID = "00000000-0000-0000-0000-000000000000";
                objView.SalesmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
            }
            else
            {
                objView.UID = User.Identity.GetUserId();
                objView.SalesmenList = CommonFunction.GetSalesmenIdByUserId(objView.UID);
            }

            objView.projectList = CommonFunction.UserProjectList(objView.UID);

            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"].ToString();
            return View(objView);
        }
        public ActionResult DeleteById(Int32 Id)
        {
            CommonData commonData = new CommonData();
            if (Id > 0)
            {
                payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                if (commonData.UpdateBudgetPaymentStatus(Id))
                {
                    _PaymentRepo.Delete(objRec);
                    _PaymentRepo.Save();
                    TempData["Message"] = "Record deleted successfully.";
                }
            }
            return RedirectToAction("Index");

        }
        public ActionResult LoadAddEdit(Int32 Id, Int64 SupplierId)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            PaymentsViewModel objView = new PaymentsViewModel();
            objView.mode_of_paymentList = CommonFunction.ModeofPaymentList();
            CommonData commonData = new CommonData();
            objView.GSTList = CommonFunction.GSTList();
            Int32 projectId = 0;

            if (Id > 0)
            {

                Int64 Supplier_id = 0;
                if (User.IsInRole("SuperAdmin"))
                {
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    payment objRec = commonData.LoadAddEditPayment(Id);
                    if (objRec != null)
                    {
                        Supplier_id = objRec.supplier_id;
                        // projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    payment objRec = commonData.LoadAddEditPayment(Id);
                    if (objRec != null)
                    {
                        Supplier_id = objRec.supplier_id;
                        //  projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }

                //var controllerPBC = DependencyResolver.Current.GetService<ProjectsBudgetController>();
                //Convert.ToDecimal(controllerPBC.getBudgetedCost(Convert.ToInt32(items.project_id), Convert.ToInt32(Supplier_id)))
                List<payment_details> objPDList = new List<payment_details>();
                //List<payment_detail> objPDRec = _PaymentDetailRepo.FindBy(o => o.payment_id == Id).ToList();
                Database.PMSEntities obj = new PMSEntities();

                //List<SSP_GetPaymentDetail_Result> objPDRec = obj.SSP_GetPaymentDetail(Convert.ToInt32(Id), Convert.ToInt32(Supplier_id)).ToList();
                List<Proc_GetPaymentDetail_Result> objPDRec = _IPayments.GetPaymentDetail_Result(Convert.ToInt32(Id), Convert.ToInt32(Supplier_id)).ToList();
                var list = new List<string>();
                foreach (var item in objPDRec)
                {
                    list.Add(item.supplier_inv_number);
                }
                TempData["SupInvList"] = list;
                foreach (var items in objPDRec)
                {
                    objPDList.Add(new payment_details
                    {
                        project_id = Convert.ToInt64(items.project_id),
                        supplier_inv_number = items.supplier_inv_number,
                        gst_amount = Convert.ToDecimal(items.gst_amount),
                        gst_percentage = Convert.ToDecimal(items.gst_percentage),
                        invoice_amount = Convert.ToDecimal(items.invoice_amount),
                        payment_amount = Convert.ToDecimal(items.payment_amount),
                        budgeted_cost = Convert.ToDecimal(items.budget_amount),
                        supplier_inv_list = Convert.ToString(getSupplierList(Convert.ToInt32(items.project_id), Convert.ToInt32(Supplier_id)))
                    });
                }
                objView.payment_details = objPDList;


                List<payment_details_descriptions> objPDescList = new List<payment_details_descriptions>();
                List<SSP_Payment_Detail_Description_Result> objPDesc = obj.SSP_Payment_Detail_Description(Convert.ToInt32(Id), null, null, "S", null).ToList();
                if (objPDesc.Count > 0)
                {
                    foreach (var items in objPDesc)
                    {
                        objPDescList.Add(new payment_details_descriptions
                        {
                            description = Convert.ToString(items.description),
                            amount = Convert.ToDecimal(items.amount),
                            descriptionID = Convert.ToInt32(items.descriptionID)
                        });
                    }
                    objView.payment_descriptions = objPDescList;
                }
                else
                {
                    objView.payment_descriptions = new List<payment_details_descriptions>();
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



            }
            else
            {
                objView.gst_amount = Convert.ToDecimal(0.00);
                objView.rebate_amount = Convert.ToDecimal(0.00);
                objView.invoice_amount = Convert.ToDecimal(0.00);
                objView.payment_amount = Convert.ToDecimal(0.00);
                objView.payment_date = DateTime.Now;
                objView.isactive = true;
                objView.payment_mode = 4;
                objView.isProjectClosed = false;
                objView.payment_details = new List<payment_details>();
                //objView.payment_details = CommonFunction.GetpaymentDetailsBySupplierId(Convert.ToString(SupplierId));
                objView.payment_descriptions = new List<payment_details_descriptions>();

            }

            if (User.IsInRole("SuperAdmin"))
            {
                objView.supplierList = CommonFunction.UserSupplierList("00000000-0000-0000-0000-000000000000");
                if (Id > 0)
                {
                    objView.projectList = CommonFunction.UserProjectListWithIDNew("00000000-0000-0000-0000-000000000000", projectId);
                }
                else
                {
                    objView.projectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
                }
            }
            else
            {
                objView.supplierList = CommonFunction.UserSupplierList(uid);
                if (Id > 0)
                {
                    objView.projectList = CommonFunction.UserProjectListWithIDNew(uid, projectId);
                }
                else
                {
                    objView.projectList = CommonFunction.UserProjectListWithID(uid, projectId);
                }
            }
            if (SupplierId > 0)
            {
                objView.supplier_id = SupplierId;
            }
            objView.bankList = CommonFunction.BankList();
            objView.id = Id;
            // objView.IsActiveList = Common.CommonFunction.StatusList();

            return View(objView);
        }

        public ActionResult LoadPaymentEdit(Int32 Id, Int64 SupplierId)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            PaymentsViewModel objView = new PaymentsViewModel();
            objView.mode_of_paymentList = CommonFunction.ModeofPaymentList();
            CommonData commonData = new CommonData();


            Int32 projectId = 0;

            if (Id > 0)
            {

                Int64 Supplier_id = 0;
                if (User.IsInRole("SuperAdmin"))
                {
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    payment objRec = commonData.LoadAddEditPayment(Id);
                    if (objRec != null)
                    {
                        Supplier_id = objRec.supplier_id;
                        // projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    //payment objRec = _PaymentRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    payment objRec = commonData.LoadAddEditPayment(Id);
                    if (objRec != null)
                    {
                        Supplier_id = objRec.supplier_id;
                        //  projectId = Convert.ToInt32(objRec.project_id);
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }

                //var controllerPBC = DependencyResolver.Current.GetService<ProjectsBudgetController>();
                //Convert.ToDecimal(controllerPBC.getBudgetedCost(Convert.ToInt32(items.project_id), Convert.ToInt32(Supplier_id)))
                List<payment_details> objPDList = new List<payment_details>();
                //List<payment_detail> objPDRec = _PaymentDetailRepo.FindBy(o => o.payment_id == Id).ToList();
                Database.PMSEntities obj = new PMSEntities();

                //List<SSP_GetPaymentDetail_Result> objPDRec = obj.SSP_GetPaymentDetail(Convert.ToInt32(Id), Convert.ToInt32(Supplier_id)).ToList();
                List<Proc_GetPaymentDetail_Result> objPDRec = _IPayments.GetPaymentDetail_Result(Convert.ToInt32(Id), Convert.ToInt32(Supplier_id)).ToList();
                var list = new List<string>();
                foreach (var item in objPDRec)
                {
                    list.Add(item.supplier_inv_number);
                }
                TempData["SupInvList"] = list;
                foreach (var items in objPDRec)
                {
                    objPDList.Add(new payment_details
                    {
                        project_id = Convert.ToInt64(items.project_id),
                        supplier_inv_number = items.supplier_inv_number,
                        gst_amount = Convert.ToDecimal(items.gst_amount),
                        gst_percentage = Convert.ToDecimal(items.gst_percentage),
                        invoice_amount = Convert.ToDecimal(items.invoice_amount),
                        payment_amount = Convert.ToDecimal(items.payment_amount),
                        budgeted_cost = Convert.ToDecimal(items.budget_amount),
                        supplier_inv_list = Convert.ToString(getSupplierList(Convert.ToInt32(items.project_id), Convert.ToInt32(Supplier_id)))
                    });
                }
                objView.payment_details = objPDList;


                List<payment_details_descriptions> objPDescList = new List<payment_details_descriptions>();
                List<SSP_Payment_Detail_Description_Result> objPDesc = obj.SSP_Payment_Detail_Description(Convert.ToInt32(Id), null, null, "S", null).ToList();
                if (objPDesc.Count > 0)
                {
                    foreach (var items in objPDesc)
                    {
                        objPDescList.Add(new payment_details_descriptions
                        {
                            description = Convert.ToString(items.description),
                            amount = Convert.ToDecimal(items.amount),
                            descriptionID = Convert.ToInt32(items.descriptionID)
                        });
                    }
                    objView.payment_descriptions = objPDescList;
                }
                else
                {
                    objView.payment_descriptions = new List<payment_details_descriptions>();
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



            }
            else
            {                
                objView.gst_amount = Convert.ToDecimal(0.00);
                objView.rebate_amount = Convert.ToDecimal(0.00);
                objView.invoice_amount = Convert.ToDecimal(0.00);
                objView.payment_amount = Convert.ToDecimal(0.00);
                objView.payment_date = DateTime.Now;
                objView.isactive = true;
                objView.payment_mode = 4;
                objView.isProjectClosed = false;
                objView.payment_details = new List<payment_details>();
                //objView.payment_details = CommonFunction.GetpaymentDetailsBySupplierId(Convert.ToString(SupplierId));
                objView.payment_descriptions = new List<payment_details_descriptions>();

            }

            if (User.IsInRole("SuperAdmin"))
            {
                objView.supplierList = CommonFunction.UserSupplierList("00000000-0000-0000-0000-000000000000");
                if (Id > 0)
                {
                    objView.projectList = CommonFunction.UserProjectListWithIDNew("00000000-0000-0000-0000-000000000000", projectId);
                }
                else
                {
                    objView.projectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);
                }
            }
            else
            {
                objView.supplierList = CommonFunction.UserSupplierList(uid);
                if (Id > 0)
                {
                    objView.projectList = CommonFunction.UserProjectListWithIDNew(uid, projectId);
                }
                else
                {
                    objView.projectList = CommonFunction.UserProjectListWithID(uid, projectId);
                }
            }
            if (SupplierId > 0)
            {
                objView.supplier_id = SupplierId;
            }
            objView.bankList = CommonFunction.BankList();
            objView.id = Id;
            // objView.IsActiveList = Common.CommonFunction.StatusList();

            return View(objView);
        }

        public JsonResult GetAddressBySalemanId(string Supplier_id)
        {
            PaymentsViewModel objView = new PaymentsViewModel();
            objView.projectList = objView.projectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", 0, Supplier_id);
            return Json(new { data = objView.projectList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBudgetCostDetailsBySupplierId(string Supplier_id)
        {
            List<BudgetCostDetails> objView = CommonFunction.GetBudgetCostDetailsBySupplierId(Supplier_id);
            return Json(new { data = objView }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetBudgetCostDetailsForEdit(string payment_Id)
        {
            List<BudgetCostDetails> objView = CommonFunction.GetBudgetCostDetailsForEdit(payment_Id);
            return Json(new { data = objView }, JsonRequestBehavior.AllowGet);
        }

        public string getSupplierList(int projectId, int supplierId)
        {
            string ddlhtml = "";
            using (Database.PMSEntities obj = new PMSEntities())
            {
                List<SSP_getBudgetedInvoice_Result> objPBList = obj.SSP_getBudgetedInvoice(projectId, supplierId).ToList();
                ddlhtml = ddlhtml + "<option value =''>Please Select</option>";
                foreach (var items in objPBList)
                {
                    ddlhtml = ddlhtml + "<option value ='" + items.inv.ToString() + "'>" + items.remarks.ToString() + "</option >";
                }
            }

            return ddlhtml;

        }

        public Int32 CheckExistingPayment(PaymentChkExistingViewModel objView)
        {
            PMSEntities pmsobj = new PMSEntities();
            if (!string.IsNullOrEmpty(Request.Params["payobjchkExist"]))
            {
                objView = PaymentChkExistingViewModel.FromJson(Request.Params["payobjchkExist"]);
            }
            var arry = objView.payment_details_chk_Existing.Select(o => new { o.supplier_inv_number, o.project_id }).ToList();


            List<string> supinvlst = arry.Select(o => o.supplier_inv_number).ToList();
            List<long?> projectlst = arry.Select(o => (long?)o.project_id).ToList();

            var PaymentObj = (from pay in pmsobj.payments
                              join paydet in pmsobj.payment_detail on pay.id equals paydet.payment_id
                              where pay.id != objView.id && pay.supplier_id == objView.supplier_id
                              && supinvlst.Contains(paydet.supplier_inv_number)
                              && projectlst.Contains(paydet.project_id)
                              select new { paydet.supplier_inv_number });
            var PaymentObj1 = PaymentObj.Count();
            if (PaymentObj1 > 0)
            {
                TempData["PaymentObj"] = PaymentObj;
            }
            return PaymentObj1;
        }

        public JsonResult SaveUpdate(PaymentsViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            payment objRec = new payment();
            CommonData commonData = new CommonData();
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            bool Flag = true;
            try
            {
                if (!string.IsNullOrEmpty(Request.Params["paymentobj"]))
                {
                    objView = PaymentsViewModel.FromJson(Request.Params["paymentobj"]);
                }
                if (objView.id > 0)
                {
                    var SupInvList = TempData.Peek("SupInvList") as List<string>;
                    foreach (var item in objView.payment_details)
                    {
                        if (item.supplier_inv_number_text != "")
                        {
                            list.Add(item.project_id + item.supplier_inv_number_text);
                            list1.Add(item.supplier_inv_number_text);
                        }
                    }
                    if (list.Count != list.Distinct().Count())
                    {
                        return Json(new { msg = "Duplicate Invoice Numbers are not Allowed", cls = "error", id = Convert.ToString(objRec.id) });
                    }
                    //foreach (var item in list1)
                    //{
                    //    bool Valid = SupInvList.Contains(item);
                    //    if (!Valid)
                    //    {
                    //        foreach (var item1 in objView.payment_details)
                    //        {
                    //            if (item1.supplier_inv_number_text == item)
                    //            {
                    //                var InvList = commonData.InvoiceList(item1.project_id, objView.supplier_id, SessionManagement.SelectedBranchID);
                    //                if (InvList.Contains(item1.supplier_inv_number_text))
                    //                {
                    //                    return Json(new { msg = "Invoice Number : " + item1.supplier_inv_number_text + " Already exists", cls = "error", id = Convert.ToString(objRec.id) });
                    //                }
                    //            }
                    //        }
                    //        //var InvList = from pay in pmsobj.payments join paydet in pmsobj.payment_detail on pay.id equals paydet.payment_id where pay.supplier_id == objView.supplier_id select paydet.supplier_inv_number;
                    //        //foreach (var item1 in objView.payment_details)
                    //        //{
                    //        //    var InvList = commonData.InvoiceList(item1.project_id, objView.supplier_id, SessionManagement.SelectedBranchID);
                    //        //    foreach (var item2 in list1)
                    //        //    {
                    //        //        bool IsValid = InvList.Contains(item2);
                    //        //        if (IsValid)
                    //        //        {
                    //        //            return Json(new { msg = "Invoice Number : " + item2 + " Already exists", cls = "error", id = Convert.ToString(objRec.id) });
                    //        //        }
                    //        //    }
                    //        //}
                    //    }
                    //}
                    if (Flag)
                    {
                        objView.created_by = new Guid(uid);
                        objRec.id = commonData.SaveUpdatePayment(objView);
                        //objRec = _PaymentRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                        if (objRec != null)
                        {
                            //objView.created_by = objRec.created_by;
                            //objView.created_date = objRec.created_date;
                            //CommonFunction.MergeObjects(objRec, objView, true);
                            //objRec.modified_date = DateTime.Now;
                            //objRec.modified_by = new Guid(uid);
                            //_PaymentRepo.Save();
                            using (Database.PMSEntities obj = new PMSEntities())
                            {
                                obj.SSP_Remove_Payment_Detail(Convert.ToInt32(objRec.id));
                                foreach (var item1 in objView.payment_details)
                                {
                                    obj.SSP_Add_Payment_Detail(Convert.ToInt32(objRec.id), Convert.ToString(item1.supplier_inv_number_text), Convert.ToDecimal(item1.invoice_amount),
                                        Convert.ToDecimal(item1.gst_percentage),
                                         Convert.ToDecimal(item1.gst_amount), Convert.ToDecimal(item1.payment_amount), Convert.ToDecimal(item1.agreed_amount), Convert.ToInt32(item1.project_id),
                                         Convert.ToString(item1.InvRemarks));
                                }
                                if (objView.payment_descriptions.Count > 0)
                                {
                                    obj.SSP_Payment_Detail_Description(Convert.ToInt32(objRec.id), null, null, "D", null);
                                    foreach (var item2 in objView.payment_descriptions)
                                    {

                                        obj.SSP_Payment_Detail_Description(Convert.ToInt32(objRec.id), Convert.ToString(item2.description), Convert.ToDecimal(item2.amount), "I", null);
                                    }
                                }
                            }
                        }
                    }

                    // Status update in project budget details table
                    commonData.UpdatePaymentStatus(objRec.id, objView.supplier_id);

                    Session["supplierId"] = objRec.supplier_id;
                    return Json(new { msg = "Payment updated successfully.", cls = "success", id = Convert.ToString(objRec.id) });
                }
                else
                {
                    using (Database.PMSEntities pmsobj = new PMSEntities())
                    {
                        //var InvList = from pay in pmsobj.payments join paydet in pmsobj.payment_detail on pay.id equals paydet.payment_id where pay.supplier_id == objView.supplier_id select paydet.supplier_inv_number;                    
                        foreach (var item in objView.payment_details)
                        {
                            if (item.supplier_inv_number_text != "")
                            {
                                list.Add(item.project_id + item.supplier_inv_number_text);
                                list1.Add(item.supplier_inv_number_text);
                            }
                        }
                        if (list.Count != list.Distinct().Count())
                        {
                            return Json(new { msg = "Duplicate Invoice Numbers are not Allowed", cls = "error", id = Convert.ToString(objRec.id) });
                        }
                        else
                        {
                            foreach (var item1 in objView.payment_details)
                            {
                                var InvList = commonData.InvoiceList(item1.project_id, objView.supplier_id, SessionManagement.SelectedBranchID);
                                foreach (var item in list1)
                                {
                                    bool IsValid = InvList.Contains(item);
                                    if (IsValid)
                                    {
                                        return Json(new { msg = "Invoice Number : " + item + " Already exists", cls = "error", id = Convert.ToString(objRec.id) });
                                    }
                                }
                            }
                        }
                    }
                    //Common.CommonFunction.MergeObjects(objRec, objView, true);
                    //objRec.created_date = DateTime.Now;
                    //objRec.created_by = new Guid(uid);
                    //objRec.modified_date = DateTime.Now;
                    //objRec.modified_by = new Guid(uid);
                    //_PaymentRepo.Add(objRec);
                    //_PaymentRepo.Save();
                    objView.created_by = new Guid(uid);
                    objRec.id = commonData.SaveUpdatePayment(objView);
                    using (Database.PMSEntities obj = new PMSEntities())
                    {
                        foreach (var item in objView.payment_details)
                        {
                            obj.SSP_Add_Payment_Detail(Convert.ToInt32(objRec.id), Convert.ToString(item.supplier_inv_number_text), Convert.ToDecimal(item.invoice_amount),
                                Convert.ToDecimal(item.gst_percentage),
                                 Convert.ToDecimal(item.gst_amount), Convert.ToDecimal(item.payment_amount), Convert.ToDecimal(item.agreed_amount), Convert.ToInt32(item.project_id),
                                  Convert.ToString(item.InvRemarks));
                        }
                        if (objView.payment_descriptions.Count > 0)
                        {
                            foreach (var item in objView.payment_descriptions)
                            {
                                obj.SSP_Payment_Detail_Description(Convert.ToInt32(objRec.id), Convert.ToString(item.description), Convert.ToDecimal(item.amount), "I", null);
                            }
                        }

                    }

                    // Status update in project budget details table
                    commonData.UpdatePaymentStatus(objRec.id, objView.supplier_id);

                    Session["supplierId"] = objRec.supplier_id;
                    return Json(new { msg = "Payment created successfully.", cls = "success", id = Convert.ToString(objRec.id) });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUpdate, Parameter : Parameters={objView}");
                return null;
            }
        }

        public ActionResult PrintPreview(string Id)
        {
            List<PrintPaymentViewModel> objPDBatchList = new List<PrintPaymentViewModel>();
            PrintBatchPaymentViewModel objBatchView = new PrintBatchPaymentViewModel();
            CommonData commonData = new CommonData();
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            var primeArray = Id.Split(',');
            TempData["PaymentId"] = Id;
            a a_ = new a();
            string PaymentID = "";

            try
            {
                List<a> _abc = new List<a>();
                PaymentID = Id;
                for (int i = 0; i < primeArray.Length; i++)
                {
                    Int32 id = Convert.ToInt32(primeArray[i]);
                    PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();

                    //objViewprint.ssp_paymentById_result = _PaymentRepo.PaymentById(Convert.ToInt32(id));
                    objViewprint.ssp_paymentById_result = commonData.PrintPayment(Convert.ToInt32(id));
                    objViewprint.payment_details = commonData.LoadpaymentList(PaymentID);
                    //for (int J = 0; J < objViewprint.payment_details.Count; J++)
                    //{
                    //    if (objViewprint.payment_details[J].supplier_inv_number.StartsWith("CN") || objViewprint.payment_details[J].supplier_inv_number.StartsWith("cn"))
                    //    {
                    //        list1.Add(objViewprint.payment_details[J].supplier_inv_number);
                    //    }
                    //    else
                    //    {
                    //        list2.Add(objViewprint.payment_details[J].supplier_inv_number);
                    //    }

                    //}
                    //list1 = list1.Concat(list2).ToList();

                    //for (int J = 0; J < objViewprint.payment_details.Count; J++)
                    //{
                    //    objViewprint.payment_details[J].supplier_inv_number = list1[J];
                    //}
                    objViewprint.SSP_PaymentsDescription_Result = _PaymentRepo.PaymentsDescription(Convert.ToInt32(id)).ToList();//_PaymentsDescription.FindBy(o => o.payment_id == id).ToList();
                    objPDBatchList.Add(objViewprint);

                    if (objViewprint.ssp_paymentById_result.cheque_number == "0")
                    {
                        objViewprint.ssp_paymentById_result.cheque_number = "";
                    }
                }
                objBatchView.printpaymentviewmodel = objPDBatchList;
                return View(objBatchView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PrintPreview, Parameter : Id={Id}");
                return View(objBatchView);
            }
            finally
            {

            }
        }

        //public ActionResult PrintPreview(Int64 PaymentId)
        //{
        //    //List<PrintPaymentViewModel> objPDBatchList = new List<PrintPaymentViewModel>();
        //   // PrintBatchPaymentViewModel objBatchView = new PrintBatchPaymentViewModel();
        //    CommonData commonData = new CommonData();
        //    PrintPaymentViewModel objViewprint = new PrintPaymentViewModel();
        //    DataLayer ObjDB = new DataLayer();
        //    //objViewprint.ssp_paymentById_result.Paymode = ObjDB.Prinpreview(PaymentId);

        //    return View(objViewprint);
        //}
        public ActionResult LoadSMS(string collectionDate)
        {

            if (collectionDate == null)
            {
                collectionDate = "";
            }
            ViewBag.collectionDate = collectionDate;
            return View();
        }
        [HttpPost]
        public JsonResult SendSMS(string collectionDate)
        {
            string supplierId = Session["supplierId"].ToString();

            if (collectionDate == null)
            {
                collectionDate = "";
            }
            PaymentsViewModel objModel = new PaymentsViewModel();
            objModel.supplierList = CommonFunction.SuppliersForSMS(supplierId);
            var mobile = CommonFunction.SupplierMobile(supplierId);
            string prefix = "65";

            string returnMsg, clsStatus, supMob = string.Empty;
            //  supMob = mobile.ToString();
            try
            {
                string sMsg = "Dear Valued Supplier";
                sMsg += "\n This is  from Design 4 Space and Areana Creation.";
                sMsg += " \n Kindly be informed that your cheque payment is ready for collection on (Collection Date)" + collectionDate;
                sMsg += "\n @ 140 Paya Lebar Rd #01-19 AZ Building,  Singapore 409015.";
                string MTURL = "http://gateway80.onewaysms.sg/api2.aspx";
                string apiusername = System.Configuration.ConfigurationManager.AppSettings["apiusername"];
                string apipassword = System.Configuration.ConfigurationManager.AppSettings["apipassword"];

                if (mobile.Count() > 0)
                {
                    for (int i = 0; i < mobile.Count(); i++)
                    {
                        if (mobile[i] != null)
                        {
                            if (ConfigurationManager.AppSettings["isStaging"].ToString() == "1")
                            {
                                supMob = ConfigurationManager.AppSettings["stagingMobileNumber"].ToString();
                            }
                            else
                            {
                                supMob = string.Join(",", mobile.Select(x => prefix + x));
                            }
                        }
                        else
                        {
                            returnMsg = "Mobile number is not provided!";
                            clsStatus = "error";
                            return Json(new { msg = returnMsg, cls = clsStatus });
                        }
                    }
                }
                else
                {
                    returnMsg = "Mobile number is not provided!";
                    clsStatus = "error";
                    return Json(new { msg = returnMsg, cls = clsStatus });
                }
                sMsg = HttpUtility.UrlEncode(sMsg, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                string sURL = MTURL + "?apiusername=" + apiusername + "&apipassword=" + apipassword + "&mobileno=" + supMob + "&senderid=onewaysms&languagetype=1&message=" + sMsg;
                using (var web = new System.Net.WebClient())
                {
                    string result = web.DownloadString(sURL);
                    returnMsg = "SMS sent successfully !";
                    clsStatus = "success";
                }
            }
            catch (Exception ex)
            {
                returnMsg = "Some error occurred !";
                clsStatus = "error";
            }
            return Json(new { msg = returnMsg, cls = clsStatus });
        }
        [HttpGet]
        public ActionResult LoadSupplier()
        {
            PaymentsViewModel objModel = new PaymentsViewModel();
            objModel.supplierList = CommonFunction.SuppliersForSMS();
            return View(objModel);
        }
        [HttpPost]
        public ActionResult LoadSupplier(string supplierIds)
        {
            PaymentsViewModel objModel = new PaymentsViewModel();
            var supplierId = supplierIds.Substring(1);
            Session["supplierId"] = supplierId;
            //  objModel.supplierList = CommonFunction.SuppliersForSMS(supplierId);
            return Json(objModel);
        }

        [HttpPost]
        public void SaveMessage(string Msg)
        {
            int PaymentId = Convert.ToInt32(TempData["PaymentId"]);
            using (PMSEntities obj = new PMSEntities())
            {
                var data = obj.payment_detail.FirstOrDefault(x => x.payment_id == PaymentId);
                var info = data.Description;
                if (data != null)
                {
                    data.Description = Msg;
                }
                obj.SaveChanges();
            }
        }

        [HttpPost]
        public void DeleteDescription(int[] DescId)
        {
            using (PMSEntities obj = new PMSEntities())
            {
                foreach (var item in DescId)
                {
                    var Decsription = obj.payment_details_description.FirstOrDefault(x => x.id == item);
                    if (Decsription != null)
                    {
                        obj.payment_details_description.Remove(Decsription);
                        obj.SaveChanges();
                    }
                }
            }
        }
    }

    public class CommonData
    {
        public Int64 SupplierId { get; set; }
        public payment LoadAddEditPayment(int Id)
        {
            payment obj = new payment();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommonConstantsforLoan.Id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_get_Payments);
                    while (ireader.Read())
                    {
                        obj.payment_date = ireader.GetNullDateTime(CommonColumnsforPayments.payment_date);
                        obj.supplier_id = ireader.GetInt64(CommonColumnsforPayments.supplier_id);
                        obj.bank_id = ireader.GetInt32(CommonColumnsforPayments.bank_id);
                        obj.cheque_number = ireader.GetString(CommonColumnsforPayments.cheque_number);
                        obj.rebate_amount = ireader.GetDecimal(CommonColumnsforPayments.rebate_amount);
                        obj.actual_payment_amount = ireader.GetDecimal(CommonColumnsforPayments.paid_amount);
                        obj.remarks = ireader.GetString(CommonColumnsforPayments.remarks);
                        obj.created_date = ireader.GetDateTime(CommonColumnsforPayments.created_date);
                        obj.created_by = ireader.GetGuid(CommonColumnsforPayments.created_by);
                        obj.modified_date = ireader.GetDateTime(CommonColumnsforPayments.modified_date);
                        obj.modified_by = ireader.GetGuid(CommonColumnsforPayments.modified_by);
                        obj.payment_mode = ireader.GetInt32(CommonColumnsforPayments.payment_mode);
                        obj.isactive = ireader.GetBoolean(CommonColumnsforPayments.isactive);
                        obj.collection_date = ireader.GetNullDateTime(CommonColumnsforPayments.collection_date);
                        obj.SmsSentStatus = ireader.GetBoolean(CommonColumnsforPayments.SmsSentStatus);
                        obj.Message = ireader.GetString(CommonColumnsforPayments.Message);
                        obj.total_payment_amount = ireader.GetDecimal(CommonColumnsforPayments.total_payment_amount);
                        obj.total_invoice_amount_after_gst = ireader.GetDecimal(CommonColumnsforPayments.total_invoice_amount_after_gst);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEditPayment, Parameter : Id={Id}");
                return obj;
            }
            finally
            {

            }
        }

        public long SaveUpdatePayment(PaymentsViewModel obj)
        {
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(obj.id, CommonConstantsforPayment.id, SqlDbType.BigInt);
                    cmd.AddParameters(obj.payment_date, CommonConstantsforPayment.payment_date, SqlDbType.DateTime);
                    cmd.AddParameters(obj.supplier_id, CommonConstantsforPayment.supplier_id, SqlDbType.BigInt);
                    cmd.AddParameters(obj.bank_id, CommonConstantsforPayment.bank_id, SqlDbType.Int);
                    cmd.AddParameters(obj.cheque_number, CommonConstantsforPayment.cheque_number, SqlDbType.VarChar);
                    cmd.AddParameters(obj.rebate_amount, CommonConstantsforPayment.rebate_amount, SqlDbType.Decimal);
                    cmd.AddParameters(obj.actual_payment_amount, CommonConstantsforPayment.actual_payment_amount, SqlDbType.Decimal);
                    cmd.AddParameters(obj.total_payment_amount, CommonConstantsforPayment.total_payment_amount, SqlDbType.Decimal);
                    cmd.AddParameters(obj.remarks, CommonConstantsforPayment.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(obj.created_by, CommonConstantsforPayment.userid, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(obj.payment_mode, CommonConstantsforPayment.payment_mode, SqlDbType.Int);
                    cmd.AddParameters(obj.isactive, CommonConstantsforPayment.is_active, SqlDbType.Bit);
                    cmd.AddParameters(obj.collection_date ?? (object)DBNull.Value, CommonConstantsforPayment.collection_date, SqlDbType.DateTime);
                    if (obj.Message == null)
                        obj.Message = "Test message";
                    cmd.AddParameters(obj.Message, CommonConstantsforPayment.Message, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_UPSERT_PAYMENTS);
                    while (Ireader.Read())
                    {
                        obj.id = Ireader.GetInt64(CommonColumnsforPayments.PaymentId);
                    }
                    return obj.id;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUpdatePayment, Parameter : obj={obj}");
                return obj.id;
            }
            finally
            {

            }
        }

        public bool UpdatePaymentStatus(long paymentId, long supplierId)
        {
            bool isFlag = false;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(paymentId, CommonConstantsforPayment.paymentId, SqlDbType.BigInt);
                    cmd.AddParameters(supplierId, CommonConstantsforPayment.supplierId, SqlDbType.BigInt);
                    isFlag = cmd.ExecuteNonQuery(SqlProcedures.SSP_UpdatePaymentStatus);
                    return isFlag;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdatePaymentStatus, Parameter : paymentId={paymentId}, supplierId={supplierId}");
                return isFlag;
            }
        }

        public bool UpdateBudgetPaymentStatus(long paymentId)
        {
            bool isFlag = false;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(paymentId, CommonConstantsforPayment.paymentId, SqlDbType.BigInt);
                    isFlag = cmd.ExecuteNonQuery(SqlProcedures.SSP_UpdateBudgetStatus);
                    return isFlag;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdateBudgetPaymentStatus, Parameter : paymentId={paymentId}");
                return isFlag;
            }
        }

        public SSP_PaymentById_Result PrintPayment(int Id)
        {
            SSP_PaymentById_Result obj = new SSP_PaymentById_Result();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommonConstantsforPayment.id, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_PaymentById);
                    while (ireader.Read())
                    {
                        obj.id = ireader.GetInt64(CommonColumnsforPayments.id);
                        obj.payment_date = ireader.GetNullDateTime(CommonColumnsforPayments.payment_date);
                        obj.supplier_id = ireader.GetInt64(CommonColumnsforPayments.supplier_id);
                        obj.bank_id = ireader.GetInt32(CommonColumnsforPayments.bank_id);
                        obj.cheque_number = ireader.GetString(CommonColumnsforPayments.cheque_number);
                        obj.rebate_amount = ireader.GetDecimal(CommonColumnsforPayments.rebate_amount);
                        obj.remarks = ireader.GetString(CommonColumnsforPayments.remarks);
                        obj.created_date = ireader.GetDateTime(CommonColumnsforPayments.created_date);
                        obj.created_by = ireader.GetGuid(CommonColumnsforPayments.created_by);
                        obj.modified_date = ireader.GetDateTime(CommonColumnsforPayments.modified_date);
                        obj.modified_by = ireader.GetGuid(CommonColumnsforPayments.modified_by);
                        obj.payment_mode = ireader.GetInt32(CommonColumnsforPayments.payment_mode);
                        obj.isactive = ireader.GetBoolean(CommonColumnsforPayments.isactive);
                        obj.Message = ireader.GetString(CommonColumnsforPayments.Message);
                        obj.supplier_name = ireader.GetString(CommonColumnsforPayments.supplier_name);
                        obj.payment_mode = ireader.GetInt32(CommonColumnsforPayments.payment_mode);
                        obj.branch_name = ireader.GetString(CommonColumnsforPayments.branch_name);
                        obj.address1 = ireader.GetString(CommonColumnsforPayments.address1);
                        obj.phone = ireader.GetString(CommonColumnsforPayments.phone);
                        obj.zip_code = ireader.GetString(CommonColumnsforPayments.zip_code);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PrintPayment, Parameter : Id={Id}");
                return obj;
            }
            finally
            {

            }
        }

        public List<SSP_Loan_Result> LoadLoanGrid(string userId, Int32 branchid, Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, DateTime FromDate, DateTime ToDate)
        {
            List<SSP_Loan_Result> obj1 = new List<SSP_Loan_Result>();
            SSP_Loan_Result obj;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(userId), CommonConstantsforLoanGrid.userId, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(branchid, CommonConstantsforLoanGrid.branchid, SqlDbType.Int);
                    cmd.AddParameters(StartIndex, CommonConstantsforLoanGrid.StartIndex, SqlDbType.Int);
                    cmd.AddParameters(PageSize, CommonConstantsforLoanGrid.PageSize, SqlDbType.Int);
                    cmd.AddParameters(SortBy, CommonConstantsforLoanGrid.SortBy, SqlDbType.VarChar);
                    cmd.AddParameters(OrderBy, CommonConstantsforLoanGrid.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(FromDate, CommonConstantsforLoanGrid.FromDate, SqlDbType.DateTime);
                    cmd.AddParameters(ToDate, CommonConstantsforLoanGrid.ToDate, SqlDbType.DateTime);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Loan);
                    while (ireader.Read())
                    {
                        obj = new SSP_Loan_Result();
                        {
                            obj.Id = ireader.GetInt32(CommonColumnsforLoanGrid.Id);
                            obj.loan_date = ireader.GetString(CommonColumnsforLoanGrid.loan_date);
                            obj.person_name = ireader.GetString(CommonColumnsforLoanGrid.person_name);
                            obj.mode_of_payment = ireader.GetString(CommonColumnsforLoanGrid.mode_of_payment);
                            obj.rec_type = ireader.GetInt32(CommonColumnsforLoanGrid.rec_type);
                            obj.bank_name = ireader.GetString(CommonColumnsforLoanGrid.bank_name);
                            obj.amount = ireader.GetDecimal(CommonColumnsforLoanGrid.amount);
                            obj.CreatedUpdated = ireader.GetString(CommonColumnsforLoanGrid.CreatedUpdated);
                            obj.Verified = ireader.GetBoolean(CommonColumnsforLoanGrid.Verified);
                        };
                        obj1.Add(obj);
                    }
                    return obj1;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadLoanGrid, Parameter : userId={userId}, branchid={branchid}, StartIndex={StartIndex}, PageSize={PageSize}, SortBy={SortBy}, OrderBy={OrderBy}, FromDate={FromDate}, ToDate={ToDate}");
                return obj1;
            }
            finally
            {

            }

        }

        public loan LoadAddEditLoan(int Id)
        {
            loan obj = new loan();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommonConstantsforLoan.Id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_Loans);
                    while (Ireader.Read())
                    {
                        obj.branch_Id = Ireader.GetInt32(CommonColumnsforLoan.branch_Id);
                        obj.LoanDate = Ireader.GetDateTime(CommonColumnsforLoan.LoanDate);
                        obj.rec_type = Ireader.GetInt32(CommonColumnsforLoan.rec_type);
                        obj.person_id = Ireader.GetInt64(CommonColumnsforLoan.person_id);
                        obj.person_type = Ireader.GetString(CommonColumnsforLoan.person_type);
                        obj.purpose = Ireader.GetString(CommonColumnsforLoan.purpose);
                        obj.payment_mode = Ireader.GetInt32(CommonColumnsforLoan.payment_mode);
                        obj.bank_id = Ireader.GetInt32(CommonColumnsforLoan.bank_id);
                        obj.cheque_number = Ireader.GetInt32(CommonColumnsforLoan.cheque_number);
                        obj.amount = Ireader.GetDecimal(CommonColumnsforLoan.amount);
                        obj.created_by = Ireader.GetGuid(CommonColumnsforLoan.created_by);
                        obj.created_on = Ireader.GetDateTime(CommonColumnsforLoan.created_on);
                        obj.updated_by = Ireader.GetGuid(CommonColumnsforLoan.updated_by);
                        obj.updated_on = Ireader.GetDateTime(CommonColumnsforLoan.updated_on);
                        obj.isactive = Ireader.GetBoolean(CommonColumnsforLoan.isactive);
                        obj.project_id = Ireader.GetInt32(CommonColumnsforLoan.project_id);
                        obj.Verified = Ireader.GetBoolean(CommonColumnsforLoan.Verified);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEditLoan, Parameter : Id={Id}");
                return obj;
            }
            finally
            {

            }
        }

        public string SaveUpdateLoan(LoanViewModel loan)
        {
            string result = string.Empty;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(loan.Id, CommonConstantsforLoan.Id, SqlDbType.Int);
                    cmd.AddParameters(loan.branch_Id, CommonConstantsforLoan.branch_Id, SqlDbType.Int);
                    cmd.AddParameters(loan.LoanDate, CommonConstantsforLoan.LoanDate, SqlDbType.DateTime);
                    cmd.AddParameters(loan.rec_type, CommonConstantsforLoan.rec_type, SqlDbType.Int);
                    cmd.AddParameters(loan.person_id, CommonConstantsforLoan.person_id, SqlDbType.BigInt);
                    cmd.AddParameters(loan.person_type, CommonConstantsforLoan.person_type, SqlDbType.Char);
                    cmd.AddParameters((loan.purpose == null) ? "" : loan.purpose, CommonConstantsforLoan.purpose, SqlDbType.VarChar);
                    cmd.AddParameters(loan.payment_mode, CommonConstantsforLoan.payment_mode, SqlDbType.Int);
                    cmd.AddParameters(loan.bank_id, CommonConstantsforLoan.bank_id, SqlDbType.Int);
                    cmd.AddParameters(loan.cheque_number, CommonConstantsforLoan.cheque_number, SqlDbType.Int);
                    cmd.AddParameters(loan.amount, CommonConstantsforLoan.amount, SqlDbType.Decimal);
                    cmd.AddParameters(loan.created_by, CommonConstantsforLoan.userid, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(loan.created_on, CommonConstantsforLoan.currentdate, SqlDbType.DateTime);
                    cmd.AddParameters(loan.isactive, CommonConstantsforLoan.isactive, SqlDbType.Bit);
                    cmd.AddParameters(loan.project_id, CommonConstantsforLoan.project_id, SqlDbType.Int);
                    cmd.AddParameters(loan.Verified, CommonConstantsforLoan.Verified, SqlDbType.Bit);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Upsert_Loans);
                    while (Ireader.Read())
                    {
                        result = Ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdateLoan, Parameter : objView={loan}");
                return null;
            }
            finally
            {

            }
        }

        public string SubmitVerifiedVal(Int64 Id, bool Verified)
        {
            string result = string.Empty;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommonConstantsforLoan.Id, System.Data.SqlDbType.Int);
                    cmd.AddParameters(Verified, CommonConstantsforLoan.Verified, SqlDbType.Bit);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Upsert_VerifiedStatus);
                    while (Ireader.Read())
                    {
                        result = Ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<string> InvoiceList(Int64 projectid, Int64 supplierid, Int32 branchid)
        {
            List<string> InvList = new List<string>();
            string obj;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(projectid, CommonConstantsforLoan.project_id, SqlDbType.Int);
                    cmd.AddParameters(supplierid, CommonConstantsforLoan.supplier_id, SqlDbType.Int);
                    cmd.AddParameters(branchid, CommonConstantsforLoan.branch_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.ssp_invoicelist);
                    while (Ireader.Read())
                    {
                        obj = Ireader.GetString(CommonColumnsforLoan.supplier_inv_number);
                        InvList.Add(obj);
                    }
                    return InvList;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: InvoiceList, Parameter : projectid={projectid}, supplierid={supplierid}, branchid={branchid}");
                return InvList;
            }
            finally
            {

            }

        }



        public List<payment_detail> LoadpaymentList(string userId)
        {
            List<payment_detail> obj1 = new List<payment_detail>();
            payment_detail obj;
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(Convert.ToUInt64(userId), CommonConstantsforLoanGrid.PaymentId, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.ssp_print);
                    while (ireader.Read())
                    {
                        obj = new payment_detail();
                        {
                            obj.supplier_name = ireader.GetString(CommonColumnsforLoanGrid.supplier_name);
                            obj.supplier_inv_number = ireader.GetString(CommonColumnsforLoanGrid.supplier_inv_number);
                            obj.invoice_amount = ireader.GetDecimal(CommonColumnsforLoanGrid.invoice_amount);
                            obj.gst_amount = ireader.GetDecimal(CommonColumnsforLoanGrid.gst_amount);
                            obj.gst_percentage = ireader.GetDecimal(CommonColumnsforLoanGrid.gst_percentage);
                            obj.payment_amount = ireader.GetDecimal(CommonColumnsforLoanGrid.payment_amount);                            
                            //obj.payment_id = ireader.GetString(CommonColumnsforLoanGrid.bank_name);
                            //obj.amount = ireader.GetDecimal(CommonColumnsforLoanGrid.amount);
                            //obj.CreatedUpdated = ireader.GetString(CommonColumnsforLoanGrid.CreatedUpdated);
                            //obj.Verified = ireader.GetBoolean(CommonColumnsforLoanGrid.Verified);
                        };
                        obj1.Add(obj);
                    }
                    return obj1;
                }
            }
            catch (Exception ex)
            {
                //ExceptionLog.WriteLog(ex, $"Method Name: LoadLoanGrid, Parameter : userId={userId}, branchid={branchid}, StartIndex={StartIndex}, PageSize={PageSize}, SortBy={SortBy}, OrderBy={OrderBy}, FromDate={FromDate}, ToDate={ToDate}");
                ExceptionLog.WriteLog(ex, $"Method Name: LoadLoanGrid, Parameter : userId={userId}");
                return obj1;
            }
            finally
            {

            }

        }

        /*public string SaveLoan(LoanViewModel loan)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                cmd.AddParameters(loan.branch_Id, CommonConstantsforLoan.branch_Id, SqlDbType.VarChar);
                cmd.AddParameters(loan.LoanDate, CommonConstantsforLoan.LoanDate, SqlDbType.VarChar);
                cmd.AddParameters(loan.rec_type, CommonConstantsforLoan.rec_type, SqlDbType.VarChar);
                cmd.AddParameters(loan.person_id, CommonConstantsforLoan.person_id, SqlDbType.VarChar);
                cmd.AddParameters(loan.person_type, CommonConstantsforLoan.person_type, SqlDbType.VarChar);
                cmd.AddParameters(loan.purpose, CommonConstantsforLoan.purpose, SqlDbType.VarChar);
                cmd.AddParameters(loan.payment_mode, CommonConstantsforLoan.payment_mode, SqlDbType.VarChar);
                cmd.AddParameters(loan.bank_id, CommonConstantsforLoan.bank_id, SqlDbType.VarChar);
                cmd.AddParameters(loan.cheque_number, CommonConstantsforLoan.cheque_number, SqlDbType.VarChar);
                cmd.AddParameters(loan.amount, CommonConstantsforLoan.amount, SqlDbType.VarChar);
                cmd.AddParameters(loan.created_by, CommonConstantsforLoan.created_by, SqlDbType.VarChar);
                cmd.AddParameters(loan.created_on, CommonConstantsforLoan.created_on, SqlDbType.VarChar);
                cmd.AddParameters(loan.updated_on, CommonConstantsforLoan.updated_on, SqlDbType.VarChar);
                cmd.AddParameters(loan.updated_by, CommonConstantsforLoan.updated_by, SqlDbType.VarChar);
                cmd.AddParameters(loan.isactive, CommonConstantsforLoan.isactive, SqlDbType.VarChar);
                cmd.AddParameters(loan.project_id, CommonConstantsforLoan.project_id, SqlDbType.VarChar);
                cmd.AddParameters(loan.Verified, CommonConstantsforLoan.Verified, SqlDbType.VarChar);
                IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_UPDATE_Loans);
                while (Ireader.Read())
                {
                    result = Ireader.GetString(CommonColumns.Result);
                }
                return result;
            }
        }*/
    }
}