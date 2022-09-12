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
    public class CustomerController : Controller
    {
        // GET: Customer
        private readonly ICustomerRepositor _CustRepo;
        private readonly IProject _ProjectRepo;
        public CustomerController(ICustomerRepositor custRepo, IProject ProjectRepo)
        {
            _CustRepo = custRepo;
            _ProjectRepo = ProjectRepo;
        }

        public ActionResult Index(CustomerViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            try
            {
                if (objView.customersearch == null)
                {
                    objView.customersearch = "";
                }
                else
                {
                    objView.customersearch = objView.customersearch;
                }
                if (TempData["Message"] != null && TempData["cls"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                    ViewBag.cls = TempData["cls"].ToString();
                }
                if (User.IsInRole("SuperAdmin"))
                {
                    objView.BranchList = Common.CommonFunction.BranchList();
                    objView.BranchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                }
                else
                {
                    objView.BranchList = Common.CommonFunction.UserBranchList(uid);
                    
                }
                if (objView.BranchId == 0)
                {
                    objView.BranchId = SessionManagement.SelectedBranchID;
                }
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

        public ActionResult _LoadCustomer(Int32 Id)
        {
            CustomerViewModel objView = new CustomerViewModel();
            customer cust = new customer();
            try
            {
                if (Id > 0)
                {
                    customer objRec = _CustRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        Common.CommonFunction.MergeObjects(objView, objRec, true);
                        cust = Common.CommonFunction.GetCustomerUnitandZipCode(objRec.id);
                        objView.unit_code = cust.unit_code;
                        objView.CountryId = cust.CountryId;
                        if (cust.zip_code > 0)
                        {
                            objView.zip_code = cust.zip_code;
                        }
                    }
                }
                else
                {
                    objView.isactive = true;
                    objView.CountryId = Common.Constants.DefaultCountry;
                }
                objView.StatusList = Common.CommonFunction.StatusList();
                objView.CountryList = Common.CommonFunction.CountryList();
                objView.GSTRegisterList = Common.CommonFunction.StatusList();
                objView.SourceList = Common.CommonFunction.SourceList();
               
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: _LoadCustomer, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
                cust = null;
            }
        }

        public JsonResult SaveCustomer(CustomerViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            customer objRec = new customer();
            try
            {
                if (objView.id > 0)
                {
                    // Int32 alreadyExist = _CustRepo.FindBy(o => o.email == objView.email && o.id != objView.id).Count();
                    //if (alreadyExist == 0)
                    //{
                    objRec = _CustRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.created_by = objRec.created_by;
                        objView.created_date = objRec.created_date;
                        if (objView.name1 != null)
                        {
                            objView.name1 = Convert.ToString(objView.name1).Trim();
                        }
                        if (objView.name2 != null)
                        {
                            objView.name2 = Convert.ToString(objView.name2).Trim();
                        }
                        if (objView.block_no != null)
                        {
                            objView.block_no = Convert.ToString(objView.block_no).Trim();
                        }
                        if (objView.job_site != null)
                        {
                            objView.job_site = Convert.ToString(objView.job_site).Trim();
                        }
                        if (objView.address != null)
                        {
                            objView.address = Convert.ToString(objView.address).Trim();
                        }
                        if (objView.contact_person != null)
                        {
                            objView.contact_person = Convert.ToString(objView.contact_person).Trim();
                        }
                        CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _CustRepo.Save();
                        CommonFunction.UpsertCustomerinfoUnitandZipCode(objView.id, objRec.zip_code.ToString(), objRec.unit_code, objRec.CountryId);
                       
                    }
                    return Json(new { msg = "Customer record updated successfully.", cls = "success", id = objView.id });
                    //}else
                    //{
                    //    return Json(new { msg = "Customer with e-mail " + objView.email + " already exist.", cls = "error", id = objView.id });
                    //}
                }
                else
                {
                    //Int32 alreadyExist = _CustRepo.FindBy(o => o.email == objView.email).Count();

                    //if (alreadyExist == 0)
                    //{     
                    if (objView.name1 != null)
                    {
                        objView.name1 = Convert.ToString(objView.name1).Trim();
                    }
                    if (objView.name2 != null)
                    {
                        objView.name2 = Convert.ToString(objView.name2).Trim();
                    }
                    if (objView.block_no != null)
                    {
                        objView.block_no = Convert.ToString(objView.block_no).Trim();
                    }
                    if (objView.job_site != null)
                    {
                        objView.job_site = Convert.ToString(objView.job_site).Trim();
                    }
                    if (objView.address != null)
                    {
                        objView.address = Convert.ToString(objView.address).Trim();
                    }
                    if (objView.contact_person != null)
                    {
                        objView.contact_person = Convert.ToString(objView.contact_person).Trim();
                    }
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _CustRepo.Add(objRec);
                    _CustRepo.Save();
                    CommonFunction.UpdateBranchForCustomer(SessionManagement.SelectedBranchID, objRec.id);
                    CommonFunction.UpsertCustomerinfoUnitandZipCode(objRec.id, objRec.zip_code.ToString(), objRec.unit_code, objRec.CountryId);
                   
                    return Json(new { msg = "Customer record created successfully.", cls = "success", id = objRec.id });
                    //}
                    //else
                    //{
                    //    return Json(new { msg = "Customer with e-mail " + objView.email + " already exist.", cls = "error", id =0 });
                    //}
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveCustomer, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        //public void UpsertCustomerUnitandZipCode(long Id)
        //{
        //    //CommonFunction.
        //}

        public ActionResult DeleteById(Int32 Id)
        {
            customer objRec = new customer();
            int count1 = 0;
            try
            {
                count1 = _ProjectRepo.FindBy(o => o.customer_id == Id).Count();
                if (count1 > 0)
                {
                    TempData["Message"] = "Customer can't be deleted.";
                    TempData["cls"] = "error";
                }
                else
                {
                    if (Id > 0)
                    {
                        objRec = _CustRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        _CustRepo.Delete(objRec);
                        _CustRepo.Save();
                        TempData["Message"] = "Customer deleted successfully.";
                        TempData["cls"] = "success";
                    }
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
                objRec = null;
            }
        }
    }
}