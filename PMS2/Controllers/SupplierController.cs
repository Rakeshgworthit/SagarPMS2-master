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
    public class SupplierController : Controller
    {
        private readonly ISupplierRepositor _SuppRepo;
        private readonly IPaymentsRepositor _PaymentsRepo;
        public SupplierController(ISupplierRepositor suppRepo, IPaymentsRepositor PaymentsRepo)
        {
            _SuppRepo = suppRepo;
            _PaymentsRepo = PaymentsRepo;
        }

        // Supplier Listing
        public ActionResult Index(SupplierViewModel objView)
        {
            if (objView.Suppliersearch == null)
            {
                objView.Suppliersearch = "";
            }
            else
            {
                objView.Suppliersearch = objView.Suppliersearch;
            }
            if (TempData["Message"] != null && TempData["cls"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
                ViewBag.cls = TempData["cls"].ToString();
            }
            return View(objView);
        }

        // Open Supplier Add/Edit
        public ActionResult LoadAddEdit(Int32? Id = 0)
        {

            SupplierViewModel objView = new SupplierViewModel();
            try
            {
                if (Id > 0)
                {
                    supplier objRec = _SuppRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        Common.CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    objView.isactive = true;
                }
                objView.StatusList = Common.CommonFunction.StatusList();

                objView.GSTRegisteredList = Common.CommonFunction.GSTRegistered();
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

        public ActionResult SaveUpdate(SupplierViewModel objView)
        {
            try
            {
                string uid = User.Identity.GetUserId();
                if (objView.id > 0)
                {
                    supplier objRec = _SuppRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                    objView.created_date = objRec.created_date;
                    objView.created_by = objRec.created_by;
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _SuppRepo.Save();
                    return Json(new { msg = "Supplier record updated successfully.", cls = "success", id = objView.id });

                }
                else
                {
                    supplier objRec = new supplier();
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _SuppRepo.Add(objRec);
                    _SuppRepo.Save();
                    return Json(new { msg = "Supplier record created successfully.", cls = "success", id = objRec.id });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUpdate, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }

        public ActionResult DeleteById(Int32 Id)
        {
            int count1 = 0;
            try
            {
                count1 = _PaymentsRepo.FindBy(o => o.supplier_id == Id).Count();
                if (count1 > 0)
                {
                    TempData["Message"] = "Supplier can't be deleted.";
                    TempData["cls"] = "error";
                }
                else
                {
                    if (Id > 0)
                    {
                        supplier objRec = _SuppRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        _SuppRepo.Delete(objRec);
                        _SuppRepo.Save();
                        TempData["Message"] = "Supplier deleted successfully.";
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
            }
        }
    }
}