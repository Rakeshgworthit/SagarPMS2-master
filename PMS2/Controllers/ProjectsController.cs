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
using System.Configuration;
using System.IO;
using PMS.Data_Access;
using System.Data;
using PMS.Interface;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ProjectsController : Controller
    {
        // GET: Projects
        private IContract _IContract;
        private readonly IProject _ProjectRepo;
        private readonly IProjectAdditionRepository _ProjectAdditionRepo;
        private readonly ICustomerRepositor _CustomerRepo;
        private readonly IDiscountRepository _DiscountRepo;
        
        public ProjectsController(IProject projRepo, IProjectAdditionRepository additionRepo, ICustomerRepositor custRepo, IDiscountRepository discountRepo)
        {
            _ProjectRepo = projRepo;
            _ProjectAdditionRepo = additionRepo;
            _CustomerRepo = custRepo;
            _DiscountRepo = discountRepo;
        }

        public ActionResult Index(ProjectViewModel objView)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.UID = "00000000-0000-0000-0000-000000000000";
                    //branchList = Common.CommonFunction.BranchList();
                }
                else
                {
                    objView.UID = User.Identity.GetUserId();
                    // branchList = Common.CommonFunction.UserBranchList(objView.UID);
                }
                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                // objView.BranchList = branchList;

                objView.StatusList = CommonFunction.ProjectStatusList();
                objView.SalesmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
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
                branchList = null;
            }
        }

        public ActionResult _LoadProject(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            ProjectViewModel objView = new ProjectViewModel();
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
                        //project objRec = _ProjectRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            Common.CommonFunction.MergeObjects(objView, objRec, true);
                        }
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
                    objView.status_id = 2;
                }

                objView.SalesmenList = Common.CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                objView.BankList = Common.CommonFunction.BankList();
                objView.CustomerList = Common.CommonFunction.CustomerList();

                //if (User.IsInRole("SuperAdmin"))
                //{
                //    objView.BranchList = Common.CommonFunction.BranchList();
                //}
                //else
                //{                
                //    objView.BranchList = Common.CommonFunction.UserBranchList(uid);
                //}

                objView.StatusList = Common.CommonFunction.ProjectStatusList();
                objView.ActiveInactiveList = Common.CommonFunction.StatusList();

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: _LoadProject, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public JsonResult SaveProject(ProjectViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            project objRec = new project();
            try
            {
                if (objView.id > 0)
                {
                    objRec = _ProjectRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.created_by = objRec.created_by;
                        objView.created_date = objRec.created_date;
                        //objView.branch_id = objRec.branch_id;
                        objView.branch_id = SessionManagement.SelectedBranchID;
                        CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _ProjectRepo.Save();
                    }
                    return Json(new { msg = "Project updated successfully.", cls = "success" });
                }
                else
                {
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.branch_id = SessionManagement.SelectedBranchID;
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _ProjectRepo.Add(objRec);
                    _ProjectRepo.Save();
                    return Json(new { msg = "Project created successfully.", cls = "success" });

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveProject, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public ActionResult Additions(ProjectAdditionsViewModel objView, int ProjectId =0)
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
                if (!SessionManagement.IsBranchAdmin)
                {
                    if (User.IsInRole("Salesman"))
                    {
                        objView.ProjectSalesmenId = Common.CommonFunction.SalesmenIDByUserID(objView.UID);
                        objView.SalesmenList = Common.CommonFunction.GetSalesmenIdByUserId(objView.UID);
                    }
                    else
                    {
                        objView.SalesmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                    }

                }
                else
                {
                    objView.SalesmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                }
                DateTime now = DateTime.Now;
                // var startDate = new DateTime(now.Year, (now.Month - 5), 1);
                var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
                var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
                if (objView.SearchFrom == null)
                {
                    objView.SearchFrom = CurrentstartDate.AddMonths(-1);
                }

                if (objView.SearchTo == null)
                {
                    objView.SearchTo = endDate;
                }
                objView.isProjectClosed = false;
                if (ProjectId > 0)
                {
                    objView.SearchProject = ProjectId;
                }
                objView.ProjectList = CommonFunction.UserProjectList(objView.UID, ProjectId, objView.ProjectSalesmenId);


                if (TempData["Message"] != null)
                    ViewBag.Message = TempData["Message"].ToString();

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Additions, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }

        public ActionResult _LoadAdditions(string Id, int projectId)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            ProjectAdditionsViewModel objView = new ProjectAdditionsViewModel();
            objView.AdditionTypeList = CommonFunction.AdditionOmissionTypeList();           
            List<project_document_list> objPDList = new List<project_document_list>();
            //Int32 projectId = 0;
            int SalesmanId = 0;
            try
            {
                if (Id != null && Id != "0")
                // if(Id>0)
                {
                    int id = 0;
                    id = Int32.Parse(Id.Substring(Id.LastIndexOf('-') + 1));
                    int Type = Int32.Parse(string.Concat(Id.TakeWhile((c) => c != '-')));
                    if (User.IsInRole("SuperAdmin") || User.IsInRole("User"))
                    {

                        additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == id).SingleOrDefault();
                        if (objRec != null)
                        {
                           projectId = Convert.ToInt32(objRec.project_id);
                            CommonFunction.MergeObjects(objView, objRec, true);
                            //objView.ProjectList = CommonFunction.UserProjectListWithID("00000000-0000-0000-0000-000000000000", projectId);                            
                            objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000",projectId,0);
                        }
                        discount objDisc = _DiscountRepo.FindBy(o => o.id == id).SingleOrDefault();
                        if (objDisc != null)
                        {
                            projectId = Convert.ToInt32(objDisc.project_id);
                            CommonFunction.MergeObjects(objView, objDisc, true);
                            objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000", projectId, 0);
                        }
                    }
                    else 
                    {
                        //additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == id).SingleOrDefault();
                        if (objRec != null)
                        {
                            projectId = Convert.ToInt32(objRec.project_id);
                            CommonFunction.MergeObjects(objView, objRec, true);
                            if(!SessionManagement.IsBranchAdmin)
                            {
                                SalesmanId = CommonFunction.SalesmenIDByUserID(uid);
                            }                            
                            objView.ProjectList = CommonFunction.UserProjectList(uid, projectId, SalesmanId);
                        }
                        discount objDisc = _DiscountRepo.FindBy(o => o.id == id).SingleOrDefault();
                        if (objDisc != null)
                        {
                            projectId = Convert.ToInt32(objDisc.project_id);
                            CommonFunction.MergeObjects(objView, objDisc, true);
                            SalesmanId = CommonFunction.SalesmenIDByUserID(uid);
                            objView.ProjectList = CommonFunction.UserProjectList(uid, projectId, SalesmanId);
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
                    ViewBag.propertydisable = true;
                    objView.GSTList = CommonFunction.GSTList();
                    ViewBag.RecordType = Common.CommonFunction.GetVoType(objView.record_type.ToString());
                    objView.project_document_list = CommonFunction.GetDocuments(Convert.ToInt64(id), Common.Constants.GetDocumentIdType.AdditionsOmissions);
                }
                else
                {
                    objView.amount = Convert.ToDecimal(0.00);
                    objView.GSTList = CommonFunction.GSTList();
                    objView.gst_percentage = Convert.ToDecimal(Common.SessionManagement.BranchGST);
                    objView.gst_amount = Convert.ToDecimal(0.00);
                    objView.total_amount = Convert.ToDecimal(0.00);
                    objView.date = DateTime.Now;
                    objView.isactive = true;
                    objView.project_id = projectId;
                    objView.isProjectClosed = false;
                    ViewBag.propertydisable = false;
                    objView.project_document_list = objPDList;
                    objView.record_type = Common.Constants.VoTypeList.Addition;
                    ViewBag.RecordType = Common.CommonFunction.GetVoType(objView.record_type.ToString());
                }
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000", projectId, 0);
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                    {
                        SalesmanId = CommonFunction.SalesmenIDByUserID(uid);
                    }
                    objView.ProjectList = CommonFunction.UserProjectList(uid, projectId, SalesmanId);
                    //objView.ProjectList = CommonFunction.UserProjectListWithID(uid, projectId);
                }
                else
                {
                    objView.ProjectList = CommonFunction.UserProjectList(uid, projectId, SalesmanId);
                }
                objView.ActiveInactiveList = Common.CommonFunction.StatusList();
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: _LoadAdditions, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }    
        public JsonResult SaveProjectAdditions(ProjectAdditionsViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            additions_omissions objRec = new additions_omissions();
            discount objDis = new discount();
            string Id = "";
            try
            {
                if (objView.id > 0)
                {
                    if (objView.record_type == 3)
                    {
                        objDis = _DiscountRepo.FindBy(o => o.id == objView.id && o.record_type == objView.record_type && o.project_id == objView.project_id).SingleOrDefault();
                        if (objDis != null)
                        {
                            objView.created_by = objDis.created_by;
                            objView.created_date = objDis.created_date;
                            CommonFunction.MergeObjects(objDis, objView, true);
                            objDis.modified_date = DateTime.Now;
                            objDis.modified_by = new Guid(uid);
                            _DiscountRepo.Save();
                            Id = objDis.record_type + "-" + objDis.id;
                        }
                    }
                    else
                    {
                        objRec = _ProjectAdditionRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                        if (objRec != null)
                        {
                            objView.created_by = objRec.created_by;
                            objView.created_date = objRec.created_date;
                            CommonFunction.MergeObjects(objRec, objView, true);
                            objRec.modified_date = DateTime.Now;
                            objRec.modified_by = new Guid(uid);
                            _ProjectAdditionRepo.Save();
                            Id = objRec.record_type + "-" + objRec.id;
                        }
                    }
                    return Json(new { msg = "Project addition updated successfully.", id = Id, cls = "success" });
                }
                else
                {
                    if (objView.record_type == 3)
                    {
                        CommonFunction.MergeObjects(objDis, objView, true);
                        objDis.created_date = DateTime.Now;
                        objDis.created_by = new Guid(uid);
                        objDis.modified_date = DateTime.Now;
                        objDis.modified_by = new Guid(uid);
                        _DiscountRepo.Add(objDis);
                        _DiscountRepo.Save();
                        Id = objDis.record_type + "-" + objDis.id;
                    }
                    else
                    {
                        CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.created_date = DateTime.Now;
                        objRec.created_by = new Guid(uid);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _ProjectAdditionRepo.Add(objRec);
                        _ProjectAdditionRepo.Save();
                        Id = objRec.record_type + "-" + objRec.id;
                    }
                    return Json(new { msg = "Project addition created successfully.",id = Id, cls = "success" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveProjectAdditions, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
                objDis = null;
            }
        }

        public string GetSalemanCommision(Int32 salemanid)
        {
            try
            {
                return CommonFunction.GetSalemanCommision(salemanid).ToString();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public string GetCustomerData(Int32 customerid)
        {
            try
            {
                return CommonFunction.GetCustomerData(customerid).ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public ActionResult DeleteAdditionById(string Id)
        {
            // if (Id>0)
            try
            {
                if (Id != null && Id != "0")
                {
                    int id = 0;
                    id = Int32.Parse(Id.Substring(Id.LastIndexOf('-') + 1));
                    int Type = Int32.Parse(string.Concat(Id.TakeWhile((c) => c != '-')));
                    additions_omissions objRec = _ProjectAdditionRepo.FindBy(o => o.id == id && o.record_type == Type).SingleOrDefault();
                    discount objDis = objDis = _DiscountRepo.FindBy(o => o.id == id && o.record_type == Type).SingleOrDefault();
                    if (objRec != null && objRec.record_type != 3)
                    {
                        _ProjectAdditionRepo.Delete(objRec);
                        _ProjectAdditionRepo.Save();
                    }

                    else if (objDis != null && objDis.record_type == 3)
                    {
                        _DiscountRepo.Delete(objDis);
                        _DiscountRepo.Save();
                    }
                    TempData["Message"] = "Additions deleted successfully.";
                }
                return RedirectToAction("Additions");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DeleteAdditionById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
            }
        }

        public ActionResult DeleteProjectById(Int32 Id)
        {
            try
            {
                if (Id > 0)
                {
                    project objRec = _ProjectRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    _ProjectRepo.Delete(objRec);
                    _ProjectRepo.Save();
                    TempData["Message"] = "Contract deleted successfully.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DeleteProjectById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
            }
        }

        #region Old Code for Documents
        //public ActionResult ProjectDocument(Int32 id, bool viewdoc = false)
        //{
        //    ProjectDocumentViewModel objView = new ProjectDocumentViewModel();
        //    List<project_document_list> objPDList = new List<project_document_list>();
        //    try
        //    {
        //        objPDList = Common.CommonFunction.GetProjectDocuments(id);

        //        if (objView == null)
        //        {
        //            objView.project_id = id;
        //        }
        //        else
        //        {
        //            objView.project_document_list = objPDList;
        //            objView.project_id = id;
        //        }
        //        if (viewdoc == true)
        //        {
        //            return View("LoadDocuments", objView);
        //        }
        //        else
        //        {
        //            return View(objView);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, $"Method Name: ProjectDocument, Parameter : id={id} , viewdoc={viewdoc} , ");
        //        return null;
        //    }
        //    finally
        //    {
        //        objView = null;
        //        objPDList = null;
        //    }
        //}

        //public void DeleteDocument(Int32 Id)
        //{
        //    Int32 project_id = 0;
        //    using (PMSEntities objDB = new PMSEntities())
        //    {
        //        tbl_projectdocument objRec = objDB.tbl_projectdocument.Where(o => o.Id == Id).SingleOrDefault();
        //        if (objRec != null)
        //        {
        //            project_id = Convert.ToInt32(objRec.project_id);
        //            System.IO.File.Delete(Server.MapPath("~/Content/ContractDucuments/" + objRec.file_name));
        //            objDB.tbl_projectdocument.Remove(objRec);
        //            objDB.SaveChanges();
        //        }
        //    }
        //    // return RedirectToAction("ProjectDocument", new { Id = project_id });
        //}

        //public ActionResult SaveDocument(Int32 project_id, string file_desc)
        //{
        //    string uid = User.Identity.GetUserId();
        //    string fname = "";
        //    string filename = "";
        //    try
        //    {
        //        if (HttpContext.Request.Files.Count > 0)
        //        {
        //            HttpFileCollectionBase files = Request.Files;
        //            HttpPostedFileBase file = files[0];
        //            if (file.FileName != "")
        //            {
        //                // Checking for Internet Explorer  
        //                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
        //                {
        //                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
        //                    fname = project_id.ToString() + "_" + testfiles[testfiles.Length - 1];
        //                }
        //                else
        //                {
        //                    fname = project_id.ToString() + "_" + file.FileName;
        //                }
        //                fname = fname.Replace(" ", "").Replace("#", "_");
        //                // Get the complete folder path and store the file inside it.  
        //                filename = System.IO.Path.Combine(Server.MapPath("~/Content/ContractDucuments/"), fname);
        //                //filename = project_id.ToString() + "_" + filename;
        //                file.SaveAs(filename);
        //                using (PMSEntities objDB = new PMSEntities())
        //                {
        //                    //tbl_projectdocument objRec = objDB.tbl_projectdocument.Where(o => o.project_id == Id).SingleOrDefault();
        //                    //if (objRec != null)
        //                    //{
        //                    //    System.IO.File.Delete(Server.MapPath("~/Content/ContractDucuments/" + objRec.file_name));

        //                    //    objRec.file_name = fname;
        //                    //    objRec.uploaded_on = DateTime.Now;
        //                    //    objRec.uploaded_by = new Guid(uid);
        //                    //    objDB.SaveChanges();
        //                    //}else
        //                    //{
        //                    tbl_projectdocument objRec2 = new tbl_projectdocument
        //                    {
        //                        file_name = fname,
        //                        file_desc = file_desc,
        //                        project_id = project_id,
        //                        uploaded_by = new Guid(uid),
        //                        uploaded_on = DateTime.Now
        //                    };
        //                    objDB.tbl_projectdocument.Add(objRec2);
        //                    objDB.SaveChanges();
        //                    // }
        //                }
        //            }
        //        }
        //        return RedirectToAction("ProjectDocument", new { Id = project_id });
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, $"Method Name: SaveDocument, Parameter : project_id={project_id} , file_desc={file_desc} , ");
        //        return null;
        //    }
        //    finally
        //    {
        //    }
        //}

        #endregion
        public ActionResult ProjectDocument(Int32 id, bool IsProject, bool viewdoc = false,bool IsDashBoard = true,bool IsContract = false)
        {
            ProjectDocumentViewModel objView = new ProjectDocumentViewModel();
            List<project_document_list> objPDList = new List<project_document_list>();
           
            IDataReader Ireader;
            try
            {
                if (IsDashBoard)
                {
                    using (var cmd = new DBSqlCommand())
                    {
                        if (!IsProject)
                        {
                            cmd.AddParameters(id, "@CustomerId", SqlDbType.BigInt);
                            Ireader = cmd.ExecuteDataReader("[dbo].[Get_Documents_For_Customer]");
                            while (Ireader.Read())
                            {
                                project_document_list _GetQuotationDeatils = new project_document_list();
                                _GetQuotationDeatils.document_id = Ireader.GetInt64("document_id");
                                _GetQuotationDeatils.document_path = Ireader.GetString("Doc_Path");
                                _GetQuotationDeatils.file_name = Ireader.GetString("file_name");
                                // _GetQuotationDeatils.document_name = Ireader.GetString(CommonColumns.document_name);
                                _GetQuotationDeatils.project_number = Ireader.GetString("project_number");
                                _GetQuotationDeatils.project_id = Ireader.GetInt64("project_id");
                                _GetQuotationDeatils.uploaded_on = Ireader.GetDateTime("uploaded_on");
                                _GetQuotationDeatils.Uploaded_By_Name = Ireader.GetInt32("Uploaded_By_Name");
                                _GetQuotationDeatils.Id = Ireader.GetInt16("Id");
                                _GetQuotationDeatils.file_desc = Ireader.GetString("file_Desc");
                                _GetQuotationDeatils.UploadedName = Ireader.GetString("UploadedName");
                                objPDList.Add(_GetQuotationDeatils);
                            }
                        }
                        else
                        {
                            cmd.AddParameters(id, "@projectid", SqlDbType.Int);
                            Ireader = cmd.ExecuteDataReader("[dbo].[Get_Documents_For_Contracts]");
                            while (Ireader.Read())
                            {
                                project_document_list _GetQuotationDeatils = new project_document_list();
                                _GetQuotationDeatils.document_id = Ireader.GetInt64("document_id");
                                _GetQuotationDeatils.document_path = Ireader.GetString("Doc_Path");
                                _GetQuotationDeatils.file_name = Ireader.GetString("file_name");
                                // _GetQuotationDeatils.document_name = Ireader.GetString(CommonColumns.document_name);
                                _GetQuotationDeatils.project_number = Ireader.GetString("project_number");
                                _GetQuotationDeatils.project_id = Ireader.GetInt64("project_id");
                                _GetQuotationDeatils.uploaded_on = Ireader.GetDateTime("uploaded_on");
                                _GetQuotationDeatils.Uploaded_By_Name = Ireader.GetInt32("Uploaded_By_Name");
                                _GetQuotationDeatils.Id = Ireader.GetInt16("Id");
                                _GetQuotationDeatils.file_desc = Ireader.GetString("file_Desc");
                                _GetQuotationDeatils.UploadedName = Ireader.GetString("UploadedName");
                                objPDList.Add(_GetQuotationDeatils);
                            }
                        }
                    }
                    if (objView == null)
                    {
                        
                        objView.project_document_list = objPDList;
                    }
                    else
                    {
                        objView.project_document_list = objPDList;
                    }
                    objView.project_id = id;
                    objView.IsProject = Convert.ToString(IsProject);
                    objView.IsDashBoard = Convert.ToString(IsDashBoard);
                    objView.IsContract = Convert.ToString(IsContract);
                }
                else
                {
                    if (IsContract)
                    {
                        objPDList = CommonFunction.GetDocuments(id, Common.Constants.GetDocumentIdType.Contract);
                        objView.project_document_list = objPDList;
                        objView.project_id = id;
                        
                    }
                    else
                    {
                        objPDList = CommonFunction.GetDocuments(id, Common.Constants.GetDocumentIdType.AdditionsOmissions);
                        objView.project_id = id;
                        objView.project_document_list = objPDList;           
                    }

                    objView.IsProject = Convert.ToString(IsProject);
                    objView.IsDashBoard = Convert.ToString(IsDashBoard);
                    objView.IsContract = Convert.ToString(IsContract);
                }
                if (viewdoc == true)
                {
                    return View("LoadDocuments", objView);
                }
                else
                {
                    return View(objView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            finally
            {
                objView = null;
                objPDList = null;
            }
        }

        public ActionResult SaveDocument(Int32 project_id, string IsProject , string file_desc, string IsDashBoard, string IsContract)
        {
            string uid = User.Identity.GetUserId();
            string fname = "";
            string filename = "";
            string extension = string.Empty;
            string fileNameWithExt = string.Empty;
            string filepathRoot = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            string doc_Path = "";
            string UploadPath = "";
            bool IsMaster = false;
            bool IsFromDashboard = Convert.ToBoolean(IsDashBoard);
            bool IsFromContract = Convert.ToBoolean(IsContract);
            try
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    fileNameWithExt = fileName;
                    if (IsProject =="False")
                    {
                        doc_Path = "/UploadDocuments/" + "/Customers/" + project_id;
                    }
                    else
                    {
                        doc_Path = "/UploadDocuments/" + "/Projects/" + project_id;
                    }
                    string dirPath = FilePath + doc_Path;
                    if (IsProject == "False")
                        UploadPath = "~/UploadDocuments/Customers/" + project_id;
                    else
                        UploadPath = "~/UploadDocuments/Projects/" + project_id;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string addTimeStampToFileName = string.Empty;
                    //if (file.ContentLength == 0)
                    //   continue;
                    if (file.ContentLength > 0)
                    {
                        //addTimeStampToFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName), DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetExtension(file.FileName));
                        filepathRoot = Path.Combine(HttpContext.Request.MapPath(UploadPath) + "/" + fileNameWithExt);
                        extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filepathRoot);
                    }
                    if (IsProject == "False")
                    {
                        Common.CommonFunction.UpsertMasterDocument(fileName, file_desc, project_id, extension, doc_Path, User.Identity.GetUserName());
                        IsMaster = false;
                    }                     
                    else if(!IsFromContract)
                    {
                        Common.CommonFunction.UpsertProjectDocument(fileName, file_desc, project_id, extension, doc_Path, new Guid(), User.Identity.GetUserName(), Common.Constants.GetDocumentIdType.AdditionsOmissions);
                        IsMaster = true;
                    }
                    else
                    {
                        Common.CommonFunction.UpsertProjectDocument(fileName, file_desc, project_id, extension, doc_Path, new Guid(), User.Identity.GetUserName());
                        IsMaster = true;
                    }

                }
                return RedirectToAction("ProjectDocument", new { Id = project_id, IsProject = IsMaster, IsDashBoard = IsFromDashboard, IsContract = IsFromContract });
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
            finally
            {
            }
        }

        [HttpPost]
        public ActionResult SaveDocumentNew(Int32 project_id, string IsProject,string file_desc,int IdType =0)
        {
            string uid = User.Identity.GetUserId();
            string fname = "";
            string filename = "";
            string extension = string.Empty;
            string fileNameWithExt = string.Empty;
            string filepathRoot = string.Empty;
            string FilePath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            string doc_Path = "";
            string UploadPath = "";
            bool IsMaster = false;
            try
            {
                
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0] as HttpPostedFileBase;
                    string fileName = file.FileName;
                    file_desc = file_desc;
                    fileNameWithExt = fileName;
                    if (IsProject == "False")
                    {
                        doc_Path = "/UploadDocuments/" + "/Customers/" + project_id;
                    }
                    else
                    {
                        doc_Path = "/UploadDocuments/" + "/Projects/" + project_id;
                    }
                    string dirPath = FilePath + doc_Path;
                    if (IsProject == "False")
                        UploadPath = "~/UploadDocuments/Customers/" + project_id;
                    else
                        UploadPath = "~/UploadDocuments/Projects/" + project_id;
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    string addTimeStampToFileName = string.Empty;
                    //if (file.ContentLength == 0)
                    //   continue;
                    if (file.ContentLength > 0)
                    {
                        //addTimeStampToFileName = string.Concat(Path.GetFileNameWithoutExtension(fileName), DateTime.Now.ToString("yyyyMMddHHmmssfff"), Path.GetExtension(file.FileName));
                        filepathRoot = Path.Combine(HttpContext.Request.MapPath(UploadPath) + "/" + fileNameWithExt);
                        extension = Path.GetExtension(file.FileName);
                        file.SaveAs(filepathRoot);
                    }
                    if (IsProject == "False")
                    {
                        Common.CommonFunction.UpsertMasterDocument(fileName, file_desc, project_id, extension, doc_Path, User.Identity.GetUserName());
                        IsMaster = false;
                    }
                    else
                    {
                        Common.CommonFunction.UpsertProjectDocument(fileName, file_desc, project_id, extension, doc_Path, new Guid(), User.Identity.GetUserName(), IdType);
                        IsMaster = true;
                    }

                }
                return Json(new { msg = "Document Uploaded successfully.", cls = "success", Id = project_id });
            }
            catch (Exception ex)
            {
                throw ex;
                return  Json(new { msg = "Document Uploaded Failed.", cls = "success", Id = project_id }); ;
            }
            finally
            {
            }
        }

        public void DeleteDocument(Int32 Id, string FilePath, string FileName, long project_id)
        {
            string DirectoryPath = ConfigurationManager.AppSettings["PhysicalPath"].ToString();
            string EnroutePath = "~" + FilePath + '/' + FileName;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath(EnroutePath));
                    cmd.AddParameters(Id, "@DocumentId", SqlDbType.BigInt);
                    cmd.AddParameters(project_id, "@CustomerId", SqlDbType.BigInt);
                    cmd.ExecuteNonQuery("[dbo].[SSP_DeleteDocument]");                 
                }
                catch (Exception ex)
                {

                }

            }
            // return RedirectToAction("ProjectDocument", new { Id = project_id });
        }


    }
}