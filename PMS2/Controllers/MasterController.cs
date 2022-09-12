using Newtonsoft.Json;
using PMS.Data_Access;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Interface;
using Microsoft.AspNet.Identity;

namespace PMS.Controllers
{
    public class MasterController : Controller
    {
        private IMaster _IMaster;

        public MasterController()
        {
            _IMaster = new DataLayer();
        }

        public ActionResult TasksFlow()
        {
            return View();
        }

        public JsonResult MasterTasksList()
        {
            string message = string.Empty;
            List<TasksList> _TasksList = new List<TasksList>();
            try
            {
                _TasksList = _IMaster.BindTasksList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: MasterTasksList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _TasksList,
                TotalCount = _TasksList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTask(string JsonTask)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            //string message = string.Empty;
            string uid = string.Empty;
            // DataLayer ObjDB = new DataLayer();            
            try
            {
                TasksList _TasksList = JsonConvert.DeserializeObject<TasksList>(JsonTask);
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IMaster.CreateTask(_TasksList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateTask");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteTask(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<TasksList> _TasksList = JsonConvert.DeserializeObject<List<TasksList>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeleteTask(_TasksList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteTask");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UOM()
        {
            return View();
        }

        public JsonResult UOMList()
        {
            string message = string.Empty;
            List<UOM> _UOMList = new List<UOM>();
            try
            {
                _UOMList = _IMaster.UOMList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UOMList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _UOMList,
                TotalCount = _UOMList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateUOM(string JsonValues)
        {
            string uid = string.Empty;
            SuccessMessage SuccessMessage = new SuccessMessage();
            UOM _UOMList = JsonConvert.DeserializeObject<UOM>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                SuccessMessage = _IMaster.CreateUOM(_UOMList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateUOM");
                SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteUOM(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<UOM> _UOMList = JsonConvert.DeserializeObject<List<UOM>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeleteUOM(_UOMList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteUOM");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SourceOfInquiry()
        {
            return View();
        }

        public JsonResult SourceOfInquiryList()
        {
            string message = string.Empty;
            List<SourceOfInquiry> _SourceOfInquiryList = new List<SourceOfInquiry>();
            try
            {
                _SourceOfInquiryList = _IMaster.SourceOfInquiryList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SourceOfInquiryList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _SourceOfInquiryList,
                TotalCount = _SourceOfInquiryList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateSourceOfInquiry(string JsonSourceOfInquiry)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            SourceOfInquiry _SourceOfInquiryList = JsonConvert.DeserializeObject<SourceOfInquiry>(JsonSourceOfInquiry);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreateSourceOfInquiry(_SourceOfInquiryList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateSourceOfInquiry");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                JsonSourceOfInquiry = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteSourceOfInquiryBySourceOfInquiryId(string SourceOfInquiryId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteSourceOfInquiryBySourceOfInquiryId(SourceOfInquiryId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteSourceOfInquiryBySourceOfInquiryId");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                SourceOfInquiryId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Category()
        {
            return View();
        }

        public JsonResult CategoryList()
        {
            string message = string.Empty;
            List<Category> _CategoryList = new List<Category>();
            try
            {
                _CategoryList = _IMaster.CategoryList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CategoryList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _CategoryList,
                TotalCount = _CategoryList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateCategory(string JsonCategory)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            Category _CategoryList = JsonConvert.DeserializeObject<Category>(JsonCategory);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreateCategory(_CategoryList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateCategory");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                JsonCategory = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteCategory(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<Category> _CategoryList = JsonConvert.DeserializeObject<List<Category>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeleteCategory(_CategoryList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteCategory");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Floor()
        {
            return View();
        }

        public JsonResult FloorList()
        {
            string message = string.Empty;
            List<Floor> _FloorList = new List<Floor>();
            try
            {
                _FloorList = _IMaster.FloorList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: FloorList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _FloorList,
                TotalCount = _FloorList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateFloor(string JsonFloor)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = string.Empty;
            Floor _FloorList = JsonConvert.DeserializeObject<Floor>(JsonFloor);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IMaster.CreateFloor(_FloorList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateFloor");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                JsonFloor = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteFloor(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<Floor> _FloorList = JsonConvert.DeserializeObject<List<Floor>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeleteFloor(_FloorList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteFloor");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Plan()
        {
            return View();
        }

        public JsonResult PlanList()
        {
            string message = string.Empty;
            List<Plan> _PlanList = new List<Plan>();
            try
            {
                _PlanList = _IMaster.PlanList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PlanList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _PlanList,
                TotalCount = _PlanList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreatePlan(string JsonPlanValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            Plan _PlanList = JsonConvert.DeserializeObject<Plan>(JsonPlanValues);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreatePlan(_PlanList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreatePlan");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                JsonPlanValues = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeletePlan(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<Plan> _PlanList = JsonConvert.DeserializeObject<List<Plan>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeletePlan(_PlanList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePlan");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PackageType()
        {
            return View();
        }

        public JsonResult PackageTypeList()
        {
            string message = string.Empty;
            List<PackageType> _PackageTypeList = new List<PackageType>();
            try
            {
                _PackageTypeList = _IMaster.PackageTypeList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PackageTypeList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _PackageTypeList,
                TotalCount = _PackageTypeList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreatePackageType(string JsonPackageTypeValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            PackageType _PackageTypeList = JsonConvert.DeserializeObject<PackageType>(JsonPackageTypeValues);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreatePackageType(_PackageTypeList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreatePackageType");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                JsonPackageTypeValues = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeletePackageType(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<PackageType> _PackageTypeList = JsonConvert.DeserializeObject<List<PackageType>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeletePackageType(_PackageTypeList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackageType");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Item()
        {
            return View();
        }

        public JsonResult ItemList()
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<Item> _ItemsList = new List<Item>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    _ItemsList = _IMaster.ItemList(uid);
                }
                else
                {
                    uid = User.Identity.GetUserId();
                    _ItemsList = _IMaster.ItemList(uid);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ItemsList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _ItemsList,
                TotalCount = _ItemsList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateItem(string JsonItems)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            Item _ItemList = JsonConvert.DeserializeObject<Item>(JsonItems);
            string uid = string.Empty;
            // List<Item> _ItemList = JsonConvert.DeserializeObject<List<Item>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IMaster.CreateItem(_ItemList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateItem");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteItem(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<Item> _ItemList = JsonConvert.DeserializeObject<List<Item>>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.DeleteItem(_ItemList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteItem");
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }


        #region
        /// <summary>
        /// ElectricType
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult PropertyType()
        {
            return View();
        }

        public JsonResult PropertyTypeList()
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<PropertyType> _PropertyTypeList = new List<PropertyType>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    _PropertyTypeList = _IMaster.PropertyTypeList(uid);
                }
                else
                {
                    uid = User.Identity.GetUserId();
                    _PropertyTypeList = _IMaster.PropertyTypeList(uid);
                }
                
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: PropertyTypeList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                ElectricType = _PropertyTypeList,
                TotalCount = _PropertyTypeList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CrudPropertyType(string JsonItems,string mode = null)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            PropertyType _PropertyList = JsonConvert.DeserializeObject<PropertyType>(JsonItems);
            string uid = string.Empty;
           
            // List<Item> _ItemList = JsonConvert.DeserializeObject<List<Item>>(obj[0]);
            try
            {
                if (mode == ModeConstants.Insert)
                {
                    uid = User.Identity.GetUserId();
                    _SuccessMessage = _IMaster.CreatePropertyType(_PropertyList, uid, mode);
                }
                else if(mode == ModeConstants.Update)
                {
                    uid = User.Identity.GetUserId();
                    _SuccessMessage = _IMaster.CreatePropertyType(_PropertyList, uid, mode);
                }
                else if(mode == ModeConstants.Delete)
                {
                        uid = User.Identity.GetUserId();
                        _SuccessMessage = _IMaster.CreatePropertyType(_PropertyList, uid, mode);
                   
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateItem");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }


        #endregion

        public JsonResult UOMDropDownList()
        {
            string message = string.Empty;
            List<UOMDropDown> _UOMDropDownList = new List<UOMDropDown>();
            try
            {
                _UOMDropDownList = _IMaster.UOMDropDownList();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _UOMDropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TaskDropDownList()
        {
            string message = string.Empty;
            List<TaskDropDown> _TaskDropDownList = new List<TaskDropDown>();
            try
            {
                _TaskDropDownList = _IMaster.TaskDropDownList();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _TaskDropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PlanDropDownList()
        {
            string message = string.Empty;
            List<PlanDropdown> _PlanDropDownList = new List<PlanDropdown>();
            try
            {
                _PlanDropDownList = _IMaster.PlanDropDownList();
                //// Convert to string
                //string serializedAsString = JsonConvert.SerializeObject(_PlanDropDownList, Formatting.Indented);
                //var descListOb = _PlanDropDownList.OrderBy(x => x.plan_name);
                //// Convert to C# object
                //_PlanDropDownList = JsonConvert.DeserializeObject<List<PlanDropdown>>(serializedAsString);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _PlanDropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PackageDropDownList()
        {
            string message = string.Empty;
            List<PackageDropdown> _PackageDropDownList = new List<PackageDropdown>();
            try
            {
                _PackageDropDownList = _IMaster.PackageDropDownList();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _PackageDropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FloorDropDownList()
        {
            string message = string.Empty;
            List<FloorDropdown> _FloorDropDownList = new List<FloorDropdown>();
            try
            {
                _FloorDropDownList = _IMaster.FloorDropDownList();
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _FloorDropDownList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CategoryDropDownList()
        {
            string message = string.Empty;
            List<CategoryDropDown> _DropDownList = new List<CategoryDropDown>();
            try
            {
                _DropDownList = _IMaster.CategoryDropDownList();
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

        public JsonResult ItemDropDownList(string TaskId)
        {
            string message = string.Empty;
            List<ItemDropDown> _DropDownList = new List<ItemDropDown>();
            try
            {
                _DropDownList = _IMaster.ItemDropDownList(TaskId);
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
        public JsonResult UpsertMasterContractTerms(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            MasterContractTermsCriteria _MasterContractTermsCriteria = JsonConvert.DeserializeObject<MasterContractTermsCriteria>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.UpsertMasterContractTerms(_MasterContractTermsCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertMasterContractTerms");
                message = ex.Message;
            }
            finally
            {
                _MasterContractTermsCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpsertMasterPaymentTerms(FormCollection obj)
        {
            string message = string.Empty;
            string uid = string.Empty;
            MasterPaymentTermsCriteria _MasterPaymentTermsCriteria = JsonConvert.DeserializeObject<MasterPaymentTermsCriteria>(obj[0]);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.UpsertMasterPaymentTerms(_MasterPaymentTermsCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertMasterPaymentTerms");
                message = ex.Message;
            }
            finally
            {
                _MasterPaymentTermsCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMasterTasksList()
        {
            string message = string.Empty;
            List<TasksList> _MasterTasksList = new List<TasksList>();
            try
            {
                _MasterTasksList = _IMaster.GetMasterTasksList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetMasterTasksList");
                message = ex.Message;
            }
            finally
            {
                //ObjDB = null;
            }
            var data = new
            {
                Items = _MasterTasksList,
                TotalCount = _MasterTasksList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTasksbytaskid()
        {
            string message = string.Empty;
            TasksList _GetTasksByTaskId = new TasksList();
            List<TasksList> _MasterTasksList = new List<TasksList>();
            try
            {
                _MasterTasksList = _IMaster.GetTasksbytaskid(_GetTasksByTaskId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTasksbytaskid");
                message = ex.Message;
            }
            finally
            {
                _GetTasksByTaskId = null;
            }
            var data = new
            {
                Items = _MasterTasksList,
                TotalCount = _MasterTasksList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemByItemId(Int32 ItemId)
        {
            string message = string.Empty;
            // Item _GetItemByItemId = new Item();
            List<Item> _MasterItemList = new List<Item>();
            try
            {
                _MasterItemList = _IMaster.GetItemByItemId(ItemId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetItemByItemId");
                message = ex.Message;
            }
            finally
            {
                // _GetItemByItemId = null;
            }
            var data = new
            {
                Items = _MasterItemList,
                TotalCount = _MasterItemList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlansByPlanId()
        {
            string message = string.Empty;
            Plan _GetPlansByPlanId = new Plan();
            List<Plan> _MasterPlansList = new List<Plan>();
            try
            {
                _MasterPlansList = _IMaster.GetPlansByPlanId(_GetPlansByPlanId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPlansByPlanId");
                message = ex.Message;
            }
            finally
            {
                _GetPlansByPlanId = null;
            }
            var data = new
            {
                Items = _MasterPlansList,
                TotalCount = _MasterPlansList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUomByUomId()
        {
            string message = string.Empty;
            UOM _GetUomByUomId = new UOM();
            List<UOM> _MasterUOMList = new List<UOM>();
            try
            {
                _MasterUOMList = _IMaster.GetUomByUomId(_GetUomByUomId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetUomByUomId");
                message = ex.Message;
            }
            finally
            {
                _GetUomByUomId = null;
            }
            var data = new
            {
                Items = _MasterUOMList,
                TotalCount = _MasterUOMList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFloorByFloorId()
        {
            string message = string.Empty;
            Floor _GetFloorByFloorId = new Floor();
            List<Floor> _MasterFloorList = new List<Floor>();
            try
            {
                _MasterFloorList = _IMaster.GetFloorByFloorId(_GetFloorByFloorId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetFloorByFloorId");
                message = ex.Message;
            }
            finally
            {
                _GetFloorByFloorId = null;
            }
            var data = new
            {
                Items = _MasterFloorList,
                TotalCount = _MasterFloorList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPackageTypeByPackageId()
        {
            string message = string.Empty;
            PackageType _GetPackageTypeByPackageId = new PackageType();
            List<PackageType> _MasterPackageTypeList = new List<PackageType>();
            try
            {
                _MasterPackageTypeList = _IMaster.GetPackageTypeByPackageId(_GetPackageTypeByPackageId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTypeByPackageId");
                message = ex.Message;
            }
            finally
            {
                _GetPackageTypeByPackageId = null;
            }
            var data = new
            {
                Items = _MasterPackageTypeList,
                TotalCount = _MasterPackageTypeList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetPackagesByPackageId()
        //{
        //    string message = string.Empty;
        //    Package _GetPackagesByPackageId = new Package();
        //    List<Package> _MasterPackageList = new List<Package>();
        //    try
        //    {
        //        _MasterPackageList = _IMaster.GetPackagesByPackageId(_GetPackagesByPackageId);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: GetPackageTypeByPackageId");
        //        message = ex.Message;
        //    }
        //    finally
        //    {
        //        _GetPackagesByPackageId = null;
        //    }
        //    var data = new
        //    {
        //        Items = _MasterPackageList,
        //        TotalCount = _MasterPackageList.Count
        //    };
        //    return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult DeleteTasksByTaskId(string TaskId)
        {
            string message = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteTasksByTaskId(TaskId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteTasksByTaskId");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteItemByItemId(string ItemId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteItemByItemId(ItemId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteItemByItemId");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                ItemId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCategoryByCategoryId(string CategoryId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteCategoryByCategoryId(CategoryId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteCategoryByCategoryId");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                CategoryId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteUOMByUomId(string Uom_Id)
        {
            string message = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteUOMByUomId(Uom_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteUOMByUomId");
                message = ex.Message;
            }
            finally
            {
                Uom_Id = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePlansByPlanId(string PlanId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeletePlansByPlanId(PlanId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePlansByPlanId");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                PlanId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFloorByFloorId(string FloorId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteFloorByFloorId(FloorId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteFloorByFloorId");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                FloorId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePackageTypeByPackageTypeId(string PackageTypeId)
        {
            string message = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeletePackageTypeByPackageTypeId(PackageTypeId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackageTypeByPackageTypeId");
                message = ex.Message;
            }
            finally
            {
                PackageTypeId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePackagesByPackageTypeId(FormCollection obj)
        {
            string message = string.Empty;
            List<SuccessMessage> _SuccessMessage = new List<SuccessMessage>();
            PackageDetail _DeletePackageById = JsonConvert.DeserializeObject<PackageDetail>(obj[0]);
            try
            {
                _SuccessMessage = _IMaster.DeletePackagesByPackageTypeId(_DeletePackageById);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackagesByPackageTypeId");
                message = ex.Message;
            }
            finally
            {
                _DeletePackageById = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Bind_StatusLookUp(string LookUpColumnId)
        {
            string message = string.Empty;
            List<StatusLookup> __GetStatusLookupById = new List<StatusLookup>();
            try
            {
                __GetStatusLookupById = _IMaster.Bind_StatusLookUp(LookUpColumnId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Bind_StatusLookUp");
                message = ex.Message;
            }
            finally
            {
                LookUpColumnId = null;
            }
            return Json(new { data = __GetStatusLookupById }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindMasterpaymentterms()
        {
            string message = string.Empty;
            List<PaymentDescription> _GetPackagepaymentterms = new List<PaymentDescription>();
            try
            {
                _GetPackagepaymentterms = _IMaster.BindMasterpaymentterms();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindMasterpaymentterms");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _GetPackagepaymentterms }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindPackageMasterPaymentTerms()
        {
            string message = string.Empty;
            List<PaymentDescription> _GetPackagepaymentterms = new List<PaymentDescription>();
            try
            {
                _GetPackagepaymentterms = _IMaster.BindPackageMasterPaymentTerms();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindPackageMasterPaymentTerms");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _GetPackagepaymentterms }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindBanks()
        {
            string message = string.Empty;
            List<Banks> __GetStatusLookupById = new List<Banks>();
            try
            {
                __GetStatusLookupById = _IMaster.BindBanks();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindBanks");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = __GetStatusLookupById }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewFloor(string JsonValues)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            string uid = string.Empty;
            Floor _FloorList = JsonConvert.DeserializeObject<Floor>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IMaster.CreateFloor(_FloorList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateFloor");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _FloorList = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewPackageType(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            PackageType _PackageTypeList = JsonConvert.DeserializeObject<PackageType>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreatePackageType(_PackageTypeList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewPackageType");
            }
            finally
            {
                _PackageTypeList = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewPlan(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            Plan _PlanList = JsonConvert.DeserializeObject<Plan>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreatePlan(_PlanList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewPlan");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _PlanList = null;
            }
            return Json(new { data = _successMessage.Errormessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewCategory(string JsonValues)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            string uid = string.Empty;
            Category _CategoryList = JsonConvert.DeserializeObject<Category>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                _successMessage = _IMaster.CreateCategory(_CategoryList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewCategory");
                _successMessage.Errormessage = ex.Message;
            }
            finally
            {
                _CategoryList = null;
            }
            return Json(new { data = _successMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewUOM(string JsonValues)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            string uid = string.Empty;
            UOM _UOMList = JsonConvert.DeserializeObject<UOM>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                SuccessMessage = _IMaster.CreateUOM(_UOMList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewUOM");
                SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _UOMList = null;
            }
            return Json(new { data = SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewTask(string JsonValues)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            //string message = string.Empty;
            string uid = string.Empty;
            TasksList _TasksList = JsonConvert.DeserializeObject<TasksList>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                _SuccessMessage = _IMaster.CreateTask(_TasksList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewTask");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                _TasksList = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateNewMasterPaymentTerms(string JsonValues)
        {
            string message = string.Empty;
            string uid = string.Empty;
            MasterPaymentTermsCriteria _MasterPaymentTermsCriteria = JsonConvert.DeserializeObject<MasterPaymentTermsCriteria>(JsonValues);
            try
            {
                uid = User.Identity.GetUserId();
                message = _IMaster.UpsertMasterPaymentTerms(_MasterPaymentTermsCriteria, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateNewMasterPaymentTerms");
                message = ex.Message;
            }
            finally
            {
                _MasterPaymentTermsCriteria = null;
            }
            return Json(new { data = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindCustomer()
        {
            string message = string.Empty;
            List<CustomerDropDown> _BindCustomer = new List<CustomerDropDown>();
            try
            {
                _BindCustomer = _IMaster.BindCustomer();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindCustomer");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindCustomer }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSalesmen()
        {
            string message = string.Empty;
            List<SalesmanDropDown> _BindSalesmen = new List<SalesmanDropDown>();
            try
            {
                _BindSalesmen = _IMaster.BindSalesmen();
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

        public JsonResult BindAddressSite(string Project_Id, string Salesmen_Id)
        {
            string message = string.Empty;
            List<AddressDropDown> _BindAddress = new List<AddressDropDown>();
            try
            {
                if (String.IsNullOrEmpty(Project_Id) && String.IsNullOrEmpty(Salesmen_Id))
                {
                    Project_Id = "0";
                    Salesmen_Id = "0";
                }
                _BindAddress = _IMaster.BindAddressSite(Project_Id, Salesmen_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindAddress }, JsonRequestBehavior.AllowGet);
        }//SSP_BindMode_of_Payments
        public JsonResult BindContractTerms()
        {
            List<ContractTerm> BindCT = new List<ContractTerm>();
            try
            {
                BindCT = _IMaster.ContractTermsList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindContractTerms");
            }
            finally
            {
            }
            return Json(new { data = BindCT }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ContractTerms()
        {
            return View();
        }

        public JsonResult ContractTermList()
        {
            List<ContractTerm> CTList = new List<ContractTerm>();
            CTList = _IMaster.ContractTermsList();
            var data = new
            {
                Items = CTList,
                TotalCount = CTList.Count
            };
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateContractTerm(string JsonContractTerms)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            ContractTerm CTList = JsonConvert.DeserializeObject<ContractTerm>(JsonContractTerms);
            try
            {
                CTList.Description = HttpUtility.UrlDecode(CTList.Description, System.Text.Encoding.Default);
                Guid uid = Guid.Parse(User.Identity.GetUserId());
                _SuccessMessage = _IMaster.CreateContractTerm(CTList, uid);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CreateContractTerm");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                CTList = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMasterContractTermById(string ContractTermId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                _SuccessMessage = _IMaster.DeleteMasterContractTermsByID(ContractTermId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteMasterContractTermById");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                ContractTermId = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindMode_of_Payments()
        {
            string message = string.Empty;
            List<ModeOfPaymentDropDown> _ModeOfPaymentDropDown = new List<ModeOfPaymentDropDown>();
            try
            {
                _ModeOfPaymentDropDown = _IMaster.BindMode_of_Payments();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindMode_of_Payments");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _ModeOfPaymentDropDown }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindSupplier()
        {
            List<SupplierDropDown> _SupplierDropDown = new List<SupplierDropDown>();
            try
            {
                if (User.IsInRole("Supplier"))
                {
                    _SupplierDropDown = _IMaster.BindSupplier(User.Identity.GetUserId());
                } 
                else
                {
                    _SupplierDropDown = _IMaster.BindSupplier();
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
            }
            finally
            {
            }
            return Json(new { data = _SupplierDropDown }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindAddressSiteBySupplierId(Int64 Supplier_Id)
        {
            List<AddressDropDown> _AddressSiteDropDown = new List<AddressDropDown>();
            try
            {
                _AddressSiteDropDown = _IMaster.BindAddressSite(Supplier_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
            }
            finally
            {
            }
            return Json(new { data = _AddressSiteDropDown }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSuppliernvoiceNo(Int64 id)
        {
            List<SupplierInvoiceDropDown> _SuppliernvoiceDropDown = new List<SupplierInvoiceDropDown>();
            try
            {
                _SuppliernvoiceDropDown = _IMaster.BindSuppliernvoiceNo(id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
            }
            finally
            {
            }
            return Json(new { data = _SuppliernvoiceDropDown }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ElectricalItems()
        {
            return View();
        }

        public JsonResult ElectricalItemsDropDownList(int PropertyType_Id)
        {
            string message = string.Empty;
            List<ItemDropDown> _DropDownList = new List<ItemDropDown>();
            try
            {
                _DropDownList = _IMaster.ElectricalItemDropDownList(PropertyType_Id);
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

        public JsonResult GetElectricalItemsMappingDetails(int PropertyType_Id)
        {
            string message = string.Empty;
            string uid = string.Empty;
            List<ElectricalItemMapping> _DropDownList = new List<ElectricalItemMapping>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    _DropDownList = _IMaster.GetElectricalItemsMappingDetails(PropertyType_Id, uid);
                }
                else
                {
                    uid = User.Identity.GetUserId();
                    _DropDownList = _IMaster.GetElectricalItemsMappingDetails(PropertyType_Id, uid);
                }
                
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
        public JsonResult CrudElectriItemsType(string JsonItems, string mode = null)
        {

            SuccessMessage _SuccessMessage = new SuccessMessage();
            ElectricalItemMapping _ElectricalItemsList = JsonConvert.DeserializeObject<ElectricalItemMapping>(JsonItems);
            string uid = string.Empty;
            
            try
            {
                uid = User.Identity.GetUserId();
                if (mode == ModeConstants.Insert)
                {
                    _SuccessMessage = _IMaster.CreateElectricalItemType(_ElectricalItemsList, uid, mode);
                }
                else if (mode == ModeConstants.Update)
                {
                    _SuccessMessage = _IMaster.CreateElectricalItemType(_ElectricalItemsList, uid, mode);
                }
                else if (mode == ModeConstants.Delete)
                {
                    _SuccessMessage = _IMaster.CreateElectricalItemType(_ElectricalItemsList, uid, mode);

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CrudElectriItemsType");
                _SuccessMessage.Errormessage = ex.Message;
            }
            finally
            {
                // ObjDB = null;
            }
            return Json(new { data = _SuccessMessage }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSalesmenByLogin(string Uid)
        {
            string message = string.Empty;
            SalesmanDropDown _BindSalesmen = new SalesmanDropDown();
            try
            {
                _BindSalesmen = _IMaster.GetSalesmenIdByUserId(Uid);
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

        public JsonResult BindAddressSiteByStatus(string Project_Id, string Salesmen_Id)
        {
            string message = string.Empty;
            List<AddressDropDown> _BindAddress = new List<AddressDropDown>();
            try
            {
                if (String.IsNullOrEmpty(Project_Id) && String.IsNullOrEmpty(Salesmen_Id))
                {
                    Project_Id = "0";
                    Salesmen_Id = "0";
                }
                _BindAddress = _IMaster.BindAddressSiteByStatus(Project_Id, Salesmen_Id);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSiteByStatus");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindAddress }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindGSTList()
        {
            string message = string.Empty;
            List<SelectListItem> _GStList = new List<SelectListItem>();
            try
            {
                _GStList = Common.CommonFunction.GSTList();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindGSTList");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _GStList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindAddressSiteBySalesman(string ZipCodeId, string UnitCodeId, string SalesmanId)
        {
            string message = string.Empty;
            List<AddressSiteDropDown> bindAddressSite = new List<AddressSiteDropDown>();
            string BranchId = "";
            try
            {
                if (!User.IsInRole("Supplier"))
                {
                    BranchId =Convert.ToString( Common.SessionManagement.SelectedBranchID);
                }
                bindAddressSite = _IMaster.GetAddressSiteList(ZipCodeId, UnitCodeId, BranchId, SalesmanId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSiteByStatus");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = bindAddressSite }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindUnitCode(string ZipCodeId)
        {
            string message = string.Empty;
            List<UnitCodeDropDown> _BindUnitCode = new List<UnitCodeDropDown>();
            try
            {
                _BindUnitCode = _IMaster.BindUnitCode(ZipCodeId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindUnitCode");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindUnitCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSalesmenAndAddressSite(string ZipCodeId, string UnitCodeId,string SalesmanId)
        {
            string message = string.Empty;
            List<SalesmanDropDown> bindSalesmen = new List<SalesmanDropDown>();
            List<AddressSiteDropDown> bindAddressSite = new List<AddressSiteDropDown>();
            string BranchId = "";
            try
            {
                if(!User.IsInRole("Supplier"))
                {
                    BranchId = Convert.ToString(Common.SessionManagement.SelectedBranchID);
                }
                bindSalesmen = _IMaster.GetSalesmenList(ZipCodeId, UnitCodeId, BranchId);
                bindAddressSite = _IMaster.GetAddressSiteList(ZipCodeId, UnitCodeId,BranchId,SalesmanId);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmenAndAddressSite");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = bindSalesmen, addreessSite = bindAddressSite }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindSalesmenByProject(int ProjectId)
        {
            string message = string.Empty;
            List<SalesmanDropDown> bindSalesmen = new List<SalesmanDropDown>();
            string BranchId = "";
            try
            {
                if (!User.IsInRole("Supplier"))
                {
                    BranchId = Convert.ToString(Common.SessionManagement.SelectedBranchID);
                }
                bindSalesmen = _IMaster.GetSalesmenListByProject(ProjectId);
             
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmenAndAddressSite");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = bindSalesmen}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindZipCode()
        {
            string message = string.Empty;
            List<ZipCodeDropDown> _BindUnitCode = new List<ZipCodeDropDown>();
            try
            {
                _BindUnitCode = _IMaster.BindZipCode();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindZipCode");
                message = ex.Message;
            }
            finally
            {
            }
            return Json(new { data = _BindUnitCode }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BindBudgetCostType()
        {
            List<BudgetCostTypeDropDown> _BudgetCostTypeDropDown = new List<BudgetCostTypeDropDown>();
            try
            {
                _BudgetCostTypeDropDown = _IMaster.BindBudgetCostType();
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindBudgetCostType");
            }
            finally
            {
            }
            return Json(new { data = _BudgetCostTypeDropDown }, JsonRequestBehavior.AllowGet);
        }
    }
}
