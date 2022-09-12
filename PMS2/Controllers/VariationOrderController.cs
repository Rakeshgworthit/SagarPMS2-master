using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PMS.Data_Access;
using PMS.Interface;
using PMS.Models;
using System.IO;
using PMS.Common;
using Newtonsoft.Json.Converters;

namespace PMS.Controllers
{
    public class VariationOrderController : Controller
    {
        private IVariationOrder _IVariationOrder;

        public VariationOrderController()
        {
            _IVariationOrder = new DataLayer();
        }

        public ActionResult VariationOrderList(string ProjectId)
        {
            ViewBag.ProjectId = ProjectId;
            return View();
        }

        public ActionResult ElectricalVariationOrderList(string ProjectId)
        {
            ViewBag.ProjectId = ProjectId;
            return View();
        }

        public ActionResult VariationOrderDetail(string ProjectId)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectId(ProjectId);
                _QuotationDetails.shortForwardDate = _QuotationDetails.quotationForwardDate.ToShortDateString();
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: VariationOrderDetail");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        public ActionResult NewVariationOrder()
        {
            return View();
        }
        public ActionResult AdditionOmission(string ProjectId, Boolean ShowHide, string vo_id)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.vo_id = vo_id;
                ViewBag.ProjectId = ProjectId;
                ViewBag.ShowHide = ShowHide;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectId(ProjectId);
                _QuotationDetails.shortForwardDate = _QuotationDetails.quotationForwardDate.ToShortDateString();
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: AdditionOmission");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        public ActionResult ElectricalVariationOrder(string ProjectId, Boolean ShowHide, string vo_id, bool IsEdit = false)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.vo_id = vo_id;
                ViewBag.ProjectId = ProjectId;
                ViewBag.ShowHide = ShowHide;
                ViewBag.isEdit = IsEdit;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectIdForEvo(ProjectId, vo_id);
                _QuotationDetails.shortForwardDate = _QuotationDetails.quotationForwardDate.ToShortDateString();
                ViewBag.GstAmount = _QuotationDetails.gstamount;
                ViewBag.TotalAmount = _QuotationDetails.totalamount;
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ElectricalVariationOrder");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        public ActionResult GetElectricalVariationOrderAfterSave(string ProjectId, Boolean ShowHide, string vo_id, bool IsEdit = false)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.vo_id = vo_id;
                ViewBag.ProjectId = ProjectId;
                ViewBag.ShowHide = ShowHide;
                ViewBag.isEdit = IsEdit;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectIdForEvo(ProjectId, vo_id);
                _QuotationDetails.shortForwardDate = _QuotationDetails.quotationForwardDate.ToShortDateString();
                ViewBag.GstAmount = _QuotationDetails.gstamount;
                ViewBag.TotalAmount = _QuotationDetails.totalamount;
                return Json(new { data = _QuotationDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ElectricalVariationOrder");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        public ActionResult AdditionDetail(string ProjectId)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectId(ProjectId);
                _QuotationDetails.shortForwardDate = _QuotationDetails.quotationForwardDate.ToShortDateString();
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: AdditionDetail");
                return null;
            }
            finally
            {

            }
            //return View();
        }
        public ActionResult OmissionDetail(string ProjectId)
        {
            QuotationDetails _QuotationDetails = new QuotationDetails();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _QuotationDetails = _IVariationOrder.GetQuotationDetailsByProjectId(ProjectId);
                return View(_QuotationDetails);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: OmissionDetail");
                return null;
            }
            finally
            {

            }
            //return View();
        }

        //[HttpPost]
        //public JsonResult UpsertVO_Details(string JsonVODetails, string ProjectId, string TaskId, string TaskName, int RecordType)
        //{
        //    string uid = string.Empty;
        //    SuccessMessage _SuccessMessage = new SuccessMessage();
        //    VariationOrder _VariationOrderCriteria = JsonConvert.DeserializeObject<VariationOrder>(JsonVODetails);
        //    try
        //    {

