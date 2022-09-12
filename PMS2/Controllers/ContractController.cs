using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;
using PMS.Data_Access;
using PMS.Interface;
using PMS.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
//using iText.Kernel.Pdf;
//using System.Configuration;
//using iText.Layout;
//using iText.Layout.Element;
using PMS.Database;
using PMS.Repository.Infrastructure;
using System.Linq;
//using iText.IO.Image;
//using iText.Layout.Borders;
using System.Web;
using System.IO;
using PMS.Common;
using iTextSharp.text.html.simpleparser;
using System.Configuration;

namespace PMS.Controllers
{
    public class ContractController : Controller
    {
        private IContract _IContract;
        private readonly IProject _ProjectRepo;
        public ContractController(IProject ProjectRepo)
        {
            _ProjectRepo = ProjectRepo;
            _IContract = new DataLayer();
        }
        //public ContractController()
        //{
        //    _IContract = new DataLayer();
        //}
        // GET: Contract
        public ActionResult ContractList(Quotation _obj)
        {
            string message = string.Empty;
            SalesmanDropDown salesmen = new SalesmanDropDown();
            DateTime now = DateTime.Now;
            string uid = string.Empty;
            try
            {
                var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
                var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
                if (_obj.from_date == null)
                    _obj.from_date = CurrentstartDate.AddMonths(-1).ToShortDateString();
                if (_obj.to_date == null)
                    _obj.to_date = endDate.ToShortDateString();
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        uid = User.Identity.GetUserId();
                        List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenIdByUserId(uid);
                        _obj.id = Convert.ToInt32(salesMenUsrs[0].Value);
                        _obj.salesmen_name = salesMenUsrs[0].Text;
                        _obj.role = "Salesman";
                        _obj.role = "Salesman";
                    }
                    else
                    {
                        List<SelectListItem> salesMenUsrs = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                    }
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
            }
        }
        public ActionResult GetContractList(string JsonValues)
        {
            string message = string.Empty;
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            QuotationListCriteria _QuotationListCriteria = JsonConvert.DeserializeObject<QuotationListCriteria>(JsonValues, dateTimeConverter);
            List<QuotationList> _QuotationsList = new List<QuotationList>();
            try
            {
                _QuotationListCriteria.Type = 2;
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        string uid = User.Identity.GetUserId();
                        List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenIdByUserId(uid);
                        _QuotationListCriteria.salesMenId = Convert.ToInt32(salesMenUsrs[0].Value);
                    }
                    else
                    {
                        List<SelectListItem> salesMenUsrs = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                    }
                }
                if(SessionManagement.IsBranchAdmin)
                {
                    _QuotationListCriteria.salesMenId = 0;
                }
                _QuotationsList = _IContract.QuotationsList(_QuotationListCriteria);
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
        [HttpPost]
        public JsonResult UpsertProjectContractTerms(string JsonContractTerms, string ProjectId)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            ProjectContractTermsCriteria _ProjectContractTermsCriteria = JsonConvert.DeserializeObject<ProjectContractTermsCriteria>(JsonContractTerms);
            try
            {
                _ProjectContractTermsCriteria.contract_desrcription = HttpUtility.UrlDecode(_ProjectContractTermsCriteria.contract_desrcription, System.Text.Encoding.Default);
                uid = User.Identity.GetUserId();
                _successMessage = _IContract.UpsertProjectContractTerms(_ProjectContractTermsCriteria, uid, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectContractTerms");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _ProjectContractTermsCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertProjectPaymentTerms(string JsonContract, string ProjectId)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            PaymentTerms _ProjectPaymentTermsCriteria = JsonConvert.DeserializeObject<PaymentTerms>(JsonContract);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.UpsertProjectPaymentTerms(_ProjectPaymentTermsCriteria, uid, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectPaymentTerms");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _ProjectPaymentTermsCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult NewContract()
        //{
        //    return View();
        //}
        public ActionResult NewContract(long Id)
        {
            string uid = User.Identity.GetUserId();
            ProjectViewModel objView = new ProjectViewModel();
            SalesmanDropDown salesmen = new SalesmanDropDown();
            List<project_document_list> objPDList = new List<project_document_list>();
            try
            {
                if (Id > 0)
                {
                    if (User.IsInRole("SuperAdmin"))
                    {

                        project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            Common.CommonFunction.MergeObjects(objView, objRec, true);                            
                        }
                    }
                    else
                    {
                        project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    objView.project_id = CommonFunction.GetProjectGuidByID(objView.id.ToString());
                    objView.ReferenceNo = CommonFunction.GetProjectReferenceNo(objView.id);
                    objView.project_document_list = CommonFunction.GetDocuments(Id, Common.Constants.GetDocumentIdType.Contract);
                    if(objView.project_start_date == null)
                    {
                        objView.project_start_date = System.DateTime.Now;
                    }
                }
                else
                {
                    objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                    objView.contract_amount = Convert.ToDecimal(0.00);
                    objView.gst_amount = Convert.ToDecimal(0.00);
                    objView.total_amount = Convert.ToDecimal(0.00);
                    objView.contract_date = DateTime.Now;
                    objView.isactive = true;
                    objView.status_id = 5;
                    objView.project_document_list = objPDList;
                }

                objView.SalesmenList = Common.CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                objView.BankList = Common.CommonFunction.BankList();
                objView.CustomerList = Common.CommonFunction.CustomerListByBranchId();
                objView.StatusList = Common.CommonFunction.ProjectStatusList();
                objView.ActiveInactiveList = Common.CommonFunction.StatusList();
                objView.GSTList = Common.CommonFunction.GSTList();
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        //salesmen = _IContract.GetSalesmenIdByUserId(uid);
                        List<SelectListItem> salesMenUsrs = CommonFunction.GetSalesmenIdByUserId(uid);
                        salesmen.id = Convert.ToInt32(salesMenUsrs[0].Value);
                        salesmen.salesmen_name = salesMenUsrs[0].Text;
                        objView.salesmen_id = salesmen.id;
                        objView.SalesmenList = salesMenUsrs;
                        objView.saleman_commission = CommonFunction.GetSalemanCommision(Convert.ToInt32(objView.salesmen_id));
                        ViewBag.role = "Salesman";
                    }
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: NewContract, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public ActionResult ContractDetail(string ProjectId ,bool bIsFromQuotation = false)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(ProjectId);
                ViewBag.IsFromQuotation = bIsFromQuotation;
                ViewBag.StatusId = _QuotationDetails.status_id;
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ContractDetail");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        public JsonResult GetContractTasksItem(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IContract.GetProjectTasksItem(_ProjectTasksItemCriteria);
                message = "Fetching Records Sucessfull";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractTasksItem");
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
            return Json(new { data = data ,msg = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContractTasksItemPackage(string ProjectId, string TaskId, bool IsFromPackage)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, IsFromPackage);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractTasksItem");
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

        public JsonResult GetContractTasksListItem(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IContract.GetProjectTasksListItem(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractTasksListItem");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            return Json(new { data = _ProjectTasksItemList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetContractTasksItemDetails(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                //_ProjectTasksItemList = _IContract.GetProjectTasksItemDetails(_ProjectTasksItemCriteria);
                _ProjectTasksItemList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractTasksItemDetails");
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
        public JsonResult GetContractpaymentterms(string ProjectId)
        {
            string message = string.Empty;
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            try
            {
                _GetProjectpaymentterms = _IContract.GetProjectpaymentterms(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractpaymentterms");
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
        public JsonResult UpsertContractDetails(string JsonContractDetails, string ProjectId, string TaskId, string TaskName)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            QuotationUpsertProjectDetails _CreateQuotationCriteria = JsonConvert.DeserializeObject<QuotationUpsertProjectDetails>(JsonContractDetails);
            _CreateQuotationCriteria.AdditionalDescription = Common.Constants.AdditionalDescription;
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
                if (_CreateQuotationCriteria.Task_Id == "" || _CreateQuotationCriteria.Task_Id == "0")
                {
                    _CreateQuotationCriteria.Task_Id = TaskId;
                }
                if (_CreateQuotationCriteria.project_det_Id == "")
                {
                    _CreateQuotationCriteria.project_det_Id = "00000000-0000-0000-0000-000000000000";
                }
                _CreateQuotationCriteria.Task_Name = TaskName;
                _CreateQuotationCriteria.userId = User.Identity.GetUserId();
                _SuccessMessage = _IContract.UpsertProjectDetails(_CreateQuotationCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertContractDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertProjectContract(string JsonContract)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            //List<CreateQuotationCriteria> _CreateQuotationCriteria = JsonConvert.DeserializeObject<List<CreateQuotationCriteria>>(obj[0]);
            CreateQuotationCriteria _CreateQuotationCriteria = JsonConvert.DeserializeObject<CreateQuotationCriteria>(JsonContract);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.UpsertProjectContractQuotation(_CreateQuotationCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectContract");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateQuotationCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpsertContractTasks(FormCollection obj, string ProjectId)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<ProjectTasksItemList> _ContractTasksCriteria = JsonConvert.DeserializeObject<List<ProjectTasksItemList>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IContract.UpsertProjectTasks(_ContractTasksCriteria, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertContractTasks");
                message = ex.Message;
            }
            finally
            {
                _ContractTasksCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProjectContractTermsList(String ProjectId)
        {
            string message = string.Empty;
            List<ProjectContractTermsCriteria> _GetProjectContracttterms = new List<ProjectContractTermsCriteria>();
            try
            {
                _GetProjectContracttterms = _IContract.GetProjectContractTermsList(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectContractTermsList");
                message = ex.Message;
            }
            finally
            {
            }
            var data = new
            {
                Items = _GetProjectContracttterms,
                TotalCount = _GetProjectContracttterms.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert_Projects(string JsonContract)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            CreateQuotationCriteria _CreateContractCriteria = JsonConvert.DeserializeObject<CreateQuotationCriteria>(JsonContract);
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            Signature _ContractCriteria;
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.Insert_Projects(_CreateContractCriteria, uid);
                if (_CreateContractCriteria.project_id == null)
                {
                    _ContractCriteria = new Signature();
                    _ContractCriteria.project_id = _SuccessMessage.Id;
                    _ContractCriteria.CustomerId = _CreateContractCriteria.Customer_id.ToString();
                    _ContractCriteria.ContractDate = _CreateContractCriteria.contract_date.ToString();
                    _ContractCriteria.Customer = CommonFunction.GetCustomerData(Convert.ToInt32(_CreateContractCriteria.Customer_id));
                    _ContractCriteria.Salesmen = _CreateContractCriteria.salesmen_name;
                    _ContractCriteria.CustomerAddress = CommonFunction.GetCustomerData(Convert.ToInt32(_CreateContractCriteria.Customer_id)); ;
                    _ContractCriteria.contract_number = _SuccessMessage.Project_Number;
                    _ContractCriteria.SuperId = _SuccessMessage.Id;
                    string json = JsonConvert.SerializeObject(_ContractCriteria);
                    SendingMail(json);
                }
              
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Insert_Projects");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CreateContractCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProject_Contract(string ProjectId)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.DeleteProjectQuotation(uid, ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProject_Contract");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProject_ContractDetails(string Project_det_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.DeleteProjectDetails(uid, Project_det_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProject_ContractDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProject_ContractPaymentTermsByID(string Payment_term_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IContract.DeleteProjectPaymentTermsByID(uid, Payment_term_id);
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

        public ActionResult Upload(string Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, string Id)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                    file.SaveAs(physicalPath);
                    //var result = _IContract.SavePath(Id, physicalPath);
                }
            }
            return Content("");
        }

        public ActionResult Remove(string[] fileNames, string Id)
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                        //var result = _IContract.DeletePath(Id, physicalPath);
                    }
                }
            }
            return Content("");
        }

        public void PrintPreview(string Id, string TaskId, Boolean Qty)
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
            StyleSheet styles = new StyleSheet();
            IMaster _IMaster = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 6;
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(Id, id_type);
                for (int i = 0; _GetDocumentList.Count > i; i++)
                {
                    if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                        }
                    }
                    else
                    {
                        _QuotationDetails.document_path = "";
                        _QuotationDetails.Customer_document_path = "";
                    }

                }
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter() { CustomerSignature = _QuotationDetails.Customer_document_path }; // headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;

                    _QuotationsList = _IContract.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);

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
                    chunk = new Chunk("Renovation Contract", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

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
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para1);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 10f;

                    ////Cell
                    //cell = new PdfPCell();
                    //chunk = new Chunk("Renovation Quotation", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                    //chunk.SetUnderline(2, -3);
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    ////cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.AddElement(chunk);
                    //cell.Colspan = 5;
                    //cell.Border = 0;
                    ////cell.BackgroundColor = BaseColor.BLACK;
                    //  table.AddCell(cell);

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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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

                    // table.AddCell("Amount");

                    cell = new PdfPCell();
                    chunk = new Chunk("S Total (S$)", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //  table.AddCell("S Total(S$)");

                    if (Qty == true)
                    {
                        columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
                    }
                    else
                    {
                        columnWidths = new float[] { 5f, 59f, 10f, 14f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, true);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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

                            PdfPCell cell6 = new PdfPCell(new Phrase("Sub Total : $" + amount.ToString("#,##0.00"), NormalFont));
                            cell6.Colspan = 5;
                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell6);
                            amount = 0;
                        }
                        //float[] columnWidths = new float[] { 8f, 50f, 8f, 8f, 10f };


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
                    chunk = new Chunk("Contract Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para2 = new Paragraph(chunk);
                    para2.Alignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para2);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);


                    //Table
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 10f;

                    ////Cell
                    //cell = new PdfPCell();
                    //chunk = new Chunk("Renovation Quotation", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                    //chunk.SetUnderline(2, -3);
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    ////cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.AddElement(chunk);
                    //cell.Colspan = 5;
                    //cell.Border = 0;
                    ////cell.BackgroundColor = BaseColor.BLACK;
                    //  table.AddCell(cell);

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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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

                    // table.AddCell("Amount");

                    cell = new PdfPCell();
                    chunk = new Chunk("S Total (S$)", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //  table.AddCell("S Total(S$)");

                    if (Qty == true)
                    {
                        columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
                    }
                    else
                    {
                        columnWidths = new float[] { 5f, 59f, 10f, 14f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList!= null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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

                            PdfPCell cell6 = new PdfPCell(new Phrase("Sub Total : $" + amount.ToString("#,##0.00"), NormalFont));
                            cell6.Colspan = 5;
                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell6);
                            amount = 0;
                        }
                        //float[] columnWidths = new float[] { 8f, 50f, 8f, 8f, 10f };


                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);



                    }

                    //_QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);

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

                    _GetProjectpaymentterms = _IContract.GetProjectpaymentterms(Id);

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

                    if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                    {
                        //table = new PdfPTable(1);
                        //table.WidthPercentage = 100;
                        ////0=Left, 1=Centre, 2=Right
                        //table.HorizontalAlignment = 1;
                        //table.SpacingBefore = 5f;
                        //table.SpacingAfter = 5f;

                        ////Cell no 1
                        //cell = new PdfPCell();
                        //cell.Border = 0;
                        //Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                        //image.ScaleAbsolute(100, 50);
                        //cell.AddElement(image);
                        //table.AddCell(cell);

                        //////Add table to document
                        //pdfDoc.Add(table);

                        table = new PdfPTable(2);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 2;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 5f;
                        columnWidths = new float[] { 60f, 30f };

                        chunk = new Chunk("Price, Layout, Terms and Conditions Agreed & Accepted By :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        // para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Design 4 Space Pte Ltd :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        //r para.Alignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        cell.AddElement(para);
                        table.AddCell(cell);



                        //chunk = new Chunk("Customer Signature Here", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        //  para = new Paragraph(chunk);
                        cell.Border = 0;
                        Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        //cell.AddElement(para);
                        table.AddCell(cell);

                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        // cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Name/Signature :" + _QuotationDetails.customer, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Signed and Issued By : " + _QuotationDetails.salesmen, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("NRIC : " + _QuotationDetails.NRIC, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Check should be crossed or A/C Payee only and made payable to : Design 4 Space Pte Ltd. ", FontFactory.GetFont("Times New Roman", "serif", 7, Font.NORMAL, BaseColor.BLACK));

                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        //////Add table to document
                        pdfDoc.Add(table);
                    }

                    //For Starting From New Page 
                    pdfDoc.NewPage();
                    _GetProjectContracttterms = _IMaster.ContractTermsList();


                    if (_GetProjectContracttterms != null)
                    {
                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        //0=Left, 1=Centre, 2=Right
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 5f;
                        table.SpacingAfter = 5f;

                        //Cell no 2
                        //cell = new PdfPCell();
                        //chunk = new Chunk("Contract Terms & Conditions :");
                        //cell.AddElement(chunk);
                        ////cell.Colspan = ;
                        //cell.Border = 0;
                        ////cell.BackgroundColor = BaseColor.BLACK;
                        //table.AddCell(cell);
                        for (int i = 0; i < _GetProjectContracttterms.Count; i++)
                        {
                            String htmlText = _GetProjectContracttterms[i].Description.ToString();
                            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);

                            //add the collection to the document
                            for (int k = 0; k < htmlarraylist.Count; k++)
                            {
                                pdfDoc.Add((IElement)htmlarraylist[k]);
                            }

                            //document.Add(new Paragraph("And the same with indentation...."));

                            // or add the collection to an paragraph
                            // if you add it to an existing non emtpy paragraph it will insert it from
                            //the point youwrite -
                            //Paragraph mypara = new Paragraph();//make an emtphy paragraph as "holder"
                            //mypara.IndentationLeft = 36;
                            //mypara.InsertRange(0, htmlarraylist);
                            //pdfDoc.Add(mypara);
                            // cell.Border = 0;
                            //table.AddCell(_GetProjectContracttterms[i].Description);
                            //table.AddCell();
                        }
                        //Add table to document
                        pdfDoc.Add(table);
                    }

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Contract.pdf");
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
                _QuotationDetails = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            //return View();
        }

        public void PrintPreviewfromDashboard(string Id, string TaskId, Boolean Qty)
        {
            Id = Common.CommonFunction.GetProjectGuidByID(Id);
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
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 6;
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(Id, id_type);
                for (int i = 0; _GetDocumentList.Count > i; i++)
                {
                    if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                        }
                    }
                    else
                    {
                        _QuotationDetails.document_path = "";
                        _QuotationDetails.Customer_document_path = "";
                    }

                }
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter() { CustomerSignature = _QuotationDetails.Customer_document_path }; // headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;// = new PdfPTable()
                                    //pdfDoc.Add(chunk);

