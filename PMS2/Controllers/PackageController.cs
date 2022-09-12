using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PMS.Data_Access;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Interface;
using System.Threading.Tasks;

namespace PMS.Controllers
{
    public class PackageController : Controller
    {

        private IPackage _IPackage;

        public PackageController()
        {
            _IPackage = new DataLayer();
        }

        // GET: Package
        public ActionResult Packages()
        {

            return View();
        }

        public JsonResult PackagesList()
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<PackageList> _PackageList = new List<PackageList>();
            try
            {
                
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    _PackageList = _IPackage.PackagesList(uid);
                }
                else
                {
                    uid = User.Identity.GetUserId();
                    _PackageList = _IPackage.PackagesList(uid);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PackagesList");
                message = ex.Message;
            }
            finally
            {

            }
            var data = new
            {
                Items = _PackageList,
                TotalCount = _PackageList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreatePackage(string JsonPackage)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                PackageList _PackageList = JsonConvert.DeserializeObject<PackageList>(JsonPackage);
                _PackageList.userid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.CreatePackage(_PackageList);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreatePackage");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeletePackage(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<PackageList> _ItemList = JsonConvert.DeserializeObject<List<PackageList>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IPackage.DeletePackage(_ItemList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackage");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertPackageDetails(string JsonPackageDetails, string PackageId, string TaskId, string TaskName)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            PackageTasksItemList _PackageList;
            try
            {
                // ExceptionLog.WriteDebugLog("UpsertPackageDetails", "1");
                _PackageList = JsonConvert.DeserializeObject<PackageTasksItemList>(JsonPackageDetails);
                // ExceptionLog.WriteDebugLog("UpsertPackageDetails", "2");
                if (_PackageList.Package_Id == "")
                {
                    _PackageList.Package_Id = PackageId;
                }
                if (_PackageList.Task_Id == "" || _PackageList.Task_Id == "0" || _PackageList.Task_Id == null)
                {
                    _PackageList.Task_Id = TaskId;
                }
                if (_PackageList.Package_Det_Id == "")
                {
                    _PackageList.Package_Det_Id = "00000000-0000-0000-0000-000000000000";
                }
                _PackageList.Task_Name = TaskName;
                // ExceptionLog.WriteDebugLog("UpsertPackageDetails", "3");
                _PackageList.userid = User.Identity.GetUserId();
                //ExceptionLog.WriteDebugLog("UpsertPackageDetails", "4");
                _SuccessMessage = _IPackage.UpsertPackageDetails(_PackageList);
                //ExceptionLog.WriteDebugLog("UpsertPackageDetails", "5");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertPackageDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _PackageList = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPackageTasks()
        {
            string message = string.Empty;
            PackageTasksCriteria _PackageTasksCriteria = new PackageTasksCriteria();
            List<PackageTasksList> _PackageTasksList = new List<PackageTasksList>();
            try
            {
                _PackageTasksList = _IPackage.GetPackageTasks(_PackageTasksCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasks");
                message = ex.Message;
            }
            finally
            {
                _PackageTasksCriteria = null;
            }
            var data = new
            {
                Items = _PackageTasksList,
                TotalCount = _PackageTasksList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertPackage(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            UpsertPackageCriteria _UpsertPackageCriteria = JsonConvert.DeserializeObject<UpsertPackageCriteria>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IPackage.UpsertPackage(_UpsertPackageCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertPackage");
                message = ex.Message;
            }
            finally
            {
                _UpsertPackageCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewPackage()
        {
            PackageDetail _PackageList = new PackageDetail();
            //ViewBag.PackageId = PackageId;
            return View(_PackageList);
        }
        public JsonResult GetPackageTasksItem(string PackageId, string TaskId)
        {
            string message = string.Empty;
            //PackageTasksItemList _PackageTasksItemCriteria = new PackageTasksItemList();
            List<PackageTasksItem> _PackageTasksItemList = new List<PackageTasksItem>();
            try
            {
                _PackageTasksItemList = _IPackage.GetPackageTasksItem(PackageId, TaskId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksItem");
                message = ex.Message;
            }
            finally
            {
                //_PackageTasksItemCriteria = null;
            }
            var data = new
            {
                Items = _PackageTasksItemList,
                TotalCount = _PackageTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackageTasksListItem(string PackageId, string TaskId)
        {
            string message = string.Empty;
            //TaskDropDown _PackageTasksItemCriteria = new TaskDropDown();
            List<TaskDropDown> _PackageTasksItemList = new List<TaskDropDown>();
            try
            {
                _PackageTasksItemList = _IPackage.GetPackageTasksListItem(PackageId, TaskId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksListItem");
                message = ex.Message;
            }
            finally
            {
                // _PackageTasksItemCriteria = null;
            }
            //var data = new
            //{
            //    Items = _PackageTasksItemList,
            //    TotalCount = _PackageTasksItemList.Count
            //};
            return Json(new { data = _PackageTasksItemList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPackageTasksItemDetails(string PackageId, string TaskId)
        {
            string message = string.Empty;
            List<PackageTasksItemList> _PackageTasksItemList = new List<PackageTasksItemList>();
            try
            {
                //if(TaskId == "0" || TaskId == "")
                //{
                //    TaskId = "00000000-0000-0000-0000-000000000000";
                //}
                _PackageTasksItemList = _IPackage.GetPackageTasksItemDetails(PackageId, TaskId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksItemDetails");
                message = ex.Message;
            }
            finally
            {
            }
            var data = new
            {
                Items = _PackageTasksItemList,
                TotalCount = _PackageTasksItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPackageAmount()
        {
            string message = string.Empty;
            PackageAmountList _PackageAmountCriteria = new PackageAmountList();
            List<PackageAmountList> _PackageAmountList = new List<PackageAmountList>();
            try
            {
                _PackageAmountList = _IPackage.GetPackageAmount(_PackageAmountCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageAmount");
                message = ex.Message;
            }
            finally
            {
                _PackageAmountCriteria = null;
            }
            var data = new
            {
                Items = _PackageAmountList,
                TotalCount = _PackageAmountList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PackageDetails(string PackageId)
        {
            ViewBag.PackageId = PackageId;
            return View();
        }

        public ActionResult PackageDetail(string PackageId)
        {
            PackageDetail _PackageList = new PackageDetail();
            try
            {
                ViewBag.PackageId = PackageId;
                _PackageList = _IPackage.GetPackagesByPackageId(PackageId);
                _PackageList.fromDate = _PackageList.valid_from.ToShortDateString();
                _PackageList.toDate = _PackageList.valid_to.ToShortDateString();
                //  PackageList = IPackage.GetPackagesByPackageId(PackageId);                              
                return View(_PackageList);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PackageDetail");
                return null;
            }
            finally
            {

            }
        }
        public JsonResult GetPackagepaymentterms(string PackageId)
        {
            string message = string.Empty;
            List<PaymentTerms> _GetPackagepaymentterms = new List<PaymentTerms>();
            try
            {
                _GetPackagepaymentterms = _IPackage.GetPackagepaymentterms(PackageId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackagepaymentterms");
                message = ex.Message;
            }
            finally
            {
                PackageId = null;
            }
            var data = new
            {
                Items = _GetPackagepaymentterms,
                TotalCount = _GetPackagepaymentterms.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertPackagePaymentTerms(string JsonPackage, string PackageId)//string PackageId, string payment_term_id, string payment_description
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = string.Empty;
            PaymentTerms _PackagePaymentTermsCriteria = JsonConvert.DeserializeObject<PaymentTerms>(JsonPackage);
            // Package _PackagePaymentTermsCriteria = new Package();
            try
            {

                //_PackagePaymentTermsCriteria.payment_term_id = payment_term_id;
                //_PackagePaymentTermsCriteria.payment_description = payment_description;
                //_PackagePaymentTermsCriteria.package_id = PackageId;
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.UpsertPackagePaymentTerms(_PackagePaymentTermsCriteria, uid, PackageId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertPackagePaymentTerms");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _PackagePaymentTermsCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertPackageTasks(FormCollection obj, string PackageId)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<PackageTasksItemList> _PackageTaskCriteria = JsonConvert.DeserializeObject<List<PackageTasksItemList>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IPackage.UpsertPackageTasks(_PackageTaskCriteria, PackageId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertPackageTasks");
                message = ex.Message;
            }
            finally
            {
                _PackageTaskCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuotationForSearchPackage(string JsonValues)
        {
            string message = string.Empty;
            List<PackageDetail> _PackageList = new List<PackageDetail>();

            PackageDetail _PackageListCriteria = JsonConvert.DeserializeObject<PackageDetail>(JsonValues);
            try
            {
                _PackageList = _IPackage.GetQuotationForSearchPackage(_PackageListCriteria);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationForSearchPackage");
                message = ex.Message;
            }
            finally
            {

            }
            var data = new
            {
                Items = _PackageList,
                TotalCount = _PackageList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertPackage_PackageDetails(string JsonPackageDetails, string JsonGridDetails, string PackageId, string TaskId, string TaskName)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            NewPackageDetailTasksItem _GridList = JsonConvert.DeserializeObject<NewPackageDetailTasksItem>(JsonGridDetails);
            NewPackageDetailTasksItem _HeaderDetails = JsonConvert.DeserializeObject<NewPackageDetailTasksItem>(JsonPackageDetails);
            try
            {
                //if (_GridList.package_id == "" && _GridList.Task_Id == "" && _GridList.Package_Det_Id == "")
                //{
                //    _GridList.package_id = PackageId;
                //    _GridList.Task_Id = TaskId;
                //    _GridList.Package_Det_Id = "00000000-0000-0000-0000-000000000000";
                //}
                if (_GridList.package_id == "")
                {
                    _GridList.package_id = PackageId;
                }
                if (_GridList.Task_Id == "" || _GridList.Task_Id == "0" || _GridList.Task_Id == null)
                {
                    _GridList.Task_Id = TaskId;
                }
                if (_GridList.Package_Det_Id == "")
                {
                    _GridList.Package_Det_Id = "00000000-0000-0000-0000-000000000000";
                }
                _GridList.Task_Name = TaskName;
                string uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.UpsertPackage_PackageDetails(_GridList, _HeaderDetails, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertPackage_PackageDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _GridList = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePackages(string package_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.DeletePackages(uid, package_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackages");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult deletepackageDetails(string package_det_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.deletepackageDetails(uid, package_det_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: deletepackageDetails");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePackagePaymentTermsByID(string Payment_term_id)
        {
            string uid = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.DeletePackagePaymentTermsByID(uid, Payment_term_id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackagePaymentTermsByID");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update_ActDeactPackage(string JsonPackage)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = string.Empty;
            PackageList _PackageActDeactCriteria = JsonConvert.DeserializeObject<PackageList>(JsonPackage);
            try
            {
                string PackageId = _PackageActDeactCriteria.package_id;
                Boolean IsActive = _PackageActDeactCriteria.isactive;
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.Update_ActDeactPackage(PackageId, IsActive);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Update_ActDeactPackage");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _PackageActDeactCriteria = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Upsert_Package_For_Clone(string JsonPackage)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                PackageList _PackageList = JsonConvert.DeserializeObject<PackageList>(JsonPackage);
                _PackageList.userid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.Upsert_Package_For_Clone(_PackageList);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Upsert_Package_For_Clone");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Upsert_Package_For_Clone_InProject(string JsonPackage)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                PackageList _PackageList = JsonConvert.DeserializeObject<PackageList>(JsonPackage);
                _PackageList.userid = User.Identity.GetUserId();
                _SuccessMessage = _IPackage.Upsert_Package_For_Clone_InProject(_PackageList);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Upsert_Package_For_Clone_InProject");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public void PrintPreview(string Id, string TaskId, Boolean Qty)
        {
            byte[] bytes;
            int z = 1;
            decimal amount = 0;
            float[] columnWidths;
            PackageTasksItemList _PackageTasksItemCriteria = new PackageTasksItemList();
            //List<PackageDetail> _PackagesList = new List<PackageDetail>();
            PackageDetail _PackagesList = new PackageDetail();
            List<PackageTasksItem> _PackageTasksItemList = new List<PackageTasksItem>();
            List<PackageTasksItemList> _PackageTasksItemDetailList = new List<PackageTasksItemList>();
            PackageDetail _PackageDetails = new PackageDetail();
            List<PaymentTerms> _GetPackagepaymentterms = new List<PaymentTerms>();
            Font boldFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK);
            Font NormalFont = FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK);
            try
            {
                _PackageTasksItemCriteria.Package_Id = Id;
                _PackageTasksItemCriteria.Task_Id = TaskId;
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 110, 50);
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfWriter.PageEvent = new PDFFooter();
                    pdfDoc.Open();

                    Chunk chunk = new Chunk();
                    PdfPCell cell = new PdfPCell();
                    PdfPTable table;
                    _PackagesList = _IPackage.GetPackagesByPackageId(_PackageTasksItemCriteria.Package_Id);
                    if (_PackagesList != null)
                    {
                        // Table
                        table = new PdfPTable(6);
                        table.WidthPercentage = 100;
                        table.HorizontalAlignment = 0;
                        columnWidths = new float[] { 10f, 30f, 8f, 10f, 14f, 20f };
                        table.SetWidths(columnWidths);
                        //table.SpacingBefore =600f;

                        chunk = new Chunk("Package : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        var para6 = new Paragraph(chunk);
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_PackagesList.package_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Floor : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);
                        chunk = new Chunk(_PackagesList.floor_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Package Code : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_PackagesList.package_cd, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);


                        chunk = new Chunk("Plan : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_PackagesList.plan_name, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Valid From : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);
                        chunk = new Chunk(_PackagesList.valid_from.ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        chunk = new Chunk("Valid To : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_PackagesList.valid_to.ToShortDateString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell = new PdfPCell();
                        cell.Border = 0;
                        cell.Colspan = 3;
                        cell.AddElement(chunk);
                        table.AddCell(cell);

                        cell = new PdfPCell();
                        chunk = new Chunk("Total Amount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
                        cell.Border = 0;
                        para6 = new Paragraph(chunk);
                        para6.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para6);
                        table.AddCell(cell);

                        chunk = new Chunk(_PackagesList.total_amount.ToString(), FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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
                    chunk = new Chunk("Renovation Package", FontFactory.GetFont("Times New Roman", "serif", 12, Font.NORMAL, BaseColor.BLACK));
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


                    _PackageTasksItemList = _IPackage.GetPackageTasksItem(Id, TaskId);
                    _PackageTasksItemDetailList = _IPackage.GetPackageTasksItemDetails(Id, TaskId);
                    if (_PackageTasksItemList != null)
                    {
                        for (int i = 0; i < _PackageTasksItemList.Count; i++)
                        {

                            //Cell

                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1) + ". " + _PackageTasksItemList[i].Task.Task_Name);
                            chunk.SetUnderline(1, -3);
                            cell.AddElement(chunk);
                            cell.Colspan = 5;
                            cell.Border = 0;
                            //cell.BackgroundColor = BaseColor.PINK;
                            table.AddCell(cell);



                            for (int j = 0; j < _PackageTasksItemDetailList.Count; j++)
                            {
                                if (_PackageTasksItemDetailList[j].Task_Name == _PackageTasksItemList[i].Task.Task_Name)
                                {
                                    //if (_PackageTasksItemDetailList[j].Highlight == true)
                                    //{
                                    //    PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), boldFont));
                                    //    table.AddCell(cell1);
                                    //    if (string.IsNullOrEmpty(_PackageTasksItemDetailList[j].item_remarks))
                                    //    {
                                    //        PdfPCell cell2 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Item.item_description, boldFont));
                                    //        table.AddCell(cell2);
                                    //    }
                                    //    else
                                    //    {
                                    //        PdfPCell cell2 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Item.item_description + " (" + _PackageTasksItemDetailList[j].item_remarks + ")", boldFont));
                                    //        table.AddCell(cell2);
                                    //    }
                                    //    if (Qty == true)
                                    //    {
                                    //        PdfPCell cell3 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Qty.ToString(), boldFont));
                                    //        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //        table.AddCell(cell3);

                                    //        PdfPCell cell4 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                    //        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //        table.AddCell(cell4);

                                    //        PdfPCell cell5 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                    //        cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //        table.AddCell(cell5);
                                    //    }
                                    //    else
                                    //    {
                                    //        PdfPCell cell3 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Price.ToString("#,##0.00"), boldFont));
                                    //        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //        table.AddCell(cell3);
                                    //        PdfPCell cell4 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Amount.ToString("#,##0.00"), boldFont));
                                    //        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //        table.AddCell(cell4);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    PdfPCell cell1 = new PdfPCell(new Phrase((i + 1).ToString() + "." + z.ToString(), NormalFont));
                                    table.AddCell(cell1);
                                    if (string.IsNullOrEmpty(_PackageTasksItemDetailList[j].item_remarks))
                                    {
                                        PdfPCell cell2 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Item.item_description, NormalFont));
                                        table.AddCell(cell2);
                                    }
                                    else
                                    {
                                        PdfPCell cell2 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Item.item_description + " (" + _PackageTasksItemDetailList[j].item_remarks + ")", NormalFont));
                                        table.AddCell(cell2);
                                    }
                                    if (Qty == true)
                                    {
                                        PdfPCell cell3 = new PdfPCell(new Phrase(_PackageTasksItemDetailList[j].Qty.ToString(), NormalFont));
                                        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        table.AddCell(cell3);


                                        PdfPCell cell4 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        table.AddCell(cell4);
                                        PdfPCell cell5 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                        cell5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        table.AddCell(cell5);
                                    }
                                    else
                                    {
                                        PdfPCell cell3 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Price.ToString("#,##0.00"), NormalFont));
                                        cell3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        table.AddCell(cell3);
                                        PdfPCell cell4 = new PdfPCell(new Phrase("$" + _PackageTasksItemDetailList[j].Amount.ToString("#,##0.00"), NormalFont));
                                        cell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        table.AddCell(cell4);
                                    }
                                    //table.AddCell((i + 1).ToString() + "." + z.ToString());
                                    //table.AddCell(_PackageTasksItemDetailList[j].Item.item_description);
                                    //if (Qty == true)
                                    //{
                                    //    table.AddCell(_PackageTasksItemDetailList[j].Qty);
                                    //}
                                    //table.AddCell(_PackageTasksItemDetailList[j].Price.ToString());
                                    //table.AddCell(_PackageTasksItemDetailList[j].Amount.ToString());
                                    // }
                                    z++;
                                    amount = amount + _PackageTasksItemDetailList[j].Amount;
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

                    _PackageDetails = _IPackage.GetPackagesByPackageId(Id);

                    if (_PackageDetails != null)
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

                        chunk = new Chunk("$" + _PackageDetails.amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        if (_PackageDetails.discount_amount.ToString() != "0.00")
                        {
                            chunk = new Chunk("Less Discount : ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            table.AddCell(cell);

                            chunk = new Chunk("$" + _PackageDetails.discount_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                            cell = new PdfPCell();
                            //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            para = new Paragraph(chunk);
                            para.Alignment = Element.ALIGN_RIGHT;
                            cell.AddElement(para);
                            //cell.AddElement(chunk);
                            table.AddCell(cell);
                        }

                        chunk = new Chunk(_PackageDetails.gst_percentage + "% GST: ", FontFactory.GetFont("Times New Roman", "serif", 10, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        table.AddCell(cell);

                        chunk = new Chunk("$" + _PackageDetails.gst_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
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

                        chunk = new Chunk("$" + _PackageDetails.total_amount.ToString("#,##0.00"), FontFactory.GetFont("Times New Roman", "serif", 11, Font.BOLD, BaseColor.BLACK));
                        cell = new PdfPCell();
                        para = new Paragraph(chunk);
                        para.Alignment = Element.ALIGN_RIGHT;
                        cell.AddElement(para);
                        //cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //cell.AddElement(chunk);
                        table.AddCell(cell);

                        pdfDoc.Add(table);
                    }

                    _GetPackagepaymentterms = _IPackage.GetPackagepaymentterms(Id);

                    if (_GetPackagepaymentterms.Count != 0)
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

                        for (int i = 0; i < _GetPackagepaymentterms.Count; i++)
                        {
                            cell = new PdfPCell();
                            chunk = new Chunk((i + 1).ToString() + ". " + _GetPackagepaymentterms[i].paymentdescription.Master_payment_description, FontFactory.GetFont("Times New Roman", "serif", 10, Font.NORMAL, BaseColor.BLACK));
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

                    pdfWriter.CloseStream = false;
                    pdfDoc.Close();

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=Package.pdf");
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
                _PackageTasksItemCriteria = null;
                _PackagesList = null;
                _PackageTasksItemList = null;
                _PackageTasksItemDetailList = null;
                _GetPackagepaymentterms = null;
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

    }
}