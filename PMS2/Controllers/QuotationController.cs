using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PMS.Common;
using PMS.Data_Access;
using PMS.Interface;
using PMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WhatsAppApi;


namespace PMS.Controllers
{
    public class QuotationController : Controller
    {
        private IQuotation _IQuotation;

        public QuotationController()
        {
            _IQuotation = new DataLayer();
        }
        // GET: Quotation
        public ActionResult Quotation()
        {
            string message = string.Empty;
            Quotation _obj = new Quotation();
            SalesmanDropDown salesmen = new SalesmanDropDown();
            DateTime now = DateTime.Now;
            string uid = string.Empty;
            try
            {
                _obj.from_date = DateTime.Today.AddMonths(-1).ToShortDateString();
                _obj.to_date = DateTime.Today.ToShortDateString();
                if (User.IsInRole("Salesman"))
                {
                    uid = User.Identity.GetUserId();
                    //salesmen = _IQuotation.GetSalesmenIdByUserId(uid);
                    //_obj.id = salesmen.id;
                    //_obj.salesmen_name = salesmen.salesmen_name;
                    List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenByUserIdExistsInUsersTbl(uid);
                    _obj.id = Convert.ToInt32(salesMenUsrs[0].Value);
                    _obj.salesmen_name = salesMenUsrs[0].Text;
                    _obj.role = "Salesman";
                }
                return View(_obj);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Quotation");
                message = ex.Message;
                return null;
            }
            finally
            {
                _obj = null;
                salesmen = null;
            }
        }
        public ActionResult QuotationsList(string JsonValues)
        {
            string message = string.Empty;
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            // var ld = JsonConvert.DeserializeObject<EoiDraftViewModel>(json, dateTimeConverter);
            QuotationListCriteria _QuotationListCriteria = JsonConvert.DeserializeObject<QuotationListCriteria>(JsonValues, dateTimeConverter);
            List<QuotationList> _QuotationsList = new List<QuotationList>();
            try
            {
                //_QuotationListCriteria.fromdate = DateTime.Today.AddMonths(-1);
                //_QuotationListCriteria.todate = DateTime.Today;
                _QuotationListCriteria.Type = 1;
                if (User.IsInRole("Salesman"))
                {
                    string uid = User.Identity.GetUserId();
                    List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenByUserIdExistsInUsersTbl(uid);
                    _QuotationListCriteria.salesMenId = Convert.ToInt32(salesMenUsrs[0].Value);
                    _QuotationListCriteria.branchId = Common.SessionManagement.SelectedBranchID;
                }

                _QuotationsList = _IQuotation.QuotationsList(_QuotationListCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: QuotationsList");
                message = ex.Message;
            }
            finally
            {
                _QuotationListCriteria = null;
            }
            var data = new
            {
                Items = _QuotationsList,
                TotalCount = _QuotationsList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        public ActionResult LoadDescription(string description, string Id)
        {
            AdditionalDescription objView = new AdditionalDescription();
            string txtDescription = "";
            string AdditionalDescription = "";
            try
            {
                if (Id != "")
                {
                    AdditionalDescription = _IQuotation.GetAdditionDescription(Id);
                }
                if (AdditionalDescription == "")
                {
                    if (Common.Constants.AdditionalDescription != null && Common.Constants.AdditionalDescription != "")
                    {
                        txtDescription = Common.Constants.AdditionalDescription;
                    }
                    else
                    {
                        txtDescription = HttpUtility.UrlDecode(description.ToString(), Encoding.Default);
                    }
                }
                else
                {
                    txtDescription = AdditionalDescription;
                }

                objView.Description = txtDescription;
                objView.Id = Id;
                //objView.Description = description;
                return View(objView);
            }
            catch
            {
                return View(objView);
            }
            finally
            {
                Common.Constants.AdditionalDescription = "";
            }
        }

        
        public JsonResult SaveDescription(string description,string Id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            bool bRetval = true;
            try
            {
                Common.Constants.AdditionalDescription = HttpUtility.UrlDecode(description, Encoding.Default);
                if (Id != null && Id.Length >0)
                {
                    bRetval = _IQuotation.UpsertAdditionDescription(Common.Constants.AdditionalDescription, Id);

                }
                if (bRetval)
                {
                    _SuccessMessage.Result = "sucess";
                    _SuccessMessage.Errormessage = "Sucessfully Updated Description";
                    _SuccessMessage.Id = "1";
                }
                else
                {
                    _SuccessMessage.Result = "error";
                    _SuccessMessage.Errormessage = "Sucessfully Updated Failed";
                    _SuccessMessage.Id = "0";
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SaveDescription");
                _SuccessMessage.Errormessage = ex.Message;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
                
        [HttpPost]
        public JsonResult UpsertProjectQuotation(string JsonQuotation)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            CreateQuotationCriteria _CreateQuotationCriteria = JsonConvert.DeserializeObject<CreateQuotationCriteria>(JsonQuotation);

            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.UpsertProjectQuotation(_CreateQuotationCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertQuotationFromPackage(string JsonValues)
        {
            string message = string.Empty;
            string uid = string.Empty;
            QuotationFromPackageResponse _QuotationFromPackageResponse = new QuotationFromPackageResponse();

            try
            {
                QuotationFromPackageCriteria _CreateQuotationCriteria = JsonConvert.DeserializeObject<QuotationFromPackageCriteria>(JsonValues);
                uid = User.Identity.GetUserId();
                _QuotationFromPackageResponse = _IQuotation.UpsertQuotationFromPackage(_CreateQuotationCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertQuotationFromPackage");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _QuotationFromPackageResponse }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpsertProjectDetails(string JsonQuotationDetails, string ProjectId, string TaskId, string TaskName)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            // string uid = string.Empty;
            QuotationUpsertProjectDetails _CreateQuotationCriteria = JsonConvert.DeserializeObject<QuotationUpsertProjectDetails>(JsonQuotationDetails);
            _CreateQuotationCriteria.AdditionalDescription = Common.Constants.AdditionalDescription;
          
            //string DescriptionTest = ViewData["AdditionalDescription"].ToString();
            try
            {
                //if (_CreateQuotationCriteria.project_id == "" && _CreateQuotationCriteria.Task_Id == "" && _CreateQuotationCriteria.project_det_Id == "")
                //{
                //    _CreateQuotationCriteria.project_id = ProjectId;
                //    _CreateQuotationCriteria.Task_Id = TaskId;
                //    _CreateQuotationCriteria.project_det_Id = "00000000-0000-0000-0000-000000000000";
                //}
                if (_CreateQuotationCriteria.project_id == "")
                {
                    _CreateQuotationCriteria.project_id = ProjectId;
                }
                if (_CreateQuotationCriteria.Task_Id == "" || _CreateQuotationCriteria.Task_Id == "0" || _CreateQuotationCriteria.Task_Id == null)
                {
                    _CreateQuotationCriteria.Task_Id = TaskId;
                }
                if (_CreateQuotationCriteria.project_det_Id == "")
                {
                    _CreateQuotationCriteria.project_det_Id = "00000000-0000-0000-0000-000000000000";
                }
                _CreateQuotationCriteria.Task_Name = TaskName;
                _CreateQuotationCriteria.userId = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.UpsertProjectDetails(_CreateQuotationCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
                Common.Constants.AdditionalDescription = "";
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectTasks()
        {
            string message = string.Empty;
            ProjectTasksCriteria _ProjectTasksCriteria = new ProjectTasksCriteria();
            List<ProjectTasksList> _ProjectTasksList = new List<ProjectTasksList>();
            try
            {
                _ProjectTasksList = _IQuotation.GetProjectTasks(_ProjectTasksCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasks");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksCriteria = null;
            }
            var data = new
            {
                Items = _ProjectTasksList,
                TotalCount = _ProjectTasksList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProjectAmountList()
        {
            string message = string.Empty;
            ProjectTasksCriteria _ProjectTasksCriteria = new ProjectTasksCriteria();
            List<ProjectAmountList> _ProjectAmountList = new List<ProjectAmountList>();
            try
            {
                _ProjectAmountList = _IQuotation.ProjectAmountList(_ProjectTasksCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ProjectAmountList");
                message = ex.Message;
            }
            finally
            {
                _ProjectAmountList = null;
            }
            var data = new
            {
                Items = _ProjectAmountList,
                TotalCount = _ProjectAmountList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectTasksItem(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IQuotation.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItem");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _ProjectTasksItemList,
                TotalCount = _ProjectTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectTasksQuotationItem(string ProjectId, string TaskId, bool IsFromPackage)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IQuotation.GetProjectTasksQuotationItem(_ProjectTasksItemCriteria, IsFromPackage);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItem");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _ProjectTasksItemList,
                TotalCount = _ProjectTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectTasksListItem(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IQuotation.GetProjectTasksListItem(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksListItem");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            //var data = new
            //{
            //    Items = _ProjectTasksItemList,
            //    TotalCount = _ProjectTasksItemList.Count
            //};
            return Json(new { data = _ProjectTasksItemList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProjectTasksItemDetails(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IQuotation.GetProjectTasksItemDetails(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItemDetails");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _ProjectTasksItemList,
                TotalCount = _ProjectTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectTasksQuotationItemDetails(string ProjectId, string TaskId,bool IsFromPackage)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IQuotation.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, IsFromPackage);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItemDetails");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _ProjectTasksItemList,
                TotalCount = _ProjectTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindProjectStatus(String Type)
        {
            string message = string.Empty;
            List<Quotation> _BindProjectStatus = new List<Quotation>();
            try
            {
                _BindProjectStatus = _IQuotation.BindProjectStatus(Type);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindProjectStatus");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindProjectStatus }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult BindSalesmen()
        //{
        //    string message = string.Empty;
        //    List<QuotationList> _BindSalesmen = new List<QuotationList>();
        //    try
        //    {
        //        _BindSalesmen = _IQuotation.BindSalesmen();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: BindSalesmen");
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    return Json(new { data = _BindSalesmen }, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult BindCustomer()
        //{
        //    string message = string.Empty;
        //    List<QuotationList> _BindCustomer = new List<QuotationList>();
        //    try
        //    {
        //        _BindCustomer = _IQuotation.BindCustomer();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: BindCustomer");
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //    }
        //    return Json(new { data = _BindCustomer }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult NewQuotation()
        {
            Quotation _obj = new Quotation();
            SalesmanDropDown salesmen = new SalesmanDropDown();
            DateTime now = DateTime.Now;
            string uid = string.Empty;
            try
            {
                ViewBag.date = DateTime.Today.ToShortDateString();
                if (User.IsInRole("Salesman"))
                {
                    uid = User.Identity.GetUserId();
                    //salesmen = _IQuotation.GetSalesmenIdByUserId(uid);
                    //_obj.id = salesmen.id;
                    //_obj.salesmen_name = salesmen.salesmen_name;
                    List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenByUserIdExistsInUsersTbl(uid);
                    _obj.id = Convert.ToInt32(salesMenUsrs[0].Value);
                    _obj.salesmen_name = salesMenUsrs[0].Text;

                    _obj.role = "Salesman";
                }
                return View(_obj);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: NewQuotation");
                return null;
            }
            finally
            {
            }
            //return View();
        }

        public JsonResult BindSalesmenForQuotation()
        {
            string message = string.Empty;
            List<SalesmanDropDown> _BindSalesmen = new List<SalesmanDropDown>();
            try
            {
                string uid = User.Identity.GetUserId();
                if (User.IsInRole("Salesman"))
                {
                    List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenByUserIdExistsInUsersTbl(uid);
                    SalesmanDropDown slsdd = new SalesmanDropDown();
                    slsdd.id = Convert.ToInt32(salesMenUsrs[0].Value);
                    slsdd.salesmen_name = salesMenUsrs[0].Text;
                    _BindSalesmen.Add(slsdd);
                }
                else
                {
                    _BindSalesmen = CommonFunction.BindSalesmenForQuotation();
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmen");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindSalesmen }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult QuotationDetail(string ProjectId, Boolean ShowHide)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            string forProjectStatus = "";
            try
            {
                if (!ShowHide)
                {
                    forProjectStatus = CommonFunction.GetProjectStatus(ProjectId);
                    string PrjId = CommonFunction.GetProjectGuidByID(ProjectId);
                    ProjectId = PrjId;
                    ShowHide = true;
                }
                ViewBag.ProjectId = ProjectId;
                ViewBag.ShowHide = ShowHide;
                SessionManagement.AdditionalDescription = "test";
                _QuotationDetails = _IQuotation.GetQuotationDetailsByProjectId(ProjectId);
                //_QuotationDetails.fromDate = _PackageList.valid_from.ToShortDateString();
                //_QuotationDetails.toDate = _PackageList.valid_to.ToShortDateString();
                if (forProjectStatus == "5")
                    return RedirectToAction("ContractDetail", "Contract", new { ProjectId = ProjectId });
                else
                    return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: QuotationDetail");
                return null;
            }
            finally
            {

            }
        }        
        public JsonResult GetQuotationpaymentterms(string ProjectId)
        {
            string message = string.Empty;
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            try
            {
                _GetProjectpaymentterms = _IQuotation.GetProjectpaymentterms(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationpaymentterms");
                message = ex.Message;
            }
            finally
            {
                ProjectId = null;
            }
            var data = new
            {
                Items = _GetProjectpaymentterms,
                TotalCount = _GetProjectpaymentterms.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertQuotationPaymentTerms(string JsonQuotation, string ProjectId)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            //List<PaymentTerms> _ProjectPaymentTermsCriteria = JsonConvert.DeserializeObject<List<PaymentTerms>>(obj[0]);
            PaymentTerms _ProjectPaymentTermsCriteria = JsonConvert.DeserializeObject<PaymentTerms>(JsonQuotation);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.UpsertProjectPaymentTerms(_ProjectPaymentTermsCriteria, uid, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertQuotationPaymentTerms");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _ProjectPaymentTermsCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpsertProjectTasks(FormCollection obj, string ProjectId)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<ProjectTasksItemList> _ProjectTasksCriteria = JsonConvert.DeserializeObject<List<ProjectTasksItemList>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IQuotation.UpsertProjectTasks(_ProjectTasksCriteria, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectTasks");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSalesmenDetailsById(string Salesmen_Id)
        {
            string message = string.Empty;
            List<Salesmancommission> _BindSalesmen = new List<Salesmancommission>();
            try
            {
                _BindSalesmen = _IQuotation.GetSalesmenDetailsById(Salesmen_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenDetailsById");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindSalesmen }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerDetailsById(string customer_id)
        {
            string message = string.Empty;
            List<CustomerDetailsById> _BindCustomer = new List<CustomerDetailsById>();
            try
            {
                _BindCustomer = _IQuotation.GetCustomerDetailsById(customer_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetCustomerDetailsById");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindCustomer }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_ProjectStatus(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            QuotationStatusCriteria _QuotationStatusCriteria = JsonConvert.DeserializeObject<QuotationStatusCriteria>(JsonValues);
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string Project_Id = _QuotationStatusCriteria.project_id;
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IQuotation.Update_ProjectStatus(_QuotationStatusCriteria, uid);
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IQuotation.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_Id, Common.Constants.GetTransaction.Quotation);
                }
                else
                    emailAddress = _IQuotation.GetAdminAndSalesmenEmailAddress("", Project_Id, Common.Constants.GetTransaction.Quotation);
                bool ShowHide = true;
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                SendEmailWithPDF(emailIds, "[Test] -Quotation has been accepted ", "", "", _QuotationStatusCriteria, ShowHide, "accepted", Project_Id, "00000000-0000-0000-0000-000000000000");
                //SendEmail(emailIds, "[Test] -Quotation has been accepted ", "", "", _QuotationStatusCriteria, ShowHide, "accepted"); //dirPath + fileNameWithExt, fileNameWithExt
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_ProjectStatus");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _QuotationStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProjectQuotation(string ProjectId)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.DeleteProjectQuotation(uid, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProjectQuotation");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProjectDetails(string Project_det_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.DeleteProjectDetails(uid, Project_det_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProjectDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProject_QuotationPaymentTermsByID(string Payment_term_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.DeleteProjectPaymentTermsByID(uid, Payment_term_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProject_ContractPaymentTermsByID");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public void PrintPreview(string Id, string TaskId, Boolean Price)
        {

            byte[] bytes;
            int z = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            StyleSheet styles = new StyleSheet();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);
            try
            {
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    #region PDF

                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter();// headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;

                    _QuotationsList = _IQuotation.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);
                    if (_QuotationsList != null)
                    {
                        // Table
                        table = new PdfPTable(6);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 8f, 30f, 8f, 10f, 14f, 20f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Name : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].customer, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("NRIC : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].NRIC, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Reference No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].project_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("JobSite : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].Jobsite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].quotationForwardDate.ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Email : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].Email, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("H/P : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].Phone, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                    }
                    //Table
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;


                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Renovation Quotation", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;

                    cell.AddElement(para);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;


                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Package Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para1 = new Paragraph(chunk);
                    para1.Alignment = Element.ALIGN_LEFT;
                   

                    cell.AddElement(para1);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 5f;

                    cell = new PdfPCell();
                    chunk = new Chunk("Item", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    chunk = new Chunk("Description", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    cell = new PdfPCell();
                    chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    cell = new PdfPCell();
                    chunk = new Chunk("Uom", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    if (Price == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Unit Price", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        // table.AddCell("Qty");
                    }



                    cell = new PdfPCell();
                    chunk = new Chunk("Amount", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);                                      


                    if (Price == true)
                    {
                        columnWidths = new float[] { 6f, 48f, 6f, 8f, 12f, 10f };
                    }
                    else
                    {
                        columnWidths = new float[] { 6f, 59f, 10f, 8f, 13f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IQuotation.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, true);
                    _ProjectTasksItemDetailList = _IQuotation.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);
                    
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {
                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 7;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _ProjectTasksItemDetailList.Count; j++)
                            {
                                if (_ProjectTasksItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_ProjectTasksItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);


                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }

                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            //PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //table.AddCell(cell3);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                        }
                                    }
                                    z++;
                                    amount = amount + _ProjectTasksItemDetailList[j].Amount;
                                }
                                else
                                {
                                    z = 1;
                                }

                            }


                        }
                        
                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);
                    }
                    
                    
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;
                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Quotation Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para2 = new Paragraph(chunk);
                    para2.Alignment = Element.ALIGN_LEFT;
                  

                    cell.AddElement(para2);
                    cell.Border = 0;
                    table.AddCell(cell);

                    //Add table to document
                    pdfDoc.Add(table);

                    //Table
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 5f;                   

                    cell = new PdfPCell();
                    chunk = new Chunk("Item", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    chunk = new Chunk("Description", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //table.AddCell("Item");
                    //table.AddCell("Description");

                    cell = new PdfPCell();
                    chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    cell = new PdfPCell();
                    chunk = new Chunk("Uom", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    if (Price == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Unit Price", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        // table.AddCell("Qty");
                    }



                    cell = new PdfPCell();
                    chunk = new Chunk("Amount", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);



                    if (Price == true)
                    {
                        columnWidths = new float[] { 6f, 48f, 6f, 8f, 12f, 10f};
                    }
                    else
                    {
                        columnWidths = new float[] { 6f, 59f, 10f, 8f, 13f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IQuotation.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
                    _ProjectTasksItemDetailList = _IQuotation.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 7;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _ProjectTasksItemDetailList.Count; j++)
                            {
                                if (_ProjectTasksItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_ProjectTasksItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description , boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "" )
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);


                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {                                           
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }
                                         
                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "" )
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);
                                            
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            //PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //table.AddCell(cell3);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                            
                                        }
                                    }
                                    z++;
                                    amount = amount + _ProjectTasksItemDetailList[j].Amount;
                                }
                                else
                                {
                                    z = 1;
                                }
                               
                            }                           


                        }


                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);


                    }

                    _QuotationDetails = _IQuotation.GetQuotationDetailsByProjectId(Id);

                    if (_QuotationDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;

                        chunk = new Chunk("Sub Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.contract_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_QuotationDetails.discount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _QuotationDetails.discount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_QuotationDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Grand Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(chunk);
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                    }

                    _GetProjectpaymentterms = _IQuotation.GetProjectpaymentterms(Id);

                    if (_GetProjectpaymentterms.Count != 0)
                    {
                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        //0=Left, 1=Centre, 2=Right
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 5f;
                        table.SpacingAfter = 5f;

                        //Cell no 2
                        cell = new PdfPCell();
                        chunk = new Chunk("Payment Terms :", FontFactory.GetFont("Times New Roman", "serif", 12, Font.BOLD, BaseColor.BLACK));
                        cell.AddElement(chunk);
                        //cell.Colspan = ;
                        cell.Border = 0;
                        //cell.BackgroundColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        for (int i = 0; i < _GetProjectpaymentterms.Count; i++)
                        {
                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1).ToString() + ". " + _GetProjectpaymentterms[i].paymentdescription.Master_payment_description, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                            cell = new PdfPCell();
                            cell.Border = 0;
                            cell.AddElement(chunk);
                            table.AddCell(cell);

                            //table.AddCell((i + 1).ToString() + ". " + _GetProjectpaymentterms[i].paymentdescription.Master_payment_description);
                            //table.AddCell();
                        }

                        //Add table to document
                        pdfDoc.Add(table);
                    }

                    //For Starting From New Page 
                    pdfDoc.NewPage();                  

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    #endregion

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Quotation.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PrintPreview");
            }
            finally
            {
                bytes = null;
                z = 1;
                _ProjectTasksItemCriteria = null;
                _QuotationsList = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            //return View();
        }

        public class PDFFooter : PdfPageEventHelper
        {
            // write on top of document
            //public override void OnOpenDocument(PdfWriter pdfWriter, Document pdfDoc)
            //{
            //    base.OnOpenDocument(pdfWriter, pdfDoc);
            //    PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            //    tabFot.SpacingAfter = 10F;
            //    PdfPCell cell;
            //    tabFot.TotalWidth = 300F;
            //    cell = new PdfPCell(new Phrase("Header"));
            //    tabFot.AddCell(cell);
            //    tabFot.WriteSelectedRows(0, -1, 150, pdfDoc.Top, pdfWriter.DirectContent);
            //}

            //// write on start of each page
            //public override void OnStartPage(PdfWriter pdfWriter, Document pdfDoc)
            //{
            //    base.OnStartPage(pdfWriter, pdfDoc);
            //}

            //// write on end of each page
            public override void OnEndPage(PdfWriter pdfWriter, Document pdfDoc)
            {

                PdfPTable tbHeader = new PdfPTable(2);
                tbHeader.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
                tbHeader.DefaultCell.Border = 0;
                //tbHeader.SpacingAfter = 150f;
                Chunk chunk = new Chunk();
                // PdfPCell cell = new PdfPCell();
                // PdfPTable table;// = new PdfPTable()
                //pdfDoc.Add(chunk);

                //Table
                // PdfPTable table = new PdfPTable(2);
                tbHeader.WidthPercentage = 100;
                //0=Left, 1=Centre, 2=Right
                tbHeader.HorizontalAlignment = 0;
                //tbHeader.SpacingBefore = 250f;
                tbHeader.SpacingAfter = 100;


                //Cell no 1
                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + "/Content/img/print_logo_new.png");
                image.ScaleAbsolute(200, 150);
                // image.SetAbsolutePosition(200, 150);
                // image.ScaleToFit(200, 150);
                // cell.PaddingBottom =89;
                cell.AddElement(image);
                tbHeader.AddCell(cell);

                //Cell no 2
                chunk = new Chunk("HDB | CONDOMINIUM | LANDED | COMMERCIAL   design4space.com.sg", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                cell = new PdfPCell();
                cell.Border = 0;
                var para6 = new Paragraph(chunk);
                para6.Alignment = Element.ALIGN_RIGHT;
                //para6.SpacingAfter = 100;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.AddElement(para6);
                tbHeader.AddCell(cell);

                //Add table to document               

                //tbHeader.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfWriter.PageSize.GetTop(pdfDoc.TopMargin) + 30, pdfWriter.DirectContent);
                tbHeader.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfDoc.PageSize.Height - 10, pdfWriter.DirectContent);
                // tbHeader.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfDoc.Top, pdfWriter.DirectContent);               // tbHeader.WriteSelectedRows(0, -1, 10, pdfDoc.Height - cellHeight + head.TotalHeight, writer.DirectContent);
                //pdfDoc.Add(tbHeader);
                //Add table to document
                //pdfDoc.Add(tbHeader);

                PdfPTable tbFooter = new PdfPTable(1);
                tbFooter.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
                tbFooter.DefaultCell.Border = 0;
                //Chunk chunk = new Chunk();


                ////Cell no 1
                //PdfPCell cell = new PdfPCell();               

                //Cell no 2
                cell = new PdfPCell();
                chunk = new Chunk("Page No : " + pdfWriter.PageNumber, FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK));
                var para = new Paragraph(chunk);
                para.Alignment = Element.ALIGN_RIGHT;
                cell.Border = 0;
                cell.AddElement(para);
                tbFooter.AddCell(cell);

                tbFooter.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfWriter.PageSize.GetBottom(pdfDoc.BottomMargin) - 5, pdfWriter.DirectContent);
                //tbFooter.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfWriter.PageSize.GetBottom(pdfDoc.BottomMargin) - 40, pdfWriter.DirectContent);

                //PdfPTable tbFooter = new PdfPTable(2);
                //tbFooter.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
                //tbFooter.DefaultCell.Border = 0;
                ////tbFooter.AddCell(new Paragraph());
                //cell = new PdfPCell(new Paragraph("Page :" + pdfWriter.PageNumber));
                //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                //tbFooter.AddCell(cell);
                //tbFooter.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, pdfWriter.PageSize.GetBottom(pdfDoc.BottomMargin) - 150, pdfWriter.DirectContent);
                // }
                //    base.OnEndPage(pdfWriter, pdfDoc);
                //    PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                //    PdfPCell cell;
                //    tabFot.TotalWidth = 300F;
                //    cell = new PdfPCell(new Phrase("Footer"));
                //    tabFot.AddCell(cell);
                //    tabFot.WriteSelectedRows(0, -1, 150, pdfDoc.Bottom, pdfWriter.DirectContent);
            }

            ////write on close of document
            //public override void OnCloseDocument(PdfWriter pdfWriter, Document pdfDoc)
            //{
            //    base.OnCloseDocument(pdfWriter, pdfDoc);
            //}
        }

        //public class HeaderFooter : iTextSharp.text.pdf.PdfPageEventHelper
        //{
        //    public override void onEndPage(PdfWriter writer, Document pdfDoc)
        //    {
        //        PdfPTable tbHeader = new PdfPTable(2);
        //        tbHeader.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
        //        tbHeader.DefaultCell.Border = 0;
        //        Chunk chunk = new Chunk();
        //        // pdfDoc.Add(chunk);

        //        //Table
        //        //PdfPTable table = new PdfPTable(2);
        //        //table.WidthPercentage = 100;
        //        ////0=Left, 1=Centre, 2=Right
        //        //table.HorizontalAlignment = 0;
        //        //table.SpacingBefore = 5f;
        //        //table.SpacingAfter = 5f;

        //        //Cell no 1
        //        PdfPCell cell = new PdfPCell();
        //        cell.Border = 0;
        //        Image image = Image.GetInstance("~/Content/img/print_logo_new.png");
        //        image.ScaleAbsolute(200, 150);
        //        cell.AddElement(image);
        //        tbHeader.AddCell(cell);

        //        //Cell no 2
        //        chunk = new Chunk("HDB | CONDOMINIUM | LANDED | COMMERCIAL", FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.PINK));
        //        cell = new PdfPCell();
        //        cell.Border = 0;
        //        cell.AddElement(chunk);
        //        tbHeader.AddCell(cell);

        //        tbHeader.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, writer.PageSize.GetTop(pdfDoc.TopMargin) + 40, writer.DirectContent);

        //        //Add table to document
        //        //pdfDoc.Add(tbHeader);

        //        PdfPTable tbFooter = new PdfPTable(2);
        //        tbFooter.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
        //        tbFooter.DefaultCell.Border = 0;
        //        tbFooter.AddCell(new Paragraph());
        //        cell = new PdfPCell(new Paragraph("Page :" + writer.PageNumber));
        //        cell.HorizontalAlignment = Element.ALIGN_RIGHT;

        //        tbFooter.WriteSelectedRows(0, -1, pdfDoc.LeftMargin, writer.PageSize.GetBottom(pdfDoc.BottomMargin) - 5, writer.DirectContent);
        //    }
        //}


        [HttpPost]
        public JsonResult UpsertProjectQuotation_Clone(string JsonQuotation)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            CreateQuotationCriteria _CreateQuotationCriteria = JsonConvert.DeserializeObject<CreateQuotationCriteria>(JsonQuotation);

            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.UpsertProjectQuotation_Clone(_CreateQuotationCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation_Clone");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertProjectQuotation_History(string JsonQuotation)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            CreateQuotationCriteria _CreateQuotationCriteria = JsonConvert.DeserializeObject<CreateQuotationCriteria>(JsonQuotation);

            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IQuotation.UpsertProjectQuotation_History(_CreateQuotationCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation_History");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PrintPreview(string Id, string TaskId)
        //{
        //    IContract _IContract = new DataLayer();
        //    ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
        //    _ProjectTasksItemCriteria.project_Id = Id;
        //    _ProjectTasksItemCriteria.Task_Id = TaskId;
        //    dynamic mymodel = new ExpandoObject();
        //    mymodel.projlist = _IQuotation.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);
        //    var tasklist = _IQuotation.GetProjectTasksItemDetails(_ProjectTasksItemCriteria);
        //    var tasklist1 = from t in tasklist select t.Task_Name;
        //    mymodel.tasknamelist = tasklist1.Distinct();
        //    //mymodel.tasknamelist = tasknamelist;
        //    mymodel.tasklist = tasklist;
        //    mymodel.paymentterms = _IQuotation.GetProjectpaymentterms(_ProjectTasksItemCriteria.project_Id);
        //    mymodel.ContractTerms = _IContract.GetProjectContractTermsList(_ProjectTasksItemCriteria.project_Id);
        //    return View(mymodel);
        //}

        [HttpPost]
        public JsonResult SendWhatsappSMS(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            QuotationStatusCriteria _QuotationStatusCriteria;
            List<string> PhoneList = new List<string>();
            try
            {
                _QuotationStatusCriteria = JsonConvert.DeserializeObject<QuotationStatusCriteria>(JsonValues);
                PhoneList.Add("+918500239423");               
                //string content = PrepareEmailBody("", _QuotationStatusCriteria, false, "");
                string message = WhatsAppService.SendWhatsAppSMS("+6590030965", "The Quotation Has Been Created");
                //string message = SendMessage("+918790823051", "HI PMS2");
                _successMessage.Errormessage = message;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendWhatsappSMS");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _QuotationStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendingMail(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            QuotationStatusCriteria _QuotationStatusCriteria;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            
            try
            {
                _QuotationStatusCriteria = JsonConvert.DeserializeObject<QuotationStatusCriteria>(JsonValues);
                string Projectid = _QuotationStatusCriteria.project_id;
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IQuotation.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Projectid, Common.Constants.GetTransaction.Quotation);

                }
                else
                    emailAddress = _IQuotation.GetAdminAndSalesmenEmailAddress("", Projectid, Common.Constants.GetTransaction.Quotation);
                bool ShowHide = true;
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));

                SendEmailWithPDF(emailIds, "[Test]-Quotation Details ", "", "", _QuotationStatusCriteria, ShowHide, "accepted", Projectid, "00000000-0000-0000-0000-000000000000");
                //SendEmail(emailIds, "[Test]-Quotation Details ", "", "", _QuotationStatusCriteria, ShowHide, "accepted");
                _successMessage.Errormessage = "The email has been sent.";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _QuotationStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        
        public void SendEmail(string emailID, string subject, string path, string fileNameWithExt, QuotationStatusCriteria quotationStatusCriteria, bool ShowHide, string mailType)
        {
            EMailInfo emaildtls = new EMailInfo();
            emaildtls.ToMail = emailID;
            emaildtls.CCMail = "ismsqateam@gmail.com";
            //emaildtls.ToMail = "ismsqateam@gmail.com";
            emaildtls.Subject = subject + quotationStatusCriteria.QuoteNumber;
            emaildtls.Body = PrepareEmailBody(fileNameWithExt, quotationStatusCriteria, ShowHide, mailType);
            emaildtls.DisplayName = "Administrator";            
            //emaildtls.AttachmentPath = path;
            //emaildtls.FileName = fileNameWithExt;
            Mail.SendMail(emaildtls);
        }

        public void SendEmailWithPDF(string emailID, string subject, string path, string fileNameWithExt, QuotationStatusCriteria quotationStatusCriteria, bool ShowHide, string mailType, string Id, string TaskId)
        {
            byte[] bytes;
            int z = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            StyleSheet styles = new StyleSheet();
            bool Price = true;
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);
            try
            {
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    #region PDF

                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter();// headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;

                    _QuotationsList = _IQuotation.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);
                    if (_QuotationsList != null)
                    {
                        // Table
                        table = new PdfPTable(6);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 8f, 30f, 8f, 10f, 14f, 20f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Name : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].customer, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("NRIC : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].NRIC, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Reference No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].project_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("JobSite : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].Jobsite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_QuotationsList[0].quotationForwardDate.ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Email : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].Email, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("H/P : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_QuotationsList[0].Phone, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                    }
                    //Table
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;


                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Renovation Quotation", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;

                    cell.AddElement(para);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;


                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Package Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para1 = new Paragraph(chunk);
                    para1.Alignment = Element.ALIGN_LEFT;


                    cell.AddElement(para1);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 5f;

                    cell = new PdfPCell();
                    chunk = new Chunk("Item", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    chunk = new Chunk("Description", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    cell = new PdfPCell();
                    chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    cell = new PdfPCell();
                    chunk = new Chunk("Uom", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    if (Price == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Unit Price", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        // table.AddCell("Qty");
                    }



                    cell = new PdfPCell();
                    chunk = new Chunk("Amount", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);


                    if (Price == true)
                    {
                        columnWidths = new float[] { 6f, 48f, 6f, 8f, 12f, 10f };
                    }
                    else
                    {
                        columnWidths = new float[] { 6f, 59f, 10f, 8f, 13f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IQuotation.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, true);
                    _ProjectTasksItemDetailList = _IQuotation.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);

                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {
                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 7;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _ProjectTasksItemDetailList.Count; j++)
                            {
                                if (_ProjectTasksItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_ProjectTasksItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);


                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }

                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            //PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //table.AddCell(cell3);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                        }
                                    }
                                    z++;
                                    amount = amount + _ProjectTasksItemDetailList[j].Amount;
                                }
                                else
                                {
                                    z = 1;
                                }

                            }


                        }

                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);
                    }


                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;
                    //Cell
                    cell = new PdfPCell();
                    chunk = new Chunk("Quotation Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para2 = new Paragraph(chunk);
                    para2.Alignment = Element.ALIGN_LEFT;


                    cell.AddElement(para2);
                    cell.Border = 0;
                    table.AddCell(cell);

                    //Add table to document
                    pdfDoc.Add(table);

                    //Table
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 5f;

                    cell = new PdfPCell();
                    chunk = new Chunk("Item", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    cell = new PdfPCell();
                    chunk = new Chunk("Description", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //table.AddCell("Item");
                    //table.AddCell("Description");

                    cell = new PdfPCell();
                    chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    cell = new PdfPCell();
                    chunk = new Chunk("Uom", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                    // table.AddCell("Qty");


                    if (Price == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Unit Price", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
                        // table.AddCell("Qty");
                    }



                    cell = new PdfPCell();
                    chunk = new Chunk("Amount", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);



                    if (Price == true)
                    {
                        columnWidths = new float[] { 6f, 48f, 6f, 8f, 12f, 10f };
                    }
                    else
                    {
                        columnWidths = new float[] { 6f, 59f, 10f, 8f, 13f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Price == true)
                    {
                        table = new PdfPTable(6);
                    }
                    else
                    {
                        table = new PdfPTable(5);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IQuotation.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
                    _ProjectTasksItemDetailList = _IQuotation.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 7;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _ProjectTasksItemDetailList.Count; j++)
                            {
                                if (_ProjectTasksItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_ProjectTasksItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);


                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description;
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            }

                                            table.AddCell(Cell2);
                                        }
                                        else
                                        {
                                            PdfPCell Cell2 = new PdfPCell();
                                            Chunk chunk1 = new Chunk("<p>Description :</p>", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                                            string str = _ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")";
                                            if (_ProjectTasksItemDetailList[j].AdditionalDescription != "")
                                            {
                                                var htmlarraylist = HTMLWorker.ParseToList(new StringReader(str + chunk1 + _ProjectTasksItemDetailList[j].AdditionalDescription), styles);
                                                for (int k = 0; k < htmlarraylist.Count; k++)
                                                {
                                                    var ele = (IElement)htmlarraylist[k];
                                                    Cell2.AddElement(ele);
                                                }
                                            }
                                            else
                                            {
                                                Cell2.AddElement(new Phrase(str, boldFont));
                                            }
                                            table.AddCell(Cell2);
                                        }
                                        if (Price == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);

                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                            PdfPCell cell6 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell6);

                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);

                                            PdfPCell cell4 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].UOM.uom_description, NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            //PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            //cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //table.AddCell(cell3);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);

                                        }
                                    }
                                    z++;
                                    amount = amount + _ProjectTasksItemDetailList[j].Amount;
                                }
                                else
                                {
                                    z = 1;
                                }

                            }


                        }


                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);


                    }

                    _QuotationDetails = _IQuotation.GetQuotationDetailsByProjectId(Id);

                    if (_QuotationDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;

                        chunk = new Chunk("Sub Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.contract_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_QuotationDetails.discount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _QuotationDetails.discount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_QuotationDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Grand Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _QuotationDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(chunk);
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                    }

                    _GetProjectpaymentterms = _IQuotation.GetProjectpaymentterms(Id);

                    if (_GetProjectpaymentterms.Count != 0)
                    {
                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        //0=Left, 1=Centre, 2=Right
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 5f;
                        table.SpacingAfter = 5f;

                        //Cell no 2
                        cell = new PdfPCell();
                        chunk = new Chunk("Payment Terms :", FontFactory.GetFont("Times New Roman", "serif", 12, Font.BOLD, BaseColor.BLACK));
                        cell.AddElement(chunk);
                        //cell.Colspan = ;
                        cell.Border = 0;
                        //cell.BackgroundColor = BaseColor.BLACK;
                        table.AddCell(cell);

                        for (int i = 0; i < _GetProjectpaymentterms.Count; i++)
                        {
                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1).ToString() + ". " + _GetProjectpaymentterms[i].paymentdescription.Master_payment_description, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                            cell = new PdfPCell();
                            cell.Border = 0;
                            cell.AddElement(chunk);
                            table.AddCell(cell);

                            //table.AddCell((i + 1).ToString() + ". " + _GetProjectpaymentterms[i].paymentdescription.Master_payment_description);
                            //table.AddCell();
                        }

                        //Add table to document
                        pdfDoc.Add(table);
                    }

                    //For Starting From New Page 
                    pdfDoc.NewPage();

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    #endregion

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    EMailInfo emaildtls = new EMailInfo();
                    emaildtls.ToMail = emailID;
                    emaildtls.CCMail = "ismsqateam@gmail.com";
                    //emaildtls.ToMail = "ismsqateam@gmail.com";
                    emaildtls.Subject = subject + quotationStatusCriteria.QuoteNumber;
                    emaildtls.Body = PrepareEmailBody(fileNameWithExt, quotationStatusCriteria, ShowHide, mailType);
                    emaildtls.DisplayName = "Administrator";
                    emaildtls.FileInBytes = bytes;
                    emaildtls.AttachmentName = "Quotation Report";
                    //emaildtls.AttachmentPath = path;
                    //emaildtls.FileName = fileNameWithExt;
                    Mail.SendMail(emaildtls);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PrintPreview");
            }
            finally
            {
                bytes = null;
                z = 1;
                _ProjectTasksItemCriteria = null;
                _QuotationsList = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            
        }

        public string PrepareEmailBody(string fileNameWithExt, QuotationStatusCriteria quotationStatusCriteria, bool ShowHide, string mailType)
        {
            return "<table style='margin-right: calc(15%); width: 85%;'><tbody>"
                + "<tr> <td style='width: 25%;'>Date</td><td style='width: 75%;'>: " + quotationStatusCriteria.QuoteDate + "</td></tr>"
                + "<tr> <td style='width: 25%;'>To</td><td style='width: 75%;'>: " + quotationStatusCriteria.CustomerName + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Salesman</td><td style='width: 75%;'>: " + quotationStatusCriteria.SalesmenName + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Address</td><td style='width: 75%;'>: " + quotationStatusCriteria.CustomerAddress + "</td></tr>"
                + "<tr> <td style='width: 25%;'>RE</td><td style='width: 75%;'>: Quatation has been " + mailType + " - " + quotationStatusCriteria.QuoteNumber + ".</td></tr></tbody></table>"
                + "<br><div>Quatation can be Printed After Login</div><div>"
                + "<a href='http://118.201.113.216:38090/Quotation/QuotationDetail?ProjectId=" + quotationStatusCriteria.project_id + "&ShowHide=" + ShowHide + "'>http://118.201.113.216:38090/Contract/ContractDetail?ProjectId=" + quotationStatusCriteria.project_id + "&ShowHide=" + ShowHide + "</a></div>"
                //+ "<br><div>Quatation PDF Attachment :</div><div>" + fileNameWithExt 
                +  "</div><div><br></div><div>Thank you</div><br>"
                + "<div>REGARDS,</div><div> Design 4 Space, " + @PMS.Common.SessionManagement.SelectedBranchName + " </div>";
        }

        private DataTable StripEmptyRows(DataTable dt)
        {
            List<int> rowIndexesToBeDeleted = new List<int>();
            int indexCount = 0;
            foreach (var row in dt.Rows)
            {
                var r = (DataRow)row;
                int emptyCount = 0;
                int itemArrayCount = r.ItemArray.Length;
                foreach (var i in r.ItemArray) if (string.IsNullOrWhiteSpace(i.ToString())) emptyCount++;

                if (emptyCount == itemArrayCount) rowIndexesToBeDeleted.Add(indexCount);

                indexCount++;
            }

            int count = 0;
            foreach (var i in rowIndexesToBeDeleted)
            {
                dt.Rows.RemoveAt(i - count);
                count++;
            }

            return dt;
        }

        public DataTable RemoveUnqualifiedRows(DataTable dt)
        {
            DataTable filteredRows = new DataTable();
            List<int> rowIndexesToBeDeleted = new List<int>();
            int indexCount = 0;
            foreach (DataRow row in dt.Rows)
            {
                int price, amount;
                bool isPrice = int.TryParse(row["Price"].ToString(), out price);
                bool isAmount = int.TryParse(row["Amount"].ToString(), out amount);
                if (string.IsNullOrEmpty(row["Task_Name"].ToString()) || string.IsNullOrEmpty(row["Item_Description"].ToString()) || !isPrice || !isAmount )
                {
                    rowIndexesToBeDeleted.Add(indexCount);
                }
                indexCount++;
            }

            int count = 0;
            foreach (var i in rowIndexesToBeDeleted)
            {
                dt.Rows.RemoveAt(i - count);
                count++;
            }

            return dt;
        }

        [HttpPost]
        public JsonResult AddQuotationFromExcel(string filePath)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            QuotationStatusCriteria _QuotationStatusCriteria = new QuotationStatusCriteria();
            try
            {
                DataTable dtHeader = CommonFunction.GetExcelDataHeader(filePath);
                DataTable dtDetails = CommonFunction.GetExcelDataLineItems(filePath);
                DataTable tableHeader = HeaderPivotDataTable(dtHeader);
                DataTable dtDetailsRemoveEmtpyRows = StripEmptyRows(dtDetails);
                DataTable dtDetailsRmvUnqualifiedRows = RemoveUnqualifiedRows(dtDetailsRemoveEmtpyRows);
                SuccessMessage customerResult = _IQuotation.CheckCustomerAndSalesmenDetails(tableHeader.Rows[0][0].ToString(), tableHeader.Rows[0][1].ToString());
                if (customerResult.Result == "1")
                {
                    QuotationImportExcelResult res = _IQuotation.ImportFromExelFile(tableHeader, dtDetails);

                    if (Convert.ToBoolean(res.Status))
                    {
                        _successMessage.Result = res.Status ? "1" : "0";
                        _successMessage.Errormessage = res.StatusInformation.ToString();
                    }
                    else
                    {
                        _successMessage.Result = "0";
                        _successMessage.Errormessage = "Data Uploaded from Excel failed.";
                    }
                }
                else
                {
                    _successMessage.Result = customerResult.Result.ToString();
                    _successMessage.Errormessage = customerResult.Errormessage.ToString();
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: AddQuotationFromExcel");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _QuotationStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        public DataTable HeaderPivotDataTable(DataTable dt)
        {
            DataTable tableHeader = new DataTable();
            tableHeader.Columns.Add("SalesmanName", typeof(string));
            tableHeader.Columns.Add("CustomerName", typeof(string));
            tableHeader.Columns.Add("AddressSite", typeof(string));
            tableHeader.Columns.Add("Comission", typeof(decimal));
            tableHeader.Columns.Add("QuotationDate", typeof(DateTime));
            tableHeader.Columns.Add("Remarks", typeof(string));
            tableHeader.Columns.Add("Amount", typeof(decimal));
            tableHeader.Columns.Add("DiscountPercentage", typeof(decimal));
            tableHeader.Columns.Add("DiscountAmount", typeof(decimal));
            tableHeader.Columns.Add("BranchId", typeof(int));
            tableHeader.Columns.Add("CreatedBy", typeof(string));

            int QuotationDate = Convert.ToInt32((dt.Rows[4][1] == DBNull.Value || dt.Rows[4][1].ToString() == "") ? 0 : dt.Rows[4][1]);

            tableHeader.Rows.Add(
                dt.Rows[0][1].ToString(), 
                dt.Rows[1][1].ToString(), 
                dt.Rows[2][1].ToString(), 
                Convert.ToDecimal((dt.Rows[3][1] == DBNull.Value || dt.Rows[3][1].ToString() == string.Empty) ? 0 : dt.Rows[3][1]), 
                QuotationDate == 0 ? DateTime.Now.Date : DateTime.FromOADate(QuotationDate),
                dt.Rows[5][1].ToString(), 
                Convert.ToDecimal((dt.Rows[6][1] == DBNull.Value || dt.Rows[6][1].ToString() == string.Empty) ? 0 : dt.Rows[6][1]), 
                Convert.ToDecimal((dt.Rows[7][1] == DBNull.Value || dt.Rows[7][1].ToString() == string.Empty) ? 0 : dt.Rows[7][1]),
                Convert.ToDecimal((dt.Rows[8][1] == DBNull.Value || dt.Rows[8][1].ToString() == string.Empty) ? 0 : dt.Rows[8][1]), 
                SessionManagement.SelectedBranchID, User.Identity.GetUserId());

            return tableHeader;
        }
        
        public ActionResult _FileUpload(Int32 Id)
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: _FileUpload, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                Id = 0;
            }
        }

        private DataTable GetDataTable(string sql, string connectionString)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    conn.Open();
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        dt.Load(rdr);
                        return dt;
                    }
                }
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }   
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }

        public static List<string> ToExcelsSheetList(string excelFilePath)
        {
            string connString = string.Empty;
            string fileExt = ".xlsz";
            List<string> sheets = new List<string>();
            if (fileExt.CompareTo(".xlsx") == 0)
                connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilePath + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
           
            using (OleDbConnection connection = new OleDbConnection(connString))
            {
                connection.Open();
                DataTable dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                foreach (DataRow drSheet in dt.Rows)
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))
                    {
                        string s = drSheet["TABLE_NAME"].ToString();
                        sheets.Add(s.StartsWith("'") ? s.Substring(1, s.Length - 3) : s.Substring(0, s.Length - 1));
                    }
                connection.Close();
            }
            return sheets;
        }

        //private bool GetExcel(string fileInfo)
        //{
        //    QuotationLoadFromExcel quotationLoadFromExcel = new QuotationLoadFromExcel();
        //    QuotationDetailsForExcel quotationDetailsForExcel = new QuotationDetailsForExcel();
        //    string fileExt = ".xlsz";
        //    string connString = string.Empty;
        //    DataTable tableHeader = new DataTable();
        //    int headerRowsCount = 9, headerColumnCount = 2, detailsColumnCount = 10, detailsRowsCount = 100;
        //    string fullPathToExcel = @"C:\Users\Joy\Downloads\efile.xlsx"; //ie C:\Temp\YourExcel.xls

        //    if (fileExt.CompareTo(".xlsx") == 0)
        //        connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileInfo + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
        //    else
        //        connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileInfo + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
        //    List<string> listOfSheets = ToExcelsSheetList(fileInfo);
        //    DataTable dt = GetDataTable("SELECT * from ["+ listOfSheets[0] +"$]", connString);
            
        //    tableHeader.Columns.Add("SalesmanName", typeof(string));
        //    tableHeader.Columns.Add("CustomerName", typeof(string));
        //    tableHeader.Columns.Add("AddressSite", typeof(string));
        //    tableHeader.Columns.Add("Comission", typeof(decimal));
        //    tableHeader.Columns.Add("QuotationDate", typeof(DateTime));
        //    tableHeader.Columns.Add("Remarks", typeof(string));
        //    tableHeader.Columns.Add("Amount", typeof(decimal));
        //    tableHeader.Columns.Add("DiscountPercentage", typeof(decimal));
        //    tableHeader.Columns.Add("DiscountAmount", typeof(decimal));


        //    tableHeader.Rows.Add(dt.Rows[0][1].ToString(), dt.Rows[1][1].ToString(), dt.Rows[2][1].ToString(), Convert.ToDecimal(dt.Rows[3][1] == DBNull.Value ? 0 : dt.Rows[3][1]), dt.Rows[4][1] == DBNull.Value ? null : dt.Rows[4][1],
        //        dt.Rows[5][1].ToString(), Convert.ToDecimal(dt.Rows[6][1] == DBNull.Value ? 0 : dt.Rows[6][1]), Convert.ToDecimal(dt.Rows[7][1] == DBNull.Value ? 0 : dt.Rows[7][1]),
        //        Convert.ToDecimal(dt.Rows[8][1] == DBNull.Value ? 0 : dt.Rows[8][1]));

        //    quotationLoadFromExcel._quotationDetailsForExcel = new List<QuotationDetailsForExcel>();
        //    for (int i = 11; i <= detailsRowsCount; i++)
        //    {
        //        if(dt.Rows[i][0] == DBNull.Value && dt.Rows[i][1] == DBNull.Value && dt.Rows[i][2] == DBNull.Value
        //            && dt.Rows[i][3] == DBNull.Value && dt.Rows[i][4] == DBNull.Value && dt.Rows[i][5] == DBNull.Value
        //            && dt.Rows[i][6] == DBNull.Value && dt.Rows[i][7] == DBNull.Value && dt.Rows[i][8] == DBNull.Value
        //            && dt.Rows[i][9] == DBNull.Value)
        //        {
        //            break;
        //        }
        //        if(dt.Rows[i][0] == DBNull.Value) // || dt.Rows[i][5] == DBNull.Value || dt.Rows[i][6] == DBNull.Value || dt.Rows[i][7] == DBNull.Value
        //        {
        //            continue;
        //        }
        //        quotationDetailsForExcel = new QuotationDetailsForExcel()
        //        {
        //            TaskName = dt.Rows[i][0].ToString(),
        //            Category = dt.Rows[i][1].ToString(),
        //            Item_Description = dt.Rows[i][2].ToString(),
        //            BillingUom = dt.Rows[i][3].ToString(),
        //            Uom = dt.Rows[i][4].ToString(),
        //            Quantity = Convert.ToDecimal(dt.Rows[i][5]),
        //            Price = Convert.ToDecimal(dt.Rows[i][6]),
        //            Amount = Convert.ToDecimal(dt.Rows[i][7]),
        //            Cost_Amount = Convert.ToDecimal(dt.Rows[i][8]),
        //            Remarks = dt.Rows[i][9].ToString()
        //        };
        //        quotationLoadFromExcel._quotationDetailsForExcel.Add(quotationDetailsForExcel);
        //    }
        //    DataTable tableDetails = ToDataTable(quotationLoadFromExcel._quotationDetailsForExcel);
        //    bool result = _IQuotation.ImportFromExelFile(tableHeader, tableDetails);
        //    return result;
        //}

        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string fname = string.Empty;
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fname = Path.Combine(Server.MapPath("~/Excel/"), fname);
                        if (System.IO.File.Exists(fname))
                        {
                            System.IO.File.Delete(fname);
                        }
                        file.SaveAs(fname);
                    }
                    return Json(fname);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }

    public class QuotationLoadFromExcel
    {
        public string SalesmanName { get; set; }
        public string CustomerName { get; set; }
        public string AddressSite { get; set; }
        public string Comission { get; set; }
        public DateTime QuotationDate { get; set; }
        public string Remarks { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<QuotationDetailsForExcel> _quotationDetailsForExcel { get; set; }
    }

    public class QuotationDetailsForExcel
    {
        //public QuotationTasksInExcel _quotationTasksInExcel { get; set; }
        public string TaskName { get; set; }
        public string Category { get; set; }
        public string Item_Description { get; set; }
        public string BillingUom { get; set; }
        public string Uom { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost_Amount { get; set; }
        public string Remarks { get; set; }
    }

    public class QuotationTasksInExcel
    {
        public string taskid { get; set; }
        public string taskname { get; set; }
    }

    public class QuotationImportExcelResult
    {
        public bool Status { get; set; }
        public string StatusInformation { get; set; }
    }

    //public QuotationLoadFromExcel ConvertDTToQuotationLoadExcel(DataTable dtquotationLoadExcel)
    //{
    //    try
    //    {
    //        QuotationLoadFromExcel _loadDataToDTO = new QuotationLoadFromExcel();

    //        return _loadDataToDTO;
    //    }
    //    catch(Exception ex)
    //    {
    //        ExceptionLog.WriteLog(ex, "Method Name: AddQuotationFromExcel");
    //        return null;
    //    }
    //}
}
