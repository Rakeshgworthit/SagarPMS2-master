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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using PMS.Interface;
using PMS.Data_Access;
using System.Threading.Tasks;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        private readonly ICompanyRepositor _CompanyRepo;
        private readonly IBanksRepositor _BankRepo;
        private readonly IBranchesRepositor _BranchRepo;
        private readonly ISalesmenRepositor _SalesmenRepo;
        private readonly IProject _ProjectRepo;
        private readonly IProjectsBudgetRepository _ProjectbudgetRepo;
        private readonly IReceiptsRepositor _ReceiptRepo;
        private readonly IPaymentsRepositor _PaymentsRepo;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AdminController(ICompanyRepositor CompanyRepo, IBranchesRepositor BranchRepo, IBanksRepositor bankRepo, ISalesmenRepositor SalesmenRepo, IProject ProjectRepo, IProjectsBudgetRepository ProjectbudgetRepo, IReceiptsRepositor ReceiptRepo, IPaymentsRepositor PaymentsRepo)
        {
            _CompanyRepo = CompanyRepo;
            _BankRepo = bankRepo;
            _BranchRepo = BranchRepo;
            _SalesmenRepo = SalesmenRepo;
            _ProjectRepo = ProjectRepo;
            _ProjectbudgetRepo = ProjectbudgetRepo;
            _ReceiptRepo = ReceiptRepo;
            _PaymentsRepo = PaymentsRepo;
        }
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Company(CompanyViewModel objView)
        {
            return View(objView);
        }

        public ActionResult CompanySaveUpdate(CompanyViewModel objView)
        {
            company objRec = new company();
            try
            {
                string uid = User.Identity.GetUserId();
                if (objView.id > 0)
                {
                    objRec = _CompanyRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                    objView.created_date = objRec.created_date;
                    objView.created_by = objRec.created_by;
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _CompanyRepo.Save();
                    return Json(new { msg = "Company record updated successfully.", cls = "success", id = objView.id });

                }
                else
                {
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _CompanyRepo.Add(objRec);
                    _CompanyRepo.Save();
                    return Json(new { msg = "Company record created successfully.", cls = "success", id = objRec.id });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CompanySaveUpdate, Parameter : CompanyViewModel={objView}");
                return Json(new { msg = "Company record created successfully.", cls = "success", id = objRec.id });
            }
            finally
            {

            }
        }


        public ActionResult CompanyLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            CompanyViewModel objView = new CompanyViewModel();
            try
            {
                if (Id > 0)
                {
                    if (User.IsInRole("SuperAdmin"))
                    {
                        company objRec = _CompanyRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    else
                    {
                        company objRec = _CompanyRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        if (objRec != null)
                        {
                            CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                }
                else
                {
                    objView.isactive = true;
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CompanyLoadAddEdit, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public ActionResult CompanyDeleteById(Int32 Id)
        {
            company objRec = new company();
            try
            {
                if (Id > 0)
                {
                    objRec = _CompanyRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    _CompanyRepo.Delete(objRec);
                    _CompanyRepo.Save();
                }
                return RedirectToAction("Company");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CompanyDeleteById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Branches(BranchViewModel objView)
        {
            try
            {
                objView.CompanyList = Common.CommonFunction.CompanyList();
                objView.CountryList = Common.CommonFunction.CountryList();
                if (TempData["Message"] != null && TempData["cls"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                    ViewBag.cls = TempData["cls"].ToString();
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Branches, Parameter : objView={objView}");
                return null;
            }
            finally
            {

            }
        }
        public ActionResult BranchSaveUpdate(BranchViewModel objView)
        {
            branch objRec = new branch();
            try
            {
                string uid = User.Identity.GetUserId();
                if (objView.id > 0)
                {
                    branch existingBranch = _BranchRepo.FindBy(i => i.id != objView.id && i.branch_name == objView.branch_name).FirstOrDefault();
                    if (existingBranch != null)
                    {
                        return Json(new { msg = "Branch Name already exists.", cls = "error", id = existingBranch.id });
                    }
                    else
                    {
                        objRec = _BranchRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                        objView.created_date = objRec.created_date;
                        objView.created_by = objRec.created_by;
                        Common.CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _BranchRepo.Save();
                        return Json(new { msg = "Branch record updated successfully.", cls = "success", id = objView.id });
                    }
                }
                else
                {
                    branch existingBranch = _BranchRepo.FindBy(i => i.id != objView.id && i.branch_name == objView.branch_name).FirstOrDefault();
                    if (existingBranch != null)
                    {
                        return Json(new { msg = "Branch Name already exists.", cls = "error", id = existingBranch.id });
                    }
                    else
                    {
                        Common.CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.created_date = DateTime.Now;
                        objRec.created_by = new Guid(uid);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _BranchRepo.Add(objRec);
                        _BranchRepo.Save();
                        return Json(new { msg = "Branch record created successfully.", cls = "success", id = objRec.id });
                    }

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchSaveUpdate, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }
        public ActionResult BranchLoadAddEdit(Int32 Id)
        {
            BranchViewModel objView = new BranchViewModel();
            try
            {
                string uid = User.Identity.GetUserId();
                Int32 branchid = SessionManagement.SelectedBranchID;
                objView.CompanyList = Common.CommonFunction.CompanyList();
                objView.CountryList = Common.CommonFunction.CountryList();
                if (Id > 0)
                {
                    if (User.IsInRole("SuperAdmin"))
                    {
                        branch objRec = _BranchRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        if (objRec != null)
                        {
                            CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                    else
                    {
                        branch objRec = _BranchRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                        if (objRec != null)
                        {
                            CommonFunction.MergeObjects(objView, objRec, true);
                        }
                    }
                }
                else
                {
                    objView.isactive = true;
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchLoadAddEdit, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }
        public ActionResult BranchDeleteById(Int32 Id)
        {
            branch objRec = new branch();
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            try
            {
                count1 = _ProjectRepo.FindBy(o => o.branch_id == Id).Count();
                count2 = _ProjectbudgetRepo.FindBy(o => o.branch_id == Id).Count();
                count3 = _ReceiptRepo.FindBy(o => o.branch_id == Id).Count();
                if (count1 > 0 || count2 > 0 || count3 > 0)
                {
                    TempData["Message"] = "Branch can't be deleted.";
                    TempData["cls"] = "error";
                }
                else
                {
                    if (Id > 0)
                    {
                        objRec = _BranchRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        _BranchRepo.Delete(objRec);
                        _BranchRepo.Save();
                        //Session["Message"] = "Branch record deleted successfully.";
                        //ViewData["message"] = "Branch record deleted successfully.";
                        TempData["Message"] = "Branch deleted successfully.";
                        TempData["cls"] = "success";
                    }
                }
                return RedirectToAction("Branches");

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchDeleteById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public ActionResult Salesmen(SalesmenViewModel objView)
        {
            try
            {
                //objView.BranchList = Common.CommonFunction.BranchList();
                objView.branch_Id = SessionManagement.SelectedBranchID;
                if(objView.SalesmenSearch ==null)
                {
                    objView.SalesmenSearch = "";
                }
                if (TempData["Message"] != null && TempData["cls"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                    ViewBag.cls = TempData["cls"].ToString();
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Salesmen, Parameter : objView={objView}");
                return null;
            }
            finally
            {

            }
        }

        public ActionResult SalesmenSaveUpdate(SalesmenViewModel objView)
        {
            salesman objRec = new salesman();
            SuccessMessage successMessage = new SuccessMessage();
            string cls = "";
            try
            {
                string uid = User.Identity.GetUserId();
                if (objView.id > 0)
                {
                    objRec = _SalesmenRepo.FindBy(i => i.id == objView.id).SingleOrDefault();
                    objView.created_date = objRec.created_date;
                    objView.created_by = objRec.created_by;
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _SalesmenRepo.Save();
                    InsertUpdateICNumber(objView.id, objView.ICNumber, objView.User_Id);
                    if(!objView.IsCreateLogin)
                    {
                        if(!CheckIfSalesmenCredentialsExists(objRec.salesmen_name))
                        {
                            SaveUserCredentials(uid, objView);
                            //SendEmail(objView.email);
                        }

                    }
                    return Json(new { msg = "Salesman record updated successfully.", cls = "success", id = objView.id });
                }
                else
                {
                    Common.CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    objRec.ICNumber = objView.ICNumber;
                    List<salesman> IsEmailIDExistsLst = _SalesmenRepo.FindBy(x => x.email == objRec.email && x.isactive == true).ToList();
                    if (IsEmailIDExistsLst.Count > 0)
                    {
                        return Json(new { msg = "Salesman record creation error. Email ID exists", cls = "error", id = objRec.id });
                    }
                    else
                    {                        
                        if (objView.IsCreateLogin)
                        {

                            successMessage = CheckUser(objView.UserName, objView.Password, objView.email);
                            if (successMessage.Result == "1")
                            {
                                SaveUserCredentials(uid, objView);
                                SendEmail(objView.email, "Login Created for ", objView);
                                objRec.User_id = objView.User_Id.ToString();
                                _SalesmenRepo.Add(objRec);
                                _SalesmenRepo.Save();
                                GetUserIdfromSalesmenId(objView);
                                InsertUpdateICNumber(objRec.id, objView.ICNumber, objView.User_Id);
                                successMessage.Errormessage = "Salesman Created Successfully";
                                cls = "success";
                            }
                            else
                            {
                                cls = "error";
                            }

                        }
                        else
                        {
                            _SalesmenRepo.Add(objRec);
                            _SalesmenRepo.Save();
                            InsertUpdateICNumber(objRec.id, objView.ICNumber, objView.User_Id);
                            successMessage.Errormessage = "Salesman Created Successfully";
                            cls = "success";
                        }                     

                        return Json(new { msg = successMessage.Errormessage, cls = cls, id = objRec.id });

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenSaveUpdate, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public  async void SaveUserCredentials(string userId, SalesmenViewModel objUserCredentials)
        {
            string uid = User.Identity.GetUserId();

            try
            {
                var user = new ApplicationUser { UserName = objUserCredentials.UserName, Email = objUserCredentials.email };
                var result = await UserManager.CreateAsync(user, objUserCredentials.Password);
                if (result.Succeeded)
                {
                    using (Database.PMSEntities obj = new Database.PMSEntities())
                    {
                        Database.user_detail ud = new Database.user_detail()
                        {
                            user_id = new Guid(user.Id),
                            Name = objUserCredentials.salesmen_name,
                            mobile = objUserCredentials.mobile,
                            phone = objUserCredentials.phone,
                            address1 = objUserCredentials.address1,
                            address2 = objUserCredentials.address2,
                            address3 = objUserCredentials.address3,
                            address4 = objUserCredentials.address4,
                            //country_id = objUserCredentials.country_id,
                            website = objUserCredentials.website,
                            zip_code = objUserCredentials.zip_code,
                            is_active = objUserCredentials.isactive,
                            //ICNumber = objUserCredentials.ICNumber,
                            created_by = new Guid(uid),
                            created_date = DateTime.Now,
                            modified_by = new Guid(uid),
                            modified_date = DateTime.Now                           
                        };
                        obj.user_detail.Add(ud);
                        obj.SaveChanges();
                    }                   
                    UserManager.AddToRole(user.Id, "Salesman");
                    //Assign branch
                    using (Database.PMSEntities objDB = new Database.PMSEntities())
                    {
                        Int32 totalBranch = objDB.user_access_rights.Where(o => o.branch_id == objUserCredentials.branch_Id && o.user_id == new Guid(user.Id)).Count();
                        if (totalBranch == 0)
                        {
                            Database.user_access_rights objRec = new Database.user_access_rights
                            {
                                user_id = new Guid(user.Id),
                                branch_id = objUserCredentials.branch_Id,
                                created_by = new Guid(uid),
                                created_date = DateTime.Now,
                                modified_by = new Guid(uid),
                                modified_date = DateTime.Now,
                                isactive = true
                            };
                            objDB.user_access_rights.Add(objRec);
                            objDB.SaveChanges();
                            //return Json(new { msg = "Branch assign successfully.", cls = "success" });
                        }
                        else
                        {
                            //return Json(new { msg = "Branch already assigned.", cls = "error" });
                        }
                    }
                    objUserCredentials.User_Id = new Guid(user.Id);
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveUserCredentials, Parameter : objUserCredentials={objUserCredentials}");
            }
        }
        
        public SuccessMessage CheckUser(string UserName,string Password, string EmailId)
        {
            int iUserCount = 0;
            int iEmailCount = 0;
            SuccessMessage successMessage = new SuccessMessage();
            try
            {
                string SqlUser = "Select Count(*) from AspNetUsers Asp Left Join user_detail Ud on Asp.Id = Ud.User_Id  Where Ud.Is_active =1 and Asp.UserName  ='" + UserName + "'";
                string SqlUserEmail = "Select Count(*) from AspNetUsers Asp Left Join user_detail Ud on Asp.Id = Ud.User_Id  Where Ud.Is_active =1 and Email  ='" + EmailId + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                iUserCount = (int)cmd.ExecuteScalar(SqlUser);
                if (iUserCount == 0)
                {
                    iEmailCount = (int)cmd.ExecuteScalar(SqlUserEmail);                    
                    if (iEmailCount == 0)
                    {
                        successMessage.Result = "1";
                        successMessage.Errormessage = "User Created SucessFully";
                    }
                    else
                    {
                        successMessage.Result = "0";
                        successMessage.Errormessage = "User With the Same Email Already Exist. Cannot Create the User and Salesman ";
                    }
                }
                else
                {
                    successMessage.Result = "0";
                    successMessage.Errormessage = "User With the Same Name Already Exist.Cannot Create the User and Salesman ";

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckUser, Parameter : UserName ={UserName}");
            }
            return successMessage;
        }


        public void InsertUpdateICNumber(long id, string ICNum, Guid UserId)
        {
            try
            {
                string qry = "update salesmen SET ICNumber = '" + ICNum + "'" + ", USER_id = '" + UserId + "'" + " where id = " + "'" + id + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                cmd.ExecuteNonQuery(qry);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetUserIdfromSalesmenId, Parameter : UserId={UserId}");               
            }
        }

        public bool CheckIfSalesmenCredentialsExists(string salesMenName)
        {
            try
            {
                string qry = @"select UserName, PasswordHash from dbo.salesmen slsmen join dbo.user_detail usrdtl on slsmen.salesmen_name = usrdtl.Name 
                        join AspNetUsers usrs on usrdtl.user_id = usrs.Id
                        where slsmen.salesmen_name = " + "'" + salesMenName + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                DataSet dsGetUserlogindtls = cmd.ExecuteDataSet(qry);
                if (dsGetUserlogindtls.Tables.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckIfSalesmenCredentialsExists, Parameter : salesMenName={salesMenName}");
                return false;
            }
        }

        public bool CheckIfSalesmenInUserdetail(int Id)
        {
            try
            {
                int rtnSalesmenID = 0;
                string qry = "select id from user_detail where salesmen_id = " + Id + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                rtnSalesmenID = (int)cmd.ExecuteScalar(qry);
                if (rtnSalesmenID == 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CheckIfSalesmenInUserdetail, Parameter : Id={Id}");
                return false;
            }
        }
        public void GetUserIdfromSalesmenId(SalesmenViewModel objview)
        {
            try
            {
                int rtnSalesmenID = 0;
                string qry = @"select Isnull(UserName,'') UserName,Isnull( PasswordHash,'')PasswordHash , Isnull(usrs.Id,NewID()) UserId from AspNetUsers usrs where usrs.UserName = " + "'" + objview.UserName + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                System.Data.DataSet dsGetICNumUserlogindtls = cmd.ExecuteDataSet(qry);
                if (dsGetICNumUserlogindtls.Tables[0].Rows.Count > 0)
                {
                    objview.UserName = dsGetICNumUserlogindtls.Tables[0].Rows[0][0].ToString();
                    objview.Password = dsGetICNumUserlogindtls.Tables[0].Rows[0][1].ToString();
                    objview.ConfirmPassword = dsGetICNumUserlogindtls.Tables[0].Rows[0][1].ToString();
                    objview.User_Id = new Guid(dsGetICNumUserlogindtls.Tables[0].Rows[0][2].ToString());
                    objview.IsCreateLogin = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetUserIdfromSalesmenId, Parameter : objUserCredentials={objview}");
            }
        }

        //public void SendEmail(string emailID)
        //{
        //    EMailInfo emaildtls = new EMailInfo();
        //    emaildtls.ToMail = emailID;
        //    //emaildtls.FromMail = "";
        //    emaildtls.Subject = "TestEmail from PMS2 - Login Sucessfully created";
        //    emaildtls.Body = "Congratulations, Your Login credentials have been sucessfully created";
        //    //emaildtls.DisplayName = "Administrator";
        //    Mail.SendMail(emaildtls);
        //}

        public bool VerifyIFINCNumberExists(string ICNum)
        {
            try
            {
                string qry = "select * from user_detail" + " where ICNumber = " + "'" + ICNum + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                System.Data.DataSet dsICNumber = cmd.ExecuteDataSet(qry);
                if (dsICNumber.Tables[0].Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: VerifyIFINCNumberExists, Parameter : ICNum={ICNum}");
                return false;

            }
        }

        public void UpdateUserDetailTable(SalesmenViewModel objUserCredentials)
        {
            try
            {
                string uid = User.Identity.GetUserId();
                if (objUserCredentials.id > 0)
                {
                    string qry = "update user_detail SET Name = '" + objUserCredentials.salesmen_name + "'," +
                    "phone = '" + objUserCredentials.phone + "'," + "address1 = '" + objUserCredentials.address1 + "'," +
                    "address2 = '" + objUserCredentials.address2 + "'," + "address3 = '" + objUserCredentials.address3 + "'," +
                    "address4 = '" + objUserCredentials.address4 + "'," + "website = '" + objUserCredentials.website + "'," +
                    "zip_code = '" + objUserCredentials.zip_code + "'" + //"created_by = '" + new Guid(uid) + "'," +
                                                                         //"created_date = '" + DateTime.Now + "'," + "modified_by = '" + new Guid(uid) + "'," + "modified_date = '" + DateTime.Now + "'" +
                    " where Name = " + "'" + objUserCredentials.salesmen_name + "'";
                    PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                    System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                    PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                    cmd.ExecuteNonQuery(qry);
                }
                else
                {
                    var user = new ApplicationUser { UserName = objUserCredentials.UserName, Email = objUserCredentials.email };
                    using (Database.PMSEntities obj = new Database.PMSEntities())
                    {
                        Database.user_detail ud = new Database.user_detail()
                        {
                            user_id = new Guid(user.Id),
                            Name = objUserCredentials.salesmen_name,
                            mobile = objUserCredentials.mobile,
                            phone = objUserCredentials.phone,
                            address1 = objUserCredentials.address1,
                            address2 = objUserCredentials.address2,
                            address3 = objUserCredentials.address3,
                            address4 = objUserCredentials.address4,
                            //country_id = objUserCredentials.country_id,
                            website = objUserCredentials.website,
                            zip_code = objUserCredentials.zip_code,
                            is_active = objUserCredentials.isactive,
                            //ICNumber = objUserCredentials.ICNumber,
                            created_by = new Guid(uid),
                            created_date = DateTime.Now,
                            modified_by = new Guid(uid),
                            modified_date = DateTime.Now
                        };
                        obj.user_detail.Add(ud);
                        obj.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdateUserDetailTable, Parameter : objUserCredentials={objUserCredentials}");                

            }

        }

        public void FillICNumberandUserCredentials(string usrID, SalesmenViewModel objview)
        {
            try
            {
                string qry = @"select Isnull(UserName,'') UserName,Isnull( PasswordHash,'')PasswordHash , Isnull(slsmen.ICNumber,'') ICNumber from dbo.salesmen slsmen Left join dbo.user_detail usrdtl on slsmen.salesmen_name = usrdtl.Name 
                        Left join AspNetUsers usrs on usrdtl.user_id = usrs.Id
                        where slsmen.salesmen_name = " + "'" + objview.salesmen_name + "'";
                PMS.Data_Access.SqlFunctions conn = new Data_Access.SqlFunctions();
                System.Data.SqlClient.SqlConnection sqlconn = conn.GetConnection();
                PMS.Data_Access.DBSqlCommand cmd = new Data_Access.DBSqlCommand(System.Data.CommandType.Text);
                System.Data.DataSet dsGetICNumUserlogindtls = cmd.ExecuteDataSet(qry);
                if (dsGetICNumUserlogindtls.Tables[0].Rows.Count > 0)
                {
                    objview.UserName = dsGetICNumUserlogindtls.Tables[0].Rows[0][0].ToString();
                    objview.Password = dsGetICNumUserlogindtls.Tables[0].Rows[0][1].ToString();
                    objview.ConfirmPassword = dsGetICNumUserlogindtls.Tables[0].Rows[0][1].ToString();
                    objview.ICNumber = dsGetICNumUserlogindtls.Tables[0].Rows[0][2].ToString();
                    objview.IsCreateLogin = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: FillICNumberandUserCredentials, Parameter : objview={objview}");

            }

        }

        public ActionResult SalesmenLoadAddEdit(Int32 Id)
        {
            string uid = User.Identity.GetUserId();
            Int32 branchid = SessionManagement.SelectedBranchID;
            SalesmenViewModel objView = new SalesmenViewModel();
            salesman objRec = new salesman();
            try
            {
                objView.BranchList = Common.CommonFunction.BranchList();
                if (Id > 0)
                {
                    objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                        FillICNumberandUserCredentials(uid, objView);
                    }

                    //if (User.IsInRole("SuperAdmin"))
                    //{
                    //    salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    //    if (objRec != null)
                    //    {
                    //        CommonFunction.MergeObjects(objView, objRec, true);
                    //    }
                    //}
                    //else
                    //{
                    //    salesman objRec = _SalesmenRepo.FindBy(o => o.id == Id && o.created_by == new Guid(uid)).SingleOrDefault();
                    //    if (objRec != null)
                    //    {
                    //        CommonFunction.MergeObjects(objView, objRec, true);
                    //    }
                    //}               

                }
                else
                {
                    objView.saleman_commission = 50;
                    objView.isactive = true;
                    objView.branch_Id = SessionManagement.SelectedBranchID;
                }

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenLoadAddEdit, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
                objRec = null;
            }
        }
        public ActionResult SalesmenDeleteById(Int32 Id)
        {
            salesman objRec = new salesman();
            int count1 = 0;
            int count2 = 0;
            try
            {
                count1 = _ProjectRepo.FindBy(o => o.salesmen_id == Id).Count();
                count2 = _ReceiptRepo.FindBy(o => o.sales_man_id == Id).Count();
                if (count1 > 0 || count2 > 0)
                {
                    TempData["Message"] = "Salesmen can't be deleted.";
                    TempData["cls"] = "error";
                }
                else
                {
                    if (Id > 0)
                    {
                        objRec = _SalesmenRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        //  _SalesmenRepo.Delete(objRec);
                        objRec.isactive = false;
                        _SalesmenRepo.Save();
                        UpdateUserDetailStatus(Id);
                        TempData["Message"] = "Salesmen deleted successfully.";
                        TempData["cls"] = "success";
                    }
                }
                return RedirectToAction("Salesmen");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenDeleteById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public void UpdateUserDetailStatus(Int32 Id)
        {
            try
            {
                CommonFunction.UpdateUserDetailStatus(Id);
            }
            catch(Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdateUserDetailStatus, Parameter : Id={Id}");               
            }           
        }


        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Banks()
        {
            try
            {
                if (TempData["Message"] != null && TempData["cls"] != null)
                {
                    ViewBag.Message = TempData["Message"].ToString();
                    ViewBag.cls = TempData["cls"].ToString();
                }
                return View();
            }
            catch(Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: Banks");
                return null;
            }
        }


        public ActionResult LoadAddEditBank(Int32? Id = 0)
        {
            BankViewModel objView = new BankViewModel();
            bank objRec = new bank();
            try
            {

                if (Id > 0)
                {
                    objRec = _BankRepo.FindBy(o => o.id == Id).SingleOrDefault();
                    if (objRec != null)
                    {
                        CommonFunction.MergeObjects(objView, objRec, true);
                    }
                }
                else
                {
                    objView.isactive = true;
                }
                List<SelectListItem> branchList = CommonFunction.BranchList();
                branchList.Insert(0, new SelectListItem { Value = "0", Text = "Please Select" });
                objView.BranchList = branchList;
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoadAddEditBank, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objView = null;
                objRec = null;
            }
        }

        public JsonResult SaveBank(BankViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            bank objRec = new bank();
            try
            {
                if (objView.id > 0)
                {
                    objRec = _BankRepo.FindBy(o => o.id == objView.id).SingleOrDefault();
                    if (objRec != null)
                    {
                        objView.created_by = objRec.created_by;
                        objView.created_date = objRec.created_date;
                        CommonFunction.MergeObjects(objRec, objView, true);
                        objRec.modified_date = DateTime.Now;
                        objRec.modified_by = new Guid(uid);
                        _BankRepo.Save();
                    }
                    return Json(new { msg = "Bank updated successfully.", cls = "success" });
                }
                else
                {
                    CommonFunction.MergeObjects(objRec, objView, true);
                    objRec.created_date = DateTime.Now;
                    objRec.created_by = new Guid(uid);
                    objRec.modified_date = DateTime.Now;
                    objRec.modified_by = new Guid(uid);
                    _BankRepo.Add(objRec);
                    _BankRepo.Save();
                    return Json(new { msg = "Bank created successfully.", cls = "success" });
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaveBank, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public ActionResult DeleteBankById(Int32 Id)
        {
            bank objRec = new bank();
            int count1 = 0;
            try
            {
                count1 = _PaymentsRepo.FindBy(o => o.bank_id == Id).Count();
                if (count1 > 0)
                {
                    TempData["Message"] = "Bank can't be deleted.";
                    TempData["cls"] = "error";
                }
                else
                {
                    if (Id > 0)
                    {
                        objRec = _BankRepo.FindBy(o => o.id == Id).SingleOrDefault();
                        _BankRepo.Delete(objRec);
                        _BankRepo.Save();
                        TempData["Message"] = "Bank deleted successfully.";
                        TempData["cls"] = "success";
                    }
                }
                return RedirectToAction("Banks");
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DeleteBankById, Parameter : Id={Id}");
                return null;
            }
            finally
            {
                objRec = null;
            }
        }

        public JsonResult UpdatesalesMenUserMap(string slsId, string usrId)
        {
            try
            {
                //string userID = CommonFunction.SalesmenNameUserID(Convert.ToInt32(slsId));
                int rtnSuccess = CommonFunction.UpdateAspNetUserRole(Convert.ToInt64(usrId), Convert.ToInt64(slsId));
                
                //UserManager.RemoveFromRole(Id, "SuperAdmin");
                //UserManager.RemoveFromRole(userId, "User");
                ////UserManager.RemoveFromRole(Id, "Salesman");
                //UserManager.AddToRole(new Guid(userID), "Salesman");
                //if (roleValue)
                //{               
                //    UserManager.AddToRole(Id, roleId);
                //} else
                //{
                //    UserManager.RemoveFromRole(Id, roleId);
                //}            

                return Json(new { msg = "Record updated successfully.", cls = "success" });
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdateRole, Parameter : Id={slsId},roleId={usrId}");
                return null;
            }
            finally
            {

            }
        }

        public void SendEmail(string emailID, string subject, SalesmenViewModel model)
        {
            try
            {
                EMailInfo emaildtls = new EMailInfo();
                emaildtls.ToMail = emailID;
                emaildtls.CCMail = "ismsqateam@gmail.com";
                //emaildtls.ToMail = "ismsqateam@gmail.com";
                emaildtls.Subject = subject + model.salesmen_name;
                emaildtls.Body = PrepareEmailBody(model);
                emaildtls.DisplayName = "Administrator";
                emaildtls.AttachmentPath = "";
                emaildtls.FileName = "";
                Mail.SendMail(emaildtls);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SendEmail, Parameter : model={model}");
            }
        }

        public string PrepareEmailBody(SalesmenViewModel model)
        {
            try
            {
                return "<table style='margin-right: calc(15%); width: 85%;'><tbody>"
                    + "<tr> <td style='width: 25%;'>Date</td><td style='width: 75%;'>: " + DateTime.Now.ToString("d") + "</td></tr>"
                    + "<tr> <td style='width: 25%;'>To</td><td style='width: 75%;'>: " + model.salesmen_name + "</td></tr>"
                    + "<tr> <td style='width: 25%;'>RE</td><td style='width: 75%;'>: LOGIN CREDENTIALS CREATED FOR " + model.salesmen_name + ".</td></tr></tbody></table>"
                    + "<br><div>A login had been created with below credentials:</div><div>"
                    + "<table style='margin-right: calc(15%); width: 85%;'><tbody>"
                    + "<tr> <td style='width: 25%;'>USER NAME</td><td style='width: 75%;'>: " + model.UserName + "</td></tr>"
                    + "<tr> <td style='width: 25%;'>Password</td><td style='width: 75%;'>: " + model.Password + "</td></tr></tbody></table>"
                    + "<br><div>Login using the below link:</div><div>"
                    + "<a href='http://118.201.113.216:38080/Account/Login'>http://118.201.113.216:38080/Account/Login</a></div>"
                    + "<br><div>Thank you</div><br>"
                    + "<div>REGARDS,</div><div> Design 4 Space </div>";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PrepareEmailBody, Parameter : model={model}");
                return null;
            }
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
            try
            {
                fileNameWithExt = List.SalesmenFileName;
                List.SuperId = User.Identity.GetUserId();
                var SalesmenImage_PATH = "/Signature/" + "/Salesmen/" + fileNameWithExt;
                dirPath = PMS.Common.Constants.PhysicalPath + "/Signature/" + "/Salesmen/";

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
                        List.ID_TYPE = "11";
                        List.SalesmenImage_PATH = SalesmenImage_PATH;
                        List.FILE_TYPE = "jpg";
                        _SuccessMessage = UpsertSignature(List, uid);
                    }
                }
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

        [HttpPost]
        public JsonResult GetSalesmenSignaturePath(int salesmenId)
        {
            try
            {
                string docpath = CommonFunction.GetSalesmenSignaturePath(salesmenId);
                return Json(new { msg = "Record updated successfully.", result = docpath.ToString(), cls = "success" });
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UpdatesalesMenUserMap, Parameter : salesmenId={salesmenId}");
                return null;
            }
            finally
            {

            }
        }

        [HttpPost]
        public JsonResult DeleteSalesmenSignature(int salesmenId)
        {
            try
            {
                bool result = CommonFunction.DeleteSalesmenSignature(salesmenId);
                if (result)
                    return Json(new { msg = "Salesmen signature deleted successfully.", result = true, cls = "success" });
                else
                    return Json(new { msg = "Salesmen signature deletion failed.", result = false, cls = "error" });
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DeleteSalesmenSignature, Parameter : salesmenId={salesmenId}");
                return null;
            }
            finally
            {

            }
        }


        public SuccessMessage UpsertSignature(Signature _Signature, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_Signature.DOCUMENT_NAME, CommanConstans.DOCUMENT_NAME, SqlDbType.VarChar);
                    cmd.AddParameters(0, CommanConstans.ACTIVITY_TYPE, SqlDbType.Int);
                    cmd.AddParameters(0, CommanConstans.SUB_ACTIVITY_TYPE, SqlDbType.Int);
                    cmd.AddParameters("", CommanConstans.REMARKS, SqlDbType.VarChar);

                    cmd.AddParameters(0, CommanConstans.ACTIVE_FLAG, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.CREATED_BY, SqlDbType.Int);
                    cmd.AddParameters(_Signature.SuperId, CommanConstans.SUPER_ID, SqlDbType.VarChar);
                    cmd.AddParameters(_Signature.ID, CommanConstans.ID, SqlDbType.Int);
                    cmd.AddParameters(_Signature.ID_TYPE, CommanConstans.ID_TYPE, SqlDbType.Int);
                    if(_Signature.SalesmenId > 0)
                        cmd.AddParameters(_Signature.SalesmenId, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);
                    else
                        cmd.AddParameters(0, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(0, CommanConstans.SUBSUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(_Signature.FILE_TYPE, CommanConstans.FILE_TYPE, SqlDbType.VarChar);

                    //cmd.AddParameters(_Signature.CustomerImage_PATH, CommanConstans.CUSTOMER_DOCUMENT_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(_Signature.SalesmenImage_PATH, CommanConstans.SalesmenImage_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(0, CommanConstans.DOCUMENT_CONTENT_TYPE_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.DOC_CONFIG_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.COMPANY_ID, SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.UpsertSignature);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertSignature, Parameters =" + _Signature.ToString());
                    throw ex;
                }
                finally
                {
                    _Signature = null;
                }
            }
        }
    }
}