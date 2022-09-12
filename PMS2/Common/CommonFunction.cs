using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;
using PMS.Database;
using PMS.Data_Access;
using PMS.Models;
using System.Data;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

namespace PMS.Common
{
    public static class CommonFunction
    {
        public static string ConvertAmountoDecimal(decimal? amount)
        {
            return String.Format("{0:n}", Convert.ToDecimal(amount));
        }

        public static string GetCustomerData(Int32 customerid)
        {
            string ZipCode = "";
            try
            {
                //using (PMSEntities objDB = new PMSEntities())
                //{
                //customer objCust = objDB.customers.Where(o => o.id == customerid).SingleOrDefault();
                customer objCust = CommonFunction.GetCustomerUnitandZipCode(customerid);
                string SCustomerDate = "";
                if (objCust != null)
                {
                    ZipCode = objCust.zip_code.ToString();
                    if (ZipCode == null || ZipCode == "0")
                    {
                        ZipCode = "";
                    }
                    if (objCust.block_no != null && objCust.block_no != "")
                        SCustomerDate += objCust.block_no;
                    if (objCust.job_site != null && objCust.job_site != "")
                        SCustomerDate += ", " + objCust.job_site;
                    if (objCust.unit_code != null && objCust.unit_code != "")
                        SCustomerDate += ",  " + objCust.unit_code;
                    if (objCust.CountryName != null && objCust.CountryName != "")
                        SCustomerDate += ",  "+objCust.CountryName ;
                    if (ZipCode != "")
                        SCustomerDate += " - " + objCust.zip_code;
                    return SCustomerDate;
                }

                //}
                return "";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetCustomerData, Parameter : customerid={customerid}");
                return null;
            }
            finally
            {

            }

        }