                    ////Table
                    //PdfPTable table = new PdfPTable(2);
                    //table.WidthPercentage = 100;
                    ////0=Left, 1=Centre, 2=Right
                    //table.HorizontalAlignment = 0;
                    //table.SpacingBefore = 5f;
                    //table.SpacingAfter = 5f;

                    ////Cell no 1
                    //PdfPCell cell = new PdfPCell();
                    //cell.Border = 0;
                    //Image image = Image.GetInstance(Server.MapPath("~/Content/img/print_logo_new.png"));
                    //image.ScaleAbsolute(200, 150);
                    //cell.AddElement(image);
                    //table.AddCell(cell);

                    ////Cell no 2
                    //chunk = new Chunk("HDB | CONDOMINIUM | LANDED | COMMERCIAL", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                    //cell = new PdfPCell();
                    //cell.Border = 0;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.AddElement(chunk);
                    //table.AddCell(cell);

                    ////Add table to document
                    //pdfDoc.Add(table);

                    _QuotationsList = _IContract.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);

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
                    chunk = new Chunk("Renovation Contract", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    //Table
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 5f;

                    ////Cell
                    //cell = new PdfPCell();
                    //chunk = new Chunk("Renovation Quotation", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                    //chunk.SetUnderline(2, -3);
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    ////cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    //cell.AddElement(chunk);
                    //cell.Colspan = 5;
                    //cell.Border = 0;
                    ////cell.BackgroundColor = BaseColor.BLACK;
                    //  table.AddCell(cell);

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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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

                    // table.AddCell("Amount");

                    cell = new PdfPCell();
                    chunk = new Chunk("S Total (S$)", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //  table.AddCell("S Total(S$)");

                    if (Qty == true)
                    {
                        columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
                    }
                    else
                    {
                        columnWidths = new float[] { 5f, 59f, 10f, 14f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksItemDetails(_ProjectTasksItemCriteria);
                    if (_ProjectTasksItemList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
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
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, boldFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", boldFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_ProjectTasksItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description, NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                        //table.AddCell((i + 1).ToString() + "." + z.ToString());
                                        //table.AddCell(_ProjectTasksItemDetailList[j].Item.item_description);
                                        //if (Qty == true)
                                        //{
                                        //    table.AddCell(_ProjectTasksItemDetailList[j].Qty);
                                        //}
                                        //table.AddCell(_ProjectTasksItemDetailList[j].Price.ToString());
                                        //table.AddCell(_ProjectTasksItemDetailList[j].Amount.ToString());
                                    }
                                    z++;
                                    amount = amount + _ProjectTasksItemDetailList[j].Amount;
                                }
                                else
                                {
                                    z = 1;
                                }
                            }

                            //PdfPCell cell6 = new PdfPCell();
                            //chunk = new Chunk("Sub Total : " + amount, FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK));
                            //cell6.AddElement(chunk);
                            //cell6.Colspan = 5;
                            //cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //cell6.Border = 0;

                            //table.AddCell(cell6);
                            PdfPCell cell6 = new PdfPCell(new Phrase("Sub Total : $" + amount.ToString("#,##0.00"), NormalFont));
                            cell6.Colspan = 5;
                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell6);
                            amount = 0;
                        }
                        //float[] columnWidths = new float[] { 8f, 50f, 8f, 8f, 10f };


                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);

                        //Paragraph para = new Paragraph();
                        //para.Add("Sub Total : " + amount);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //pdfDoc.Add(para);


                    }

                    //_QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);

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

                    _GetProjectpaymentterms = _IContract.GetProjectpaymentterms(Id);

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

                    if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                    {
                        //table = new PdfPTable(1);
                        //table.WidthPercentage = 100;
                        ////0=Left, 1=Centre, 2=Right
                        //table.HorizontalAlignment = 1;
                        //table.SpacingBefore = 5f;
                        //table.SpacingAfter = 5f;

                        ////Cell no 1
                        //cell = new PdfPCell();
                        //cell.Border = 0;
                        //Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                        //image.ScaleAbsolute(100, 50);
                        //cell.AddElement(image);
                        //table.AddCell(cell);

                        //////Add table to document
                        //pdfDoc.Add(table);

                        table = new PdfPTable(2);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 2;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 5f;
                        columnWidths = new float[] { 60f, 30f };

                        chunk = new Chunk("Price, Layout, Terms and Conditions Agreed & Accepted By :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        // para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Design 4 Space Pte Ltd :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        //r para.Alignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        cell.AddElement(para);
                        table.AddCell(cell);



                        //chunk = new Chunk("Customer Signature Here", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        //  para = new Paragraph(chunk);
                        cell.Border = 0;
                        Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        //cell.AddElement(para);
                        table.AddCell(cell);

                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        // cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Name/Signature :" + _QuotationDetails.customer, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Signed and Issued By : " + _QuotationDetails.salesmen, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("NRIC : " + _QuotationDetails.NRIC, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Check should be crossed or A/C Payee only and made payable to : Design 4 Space Pte Ltd. ", FontFactory.GetFont("Times New Roman", "serif", 7, Font.NORMAL, BaseColor.BLACK));

                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        //////Add table to document
                        pdfDoc.Add(table);
                    }

                    //For Starting From New Page 
                    pdfDoc.NewPage();
                    _GetProjectContracttterms = _IMaster.ContractTermsList();


                    if (_GetProjectContracttterms != null)
                    {
                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        //0=Left, 1=Centre, 2=Right
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 5f;
                        table.SpacingAfter = 5f;

                        //Cell no 2
                        //cell = new PdfPCell();
                        //chunk = new Chunk("Contract Terms & Conditions :");
                        //cell.AddElement(chunk);
                        ////cell.Colspan = ;
                        //cell.Border = 0;
                        ////cell.BackgroundColor = BaseColor.BLACK;
                        //table.AddCell(cell);
                        for (int i = 0; i < _GetProjectContracttterms.Count; i++)
                        {
                            String htmlText = _GetProjectContracttterms[i].Description.ToString();
                            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);

                            //add the collection to the document
                            for (int k = 0; k < htmlarraylist.Count; k++)
                            {
                                pdfDoc.Add((IElement)htmlarraylist[k]);
                            }

                            //document.Add(new Paragraph("And the same with indentation...."));

                            // or add the collection to an paragraph
                            // if you add it to an existing non emtpy paragraph it will insert it from
                            //the point youwrite -
                            //Paragraph mypara = new Paragraph();//make an emtphy paragraph as "holder"
                            //mypara.IndentationLeft = 36;
                            //mypara.InsertRange(0, htmlarraylist);
                            //pdfDoc.Add(mypara);
                            // cell.Border = 0;
                            //table.AddCell(_GetProjectContracttterms[i].Description);
                            //table.AddCell();
                        }
                        //Add table to document
                        pdfDoc.Add(table);
                    }

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Contract.pdf");
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
                _QuotationDetails = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            //return View();
        }
        [HttpPost]
        public JsonResult Update_ContractStatus(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            QuotationStatusCriteria _QuotationStatusCriteria = JsonConvert.DeserializeObject<QuotationStatusCriteria>(JsonValues);

            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IContract.Update_ContractStatus(_QuotationStatusCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_ContractStatus");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _QuotationStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        public class PDFFooter : PdfPageEventHelper
        {
            public string CustomerSignature { get; set; }
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
                chunk = new Chunk("HDB | CONDOMINIUM | LANDED | COMMERCIAL    design4space.com.sg", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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


                PdfPTable tbFooter = new PdfPTable(2);
                tbFooter.TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin;
                tbFooter.DefaultCell.Border = 0;
                //Chunk chunk = new Chunk();


                if (CustomerSignature != "" && CustomerSignature != null)
                {
                    cell = new PdfPCell();
                    cell.Border = 0;
                    image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + CustomerSignature);
                    image.ScaleAbsolute(60, 50);
                    // image.SetAbsolutePosition(200, 150);
                    // image.ScaleToFit(200, 150);
                    // cell.PaddingBottom =89;
                    cell.AddElement(image);
                    tbFooter.AddCell(cell);

                }
                else
                {

                }
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

        public void SaveSignature(string contentType, string base64, string fileName)
        {
            String dirPath = "D:\\Proj\\BTS\\PMS2\\PMS2\\Contracts\\2021\\";
            //String dirPath = "C:\myfolder\";
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                {
                    bm2.Save(dirPath + "ImageName.jpg");
                }
            }
            //String dirPath = "D:\\Proj\\BTS\\PMS2\\PMS2\\Contracts\\2021";
            //String imgName = "my_mage_name.bmp";

            //byte[] imgByteArray = Convert.FromBase64String(base64);
            //FileteAllBytes(dirPath + imgName, imgByteArray);

            //string fullOutputPath = "D:\\Proj\\BTS\\PMS2\\PMS2\\Contracts\\2021";
            //byte[] bytes = Convert.FromBase64String(base64);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}

            //image.Save(fullOutputPath, System.Drawing.Imaging.ImageFormat.Png);
            //var fileContents = Convert.FromBase64String(base64);

            //return File(fileContents, contentType, fileName);
        }
        [HttpPost]
        public JsonResult UploadSignature(string JsonSignature)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            Signature List = JsonConvert.DeserializeObject<Signature>(JsonSignature);
            String dirPath = string.Empty;
            String dirPath_Customer = string.Empty;
            String dirPath_Salesmen = string.Empty;
            string uid = string.Empty;
            string fileNameWithExt = string.Empty;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string Project_id = List.SuperId;
            try
            {
                uid = User.Identity.GetUserId();
                fileNameWithExt = List.SalesmenFileName;
                var CustomerImage_PATH = "/Signature/" + "/Contract/" + List.ContractYear + "/" + List.contract_number + "/" + List.CustomerFileName;
                var SalesmenImage_PATH = "/Signature/" + "/Contract/" + List.ContractYear + "/" + List.contract_number + "/" + List.SalesmenFileName;
                dirPath = PMS.Common.Constants.PhysicalPath + "/Signature/" + "/Contract/" + List.ContractYear + "/" + List.contract_number + "/";

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                List.SalesmanimageData = List.SalesmanimageData.Replace("data:image/png;base64,", "");
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(List.SalesmanimageData)))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        bm2.Save(dirPath + List.SalesmenFileName);
                        List.DOCUMENT_NAME = "Salesman Signature";
                        List.ID = "13";
                        List.ID_TYPE = "6";
                        List.SalesmenImage_PATH = SalesmenImage_PATH;
                        _SuccessMessage = _IContract.UpsertSignature(List, uid);
                    }
                }
                List.CustomerimageData = List.CustomerimageData.Replace("data:image/png;base64,", "");
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(List.CustomerimageData)))
                {
                    using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
                    {
                        bm2.Save(dirPath + List.CustomerFileName);
                        List.DOCUMENT_NAME = "Customer Signature";
                        List.ID = "12";
                        List.ID_TYPE = "6";
                        List.SalesmenImage_PATH = CustomerImage_PATH;
                        _SuccessMessage = _IContract.UpsertSignature(List, uid);
                    }
                }
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IContract.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_id, Common.Constants.GetTransaction.Contract);
                }
                else
                    emailAddress = _IContract.GetAdminAndSalesmenEmailAddress("", Project_id, Common.Constants.GetTransaction.Contract);
                List<CustomerDetailsById> customerDetails = _IContract.GetCustomerDetailsById(List.CustomerId);

                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                SendEmailWithPDF(emailIds, "[Test] -Contract has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, customerDetails[0].JobSite, List, "accepted", Project_id, "00000000-0000-0000-0000-000000000000");
                //SendEmail(emailIds, "[Test] -Contract has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, customerDetails[0].JobSite, List, "accepted");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UploadSignature SignatureList=" + List.ToString());
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                List = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult UploadSignature(string JsonSignature)
        //{
        //    SuccessMessage _SuccessMessage = new SuccessMessage();
        //    Signature List = JsonConvert.DeserializeObject<Signature>(JsonSignature);
        //    String dirPath = string.Empty;
        //    String dirPath_Customer = string.Empty;
        //    String dirPath_Salesmen = string.Empty;
        //    string uid = string.Empty;
        //    try
        //    {
        //        dirPath = PMS.Common.Constants.PhysicalPath + "/Contracts/" + List.ContractYear + "/";

        //        List.SalesmanimageData = List.SalesmanimageData.Replace("data:image/png;base64,", "");
        //        // List.FileName = List.FileName.Replace("/", "_");
        //        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(List.SalesmanimageData)))
        //        {
        //            using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
        //            {
        //                bm2.Save(dirPath + List.SalesmenFileName);
        //            }
        //        }
        //        List.CustomerimageData = List.CustomerimageData.Replace("data:image/png;base64,", "");
        //        // List.FileName = List.FileName.Replace("/", "_");
        //        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(List.CustomerimageData)))
        //        {
        //            using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
        //            {
        //                bm2.Save(dirPath + List.CustomerFileName);
        //            }
        //        }

        //        List.ID = "4";
        //        List.ID_TYPE = "6";
        //        uid = User.Identity.GetUserId();
        //        _SuccessMessage = _IContract.UpsertSignature(List, uid);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: UploadSignature");
        //        _SuccessMessage.Errormessage = ex.Message;
        //    }
        //    finally
        //    {
        //        List = null;
        //    }
        //    return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult Upload_File(string SuperId, string ContractNumber)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            Signature List = new Signature();// JsonConvert.DeserializeObject<Signature>(JsonSignature,s);
            string uid = string.Empty;
            String path = string.Empty;
            string extension = string.Empty;
            try
            {
                uid = User.Identity.GetUserId();
                foreach (string item in Request.Files)
                {

                    HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    string doc_Path = "/UploadDocuments/" + "/Contract/"+ ContractNumber + "/";
                    string dirPath = PMS.Common.Constants.PhysicalPath + doc_Path;

                    string UploadPath = "~/UploadDocuments/Contract/"+ ContractNumber;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    if (file.ContentLength == 0)
                        continue;
                    if (file.ContentLength > 0)
                    {
                        path = Path.Combine(HttpContext.Request.MapPath(UploadPath), fileName);
                        extension = Path.GetExtension(file.FileName);

                        file.SaveAs(path);
                    }
                    List.SuperId = SuperId;
                    List.DOCUMENT_NAME = "UploadFile";
                    List.ID = "11";
                    List.ID_TYPE = "6";
                    List.SalesmenImage_PATH = doc_Path+ fileName;
                    List.FILE_TYPE = extension;

                    _SuccessMessage = _IContract.UpsertSignature(List, uid);
                }
                return Json("");
            }
            catch(Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Upload_File Parameters JsonSignature=" + List.ToString());
                _SuccessMessage.Errormessage = ex.Message;
                return Json("");
            }
            finally
            {
                List = null;
                _SuccessMessage = null;
            }
           
            //if (files != null)
            //{
            //    //foreach (var file in files)
            //    //{
            //    var fileName = Path.GetFileName(files.FileName);
            //    var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), fileName);
            //    files.SaveAs(physicalPath);
            //    //var result = _IContract.SavePath(Id, physicalPath);
            //    // }
            //}
            //return Content("");
        }
        public JsonResult Get_Documents(string ProjectId,int IdType)
        {
            List<QuotationDetails> _GetDocumentList;
            List<GetDocumentdetails> _GetDocumentdetails = new List<GetDocumentdetails>();

            //QuotationDetails _GetDocumentpath = new QuotationDetails();
            try
            {

                _GetDocumentList = new List<QuotationDetails>();
                // int id_type = IdType;// 6;
                string path = "";
                _GetDocumentList = _IContract.Get_Documents(ProjectId, IdType);
                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "11" )
                        {

                            GetDocumentdetails _GetDocumentpath = new GetDocumentdetails();
                            _GetDocumentpath.document_path = _GetDocumentList[i].document_path;
                            // path = _GetDocumentList[i].document_path;
                            //_GetDocumentdetails[i].document_path = path;
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
                //_GetDocumentdetails = null;
            }
            //var data = new
            //{
            //    Items = _GetDocumentList,
            //    TotalCount = _GetDocumentList.Count
            //};
            return Json(new { data = _GetDocumentdetails }, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult UploadDocument_Contract(string JsonSignature)
        //{
        //    SuccessMessage _SuccessMessage = new SuccessMessage();
        //    Signature List = JsonConvert.DeserializeObject<Signature>(JsonSignature);
        //    String dirPath = string.Empty;
        //    string uid = string.Empty;
        //    try
        //    {


        //        dirPath = PMS.Common.Constants.PhysicalPath + "/Contracts/" + List.ContractYear + "/";
        //        string strFileName;
        //        string strFilePath;
        //        string strFolder;
        //        strFolder = dirPath + List.FileName;
        //        // Retrieve the name of the file that is posted.
        //        // strFileName = oFile.PostedFile.FileName;
        //        strFileName = Path.GetFileName(List.FileName);
        //        if (strFileName != "")
        //        {
        //            // Create the folder if it does not exist.
        //            if (!Directory.Exists(strFolder))
        //            {
        //                Directory.CreateDirectory(strFolder);
        //            }
        //            // Save the uploaded file to the server.
        //            strFilePath = strFolder + strFileName;
        //            //if (File.Exists(strFilePath))
        //            //{
        //            //    lblUploadResult.Text = strFileName + " already exists on the server!";
        //            //}
        //            //else
        //            //{
        //            //    oFile.PostedFile.SaveAs(strFilePath);
        //            //    lblUploadResult.Text = strFileName + " has been successfully uploaded.";
        //            //}
        //            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(List.SalesmanimageData)))
        //            {
        //                using (System.Drawing.Bitmap bm2 = new System.Drawing.Bitmap(ms))
        //                {
        //                    bm2.Save(dirPath + List.FileName);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
        //        }
        //        // Display the result of the upload.
        //        //frmConfirmation.Visible = true;

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Json("0");

        //}


        //public ActionResult PrintPreview(string Id)
        //{
        //    var terms = _IContract.GetContractTerms(Id);
        //    //var quotationdetails = _IContract.GetQuotationDetailsByProjectId(Id);
        //    //var taskitemdetails = _IContract.GetProjectTasksItemDetails
        //    return View(terms);
        //}

        //private static void RemoveBorder(Table table)
        //{
        //    for (IElement iElement = (IElement)table.GetChildren())
        //    {
        //        ((Cell)iElement).SetBorder(border:null);
        //    }
        //}

        //public string PrintPreview(string id)
        //{
        //    var terms = _IContract.GetContractTerms(id);
        //    string dest = ConfigurationManager.AppSettings["PrintPath"] + "Sample.pdf";
        //    //string Img = "D:\\proj\\BTS\\PMS_D4S\\PMS\\Content\\img\\avatar.jpg";
        //    if (System.IO.File.Exists(dest))
        //    {
        //        System.IO.File.Delete(dest);
        //    }
        //    PdfWriter writer = new PdfWriter(dest);
        //    PdfDocument pdfDoc = new PdfDocument(writer);
        //    Document document = new Document(pdfDoc);
        //    float[] pointColumnWidths = { 150F, 150F, 150F };
        //    Table table = new Table(pointColumnWidths);
        //    Cell c1 = new Cell();
        //    Cell c2 = new Cell();
        //    //ImageData data = ImageDataFactory.Create(Img);
        //    //Image img = new Image(data);
        //    //img.SetFixedPosition(20, 50);
        //    c1.Add(new Cell().Add(new Paragraph("Image")));
        //    //c1.Add(img);
        //    c1.SetHeight(72);
        //    c1.SetWidth(150);
        //    //c1.SetBorder(border: null);
        //    c2.Add(new Cell().Add(new Paragraph("")));
        //    c2.SetBorder(border: null);
        //    table.AddCell(c1);
        //    table.AddCell(c2);
        //    //table.SetBorder(border: );
        //    document.Add(table);
        //    document.Close();
        //    //return View(terms);
        //    return "Hello World";
        //}

        [HttpPost]
        public JsonResult SendingMail(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            Signature _ContractCriteria;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            try
            {
                _ContractCriteria = JsonConvert.DeserializeObject<Signature>(JsonValues);
                _ContractCriteria.contract_date = Convert.ToDateTime(_ContractCriteria.ContractDate);
                string Project_id = _ContractCriteria.project_id;
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IContract.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_id, Common.Constants.GetTransaction.Contract);
                }
                else
                    emailAddress = _IContract.GetAdminAndSalesmenEmailAddress("", Project_id, Common.Constants.GetTransaction.Contract);
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                SendEmailWithPDF(emailIds, "[Test] -Contract Details ", "", "", _ContractCriteria.CustomerAddress, _ContractCriteria, "accepted", Project_id, "00000000-0000-0000-0000-000000000000");
                //SendEmail(emailIds, "[Test] -Contract Details ", "", "", _ContractCriteria.CustomerAddress, _ContractCriteria, "accepted");
                _successMessage.Result = "1";
                _successMessage.Errormessage = "The email has been sent.";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _ContractCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        public void SendEmail(string emailID, string subject, string path, string fileNameWithExt, string fullAddress, Signature signature, string mailType)
        {
            EMailInfo emaildtls = new EMailInfo();
            emaildtls.ToMail = emailID;
            emaildtls.CCMail = "ismsqateam@gmail.com";
            //emaildtls.ToMail = "ismsqateam@gmail.com";
            emaildtls.Subject = subject + signature.contract_number;
            emaildtls.Body = PrepareEmailBody(fileNameWithExt, fullAddress, signature, mailType);
            emaildtls.DisplayName = "Administrator";
            //emaildtls.AttachmentPath = path;
            //emaildtls.FileName = fileNameWithExt;
            Mail.SendMail(emaildtls);
        }

        public void SendEmailWithPDF(string emailID, string subject, string path, string fileNameWithExt, string fullAddress, Signature signature, string mailType, string Id,string TaskId)
        {
            byte[] bytes;
            int z = 1;
            decimal amount = 0;
            float[] columnWidths;
            bool Qty = true;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            StyleSheet styles = new StyleSheet();
            IMaster _IMaster = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 6;
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(Id, id_type);
                for (int i = 0; _GetDocumentList.Count > i; i++)
                {
                    if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "6")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                        }
                    }
                    else
                    {
                        _QuotationDetails.document_path = "";
                        _QuotationDetails.Customer_document_path = "";
                    }

                }
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter() { CustomerSignature = _QuotationDetails.Customer_document_path }; // headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    #region PDF 
                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;

                    _QuotationsList = _IContract.GetQuotationDetailsListByProjectId(_ProjectTasksItemCriteria.project_Id);

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
                    chunk = new Chunk("Renovation Contract", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);

                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 10f;

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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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

                    // table.AddCell("Amount");

                    cell = new PdfPCell();
                    chunk = new Chunk("S Total (S$)", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //  table.AddCell("S Total(S$)");

                    if (Qty == true)
                    {
                        columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
                    }
                    else
                    {
                        columnWidths = new float[] { 5f, 59f, 10f, 14f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, true);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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

                            PdfPCell cell6 = new PdfPCell(new Phrase("Sub Total : $" + amount.ToString("#,##0.00"), NormalFont));
                            cell6.Colspan = 5;
                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell6);
                            amount = 0;
                        }
                        //float[] columnWidths = new float[] { 8f, 50f, 8f, 8f, 10f };


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
                    chunk = new Chunk("Contract Details", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    var para2 = new Paragraph(chunk);
                    para2.Alignment = Element.ALIGN_LEFT;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para2);
                    cell.Border = 0;
                    //cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(cell);
                    //Add table to document
                    pdfDoc.Add(table);


                    //Table
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 2f;
                    table.SpacingAfter = 10f;

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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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

                    // table.AddCell("Amount");

                    cell = new PdfPCell();
                    chunk = new Chunk("S Total (S$)", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(para);
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);

                    //  table.AddCell("S Total(S$)");

                    if (Qty == true)
                    {
                        columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
                    }
                    else
                    {
                        columnWidths = new float[] { 5f, 59f, 10f, 14f };
                    }
                    table.SetWidths(columnWidths);
                    pdfDoc.Add(table);
                    if (Qty == true)
                    {
                        table = new PdfPTable(5);
                    }
                    else
                    {
                        table = new PdfPTable(4);
                    }

                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, false);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
                    if (_ProjectTasksItemList != null && _ProjectTasksItemDetailList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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
                                                Cell2.AddElement(new Phrase(str, boldFont));
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
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_ProjectTasksItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _ProjectTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
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

                            PdfPCell cell6 = new PdfPCell(new Phrase("Sub Total : $" + amount.ToString("#,##0.00"), NormalFont));
                            cell6.Colspan = 5;
                            cell6.HorizontalAlignment = Element.ALIGN_RIGHT;
                            table.AddCell(cell6);
                            amount = 0;
                        }
                        //float[] columnWidths = new float[] { 8f, 50f, 8f, 8f, 10f };


                        table.SetWidths(columnWidths);
                        pdfDoc.Add(table);



                    }

                    //_QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);

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

                    _GetProjectpaymentterms = _IContract.GetProjectpaymentterms(Id);

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

                    if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                    {
                        table = new PdfPTable(2);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 2;
                        table.SpacingBefore = 10f;
                        table.SpacingAfter = 5f;
                        columnWidths = new float[] { 60f, 30f };

                        chunk = new Chunk("Price, Layout, Terms and Conditions Agreed & Accepted By :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        // para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Design 4 Space Pte Ltd :  ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        //r para.Alignment = Element.ALIGN_RIGHT;
                        cell.Border = 0;
                        cell.AddElement(para);
                        table.AddCell(cell);



                        //chunk = new Chunk("Customer Signature Here", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        //  para = new Paragraph(chunk);
                        cell.Border = 0;
                        Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        //cell.AddElement(para);
                        table.AddCell(cell);

                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                        image.ScaleAbsolute(60, 50);
                        cell.AddElement(image);
                        //para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        //para.SpacingBefore = 8f;
                        // cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Name/Signature :" + _QuotationDetails.customer, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Signed and Issued By : " + _QuotationDetails.salesmen, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("NRIC : " + _QuotationDetails.NRIC, FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        chunk.SetUnderline(1, -3);
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        //para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Check should be crossed or A/C Payee only and made payable to : Design 4 Space Pte Ltd. ", FontFactory.GetFont("Times New Roman", "serif", 7, Font.NORMAL, BaseColor.BLACK));

                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        cell.Border = 0;
                        //cell.BorderWidthBottom = 1;
                        // para.Alignment = Element.ALIGN_CENTER;
                        para.SpacingBefore = 8f;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        //////Add table to document
                        pdfDoc.Add(table);
                    }

                    //For Starting From New Page 
                    pdfDoc.NewPage();
                    _GetProjectContracttterms = _IMaster.ContractTermsList();


                    if (_GetProjectContracttterms != null)
                    {
                        table = new PdfPTable(1);
                        table.WidthPercentage = 100;
                        //0=Left, 1=Centre, 2=Right
                        table.HorizontalAlignment = 0;
                        table.SpacingBefore = 5f;
                        table.SpacingAfter = 5f;

                        for (int i = 0; i < _GetProjectContracttterms.Count; i++)
                        {
                            String htmlText = _GetProjectContracttterms[i].Description.ToString();
                            List<IElement> htmlarraylist = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(htmlText), null);

                            //add the collection to the document
                            for (int k = 0; k < htmlarraylist.Count; k++)
                            {
                                pdfDoc.Add((IElement)htmlarraylist[k]);
                            }
                           
                        }
                        //Add table to document
                        pdfDoc.Add(table);
                    }

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    #endregion

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    EMailInfo emaildtls = new EMailInfo();
                    emaildtls.ToMail = emailID;
                    emaildtls.CCMail = "ismsqateam@gmail.com";
                    //emaildtls.ToMail = "ismsqateam@gmail.com";
                    emaildtls.Subject = subject + signature.contract_number;
                    emaildtls.Body = PrepareEmailBody(fileNameWithExt, fullAddress, signature, mailType);
                    emaildtls.DisplayName = "Administrator";
                    emaildtls.FileInBytes = bytes;
                    emaildtls.AttachmentName = "Contract Report";                   
                    Mail.SendMail(emaildtls);

                }


            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
            }
            finally
            {
                bytes = null;
                z = 1;
                _ProjectTasksItemCriteria = null;
                _QuotationsList = null;
                _QuotationDetails = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
        }

        public string PrepareEmailBody(string fileNameWithExt, string fullAddress, Signature signature, string mailType)
        {
            return "<table style='margin-right: calc(15%); width: 85%;'><tbody>"
                + "<tr> <td style='width: 25%;'>Date</td><td style='width: 75%;'>: "+ signature.contract_date.ToString("d") +"</td></tr>"
                + "<tr> <td style='width: 25%;'>To</td><td style='width: 75%;'>: "+ signature.Customer +"</td></tr>"
                + "<tr> <td style='width: 25%;'>Salesman</td><td style='width: 75%;'>: "+ signature.Salesmen +"</td></tr>"
                + "<tr> <td style='width: 25%;'>Address</td><td style='width: 75%;'>: "+ fullAddress + "</td></tr>"
                + "<tr> <td style='width: 25%;'>RE</td><td style='width: 75%;'>: Contract has been " + mailType + " - " + signature.contract_number + ".</td></tr></tbody></table>"
                + "<br><div>Contract can be Printed After Login</div><div>"
                + "<a href='http://118.201.113.216:38090/Contract/ContractDetail?ProjectId=" + signature.SuperId + "'>http://118.201.113.216:38090/Contract/ContractDetail?ProjectId=" + signature.SuperId + "</a></div>"
                //+ "<br><div>Contract PDF Attachment :</div><div>" + fileNameWithExt + "</div><div><br></div><div>Thank you</div><br>"
                + "<br><div>REGARDS,</div><div> Design 4 Space, " + @PMS.Common.SessionManagement.SelectedBranchName + " </div>";
        }

        public ActionResult ContractSupplierMapping(string ProjectId)
        {
            contractSupplierMapping csmap = _IContract.GetContractSupplierMappingForProject(ProjectId);
            SupplierDropDown suppdrpdwn = new SupplierDropDown();
            suppdrpdwn.Supplier_id = 0;
            suppdrpdwn.Supplier_Name = "--Select--";
            csmap.Supplier = suppdrpdwn;
            ViewBag.ProjectId = ProjectId;
            return View(csmap);
        }

        [HttpPost]
        public JsonResult SaveContractsupplierMapping(string SuppIDValues, string PrjID)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            try
            {
                string SuppIDValuesFormat = SuppIDValues.Remove(SuppIDValues.Length - 1, 1);
                _successMessage = _IContract.UpsertContractSupplierMapping(SuppIDValuesFormat, PrjID);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                SuppIDValues = null;
                PrjID = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAdditionOmissionRowsCount(long ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IContract.GetAdditionOmissionRowsCount(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetRowsCount");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {

            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}