        //        _VariationOrderCriteria.Task_Id = TaskId;
        //        _VariationOrderCriteria.Task_Name = TaskName;
        //        _VariationOrderCriteria.record_type = RecordType;
        //        uid = User.Identity.GetUserId();
        //        _SuccessMessage = _IVariationOrder.UpsertVO_Details(_VariationOrderCriteria, uid, ProjectId);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: UpsertVO_Details");
        //        _SuccessMessage.Errormessage = ex.Message;
        //    }
        //    finally
        //    {
        //        _VariationOrderCriteria = null;
        //    }
        //    return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult UpsertVO_Details(string JsonVODetails, string ProjectId, string TaskId, string TaskName, int RecordType)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            VariationOrder _VariationOrderCriteria = JsonConvert.DeserializeObject<VariationOrder>(JsonVODetails);
            try
            {

                _VariationOrderCriteria.Task_Id = TaskId;
                _VariationOrderCriteria.Task_Name = TaskName;
                _VariationOrderCriteria.record_type = RecordType;
                uid = User.Identity.GetUserId();

                if (RecordType == 2)/* && _VariationOrderCriteria.vo_det_id != null && _VariationOrderCriteria.vo_det_id != ""*/
                {
                    _SuccessMessage = _IVariationOrder.Validation_VO_Omission(_VariationOrderCriteria, ProjectId);
                    if (_SuccessMessage.Result == "1" || _SuccessMessage.Result == "2")
                    {
                        _SuccessMessage = _IVariationOrder.UpsertVO_Details(_VariationOrderCriteria, uid, ProjectId);
                    }
                    else
                    {
                        _SuccessMessage.Result = _SuccessMessage.Result;
                        _SuccessMessage.Errormessage = _SuccessMessage.Errormessage;
                    }
                }
                else
                {
                    _SuccessMessage = _IVariationOrder.UpsertVO_Details(_VariationOrderCriteria, uid, ProjectId);
                }
                // _SuccessMessage = _IVariationOrder.UpsertVO_Details(_VariationOrderCriteria, uid, ProjectId);

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertVO_Details");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _VariationOrderCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVOTasksItemDetails(string ProjectId, string TaskId, string Project_Det_Id, string VO_Id)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            //List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            List<VariationOrder> _VariationOrderDetail = new List<VariationOrder>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemCriteria.Project_Det_Id = Project_Det_Id;
                _ProjectTasksItemCriteria.Vo_Id = VO_Id;
                // _ProjectTasksItemCriteria.record_type = RecordType;
                _VariationOrderDetail = _IVariationOrder.GetVOTasksItemDetails(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItemDetails");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _VariationOrderDetail,
                TotalCount = _VariationOrderDetail.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVO(string ProjectId, string JsonValues)
        {
            SalesmanDropDown salesmen = new SalesmanDropDown();
            string uid = string.Empty;
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            VOListCriteria _evoListCriteria = JsonConvert.DeserializeObject<VOListCriteria>(JsonValues, dateTimeConverter);
            List<VariationOrderList> _VariationOrder = new List<VariationOrderList>();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    uid = User.Identity.GetUserId();
                    salesmen = _IVariationOrder.GetSalesmenIdByUserId(uid);
                }
                else
                {
                    salesmen.id = 0;
                }
                _VariationOrder = _IVariationOrder.GetVO(salesmen, ProjectId, _evoListCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVO");
                return null;
            }
            finally
            {

            }
            var data = new
            {
                Items = _VariationOrder,
                TotalCount = _VariationOrder.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetEVO(string ProjectId, string JsonValues)
        {
            SalesmanDropDown salesmen = new SalesmanDropDown();
            string uid = string.Empty;
            string message = string.Empty;
            var format = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            EvoListCriteria _evoListCriteria = JsonConvert.DeserializeObject<EvoListCriteria>(JsonValues, dateTimeConverter);
            List<ElectricalVariationOrderList> _EVariationOrder = new List<ElectricalVariationOrderList>();
            try
            {
                if (User.IsInRole("Salesman"))
                {
                    uid = User.Identity.GetUserId();
                    salesmen = _IVariationOrder.GetSalesmenIdByUserId(uid);
                }
                else
                {
                    salesmen.id = 0;
                }
                _EVariationOrder = _IVariationOrder.GetEVO(salesmen, ProjectId, _evoListCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetEVO");
                return null;
            }
            finally
            {

            }
            var data = new
            {
                Items = _EVariationOrder,
                TotalCount = _EVariationOrder.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetVODetails(string vo_det_id)
        {
            List<VariationOrder> _VariationOrderDetail = new List<VariationOrder>();
            try
            {
                _VariationOrderDetail = _IVariationOrder.GetVO_Details(vo_det_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVODetails");
                return null;
            }
            finally
            {

            }
            var data = new
            {
                Items = _VariationOrderDetail,
                TotalCount = _VariationOrderDetail.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetVOTasksItem(string ProjectId, string TaskId, string VO_Id)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemCriteria.Vo_Id = VO_Id;
                // _ProjectTasksItemCriteria.record_type = RecordType;
                if(!String.IsNullOrEmpty(VO_Id))
                    _ProjectTasksItemList = _IVariationOrder.GetVOTasksItem(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItem");
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


        [HttpPost]
        public JsonResult Delete_Vo_Details(string vo_det_id, string ProjectId, string RecordType)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                string record_type = RecordType;//addition
                _SuccessMessage = _IVariationOrder.Delete_Vo_Details(uid, vo_det_id, ProjectId, record_type);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Vo_Details");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete_vo(string vo_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IVariationOrder.Delete_vo(uid, vo_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Vo");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVODetailsByProjectId(string ProjectId, string VO_Id)
        {
            VariationOrderList _VariationOrderDet = new VariationOrderList();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _VariationOrderDet = _IVariationOrder.GetVODetailsByProjectId(ProjectId, VO_Id);
                _VariationOrderDet.voDate = Convert.ToDateTime(_VariationOrderDet.voDate).ToShortDateString();
                _VariationOrderDet.project_id = ProjectId;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVODetailsByProjectId");
                return null;
            }
            finally
            {

            }
            return Json(new { data = _VariationOrderDet }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEVODetailsByProjectId(string ProjectId, string VO_Id)
        {
            VariationOrderList _VariationOrderDet = new VariationOrderList();
            try
            {
                ViewBag.ProjectId = ProjectId;
                _VariationOrderDet = _IVariationOrder.GetEVODetailsByProjectId(ProjectId, VO_Id);
                _VariationOrderDet.voDate = Convert.ToDateTime(_VariationOrderDet.voDate).ToShortDateString();
                _VariationOrderDet.project_id = ProjectId;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetEVODetailsByProjectId");
                return null;
            }
            finally
            {

            }
            return Json(new { data = _VariationOrderDet }, JsonRequestBehavior.AllowGet);
        }
        public void PrintPreview(string Id, string TaskId, Boolean Qty, string vo_id)
        {
            byte[] bytes;
            int z = 1;
            int v = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            List<VariationOrder> _VariationOrderItemDetailList = new List<VariationOrder>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            QuotationDetails _VariationOrder = new QuotationDetails();
            VariationOrderList _VariationOrderDetails = new VariationOrderList();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            IContract _IContract = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 7;
                string VO_Id = vo_id; // "00000000-0000-0000-0000-000000000000";
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(vo_id, id_type);
                #region Pdf

                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                            var Customer_document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path;

                            //DirectoryInfo di = new DirectoryInfo(Customer_document_path);
                            //FileInfo[] TXTFiles = di.GetFiles("*.jpg");
                            //if (TXTFiles.Length == 0)
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    _QuotationDetails.Customer_document_path = "";
                            //    // log.Info("no files present")
                            //}
                            //string filename = null;

                            //filename = Path.GetFileName(Customer_document_path);

                            string finalCustomer_document_path = Path.GetDirectoryName(Customer_document_path);
                            if (!Directory.Exists(finalCustomer_document_path))
                            {
                                _QuotationDetails.Customer_document_path = "";
                            }
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                            var document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path;
                            //if (!Directory.Exists(document_path))
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    //var path = Directory.GetParent(document_path);
                            //    //path = String.Join(@"\", path.Split('\\').Take(3));
                            //    //path = path.Remove(path.IndexOf("\bin\Debug"));path = path.Replace("\bin\Debug", "");
                            //    //path = path.Remove(path.LastIndexOfAny(new char[] { '\\' }, path.LastIndexOf('\\') - 1));
                            //}
                            string finaldocument_path = Path.GetDirectoryName(document_path);
                            if (!Directory.Exists(finaldocument_path))
                            {
                                _QuotationDetails.document_path = "";
                            }
                        }

                    }


                }
                else
                {
                    _QuotationDetails.document_path = "";
                    _QuotationDetails.Customer_document_path = "";
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
                    para1.Alignment = Element.ALIGN_CENTER;
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


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria,true);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);
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
                    para2.Alignment = Element.ALIGN_CENTER;
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


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria,false);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria,false);
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

                    pdfDoc.NewPage();
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;

                    _VariationOrderDetails = _IVariationOrder.GetVODetailsByProjectId(Id, VO_Id);

                    if (_VariationOrderDetails != null)
                    {
                        // Table
                        table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 15f, 40f, 25f, 30f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Salesmen : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.salesmen_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);



                        chunk = new Chunk("VO No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.vo_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Customer :", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.name1, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(Convert.ToDateTime(_VariationOrderDetails.voDate).ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                        chunk = new Chunk(_VariationOrderDetails.addressSite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Amount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.total_amount.ToString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("Status : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.status, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                    chunk = new Chunk("Variation Order", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para);
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

                    //table = new PdfPTable(5);
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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
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

                    //columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
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

                    //table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemCriteria.Vo_Id = vo_id;
                    _ProjectTasksItemList = _IVariationOrder.GetVOTasksItem(_ProjectTasksItemCriteria);
                    _VariationOrderItemDetailList = _IVariationOrder.GetVOTasksItemDetails(_ProjectTasksItemCriteria);
                    if (_ProjectTasksItemList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {


                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _VariationOrderItemDetailList.Count; j++)
                            {
                                if (_VariationOrderItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_VariationOrderItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, boldFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", boldFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    v++;
                                    amount = amount + _VariationOrderItemDetailList[j].Amount;
                                }
                                else
                                {
                                    v = 1;
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



                    if (_VariationOrderDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;
                        //prabnadha 
                        decimal totalAfterDiscounts = _VariationOrderDetails.amount - _VariationOrderDetails.discount_amount;
                        string Total = totalAfterDiscounts.ToString("#,##0.00");

                        chunk = new Chunk("Sub Total (A) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.addition_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Sub Total (B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.omission_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("Sub Total (A-B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_VariationOrderDetails.discount_amount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _VariationOrderDetails.discount_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);

                            chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + Total, FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_VariationOrderDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("$" + _VariationOrderDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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

                        chunk = new Chunk("$" + _VariationOrderDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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
                        if (!string.IsNullOrEmpty(_QuotationDetails.Customer_document_path))
                        {
                            cell = new PdfPCell();
                            //  para = new Paragraph(chunk);
                            cell.Border = 0;
                            Image imageCustomer = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                            imageCustomer.ScaleAbsolute(60, 50);
                            cell.AddElement(imageCustomer);
                            //cell.BorderWidthBottom = 1;
                            //para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            //cell.AddElement(para);
                            table.AddCell(cell);
                        }
                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                        {
                            cell = new PdfPCell();
                            Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                            image.ScaleAbsolute(60, 50);
                            cell.AddElement(image);
                            //para = new Paragraph(chunk);
                            cell.Border = 0;
                            //cell.BorderWidthBottom = 1;
                            // para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            // cell.AddElement(para);
                            table.AddCell(cell);
                        }

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
                    #endregion
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=VariationOrder.pdf");
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
                _GetDocumentList = null;
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
                    //var path = PMS.Common.Constants.PhysicalPath + CustomerSignature;
                    //if (Directory.Exists(path))
                    //{
                    //Directory.CreateDirectory(path);
                    cell = new PdfPCell();
                    cell.Border = 0;
                    image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + CustomerSignature);
                    image.ScaleAbsolute(60, 50);
                    // image.SetAbsolutePosition(200, 150);
                    // image.ScaleToFit(200, 150);
                    // cell.PaddingBottom =89;
                    cell.AddElement(image);
                    tbFooter.AddCell(cell);
                    //}
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
        public JsonResult GetVOTasksListItem(string ProjectId, string TaskId)
        {
            string message = string.Empty;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<ProjectTasksItemList> _ProjectTasksItemList = new List<ProjectTasksItemList>();
            try
            {
                _ProjectTasksItemCriteria.project_Id = ProjectId;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                _ProjectTasksItemList = _IVariationOrder.GetVOTasksListItem(_ProjectTasksItemCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksListItem");
                message = ex.Message;
            }
            finally
            {
                _ProjectTasksItemCriteria = null;
            }
            return Json(new { data = _ProjectTasksItemList }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update_VOStatus(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            VOStatusCriteria _VOStatusCriteria = JsonConvert.DeserializeObject<VOStatusCriteria>(JsonValues);

            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IVariationOrder.Update_VOStatus(_VOStatusCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_VOStatus JsonValues= " + _VOStatusCriteria.ToString());
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _VOStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Update_EVOStatus(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            VOStatusCriteria _VOStatusCriteria = JsonConvert.DeserializeObject<VOStatusCriteria>(JsonValues);

            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IVariationOrder.Update_EVOStatus(_VOStatusCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_EVOStatus JsonValues= " + _VOStatusCriteria.ToString());
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _VOStatusCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Upload_VOSignature(string JsonSignature)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            Signature List = JsonConvert.DeserializeObject<Signature>(JsonSignature);
            String dirPath = string.Empty;
            String dirPath_Customer = string.Empty;
            String dirPath_Salesmen = string.Empty;
            string uid = string.Empty;
            string fileNameWithExt = string.Empty;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string Project_Id = List.ProjectId;
            string Vo_Id = List.SuperId;
            try
            {
                uid = User.Identity.GetUserId();
                var CustomerImage_PATH = "/Signature/" + "/VO/" + List.ContractYear + "/" + List.contract_number + "/" + List.CustomerFileName;
                var SalesmenImage_PATH = "/Signature/" + "/VO/" + List.ContractYear + "/" + List.contract_number + "/" + List.SalesmenFileName;
                dirPath = PMS.Common.Constants.PhysicalPath + "/Signature/" + "/VO/" + List.ContractYear + "/" + List.contract_number + "/";
                fileNameWithExt = List.CustomerFileName;
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
                        List.ID_TYPE = "7";
                        List.SalesmenImage_PATH = SalesmenImage_PATH;
                        _SuccessMessage = _IVariationOrder.UpsertSignature(List, uid);
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
                        List.ID_TYPE = "7";
                        List.SalesmenImage_PATH = CustomerImage_PATH;
                        _SuccessMessage = _IVariationOrder.UpsertSignature(List, uid);
                    }
                }

                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_Id, Common.Constants.GetTransaction.VO);
                }
                else
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress("", Project_Id, Common.Constants.GetTransaction.VO);

                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                bool ShowHide = true;
                SendEmailwithPDFVO(emailIds, "[Test]-Variation has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, List, ShowHide, "accepted",Project_Id, "00000000-0000-0000-0000-000000000000", Vo_Id);
                //SendEmail(emailIds, "[Test]-Variation has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, List, ShowHide, "accepted");

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Upload_VOSignature Parameters JsonSignature=" + List.ToString());
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                List = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Upload_EVOSignature(string JsonSignature)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            Signature List = JsonConvert.DeserializeObject<Signature>(JsonSignature);
            String dirPath = string.Empty;
            String dirPath_Customer = string.Empty;
            String dirPath_Salesmen = string.Empty;
            string uid = string.Empty;
            string fileNameWithExt = string.Empty;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            string Project_Id = List.ProjectId;
            string Vo_Id = List.SuperId;
            try
            {
                uid = User.Identity.GetUserId();
                var CustomerImage_PATH = "/Signature/" + "/EVO/" + List.ContractYear + "/" + List.contract_number + "/" + List.CustomerFileName;
                var SalesmenImage_PATH = "/Signature/" + "/EVO/" + List.ContractYear + "/" + List.contract_number + "/" + List.SalesmenFileName;
                dirPath = PMS.Common.Constants.PhysicalPath + "/Signature/" + "/EVO/" + List.ContractYear + "/" + List.contract_number + "/";
                fileNameWithExt = List.CustomerFileName;
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
                        List.ID_TYPE = "7";
                        List.SalesmenImage_PATH = SalesmenImage_PATH;
                        _SuccessMessage = _IVariationOrder.UpsertEvoSignature(List, uid);
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
                        List.ID_TYPE = "7";
                        List.SalesmenImage_PATH = CustomerImage_PATH;
                        _SuccessMessage = _IVariationOrder.UpsertEvoSignature(List, uid);
                    }
                }
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_Id, Common.Constants.GetTransaction.EVO);
                }
                else
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress("", Project_Id, Common.Constants.GetTransaction.EVO);
                

                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                bool ShowHide = true;
                SendEmailwithPDFEVO(emailIds, "[Test]-Electrical Variation has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, List, ShowHide, "accepted", Project_Id, "00000000-0000-0000-0000-000000000000", Vo_Id);
                //SendEmail(emailIds, "[Test]-Electrical Variation has been Confirmed ", dirPath + fileNameWithExt, fileNameWithExt, List, ShowHide, "accepted");

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Upload_EVOSignature Parameters JsonSignature=" + List.ToString());
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                List = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpsertVO(string JsonVO)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            VariationOrderCriteria _VariationOrderCriteria = JsonConvert.DeserializeObject<VariationOrderCriteria>(JsonVO);
            try
            {

                uid = User.Identity.GetUserId();
                _SuccessMessage = _IVariationOrder.UpsertVO(_VariationOrderCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertVO");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _VariationOrderCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpsertEVO(string JsonEVO)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            VariationOrderCriteria _EVOCriteria = JsonConvert.DeserializeObject<VariationOrderCriteria>(JsonEVO);
            try
            {

                uid = User.Identity.GetUserId();
                _SuccessMessage = _IVariationOrder.UpsertEVO(_EVOCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertEVO");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _EVOCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRowsCount(string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IVariationOrder.GetRowsCount(ProjectId);
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

        
        [HttpPost]
        public JsonResult GetEVORowsCount(string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IVariationOrder.GetEVORowsCount(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetEVORowsCount");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {

            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_VODetails_RowsCount(string VO_Id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IVariationOrder.Get_VODetails_RowsCount(VO_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_VODetails_RowsCount");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {

            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get_EVODetails_RowsCount(string VO_Id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IVariationOrder.Get_EVODetails_RowsCount(VO_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_EVODetails_RowsCount");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {

            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDiscountPercentageFromContract(string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IVariationOrder.GetDiscountPercentageFromContract(ProjectId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_EVODetails_RowsCount");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {

            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VOCategoryDropDownList()
        {
            string message = string.Empty;
            List<VOCategoryDropDown> _DropDownList = new List<VOCategoryDropDown>();
            try
            {
                _DropDownList = _IVariationOrder.VOCategoryDropDownList();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _DropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetElectricalItemsDetails(int PropertyType_Id, string projectId, string evo_id = null)
        {
            string message = string.Empty;
            List<ElectricalItemMapping> _DropDownList = new List<ElectricalItemMapping>();
            try
            {
                _DropDownList = _IVariationOrder.GetElectricalItemsDetails(PropertyType_Id, projectId, evo_id);
                //ViewBag.ElectericalMappingId = _DropDownList.


            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _DropDownList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEvoSelectedData(string projectId, string SelectedData, string UpdatedData, int propertyType_Id, string evo_det_id, bool IsCheckboxSelected = false)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = User.Identity.GetUserId();
            List<ElectricalItemMapping> electricalMapping = new List<ElectricalItemMapping>();

            try
            {
                _SuccessMessage = _IVariationOrder.GetElectricalItemsDetailsCheckedorUnchecked(projectId, SelectedData, UpdatedData, propertyType_Id, uid, evo_det_id, IsCheckboxSelected);

                // _SuccessMessage = _IVariationOrder.SaveEvoSelectedData(projectId, SelectedData, UpdatedData, propertyType_Id, evo_det_id, uid);

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SaveEvoSelectedData");
                _SuccessMessage.Errormessage = ex.Message;
            }

            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewMethodEvoSelectedData(string JsonEVODetails, string JsonEVOTotals, string Project_id, int PropertyType_Id, string evo_id, string evo_det_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = User.Identity.GetUserId();
            var result = JsonConvert.DeserializeObject<List<EVOCriteria>>(JsonEVODetails);
            var totalCriterias = JsonConvert.DeserializeObject<EVOTotalCriteria>(JsonEVOTotals);

            try
            {
                //if (result.Count > 0)
                //{
                _SuccessMessage = _IVariationOrder.SaveNewMethodEvoSelectedData(result, totalCriterias, Project_id, PropertyType_Id, evo_id, uid, evo_det_id);
                //}

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SaveEvoSelectedData");
                _SuccessMessage.Errormessage = ex.Message;
            }

            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertHeaderNewEvo(string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = User.Identity.GetUserId();
            //var result = JsonConvert.DeserializeObject<List<EVOCriteria>>(JsonEVODetails);
            try
            {
                //if (result.Count > 0)
                //{
                _SuccessMessage = _IVariationOrder.InsertHeaderNewEvo(ProjectId, true);
                //}

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SaveEvoSelectedData");
                _SuccessMessage.Errormessage = ex.Message;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SendingMail(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            EVOEmailSendCriteria _EVOEmailSendCriteria;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            try
            {
                _EVOEmailSendCriteria = JsonConvert.DeserializeObject<EVOEmailSendCriteria>(JsonValues);
                string Project_id = _EVOEmailSendCriteria.ProjectId;
                string Vo_Id = _EVOEmailSendCriteria.VO_ID;
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_id, Common.Constants.GetTransaction.EVO);
                }
                else
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress("", Project_id, Common.Constants.GetTransaction.EVO);
                //bool ShowHide = true;
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                Signature signatureDtls = new Signature();
                signatureDtls.VODate = _EVOEmailSendCriteria.VODate;
                signatureDtls.CustomerName = _EVOEmailSendCriteria.CustomerName;
                signatureDtls.SalesmenName = _EVOEmailSendCriteria.SalesmenName;
                signatureDtls.CustomerAddress = _EVOEmailSendCriteria.CustomerAddress;
                signatureDtls.contract_number = _EVOEmailSendCriteria.contract_number;
                signatureDtls.CustomerName = _EVOEmailSendCriteria.CustomerName;
                signatureDtls.project_id = _EVOEmailSendCriteria.ProjectId;
                bool ShowHide = true;
                SendEmailwithPDFEVO(emailIds, "[Test]-Electical Variation Order Details", "", "", signatureDtls, ShowHide, "accepted",Project_id, "00000000-0000-0000-0000-000000000000", Vo_Id);
                //SendEmail(emailIds, "[Test]-Electical Variation Order Details", "", "", signatureDtls, ShowHide, "accepted");
                _successMessage.Errormessage = "An email has been sent.";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _EVOEmailSendCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendingVOMail(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            EVOEmailSendCriteria _EVOEmailSendCriteria;
            List<UserEmailAddress> emailAddress = new List<UserEmailAddress>();
            try
            {
                _EVOEmailSendCriteria = JsonConvert.DeserializeObject<EVOEmailSendCriteria>(JsonValues);
                string Project_Id = _EVOEmailSendCriteria.ProjectId;
                string Vo_Id = _EVOEmailSendCriteria.VO_ID;
                if (User.IsInRole("Salesman"))
                {
                    int getsalesmanID = CommonFunction.SalesmenIDByUserID(User.Identity.GetUserId());
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress(getsalesmanID.ToString(), Project_Id, Common.Constants.GetTransaction.VO);
                }
                else
                    emailAddress = _IVariationOrder.GetAdminAndSalesmenEmailAddress("", Project_Id, Common.Constants.GetTransaction.VO);
                //bool ShowHide = true;
                string emailIds = string.Join(",", emailAddress.Select(x => x.Email));
                Signature signatureDtls = new Signature();
                signatureDtls.VODate = _EVOEmailSendCriteria.VODate;
                signatureDtls.CustomerName = _EVOEmailSendCriteria.CustomerName;
                signatureDtls.SalesmenName = _EVOEmailSendCriteria.SalesmenName;
                signatureDtls.CustomerAddress = _EVOEmailSendCriteria.CustomerAddress;
                signatureDtls.contract_number = _EVOEmailSendCriteria.contract_number;
                signatureDtls.CustomerName = _EVOEmailSendCriteria.CustomerName;
                signatureDtls.project_id = _EVOEmailSendCriteria.ProjectId;
                bool ShowHide = true;
                SendEmailwithPDFVO(emailIds, "[Test] -Variation Order Details", "", "", signatureDtls, ShowHide, "accepted", Project_Id, "00000000-0000-0000-0000-000000000000", Vo_Id);
                //SendEmail(emailIds, "[Test] -Variation Order Details", "", "", signatureDtls, ShowHide, "accepted");
                _successMessage.Errormessage = "An email has been sent.";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendingMail");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _EVOEmailSendCriteria = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        public void SendEmail(string emailID, string subject, string path, string fileNameWithExt, Signature signature, bool ShowHide, string mailType)
        {
            EMailInfo emaildtls = new EMailInfo();
            emaildtls.ToMail = emailID;
            emaildtls.CCMail = "ismsqateam@gmail.com";
            //emaildtls.ToMail = "ismsqateam@gmail.com";
            emaildtls.Subject = subject + signature.contract_number;
            emaildtls.Body = PrepareEmailBody(fileNameWithExt, signature, ShowHide, mailType);
            emaildtls.DisplayName = "Administrator";
            //emaildtls.AttachmentPath = path;
            //emaildtls.FileName = fileNameWithExt;
            Mail.SendMail(emaildtls);
        }

        public void SendEmailwithPDFVO(string emailID, string subject, string path, string fileNameWithExt, Signature signature, bool ShowHide, string mailType,string Id, string TaskId, string vo_id)
        {
            byte[] bytes;
            int z = 1;
            int v = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            List<VariationOrder> _VariationOrderItemDetailList = new List<VariationOrder>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            QuotationDetails _VariationOrder = new QuotationDetails();
            VariationOrderList _VariationOrderDetails = new VariationOrderList();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            IContract _IContract = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 7;
                string VO_Id = vo_id; // "00000000-0000-0000-0000-000000000000";
                bool Qty = true;
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(vo_id, id_type);
                #region Pdf

                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                            var Customer_document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path;

                            //DirectoryInfo di = new DirectoryInfo(Customer_document_path);
                            //FileInfo[] TXTFiles = di.GetFiles("*.jpg");
                            //if (TXTFiles.Length == 0)
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    _QuotationDetails.Customer_document_path = "";
                            //    // log.Info("no files present")
                            //}
                            //string filename = null;

                            //filename = Path.GetFileName(Customer_document_path);

                            string finalCustomer_document_path = Path.GetDirectoryName(Customer_document_path);
                            if (!Directory.Exists(finalCustomer_document_path))
                            {
                                _QuotationDetails.Customer_document_path = "";
                            }
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                            var document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path;
                            //if (!Directory.Exists(document_path))
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    //var path = Directory.GetParent(document_path);
                            //    //path = String.Join(@"\", path.Split('\\').Take(3));
                            //    //path = path.Remove(path.IndexOf("\bin\Debug"));path = path.Replace("\bin\Debug", "");
                            //    //path = path.Remove(path.LastIndexOfAny(new char[] { '\\' }, path.LastIndexOf('\\') - 1));
                            //}
                            string finaldocument_path = Path.GetDirectoryName(document_path);
                            if (!Directory.Exists(finaldocument_path))
                            {
                                _QuotationDetails.document_path = "";
                            }
                        }

                    }


                }
                else
                {
                    _QuotationDetails.document_path = "";
                    _QuotationDetails.Customer_document_path = "";
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
                    para1.Alignment = Element.ALIGN_CENTER;
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


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria, true);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, true);
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
                    para2.Alignment = Element.ALIGN_CENTER;
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
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria, false);
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

                    pdfDoc.NewPage();
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;

                    _VariationOrderDetails = _IVariationOrder.GetVODetailsByProjectId(Id, VO_Id);

                    if (_VariationOrderDetails != null)
                    {
                        // Table
                        table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 15f, 40f, 25f, 30f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Salesmen : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.salesmen_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);



                        chunk = new Chunk("VO No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.vo_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Customer :", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.name1, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(Convert.ToDateTime(_VariationOrderDetails.voDate).ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                        chunk = new Chunk(_VariationOrderDetails.addressSite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Amount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.total_amount.ToString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("Status : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.status, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                    chunk = new Chunk("Variation Order", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para);
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

                    //table = new PdfPTable(5);
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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
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

                    //columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
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

                    //table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemCriteria.Vo_Id = vo_id;
                    _ProjectTasksItemList = _IVariationOrder.GetVOTasksItem(_ProjectTasksItemCriteria);
                    _VariationOrderItemDetailList = _IVariationOrder.GetVOTasksItemDetails(_ProjectTasksItemCriteria);
                    if (_ProjectTasksItemList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {


                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _VariationOrderItemDetailList.Count; j++)
                            {
                                if (_VariationOrderItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_VariationOrderItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, boldFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", boldFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    v++;
                                    amount = amount + _VariationOrderItemDetailList[j].Amount;
                                }
                                else
                                {
                                    v = 1;
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



                    if (_VariationOrderDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;
                        //prabnadha 
                        decimal totalAfterDiscounts = _VariationOrderDetails.amount - _VariationOrderDetails.discount_amount;
                        string Total = totalAfterDiscounts.ToString("#,##0.00");

                        chunk = new Chunk("Sub Total (A) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.addition_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("Sub Total (B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.omission_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("Sub Total (A-B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_VariationOrderDetails.discount_amount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _VariationOrderDetails.discount_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);

                            chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + Total, FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_VariationOrderDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("$" + _VariationOrderDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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

                        chunk = new Chunk("$" + _VariationOrderDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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
                        if (!string.IsNullOrEmpty(_QuotationDetails.Customer_document_path))
                        {
                            cell = new PdfPCell();
                            //  para = new Paragraph(chunk);
                            cell.Border = 0;
                            Image imageCustomer = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                            imageCustomer.ScaleAbsolute(60, 50);
                            cell.AddElement(imageCustomer);
                            //cell.BorderWidthBottom = 1;
                            //para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            //cell.AddElement(para);
                            table.AddCell(cell);
                        }
                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                        {
                            cell = new PdfPCell();
                            Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                            image.ScaleAbsolute(60, 50);
                            cell.AddElement(image);
                            //para = new Paragraph(chunk);
                            cell.Border = 0;
                            //cell.BorderWidthBottom = 1;
                            // para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            // cell.AddElement(para);
                            table.AddCell(cell);
                        }

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
                    #endregion
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    EMailInfo emaildtls = new EMailInfo();
                    emaildtls.ToMail = emailID;
                    emaildtls.CCMail = "ismsqateam@gmail.com";
                    //emaildtls.ToMail = "ismsqateam@gmail.com";
                    emaildtls.Subject = subject + signature.contract_number;
                    emaildtls.Body = PrepareEmailBody(fileNameWithExt, signature, ShowHide, mailType);
                    emaildtls.DisplayName = "Administrator";
                    emaildtls.FileInBytes = bytes;
                    emaildtls.AttachmentName = "VariationOrder Report";
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
                _GetDocumentList = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            
        }

        public void SendEmailwithPDFEVO(string emailID, string subject, string path, string fileNameWithExt, Signature signature, bool ShowHide, string mailType, string Id, string TaskId, string vo_id)
        {

            byte[] bytes;
            int z = 1;
            int v = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            List<VariationOrder> _VariationOrderItemDetailList = new List<VariationOrder>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            QuotationDetails _VariationOrder = new QuotationDetails();
            VariationOrderList _VariationOrderDetails = new VariationOrderList();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            IContract _IContract = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 7;
                string VO_Id = vo_id; // "00000000-0000-0000-0000-000000000000";
                bool Qty = true;
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(vo_id, id_type);
                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                            var Customer_document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path;

                            //DirectoryInfo di = new DirectoryInfo(Customer_document_path);
                            //FileInfo[] TXTFiles = di.GetFiles("*.jpg");
                            //if (TXTFiles.Length == 0)
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    _QuotationDetails.Customer_document_path = "";
                            //    // log.Info("no files present")
                            //}
                            //string filename = null;

                            //filename = Path.GetFileName(Customer_document_path);

                            string finalCustomer_document_path = Path.GetDirectoryName(Customer_document_path);
                            if (!Directory.Exists(finalCustomer_document_path))
                            {
                                _QuotationDetails.Customer_document_path = "";
                            }
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                            var document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path;
                            //if (!Directory.Exists(document_path))
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    //var path = Directory.GetParent(document_path);
                            //    //path = String.Join(@"\", path.Split('\\').Take(3));
                            //    //path = path.Remove(path.IndexOf("\bin\Debug"));path = path.Replace("\bin\Debug", "");
                            //    //path = path.Remove(path.LastIndexOfAny(new char[] { '\\' }, path.LastIndexOf('\\') - 1));
                            //}
                            string finaldocument_path = Path.GetDirectoryName(document_path);
                            if (!Directory.Exists(finaldocument_path))
                            {
                                _QuotationDetails.document_path = "";
                            }
                        }

                    }


                }
                else
                {
                    _QuotationDetails.document_path = "";
                    _QuotationDetails.Customer_document_path = "";
                }
                // pdfDoc.SetMargins(50, 50, 100, 40);
                //  Document pdfDoc = new Document(new Rectangle(288f, 144f), 25, 25, 50, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    #region PDF

                    // HeaderFooter headerFooter = new HeaderFooter();
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter() { CustomerSignature = _QuotationDetails.Customer_document_path }; // headerFooter.OnEndPage(pdfWriter, pdfDoc); //new HeaderFooter();
                    pdfDoc.Open();

                    ////Top Heading
                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;// = new PdfPTable()

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

                    pdfDoc.NewPage();
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;

                    //_VariationOrderDetails = _IVariationOrder.GetVODetailsByProjectId(Id, VO_Id);
                    _VariationOrderDetails = _IVariationOrder.GetEVODetailsByProjectId(Id, VO_Id);

                    if (_VariationOrderDetails != null)
                    {
                        // Table
                        table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 15f, 40f, 25f, 30f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Salesmen : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.salesmen_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);



                        chunk = new Chunk("VO No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.vo_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Customer :", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.name1, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(Convert.ToDateTime(_VariationOrderDetails.voDate).ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                        chunk = new Chunk(_VariationOrderDetails.addressSite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Amount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.total_amount.ToString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("Status : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.status, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                    chunk = new Chunk("Electrical Variation Order", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para);
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

                    //table = new PdfPTable(5);
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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
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

                    //columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
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

                    //table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemCriteria.Vo_Id = vo_id;
                    //_ProjectTasksItemList = _IVariationOrder.GetVOTasksItem(_ProjectTasksItemCriteria);
                    //_VariationOrderItemDetailList = _IVariationOrder.GetVOTasksItemDetails(_ProjectTasksItemCriteria);
                    _ProjectTasksItemList = _IVariationOrder.GetEVOTasksItem(_ProjectTasksItemCriteria);
                    _VariationOrderItemDetailList = _IVariationOrder.GetEVOTasksItemDetails(_ProjectTasksItemCriteria);

                    if (_ProjectTasksItemList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {


                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _VariationOrderItemDetailList.Count; j++)
                            {
                                if (_VariationOrderItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_VariationOrderItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, boldFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", boldFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    v++;
                                    amount = amount + _VariationOrderItemDetailList[j].Amount;
                                }
                                else
                                {
                                    v = 1;
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



                    if (_VariationOrderDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;
                        //prabnadha 
                        decimal totalAfterDiscounts = _VariationOrderDetails.amount - _VariationOrderDetails.discount_amount;
                        string Total = totalAfterDiscounts.ToString("#,##0.00");

                        //chunk = new Chunk("Sub Total (A) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("$" + _VariationOrderDetails.addition_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("Sub Total (B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("$" + _VariationOrderDetails.omission_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);


                        chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_VariationOrderDetails.discount_amount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _VariationOrderDetails.discount_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);

                            chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + Total, FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_VariationOrderDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("$" + _VariationOrderDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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

                        chunk = new Chunk("$" + _VariationOrderDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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
                        if (!string.IsNullOrEmpty(_QuotationDetails.Customer_document_path))
                        {
                            cell = new PdfPCell();
                            //  para = new Paragraph(chunk);
                            cell.Border = 0;
                            Image imageCustomer = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                            imageCustomer.ScaleAbsolute(60, 50);
                            cell.AddElement(imageCustomer);
                            //cell.BorderWidthBottom = 1;
                            //para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            //cell.AddElement(para);
                            table.AddCell(cell);
                        }
                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                        {
                            cell = new PdfPCell();
                            Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                            image.ScaleAbsolute(60, 50);
                            cell.AddElement(image);
                            //para = new Paragraph(chunk);
                            cell.Border = 0;
                            //cell.BorderWidthBottom = 1;
                            // para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            // cell.AddElement(para);
                            table.AddCell(cell);
                        }

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
#endregion
                    bytes = memoryStream.ToArray();
                    memoryStream.Close();

                    EMailInfo emaildtls = new EMailInfo();
                    emaildtls.ToMail = emailID;
                    emaildtls.CCMail = "ismsqateam@gmail.com";
                    //emaildtls.ToMail = "ismsqateam@gmail.com";
                    emaildtls.Subject = subject + signature.contract_number;
                    emaildtls.Body = PrepareEmailBody(fileNameWithExt, signature, ShowHide, mailType);
                    emaildtls.DisplayName = "Administrator";
                    emaildtls.FileInBytes = bytes;
                    emaildtls.AttachmentName = "Electrical VariationOrder Report";
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
                _GetDocumentList = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
           

        }


        public string PrepareEmailBody(string fileNameWithExt, Signature signature, bool ShowHide, string mailType)
        {
            return "<table style='margin-right: calc(15%); width: 85%;'><tbody>"
                + "<tr> <td style='width: 25%;'>Date</td><td style='width: 75%;'>: " + signature.VODate + "</td></tr>"
                + "<tr> <td style='width: 25%;'>To</td><td style='width: 75%;'>: " + signature.CustomerName + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Salesman</td><td style='width: 75%;'>: " + signature.SalesmenName + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Address</td><td style='width: 75%;'>: " + signature.CustomerAddress + "</td></tr>"
                + "<tr> <td style='width: 25%;'>Contract No</td><td style='width: 75%;'>: " + signature.contract_number + "</td></tr>"
                + "<tr> <td style='width: 25%;'>RE</td><td style='width: 75%;'>: Variation Order has been " + mailType + " - " + signature.contract_number + ".</td></tr></tbody></table>"
                + "<br><div>Contract can be Printed After Login</div><div>"
                + "<a href='http://118.201.113.216:38090/VariationOrder/AdditionOmission?ProjectId=" + signature.ProjectId + "&ShowHide=" + ShowHide + "&vo_id="
                + signature.SuperId + "'>http://118.201.113.216:38090/VariationOrder/AdditionOmission?ProjectId=" + signature.ProjectId + "&ShowHide=" + ShowHide + "&vo_id=" + signature.SuperId + "</a></div>"
                //+ "<br><div>Variation Order PDF Attachment :</div><div>" + fileNameWithExt 
                + "</div><div><br></div><div>Thank you</div><br>"
                + "<div>REGARDS,</div><div> Design 4 Space, " + @PMS.Common.SessionManagement.SelectedBranchName + " </div>";
        }

        public void PrintPreviewEVO(string Id, string TaskId, Boolean Qty, string vo_id)
        {
            byte[] bytes;
            int z = 1;
            int v = 1;
            decimal amount = 0;
            float[] columnWidths;
            ProjectTasksItemList _ProjectTasksItemCriteria = new ProjectTasksItemList();
            List<QuotationDetails> _QuotationsList = new List<QuotationDetails>();
            List<ProjectTasksItem> _ProjectTasksItemList = new List<ProjectTasksItem>();
            List<ProjectTasksItemList> _ProjectTasksItemDetailList = new List<ProjectTasksItemList>();
            List<VariationOrder> _VariationOrderItemDetailList = new List<VariationOrder>();
            QuotationDetails _QuotationDetails = new QuotationDetails();
            QuotationDetails _VariationOrder = new QuotationDetails();
            VariationOrderList _VariationOrderDetails = new VariationOrderList();
            List<PaymentTerms> _GetProjectpaymentterms = new List<PaymentTerms>();
            IMaster _IMaster = new DataLayer();
            IContract _IContract = new DataLayer();
            List<ContractTerm> _GetProjectContracttterms = new List<ContractTerm>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);

            List<QuotationDetails> _GetDocumentList = new List<QuotationDetails>();
            try
            {
                int id_type = 7;
                string VO_Id = vo_id; // "00000000-0000-0000-0000-000000000000";
                _ProjectTasksItemCriteria.project_Id = Id;
                _ProjectTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                _QuotationDetails = _IContract.GetQuotationDetailsByProjectId(Id);
                _GetDocumentList = _IContract.Get_Documents(vo_id, id_type);
                if (_GetDocumentList != null && _GetDocumentList.Count > 0)
                {
                    for (int i = 0; _GetDocumentList.Count > i; i++)
                    {
                        if (_GetDocumentList[i].doc_id == "12" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.Customer_document_path = _GetDocumentList[i].document_path;
                            var Customer_document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path;

                            //DirectoryInfo di = new DirectoryInfo(Customer_document_path);
                            //FileInfo[] TXTFiles = di.GetFiles("*.jpg");
                            //if (TXTFiles.Length == 0)
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    _QuotationDetails.Customer_document_path = "";
                            //    // log.Info("no files present")
                            //}
                            //string filename = null;

                            //filename = Path.GetFileName(Customer_document_path);

                            string finalCustomer_document_path = Path.GetDirectoryName(Customer_document_path);
                            if (!Directory.Exists(finalCustomer_document_path))
                            {
                                _QuotationDetails.Customer_document_path = "";
                            }
                        }
                        if (_GetDocumentList[i].doc_id == "13" && _GetDocumentList[i].id_type == "7")
                        {
                            _QuotationDetails.document_path = _GetDocumentList[i].document_path;
                            var document_path = PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path;
                            //if (!Directory.Exists(document_path))
                            //{
                            //    _QuotationDetails.document_path = "";
                            //    //var path = Directory.GetParent(document_path);
                            //    //path = String.Join(@"\", path.Split('\\').Take(3));
                            //    //path = path.Remove(path.IndexOf("\bin\Debug"));path = path.Replace("\bin\Debug", "");
                            //    //path = path.Remove(path.LastIndexOfAny(new char[] { '\\' }, path.LastIndexOf('\\') - 1));
                            //}
                            string finaldocument_path = Path.GetDirectoryName(document_path);
                            if (!Directory.Exists(finaldocument_path))
                            {
                                _QuotationDetails.document_path = "";
                            }
                        }

                    }


                }
                else
                {
                    _QuotationDetails.document_path = "";
                    _QuotationDetails.Customer_document_path = "";
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


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria,true);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria,true);
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


                    _ProjectTasksItemList = _IContract.GetProjectTasksItemPackage(_ProjectTasksItemCriteria,false);
                    _ProjectTasksItemDetailList = _IContract.GetProjectTasksQuotationItemDetails(_ProjectTasksItemCriteria,false);
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

                    pdfDoc.NewPage();
                    table = new PdfPTable(1);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 15f;
                    table.SpacingAfter = 5f;

                    //_VariationOrderDetails = _IVariationOrder.GetVODetailsByProjectId(Id, VO_Id);
                    _VariationOrderDetails = _IVariationOrder.GetEVODetailsByProjectId(Id, VO_Id);

                    if (_VariationOrderDetails != null)
                    {
                        // Table
                        table = new PdfPTable(4);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 15f, 40f, 25f, 30f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Salesmen : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.salesmen_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);



                        chunk = new Chunk("VO No : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.vo_number, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Customer :", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_VariationOrderDetails.name1, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("Date : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(Convert.ToDateTime(_VariationOrderDetails.voDate).ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                        chunk = new Chunk(_VariationOrderDetails.addressSite, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Amount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.total_amount.ToString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("Status : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_VariationOrderDetails.status, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                    chunk = new Chunk("Electrical Variation Order", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                    chunk.SetUnderline(1, -3);
                    para = new Paragraph(chunk);
                    para.Alignment = Element.ALIGN_CENTER;
                    //cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    cell.AddElement(para);
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

                    //table = new PdfPTable(5);
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
                    if (Qty == true)
                    {
                        cell = new PdfPCell();
                        chunk = new Chunk("Qty", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(para);
                        cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                        table.AddCell(cell);
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

                    //columnWidths = new float[] { 5f, 55f, 8f, 10f, 14f };
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

                    //table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.HorizontalAlignment = 0;
                    table.SpacingBefore = 5f;
                    table.SpacingAfter = 5f;


                    _ProjectTasksItemCriteria.Vo_Id = vo_id;
                    //_ProjectTasksItemList = _IVariationOrder.GetVOTasksItem(_ProjectTasksItemCriteria);
                    //_VariationOrderItemDetailList = _IVariationOrder.GetVOTasksItemDetails(_ProjectTasksItemCriteria);
                    _ProjectTasksItemList = _IVariationOrder.GetEVOTasksItem(_ProjectTasksItemCriteria);
                    _VariationOrderItemDetailList = _IVariationOrder.GetEVOTasksItemDetails(_ProjectTasksItemCriteria);

                    if (_ProjectTasksItemList != null)
                    {
                        for (int i = 0; i < _ProjectTasksItemList.Count; i++)
                        {


                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _ProjectTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _VariationOrderItemDetailList.Count; j++)
                            {
                                if (_VariationOrderItemDetailList[j].Task_Name == _ProjectTasksItemList[i].Task.Task_Name)
                                {
                                    if (_VariationOrderItemDetailList[j].Highlight == true)
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), boldFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, boldFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", boldFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, boldFont));
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + v.ToString(), NormalFont));
                                        table.AddCell(cell1);
                                        if (string.IsNullOrEmpty(_VariationOrderItemDetailList[j].item_remarks))
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description, NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        else
                                        {
                                            PdfPCell cell2 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Item.item_description + " (" + _ProjectTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                            table.AddCell(cell2);
                                        }
                                        if (Qty == true)
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase(_VariationOrderItemDetailList[j].Qty, NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);


                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                            PdfPCell cell5 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell5);
                                        }
                                        else
                                        {
                                            PdfPCell cell3 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                            cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell3);
                                            PdfPCell cell4 = new PdfPCell(new Phrase("$" + _VariationOrderItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                            cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            table.AddCell(cell4);
                                        }
                                    }
                                    v++;
                                    amount = amount + _VariationOrderItemDetailList[j].Amount;
                                }
                                else
                                {
                                    v = 1;
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



                    if (_VariationOrderDetails != null)
                    {


                        table = new PdfPTable(2);
                        table.WidthPercentage = 35;
                        table.HorizontalAlignment = 2;
                        //prabnadha 
                        decimal totalAfterDiscounts = _VariationOrderDetails.amount - _VariationOrderDetails.discount_amount;
                        string Total = totalAfterDiscounts.ToString("#,##0.00");

                        //chunk = new Chunk("Sub Total (A) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("$" + _VariationOrderDetails.addition_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("Sub Total (B) : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);

                        //chunk = new Chunk("$" + _VariationOrderDetails.omission_Amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        //cell = new PdfPCell();
                        ////cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //para = new Paragraph(chunk);
                        //para.Alignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(para);
                        //table.AddCell(cell);


                        chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _VariationOrderDetails.amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_VariationOrderDetails.discount_amount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _VariationOrderDetails.discount_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);

                            chunk = new Chunk("Total : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + Total, FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_VariationOrderDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);


                        chunk = new Chunk("$" + _VariationOrderDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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

                        chunk = new Chunk("$" + _VariationOrderDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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
                        if (!string.IsNullOrEmpty(_QuotationDetails.Customer_document_path))
                        {
                            cell = new PdfPCell();
                            //  para = new Paragraph(chunk);
                            cell.Border = 0;
                            Image imageCustomer = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.Customer_document_path);
                            imageCustomer.ScaleAbsolute(60, 50);
                            cell.AddElement(imageCustomer);
                            //cell.BorderWidthBottom = 1;
                            //para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            //cell.AddElement(para);
                            table.AddCell(cell);
                        }
                        //chunk = new Chunk("Salesman Signature Here ", FontFactory.GetFont("Times New Roman", "serif", 8, Font.NORMAL, BaseColor.BLACK));
                        //chunk.SetUnderline(1, -3);
                        if (!string.IsNullOrEmpty(_QuotationDetails.document_path))
                        {
                            cell = new PdfPCell();
                            Image image = Image.GetInstance(PMS.Common.Constants.PhysicalPath + _QuotationDetails.document_path);
                            image.ScaleAbsolute(60, 50);
                            cell.AddElement(image);
                            //para = new Paragraph(chunk);
                            cell.Border = 0;
                            //cell.BorderWidthBottom = 1;
                            // para.Alignment = Element.ALIGN_CENTER;
                            //para.SpacingBefore = 8f;
                            // cell.AddElement(para);
                            table.AddCell(cell);
                        }

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
                    //Response.AddHeader("content-disposition", "attachment;filename=VariationOrder.pdf");
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
                _GetDocumentList = null;
                _ProjectTasksItemList = null;
                _ProjectTasksItemDetailList = null;
                _GetProjectpaymentterms = null;
                _IMaster = null;
                _GetProjectContracttterms = null;
            }
            //return View();
        }
    }
}