        public static string GetuserEmail(string sUserId)
        {
            string userEmail = "";
            string SqlWhere = "";
            try
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = " Select Email from AspNetUsers Where id = '" + sUserId + "'";
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        userEmail = Ireader.GetString("Email");
                    }
                }
                return userEmail;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchList");
                return userEmail;
            }
            finally
            {
                userEmail = null;
            }
        }
        public static string GetVoType(string Id)
        {
            string VoType = "";
            try
            {
                int Type = Int32.Parse(string.Concat(Id.TakeWhile((c) => c != '-')));
                if (Type == Constants.VoTypeList.Addition)
                {
                    VoType = "Addition";
                }
                else if (Type == Constants.VoTypeList.Omission)
                {
                    VoType = "Omission";
                }
                else if (Type == Constants.VoTypeList.Discount)
                {
                    VoType = "Discount";
                }
                else if (Type == Constants.VoTypeList.Electrical)
                {
                    VoType = "Electrical";
                }
                else
                {
                    VoType = "";
                }
                return VoType;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetCustomerData, Parameter : customerid={Id}");
                return null;
            }
            finally
            {

            }

        }
        public static string GetCustomerName(Int32 customerid)
        {
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    customer objCust = objDB.customers.Where(o => o.id == customerid).SingleOrDefault();
                    if (objCust != null)
                    {
                        return Convert.ToString(objCust.name1 + " " + objCust.job_site);
                    }

                }
                return "";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetCustomerData, Parameter : customerid={customerid}");
                return null;
            }
            finally
            {

            }

        }

        public static decimal GetSalemanCommision(Int32 salemanid)
        {
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    salesman objCust = objDB.salesmen.Where(o => o.id == salemanid).SingleOrDefault();
                    if (objCust != null)
                    {
                        return Convert.ToDecimal(objCust.saleman_commission);
                    }

                }
                return 0;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalemanCommision, Parameter : salemanid={salemanid}");
                return 0;
            }
            finally
            {

            }

        }


        public static List<SelectListItem> CountryList()
        {
            List<SelectListItem> countryList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.countries.ToList();
                    foreach (var items in sList)
                    {
                        countryList.Add(new SelectListItem { Text = items.name, Value = items.id.ToString() });
                    }
                }

                countryList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                return countryList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: countryList");
                return countryList;
            }
            finally
            {
                countryList = null;
            }

        }
        public static List<SelectListItem> GetSalesmenAndUser(string status = null)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    if (status == "")
                    {
                        status = null;
                        var sList = (from s in objDB.SSP_GetSalesmenAndUser(SessionManagement.SelectedBranchID, status)
                                     select new SelectListItem
                                     {
                                         Text = s.Name.ToString(),
                                         Value = s.id.ToString()
                                     }
                                ).ToList();

                        foreach (var items in sList)
                        {
                            objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                        }
                    }
                    else
                    {
                        var sList = (from s in objDB.SSP_GetSalesmenAndUser(SessionManagement.SelectedBranchID, status)
                                     select new SelectListItem
                                     {
                                         Text = s.Name.ToString(),
                                         Value = s.id.ToString()
                                     }
                               ).ToList();

                        foreach (var items in sList)
                        {
                            objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                        }
                    }
                }
                objList = objList.OrderBy(o => o.Text).ToList();
                objList.Insert(0, new SelectListItem { Text = "Select Employee", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmenAndUser,Parameter : status={status}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }
        public static List<SelectListItem> GetRecordTypeList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                objList.Insert(0, new SelectListItem { Text = "Select Type", Value = "0" });
                objList.Insert(1, new SelectListItem { Text = "Loan Payout", Value = "1" });
                objList.Insert(2, new SelectListItem { Text = "File Closed", Value = "2" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetRecordTypeList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }
        public static string GetRecordTypeById(int rectype)
        {
            try
            {
                if (rectype == 1)
                {
                    return "Loan Payout";
                }
                else if (rectype == 2)
                {
                    return "File Closed";
                }

                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetRecordTypeById,Parameter : rectype={rectype}");
                return null;
            }
            finally
            {

            }

        }

        public static List<SSP_UsersRoles_Result> UserRoleList(string uid)
        {
            List<SSP_UsersRoles_Result> countryList = new List<SSP_UsersRoles_Result>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    countryList = objDB.SSP_UsersRoles(uid).ToList();
                }
                return countryList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserRoleList,Parameter : uid={uid}");
                return countryList;
            }
            finally
            {
                countryList = null;
            }

        }

        public static List<SelectListItem> BranchListByCode(string code)
        {
            List<SelectListItem> branchList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.branches
                                 join c in objDB.companies on p.company_id equals c.id
                                 where p.branch_name.Contains(code)
                                 && p.isactive == true
                                 select new SelectListItem
                                 {
                                     Text = p.branch_name, //+ ", " + c.company_name
                                     Value = p.id.ToString()
                                 }
                                 ).ToList();

                    //objDB.branches.Where(o => o.branch_name.Contains(code) && o.isactive == true).ToList();
                    foreach (var items in sList)
                    {
                        branchList.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                    }
                }

                return branchList;

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchListByCode, Parameter : code={code}");
                return branchList;
            }
            finally
            {
                branchList = null;
            }

        }

        public static List<SelectListItem> BranchList()
        {
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.branches
                                 join c in objDB.companies on p.company_id equals c.id
                                 where p.isactive == true
                                 select new SelectListItem
                                 {
                                     Text = p.branch_name,//+ ", " + c.company_name
                                     Value = p.id.ToString()
                                 }
                                 ).ToList();

                    //objDB.branches.Where(o => o.branch_name.Contains(code) && o.isactive == true).ToList();
                    foreach (var items in sList)
                    {
                        branchList.Add(new SelectListItem { Text = items.Text, Value = items.Value });
                    }
                }
                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                return branchList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchList");
                return branchList;
            }
            finally
            {
                branchList = null;
            }

        }

        public static List<SelectListItem> GSTList()
        {
            List<SelectListItem> GSTList = new List<SelectListItem>();
            SelectListItem _gstList;
            string SqlWhere = "";
            try
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = " Select Gstpercentage from GST Where IsActive =1 ";
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        _gstList = new SelectListItem();
                        {
                            _gstList.Value = Ireader.GetString("Gstpercentage");
                            _gstList.Text = Ireader.GetString("Gstpercentage");

                        };
                        GSTList.Add(_gstList);
                    }
                }
                return GSTList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BranchList");
                return GSTList;
            }
            finally
            {
                GSTList = null;
            }

        }

        public static List<SelectListItem> SalesmenList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.salesmen.Where(o => o.isactive == true).OrderBy(o => o.salesmen_name).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.salesmen_name, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserSalesmenList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {

                    var sList = (from s in objDB.salesmen
                                 where s.isactive == true && s.branch_Id == SessionManagement.SelectedBranchID
                                 orderby s.salesmen_name
                                 select new SelectListItem
                                 {
                                     Text = s.salesmen_name,
                                     Value = s.id.ToString()
                                 }
                                ).ToList();

                    //var sList = (from p in objDB.branches
                    //             join c in objDB.user_access_rights on p.id equals c.branch_id
                    //             join s in objDB.salesmen on p.id equals s.branch_Id
                    //             where p.isactive == true // && c.user_id == new Guid(uid)
                    //             && s.isactive == true && p.id == SessionManagement.SelectedBranchID
                    //             select new SelectListItem
                    //             {
                    //                 Text = s.salesmen_name,
                    //                 Value = s.id.ToString()
                    //             }
                    //            ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserSalesmenList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> SalesmenListByBranch(int BranchId)
        {
            List<SelectListItem> BindSalesman = new List<SelectListItem>();
            SelectListItem _BindSalesman;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(BranchId, CommanConstans.branch_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindSalesmen);
                    while (Ireader.Read())
                    {
                        _BindSalesman = new SelectListItem();
                        {
                            _BindSalesman.Value = Ireader.GetString(CommonColumns.id);
                            //_BindSalesman.branch_Id = Ireader.GetString(CommonColumns.branch_Id);
                            _BindSalesman.Text = Ireader.GetString(CommonColumns.salesmen_name);
                            //_BindSalesman.saleman_commission = Ireader.GetDecimal(CommonColumns.saleman_commission);

                        };

                        BindSalesman.Add(_BindSalesman);
                    }

                }
                BindSalesman.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
                return BindSalesman;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmen");
                throw ex;
            }
            finally
            {
                BindSalesman = null;
                _BindSalesman = null;
            }
        }

        public static List<SelectListItem> SalesmenListwithoutlogin()
        {
            List<SelectListItem> BindSalesman = new List<SelectListItem>();
            SelectListItem _BindSalesman;
            string SqlWhere = "";
            try
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = "Select Id,salesmen_name from  salesmen where isActive =1 and (User_id='00000000-0000-0000-0000-000000000000' Or User_id = null)";
                    SqlWhere += " And branch_id = " + SessionManagement.SelectedBranchID + "";
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        _BindSalesman = new SelectListItem();
                        {
                            _BindSalesman.Value = Ireader.GetString(CommonColumns.id);
                            //_BindSalesman.branch_Id = Ireader.GetString(CommonColumns.branch_Id);
                            _BindSalesman.Text = Ireader.GetString(CommonColumns.salesmen_name);
                            //_BindSalesman.saleman_commission = Ireader.GetDecimal(CommonColumns.saleman_commission);

                        };

                        BindSalesman.Add(_BindSalesman);
                    }
                                  
                }
                BindSalesman.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
                return BindSalesman;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmen");
                throw ex;
            }
            finally
            {
                BindSalesman = null;
                _BindSalesman = null;
            }
        }
              
        public static List<SelectListItem> UserBranchList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.branches
                                 join c in objDB.user_access_rights on p.id equals c.branch_id
                                 where p.isactive == true && c.user_id == new Guid(uid)
                                 && p.isactive == true
                                 select new SelectListItem
                                 {
                                     Text = p.branch_name,
                                     Value = p.id.ToString()
                                 }
                                 ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserBranchList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserBranchListNew(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.branches
                                 join c in objDB.user_access_rights on p.id equals c.branch_id
                                 join ca in objDB.companies on p.company_id equals ca.id
                                 where p.isactive == true && c.user_id == new Guid(uid)
                                 && p.isactive == true
                                 select new SelectListItem
                                 {
                                     Text = p.branch_name + ", " + ca.company_name,
                                     Value = p.id.ToString()
                                 }
                                 ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserBranchListNew, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserProjectListWithID(string uid, Int32 projectid = 0, string supplierId = null)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            SelectListItem _objList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //var sList = (from p in objDB.projects
                    //             where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                    //             && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                    //             && ((p.status_id == 3 && p.id == projectid && p.id != 0) || (p.status_id != 3))
                    //             select new SelectListItem
                    //             {
                    //                 Text = p.project_name + " - " + p.project_number,
                    //                 Value = p.id.ToString()
                    //             }
                    //            ).ToList();
                    cmd.AddParameters(projectid, CommanConstans.project_Id, SqlDbType.Int);
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    cmd.AddParameters(supplierId, CommanConstans.supplier_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.ssp_Get_Payments_Address_Details);
                    while (Ireader.Read())
                    {
                        _objList = new SelectListItem();
                        {
                            _objList.Value = Ireader.GetString(CommonColumns.Value);
                            _objList.Text = Ireader.GetString(CommonColumns.Text);

                        };

                        objList.Add(_objList);
                    }
                    //foreach (var items in sList.OrderBy(o => o.Text).ToList())
                    //{
                    //    objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    //}
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithID, Parameter : uid={uid},projectid={projectid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<BudgetCostDetails> GetBudgetCostDetailsBySupplierId(string supplierId = null)
        {
            List<BudgetCostDetails> budgetCostDetails = new List<BudgetCostDetails>();
            BudgetCostDetails _budgetCostDetails;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    cmd.AddParameters(supplierId, CommanConstans.supplier_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_BugetCost_Details_By_SupplierId);
                    while (Ireader.Read())
                    {
                        _budgetCostDetails = new BudgetCostDetails();
                        {
                            _budgetCostDetails.Value = Ireader.GetString(CommonColumns.Value);
                            _budgetCostDetails.Text = Ireader.GetString(CommonColumns.Text);
                            _budgetCostDetails.invoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _budgetCostDetails.document_path = Ireader.GetString(CommonColumns.document_path);

                            _budgetCostDetails.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            _budgetCostDetails.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _budgetCostDetails.ApprovedAmount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            _budgetCostDetails.GSTPercent = Ireader.GetDecimal(CommonColumns.GSTPercent);
                            _budgetCostDetails.GSTAmount = Ireader.GetDecimal(CommonColumns.GSTAmount);
                            _budgetCostDetails.InvRemarks = Ireader.GetString(CommonColumns.InvRemarks);

                        };

                        budgetCostDetails.Add(_budgetCostDetails);
                    }
                }

                return budgetCostDetails;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithID, Parameter : supplierId={supplierId}");
                return budgetCostDetails;
            }
            finally
            {
                budgetCostDetails = null;
            }

        }

        public static List<payment_details> GetpaymentDetailsBySupplierId(string supplierId = null)
        {
            List<payment_details> budgetCostDetails = new List<payment_details>();
            payment_details _budgetCostDetails;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    cmd.AddParameters(supplierId, CommanConstans.supplier_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_BugetCost_Details_By_SupplierId);
                    while (Ireader.Read())
                    {
                        _budgetCostDetails = new payment_details();
                        {
                            //_budgetCostDetails.Value = Ireader.GetString(CommonColumns.Value);
                            //_budgetCostDetails.Text = Ireader.GetString(CommonColumns.Text);
                            _budgetCostDetails.supplier_inv_number_text = Ireader.GetString(CommonColumns.InvoiceNumber);
                            //_budgetCostDetails.Do = Ireader.GetString(CommonColumns.document_path);

                            _budgetCostDetails.invoice_amount = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            _budgetCostDetails.budgeted_cost = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _budgetCostDetails.agreed_amount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            _budgetCostDetails.gst_percentage = Ireader.GetDecimal(CommonColumns.GSTPercent);
                            _budgetCostDetails.gst_amount = Ireader.GetDecimal(CommonColumns.GSTAmount);

                        };

                        budgetCostDetails.Add(_budgetCostDetails);
                    }
                }

                return budgetCostDetails;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithID, Parameter : supplierId={supplierId}");
                return budgetCostDetails;
            }
            finally
            {
                budgetCostDetails = null;
            }

        }


        public static List<BudgetCostDetails> GetBudgetCostDetailsForEdit(string paymentId)
        {
            List<BudgetCostDetails> budgetCostDetails = new List<BudgetCostDetails>();
            BudgetCostDetails _budgetCostDetails;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(paymentId, CommanConstans.PaymentId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_GetSupplierPaymentInfo);
                    while (Ireader.Read())
                    {
                        _budgetCostDetails = new BudgetCostDetails();
                        {
                            _budgetCostDetails.Value = Ireader.GetString(CommonColumns.Value);
                            _budgetCostDetails.Text = Ireader.GetString(CommonColumns.Text);
                            _budgetCostDetails.invoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _budgetCostDetails.document_path = Ireader.GetString(CommonColumns.document_path);

                            _budgetCostDetails.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            _budgetCostDetails.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _budgetCostDetails.ApprovedAmount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            _budgetCostDetails.GSTPercent = Ireader.GetDecimal(CommonColumns.GSTPercent);
                            _budgetCostDetails.GSTAmount = Ireader.GetDecimal(CommonColumns.GSTAmount);
                            _budgetCostDetails.InvRemarks = Ireader.GetString(CommonColumns.InvRemarks);
                            _budgetCostDetails.PaymentAmount = Ireader.GetDecimal(CommonColumns.Payment_amount);

                        };

                        budgetCostDetails.Add(_budgetCostDetails);
                    }
                }

                return budgetCostDetails;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithID, Parameter : supplierId={paymentId}");
                return budgetCostDetails;
            }
            finally
            {
                budgetCostDetails = null;
            }

        }

        public static List<SelectListItem> UserProjectListWithIDNew(string uid, Int32 projectid = 0)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.projects
                                 join pb in objDB.projects_budget on p.id equals pb.project_id
                                 join pbd in objDB.projects_budget_detail on pb.id equals pbd.project_budget_id
                                 where p.isactive == true
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                                 && ((p.status_id == 2 && p.id == projectid && p.id != 0))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();

                    foreach (var items in sList.OrderBy(o => o.Text).ToList())
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithIDNew, Parameter : uid={uid},projectid={projectid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserProjectListWithSaleman(string uid, Int32 projectid = 0, Int32 salemanid = 0)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            List<SelectListItem> sList;
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    if (salemanid == 0)
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                                 && ((p.status_id == 3 && p.id == projectid && p.id != 0) || (p.status_id != 3))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();
                    }
                    else
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID && p.salesmen_id == salemanid
                                 && ((p.status_id == 3 && p.id == projectid && p.id != 0) || (p.status_id != 3))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();
                    }


                    foreach (var items in sList.OrderBy(o => o.Text).ToList())
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithSaleman, Parameter : uid={uid},projectid={projectid},salemanid={salemanid}");
                return objList;
            }
            finally
            {
                objList = null;
                sList = null;
            }

        }

        public static List<SelectListItem> UserProjectList(string uid, Int32 projectid = 0, Int32 salemanid = 0)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            List<SelectListItem> sList;            
          
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    if (projectid != 0 && salemanid !=0)
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.branch_id == SessionManagement.SelectedBranchID && p.id == projectid && p.salesmen_id == salemanid
                                 && ((p.status_id == 5 && p.id != 0))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }).ToList();
                    }                    
                    else if (salemanid != 0 && projectid ==0)
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 &&  p.branch_id == SessionManagement.SelectedBranchID && p.salesmen_id == salemanid
                                 && ((p.status_id == 5 && p.id != 0))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();
                    }
                    else if (salemanid == 0 && projectid != 0)
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.branch_id == SessionManagement.SelectedBranchID && p.id == projectid
                                 && ((p.status_id == 5 && p.id != 0))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();
                    }
                    else
                    {
                        sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.branch_id == SessionManagement.SelectedBranchID 
                                 && ((p.status_id == 5 && p.id != 0))
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();
                    }


                    foreach (var items in sList.OrderBy(o => o.Text).ToList())
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithSaleman, Parameter : uid={uid},projectid={projectid},salemanid={salemanid}");
                return objList;
            }
            finally
            {
                objList = null;
                sList = null;
            }

        }

        public static List<Database.SSP_UserBranches_Result> UserBranches(Guid UserId)
        {
            
            List<Database.SSP_UserBranches_Result> branchList = new List<Database.SSP_UserBranches_Result>();
            SSP_UserBranches_Result sBranchList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(UserId, CommanConstans.UserID, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_UserBranches);
                    while (Ireader.Read())
                    {
                        sBranchList = new SSP_UserBranches_Result();
                        {
                            sBranchList.Id = Ireader.GetInt64(CommonColumns.id);
                            sBranchList.branch_name = Ireader.GetString(CommonColumns.branch_name);
                            sBranchList.company_name = Ireader.GetString(CommonColumns.company_name);
                            sBranchList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                            sBranchList.IsBranchAdmin = Ireader.GetBoolean(CommonColumns.IsBranchAdmin);

                        };

                        branchList.Add(sBranchList);
                    }

                }
                return branchList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListWithSaleman, Parameter : uid={UserId}");
                return branchList;
            }
            finally
            {
                branchList = null;
                sBranchList = null;
            }

        }


        public static List<SelectListItem> FilterSupplierByAddress(int AddressId = 0)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from s in objDB.SSP_GetSupplierByAddress(AddressId)
                                 select new SelectListItem
                                 {
                                     Text = s.Name.ToString(),
                                     Value = s.id.ToString()
                                 }
                            ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }
                objList = objList.OrderBy(o => o.Text).ToList();
                objList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmenAndUser,Parameter : status={AddressId}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserProjectList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.projects
                                     //where p.isactive == true && (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();

                    foreach (var items in sList.OrderBy(o => o.Text))
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> ModeofPaymentList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.mode_of_payment.OrderBy(o => o.mode_of_payment1).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.mode_of_payment1, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ModeofPaymentList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> CustomerList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.customers.Where(o => o.isactive == true).OrderBy(o => o.name1).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.name1, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Customer", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CustomerList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> CustomerListByBranchId()
        {
            CustomerDropDown _GetCustomer = new CustomerDropDown();
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerListByBranch);
                    while (Ireader.Read())
                    {
                        objList.Add(new SelectListItem { Text = Ireader.GetString(CommonColumns.name1), Value = Ireader.GetString(CommonColumns.id) });
                        _GetCustomer.Customer_id = Ireader.GetInt32(CommonColumns.id);
                        _GetCustomer.name1 = Ireader.GetString(CommonColumns.name1);
                    }
                    objList.Insert(0, new SelectListItem { Text = "Select Customer", Value = "0" });
                }

                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CustomerListByBranchId  ");
                throw ex;
            }
            finally
            {
                _GetCustomer = null;
            }
        }


        public static List<SelectListItem> BankList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.banks.Where(o => o.isactive == true && o.branch_id == SessionManagement.SelectedBranchID).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.bank_name, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Bank", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BankList");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        public static List<SelectListItem> ProjectStatusList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.project_status.ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.project_status1, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Status", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectStatusList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static int GetSalesmanByProjectId(Int32 ProjectId)
        {
            int SalesmanId = 0;
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    project ObjProj = objDB.projects.Where(o => o.id == ProjectId).SingleOrDefault();
                    if (ObjProj != null)
                    {
                        SalesmanId = Convert.ToInt32(ObjProj.salesmen_id);
                    }

                }             

                //}
                return SalesmanId;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmanByProjectId, Parameter : ProjectId={ProjectId}");
                return 0;
            }
            finally
            {

            }

        }

        public static List<SelectListItem> DDLProjectStatusList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.project_status.ToList();
                    objList.Insert(0, new SelectListItem { Text = "All", Value = "" });
                    foreach (var items in sList)
                    {
                        if (items.id.ToString() != "1")
                        {
                            objList.Add(new SelectListItem { Text = items.project_status1, Value = items.id.ToString() });
                        }
                    }
                }

                //objList.Insert(2, new SelectListItem { Text = "All", Value = "" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: DDLProjectStatusList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> UserSupplierList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from s in objDB.suppliers
                                 where s.isactive == true //&& (s.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 orderby s.supplier_name
                                 select new SelectListItem
                                 {
                                     Text = s.supplier_name,
                                     Value = s.id.ToString()
                                 }
                                 ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: UserSupplierList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> StatusList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            try
            {

                statusList.Insert(0, new SelectListItem { Text = "Yes", Value = "true" });
                statusList.Insert(1, new SelectListItem { Text = "No", Value = "false" });
                return statusList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: StatusList");
                return statusList;
            }
            finally
            {
                statusList = null;
            }

        }

        public static List<SelectListItem> ProjectBudgetStatusList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.status.ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.status1, Value = items.id.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Status ", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectStatusList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> GSTRegistered(bool gstregistered = true)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            try
            {
                if (gstregistered)
                {
                    result.Add(new SelectListItem { Text = "Yes", Value = "true", Selected = true });
                    result.Add(new SelectListItem { Text = "No", Value = "false" });
                }
                else if (gstregistered == false)
                {
                    result.Add(new SelectListItem { Text = "Yes", Value = "true" });
                    result.Add(new SelectListItem { Text = "No", Value = "false", Selected = true });


                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GSTRegistered, Parameter : gstregistered={gstregistered}");
                return result;
            }
            finally
            {
                result = null;
            }

        }

        public static List<SelectListItem> AdditionOmissionTypeList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            try
            {
                statusList.Insert(0, new SelectListItem { Text = "Addition", Value = "1" });
                statusList.Insert(1, new SelectListItem { Text = "Omission", Value = "2" });
                statusList.Insert(2, new SelectListItem { Text = "Discount", Value = "3" });
                statusList.Insert(2, new SelectListItem { Text = "Electrical", Value = "4" });
                return statusList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: AdditionOmissionTypeList");
                return statusList;
            }
            finally
            {
                statusList = null;
            }

        }
        
       
        public static List<SelectListItem> MonthList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                objList.Add(new SelectListItem { Text = "January", Value = "1" });
                objList.Add(new SelectListItem { Text = "February", Value = "2" });
                objList.Add(new SelectListItem { Text = "March", Value = "3" });
                objList.Add(new SelectListItem { Text = "April", Value = "4" });
                objList.Add(new SelectListItem { Text = "May", Value = "5" });
                objList.Add(new SelectListItem { Text = "June", Value = "6" });
                objList.Add(new SelectListItem { Text = "July", Value = "7" });
                objList.Add(new SelectListItem { Text = "August", Value = "8" });
                objList.Add(new SelectListItem { Text = "September", Value = "9" });
                objList.Add(new SelectListItem { Text = "October", Value = "10" });
                objList.Add(new SelectListItem { Text = "November", Value = "11" });
                objList.Add(new SelectListItem { Text = "December", Value = "12" });

                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: MonthList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> YearList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                for (int i = 2016; i <= DateTime.Now.Year + 1; i++)
                {
                    objList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: YearList");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }
        public static List<SelectListItem> CompanyList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from s in objDB.companies
                                 where s.isactive == true
                                 orderby s.company_name
                                 select new SelectListItem
                                 {
                                     Text = s.company_name,
                                     Value = s.id.ToString()
                                 }
                                 ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Company", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: CompanyList");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }


        public static List<SelectListItem> UserProjectListByYear(string uid, Int32 year, Int32 salesmenId)
        {
            DateTime start = Convert.ToDateTime("01/01/" + year.ToString());
            DateTime endDate = start.AddYears(1);
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.projects
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                                 && p.contract_date >= start && p.contract_date < endDate
                                 && (p.salesmen_id == salesmenId || salesmenId == 0)
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                 ).ToList();

                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectListByYear, Parameter : uid={uid},year={year},salesmenId{salesmenId}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> AlphabetList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                for (char c = 'A'; c < 'Z'; c++)
                {
                    objList.Add(new SelectListItem { Text = c.ToString(), Value = c.ToString() });
                }
                objList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                objList.Insert(1, new SelectListItem { Text = "0-9", Value = "1" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: AlphabetList");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        #region 
        public static Boolean MergeObjects(Object originalObj, Object modifiedObj, Boolean updateNullValues = false)
        {
            try
            {
                // Uncomment in case objects of different type are not allowed to be merged
                //if (originalObj.GetType() != modifiedObj.GetType()) return false;

                PropertyInfo[] propertyInfos = originalObj.GetType().GetProperties();
                Object originalVal;
                Object modifiedVal;

                //loop through each property of the originalObj
                foreach (PropertyInfo objProperty in propertyInfos)
                {
                    //retrieve the value of the current property
                    originalVal = originalObj.GetType().GetProperty(objProperty.Name).GetValue(originalObj, null);

                    //check if a similar property exists in the modifiedObj
                    if (modifiedObj.GetType().GetProperty(objProperty.Name) != null &&
                        Convert.ToString(objProperty.Name) != "EntityState" &&
                        Convert.ToString(objProperty.Name) != "EntityKey")
                    {
                        //retrieve the modified value of the current property from the modifiedVal
                        modifiedVal = modifiedObj.GetType().GetProperty(objProperty.Name).GetValue(modifiedObj, null);

                        //update the origional object if the origional and the modified value are different and the modified value is not null
                        if ((Convert.ToString(originalVal) != Convert.ToString(modifiedVal)) && (modifiedVal != null || updateNullValues == true))
                            originalObj.GetType().GetProperty(objProperty.Name).SetValue(originalObj, modifiedVal, null);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static Boolean MergeObjects(Object originalObj, Object modifiedObj, String[] ignoreProperties)
        {
            try
            {
                // Uncomment in case objects of different type are not allowed to be merged
                //if (originalObj.GetType() != modifiedObj.GetType()) return false;

                PropertyInfo[] propertyInfos = originalObj.GetType().GetProperties();
                Object originalVal;
                Object modifiedVal;

                //loop through each property of the originalObj
                foreach (PropertyInfo objProperty in propertyInfos)
                {
                    //retrieve the value of the current property
                    originalVal = originalObj.GetType().GetProperty(objProperty.Name).GetValue(originalObj, null);

                    //check if a similar property exists in the modifiedObj
                    if (modifiedObj.GetType().GetProperty(objProperty.Name) != null)
                    {
                        //retrieve the modified value of the current property from the modifiedVal
                        modifiedVal = modifiedObj.GetType().GetProperty(objProperty.Name).GetValue(modifiedObj, null);

                        //check if the origional and the modified value are different and the modified value is not null
                        if ((Convert.ToString(originalVal) != Convert.ToString(modifiedVal)) && modifiedVal != null)
                        {
                            //update the origional object if the current property is not specified to be ignored
                            if (!ignoreProperties.Contains(objProperty.Name))
                                originalObj.GetType().GetProperty(objProperty.Name).SetValue(originalObj, modifiedVal, null);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public static List<SelectListItem> SalesmenStatusList()
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            try
            {
                statusList.Insert(0, new SelectListItem { Text = "All", Value = "", Selected = true });
                statusList.Insert(1, new SelectListItem { Text = "Active", Value = "1" });
                statusList.Insert(2, new SelectListItem { Text = "Inactive", Value = "0" });
                return statusList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenStatusList");
                return statusList;
            }
            finally
            {
                statusList = null;
            }

        }
        public static List<SelectListItem> SalesmenListStatusWise(string status)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    if (status == "")
                    {
                        var sList = (from s in objDB.salesmen
                                     where s.branch_Id == SessionManagement.SelectedBranchID
                                     orderby s.salesmen_name
                                     select new SelectListItem
                                     {
                                         Text = s.salesmen_name,
                                         Value = s.id.ToString()
                                     }
                               ).ToList();
                        foreach (var items in sList)
                        {
                            objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                        }
                    }
                    else
                    {
                        bool isactive = true;
                        if (status == "0")
                        {
                            isactive = false;
                        }
                        var sList = (from s in objDB.salesmen
                                     where s.isactive == isactive && s.branch_Id == SessionManagement.SelectedBranchID
                                     orderby s.salesmen_name
                                     select new SelectListItem
                                     {
                                         Text = s.salesmen_name,
                                         Value = s.id.ToString()
                                     }
                               ).ToList();
                        foreach (var items in sList)
                        {
                            objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                        }
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Salesmen", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SalesmenListStatusWise, Parameter : status={status}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<SelectListItem> PaymentList(int id)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.SSP_PaymentDetailsForSMS(id).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem
                        {
                            Text = items.date_amount
                            ,
                            Value = items.cheque_details.ToString()
                        });
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PaymentList, Parameter : id={id}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<string> SupplierMobile(string id)
        {
            List<long> TagIds = id.Split(',').Select(long.Parse).ToList();

            List<string> mobile = new List<string>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {

                    //mobile = objDB.Database.SqlQuery<List<string>>("select mobile from supplier where id in ("+id+")")
                    //               .FirstOrDefault();

                    mobile = (from s in objDB.suppliers where TagIds.Contains(s.id) select s.mobile).ToList();
                    //var userProfiles = objDB.suppliers
                    //                .Where(t => TagIds.Contains(t.id));
                }
                return mobile;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SupplierMobile, Parameter : id={id}");
                return mobile;
            }
            finally
            {
                mobile = null;
            }


        }
        public static List<SelectListItem> SuppliersForSMS(string ids = null)
        {
            List<SelectListItem> objList = new List<SelectListItem>();

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = objDB.SSP_GetSuppliersForSMS(ids).ToList();
                    foreach (var items in sList)
                    {
                        objList.Add(new SelectListItem
                        {
                            Text = items.supplier_name
                            ,
                            Value = items.id.ToString()
                        });
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SuppliersForSMS, Parameter : ids={ids}");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        public static List<SelectListItem> SourceList()
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            DataLayer objDB = new DataLayer();
            try
            {
                //using (PMSEntities objDB = new PMSEntities())
                //{
                //    var sList = objDB.banks.Where(o => o.isactive == true && o.branch_id == SessionManagement.SelectedBranchID).ToList();
                //    foreach (var items in sList)
                //    {
                //        objList.Add(new SelectListItem { Text = items.bank_name, Value = items.id.ToString() });
                //    }
                //}
                var sList = objDB.BindSourceList();
                foreach (var items in sList)
                {
                    objList.Add(new SelectListItem { Text = items.Source_Name, Value = items.Source_Id.ToString() });
                }

                objList.Insert(0, new SelectListItem { Text = "Select Source", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SourceList");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }



        public static int SalesmenIDByUserID(string uid)
        {
            int objList = 0;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_GetSalesManIDbyBranchID]");
                    while (Ireader.Read())
                    {
                        objList = Ireader.GetInt32(CommonColumns.id);
                    }
                }

                //using (PMSEntities objDB = new PMSEntities())
                //{
                //    var sList = (from s in objDB.salesmen
                //                 join netUser in objDB.AspNetUsers on s.salesmen_name equals netUser.UserName
                //                 join usraccrights in objDB.user_access_rights on netUser.Id equals usraccrights.user_id.ToString()
                //                 where s.isactive == true && s.branch_Id == SessionManagement.SelectedBranchID && netUser.Id == uid
                //                 select s.id
                //                ).ToList();
                //    if (sList.Count > 0)
                //        objList = Convert.ToInt32(sList[0]);
                //}
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserSalesmenList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                //objList = null;
            }
        }

        public static string SalesmenNameUserID(Int32 slsMenID)
        {
            string objList = "";

            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {

                    var sList = (from s in objDB.salesmen
                                 join usrdtl in objDB.user_detail
                                 on s.salesmen_name equals usrdtl.Name
                                 where s.isactive == true && s.branch_Id == SessionManagement.SelectedBranchID && s.id == slsMenID
                                 select usrdtl.user_id
                                ).ToList();
                    if (sList.Count > 0)
                        objList = sList[0].ToString();
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserSalesmenList, Parameter : uid={slsMenID}");
                return objList;
            }
            finally
            {
                //objList = null;
            }
        }

        public static int UpdateAspNetUserRole(long UserID,long SalesmanId)
        {
            int rtn = 0;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(UserID, CommanConstans.user_id, SqlDbType.BigInt);
                    cmd.AddParameters(SalesmanId, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    var rtnVal = cmd.ExecuteScalar(SqlProcedures.UpdateAspNetUserRoles);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpdateAspNetUserRole Parameters= " + UserID);
                throw ex;
            }
        }

        public static int UpdateUserDetailStatus(long SalesmanId)
        {
            int rtn = 0;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(SalesmanId, CommanConstans.SalesMenId, SqlDbType.BigInt);
                    var rtnVal = cmd.ExecuteScalar(SqlProcedures.UpdateUserStatus);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpdateUserDetailStatus Parameters= " + SalesmanId);
                throw ex;
            }
        }

        public static int UpdateBranchAdmin(long Id,bool IsBranchAdmin)
        {
            int rtn = 0;
            string SqlWhere = "";
            int bIsBranchAdmin = 0;
            try
            {
                if (IsBranchAdmin)
                    bIsBranchAdmin = 1;
                else
                    bIsBranchAdmin = 0;
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = "Update user_access_rights Set IsbranchAdmin =" + bIsBranchAdmin + " Where id =" + Id;
                    var rtnVal = cmd.ExecuteScalar(SqlWhere);
                }
                return rtn;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpdateBranchAdmin Parameters= " + Id);
                throw ex;
            }
        }

        public static bool GetIsBranchAdmin(string UserId, Int32 BranchId)
        {
            string SqlWhere = "";
            bool bIsBranchAdmin = false;
            try 
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = "Select IsBranchAdmin from  user_access_rights where  User_id = '" + UserId + "' and  branch_id =" + BranchId;
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        bIsBranchAdmin = Ireader.GetBoolean("IsBranchAdmin");
                    }
                }
                return bIsBranchAdmin;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetIsBranchAdmin Parameters= " + UserId);
                throw ex;
            }
        }

        public static List<SelectListItem> GetSalesmenByUserIdExistsInUsersTbl(string uid)
        {
            SalesmanDropDown _GetSalesmen = new SalesmanDropDown();
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_SalesmenByUserIdExistsInUsersTbl);
                    while (Ireader.Read())
                    {
                        objList.Add(new SelectListItem { Text = Ireader.GetString(CommonColumns.salesmen_name), Value = Ireader.GetString(CommonColumns.id) });
                        _GetSalesmen.id = Ireader.GetInt32(CommonColumns.id);
                        _GetSalesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                    }

                }

                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenIdByUserId Parameters= " + uid);
                throw ex;
            }
            finally
            {
                _GetSalesmen = null;
            }
        }

        public static List<SalesmanDropDown> BindSalesmenForQuotation()
        {
            List<SalesmanDropDown> BindSalesman = new List<SalesmanDropDown>();
            SalesmanDropDown _BindSalesman;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindSalesmen);
                    while (Ireader.Read())
                    {
                        _BindSalesman = new SalesmanDropDown();
                        {
                            _BindSalesman.id = Ireader.GetInt32(CommonColumns.id);
                            //_BindSalesman.branch_Id = Ireader.GetString(CommonColumns.branch_Id);
                            _BindSalesman.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            //_BindSalesman.saleman_commission = Ireader.GetDecimal(CommonColumns.saleman_commission);

                        };

                        BindSalesman.Add(_BindSalesman);
                    }

                }

                return BindSalesman;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSalesmen");
                throw ex;
            }
            finally
            {
                BindSalesman = null;
                _BindSalesman = null;
            }
        }

        public static List<SelectListItem> UserSalesMenProjectList(string uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.projects
                                 join slsmen in objDB.salesmen on p.salesmen_id equals slsmen.id
                                 join usrdtls in objDB.user_detail on slsmen.salesmen_name equals usrdtls.Name
                                 //join r in objDB.receipts on p.id equals r.project_id
                                 //where p.isactive == true && (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 where p.isactive == true //&& (p.created_by == new Guid(uid) || uid == "00000000-0000-0000-0000-000000000000")
                                 && p.isactive == true && p.branch_id == SessionManagement.SelectedBranchID
                                 && usrdtls.user_id == new Guid(uid)
                                 select new SelectListItem
                                 {
                                     Text = p.project_name + " - " + p.project_number,
                                     Value = p.id.ToString()
                                 }
                                ).ToList();

                    foreach (var items in sList.OrderBy(o => o.Text))
                    {
                        objList.Add(new SelectListItem { Text = items.Text, Value = items.Value.ToString() });
                    }
                }

                objList.Insert(0, new SelectListItem { Text = "Select Address/Site", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: UserProjectList, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }

        }

        public static List<Database.additions_omissions> GetVODataForProjectCostingReport(int VOId)
        {
            List<Database.additions_omissions> VOList = new List<Database.additions_omissions>();
            Database.additions_omissions _VOList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(VOId, CommanConstans.projectid, SqlDbType.BigInt);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetVODetailsForProject);
                    while (Ireader.Read())
                    {
                        _VOList = new additions_omissions();
                        {
                            _VOList.id = Convert.ToInt32(Ireader.GetString("id"));
                            _VOList.date = Ireader.GetDateTime("date");
                            //_VOList.project_id = Convert.ToInt32(Ireader.GetGuid("project_id"));
                            if (!String.IsNullOrEmpty(Ireader.GetString("record_type")))
                                _VOList.record_type = Convert.ToInt32(Ireader.GetString("record_type"));
                            _VOList.addition_omissioni_description = Ireader.GetString("addition_omissioni_description");
                            _VOList.amount = Convert.ToDecimal(Ireader.GetString("amount"));
                            _VOList.gst_percentage = Convert.ToDecimal(Ireader.GetString("gst_percentage"));
                            _VOList.gst_amount = Convert.ToDecimal(Ireader.GetString("gst_amount"));
                            _VOList.total_amount = Convert.ToDecimal(Ireader.GetString("total_amount"));
                            _VOList.remarks = Ireader.GetString("remarks");
                            _VOList.created_date = Ireader.GetDateTime("created_date");
                            //_VOList.created_by = Ireader.GetGuid("created_by");
                            _VOList.modified_date = Ireader.GetDateTime("modified_date");
                            //_VOList.modified_by = Ireader.GetGuid("modified_by");
                            //_VOList.isactive = Convert.ToBoolean(Ireader.GetString("isactive"));
                        };

                        VOList.Add(_VOList);
                    }

                }

                return VOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationForSearchPackage");
                throw ex;
            }
            finally
            {
                VOList = null;
            }
        }

        public static List<GetPaymentDetails> GetPaymentDataProjectCostingReport(int Project_Id)
        {
            List<GetPaymentDetails> PaymentList = new List<GetPaymentDetails>();
            GetPaymentDetails _PaymentList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Project_Id, CommanConstans.project_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetPaymentDetailsForProject);
                    while (Ireader.Read())
                    {
                        _PaymentList = new GetPaymentDetails();
                        _PaymentList.SupplierId = Ireader.GetInt64(CommonColumns.SupplierId);
                        _PaymentList.Date = Ireader.GetDateTime(CommonColumns.Date);
                        _PaymentList.Description = Ireader.GetString(CommonColumns.description);
                        _PaymentList.SupplierName = Ireader.GetString(CommonColumns.Supplier_Name);
                        _PaymentList.InvNo = Ireader.GetString(CommonColumns.InvNo);
                        _PaymentList.NonGst = Ireader.GetDecimal(CommonColumns.NonGst);
                        _PaymentList.Gst = Ireader.GetDecimal(CommonColumns.Gst);
                        _PaymentList.BudgetAmount = Ireader.GetDecimal(CommonColumns.budgetamount);
                        _PaymentList.projectId = Ireader.GetInt64(CommonColumns.Project_id);
                        _PaymentList.ProjectBudgetdetailsId = Ireader.GetInt64(CommonColumns.ProjectBudgetdetailsId);
                        PaymentList.Add(_PaymentList);
                    }
                }

                return PaymentList;

            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationForSearchPackage");
                throw ex;
            }
            finally
            {
                PaymentList = null;
            }
        }

        public static List<Database.additions_omissions> GetEVODataForProjectCostingReport(int EVOId)
        {
            List<Database.additions_omissions> EVOList = new List<Database.additions_omissions>();
            Database.additions_omissions _EVOList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(EVOId, CommanConstans.projectid, SqlDbType.BigInt);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetEVODetailsForProject);
                    while (Ireader.Read())
                    {
                        _EVOList = new additions_omissions();
                        {
                            //_VOList.id = Convert.ToInt32(Ireader.GetString("id"));
                            _EVOList.date = Ireader.GetDateTime("date");
                            //_VOList.project_id = Convert.ToInt32(Ireader.GetGuid("project_id"));
                            if (!String.IsNullOrEmpty(Ireader.GetString("record_type")))
                                _EVOList.record_type = Convert.ToInt32(Ireader.GetString("record_type"));
                            //_VOList.addition_omissioni_description = Ireader.GetString("addition_omissioni_description");
                            _EVOList.amount = Convert.ToDecimal(Ireader.GetString("amount"));
                            _EVOList.gst_percentage = Convert.ToDecimal(Ireader.GetString("gst_percentage"));
                            _EVOList.gst_amount = Convert.ToDecimal(Ireader.GetString("gst_amount"));
                            _EVOList.total_amount = Convert.ToDecimal(Ireader.GetString("total_amount"));
                            _EVOList.remarks = Ireader.GetString("remarks");
                            _EVOList.created_date = Ireader.GetDateTime("created_date");
                            //_VOList.created_by = Ireader.GetGuid("created_by");
                            _EVOList.modified_date = Ireader.GetDateTime("modified_date");
                            //_VOList.modified_by = Ireader.GetGuid("modified_by");
                            //_VOList.isactive = Convert.ToBoolean(Ireader.GetString("isactive"));
                        };

                        EVOList.Add(_EVOList);
                    }

                }

                return EVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationForSearchPackage");
                throw ex;
            }
            finally
            {
                EVOList = null;
            }
        }

        public static string GetProjectGuidByID(string prjID)
        {
            string objList = "";

            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(prjID, CommanConstans.projectid, SqlDbType.BigInt);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetProjectGuidByID);
                    while (Ireader.Read())
                    {
                        objList = Ireader.GetString("project_id");
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectGuidByID, Parameter : uid={prjID}");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        public static string GetProjectReferenceNo(long prjID)
        {
            string ReferenceNo = "";
            string SqlWhere = "";

            try
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlWhere = " Select ReferenceNo From Projects where id =" + prjID;
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        ReferenceNo = Ireader.GetString("ReferenceNo");
                    }            
                }
                return ReferenceNo;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectReferenceNo, Parameter : uid={prjID}");
                return ReferenceNo;
            }
            finally
            {
                ReferenceNo = null;
            }
        }
        public static Guid GetProjectGuidByprojectid(string prjID)
        {
            Guid objList = new Guid();

            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(prjID, CommanConstans.projectid, SqlDbType.BigInt);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetProjectGuidByID);
                    while (Ireader.Read())
                    {
                        objList = Ireader.GetGuid("project_id");
                    }
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectGuidByID, Parameter : uid={prjID}");
                return objList;
            }
            finally
            {
                objList = new Guid();
            }
        }


        public static string GetProjectStatus(string prjID)
        {
            string objList = "";

            try
            {
                int projIdConv = Convert.ToInt32(prjID);
                using (PMSEntities objDB = new PMSEntities())
                {
                    var sList = (from p in objDB.projects
                                 where p.id == projIdConv
                                 select p.status_id
                               ).ToList();
                    if (sList.Count > 0)
                        objList = sList[0].ToString();
                }
                return objList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectStatus, Parameter : uid={prjID}");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        public static List<Database.SSP_Loan_Result> GetLoan_Results(Guid userId, int branchid, DateTime FromDate, DateTime ToDate, int StartIndex, int PageSize, string SortBy, string OrderBy)
        {
            List<Database.SSP_Loan_Result> LoanList = new List<Database.SSP_Loan_Result>();
            Database.SSP_Loan_Result _LoanList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(userId, CommanConstans.UserID, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(branchid, CommanConstans.branchId, SqlDbType.Int);
                    cmd.AddParameters(FromDate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(ToDate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(StartIndex, CommanConstans.startRowIndex, SqlDbType.Int);
                    cmd.AddParameters(PageSize, CommanConstans.pageSize, SqlDbType.Int);
                    cmd.AddParameters(SortBy, CommanConstans.ColSort, SqlDbType.VarChar);
                    cmd.AddParameters(OrderBy, CommanConstans.OrderBy, SqlDbType.VarChar);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetLoanList);
                    while (Ireader.Read())
                    {
                        _LoanList = new SSP_Loan_Result();
                        {
                            _LoanList.Id = Convert.ToInt32(Ireader.GetString("id"));
                            _LoanList.loan_date = Ireader.GetString("loan_date");
                            _LoanList.person_name = Ireader.GetString("person_name");
                            _LoanList.mode_of_payment = Ireader.GetString("mode_of_payment");
                            _LoanList.rec_type = Convert.ToInt32(Ireader.GetString("rec_type"));
                            _LoanList.bank_name = Ireader.GetString("bank_name");
                            _LoanList.amount = Convert.ToDecimal(Ireader.GetString("amount"));
                            _LoanList.CreatedUpdated = Ireader.GetString("CreatedUpdated");
                            _LoanList.project_id = Ireader.GetInt64("project_id");
                        };

                        LoanList.Add(_LoanList);
                    }

                }

                return LoanList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetLoan_Results");
                throw ex;
            }
            finally
            {
                LoanList = null;
            }
        }

        public static List<Database.SSP_Customer_Result> GetCustomer_Result (Int32 StartIndex, Int32 PageSize, string SortBy, string OrderBy, string customersearch,int BranchId)
        {
            List<Database.SSP_Customer_Result> CustomerList = new List<Database.SSP_Customer_Result>();
            Database.SSP_Customer_Result _CustomerList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(StartIndex, CommanConstans.startRowIndex, SqlDbType.Int);
                    cmd.AddParameters(PageSize, CommanConstans.pageSize, SqlDbType.Int);
                    cmd.AddParameters(SortBy, CommanConstans.ColSort, SqlDbType.VarChar);
                    cmd.AddParameters(OrderBy, CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(customersearch, CommanConstans.CustomerSearch, SqlDbType.VarChar);
                    cmd.AddParameters(BranchId, CommanConstans.branchId, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerList);
                    while (Ireader.Read())
                    {
                        _CustomerList = new SSP_Customer_Result();
                        {
                            _CustomerList.Rk = Convert.ToInt32(Ireader.GetString("Rk"));
                            _CustomerList.id = Convert.ToInt32(Ireader.GetString("id"));
                            _CustomerList.name1 = Ireader.GetString("name1");
                            _CustomerList.nric1 = Ireader.GetString("nric1");
                            _CustomerList.block_no = Ireader.GetString("block_no");
                            _CustomerList.job_site = Ireader.GetString("job_site");
                            _CustomerList.gst_no = Ireader.GetString("gst_no");
                            _CustomerList.isactive = Convert.ToBoolean(Ireader.GetString("isactive"));
                            _CustomerList.branch_name = Ireader.GetString("branch_name");
                            _CustomerList.SourceName = Ireader.GetString("SourceName");
                            _CustomerList.TotalRecords = Ireader.GetInt64("TotalRecords");
                        };

                        CustomerList.Add(_CustomerList);
                    }

                }

                return CustomerList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetLoan_Results");
                throw ex;
            }
            finally
            {
                CustomerList = null;
            }
        }

        public class GetPaymentDetails
        {
            public long SupplierId { get; set; }
            public Nullable<System.DateTime> Date { get; set; }
            public string Description { get; set; }
            public string SupplierName { get; set; }
            public string InvNo { get; set; }
            public Nullable<decimal> NonGst { get; set; }
            public Nullable<decimal> Gst { get; set; }
            public Nullable<decimal> BudgetAmount { get; set; }
            public long ProjectBudgetdetailsId { get; set; }

            public long projectId { get; set; }
        }

        public static List<SelectListItem> GetSupplierListBySalesManBudgetCost(int uid)
        {
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.salesMenId, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_GetSupplierListByBudgetCost);
                    while (Ireader.Read())
                    {
                        objList.Add(new SelectListItem { Text = Ireader.GetString(CommonColumns.supplier_name), Value = Ireader.GetString(CommonColumns.id) });
                        //_GetSalesmen.id = Ireader.GetInt32(CommonColumns.id);
                        //_GetSalesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                    }

                }

                objList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                return objList;
            }
            catch (Exception ex)
            {

                ExceptionLog.WriteLog(ex, $"Method Name: GetSupplierListBySalesManBudgetCost, Parameter : uid={uid}");
                return objList;
            }
            finally
            {
                objList = null;
            }
        }

        public static List<SelectListItem> GetSalesmenIdByUserId(string uid)
        {
            List<SelectListItem> salesmenList = new List<SelectListItem>();
            SelectListItem salesItem;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_SalesmenByUserId);
                    while (Ireader.Read())
                    {
                        salesItem = new SelectListItem();
                        salesItem.Value = Ireader.GetString(CommonColumns.id);
                        salesItem.Text = Ireader.GetString(CommonColumns.salesmen_name);
                        salesmenList.Add(salesItem);
                    }
                }

                return salesmenList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenIdByUserId Parameters= " + uid);
                throw ex;
            }
            finally
            {
                salesItem = null;
                salesmenList = null;
            }
        }
       
        public static List<SelectListItem> GetSupplierIdByUserId(string uid)
        {
            List<SelectListItem> SupplierList = new List<SelectListItem>();
            SelectListItem SupplierItem;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_SupplierByUserId);
                    while (Ireader.Read())
                    {
                        SupplierItem = new SelectListItem();
                        SupplierItem.Value = Ireader.GetString(CommonColumns.id);
                        SupplierItem.Text = Ireader.GetString(CommonColumns.supplier_name);
                        SupplierList.Add(SupplierItem);
                    }
                }

                return SupplierList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenIdByUserId Parameters= " + uid);
                throw ex;
            }
            finally
            {
                SupplierItem = null;
                SupplierList = null;
            }
        }

        public static List<project_document_list> GetProjectDocuments(Int32 prjid)
        {
            List<project_document_list> objPDList = new List<project_document_list>();

            //List<SelectListItem> salesmenList = new List<SelectListItem>();
            project_document_list prjDocItem;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(prjid, CommanConstans.projectid, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Documents_For_Project);
                    while (Ireader.Read())
                    {
                        prjDocItem = new project_document_list();
                        prjDocItem.Id = Ireader.GetInt16(CommonColumns.id);
                        prjDocItem.project_id = Ireader.GetInt64("Id");
                        prjDocItem.uploaded_by = Ireader.GetGuid("uploaded_by");
                        prjDocItem.Uploaded_By_Name = Ireader.GetInt32("Uploaded_By_Name");
                        prjDocItem.uploaded_on = Ireader.GetDateTime("uploaded_on");
                        prjDocItem.file_desc = Ireader.GetString("file_desc");
                        prjDocItem.file_name = Ireader.GetString("file_name");
                        //salesItem.Value = Ireader.GetString(CommonColumns.id);
                        //salesItem.Text = Ireader.GetString(CommonColumns.salesmen_name);
                        objPDList.Add(prjDocItem);
                    }
                }

                return objPDList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectDocuments Parameters= " + prjid);
                throw ex;
            }
            finally
            {
                prjDocItem = null;
                objPDList = null;
            }
        }

        public static List<project_document_list> GetDocuments(long projectid,int Id_Type)
        {
            List<project_document_list> objPDList = new List<project_document_list>();

            //List<SelectListItem> salesmenList = new List<SelectListItem>();
            project_document_list prjDocItem;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(projectid, CommanConstans.projectid, SqlDbType.Int);
                    cmd.AddParameters(Id_Type, "@IdType", SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Documents);
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

                return objPDList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectDocuments Parameters= " + projectid);
                throw ex;
            }
            finally
            {
                prjDocItem = null;
                objPDList = null;
            }
        }

        public static List<SSP_SaleIndividualReport_Result> GetSaleIndividualReport_Result(string uid, Int32 SalesmenId, DateTime? from_date, DateTime? to_date, string ProjectStatus, int SelectedBranchID)
        {
            List<SSP_SaleIndividualReport_Result> resultItems = new List<SSP_SaleIndividualReport_Result>();

            SSP_SaleIndividualReport_Result salesRptItems = new SSP_SaleIndividualReport_Result();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.UserID, SqlDbType.VarChar);
                    cmd.AddParameters(SalesmenId, "@SalesmenId", SqlDbType.Int);
                    cmd.AddParameters(from_date, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(to_date, CommanConstans.todate, SqlDbType.DateTime);
                    if (string.IsNullOrEmpty(ProjectStatus))
                        ProjectStatus = "0";
                    cmd.AddParameters(ProjectStatus, CommanConstans.projectStatus, SqlDbType.VarChar);
                    cmd.AddParameters(SelectedBranchID, CommanConstans.branchId, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_SaleIndividualReport]");
                    while (Ireader.Read())
                    {
                        salesRptItems = new SSP_SaleIndividualReport_Result();
                        salesRptItems.id = Ireader.GetInt32(CommonColumns.id);
                        salesRptItems.receipt_date = Ireader.GetDateTime("receipt_date").ToString();
                        salesRptItems.total_amount = Ireader.GetDecimal("total_amount");
                        salesRptItems.contract_date = Ireader.GetDateTime("contract_date").ToString();
                        salesRptItems.salesmen_name = Ireader.GetString("salesmen_name");
                        salesRptItems.project_number = Ireader.GetString("project_number");
                        salesRptItems.project_name = Ireader.GetString("project_name");
                        salesRptItems.remarks = Ireader.GetString("remarks");
                        resultItems.Add(salesRptItems);
                    }
                }
                return resultItems;
            }
            catch (Exception ex)
            {
                //ExceptionLog.WriteLog(ex, "Method Name: GetSaleIndividualReport_Result Parameters= " + prjid);
                throw ex;
            }
            finally
            {
                resultItems = null;
                salesRptItems = null;
            }
        }

        public static DataTable GetExcelDataHeader(string filePath)
        {
            DataTable dtHeader = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filePath, false))
            {

                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                int rowsCnt = 0;
                dtHeader.Columns.Add("HeaderColName");
                dtHeader.Columns.Add("HeaderColValue");
                foreach (Row row in rows) //this will also include your header row...
                {
                    DataRow tempRow = dtHeader.NewRow();
                    int colsCnt = 0;
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        Cell cell = row.Descendants<Cell>().ElementAt(i);
                        int index = CellReferenceToIndex(cell);
                        tempRow[index] = GetCellValue(spreadSheetDocument, cell);
                        colsCnt++;
                        if (colsCnt == 2)
                            break;
                    }
                    dtHeader.Rows.Add(tempRow);
                    rowsCnt++;
                    if (rowsCnt == 9)
                        break;
                }
            }
            return dtHeader;
        }

        public static DataTable GetExcelDataLineItems(string filePath)
        {
            DataTable dt = new DataTable();
            string[] detailsColumns =
            {
                "Task_Name", "Category", "Item_Description","BillingUom","Uom","Quantity","Price","Amount","Cost_Amount","Remarks"
            };
            //using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(@"D:\Standard Template for Import-NEW.xlsx", false))
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(filePath, false))
            {

                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Row row in rows) //this will also include your header row...
                {
                    if (row.RowIndex.Value >= 11)
                    {
                        //if (GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(0)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(1)) == "" &&
                        //       GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(2)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(3)) == "" &&
                        //       GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(4)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(5)) == "" &&
                        //       GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(6)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(7)) == "" &&
                        //       GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(8)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(9)) == ""
                        //       )
                        //{
                        //    break;
                        //}
                        if (row.RowIndex.Value == 11)
                        {
                            foreach (var item in detailsColumns)
                            {
                                dt.Columns.Add(item);
                            }
                            //foreach (Cell cell in row.Descendants<Cell>())
                            //{
                            //    dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                            //}
                        }
                        else
                        {
                            //int price, amount;
                            //bool isPrice = int.TryParse(GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(6)), out price);
                            //bool isAmount = int.TryParse(GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(7)), out amount);
                            //if (GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(0)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(1)) == "" &&
                            //    GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(2)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(3)) == "" &&
                            //    GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(4)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(5)) == "" &&
                            //    GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(6)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(7)) == "" &&
                            //    GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(8)) == "" && GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(9)) == ""
                            //    )
                            //{
                            //    break;
                            //}
                            //else if (GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(0)) == "" ||
                            //    GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(2)) == "")
                            //{
                            //    continue;
                            //}
                            //else if(!isPrice || !isAmount || price.GetType() != typeof(int) ||
                            //    amount.GetType() != typeof(int))
                            //{
                            //    continue;
                            //}

                            DataRow tempRow = dt.NewRow();
                            for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                            {
                                Cell cell = row.Descendants<Cell>().ElementAt(i);
                                int index = CellReferenceToIndex(cell);
                                tempRow[index] = GetCellValue(spreadSheetDocument, cell);
                                //tempRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i - 1));
                            }
                            dt.Rows.Add(tempRow);
                        }
                    }
                }
            }
            return dt;
        }

        private static int CellReferenceToIndex(Cell cell)
        {
            int index = -1;
            string reference = cell.CellReference.ToString().ToUpper();
            foreach (char ch in reference)
            {
                if (Char.IsLetter(ch))
                {
                    int value = (int)ch - (int)'A';
                    index = (index + 1) * 26 + value;
                }
                else
                    return index;
            }
            return index;
        }

        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue != null)
            {
                string value = cell.CellValue.InnerXml;

                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                }
                else
                {
                    return value;
                }
            }
            return string.Empty;
        }

        public static bool UpsertCustomerinfoUnitandZipCode(long uid, string zipcode, string unitcode,long CountryId)
        {
            bool IsUpsert = false;
            SalesmanDropDown _GetSalesmen = new SalesmanDropDown();
            List<SelectListItem> objList = new List<SelectListItem>();
            try
            {
                if (zipcode == "")
                {
                    zipcode = null;
                }
               
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, "@Id", SqlDbType.BigInt);
                    cmd.AddParameters(zipcode, "@zipcode", SqlDbType.VarChar);
                    cmd.AddParameters(unitcode, "@unitcode", SqlDbType.VarChar);
                    cmd.AddParameters(CountryId, "@CountryId", SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_Upsert_CustomerInfoUnitandZipCode]");
                    while (Ireader.Read())
                    {
                        //objList.Add(new SelectListItem { Text = Ireader.GetString(CommonColumns.salesmen_name), Value = Ireader.GetString(CommonColumns.id) });
                        //_GetSalesmen.id = Ireader.GetInt32(CommonColumns.id);
                        //_GetSalesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                    }
                }
                return IsUpsert;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertCustomerinfoUnitandZipCode Parameters= " + uid);
                throw ex;
            }
            finally
            {
                _GetSalesmen = null;
            }
        }

        public static bool UpdateBranchForCustomer(Int32 BranchId, long CustomerId)
        {
            bool IsUpsert = false;
            string SqlCommand = "";
            try
            { 
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    SqlCommand = " Update Customer set branch_id =" + BranchId + "";
                    SqlCommand += " Where Id =" + CustomerId;
                    IsUpsert = cmd.ExecuteNonQuery(SqlCommand);                    
                }
                return IsUpsert;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpdateBranchForCustomer Parameters= " + CustomerId);
                throw ex;
            }
            finally
            {
               
            }
        }

        public static customer GetCustomerUnitandZipCode(long CustomerId)
        {
            customer Cust = new customer();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(CustomerId, "@inCustomerId", SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_CustomerDetails]");
                    while (Ireader.Read())
                    {
                        Cust.block_no = Ireader.GetString(CommonColumns.block_no);
                        Cust.job_site = Ireader.GetString(CommonColumns.job_site);
                        Cust.unit_code = Ireader.GetString(CommonColumns.unit_code);
                        Cust.zip_code = Ireader.GetInt32(CommonColumns.zip_code);
                        Cust.CountryId = Ireader.GetInt64(CommonColumns.CountryId);
                        Cust.CountryName = Ireader.GetString(CommonColumns.CountryName);
                    }
                }
                return Cust;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetCustomerUnitandZipCode Parameters= " + CustomerId);
                throw ex;
            }
            finally
            {
                Cust = null;
            }
        }

        public static ProjectsBudgetView GetProjectBudgetUnitandZipCode(int ProjectBudgetId)
        {
            ProjectsBudgetView ProjectBudget = new ProjectsBudgetView();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectBudgetId, "@inProjectBudgetId", SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_GetBudgetUnitandZipCode]");
                    while (Ireader.Read())
                    {
                        ProjectBudget.UnitCode = Ireader.GetString(CommonColumns.unit_code);
                        ProjectBudget.ZipCodeId = Ireader.GetInt32(CommonColumns.zip_code);
                    }
                }
                return ProjectBudget;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetCustomerUnitandZipCode Parameters= " + ProjectBudgetId);
                throw ex;
            }
            finally
            {
                ProjectBudget = null;
            }
        }

        public static ProjectsBudgetView GetSupplierBudget(int ProjectBudgetId)
        {
            ProjectsBudgetView ProjectBudget = new ProjectsBudgetView();
            string SqlWhere = "";
            try
            {
                using (var cmd = new DBSqlCommand(CommandType.Text))
                {
                    //cmd.AddParameters(ProjectBudgetId, "@inProjectBudgetId", SqlDbType.BigInt);
                    SqlWhere = " Select Supplier_id From projects_budget where id =" + ProjectBudgetId;
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlWhere);
                    while (Ireader.Read())
                    {
                        ProjectBudget.SupplierId = Ireader.GetInt32("Supplier_id");
                    }
                }
                return ProjectBudget;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetCustomerUnitandZipCode Parameters= " + ProjectBudgetId);
                throw ex;
            }
            finally
            {
                ProjectBudget = null;
            }
        }

        public static string GetSalesmanNameBySalesmanId(Int32 salemanid)
        {
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    salesman objCust = objDB.salesmen.Where(o => o.id == salemanid).SingleOrDefault();
                    if (objCust != null)
                    {
                        return objCust.salesmen_name.ToString();
                    }

                }
                return "";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmanNameBySalesmanId, Parameter : salemanid={salemanid}");
                return "";
            }
            finally
            {

            }

        }
        public static string GetProjectNameByProjectId(long? projectId = 0)
        {
            try
            {
                using (PMSEntities objDB = new PMSEntities())
                {
                    project objProj = objDB.projects.Where(o => o.id == projectId).SingleOrDefault();
                    if (objProj != null)
                    {
                        return objProj.project_name.ToString();
                    }

                }
                return "";
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetProjectNameByProjectId, Parameter : projectId={projectId}");
                return "";
            }
            finally
            {

            }

        }

        public static string GetSalesmenSignaturePath(int salesmenId)
        {
            string docpath = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (salesmenId > 0)
                        cmd.AddParameters(salesmenId, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);
                    else
                        cmd.AddParameters(0, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Salesmen_Signature_Path);
                    while (ireader.Read())
                    {
                        docpath = ireader.GetString(CommonColumns.Result);
                    }
                    return docpath;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenSignaturePath, Parameters =" + salesmenId.ToString());
                    throw ex;
                }
                finally
                {
                    salesmenId = 0;
                }
            }
        }

        public static bool DeleteSalesmenSignature(int salesmenId)
        {
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (salesmenId > 0)
                        cmd.AddParameters(salesmenId, CommanConstans.salesmen_id, SqlDbType.Int);
                    else
                        cmd.AddParameters(0, CommanConstans.salesmen_id, SqlDbType.Int);

                    bool result = cmd.ExecuteNonQuery(SqlProcedures.DeleteSalesmenSignature);
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: DeleteSalesmenSignature, Parameters =" + salesmenId.ToString());
                    throw ex;
                }
                finally
                {
                    salesmenId = 0;
                }
            }
        }

        public static string UpsertMasterDocument(string fileName,string file_desc,Int64  project_id,string extension,string doc_Path,string UserName)
        {
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(fileName, "@DOCUMENT_NAME", SqlDbType.NVarChar);
                    cmd.AddParameters(0, "@ACTIVITY_TYPE", SqlDbType.Int);
                    cmd.AddParameters(0, "@SUB_ACTIVITY_TYPE", SqlDbType.Int);
                    cmd.AddParameters(file_desc, "@REMARKS", SqlDbType.NVarChar);
                    cmd.AddParameters(1, "@ACTIVE_FLAG", SqlDbType.Bit);
                    cmd.AddParameters(1, "@CREATED_BY", SqlDbType.Int);
                    cmd.AddParameters(new Guid(), "@SUPER_ID", SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(11, "@ID", SqlDbType.SmallInt);
                    cmd.AddParameters(8, "@ID_TYPE", SqlDbType.SmallInt);
                    cmd.AddParameters(project_id, "@SUBDETAILS_ID", SqlDbType.BigInt);
                    cmd.AddParameters(0, "@SUBSUBDETAILS_ID", SqlDbType.Int);
                    cmd.AddParameters(extension, "@FILE_TYPE", SqlDbType.NVarChar);
                    cmd.AddParameters(0, "@DOCUMENT_CONTENT_TYPE_ID", SqlDbType.Int);
                    cmd.AddParameters(doc_Path, "@DOCUMENT_PATH", SqlDbType.VarChar);
                    cmd.AddParameters(1, "@DOC_CONFIG_ID", SqlDbType.Bit);
                    cmd.AddParameters(0, "@COMPANY_ID", SqlDbType.Int);
                    cmd.AddParameters(0, "@TaskDetailId", SqlDbType.Int);
                    cmd.AddParameters(UserName, "@UserName", SqlDbType.NVarChar);
                    cmd.ExecuteNonQuery("[dbo].[Upsert_Customer_Document]");

                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string UpsertProjectDocument(string fileName, string file_desc, Int64 project_id, string extension, string doc_Path,Guid Project_Id,string sUserName, int IdType = 0)
        {
            Guid SuperId = GetProjectGuidByprojectid(project_id.ToString());
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(fileName, "@DOCUMENT_NAME", SqlDbType.NVarChar);
                    cmd.AddParameters(0, "@ACTIVITY_TYPE", SqlDbType.Int);
                    cmd.AddParameters(0, "@SUB_ACTIVITY_TYPE", SqlDbType.Int);
                    cmd.AddParameters(file_desc, "@REMARKS", SqlDbType.NVarChar);
                    cmd.AddParameters(1, "@ACTIVE_FLAG", SqlDbType.Bit);
                    cmd.AddParameters(1, "@CREATED_BY", SqlDbType.Int);
                    cmd.AddParameters(SuperId, "@SUPER_ID", SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(11, "@ID", SqlDbType.SmallInt);
                    if (IdType == 0)
                        cmd.AddParameters(6, "@ID_TYPE", SqlDbType.SmallInt);
                    else
                        cmd.AddParameters(IdType, "@ID_TYPE", SqlDbType.SmallInt);
                    cmd.AddParameters(project_id, "@SUBDETAILS_ID", SqlDbType.BigInt);
                    cmd.AddParameters(0, "@SUBSUBDETAILS_ID", SqlDbType.Int);
                    cmd.AddParameters(extension, "@FILE_TYPE", SqlDbType.NVarChar);
                    cmd.AddParameters(0, "@DOCUMENT_CONTENT_TYPE_ID", SqlDbType.Int);
                    cmd.AddParameters(doc_Path, "@DOCUMENT_PATH", SqlDbType.VarChar);
                    cmd.AddParameters(1, "@DOC_CONFIG_ID", SqlDbType.Bit);
                    cmd.AddParameters(0, "@COMPANY_ID", SqlDbType.Int);
                    cmd.AddParameters(0, "@TaskDetailId", SqlDbType.Int);
                    cmd.AddParameters(sUserName, "@UserName", SqlDbType.NVarChar);
                    cmd.ExecuteNonQuery("[dbo].[Upsert_Contract_Document]");

                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
