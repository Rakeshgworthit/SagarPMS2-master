using PMS.Common;
using PMS.Controllers;
using PMS.Database;
using PMS.Interface;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMS.Data_Access
{
    public class DataLayer : IMaster, IPackage, IQuotation, IContract, IVariationOrder, IReceipts, IProjectsBudget, IPayments,
        IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        private string ConStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

        public List<TasksList> BindTasksList()
        {
            List<TasksList> TaskList = new List<TasksList>();
            TasksList _TaskList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader("SSP_BindTasks_MasterScreen");
                    while (Ireader.Read())
                    {
                        _TaskList = new TasksList();
                        {
                            _TaskList.task_id = Ireader.GetString(CommonColumns.task_id);
                            //_TaskList.task_cd = Ireader.GetString(CommonColumns.task_cd);
                            _TaskList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _TaskList.task_description = Ireader.GetString(CommonColumns.task_description);
                            _TaskList.Seq_No = Ireader.GetInt32("Seq_No");
                            //_TaskList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };

                        TaskList.Add(_TaskList);
                    }

                }

                return TaskList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindTasksList");
                throw ex;

            }
            finally
            {
                TaskList = null;
                _TaskList = null;
            }
        }

        public List<Top10Salesman> GetTop10Salesman(int year, int salesMenId, int BranchID)
        {
            List<Top10Salesman> top10salesmanlst = new List<Top10Salesman>();
            Top10Salesman _top10salesmanlst;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    //cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Top10Salesmen);
                    while (Ireader.Read())
                    {
                        if (top10salesmanlst.Count < 10)
                        {
                            _top10salesmanlst = new Top10Salesman();
                            {
                                _top10salesmanlst.Amount = Ireader.GetInt64(CommonColumns.Amount);
                                _top10salesmanlst.Salesman = Ireader.GetString(CommonColumns.salesmen_name);
                            };
                            top10salesmanlst.Add(_top10salesmanlst);
                        }
                        else
                        {
                            top10salesmanlst.ToList();
                        }
                    }
                }
                return top10salesmanlst;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindToptensalesman");
                throw ex;
            }
            finally
            {
                top10salesmanlst = null;
                _top10salesmanlst = null;
            }
        }

        public List<Top10Customer> GetTopTenCustomer(int year, int salesMenId, int BranchID)
        {
            List<Top10Customer> top10customerlist = new List<Top10Customer>();
            Top10Customer _top10customer;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    // cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Top10customers);
                    while (Ireader.Read())
                    {
                        if (top10customerlist.Count < 10)
                        {
                            _top10customer = new Top10Customer();
                            {
                                _top10customer.Amount = Ireader.GetInt64(CommonColumns.Amount);
                                _top10customer.name1 = Ireader.GetString(CommonColumns.name1);
                            };
                            top10customerlist.Add(_top10customer);
                        }
                        else
                        {
                            top10customerlist.ToList();
                        }
                    }
                }
                return top10customerlist;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Bindtoptencustomer");
                throw ex;
            }
            finally
            {
                top10customerlist = null;
                _top10customer = null;
            }
        }

        public List<Top10Projects> GetTopTenProjects(int year, int salesMenId, int BranchID)
        {
            List<Top10Projects> top10projectlist = new List<Top10Projects>();
            Top10Projects _top10project;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    // cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Top10Projects);
                    while (Ireader.Read())
                    {
                        if (top10projectlist.Count < 10)
                        {
                            _top10project = new Top10Projects();
                            {
                                _top10project.project_name = Ireader.GetString(CommonColumns.project_name);
                                _top10project.Amount = Ireader.GetInt64(CommonColumns.Amount);
                            };
                            top10projectlist.Add(_top10project);
                        }
                        else
                        {
                            top10projectlist.ToList();
                        }
                    }
                }
                return top10projectlist;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindToptenprojects");
                throw ex;
            }
            finally
            {
                top10projectlist = null;
                _top10project = null;
            }
        }

        public List<Top10Supplier> GetTopTenSupplier(int year, int salesMenId, int BranchID)
        {
            List<Top10Supplier> top10supplierlist = new List<Top10Supplier>();
            Top10Supplier _top10supplier;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    // cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Top10Supplier);
                    while (Ireader.Read())
                    {
                        if (top10supplierlist.Count < 10)
                        {
                            _top10supplier = new Top10Supplier();
                            {
                                _top10supplier.supplier_name = Ireader.GetString(CommonColumns.supplier_name);
                                _top10supplier.Amount = Ireader.GetInt64(CommonColumns.Amount);
                            };
                            top10supplierlist.Add(_top10supplier);
                        }
                        else
                        {
                            top10supplierlist.ToList();
                        }
                    }
                }
                return top10supplierlist;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindTasksList");
                throw ex;
            }
            finally
            {
                top10supplierlist = null;
                _top10supplier = null;
            }
        }
        public List<Dashboard> GetDetailsBasedonYear(int year, int salesMenId, int BranchID)
        {
            List<Dashboard> dashboards = new List<Dashboard>();
            Dashboard _DasBoard = new Dashboard();
            DataSet dataSet = new DataSet();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    // cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    dataSet = cmd.ExecuteDataSet(SqlProcedures.SSP_GetProjectInfoByYear);

                    _DasBoard.NoOfContracts = Convert.ToInt32(dataSet.Tables[0].Rows[0]["NoOfContracts"]);
                    _DasBoard.TotalSales = Convert.ToDecimal(dataSet.Tables[1].Rows[0]["TotalSales"]);
                    _DasBoard.CurrentMonthSales = Convert.ToDecimal(dataSet.Tables[2].Rows[0]["CurrentMonthSales"]);
                    _DasBoard.CurrentMonthContracts = Convert.ToInt32(dataSet.Tables[3].Rows[0]["CurrentMonthContracts"]);


                    dashboards.Add(_DasBoard);

                    // top10salesmanlst.ToList();


                }
                return dashboards;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindToptensalesman");
                throw ex;
            }
            finally
            {
                dashboards = null;
                _DasBoard = null;
            }
        }
        public List<GetTopTenLoan> GetTopTenLoan(int year, int salesMenId, int BranchID)
        {
            List<GetTopTenLoan> top10loans = new List<GetTopTenLoan>();
            GetTopTenLoan _top10loans;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(year, CommanConstans.year, SqlDbType.BigInt);
                    cmd.AddParameters(salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(BranchID, CommanConstans.branchId, SqlDbType.BigInt);
                    //cmd.AddParameters(Convert.ToInt64(SessionManagement.SelectedBranchID), CommanConstans.branchId, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_GetTOP10Loans);
                    while (Ireader.Read())
                    {
                        if (top10loans.Count < 10)
                        {
                            _top10loans = new GetTopTenLoan();
                            {
                                _top10loans.Address = Ireader.GetString(CommonColumns.Address);
                                _top10loans.Comission = Ireader.GetDecimal(CommonColumns.Comission);
                                _top10loans.LoanAmount = Ireader.GetDecimal(CommonColumns.LoanAmount);
                                _top10loans.loanpaid = Ireader.GetDecimal(CommonColumns.loanpaid);
                                _top10loans.repaidloan = Ireader.GetDecimal(CommonColumns.repaidloan);

                            };
                            top10loans.Add(_top10loans);
                        }
                        else
                        {
                            top10loans.ToList();
                        }
                    }
                }
                return top10loans;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindToptensalesman");
                throw ex;
            }
            finally
            {
                top10loans = null;
                _top10loans = null;
            }
        }
        public SuccessMessage CreateTask(TasksList _TasksList, string uid)
        {
            // string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_TasksList.task_id == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.task_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_TasksList.task_id, CommanConstans.task_id, SqlDbType.VarChar);
                    }
                    //cmd.AddParameters(_TasksList[0].task_cd, CommanConstans.task_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_TasksList.task_name, CommanConstans.task_name, SqlDbType.VarChar);
                    cmd.AddParameters(_TasksList.task_description, CommanConstans.task_description, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].modified_by, CommanConstans.modified_by, SqlDbType.VarChar);
                    //cmd.AddParameters(Type, CommanConstans.Type, SqlDbType.VarChar);
                    cmd.AddParameters(_TasksList.Seq_No, "@SeqNo", SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_MasterTasks);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {

                }
            }
        }

        public string DeleteTask(List<TasksList> _TasksList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_TasksList[0].task_id, CommanConstans.task_id, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].task_cd, CommanConstans.task_cd, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].task_name, CommanConstans.task_name, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].task_description, CommanConstans.task_description, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Tasks);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }
       
        public List<UOM> UOMList()
        {
            List<UOM> UOMList = new List<UOM>();
            UOM _UOMList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_UOMList);
                    while (Ireader.Read())
                    {
                        _UOMList = new UOM();
                        {
                            _UOMList.uom_id = Ireader.GetInt16(CommonColumns.uom_id);
                            _UOMList.uom_cd = Ireader.GetString(CommonColumns.uom_cd);
                            _UOMList.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            _UOMList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                            _UOMList.IsSystem = Ireader.GetBoolean(CommonColumns.IsSystem);
                        };

                        UOMList.Add(_UOMList);
                    }

                }

                return UOMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _UOMList = null;
            }
        }

        public SuccessMessage CreateUOM(UOM _UOMList, string uid)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_UOMList.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_UOMList.uom_cd, CommanConstans.uom_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_UOMList.uom_description, CommanConstans.uom_description, SqlDbType.VarChar);
                    cmd.AddParameters(_UOMList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    //cmd.AddParameters(_TasksList[0].modified_by, CommanConstans.modified_by, SqlDbType.VarChar);
                    //cmd.AddParameters(Type, CommanConstans.Type, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_UOM);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    SuccessMessage = null;
                }
            }
        }

        public string DeleteUOM(List<UOM> _UOMList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_UOMList[0].uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_UOM);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<Category> CategoryList()
        {
            List<Category> CategoryList = new List<Category>();
            Category _CategoryList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_CategoryList);
                    while (Ireader.Read())
                    {
                        _CategoryList = new Category();
                        {
                            _CategoryList.category_Id = Ireader.GetInt16(CommonColumns.category_Id);
                            _CategoryList.category_cd = Ireader.GetString(CommonColumns.category_cd);
                            _CategoryList.category_name = Ireader.GetString(CommonColumns.category_name);
                            _CategoryList.category_description = Ireader.GetString(CommonColumns.category_description);
                            _CategoryList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                            _CategoryList.Seq_No = Ireader.GetInt32("Seq_No");
                        };

                        CategoryList.Add(_CategoryList);
                    }

                }

                return CategoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _CategoryList = null;
            }
        }

        public SuccessMessage CreateCategory(Category _CategoryList, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_CategoryList.category_Id, CommanConstans.category_Id, SqlDbType.Int);
                    cmd.AddParameters(_CategoryList.category_cd, CommanConstans.category_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_CategoryList.category_name, CommanConstans.category_name, SqlDbType.VarChar);
                    cmd.AddParameters(_CategoryList.category_description, CommanConstans.category_description, SqlDbType.VarChar);
                    cmd.AddParameters(_CategoryList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_CategoryList.Seq_No, "@SeqNo", SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Category);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _CategoryList = null;
                    _successMessage = null;
                }
            }
        }

        public string DeleteCategory(List<Category> _CategoryList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_CategoryList[0].category_Id, CommanConstans.category_Id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Category);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<Floor> FloorList()
        {
            List<Floor> FloorList = new List<Floor>();
            Floor _FloorList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_FloorList);
                    while (Ireader.Read())
                    {
                        _FloorList = new Floor();
                        {
                            _FloorList.floor_id = Ireader.GetInt16(CommonColumns.floor_id);
                            _FloorList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                            _FloorList.floor_description = Ireader.GetString(CommonColumns.floor_description);
                            _FloorList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };

                        FloorList.Add(_FloorList);
                    }

                }

                return FloorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _FloorList = null;
            }
        }

        public SuccessMessage CreateFloor(Floor _FloorList, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_FloorList.floor_id, CommanConstans.floor_id, SqlDbType.Int);
                    cmd.AddParameters(_FloorList.floor_name, CommanConstans.floor_name, SqlDbType.VarChar);
                    cmd.AddParameters(_FloorList.floor_description, CommanConstans.floor_description, SqlDbType.VarChar);
                    cmd.AddParameters(_FloorList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Floor);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _FloorList = null;
                    _SuccessMessage = null;
                }
            }
        }

        public string DeleteFloor(List<Floor> _FloorList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_FloorList[0].floor_id, CommanConstans.floor_id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Floor);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<Plan> PlanList()
        {
            List<Plan> PlanList = new List<Plan>();
            Plan _PlanList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PlanList);
                    while (Ireader.Read())
                    {
                        _PlanList = new Plan();
                        {
                            _PlanList.plan_id = Ireader.GetInt16(CommonColumns.plan_id);
                            _PlanList.plan_cd = Ireader.GetString(CommonColumns.plan_cd);
                            _PlanList.plan_name = Ireader.GetString(CommonColumns.plan_name);
                            _PlanList.plan_description = Ireader.GetString(CommonColumns.plan_description);
                            _PlanList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };

                        PlanList.Add(_PlanList);
                    }

                }

                return PlanList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PlanList = null;
            }
        }

        public SuccessMessage CreatePlan(Plan _PlanList, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PlanList.plan_id, CommanConstans.plan_id, SqlDbType.Int);
                    cmd.AddParameters(_PlanList.plan_cd, CommanConstans.plan_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_PlanList.plan_name, CommanConstans.plan_name, SqlDbType.VarChar);
                    cmd.AddParameters(_PlanList.plan_description, CommanConstans.plan_description, SqlDbType.VarChar);
                    cmd.AddParameters(_PlanList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Plan);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _PlanList = null;
                    _successMessage = null;
                }
            }
        }

        public string DeletePlan(List<Plan> _PlanList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PlanList[0].plan_id, CommanConstans.plan_id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Plan);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<PackageType> PackageTypeList()
        {
            List<PackageType> PackageTypeList = new List<PackageType>();
            PackageType _PackageTypeList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTypeList);
                    while (Ireader.Read())
                    {
                        _PackageTypeList = new PackageType();
                        {
                            _PackageTypeList.package_type_id = Ireader.GetInt16(CommonColumns.package_type_id);
                            _PackageTypeList.package_name = Ireader.GetString(CommonColumns.package_name);
                            _PackageTypeList.package_description = Ireader.GetString(CommonColumns.package_description);
                            _PackageTypeList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };

                        PackageTypeList.Add(_PackageTypeList);
                    }

                }

                return PackageTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PackageTypeList = null;
            }
        }

        public SuccessMessage CreatePackageType(PackageType _PackageTypeListt, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PackageTypeListt.package_type_id, CommanConstans.package_type_id, SqlDbType.Int);
                    cmd.AddParameters(_PackageTypeListt.package_name, CommanConstans.package_name, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageTypeListt.package_description, CommanConstans.package_description, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageTypeListt.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_PackageType);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _PackageTypeListt = null;
                }
            }
        }

        public string DeletePackageType(List<PackageType> _PackageTypeList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PackageTypeList[0].package_type_id, CommanConstans.package_type_id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_PackageType);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<PropertyType> PropertyTypeList(string userId)
        {
            List<PropertyType> propertyTypeList = new List<PropertyType>();
            PropertyType _PropertyTypeList;

            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (userId == "" || userId == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.userId, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(userId, CommanConstans.userId, SqlDbType.VarChar);
                    }

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PropertyTypeList);
                    while (Ireader.Read())
                    {
                        _PropertyTypeList = new PropertyType();
                        {


                            //_ItemList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _PropertyTypeList.PropertyType_Id = Ireader.GetInt32(CommonColumns.PropertyType_Id);
                            _PropertyTypeList.Property_Type = Ireader.GetString(CommonColumns.PropertyType);
                            _PropertyTypeList.PropertyType_Desc = Ireader.GetString(CommonColumns.PropertyType_Desc);
                            _PropertyTypeList.PropertyType_Code = Ireader.GetString(CommonColumns.PropertyType_Code);
                            _PropertyTypeList.Markup_Percentage = Ireader.GetDecimal(CommonColumns.Markup_Percentage);
                            _PropertyTypeList.Is_Active = Ireader.GetBoolean(CommonColumns.Is_Active);
                            //   _ItemList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            //  _ItemList.uom_description = Ireader.GetString(CommonColumns.uom_description);

                        };

                        propertyTypeList.Add(_PropertyTypeList);
                    }

                }

                return propertyTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PropertyTypeList = null;
            }
        }
        public List<Item> ItemList(string userId)
        {
            List<Item> ItemList = new List<Item>();
            Item _ItemList;
            UOMDropDown _UOMList;
            TaskDropDown _TaskList;
            CategoryDropDown _categoryList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (userId == "" || userId == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.task_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(userId, CommanConstans.task_id, SqlDbType.VarChar);
                    }
                    IDataReader Ireader = cmd.ExecuteDataReader("SSP_BindItem_Masterscreen");  //SqlProcedures.Get_ItemList);
                    while (Ireader.Read())
                    {
                        _ItemList = new Item();
                        {

                            _UOMList = new UOMDropDown();
                            {
                                _UOMList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                _UOMList.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            };
                            _ItemList.UOM = _UOMList;
                            _TaskList = new TaskDropDown();
                            {
                                _TaskList.Task_Id = Ireader.GetString(CommonColumns.task_id);
                                _TaskList.Task_Name = Ireader.GetString(CommonColumns.task_name);
                            };
                            _ItemList.Task = _TaskList;
                            _categoryList = new CategoryDropDown();
                            {
                                _categoryList.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                _categoryList.category_name = Ireader.GetString(CommonColumns.category_name);
                            }
                            _ItemList.Category = _categoryList;
                            //_ItemList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _ItemList.item_id = Ireader.GetInt32(CommonColumns.item_id);
                            _ItemList.item_cd = Ireader.GetString(CommonColumns.item_cd);
                            _ItemList.item_description = Ireader.GetString(CommonColumns.item_description);
                            _ItemList.default_qty = Ireader.GetDecimal(CommonColumns.default_qty);
                            _ItemList.price = Ireader.GetDecimal(CommonColumns.price);
                            //   _ItemList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            //  _ItemList.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            _ItemList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };

                        ItemList.Add(_ItemList);
                    }

                }

                return ItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _ItemList = null;
            }
        }

        public SuccessMessage CreateItem(Item _ItemList, string uid)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_ItemList.Task.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_ItemList.item_id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_ItemList.item_cd, CommanConstans.item_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_ItemList.item_description, CommanConstans.item_description, SqlDbType.VarChar);
                    cmd.AddParameters(_ItemList.default_qty, CommanConstans.default_qty, SqlDbType.Decimal);
                    cmd.AddParameters(_ItemList.UOM.uom_id, CommanConstans.uom_id, SqlDbType.Int);
                    cmd.AddParameters(_ItemList.price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_ItemList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_ItemList.Category.category_Id, CommanConstans.category_Id, SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Items);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage CreatePropertyType(PropertyType _PropertyList, string uid, string mode)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PropertyList.PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(_PropertyList.PropertyType_Code, CommanConstans.PropertyType_Code, SqlDbType.NVarChar);
                    cmd.AddParameters(_PropertyList.PropertyType_Desc, CommanConstans.PropertyType_Desc, SqlDbType.NVarChar);
                    cmd.AddParameters(_PropertyList.Markup_Percentage, CommanConstans.Markup_Percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_PropertyList.Is_Active, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(_PropertyList.Property_Type, CommanConstans.PropertyType, SqlDbType.NVarChar);
                    cmd.AddParameters(mode, CommanConstans.Mode, SqlDbType.NVarChar);

                    //cmd.AddParameters(_PropertyList.UOM.uom_id, CommanConstans.uom_id, SqlDbType.Int);

                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Crud_PropertyTypes);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<ElectricalItemMapping> GetElectricalItemsMappingDetails(int PropertyType_Id, string userId)
        {
            List<ElectricalItemMapping> ElectricalItemMappingList = new List<ElectricalItemMapping>();
            ElectricalItemMapping _ElectricalItemMappingList;
            ItemDropDown itemDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    if (userId == "" || userId == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.userId, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(userId, CommanConstans.userId, SqlDbType.VarChar);
                    }
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_ElectricalItemMappingListMaster);
                    while (Ireader.Read())
                    {
                        _ElectricalItemMappingList = new ElectricalItemMapping();
                        {
                            _ElectricalItemMappingList.ElectricalItemMapping_Id = Ireader.GetInt32(CommonColumns.ElectricalItemMapping_Id);
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ElectricalItemMappingList.Item = itemDropDown;
                            _ElectricalItemMappingList.PropertyType_Id = Ireader.GetInt32(CommonColumns.PropertyType_Id);
                            _ElectricalItemMappingList.Uom_Id = Ireader.GetString(CommonColumns.uom_id);
                            _ElectricalItemMappingList.Cost_Price = Ireader.GetInt32(CommonColumns.price);
                            _ElectricalItemMappingList.Selling_Price = Ireader.GetDecimal(CommonColumns.Selling_Price);
                            _ElectricalItemMappingList.PropertyType = Ireader.GetString(CommonColumns.PropertyType);
                            _ElectricalItemMappingList.Uom_Description = Ireader.GetString(CommonColumns.uom_description);


                        };

                        ElectricalItemMappingList.Add(_ElectricalItemMappingList);
                    }

                }

                return ElectricalItemMappingList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _ElectricalItemMappingList = null;
            }
        }
        public SuccessMessage CreateElectricalItemType(ElectricalItemMapping _ElectricalItemsList, string uid, string mode)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            // ItemDropDown itemDropDown;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_ElectricalItemsList.ElectricalItemMapping_Id, CommanConstans.ElectricalItemMapping_Id, SqlDbType.Int);
                    cmd.AddParameters(_ElectricalItemsList.Item_Id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_ElectricalItemsList.Uom_Id, CommanConstans.uom_id, SqlDbType.Int);
                    cmd.AddParameters(_ElectricalItemsList.Cost_Price, CommanConstans.Cost_Price, SqlDbType.Decimal);
                    cmd.AddParameters(_ElectricalItemsList.Selling_Price, CommanConstans.Selling_Price, SqlDbType.Decimal);
                    cmd.AddParameters(_ElectricalItemsList.PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(mode, CommanConstans.Mode, SqlDbType.NVarChar);

                    //cmd.AddParameters(_PropertyList.UOM.uom_id, CommanConstans.uom_id, SqlDbType.Int);

                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Crud_ElectricalItemMapping);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public string DeleteItem(List<Item> _ItemList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_ItemList[0].item_id, CommanConstans.item_id, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Item);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<PackageList> PackagesList(string userId)
        {
            List<PackageList> PackageList = new List<PackageList>();
            PackageList _PackageList;
            PlanDropdown planDropdown;
            PackageDropdown packageDropdown;
            FloorDropdown floorDropdown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (userId == "" || userId == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.userId, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(userId, CommanConstans.userId, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PakageList);
                    while (Ireader.Read())
                    {
                        //PackageDropdown _PackageListddl = new PackageDropdown();
                        //{
                        //    _PackageListddl.package_name = Ireader.GetString(CommonColumns.package_name);
                        //}
                        _PackageList = new PackageList();
                        {
                            //_PackageList.plan_name = Ireader.GetString(CommonColumns.plan_name);                            
                            //_PackageList.package_name =Ireader.GetString(CommonColumns.package_name);
                            //_PackageList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                            _PackageList.package_cd = Ireader.GetString(CommonColumns.package_cd);
                            floorDropdown = new FloorDropdown();
                            {
                                floorDropdown.floor_id = Ireader.GetInt32(CommonColumns.floor_id);
                                floorDropdown.floor_name = Ireader.GetString(CommonColumns.floor_name);
                            };
                            _PackageList.floor = floorDropdown;
                            packageDropdown = new PackageDropdown();
                            {
                                packageDropdown.package_type_id = Ireader.GetInt32(CommonColumns.package_type_id);
                                packageDropdown.package_name = Ireader.GetString(CommonColumns.package_name);
                            };
                            _PackageList.package = packageDropdown;
                            planDropdown = new PlanDropdown();
                            {
                                planDropdown.plan_id = Ireader.GetInt32(CommonColumns.plan_id);
                                planDropdown.plan_name = Ireader.GetString(CommonColumns.plan_name);
                            };
                            _PackageList.plan = planDropdown;
                            _PackageList.valid_from = Ireader.GetDateTime(CommonColumns.valid_from);
                            _PackageList.valid_to = Ireader.GetDateTime(CommonColumns.valid_to);
                            _PackageList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                            _PackageList.package_id = Ireader.GetString(CommonColumns.package_id);
                            _PackageList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                        };


                        PackageList.Add(_PackageList);
                    }

                }

                return PackageList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PackageList = null;
                planDropdown = null;
                floorDropdown = null;
                packageDropdown = null;
            }
        }

        public SuccessMessage CreatePackage(PackageList _PackageList)
        {
            string result = string.Empty;

            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_PackageList.package_id == null || _PackageList.package_id == "0")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_id, SqlDbType.VarChar);
                        cmd.AddParameters(false, CommanConstans.isactive, SqlDbType.Bit);
                        cmd.AddParameters("", CommanConstans.package_cd, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_PackageList.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                        cmd.AddParameters(_PackageList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                        cmd.AddParameters(_PackageList.package_cd, CommanConstans.package_cd, SqlDbType.VarChar);

                    }
                    cmd.AddParameters(_PackageList.plan.plan_id, CommanConstans.plan_id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.package.package_type_id, CommanConstans.package_type_id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.floor.floor_id, CommanConstans.floor_id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.valid_from, CommanConstans.valid_from, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.valid_to, CommanConstans.valid_to, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_PackageList.userid, CommanConstans.userId, SqlDbType.VarChar);
                    if (!_PackageList.isGlobalpkg)
                        cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    else
                        cmd.AddParameters(0, CommanConstans.branch_id, SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Upsert_Package);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.PackageID);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public string DeletePackage(List<PackageList> _PackageList, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_PackageList[0].package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Package);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<UOMDropDown> UOMDropDownList()
        {
            List<UOMDropDown> UOMList = new List<UOMDropDown>();
            UOMDropDown _UOMList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_UOMList);
                    while (Ireader.Read())
                    {
                        _UOMList = new UOMDropDown();
                        {
                            _UOMList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            _UOMList.uom_description = Ireader.GetString(CommonColumns.uom_description);
                        };

                        UOMList.Add(_UOMList);
                    }

                }

                return UOMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _UOMList = null;
            }
        }

        public List<TaskDropDown> TaskDropDownList()
        {
            List<TaskDropDown> TaskList = new List<TaskDropDown>();
            TaskDropDown _TaskList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_TasksList);
                    while (Ireader.Read())
                    {
                        _TaskList = new TaskDropDown();
                        {
                            _TaskList.Task_Id = Ireader.GetString(CommonColumns.task_id);
                            _TaskList.Task_Name = Ireader.GetString(CommonColumns.task_name);
                        };

                        TaskList.Add(_TaskList);
                    }

                }

                return TaskList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _TaskList = null;
            }
        }

        public List<FloorDropdown> FloorDropDownList()
        {
            List<FloorDropdown> FloorList = new List<FloorDropdown>();
            FloorDropdown _FloorList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindFloor);
                    while (Ireader.Read())
                    {
                        _FloorList = new FloorDropdown();
                        {
                            _FloorList.floor_id = Ireader.GetInt32(CommonColumns.floor_id);
                            _FloorList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                        };

                        FloorList.Add(_FloorList);
                    }

                }

                return FloorList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _FloorList = null;
            }
        }

        public List<CategoryDropDown> CategoryDropDownList()
        {
            List<CategoryDropDown> CategoryList = new List<CategoryDropDown>();
            CategoryDropDown _CategoryList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_CategoryList);
                    while (Ireader.Read())
                    {
                        _CategoryList = new CategoryDropDown();
                        {
                            _CategoryList.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                            _CategoryList.category_name = Ireader.GetString(CommonColumns.category_name);
                        };

                        CategoryList.Add(_CategoryList);
                    }

                }

                return CategoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _CategoryList = null;
            }
        }

        public List<ItemDropDown> ItemDropDownList(string task_id)
        {
            List<ItemDropDown> ItemList = new List<ItemDropDown>();
            ItemDropDown _ItemList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (task_id == null || task_id == "" || task_id == "0")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.task_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(task_id, CommanConstans.task_id, SqlDbType.VarChar);

                    }
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ItemList);
                    while (Ireader.Read())
                    {
                        _ItemList = new ItemDropDown();
                        {
                            _ItemList.item_id = Ireader.GetInt32(CommonColumns.Item_Id);
                            _ItemList.item_description = Ireader.GetString(CommonColumns.Item_Description);
                        };

                        ItemList.Add(_ItemList);
                    }

                }

                return ItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _ItemList = null;
            }
        }

        public List<ItemDropDown> ElectricalItemDropDownList(int PropertyType_Id)
        {
            List<ItemDropDown> ItemList = new List<ItemDropDown>();
            ItemDropDown _ItemList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ElectricalItemList);

                    while (Ireader.Read())
                    {
                        _ItemList = new ItemDropDown();
                        {
                            _ItemList.item_id = Ireader.GetInt32(CommonColumns.Item_Id);
                            _ItemList.item_description = Ireader.GetString(CommonColumns.Item_Description);
                        };

                        ItemList.Add(_ItemList);
                    }

                }

                return ItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _ItemList = null;
            }
        }

        public List<PackageDropdown> PackageDropDownList()
        {
            List<PackageDropdown> PackageTypeList = new List<PackageDropdown>();
            PackageDropdown _PackageTypeList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindPackageType);
                    while (Ireader.Read())
                    {
                        _PackageTypeList = new PackageDropdown();
                        {
                            _PackageTypeList.package_type_id = Ireader.GetInt32(CommonColumns.package_type_id);
                            _PackageTypeList.package_name = Ireader.GetString(CommonColumns.package_name);
                        };

                        PackageTypeList.Add(_PackageTypeList);
                    }

                }

                return PackageTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PackageTypeList = null;
            }
        }

        public List<PlanDropdown> PlanDropDownList()
        {
            List<PlanDropdown> PlanList = new List<PlanDropdown>();
            PlanDropdown _PlanList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindPlans);
                    while (Ireader.Read())
                    {
                        _PlanList = new PlanDropdown();
                        {
                            _PlanList.plan_id = Ireader.GetInt32(CommonColumns.plan_id);
                            _PlanList.plan_name = Ireader.GetString(CommonColumns.plan_name);
                        };

                        PlanList.Add(_PlanList);
                    }

                }

                return PlanList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _PlanList = null;
            }
        }

        public List<QuotationList> QuotationsList(QuotationListCriteria _QuotationListCriteria)
        {
            List<QuotationList> QuotationList = new List<QuotationList>();
            QuotationList _QuotationList;
            SalesmanDropDown salesmanDropDown;
            CustomerDropDown customerDropDown;
            StatusLookup statusDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.UserID, SqlDbType.VarChar);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branchId, SqlDbType.Int);
                    // cmd.AddParameters(_QuotationListCriteria.startRowIndex, CommanConstans.startRowIndex, SqlDbType.Int);
                    //cmd.AddParameters(_QuotationListCriteria.pageSize, CommanConstans.pageSize, SqlDbType.Int);
                    cmd.AddParameters(_QuotationListCriteria.ColSort, CommanConstans.ColSort, SqlDbType.Int);
                    cmd.AddParameters(_QuotationListCriteria.OrderBy, CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationListCriteria.fromdate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(_QuotationListCriteria.todate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(_QuotationListCriteria.projectStatus, CommanConstans.projectStatus, SqlDbType.Int);
                    cmd.AddParameters(_QuotationListCriteria.salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(_QuotationListCriteria.searchText, CommanConstans.searchText, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationListCriteria.Type, CommanConstans.Type, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectList);
                    while (Ireader.Read())
                    {
                        _QuotationList = new QuotationList();
                        {
                            //_CustomerDropdown = new CustomerDropdown();
                            //{
                            //    _CustomerDropdown.Customer_id = Ireader.GetInt32(CommonColumns.customer_id);
                            //    _CustomerDropdown.name1 = Ireader.GetString(CommonColumns.name1);
                            //};
                            //_QuotationList.Customer = _CustomerDropdown;
                            //_SalesmenDropdown = new SalesmenDropdown();
                            //{
                            //    _SalesmenDropdown.salesmen_id = Ireader.GetInt32(CommonColumns.salesmen_id);
                            //    _SalesmenDropdown.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            //};
                            //_QuotationList.Salesmen = _SalesmenDropdown;

                            //_QuotationList.id = Ireader.GetString(CommonColumns.id);
                            _QuotationList.project_id = Ireader.GetString(CommonColumns.project_id);
                            _QuotationList.Id = Ireader.GetInt64(CommonColumns.id);
                            _QuotationList.project_number = Ireader.GetString(CommonColumns.project_number);
                            _QuotationList.project_name = Ireader.GetString(CommonColumns.project_name);
                            _QuotationList.contract_date = Ireader.GetDateTime(CommonColumns.contract_date);
                            customerDropDown = new CustomerDropDown();
                            {
                                customerDropDown.Customer_id = Ireader.GetInt32(CommonColumns.customer_id);
                                customerDropDown.name1 = Ireader.GetString(CommonColumns.name1);
                            };
                            _QuotationList.Customer = customerDropDown;
                            salesmanDropDown = new SalesmanDropDown();
                            {
                                salesmanDropDown.id = Ireader.GetInt32(CommonColumns.salesmen_id);
                                salesmanDropDown.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            };
                            _QuotationList.Salesmen = salesmanDropDown;
                            //_QuotationList.name1 = Ireader.GetString(CommonColumns.name1);
                            //_QuotationList.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _QuotationList.contract_amount = Ireader.GetDecimal(CommonColumns.contract_amount);
                            _QuotationList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                            statusDropDown = new StatusLookup();
                            {
                                statusDropDown.description = Ireader.GetString(CommonColumns.project_status);
                                statusDropDown.status_lookup_id = Ireader.GetInt32(CommonColumns.status_id);
                            };
                            _QuotationList.Status = statusDropDown;
                            _QuotationList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                            

                        };
                        QuotationList.Add(_QuotationList);
                    }

                }

                return QuotationList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: QuotationsList QuotationListCriteria=" + _QuotationListCriteria.ToString());
                throw ex;
            }
            finally
            {
                QuotationList = null;
                _QuotationListCriteria = null;
                _QuotationList = null;
            }
        }
        public SuccessMessage UpsertProjectQuotation(CreateQuotationCriteria _CreateQuotationCriteria, string uid)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_CreateQuotationCriteria.project_id == null || _CreateQuotationCriteria.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters("", CommanConstans.project_number, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters(_CreateQuotationCriteria.project_number, CommanConstans.project_number, SqlDbType.VarChar);
                    }
                    //if (_CreateQuotationCriteria.name1 == null)//Customer_id
                    //{
                    //    _CreateQuotationCriteria.name1 = "0";
                    //}
                    //else
                    //{
                    //    _CreateQuotationCriteria.name1 = _CreateQuotationCriteria.name1;
                    //}
                    //if (_CreateQuotationCriteria.salesmen_name == null)//salesmen_id
                    //{
                    //    _CreateQuotationCriteria.salesmen_name = "0";
                    //}
                    //else
                    //{
                    //    _CreateQuotationCriteria.salesmen_name = _CreateQuotationCriteria.salesmen_name;
                    //}
                    cmd.AddParameters(_CreateQuotationCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);

                    if (_CreateQuotationCriteria.Customer_id != null && _CreateQuotationCriteria.salesmen_id != null)
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.Salesmen.id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    //cmd.AddParameters(_CreateQuotationCriteria[0].bank_id, CommanConstans.bank_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_date, CommanConstans.quotationForwardDate, SqlDbType.DateTime);
                    //cmd.AddParameters(_CreateQuotationCriteria[0].quotationAcceptDate, CommanConstans.quotationAcceptDate, SqlDbType.DateTime);
                    cmd.AddParameters(1, CommanConstans.is_new_record, SqlDbType.Bit);
                    cmd.AddParameters(_CreateQuotationCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.Package_Type_Id, CommanConstans.Package_Type_Id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_amount, CommanConstans.contract_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount, CommanConstans.discount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount_percentage, CommanConstans.discount_percentage, SqlDbType.Float);
                    cmd.AddParameters(_CreateQuotationCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Quotation);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.project_id);
                        //if(_SuccessMessage.Result == "1")
                        //{
                        //    ireader.Close();
                        //    cmd.AddParameters(_SuccessMessage.Id, CommanConstans.project_id, SqlDbType.VarChar);
                        //    cmd.AddParameters(_CreateQuotationCriteria.version_no, CommanConstans.version_no, SqlDbType.Int);
                        //    IDataReader ireader_history = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Quotation_History);
                        //    _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);

                        //}
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation, Parameters =" + _CreateQuotationCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _CreateQuotationCriteria = null;
                }
            }
        }

        //added by Rakesh
        public SuccessMessage UpsertProjectContractQuotation(CreateQuotationCriteria _CreateQuotationCriteria, string uid)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_CreateQuotationCriteria.project_id == null || _CreateQuotationCriteria.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters("", CommanConstans.project_number, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters(_CreateQuotationCriteria.project_number, CommanConstans.project_number, SqlDbType.VarChar);
                    }
                    //if (_CreateQuotationCriteria.name1 == null)//Customer_id
                    //{
                    //    _CreateQuotationCriteria.name1 = "0";
                    //}
                    //else
                    //{
                    //    _CreateQuotationCriteria.name1 = _CreateQuotationCriteria.name1;
                    //}
                    //if (_CreateQuotationCriteria.salesmen_name == null)//salesmen_id
                    //{
                    //    _CreateQuotationCriteria.salesmen_name = "0";
                    //}
                    //else
                    //{
                    //    _CreateQuotationCriteria.salesmen_name = _CreateQuotationCriteria.salesmen_name;
                    //}
                    cmd.AddParameters(_CreateQuotationCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);

                    if (_CreateQuotationCriteria.Customer_id != null && _CreateQuotationCriteria.salesmen_id != null)
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.Salesmen.id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    //cmd.AddParameters(_CreateQuotationCriteria[0].bank_id, CommanConstans.bank_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_date, CommanConstans.contract_date, SqlDbType.DateTime);
                    //cmd.AddParameters(_CreateQuotationCriteria[0].quotationAcceptDate, CommanConstans.quotationAcceptDate, SqlDbType.DateTime);
                    cmd.AddParameters(1, CommanConstans.is_new_record, SqlDbType.Bit);
                    cmd.AddParameters(_CreateQuotationCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.Package_Type_Id, CommanConstans.Package_Type_Id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_amount, CommanConstans.contract_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount, CommanConstans.discount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount_percentage, CommanConstans.discount_percentage, SqlDbType.Float);
                    cmd.AddParameters(_CreateQuotationCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_ContractQuotation);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.project_id);
                        //if(_SuccessMessage.Result == "1")
                        //{
                        //    ireader.Close();
                        //    cmd.AddParameters(_SuccessMessage.Id, CommanConstans.project_id, SqlDbType.VarChar);
                        //    cmd.AddParameters(_CreateQuotationCriteria.version_no, CommanConstans.version_no, SqlDbType.Int);
                        //    IDataReader ireader_history = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Quotation_History);
                        //    _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);

                        //}
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectContractQuotation, Parameters =" + _CreateQuotationCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _CreateQuotationCriteria = null;
                }
            }
        }

        public QuotationFromPackageResponse UpsertQuotationFromPackage(QuotationFromPackageCriteria _QuotationFromPackageCriteria, string uid)
        {
            //List<QuotationFromPackageResponse> QuotationList = new List<QuotationFromPackageResponse>();
            QuotationFromPackageResponse _QuotationFromPackageResponse = new QuotationFromPackageResponse();
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_QuotationFromPackageCriteria.PackageId == null || _QuotationFromPackageCriteria.PackageId == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.PackageId, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_QuotationFromPackageCriteria.PackageId, CommanConstans.PackageId, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_QuotationFromPackageCriteria.SalesMenid, CommanConstans.SalesMenid, SqlDbType.BigInt);
                    cmd.AddParameters(_QuotationFromPackageCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationFromPackageCriteria.Project_number, CommanConstans.Project_number, SqlDbType.VarChar);
                    //cmd.AddParameters(_QuotationFromPackageCriteria.project_name, CommanConstans.project_name, SqlDbType.BigInt);
                    cmd.AddParameters(_QuotationFromPackageCriteria.branch_id, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(_QuotationFromPackageCriteria.customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    cmd.AddParameters(_QuotationFromPackageCriteria.bank_id, CommanConstans.bank_id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationFromPackageCriteria.quotationForwardDate, CommanConstans.quotationForwardDate, SqlDbType.DateTime);
                    cmd.AddParameters(_QuotationFromPackageCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationFromPackageCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Quotation_FROM_Package);
                    while (ireader.Read())
                    {
                        _QuotationFromPackageResponse.result = ireader.GetString(CommonColumns.Result);
                        _QuotationFromPackageResponse.projectId = ireader.GetString(CommonColumns.Project_id);
                        _QuotationFromPackageResponse.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _QuotationFromPackageResponse;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertQuotationFromPackage, Parameters =" + _QuotationFromPackageCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _QuotationFromPackageCriteria = null;
                }
            }
        }

        public bool UpsertAdditionDescription(string description, string Id)
        {
            //string result = string.Empty;
            string SqlCommand = "";
            bool bRetval = false;
            using (var cmd = new DBSqlCommand())
            {
                try
                {

                    cmd.AddParameters(description, "@inDescription", SqlDbType.NVarChar);
                    cmd.AddParameters(Id, "@inId", SqlDbType.NVarChar);
                    bRetval = cmd.ExecuteNonQuery(SqlProcedures.UpsertAdditionalDescription);
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertAdditionDescription, Parameters =" + description);
                    throw ex;
                }
                finally
                {
                    Common.Constants.AdditionalDescription = "";
                }
                return bRetval;
            }
        }

        public string  GetAdditionDescription(string Id)
        {
            //string result = string.Empty;
            string SqlCommand = "";
            string AdditionalDescription = "";

            using (var cmd = new DBSqlCommand(CommandType.Text))
            {
                try
                {
                    SqlCommand = "Select AdditionalDescription from Project_Details where project_det_Id = '" + Id + "'";

                    IDataReader ireader = cmd.ExecuteDataReader(SqlCommand);
                    while (ireader.Read())
                    {
                        AdditionalDescription = ireader.GetString("AdditionalDescription");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertAdditionDescription");
                    throw ex;
                }
                finally
                {
                    Common.Constants.AdditionalDescription = "";
                }
                return AdditionalDescription;
            }
        }

        public SuccessMessage UpsertProjectDetails(QuotationUpsertProjectDetails _QuotationUpsertProjectDetails)
        {
            //string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            if (_QuotationUpsertProjectDetails.AdditionalDescription == null)
            {
                _QuotationUpsertProjectDetails.AdditionalDescription = "";
            }
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_QuotationUpsertProjectDetails.project_det_Id == null || _QuotationUpsertProjectDetails.project_det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_QuotationUpsertProjectDetails.project_det_Id, CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    if (_QuotationUpsertProjectDetails.project_id == null || _QuotationUpsertProjectDetails.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_QuotationUpsertProjectDetails.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    }
                    // cmd.AddParameters(_QuotationUpsertProjectDetails.project_det_Id, CommanConstans.project_det_Id, SqlDbType.VarChar);
                    // cmd.AddParameters(_QuotationUpsertProjectDetails.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.item_remarks, CommanConstans.item_remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.BillingUOM.status_lookup_id, CommanConstans.price_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Item.item_id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Task_Name, CommanConstans.task_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Item.item_description, CommanConstans.item_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Category.category_Id, CommanConstans.category_id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.UOM.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.qty, CommanConstans.qty, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Cost_Amount == null ? Convert.ToDecimal(0) : _QuotationUpsertProjectDetails.Cost_Amount, CommanConstans.Cost_Amount, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Profit_Loss == null ? Convert.ToDecimal(0) : _QuotationUpsertProjectDetails.Profit_Loss, CommanConstans.Profit_Loss, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.userId, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Category.category_name, CommanConstans.category_name, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Highlight, CommanConstans.Highlight, SqlDbType.Bit);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.AdditionalDescription, CommanConstans.AdditionalDescription, SqlDbType.NVarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_ProjectDetails);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectDetails, Parameters =" + _QuotationUpsertProjectDetails.ToString());
                    throw ex;
                }
                finally
                {
                    //result = string.Empty;
                    _QuotationUpsertProjectDetails = null;
                }
            }
        }
        public SuccessMessage UpsertPackageDetails(PackageTasksItemList _UpsertPackageDetailsCriteria)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    //ExceptionLog.WriteDebugLog("UpsertPackageDetails-DataLayer", "1");
                    if (_UpsertPackageDetailsCriteria.Package_Det_Id == null || _UpsertPackageDetailsCriteria.Package_Det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_UpsertPackageDetailsCriteria.Package_Det_Id, CommanConstans.package_det_id, SqlDbType.VarChar);
                    }
                    if (_UpsertPackageDetailsCriteria.Package_Id == null || _UpsertPackageDetailsCriteria.Package_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_UpsertPackageDetailsCriteria.Package_Id, CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    //cmd.AddParameters(_UpsertPackageDetailsCriteria.Package_Det_Id, CommanConstans.package_det_id, SqlDbType.VarChar);
                    //cmd.AddParameters(_UpsertPackageDetailsCriteria.Package_Id, CommanConstans.package_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Item.item_description, CommanConstans.item_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Item.item_id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Category.category_name, CommanConstans.category_name, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Category.category_Id, CommanConstans.category_id, SqlDbType.Int);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.BillingUOM.status_lookup_id, CommanConstans.price_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.UOM.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Qty, CommanConstans.qty, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.userid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.item_remarks, CommanConstans.item_remarks, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Task_Name, CommanConstans.task_description, SqlDbType.NVarChar);
                    // ExceptionLog.WriteDebugLog("UpsertPackageDetails-DataLayer", "2");
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_packageDetails);
                    // ExceptionLog.WriteDebugLog("UpsertPackageDetails-DataLayer", "3");
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                    }
                    return _SuccessMessage;

                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackageDetails, Parameters =" + _UpsertPackageDetailsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _UpsertPackageDetailsCriteria = null;
                }
            }
        }
        public List<ProjectTasksList> GetProjectTasks(ProjectTasksCriteria _ProjectTasksCriteria)
        {
            List<ProjectTasksList> ProjectTasksList = new List<ProjectTasksList>();
            ProjectTasksList _ProjectTasksList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_ProjectTasksCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasks);
                    while (Ireader.Read())
                    {
                        _ProjectTasksList = new ProjectTasksList();
                        {
                            _ProjectTasksList.project_id = Ireader.GetString(CommonColumns.project_id);
                            _ProjectTasksList.task_id = Ireader.GetString(CommonColumns.task_id);
                            _ProjectTasksList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _ProjectTasksList.TaskAmount = Ireader.GetString(CommonColumns.TaskAmount);
                        };
                        ProjectTasksList.Add(_ProjectTasksList);
                    }

                }

                return ProjectTasksList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasks Parameters=" + _ProjectTasksCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksList = null;
                _ProjectTasksCriteria = null;
                _ProjectTasksList = null;
            }
        }
        public List<PackageTasksList> GetPackageTasks(PackageTasksCriteria _PackageTasksCriteria)
        {
            List<PackageTasksList> PackageTasksList = new List<PackageTasksList>();
            PackageTasksList _PackageTasksList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_PackageTasksCriteria.Package_Id, CommanConstans.Package_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageTasksCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTasks);
                    while (Ireader.Read())
                    {
                        _PackageTasksList = new PackageTasksList();
                        {
                            _PackageTasksList.package_id = Ireader.GetString(CommonColumns.Package_Id);
                            _PackageTasksList.task_id = Ireader.GetString(CommonColumns.task_id);
                            _PackageTasksList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _PackageTasksList.TaskAmount = Ireader.GetString(CommonColumns.TaskAmount);
                        };
                        PackageTasksList.Add(_PackageTasksList);
                    }

                }

                return PackageTasksList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasks Parameters=" + _PackageTasksCriteria.ToString());
                throw ex;
            }
            finally
            {
                PackageTasksList = null;
                _PackageTasksCriteria = null;
                _PackageTasksList = null;
            }
        }
        public string UpsertPackage(UpsertPackageCriteria _UpsertPackageCriteria, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_UpsertPackageCriteria.package_id == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_UpsertPackageCriteria.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_UpsertPackageCriteria.package_cd, CommanConstans.package_cd, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageCriteria.valid_from, CommanConstans.valid_from, SqlDbType.DateTime);
                    cmd.AddParameters(_UpsertPackageCriteria.valid_to, CommanConstans.valid_to, SqlDbType.DateTime);
                    cmd.AddParameters(_UpsertPackageCriteria.discount_amount, CommanConstans.discount_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertPackageCriteria.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_UpsertPackageCriteria.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userid, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageCriteria.package_type_id, CommanConstans.package_type_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_UpsertPackageCriteria.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Upsert_Package);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackage Parameters=" + _UpsertPackageCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _UpsertPackageCriteria = null;
                }
            }
        }
        public List<ProjectAmountList> ProjectAmountList(ProjectTasksCriteria _ProjectAmountCriteria)
        {
            List<ProjectAmountList> ProjectAmountList = new List<ProjectAmountList>();
            ProjectAmountList _ProjectAmountList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectAmountCriteria.project_Id == null)
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_ProjectAmountCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    }
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Project_Amount);
                    while (Ireader.Read())
                    {
                        _ProjectAmountList = new ProjectAmountList();
                        {
                            _ProjectAmountList.project_number = Ireader.GetString(CommonColumns.project_number);
                            _ProjectAmountList.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _ProjectAmountList.CUstomerName = Ireader.GetString(CommonColumns.CUstomerName);
                            _ProjectAmountList.email = Ireader.GetString(CommonColumns.email);
                            _ProjectAmountList.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
                            _ProjectAmountList.project_status = Ireader.GetString(CommonColumns.project_status);
                            _ProjectAmountList.quotationForwardDate = Ireader.GetDateTime(CommonColumns.quotationForwardDate);
                            _ProjectAmountList.contract_date = Ireader.GetDateTime(CommonColumns.contract_date);
                            _ProjectAmountList.ActualAmount = Ireader.GetString(CommonColumns.ActualAmount);
                            _ProjectAmountList.discount = Ireader.GetString(CommonColumns.discount);
                            _ProjectAmountList.Subtotal = Ireader.GetString(CommonColumns.Subtotal);
                            _ProjectAmountList.gst_amount = Ireader.GetString(CommonColumns.gst_amount);
                            _ProjectAmountList.GrandTOtal = Ireader.GetString(CommonColumns.GrandTOtal);


                        };
                        ProjectAmountList.Add(_ProjectAmountList);
                    }

                }

                return ProjectAmountList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ProjectAmountList _ProjectAmountCriteria=" + _ProjectAmountCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectAmountList = null;
                _ProjectAmountCriteria = null;
                _ProjectAmountList = null;
            }
        }
        public string UpsertMasterContractTerms(MasterContractTermsCriteria _MasterContractTermsCriteria, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_MasterContractTermsCriteria.master_contract_term_id, CommanConstans.master_contract_term_id, SqlDbType.Int);
                    cmd.AddParameters(_MasterContractTermsCriteria.contract_desrcription, CommanConstans.contract_desrcription, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_master_contract_terms);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertMasterContractTerms Parameters=" + _MasterContractTermsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _MasterContractTermsCriteria = null;
                }
            }
        }
        public string UpsertMasterPaymentTerms(MasterPaymentTermsCriteria _MasterPaymentTermsCriteria, string uid)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_MasterPaymentTermsCriteria.master_payment_termid, CommanConstans.master_payment_termid, SqlDbType.Int);
                    cmd.AddParameters(_MasterPaymentTermsCriteria.description, CommanConstans.description, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Master_payment_terms);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertMasterPaymentTerms Parameters=" + _MasterPaymentTermsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _MasterPaymentTermsCriteria = null;
                }
            }
        }
        public SuccessMessage UpsertProjectContractTerms(ProjectContractTermsCriteria _ProjectContractTermsCriteria, string uid, string ProjectId)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectContractTermsCriteria.contract_term_id, CommanConstans.contract_term_id, SqlDbType.Int);
                    cmd.AddParameters(_ProjectContractTermsCriteria.contract_desrcription, CommanConstans.contract_desrcription, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectContractTermsCriteria.master_contract_term_id, CommanConstans.master_contract_term_id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_project_contract_terms);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectContractTerms Parameters=" + _ProjectContractTermsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _ProjectContractTermsCriteria = null;
                }
            }
        }
        public SuccessMessage UpsertProjectPaymentTerms(PaymentTerms _ProjectPaymentTermsCriteria, string uid, string ProjectId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_ProjectPaymentTermsCriteria.payment_term_id == null || _ProjectPaymentTermsCriteria.payment_term_id == "")
                    {
                        cmd.AddParameters(0, CommanConstans.payment_term_id, SqlDbType.Int);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectPaymentTermsCriteria.payment_term_id, CommanConstans.payment_term_id, SqlDbType.Int);

                    }
                    if (_ProjectPaymentTermsCriteria.paymentdescription.Master_payment_term_id == 0 || _ProjectPaymentTermsCriteria.paymentdescription.Master_payment_term_id == null)
                    {
                        cmd.AddParameters(null, CommanConstans.master_payment_termid, SqlDbType.Int);

                    }
                    else
                    {
                        cmd.AddParameters(Convert.ToInt32(_ProjectPaymentTermsCriteria.paymentdescription.Master_payment_term_id), CommanConstans.master_payment_termid, SqlDbType.Int);
                    }
                    cmd.AddParameters(ProjectId, CommanConstans.project_Id, SqlDbType.VarChar);
                    //cmd.AddParameters(_ProjectPaymentTermsCriteria[0].description, CommanConstans.description, SqlDbType.VarChar);
                    // cmd.AddParameters(Convert.ToInt32(_ProjectPaymentTermsCriteria.paymentdescription.Master_payment_term_id), CommanConstans.master_payment_termid, SqlDbType.Int);
                    cmd.AddParameters(_ProjectPaymentTermsCriteria.paymentdescription.Master_payment_description, CommanConstans.description, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_payment_terms);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectPaymentTerms Parameters=" + _ProjectPaymentTermsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _ProjectPaymentTermsCriteria = null;
                }
            }
        }
        public List<TasksList> GetMasterTasksList()
        {
            List<TasksList> MasterTasksList = new List<TasksList>();
            TasksList _MasterTasksList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_MasterTasksList);
                    while (Ireader.Read())
                    {
                        _MasterTasksList = new TasksList();
                        {
                            _MasterTasksList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _MasterTasksList.task_description = Ireader.GetString(CommonColumns.task_description);
                        };

                        MasterTasksList.Add(_MasterTasksList);
                    }

                }

                return MasterTasksList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetMasterTasksList");
                throw ex;
            }
            finally
            {
                MasterTasksList = null;
                _MasterTasksList = null;
            }
        }
        public List<TasksList> GetTasksbytaskid(TasksList _GetTasksByTaskId)
        {
            List<TasksList> GetTasksList = new List<TasksList>();
            TasksList _GetTasksList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_GetTasksByTaskId.task_id, CommanConstans.task_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Tasksbytaskid);
                    while (Ireader.Read())
                    {
                        _GetTasksList = new TasksList();
                        {
                            //_GetTasksList.task_cd = Ireader.GetString(CommonColumns.task_cd);
                            _GetTasksList.task_name = Ireader.GetString(CommonColumns.task_name);
                            _GetTasksList.task_description = Ireader.GetString(CommonColumns.task_description);
                        };
                        GetTasksList.Add(_GetTasksList);
                    }

                }

                return GetTasksList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetTasksbytaskid Parameters= " + _GetTasksByTaskId.ToString());
                throw ex;
            }
            finally
            {
                GetTasksList = null;
                _GetTasksList = null;
            }
        }
        public List<Item> GetItemByItemId(Int32 ItemId)
        {
            List<Item> GetItemList = new List<Item>();
            Item _GetItemList;
            TaskDropDown taskDropDown;
            UOMDropDown uOMDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ItemId, CommanConstans.item_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ItemByItemId);
                    while (Ireader.Read())
                    {
                        _GetItemList = new Item();
                        {
                            _GetItemList.item_cd = Ireader.GetString(CommonColumns.item_cd);
                            _GetItemList.item_description = Ireader.GetString(CommonColumns.item_description);
                            _GetItemList.default_qty = Ireader.GetDecimal(CommonColumns.default_qty);
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _GetItemList.Task = taskDropDown;
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            };
                            _GetItemList.UOM = uOMDropDown;
                            //_GetItemList.Task.Task_Id = Ireader.GetString(CommonColumns.task_id);
                            //_GetItemList.UOM.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            _GetItemList.price = Ireader.GetDecimal(CommonColumns.price);
                        };
                        GetItemList.Add(_GetItemList);
                    }

                }

                return GetItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetItemByItemId");
                throw ex;
            }
            finally
            {
                GetItemList = null;
                _GetItemList = null;
                taskDropDown = null;
                uOMDropDown = null;
            }
        }
        public List<Plan> GetPlansByPlanId(Plan _GetPlansByPlanId)
        {
            List<Plan> GetPlanList = new List<Plan>();
            Plan _GetPlanList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_GetPlansByPlanId.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PlansByPlanId);
                    while (Ireader.Read())
                    {
                        _GetPlanList = new Plan();
                        {
                            _GetPlanList.plan_description = Ireader.GetString(CommonColumns.plan_description);
                            _GetPlanList.plan_name = Ireader.GetString(CommonColumns.plan_name);
                            _GetPlanList.plan_cd = Ireader.GetString(CommonColumns.plan_cd);
                        };
                        GetPlanList.Add(_GetPlanList);
                    }

                }

                return GetPlanList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPlansByPlanId Parameters= " + _GetPlansByPlanId.ToString());
                throw ex;
            }
            finally
            {
                GetPlanList = null;
                _GetPlanList = null;
            }
        }
        public List<UOM> GetUomByUomId(UOM _GetUomByUomId)
        {
            List<UOM> GetUOMList = new List<UOM>();
            UOM _GetUOMList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_GetUomByUomId.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_UomByUomId);
                    while (Ireader.Read())
                    {
                        _GetUOMList = new UOM();
                        {
                            _GetUOMList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            _GetUOMList.uom_cd = Ireader.GetString(CommonColumns.uom_cd);
                            _GetUOMList.uom_description = Ireader.GetString(CommonColumns.uom_description);
                        };
                        GetUOMList.Add(_GetUOMList);
                    }

                }

                return GetUOMList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetUomByUomId Parameters= " + _GetUomByUomId.ToString());
                throw ex;
            }
            finally
            {
                GetUOMList = null;
                _GetUOMList = null;
            }
        }

        public List<Floor> GetFloorByFloorId(Floor _GetFloorByFloorId)
        {
            List<Floor> GetFloorList = new List<Floor>();
            Floor _GetFloorList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_GetFloorByFloorId.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_FloorByFloorId);
                    while (Ireader.Read())
                    {
                        _GetFloorList = new Floor();
                        {
                            _GetFloorList.floor_id = Ireader.GetInt32(CommonColumns.floor_id);
                            _GetFloorList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                            _GetFloorList.floor_description = Ireader.GetString(CommonColumns.floor_description);
                        };
                        GetFloorList.Add(_GetFloorList);
                    }

                }

                return GetFloorList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetFloorByFloorId Parameters= " + _GetFloorByFloorId.ToString());
                throw ex;
            }
            finally
            {
                GetFloorList = null;
                _GetFloorList = null;
            }
        }

        public List<PackageType> GetPackageTypeByPackageId(PackageType _GetPackageTypeByPackageId)
        {
            List<PackageType> GetPackageTypeList = new List<PackageType>();
            PackageType _GetPackageTypeList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_GetPackageTypeByPackageId.package_type_id, CommanConstans.package_type_id, SqlDbType.SmallInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTypeByPackageId);
                    while (Ireader.Read())
                    {
                        _GetPackageTypeList = new PackageType();
                        {
                            _GetPackageTypeList.package_type_id = Ireader.GetInt32(CommonColumns.package_type_id);
                            _GetPackageTypeList.package_name = Ireader.GetString(CommonColumns.package_name);
                            _GetPackageTypeList.package_description = Ireader.GetString(CommonColumns.package_description);
                        };
                        GetPackageTypeList.Add(_GetPackageTypeList);
                    }

                }

                return GetPackageTypeList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTypeByPackageId Parameters= " + _GetPackageTypeByPackageId.ToString());
                throw ex;
            }
            finally
            {
                GetPackageTypeList = null;
                _GetPackageTypeList = null;
            }
        }
        public PackageDetail GetPackagesByPackageId(string PackageId)
        {
            PackageDetail _GetPackageList = new PackageDetail();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PackageId, CommanConstans.package_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackagesByPackageId);
                    while (Ireader.Read())
                    {
                        _GetPackageList.package_cd = Ireader.GetString(CommonColumns.package_cd);
                        _GetPackageList.package_type_id = Ireader.GetInt32(CommonColumns.package_type_id);
                        _GetPackageList.package_name = Ireader.GetString(CommonColumns.package_name);
                        _GetPackageList.plan_id = Ireader.GetInt32(CommonColumns.plan_id);
                        _GetPackageList.plan_name = Ireader.GetString(CommonColumns.plan_name);
                        _GetPackageList.floor_id = Ireader.GetInt32(CommonColumns.floor_id);
                        _GetPackageList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                        _GetPackageList.valid_from = Ireader.GetDateTime(CommonColumns.valid_from);
                        _GetPackageList.valid_to = Ireader.GetDateTime(CommonColumns.valid_to);
                        _GetPackageList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);
                        _GetPackageList.amount = Ireader.GetDecimal(CommonColumns.amount);
                        _GetPackageList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                        _GetPackageList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetPackageList.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);

                        _GetPackageList.created_date = Ireader.GetDateTime(CommonColumns.created_date);
                        _GetPackageList.created_by = Ireader.GetString(CommonColumns.created_by);
                        _GetPackageList.modified_date = Ireader.GetDateTime(CommonColumns.modified_date);
                        _GetPackageList.modified_by = Ireader.GetString(CommonColumns.modified_by);
                        if (Ireader.GetInt32("branch_id") == 0)
                            _GetPackageList.isGlobalpkg = true;
                        else
                            _GetPackageList.isGlobalpkg = false;
                        //GetPackageList.Add(_GetPackageList);
                    }

                }

                return _GetPackageList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackagesByPackageId Parameters= " + PackageId);
                throw ex;
            }
            finally
            {
                _GetPackageList = null;
            }
        }
        public List<PackageTasksItem> GetPackageTasksItem(string PackageId, string TaskId)
        {
            List<PackageTasksItem> GetPackageList = new List<PackageTasksItem>();
            PackageTasksItem _GetPackageList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PackageId, CommanConstans.Package_Id, SqlDbType.VarChar);
                    cmd.AddParameters(TaskId, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTasks_Item);
                    while (Ireader.Read())
                    {
                        _GetPackageList = new PackageTasksItem();
                        {
                            //_GetPackageList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            //  _GetPackageList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _GetPackageList.Task = taskDropDown;
                            _GetPackageList.PackageTask_id = Ireader.GetString(CommonColumns.Package_Task_id);
                        };
                        GetPackageList.Add(_GetPackageList);
                    }

                }

                return GetPackageList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksItem Parameters= " + PackageId + "TaskId=" + TaskId);
                throw ex;
            }
            finally
            {
                GetPackageList = null;
                _GetPackageList = null;
            }
        }

        public List<TaskDropDown> GetPackageTasksListItem(string PackageId, string TaskId)
        {
            List<TaskDropDown> GetPackageList = new List<TaskDropDown>();
            TaskDropDown _GetPackageList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PackageId, CommanConstans.Package_Id, SqlDbType.VarChar);
                    cmd.AddParameters(TaskId, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTasksList_Item);
                    while (Ireader.Read())
                    {
                        _GetPackageList = new TaskDropDown();
                        {
                            _GetPackageList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _GetPackageList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        GetPackageList.Add(_GetPackageList);
                    }

                }

                return GetPackageList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksItemList Parameters= " + PackageId + "TaskId=" + TaskId);
                throw ex;
            }
            finally
            {
                GetPackageList = null;
                _GetPackageList = null;
            }
        }

        public List<PackageTasksItemList> GetPackageTasksItemDetails(string PackageId, string TaskId)
        {
            List<PackageTasksItemList> GetPackageList = new List<PackageTasksItemList>();
            PackageTasksItemList _GetPackageList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown uOMDropDown;
            //TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PackageId, CommanConstans.Package_Id, SqlDbType.VarChar);
                    cmd.AddParameters(TaskId, CommanConstans.Task_Id, SqlDbType.VarChar);
                    cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.Package_Det_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_PackageTasks_ItemDetails);
                    while (Ireader.Read())
                    {
                        _GetPackageList = new PackageTasksItemList();
                        {
                            _GetPackageList.Package_Id = Ireader.GetString(CommonColumns.package_id);
                            _GetPackageList.Package_Det_Id = Ireader.GetString(CommonColumns.package_det_id);
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _GetPackageList.Category = categoryDropDown;
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _GetPackageList.Item = itemDropDown;
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _GetPackageList.BillingUOM = statusLookup;
                            _GetPackageList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _GetPackageList.Qty = Ireader.GetDecimal(CommonColumns.Qty);
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            };
                            _GetPackageList.UOM = uOMDropDown;
                            _GetPackageList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _GetPackageList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            //taskDropDown = new TaskDropDown();
                            //{
                            //    taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            //    taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            //};
                            //_GetPackageList.Task = taskDropDown;
                            _GetPackageList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _GetPackageList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            //_GetPackageList.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                            //_GetPackageList.category_name = Ireader.GetString(CommonColumns.Category);
                            //_GetPackageList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            //_GetPackageList.item_id = Ireader.GetInt32(CommonColumns.Item_Id);
                            //_GetPackageList.item_description = Ireader.GetString(CommonColumns.Item_Description);
                            //_GetPackageList.description = Ireader.GetString(CommonColumns.BillingUom);

                            //_GetPackageList.uom_description = Ireader.GetString(CommonColumns.Uom);

                        };
                        GetPackageList.Add(_GetPackageList);
                    }

                }

                return GetPackageList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageTasksItemDetails Parameters= " + PackageId.ToString() + "TaskId=" + TaskId);
                throw ex;
            }
            finally
            {
                GetPackageList = null;
                _GetPackageList = null;
                categoryDropDown = null;
                itemDropDown = null;
                statusLookup = null;
                uOMDropDown = null;
                //taskDropDown = null;
            }
        }
        public List<PackageAmountList> GetPackageAmount(PackageAmountList _PackageAmountCriteria)
        {
            List<PackageAmountList> PackageAmountList = new List<PackageAmountList>();
            PackageAmountList _PackageAmountList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_PackageAmountCriteria.Package_Id, CommanConstans.Package_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Package_Amount);
                    while (Ireader.Read())
                    {
                        _PackageAmountList = new PackageAmountList();
                        {
                            _PackageAmountList.GrandTotal = Ireader.GetDecimal(CommonColumns.GrandTotal);
                            _PackageAmountList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                            _PackageAmountList.Subtotal = Ireader.GetDecimal(CommonColumns.Subtotal);
                            _PackageAmountList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);
                            _PackageAmountList.ActualAmount = Ireader.GetDecimal(CommonColumns.ActualAmount);
                        };
                        PackageAmountList.Add(_PackageAmountList);
                    }

                }

                return PackageAmountList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackageAmount Parameters= " + _PackageAmountCriteria.ToString());
                throw ex;
            }
            finally
            {
                PackageAmountList = null;
                _PackageAmountList = null;
            }
        }

        public List<ProjectTasksItem> GetProjectTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItem> ProjectTasksItemList = new List<ProjectTasksItem>();
            ProjectTasksItem _ProjectTasksItemList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasks_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItem();
                        {
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _ProjectTasksItemList.Task = taskDropDown;
                            //_ProjectTasksItemList = Ireader.GetString(CommonColumns.Task_Id);
                            //_ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }
        public List<ProjectTasksItem> GetProjectTasksItemPackage(ProjectTasksItemList _ProjectTasksItemCriteria,bool IsFromPackage)
        {
            List<ProjectTasksItem> ProjectTasksItemList = new List<ProjectTasksItem>();
            ProjectTasksItem _ProjectTasksItemList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    cmd.AddParameters(IsFromPackage, CommanConstans.IsFromPackage, SqlDbType.Bit);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasks_Item_Package);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItem();
                        {
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _ProjectTasksItemList.Task = taskDropDown;
                            //_ProjectTasksItemList = Ireader.GetString(CommonColumns.Task_Id);
                            //_ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public List<ProjectTasksItem> GetProjectTasksQuotationItem(ProjectTasksItemList _ProjectTasksItemCriteria,bool IsFromPackage)
        {
            List<ProjectTasksItem> ProjectTasksItemList = new List<ProjectTasksItem>();
            ProjectTasksItem _ProjectTasksItemList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    cmd.AddParameters(IsFromPackage, CommanConstans.IsFromPackage, SqlDbType.Bit);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasksQuotation_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItem();
                        {
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _ProjectTasksItemList.Task = taskDropDown;
                            //_ProjectTasksItemList = Ireader.GetString(CommonColumns.Task_Id);
                            //_ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksQuotationItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public List<ProjectTasksItemList> GetProjectTasksListItem(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItemList> ProjectTasksItemList = new List<ProjectTasksItemList>();
            ProjectTasksItemList _ProjectTasksItemList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasksList_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItemList();
                        {
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItemList Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }
        public List<ProjectTasksItemList> GetProjectTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItemList> ProjectTasksItemList = new List<ProjectTasksItemList>();
            ProjectTasksItemList _ProjectTasksItemList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown uOMDropDown;
            //TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectTasksItemCriteria.Project_Det_Id == null || _ProjectTasksItemCriteria.Project_Det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Project_Det_Id, CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }

                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    //cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasks_ItemDetails);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItemList();
                        {
                            _ProjectTasksItemList.project_Id = Ireader.GetString(CommonColumns.project_id);
                            _ProjectTasksItemList.Project_Det_Id = Ireader.GetString(CommonColumns.project_det_Id);
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _ProjectTasksItemList.Category = categoryDropDown;
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ProjectTasksItemList.Item = itemDropDown;
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _ProjectTasksItemList.BillingUOM = statusLookup;
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            };
                            _ProjectTasksItemList.UOM = uOMDropDown;
                            //_ProjectTasksItemList.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                            //_ProjectTasksItemList.category_name = Ireader.GetString(CommonColumns.Category);
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            //_ProjectTasksItemList.item_id = Ireader.GetInt32(CommonColumns.Item_Id);
                            //_ProjectTasksItemList.item_description = Ireader.GetString(CommonColumns.Item_Description);
                            //_ProjectTasksItemList.description = Ireader.GetString(CommonColumns.BillingUom);
                            //_ProjectTasksItemList.status_lookup_id = Ireader.GetInt32(CommonColumns.price_type_id);
                            _ProjectTasksItemList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _ProjectTasksItemList.Qty = Ireader.GetString(CommonColumns.Qty);
                            //_ProjectTasksItemList.uom_description = Ireader.GetString(CommonColumns.Uom);
                            //_ProjectTasksItemList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            _ProjectTasksItemList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _ProjectTasksItemList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _ProjectTasksItemList.Cost_Amount = Ireader.GetDecimal(CommonColumns.Cost_Amount);
                            _ProjectTasksItemList.Profit_Loss = Ireader.GetDecimal(CommonColumns.Profit_Loss);
                            _ProjectTasksItemList.Highlight = Ireader.GetBoolean(CommonColumns.Highlight);
                            _ProjectTasksItemList.AdditionalDescription = Ireader.GetString(CommonColumns.AdditionalDescription);
                        
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public List<ProjectTasksItemList> GetProjectTasksQuotationItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria, bool IsFromPackage)
        {
            List<ProjectTasksItemList> ProjectTasksItemList = new List<ProjectTasksItemList>();
            ProjectTasksItemList _ProjectTasksItemList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown uOMDropDown;
            //TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectTasksItemCriteria.Project_Det_Id == null || _ProjectTasksItemCriteria.Project_Det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Project_Det_Id, CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }

                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    cmd.AddParameters(IsFromPackage, CommanConstans.IsFromPackage, SqlDbType.Bit);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectTasksQuotation_ItemDetails);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItemList();
                        {
                            _ProjectTasksItemList.project_Id = Ireader.GetString(CommonColumns.project_id);
                            _ProjectTasksItemList.Project_Det_Id = Ireader.GetString(CommonColumns.project_det_Id);
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _ProjectTasksItemList.Category = categoryDropDown;
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ProjectTasksItemList.Item = itemDropDown;
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _ProjectTasksItemList.BillingUOM = statusLookup;
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.uom_description);
                            };
                            _ProjectTasksItemList.UOM = uOMDropDown;
                            //_ProjectTasksItemList.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                            //_ProjectTasksItemList.category_name = Ireader.GetString(CommonColumns.Category);
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            //_ProjectTasksItemList.item_id = Ireader.GetInt32(CommonColumns.Item_Id);
                            //_ProjectTasksItemList.item_description = Ireader.GetString(CommonColumns.Item_Description);
                            //_ProjectTasksItemList.description = Ireader.GetString(CommonColumns.BillingUom);
                            //_ProjectTasksItemList.status_lookup_id = Ireader.GetInt32(CommonColumns.price_type_id);
                            _ProjectTasksItemList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _ProjectTasksItemList.Qty = Ireader.GetString(CommonColumns.Qty);
                            //_ProjectTasksItemList.uom_description = Ireader.GetString(CommonColumns.Uom);
                            //_ProjectTasksItemList.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                            _ProjectTasksItemList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _ProjectTasksItemList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _ProjectTasksItemList.Cost_Amount = Ireader.GetDecimal(CommonColumns.Cost_Amount);
                            _ProjectTasksItemList.Profit_Loss = Ireader.GetDecimal(CommonColumns.Profit_Loss);
                            _ProjectTasksItemList.Highlight = Ireader.GetBoolean(CommonColumns.Highlight);
                            _ProjectTasksItemList.AdditionalDescription = Ireader.GetString(CommonColumns.AdditionalDescription);

                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectTasksQuotationItemDetails Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public SuccessMessage DeleteCategoryByCategoryId(string Category_Id)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Category_Id, CommanConstans.Category_Id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_CategoryByCategory_Id);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteCategoryByCategoryId Parameters= " + Category_Id.ToString());
                throw ex;
            }
            finally
            {
                Category_Id = null;
            }
        }
        public SuccessMessage DeleteTasksByTaskId(string TaskId)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(TaskId, CommanConstans.task_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_TasksByTaskId);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteTasksByTaskId Parameters= " + TaskId.ToString());
                throw ex;
            }
            finally
            {
                TaskId = null;
            }
        }
        public SuccessMessage DeleteItemByItemId(string ItemId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(ItemId, CommanConstans.item_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_ItemByItemId);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteItemByItemId Parameters= " + ItemId.ToString());
                throw ex;
            }
            finally
            {
                ItemId = null;
            }
        }
        public SuccessMessage DeleteUOMByUomId(string UomId)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(UomId, CommanConstans.uom_id, SqlDbType.TinyInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_UOMByUomId);
                    while (ireader.Read())
                    {

                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);

                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteUOMByUomId UomId= " + UomId.ToString());
                throw ex;
            }
            finally
            {
                UomId = null;
            }

        }
        public SuccessMessage DeletePlansByPlanId(string PlanId)
        {
            SuccessMessage _SuccessMessage;
            try
            {
                _SuccessMessage = new SuccessMessage();
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(PlanId, CommanConstans.plan_id, SqlDbType.SmallInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_PlansByPlanId);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePlansByPlanId PlanId= " + PlanId.ToString());
                throw ex;
            }
            finally
            {
                PlanId = null;
            }

        }
        public SuccessMessage DeleteFloorByFloorId(string FloorId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(FloorId, CommanConstans.floor_id, SqlDbType.SmallInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_FloorByFloorId);
                    while (ireader.Read())
                    {

                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteFloorByFloorId FloorId= " + FloorId.ToString());
                throw ex;
            }
            finally
            {
                FloorId = null;
            }

        }
        public SuccessMessage DeletePackageTypeByPackageTypeId(string PackageTypeId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(PackageTypeId, CommanConstans.package_type_id, SqlDbType.SmallInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_PackageTypeByPackageTypeId);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackageTypeByPackageTypeId Parameters= " + PackageTypeId.ToString());
                throw ex;
            }
            finally
            {
                PackageTypeId = null;
            }

        }
        public List<SuccessMessage> DeletePackagesByPackageTypeId(PackageDetail _DeletePackageById)
        {
            List<SuccessMessage> SuccessMessage = new List<SuccessMessage>();
            SuccessMessage _SuccessMessage;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(_DeletePackageById.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    cmd.AddParameters(_DeletePackageById.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_DeletePackageById.package_type_id, CommanConstans.package_type_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_DeletePackageById.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_PackagesByPackageTypeId);
                    while (ireader.Read())
                    {
                        _SuccessMessage = new SuccessMessage();
                        {
                            _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                            _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        };
                        SuccessMessage.Add(_SuccessMessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackagesByPackageTypeId Parameters= " + _DeletePackageById.ToString());
                throw ex;
            }
            finally
            {
                _DeletePackageById = null;
            }


        }
        public SuccessMessage DeleteMasterContractTermsByID(string ContractTermId)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ContractTermId, CommanConstans.master_contract_term_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_MasterContractTermsByID);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_MasterContractTermsByID Parameters= " + ContractTermId.ToString());
                throw ex;
            }
            finally
            {
                ContractTermId = null;
            }
        }
        public List<PaymentTerms> GetPackagepaymentterms(string PackageId)
        {
            List<PaymentTerms> GetPackagepaymentterms = new List<PaymentTerms>();
            PaymentTerms _GetPackagepaymentterms;
            PaymentDescription paymentDescription;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PackageId, CommanConstans.package_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Package_payment_terms);
                    while (Ireader.Read())
                    {
                        _GetPackagepaymentterms = new PaymentTerms();
                        {
                            _GetPackagepaymentterms.payment_term_id = Ireader.GetString(CommonColumns.payment_term_id);
                            paymentDescription = new PaymentDescription();
                            {
                                paymentDescription.Master_payment_term_id = Ireader.GetInt32(CommonColumns.payment_term_id);
                                paymentDescription.Master_payment_description = Ireader.GetString(CommonColumns.payment_description);
                            };
                            _GetPackagepaymentterms.paymentdescription = paymentDescription;

                            //_GetPackagepaymentterms.Master_payment_description = Ireader.GetString(CommonColumns.payment_description);
                        };
                        GetPackagepaymentterms.Add(_GetPackagepaymentterms);
                    }

                }

                return GetPackagepaymentterms;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackagepaymentterms PackageId= " + PackageId);
                throw ex;
            }
            finally
            {
                GetPackagepaymentterms = null;
                _GetPackagepaymentterms = null;
            }
        }
        public List<StatusLookup> Bind_StatusLookUp(string LookUpColumnId)
        {
            List<StatusLookup> GetStatusLookup = new List<StatusLookup>();
            StatusLookup _GetStatusLookup;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(LookUpColumnId, CommanConstans.LookUpColumnId, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Bind_StatusLookUp);
                    while (Ireader.Read())
                    {
                        _GetStatusLookup = new StatusLookup();
                        {
                            _GetStatusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                            _GetStatusLookup.description = Ireader.GetString(CommonColumns.description);
                        };
                        GetStatusLookup.Add(_GetStatusLookup);
                    }

                }

                return GetStatusLookup;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindStatusLookUp LookUpColumnId= " + LookUpColumnId);
                throw ex;
            }
            finally
            {
                GetStatusLookup = null;
                _GetStatusLookup = null;
            }
        }
        public List<Quotation> BindProjectStatus(string Type)
        {
            List<Quotation> ObjBindstatus = new List<Quotation>();
            Quotation _ObjBindstatus;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (Type == null)
                    {
                        Type = "0";
                    }
                    cmd.AddParameters(Type, CommanConstans.Statusgroup, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindProject_Status);
                    while (Ireader.Read())
                    {
                        _ObjBindstatus = new Quotation();
                        {
                            _ObjBindstatus.projectStatusid = Ireader.GetInt32(CommonColumns.id);
                            _ObjBindstatus.projectStatus = Ireader.GetString(CommonColumns.project_status);
                            _ObjBindstatus.statusgroup = Ireader.GetString(CommonColumns.statusgroup);
                            _ObjBindstatus.isactive = Ireader.GetString(CommonColumns.isactive);
                        };

                        ObjBindstatus.Add(_ObjBindstatus);
                    }

                }

                return ObjBindstatus;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindProjectStatus");
                throw ex;
            }
            finally
            {
                ObjBindstatus = null;
                _ObjBindstatus = null;
            }
        }
        public List<SalesmanDropDown> BindSalesmen()
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

        public List<AddressDropDown> BindAddressSite(string Project_Id, string Salesmen_Id)
        {
            List<AddressDropDown> BindAddress = new List<AddressDropDown>();
            AddressDropDown _BindAddress;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Project_Id, CommanConstans.Project_Id, SqlDbType.BigInt);
                    cmd.AddParameters(Salesmen_Id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindAddress);
                    while (Ireader.Read())
                    {
                        _BindAddress = new AddressDropDown();
                        {
                            _BindAddress.id = Ireader.GetInt32(CommonColumns.id);
                            _BindAddress.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
                        };

                        BindAddress.Add(_BindAddress);
                    }

                }

                return BindAddress;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
                throw ex;
            }
            finally
            {
                BindAddress = null;
                _BindAddress = null;
            }
        }

        public List<AddressDropDown> BindAddressSite(Int64 Supplier_Id)
        {
            List<AddressDropDown> BindAddress = new List<AddressDropDown>();
            AddressDropDown _BindAddress;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Supplier_Id, CommanConstans.Supplier_Id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindAddressBySellerId);
                    while (Ireader.Read())
                    {
                        _BindAddress = new AddressDropDown();
                        {
                            _BindAddress.id = Ireader.GetInt32(CommonColumns.id);
                            _BindAddress.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
                        };

                        BindAddress.Add(_BindAddress);
                    }

                }

                return BindAddress;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
                throw ex;
            }
            finally
            {
                BindAddress = null;
                _BindAddress = null;
            }
        }


        public List<AddressSiteDropDown> GetAddressSiteList(string ZipCodeId, string UnitCodeId,string BranchId,string SalesmanId)
        {
            List<AddressSiteDropDown> _addressSiteList = new List<AddressSiteDropDown>();
            AddressSiteDropDown _addressSite;
            try
            {
                if (SalesmanId == null)
                {
                    SalesmanId = "";
                }
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ZipCodeId, CommanConstans.ZipCodeId, SqlDbType.VarChar);
                    cmd.AddParameters(UnitCodeId, CommanConstans.UnitCodeId, SqlDbType.VarChar);
                    cmd.AddParameters(BranchId, CommanConstans.branch_id, SqlDbType.NVarChar);
                    cmd.AddParameters(SalesmanId, CommanConstans.salesmen_id, SqlDbType.NVarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_AddressSiite_By_ZC_UC);
                    while (Ireader.Read())
                    {
                        _addressSite = new AddressSiteDropDown();
                        {
                            _addressSite.AddressSite_id = Ireader.GetString(CommonColumns.id);
                            _addressSite.AddressSite_Name = Ireader.GetString(CommonColumns.AddressSite);
                        };

                        _addressSiteList.Add(_addressSite);
                    }
                    _addressSiteList.Insert(0, new AddressSiteDropDown { AddressSite_Name = "Select Address ", AddressSite_id = "0" });
                }

                return _addressSiteList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetAddressSiteList");
                throw ex;
            }
            finally
            {
                _addressSite = null;
                _addressSiteList = null;
            }
        }

        public List<SalesmanDropDown> GetSalesmenList(string ZipCodeId, string UnitCodeId,string BranchId)
        {
            List<SalesmanDropDown> _salesmenList = new List<SalesmanDropDown>();
            SalesmanDropDown _salesmen;
            
            try
            {               
                
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ZipCodeId, CommanConstans.ZipCodeId, SqlDbType.VarChar);
                    cmd.AddParameters(UnitCodeId, CommanConstans.UnitCodeId, SqlDbType.VarChar);
                    cmd.AddParameters(BranchId, CommanConstans.branch_id, SqlDbType.NVarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_Salesmen_By_ZC_UC);
                    while (Ireader.Read())
                    {
                        _salesmen = new SalesmanDropDown();
                        {
                            _salesmen.id = Ireader.GetInt32(CommonColumns.id);
                            _salesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                        };                       
                        _salesmenList.Add(_salesmen);
                    }
                    _salesmenList.Insert(0, new SalesmanDropDown { salesmen_name = "Select Salesman ", id = 0 });
                }

                return _salesmenList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenList");
                throw ex;
            }
            finally
            {
                _salesmen = null;
                _salesmenList = null;
            }
        }

        public List<SalesmanDropDown> GetSalesmenListByProject (int ProjectId)
        {
            List<SalesmanDropDown> _salesmenList = new List<SalesmanDropDown>();
            SalesmanDropDown _salesmen;

            try
            {

                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Convert.ToInt64(ProjectId), CommanConstans.Project_Id, SqlDbType.BigInt);
                    cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt) ;
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_Salesmen_By_ProjectId);
                    while (Ireader.Read())
                    {
                        _salesmen = new SalesmanDropDown();
                        {
                            _salesmen.id = Ireader.GetInt32(CommonColumns.id);
                            _salesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                        };
                        _salesmenList.Add(_salesmen);
                    }
                    //_salesmenList.Insert(0, new SalesmanDropDown { salesmen_name = "Select Salesman ", id = 0 });
                }

                return _salesmenList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenList");
                throw ex;
            }
            finally
            {
                _salesmen = null;
                _salesmenList = null;
            }
        }

        public List<SupplierInvoiceDropDown> BindSuppliernvoiceNo(Int64 id)
        {
            List<SupplierInvoiceDropDown> BidSupplierInvoice = new List<SupplierInvoiceDropDown>();
            SupplierInvoiceDropDown _BidSupplierInvoice;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(id, CommanConstans.Project_Id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindInvoiceNoByAddress);
                    while (Ireader.Read())
                    {
                        _BidSupplierInvoice = new SupplierInvoiceDropDown();
                        {
                            _BidSupplierInvoice.id = Ireader.GetInt32(CommonColumns.id);
                            _BidSupplierInvoice.InvoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                        };

                        BidSupplierInvoice.Add(_BidSupplierInvoice);
                    }

                }

                return BidSupplierInvoice;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSuppliernvoiceNo");
                throw ex;
            }
            finally
            {
                BidSupplierInvoice = null;
                _BidSupplierInvoice = null;
            }
        }

        public List<CustomerDropDown> BindCustomer()
        {
            List<CustomerDropDown> BindCustomer = new List<CustomerDropDown>();
            CustomerDropDown _BindCustomer;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindCustomer);
                    while (Ireader.Read())
                    {
                        _BindCustomer = new CustomerDropDown();
                        {
                            _BindCustomer.Customer_id = Ireader.GetInt32(CommonColumns.id);
                            _BindCustomer.name1 = Ireader.GetString(CommonColumns.name1);
                            //_BindCustomer.email = Ireader.GetString(CommonColumns.email);
                        };

                        BindCustomer.Add(_BindCustomer);
                    }

                }

                return BindCustomer;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindCustomer");
                throw ex;
            }
            finally
            {
                BindCustomer = null;
                _BindCustomer = null;
            }
        }
        public SuccessMessage UpsertPackagePaymentTerms(PaymentTerms _PackagePaymentTermsCriteria, string uid, string PackageId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_PackagePaymentTermsCriteria.payment_term_id == null || _PackagePaymentTermsCriteria.payment_term_id == "")
                    {
                        cmd.AddParameters(0, CommanConstans.payment_term_id, SqlDbType.Int);
                    }
                    else
                    {
                        cmd.AddParameters(_PackagePaymentTermsCriteria.payment_term_id, CommanConstans.payment_term_id, SqlDbType.Int);
                    }
                    cmd.AddParameters(PackageId, CommanConstans.package_id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackagePaymentTermsCriteria.paymentdescription.Master_payment_term_id, CommanConstans.master_payment_termid, SqlDbType.Int);
                    cmd.AddParameters(_PackagePaymentTermsCriteria.paymentdescription.Master_payment_description, CommanConstans.payment_term_description, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Package_payment_terms);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackagePaymentTerms Parameters=" + _PackagePaymentTermsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _PackagePaymentTermsCriteria = null;
                }
            }
        }

        public QuotationDetails GetQuotationDetailsByProjectId(string ProjectId)/*,string evo_id = null)*/
        {
            QuotationDetails _GetQuotationDeatils = new QuotationDetails();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    //cmd.AddParameters(evo_id, CommanConstans.evo_id, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_QuotationDetailsByProject_id);
                    while (Ireader.Read())
                    {
                        _GetQuotationDeatils.project_id = Ireader.GetString(CommonColumns.project_id);
                        _GetQuotationDeatils.id = Ireader.GetString(CommonColumns.id);
                        _GetQuotationDeatils.contract_date = Ireader.GetDateTime(CommonColumns.contract_date);
                        _GetQuotationDeatils.project_number = Ireader.GetString(CommonColumns.project_number);
                        _GetQuotationDeatils.project_name = Ireader.GetString(CommonColumns.project_name);
                        _GetQuotationDeatils.contract_amount = Ireader.GetDecimal(CommonColumns.contract_amount);
                        _GetQuotationDeatils.status_id = Ireader.GetInt32(CommonColumns.status_id);
                        _GetQuotationDeatils.salesmen_id = Ireader.GetInt32(CommonColumns.salesmen_id);
                        _GetQuotationDeatils.customer_id = Ireader.GetInt32(CommonColumns.customer_id);
                        _GetQuotationDeatils.project_start_date = Ireader.GetDateTime(CommonColumns.project_start_date);
                        _GetQuotationDeatils.project_end_date = Ireader.GetDateTime(CommonColumns.project_end_date);
                        _GetQuotationDeatils.quotationForwardDate = Ireader.GetDateTime(CommonColumns.quotationForwardDate);
                        _GetQuotationDeatils.quotationAcceptDate = Ireader.GetDateTime(CommonColumns.quotationAcceptDate);
                        _GetQuotationDeatils.discount = Ireader.GetDecimal(CommonColumns.discount);
                        _GetQuotationDeatils.discount_percentage = Ireader.GetDecimal(CommonColumns.discountpercentage);
                        _GetQuotationDeatils.remarks = Ireader.GetString(CommonColumns.remarks);

                        _GetQuotationDeatils.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                        _GetQuotationDeatils.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetQuotationDeatils.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                        _GetQuotationDeatils.Email = Ireader.GetString(CommonColumns.Email);
                        _GetQuotationDeatils.salesmen = Ireader.GetString(CommonColumns.salesmen);
                        _GetQuotationDeatils.customer = Ireader.GetString(CommonColumns.customer);
                        _GetQuotationDeatils.status = Ireader.GetString(CommonColumns.status);
                        _GetQuotationDeatils.package_id = Ireader.GetString(CommonColumns.package_id);
                        _GetQuotationDeatils.document_path = Ireader.GetString(CommonColumns.document_path);
                        _GetQuotationDeatils.NRIC = Ireader.GetString(CommonColumns.NRIC);
                        //_GetQuotationDeatils.Customer_document_path = Ireader.GetString(CommonColumns.Customer_document_path);

                        _GetQuotationDeatils.created_date = Ireader.GetDateTime(CommonColumns.created_date);
                        _GetQuotationDeatils.created_by = Ireader.GetString(CommonColumns.created_by);
                        _GetQuotationDeatils.modified_date = Ireader.GetDateTime(CommonColumns.modified_date);
                        _GetQuotationDeatils.modified_by = Ireader.GetString(CommonColumns.modified_by);
                        //_GetQuotationDeatils.doc_id = Ireader.GetString(CommonColumns.doc_id);
                        //_GetQuotationDeatils.id_type = Ireader.GetString(CommonColumns.id_type);

                        _GetQuotationDeatils.EvoNo = Ireader.GetString(CommonColumns.EvoNo);
                        _GetQuotationDeatils.EvoDate = Ireader.GetDateTime(CommonColumns.EvoDate);
                        _GetQuotationDeatils.Evomodified_date = Ireader.GetDateTime(CommonColumns.EvoUpdatedDate);
                        _GetQuotationDeatils.Evomodified_by = Ireader.GetString(CommonColumns.EvoupdatedBy);
                        _GetQuotationDeatils.Evocreated_date = Ireader.GetDateTime(CommonColumns.EvoCretedDate);
                        _GetQuotationDeatils.Evocreated_by = Ireader.GetString(CommonColumns.EvoCreateBy);
                        _GetQuotationDeatils.EvoAmount = Ireader.GetDecimal(CommonColumns.EvoAmount);
                        _GetQuotationDeatils.Evo_id = Ireader.GetString(CommonColumns.evouniquid);

                        _GetQuotationDeatils.totalamount = Ireader.GetDecimal(CommonColumns.totalamaount);
                        _GetQuotationDeatils.gstamount = Ireader.GetDecimal(CommonColumns.gstamount);
                        _GetQuotationDeatils.PropertyType_id = Ireader.GetInt16(CommonColumns.PropertyType_Id);
                        _GetQuotationDeatils.EvoStatus_Id = Ireader.GetInt16(CommonColumns.EvoStatus_id);
                        _GetQuotationDeatils.EvoStatus = Ireader.GetString(CommonColumns.EvoStatus);
                        //_GetQuotationDeatils.grandtotal = Ireader.GetDecimal(CommonColumns.GrandTOtal);


                    }

                }

                return _GetQuotationDeatils;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackagesByPackageId Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                _GetQuotationDeatils = null;
            }
        }

        public QuotationDetails GetQuotationDetailsByProjectIdForEvo(string ProjectId, string evo_id = null)
        {
            QuotationDetails _GetQuotationDeatils = new QuotationDetails();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(evo_id, CommanConstans.evo_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_QuotationDetailsByProject_id);
                    while (Ireader.Read())
                    {
                        _GetQuotationDeatils.project_id = Ireader.GetString(CommonColumns.project_id);
                        _GetQuotationDeatils.id = Ireader.GetString(CommonColumns.id);
                        _GetQuotationDeatils.contract_date = Ireader.GetDateTime(CommonColumns.contract_date);
                        _GetQuotationDeatils.project_number = Ireader.GetString(CommonColumns.project_number);
                        _GetQuotationDeatils.project_name = Ireader.GetString(CommonColumns.project_name);
                        _GetQuotationDeatils.contract_amount = Ireader.GetDecimal(CommonColumns.contract_amount);
                        _GetQuotationDeatils.status_id = Ireader.GetInt32(CommonColumns.status_id);
                        _GetQuotationDeatils.salesmen_id = Ireader.GetInt32(CommonColumns.salesmen_id);
                        _GetQuotationDeatils.customer_id = Ireader.GetInt32(CommonColumns.customer_id);
                        _GetQuotationDeatils.project_start_date = Ireader.GetDateTime(CommonColumns.project_start_date);
                        _GetQuotationDeatils.project_end_date = Ireader.GetDateTime(CommonColumns.project_end_date);
                        _GetQuotationDeatils.quotationForwardDate = Ireader.GetDateTime(CommonColumns.quotationForwardDate);
                        _GetQuotationDeatils.quotationAcceptDate = Ireader.GetDateTime(CommonColumns.quotationAcceptDate);
                        _GetQuotationDeatils.discount = Ireader.GetDecimal(CommonColumns.discount);
                        _GetQuotationDeatils.discount_percentage = Ireader.GetInt32(CommonColumns.discountpercentage);
                        _GetQuotationDeatils.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                        _GetQuotationDeatils.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetQuotationDeatils.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                        _GetQuotationDeatils.Email = Ireader.GetString(CommonColumns.Email);
                        _GetQuotationDeatils.salesmen = Ireader.GetString(CommonColumns.salesmen);
                        _GetQuotationDeatils.customer = Ireader.GetString(CommonColumns.customer);
                        _GetQuotationDeatils.status = Ireader.GetString(CommonColumns.status);
                        _GetQuotationDeatils.package_id = Ireader.GetString(CommonColumns.package_id);
                        _GetQuotationDeatils.document_path = Ireader.GetString(CommonColumns.document_path);
                        _GetQuotationDeatils.NRIC = Ireader.GetString(CommonColumns.NRIC);
                        //_GetQuotationDeatils.Customer_document_path = Ireader.GetString(CommonColumns.Customer_document_path);

                        _GetQuotationDeatils.created_date = Ireader.GetDateTime(CommonColumns.created_date);
                        _GetQuotationDeatils.created_by = Ireader.GetString(CommonColumns.created_by);
                        _GetQuotationDeatils.modified_date = Ireader.GetDateTime(CommonColumns.modified_date);
                        _GetQuotationDeatils.modified_by = Ireader.GetString(CommonColumns.modified_by);
                        //_GetQuotationDeatils.doc_id = Ireader.GetString(CommonColumns.doc_id);
                        //_GetQuotationDeatils.id_type = Ireader.GetString(CommonColumns.id_type);

                        _GetQuotationDeatils.EvoNo = Ireader.GetString(CommonColumns.EvoNo);
                        _GetQuotationDeatils.EvoDate = Ireader.GetDateTime(CommonColumns.EvoDate);
                        _GetQuotationDeatils.Evomodified_date = Ireader.GetDateTime(CommonColumns.EvoUpdatedDate);
                        _GetQuotationDeatils.Evomodified_by = Ireader.GetString(CommonColumns.EvoupdatedBy);
                        _GetQuotationDeatils.Evocreated_date = Ireader.GetDateTime(CommonColumns.EvoCretedDate);
                        _GetQuotationDeatils.Evocreated_by = Ireader.GetString(CommonColumns.EvoCreateBy);
                        //_GetQuotationDeatils.EvoAmount = Ireader.GetDecimal(CommonColumns.EvoAmount);
                        _GetQuotationDeatils.Evo_id = Ireader.GetString(CommonColumns.evouniquid);

                        //_GetQuotationDeatils.totalamount = Ireader.GetDecimal(CommonColumns.totalamaount);
                        //_GetQuotationDeatils.gstamount = Ireader.GetDecimal(CommonColumns.gstamount);
                        _GetQuotationDeatils.PropertyType_id = Ireader.GetInt16(CommonColumns.PropertyType_Id);
                        _GetQuotationDeatils.EvoStatus_Id = Ireader.GetInt16(CommonColumns.EvoStatus_id);
                        _GetQuotationDeatils.EvoStatus = Ireader.GetString(CommonColumns.EvoStatus);
                        //_GetQuotationDeatils.grandtotal = Ireader.GetDecimal(CommonColumns.GrandTOtal);

                        _GetQuotationDeatils.amount_before_discount = Ireader.GetDecimal(CommonColumns.amount_before_discount);
                        _GetQuotationDeatils.discount_percentage = Ireader.GetDecimal(CommanConstans.DiscountPercentage);
                        _GetQuotationDeatils.discount = Ireader.GetDecimal(CommanConstans.DiscountAmount);
                        _GetQuotationDeatils.EvoAmount = Ireader.GetDecimal(CommonColumns.EvoAmount);
                        _GetQuotationDeatils.gst_amount = Ireader.GetDecimal(CommonColumns.gstamount);
                        _GetQuotationDeatils.grandtotal = Ireader.GetDecimal(CommanConstans.TotalAmount);

                    }

                }

                return _GetQuotationDeatils;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPackagesByPackageId Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                _GetQuotationDeatils = null;
            }
        }
        public List<PaymentDescription> BindMasterpaymentterms()
        {
            List<PaymentDescription> GetPackagepaymentterms = new List<PaymentDescription>();
            PaymentDescription _GetPackagepaymentterms;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindMaster_Payment_Terms);
                    while (Ireader.Read())
                    {
                        _GetPackagepaymentterms = new PaymentDescription();
                        {
                            _GetPackagepaymentterms.Master_payment_term_id = Ireader.GetInt32(CommonColumns.masterpaymenttermid);//master_payment_termid
                            _GetPackagepaymentterms.Master_payment_description = Ireader.GetString(CommonColumns.description);
                        };
                        GetPackagepaymentterms.Add(_GetPackagepaymentterms);
                    }

                }

                return GetPackagepaymentterms;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindMasterpaymentterms");
                throw ex;
            }
            finally
            {
                GetPackagepaymentterms = null;
                _GetPackagepaymentterms = null;
            }
        }
        public List<PaymentDescription> BindPackageMasterPaymentTerms()
        {
            List<PaymentDescription> GetPackagepaymentterms = new List<PaymentDescription>();
            PaymentDescription _GetPackagepaymentterms;
            //PaymentDescription paymentDescription;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindPackage_Master_Payment_Terms);
                    while (Ireader.Read())
                    {
                        _GetPackagepaymentterms = new PaymentDescription();
                        {
                            _GetPackagepaymentterms.Master_payment_term_id = Ireader.GetInt32(CommonColumns.masterpaymenttermid);//master_payment_termid
                            _GetPackagepaymentterms.Master_payment_description = Ireader.GetString(CommonColumns.description);
                        };
                        GetPackagepaymentterms.Add(_GetPackagepaymentterms);
                    }

                }

                return GetPackagepaymentterms;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindPackageMasterPaymentTerms");
                throw ex;
            }
            finally
            {
                GetPackagepaymentterms = null;
                _GetPackagepaymentterms = null;
            }
        }
        public List<PaymentTerms> GetProjectpaymentterms(string ProjecId)
        {
            List<PaymentTerms> GetProjectpaymentterms = new List<PaymentTerms>();
            PaymentTerms _GetProjectpaymentterms;
            PaymentDescription paymentDescription;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjecId, CommanConstans.project_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Project_payment_terms);
                    while (Ireader.Read())
                    {
                        _GetProjectpaymentterms = new PaymentTerms();
                        {
                            _GetProjectpaymentterms.payment_term_id = Ireader.GetString(CommonColumns.payment_term_id);
                            paymentDescription = new PaymentDescription();
                            {
                                paymentDescription.Master_payment_term_id = Ireader.GetInt32(CommonColumns.payment_term_id);
                                paymentDescription.Master_payment_description = Ireader.GetString(CommonColumns.description);
                            };
                            _GetProjectpaymentterms.paymentdescription = paymentDescription;
                            //_GetProjectpaymentterms.Master_payment_description = Ireader.GetString(CommonColumns.description);
                        };
                        GetProjectpaymentterms.Add(_GetProjectpaymentterms);
                    }

                }

                return GetProjectpaymentterms;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetProjectpaymentterms PackageId= " + ProjecId);
                throw ex;
            }
            finally
            {
                GetProjectpaymentterms = null;
                _GetProjectpaymentterms = null;
            }
        }
        public string UpsertProjectTasks(List<ProjectTasksItemList> _ProjectTasksCriteria, string ProjectId)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    //if (_ProjectTasksCriteria[0].ProjectTaskid == null || _ProjectTasksCriteria[0].ProjectTaskid == "")
                    //{
                    //    cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.ProjectTaskid, SqlDbType.VarChar);

                    //}
                    //else
                    //{
                    //    cmd.AddParameters(_ProjectTasksCriteria[0].ProjectTaskid, CommanConstans.ProjectTaskid, SqlDbType.VarChar);

                    //}
                    if (_ProjectTasksCriteria[0].Task_Id == null || _ProjectTasksCriteria[0].Task_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.TaskId, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksCriteria[0].Task_Id, CommanConstans.TaskId, SqlDbType.VarChar);

                    }
                    cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);
                    //cmd.AddParameters(_ProjectTasksCriteria[0].Task_Id, CommanConstans.TaskId, SqlDbType.VarChar);
                    //cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar); 
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Project_Tasks);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectTasks Parameters=" + _ProjectTasksCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _ProjectTasksCriteria = null;
                }
            }
        }

        public string UpsertPackageTasks(List<PackageTasksItemList> _PackageTasksCriteria, string PackageId)
        {
            string result = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    //if (_PackageTasksCriteria[0]. PackageTask_id == null || _PackageTasksCriteria[0].PackageTask_id == "")
                    //{
                    //    cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.PackageTask_id, SqlDbType.VarChar);

                    //}
                    //else
                    //{
                    //    cmd.AddParameters(_PackageTasksCriteria[0].PackageTask_id, CommanConstans.PackageTask_id, SqlDbType.VarChar);

                    //}
                    //if (_PackageTasksCriteria[0].Task_Id == null || _PackageTasksCriteria[0].Task_Id == "")
                    //{
                    //    cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.Task_Id, SqlDbType.VarChar);

                    //}
                    //else
                    //{
                    //    cmd.AddParameters(_PackageTasksCriteria[0].Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);

                    //}
                    cmd.AddParameters(PackageId, CommanConstans.Package_Id, SqlDbType.VarChar);
                    //cmd.AddParameters(_ProjectTasksCriteria[0].Task_Id, CommanConstans.TaskId, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Package_Tasks);
                    while (ireader.Read())
                    {
                        result = ireader.GetString(CommonColumns.Result);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackageTasks Parameters=" + _PackageTasksCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _PackageTasksCriteria = null;
                }
            }
        }
        public List<Banks> BindBanks()
        {
            List<Banks> GetBankDetail = new List<Banks>();
            Banks _GetBankDetail;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindBanks);
                    while (Ireader.Read())
                    {
                        _GetBankDetail = new Banks();
                        {
                            _GetBankDetail.bank_id = Ireader.GetString(CommonColumns.id);
                            _GetBankDetail.bank_name = Ireader.GetString(CommonColumns.bank_name);
                        };
                        GetBankDetail.Add(_GetBankDetail);
                    }

                }

                return GetBankDetail;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindStatusLookUp ");
                throw ex;
            }
            finally
            {
                GetBankDetail = null;
                _GetBankDetail = null;
            }
        }

        public List<CustomerDetailsById> GetCustomerDetailsById(string customer_id)
        {
            List<CustomerDetailsById> BindCustomer = new List<CustomerDetailsById>();
            CustomerDetailsById _BindCustomer;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerDetailsById);
                    while (Ireader.Read())
                    {
                        _BindCustomer = new CustomerDetailsById();
                        {
                            _BindCustomer.Customer_id = Ireader.GetInt32(CommonColumns.id);
                            _BindCustomer.name1 = Ireader.GetString(CommonColumns.name1);
                            _BindCustomer.email = Ireader.GetString(CommonColumns.email);
                            _BindCustomer.address = Ireader.GetString(CommonColumns.JobSite);
                            _BindCustomer.Source_id = Ireader.GetInt32(CommonColumns.Source_id);
                            _BindCustomer.Source_Name = Ireader.GetString(CommonColumns.Source_Name);
                            _BindCustomer.JobSite = Ireader.GetString(CommonColumns.JobSite);
                        };

                        BindCustomer.Add(_BindCustomer);
                    }

                }

                return BindCustomer;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetCustomerDetailsById");
                throw ex;
            }
            finally
            {
                BindCustomer = null;
                _BindCustomer = null;
            }
        }
        public List<Salesmancommission> GetSalesmenDetailsById(string salesmen_id)
        {
            List<Salesmancommission> BindSalesman = new List<Salesmancommission>();
            Salesmancommission _BindSalesman;
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetSalesmenDetailsById);
                    while (Ireader.Read())
                    {
                        _BindSalesman = new Salesmancommission();
                        {
                            //_BindSalesman.id = Ireader.GetString(CommonColumns.id);
                            //_BindSalesman.branch_Id = Ireader.GetString(CommonColumns.branch_Id);
                            //_BindSalesman.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _BindSalesman.saleman_commission = Ireader.GetDecimal(CommonColumns.saleman_commission);

                        };

                        BindSalesman.Add(_BindSalesman);
                    }

                }

                return BindSalesman;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenDetailsById");
                throw ex;
            }
            finally
            {
                BindSalesman = null;
                _BindSalesman = null;
            }
        }
        public List<PackageDetail> GetQuotationForSearchPackage(PackageDetail PackagesSearchList)
        {
            List<PackageDetail> PackageList = new List<PackageDetail>();
            PackageDetail _PackageList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (PackagesSearchList.package_id == null || PackagesSearchList.package_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(PackagesSearchList.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    }
                    //cmd.AddParameters(PackagesSearchList.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    cmd.AddParameters(PackagesSearchList.plan_id, CommanConstans.Plan_id, SqlDbType.SmallInt);
                    cmd.AddParameters(PackagesSearchList.package_type_id, CommanConstans.package_type_id, SqlDbType.SmallInt);
                    cmd.AddParameters(PackagesSearchList.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(PackagesSearchList.minamount, CommanConstans.minamount, SqlDbType.Decimal);
                    cmd.AddParameters(PackagesSearchList.maxamount, CommanConstans.maxamount, SqlDbType.Decimal);
                    cmd.AddParameters(PackagesSearchList.search, CommanConstans.search, SqlDbType.VarChar);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branchId, SqlDbType.VarChar);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Quotation_For_SearchPackage);
                    while (Ireader.Read())
                    {
                        _PackageList = new PackageDetail();
                        {
                            _PackageList.plan_name = Ireader.GetString(CommonColumns.plan_name);
                            _PackageList.package_cd = Ireader.GetString(CommonColumns.package_cd);
                            _PackageList.package_name = Ireader.GetString(CommonColumns.package_name);
                            _PackageList.floor_name = Ireader.GetString(CommonColumns.floor_name);
                            //_PackageList.valid_from = Ireader.GetDateTime(CommonColumns.valid_from);
                            //_PackageList.valid_to = Ireader.GetDateTime(CommonColumns.valid_to);
                            _PackageList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                            _PackageList.package_id = Ireader.GetString(CommonColumns.package_id);
                        };

                        PackageList.Add(_PackageList);
                    }

                }

                return PackageList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationForSearchPackage");
                throw ex;
            }
            finally
            {
                _PackageList = null;
            }
        }

        public SuccessMessage Update_ProjectStatus(QuotationStatusCriteria _QuotationStatusCriteria, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_QuotationStatusCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationStatusCriteria.Status_Id, CommanConstans.Status_Id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationStatusCriteria.Reason, CommanConstans.Reason, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_ProjectStatus);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_ProjectStatus, Parameters =" + _QuotationStatusCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _QuotationStatusCriteria = null;
                }
            }
        }
        public SuccessMessage UpsertPackage_PackageDetails(NewPackageDetailTasksItem _UpsertPackageDetailsCriteria, NewPackageDetailTasksItem _UpsertPackageHeaderCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_UpsertPackageDetailsCriteria.package_id == null || _UpsertPackageDetailsCriteria.package_id == "0")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.package_id, SqlDbType.VarChar);
                        cmd.AddParameters(true, CommanConstans.isactive, SqlDbType.Bit);
                        // cmd.AddParameters("", CommanConstans.package_cd, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_UpsertPackageDetailsCriteria.package_id, CommanConstans.package_id, SqlDbType.VarChar);
                        cmd.AddParameters(_UpsertPackageHeaderCriteria.isactive, CommanConstans.isactive, SqlDbType.Bit);
                        // cmd.AddParameters(_UpsertPackageDetailsCriteria.package_cd, CommanConstans.package_cd, SqlDbType.VarChar);

                    }
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.plan.plan_id, CommanConstans.plan_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.package.package_type_id, CommanConstans.package_type_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.floor.floor_id, CommanConstans.floor_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.valid_from, CommanConstans.valid_from, SqlDbType.DateTime);
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.valid_to, CommanConstans.valid_to, SqlDbType.DateTime);
                    cmd.AddParameters(_UpsertPackageHeaderCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Package_Det_Id, CommanConstans.package_det_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Item.item_description, CommanConstans.item_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Item.item_id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Category.category_name, CommanConstans.category_name, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Category.category_Id, CommanConstans.category_id, SqlDbType.Int);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.BillingUOM.status_lookup_id, CommanConstans.price_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.UOM.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Qty, CommanConstans.qty, SqlDbType.Int);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);
                    //cmd.AddParameters(_UpsertPackageDetailsCriteria.userid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.item_remarks, CommanConstans.item_remarks, SqlDbType.NVarChar);
                    cmd.AddParameters(_UpsertPackageDetailsCriteria.Task_Name, CommanConstans.task_description, SqlDbType.NVarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_package_packageDetails);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.package_id);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackage_PackageDetails, Parameters =" + _UpsertPackageDetailsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _UpsertPackageDetailsCriteria = null;
                }
            }
        }

        public List<ContractTerm> ContractTermsList()
        {
            string result = string.Empty;
            List<ContractTerm> CTList = new List<ContractTerm>();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_contract_payment_terms);
                    while (ireader.Read())
                    {
                        ContractTerm contractTerm = new ContractTerm();
                        contractTerm.Id = ireader.GetInt32(CommonColumns.master_contract_term_id);
                        contractTerm.Description = ireader.GetString(CommonColumns.contract_desrcription);
                        contractTerm.ShortName = ireader.GetString(CommonColumns.ShortCode);
                        CTList.Add(contractTerm);
                    }
                    return CTList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CTList = null;
                }
            }
        }

        public SuccessMessage CreateContractTerm(ContractTerm CTList, Guid uid)
        {
            SuccessMessage __SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(CTList.Id, CommanConstans.master_contract_term_id, SqlDbType.Int);
                    cmd.AddParameters(CTList.Description, CommanConstans.contract_desrcription, SqlDbType.VarChar);
                    cmd.AddParameters(CTList.ShortName, CommanConstans.ShortCode, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.UniqueIdentifier);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_master_contract_terms);
                    while (ireader.Read())
                    {
                        __SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        __SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return __SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    // result = string.Empty;
                }
            }
        }

        public List<Contract> GetContractTerms(string project_id)
        {
            List<Contract> obj = new List<Contract>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(project_id), CommanConstans.project_Id, SqlDbType.UniqueIdentifier);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_ReportPrintForContract);
                    while (ireader.Read())
                    {
                        Contract contract = new Contract();
                        contract.task_description = ireader.GetString(CommonColumns.task_description);
                        contract.item_description = ireader.GetString(CommonColumns.item_description);
                        contract.Quantity = ireader.GetInt32(CommonColumns.Qty);
                        contract.Amount = ireader.GetDecimal(CommonColumns.amount);
                        contract.SubTotal = ireader.GetDecimal(CommonColumns.Subtotal);
                        obj.Add(contract);
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractTerms, project_id =" + project_id);
                return obj;
            }
            finally
            {
                obj = null;
            }
        }
        public List<ProjectContractTermsCriteria> GetProjectContractTermsList(string ProjectId)
        {
            List<ProjectContractTermsCriteria> GetProjectContractTerms = new List<ProjectContractTermsCriteria>();
            ProjectContractTermsCriteria _GetProjectContractTerms;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(ProjectId), CommanConstans.project_Id, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.get_ProjectContractTermsList);
                    while (Ireader.Read())
                    {
                        _GetProjectContractTerms = new ProjectContractTermsCriteria();
                        {
                            _GetProjectContractTerms.contract_term_id = Ireader.GetInt32(CommonColumns.contract_term_id);//master_payment_termid
                            _GetProjectContractTerms.master_contract_term_id = Ireader.GetInt32(CommonColumns.master_contract_term_id);
                            _GetProjectContractTerms.contract_desrcription = Ireader.GetString(CommonColumns.contract_desrcription);
                        };
                        GetProjectContractTerms.Add(_GetProjectContractTerms);
                    }

                }

                return GetProjectContractTerms;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindMasterpaymentterms");
                throw ex;
            }
            finally
            {
                GetProjectContractTerms = null;
                _GetProjectContractTerms = null;
            }
        }
        public List<VariationOrder> GetVO_Details(string vo_det_id)
        {
            List<VariationOrder> GetVOList = new List<VariationOrder>();
            VariationOrder _GetVOList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown UOMDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(vo_det_id), CommanConstans.vo_det_id, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VO_Details);
                    while (Ireader.Read())
                    {
                        _GetVOList = new VariationOrder();
                        {
                            _GetVOList.vo_det_id = Ireader.GetString(CommonColumns.vo_det_id);
                            _GetVOList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _GetVOList.BillingUOM = statusLookup;

                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _GetVOList.Item = itemDropDown;
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _GetVOList.Category = categoryDropDown;
                            UOMDropDown = new UOMDropDown();
                            {
                                UOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                UOMDropDown.uom_description = Ireader.GetString(CommonColumns.Uom);
                            };
                            _GetVOList.UOM = UOMDropDown;

                            _GetVOList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _GetVOList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            _GetVOList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _GetVOList.Qty = Ireader.GetString(CommonColumns.Qty);
                            _GetVOList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _GetVOList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _GetVOList.record_type = Ireader.GetInt32(CommonColumns.record_type);
                            _GetVOList.addition_omission_description = Ireader.GetString(CommonColumns.Addition_Omission_Description);
                        };
                        GetVOList.Add(_GetVOList);
                    }

                }

                return GetVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVO_Details");
                throw ex;
            }
            finally
            {
                GetVOList = null;
                _GetVOList = null;
            }
        }
        public SuccessMessage UpsertVO_Details(VariationOrder _VariationOrderDetailCriteria, string uid, string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_VariationOrderDetailCriteria.vo_det_id == null || _VariationOrderDetailCriteria.vo_det_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_VariationOrderDetailCriteria.vo_det_id, CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.record_type, CommanConstans.record_type, SqlDbType.Int);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Task_Name, CommanConstans.task_description, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Category.category_Id, CommanConstans.category_id, SqlDbType.Int);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Category.category_name, CommanConstans.category_name, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.UOM.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Item.item_id, CommanConstans.Item_id, SqlDbType.Int);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Item.item_description, CommanConstans.item_description, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.BillingUOM.status_lookup_id, CommanConstans.price_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Qty, CommanConstans.qty, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);
                    //cmd.AddParameters(_VariationOrderDetailCriteria.currentdate, CommanConstans.currentdate, SqlDbType.DateTime);
                    cmd.AddParameters(_VariationOrderDetailCriteria.item_remarks, CommanConstans.item_remarks, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Vo_Details);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.vo_det_id);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.Gst_Amount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.Gst_Percentage);

                        _SuccessMessage.addition_Amount = ireader.GetDecimal(CommonColumns.additionAmt);
                        _SuccessMessage.omission_Amount = ireader.GetDecimal(CommonColumns.OmissionAmt);
                        _SuccessMessage.TasksTotal_Amount = ireader.GetDecimal(CommonColumns.TasksTotalAMount);
                        _SuccessMessage.Vo_Id = ireader.GetString(CommonColumns.vo_id);
                        //_SuccessMessage.Status_Id = ireader.GetInt32(CommonColumns.Status_Id);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertVO_Details Parameters=" + _VariationOrderDetailCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _VariationOrderDetailCriteria = null;
                }
            }
        }

        public SuccessMessage Insert_Projects(CreateQuotationCriteria _CreateQuotationCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_CreateQuotationCriteria.project_id == null || _CreateQuotationCriteria.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters("", CommanConstans.project_number, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters(_CreateQuotationCriteria.project_number, CommanConstans.project_number, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_CreateQuotationCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);

                    if (_CreateQuotationCriteria.Customer_id != null && _CreateQuotationCriteria.salesmen_id != null)
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.Salesmen.id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(_CreateQuotationCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_date, CommanConstans.quotationForwardDate, SqlDbType.DateTime);
                    cmd.AddParameters(1, CommanConstans.is_new_record, SqlDbType.Bit);
                    cmd.AddParameters(_CreateQuotationCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.Package_Type_Id, CommanConstans.Package_Type_Id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_amount, CommanConstans.contract_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount, CommanConstans.discount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.bank_id, CommanConstans.bank_id, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.project_start_date, CommanConstans.Project_Start_Date, SqlDbType.DateTime);                    
                    cmd.AddParameters(_CreateQuotationCriteria.ReferenceNo, CommanConstans.ReferenceNo, SqlDbType.NVarChar);
                    //if (_CreateQuotationCriteria.UploadFile == null)
                    //{
                    //    _CreateQuotationCriteria.UploadFile = "";
                    //}
                    //cmd.AddParameters(_CreateQuotationCriteria.UploadFile, CommanConstans.UploadFile, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Quotation);//Insert_Projects
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.project_id);
                        _SuccessMessage.ProjectId = ireader.GetString(CommonColumns.id);
                        _SuccessMessage.Project_Number = ireader.GetString(CommonColumns.project_number);
                        
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Insert_Projects, Parameters =" + _CreateQuotationCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _CreateQuotationCriteria = null;
                }
            }
        }

        public SuccessMessage DeleteProjectQuotation(string uid, string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Project_Quotation);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProjectQuotation ProjectId= " + ProjectId.ToString());
                throw ex;
            }
            finally
            {
                ProjectId = null;
            }


        }

        public SuccessMessage DeleteProjectDetails(string uid, string Project_det_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Project_det_id, CommanConstans.Project_det_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.delete_ProjectDetails);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProjectDetails Project_det_id= " + Project_det_id.ToString());
                throw ex;
            }
            finally
            {
                Project_det_id = null;
            }


        }

        public SuccessMessage DeleteProjectPaymentTermsByID(string uid, string Payment_term_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Payment_term_id, CommanConstans.Payment_term_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_ProjectPaymentTermsByID);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteProjectPaymentTermsByID Payment_term_id= " + Payment_term_id.ToString());
                throw ex;
            }
            finally
            {
                Payment_term_id = null;
            }


        }

        public SuccessMessage DeletePackages(string uid, string package_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(package_id, CommanConstans.package_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Packages);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackages package_id= " + package_id.ToString());
                throw ex;
            }
            finally
            {
                package_id = null;
            }


        }
        public SuccessMessage deletepackageDetails(string uid, string package_det_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(package_det_id, CommanConstans.package_det_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.delete_packageDetails);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeletePackages package_det_id= " + package_det_id.ToString());
                throw ex;
            }
            finally
            {
                package_det_id = null;
            }


        }
        public SuccessMessage DeletePackagePaymentTermsByID(string uid, string Payment_term_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Payment_term_id, CommanConstans.payment_term_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_PackagePaymentTermsByID);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SSP_Delete_PackagePaymentTermsByID Payment_term_id= " + Payment_term_id.ToString());
                throw ex;
            }
            finally
            {
                Payment_term_id = null;
            }
        }
        public DateTime StartDateToIST(DateTime date)
        {
            //DateTime utcdate = new DateTime(date); //DateTime.ParseExact(date, "MM/dd/yyyy    hh: mm:ss", CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(date,
          TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
        public DateTime EndDateToIST(DateTime date)
        {
            // DateTime utcdate = DateTime.ParseExact(date, "MM/dd/yyyy    hh: mm:ss", CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(date.AddDays(-1),
          TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }

        public List<QuotationDetails> GetQuotationDetailsListByProjectId(string ProjectId)
        {
            List<QuotationDetails> obj = new List<QuotationDetails>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_QuotationDetailsByProject_id);
                    while (Ireader.Read())
                    {
                        QuotationDetails _GetQuotationDeatils = new QuotationDetails();
                        _GetQuotationDeatils.project_id = Ireader.GetString(CommonColumns.project_id);
                        _GetQuotationDeatils.id = Ireader.GetString(CommonColumns.id);
                        _GetQuotationDeatils.contract_date = Ireader.GetDateTime(CommonColumns.contract_date);
                        _GetQuotationDeatils.project_number = Ireader.GetString(CommonColumns.project_number);
                        _GetQuotationDeatils.project_name = Ireader.GetString(CommonColumns.project_name);
                        _GetQuotationDeatils.contract_amount = Ireader.GetDecimal(CommonColumns.contract_amount);
                        _GetQuotationDeatils.status_id = Ireader.GetInt32(CommonColumns.status_id);
                        _GetQuotationDeatils.salesmen_id = Ireader.GetInt32(CommonColumns.salesmen_id);
                        _GetQuotationDeatils.customer_id = Ireader.GetInt32(CommonColumns.customer_id);
                        _GetQuotationDeatils.project_start_date = Ireader.GetDateTime(CommonColumns.project_start_date);
                        _GetQuotationDeatils.project_end_date = Ireader.GetDateTime(CommonColumns.project_end_date);
                        _GetQuotationDeatils.quotationForwardDate = Ireader.GetDateTime(CommonColumns.quotationForwardDate);
                        _GetQuotationDeatils.quotationAcceptDate = Ireader.GetDateTime(CommonColumns.quotationAcceptDate);
                        _GetQuotationDeatils.discount = Ireader.GetDecimal(CommonColumns.discount);
                        _GetQuotationDeatils.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                        _GetQuotationDeatils.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetQuotationDeatils.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                        _GetQuotationDeatils.Email = Ireader.GetString(CommonColumns.Email);
                        _GetQuotationDeatils.salesmen = Ireader.GetString(CommonColumns.salesmen);
                        _GetQuotationDeatils.customer = Ireader.GetString(CommonColumns.customer);
                        _GetQuotationDeatils.status = Ireader.GetString(CommonColumns.status);
                        _GetQuotationDeatils.package_id = Ireader.GetString(CommonColumns.package_id);
                        _GetQuotationDeatils.Phone = Ireader.GetString(CommonColumns.Phone);
                        _GetQuotationDeatils.NRIC = Ireader.GetString(CommonColumns.NRIC);
                        _GetQuotationDeatils.Jobsite = Ireader.GetString(CommonColumns.project_name);
                        _GetQuotationDeatils.remarks = Ireader.GetString(CommonColumns.remarks);                        
                        obj.Add(_GetQuotationDeatils);
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetQuotationDetailsListByProjectId Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                obj = null;
            }
        }

        public SuccessMessage Update_ActDeactPackage(string PackageId, Boolean IsActive)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(PackageId, CommanConstans.Package_Id, SqlDbType.VarChar);
                    cmd.AddParameters(IsActive, CommanConstans.IsActive, SqlDbType.Bit);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.ActivateDeactivatePackage);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertPackagePaymentTerms Parameters:  PackageId =" + PackageId.ToString() + " ,IsActive=" + IsActive);
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                }
            }
        }

        public List<Source> BindSourceList()
        {
            List<Source> obSourceList = new List<Source>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindSource);
                    while (Ireader.Read())
                    {
                        Source _Source = new Source();
                        _Source.Source_Id = Ireader.GetInt32(CommonColumns.Source_Id);
                        _Source.Source_Name = Ireader.GetString(CommonColumns.Source_Name);
                        obSourceList.Add(_Source);
                    }
                }
                return obSourceList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSourceList");
                return null;
            }
            finally
            {
                obSourceList = null;
                //cmd = null;
            }
        }

        public SalesmanDropDown GetSalesmenIdByUserId(string uid)
        {
            SalesmanDropDown _GetSalesmen = new SalesmanDropDown();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_SalesmenByUserId);
                    while (Ireader.Read())
                    {
                        _GetSalesmen.id = Ireader.GetInt32(CommonColumns.id);
                        _GetSalesmen.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                    }

                }

                return _GetSalesmen;
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

        public List<VariationOrderList> GetVO(SalesmanDropDown salesmen, string ProjectId, VOListCriteria evoListCriteria)
        {
            List<VariationOrderList> GetVOList = new List<VariationOrderList>();
            VariationOrderList _GetVOList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (string.IsNullOrEmpty(ProjectId))
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.Project_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);

                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(salesmen.id, CommanConstans.salesmen_id, SqlDbType.Int);

                    // cmd.AddParameters(salesmen.salesmen_name, CommanConstans.sale, SqlDbType.BigInt);
                    cmd.AddParameters(evoListCriteria.ColSort, CommanConstans.ColSort, SqlDbType.Int);
                    cmd.AddParameters(evoListCriteria.OrderBy, CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(evoListCriteria.fromdate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(evoListCriteria.todate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(evoListCriteria.searchText, CommanConstans.searchText, SqlDbType.VarChar);
                    cmd.AddParameters(evoListCriteria.Type, CommanConstans.Type, SqlDbType.Int);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VO);
                    while (Ireader.Read())
                    {
                        _GetVOList = new VariationOrderList();
                        {

                            _GetVOList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                            _GetVOList.project_id = Ireader.GetString(CommonColumns.project_id);
                            _GetVOList.vo_number = Ireader.GetString(CommonColumns.vo_number);
                            _GetVOList.vo_date = Ireader.GetDateTime(CommonColumns.vo_date);
                            _GetVOList.project_number = Ireader.GetString(CommonColumns.project_number);
                            _GetVOList.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _GetVOList.name1 = Ireader.GetString(CommonColumns.name1);
                            _GetVOList.addressSite = Ireader.GetString(CommonColumns.AddressSite);
                            _GetVOList.amount = Ireader.GetDecimal(CommonColumns.amount);
                            _GetVOList.status_id = Ireader.GetInt32(CommonColumns.status_id);
                            _GetVOList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);


                            _GetVOList.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                            _GetVOList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                            _GetVOList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);

                            _GetVOList.isactive = Ireader.GetString(CommonColumns.isactive);
                            _GetVOList.createdBy = Ireader.GetString(CommonColumns.createdBy);
                            _GetVOList.created_date = Ireader.GetString(CommonColumns.created_date);
                            _GetVOList.modified_date = Ireader.GetString(CommonColumns.modified_date);
                            _GetVOList.modified_by = Ireader.GetString(CommonColumns.modified_by);

                            _GetVOList.is_new_record = Ireader.GetInt32(CommonColumns.is_new_record);
                            _GetVOList.version_no = Ireader.GetString(CommonColumns.version_no);

                            _GetVOList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                            _GetVOList.remarks = Ireader.GetString(CommonColumns.remarks);

                            //_GetVOList.record_type = Ireader.GetInt32(CommonColumns.record_type);
                            _GetVOList.addition_omission_description = Ireader.GetString(CommonColumns.addition_omission_description);
                            //_GetVOList.vo_number = Ireader.GetString(CommonColumns.vo_number);
                            _GetVOList.vo_number = Ireader.GetString(CommonColumns.VO_NO);

                        };
                        GetVOList.Add(_GetVOList);
                    }

                }

                return GetVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVO_Details");
                throw ex;
            }
            finally
            {
                GetVOList = null;
                _GetVOList = null;
            }
        }

        public List<ElectricalVariationOrderList> GetEVO(SalesmanDropDown salesmen, string ProjectId, EvoListCriteria _evoListCriteria)
        {
            List<ElectricalVariationOrderList> GetEVOList = new List<ElectricalVariationOrderList>();
            ElectricalVariationOrderList _GetEVOList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (string.IsNullOrEmpty(ProjectId))
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.Project_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);

                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(salesmen.id, CommanConstans.salesmen_id, SqlDbType.Int);
                    cmd.AddParameters(_evoListCriteria.ColSort, CommanConstans.ColSort, SqlDbType.Int);
                    cmd.AddParameters(_evoListCriteria.OrderBy, CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(_evoListCriteria.fromdate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(_evoListCriteria.todate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(_evoListCriteria.searchText, CommanConstans.searchText, SqlDbType.VarChar);
                    cmd.AddParameters(_evoListCriteria.Type, CommanConstans.Type, SqlDbType.Int);
                    // cmd.AddParameters(salesmen.salesmen_name, CommanConstans.sale, SqlDbType.BigInt);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVO);
                    while (Ireader.Read())
                    {
                        _GetEVOList = new ElectricalVariationOrderList();
                        {

                            _GetEVOList.evo_id = Ireader.GetString(CommonColumns.evouniquid);
                            _GetEVOList.project_id = Ireader.GetString(CommonColumns.project_id);
                            _GetEVOList.evo_number = Ireader.GetString(CommonColumns.EvoNo);
                            _GetEVOList.evoDate = Ireader.GetDateTime(CommonColumns.EvoDate);
                            _GetEVOList.project_number = Ireader.GetString(CommonColumns.project_number);
                            _GetEVOList.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _GetEVOList.name1 = Ireader.GetString(CommonColumns.name1);
                            _GetEVOList.addressSite = Ireader.GetString(CommonColumns.AddressSite);
                            _GetEVOList.amount = Ireader.GetDecimal(CommonColumns.amount);
                            _GetEVOList.status_id = Ireader.GetInt32(CommonColumns.status_id);
                            _GetEVOList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);


                            _GetEVOList.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                            _GetEVOList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                            _GetEVOList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);

                            _GetEVOList.isactive = Ireader.GetString(CommonColumns.isactive);
                            _GetEVOList.createdBy = Ireader.GetString(CommonColumns.createdBy);
                            _GetEVOList.created_date = Ireader.GetString(CommonColumns.created_date);
                            _GetEVOList.modified_date = Ireader.GetString(CommonColumns.modified_date);
                            _GetEVOList.modified_by = Ireader.GetString(CommonColumns.modified_by);

                            _GetEVOList.is_new_record = Ireader.GetInt32(CommonColumns.is_new_record);
                            _GetEVOList.version_no = Ireader.GetString(CommonColumns.version_no);

                            _GetEVOList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);
                            _GetEVOList.remarks = Ireader.GetString(CommonColumns.remarks);

                            _GetEVOList.evo_number = Ireader.GetString(CommonColumns.EvoNo);

                        };
                        GetEVOList.Add(_GetEVOList);
                    }

                }

                return GetEVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVO_Details");
                throw ex;
            }
            finally
            {
                GetEVOList = null;
                _GetEVOList = null;
            }
        }
        public SuccessMessage Upsert_Package_For_Clone(PackageList _PackageList)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {

                    cmd.AddParameters(_PackageList.userid, CommanConstans.userid, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.ExistingPackageId, CommanConstans.ExistingPackage, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.plan.plan_id, CommanConstans.plan_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.package.package_type_id, CommanConstans.package_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.floor.floor_id, CommanConstans.floor_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.valid_from, CommanConstans.valid_from, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.valid_to, CommanConstans.valid_to, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    if (!_PackageList.isGlobalpkg)
                        cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.Int);
                    else
                        cmd.AddParameters(0, CommanConstans.branch_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Package_For_Clone);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.package_id);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Upsert_Package_For_Clone Parameters:PackageList =" + _PackageList);
                    throw ex;
                }
                finally
                {
                    _PackageList = null;
                    _SuccessMessage = null;
                }
            }
        }

        public List<ProjectTasksItem> GetVOTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItem> ProjectTasksItemList = new List<ProjectTasksItem>();
            ProjectTasksItem _ProjectTasksItemList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //cmd.AddParameters(_ProjectTasksItemCriteria.record_type, CommanConstans.record_type, SqlDbType.Int);
                    if (_ProjectTasksItemCriteria.Vo_Id == null || _ProjectTasksItemCriteria.Vo_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Vo_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VOTasks_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItem();
                        {
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _ProjectTasksItemList.Task = taskDropDown;
                            //_ProjectTasksItemList = Ireader.GetString(CommonColumns.Task_Id);
                            //_ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }


        public List<VariationOrder> GetVOTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<VariationOrder> ProjectTasksItemList = new List<VariationOrder>();
            VariationOrder _ProjectTasksItemList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown uOMDropDown;
            //TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectTasksItemCriteria.Vo_Id == null || _ProjectTasksItemCriteria.Vo_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Vo_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    if (_ProjectTasksItemCriteria.Project_Det_Id == null || _ProjectTasksItemCriteria.Project_Det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Project_Det_Id, CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }

                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    //cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    // cmd.AddParameters(_ProjectTasksItemCriteria.record_type, CommanConstans.record_type, SqlDbType.Int);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VO_ItemDetails);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new VariationOrder();
                        {
                            _ProjectTasksItemList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                            _ProjectTasksItemList.vo_det_id = Ireader.GetString(CommonColumns.vo_det_id);
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _ProjectTasksItemList.Category = categoryDropDown;
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ProjectTasksItemList.Item = itemDropDown;
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _ProjectTasksItemList.BillingUOM = statusLookup;
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.Uom);
                            };
                            _ProjectTasksItemList.UOM = uOMDropDown;
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            _ProjectTasksItemList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _ProjectTasksItemList.Qty = Ireader.GetString(CommonColumns.Qty);
                            _ProjectTasksItemList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _ProjectTasksItemList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _ProjectTasksItemList.record_type = Ireader.GetInt32(CommonColumns.record_type);
                            _ProjectTasksItemList.Highlight = Ireader.GetBoolean(CommonColumns.Highlight);
                            _ProjectTasksItemList.StatusId = Ireader.GetInt32(CommonColumns.status_id);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public SuccessMessage Delete_Vo_Details(string uid, string vo_det_id, string ProjectId, string record_type)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(record_type, CommanConstans.record_type, SqlDbType.Int);
                    cmd.AddParameters(vo_det_id, CommanConstans.vo_det_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Vo_Details);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.Gst_Amount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.Gst_Percentage);

                        _SuccessMessage.addition_Amount = ireader.GetDecimal(CommonColumns.additionAmt);
                        _SuccessMessage.omission_Amount = ireader.GetDecimal(CommonColumns.OmissionAmt);
                        // AdditionAmt,OmissionAmt,Gst_Percentage,Gst_Amount

                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Vo_Details ProjectId= " + vo_det_id.ToString());
                throw ex;
            }
            finally
            {
                vo_det_id = null;
            }


        }
        public SuccessMessage Delete_vo(string uid, string vo_id)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(vo_id, CommanConstans.vo_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_vo);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return _SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Vo ProjectId= " + vo_id.ToString());
                throw ex;
            }
            finally
            {
                vo_id = null;
            }


        }
        public SuccessMessage UpsertProjectQuotation_Clone(CreateQuotationCriteria _CreateQuotationCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_CreateQuotationCriteria.ExistingProjectId, CommanConstans.ExistingProject_id, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    cmd.AddParameters(_CreateQuotationCriteria.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(_CreateQuotationCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_date, CommanConstans.quotationForwardDate, SqlDbType.DateTime);
                    cmd.AddParameters(1, CommanConstans.is_new_record, SqlDbType.Bit);
                    cmd.AddParameters(_CreateQuotationCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(uid, CommanConstans.userid, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Clone);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.Project_id);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation_Clone, Parameters =" + _CreateQuotationCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _CreateQuotationCriteria = null;
                }
            }
        }

        public SuccessMessage Upsert_Package_For_Clone_InProject(PackageList _PackageList)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {

                    cmd.AddParameters(_PackageList.userid, CommanConstans.userid, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.ExistingProjectId, CommanConstans.ExistingProject_id, SqlDbType.VarChar);
                    cmd.AddParameters(_PackageList.plan.plan_id, CommanConstans.plan_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.package.package_type_id, CommanConstans.package_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.floor.floor_id, CommanConstans.floor_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_PackageList.valid_from, CommanConstans.valid_from, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.valid_to, CommanConstans.valid_to, SqlDbType.DateTime);
                    cmd.AddParameters(_PackageList.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_Package_From_QuotationClone);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.package_id);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Upsert_Package_For_Clone_InProject Parameters:PackageList =" + _PackageList);
                    throw ex;
                }
                finally
                {
                    _PackageList = null;
                    _SuccessMessage = null;
                }
            }
        }
        public SuccessMessage UpsertProjectQuotation_History(CreateQuotationCriteria _CreateQuotationCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_CreateQuotationCriteria.project_id == null || _CreateQuotationCriteria.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters("", CommanConstans.project_number, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                        cmd.AddParameters(_CreateQuotationCriteria.project_number, CommanConstans.project_number, SqlDbType.VarChar);
                    }

                    cmd.AddParameters(_CreateQuotationCriteria.project_name, CommanConstans.project_name, SqlDbType.VarChar);

                    if (_CreateQuotationCriteria.Customer_id != null && _CreateQuotationCriteria.salesmen_id != null)
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    else
                    {
                        cmd.AddParameters(_CreateQuotationCriteria.Salesmen.id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                        cmd.AddParameters(_CreateQuotationCriteria.Customer.Customer_id, CommanConstans.customer_id, SqlDbType.BigInt);
                    }
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(_CreateQuotationCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_date, CommanConstans.quotationForwardDate, SqlDbType.DateTime);
                    cmd.AddParameters(1, CommanConstans.is_new_record, SqlDbType.Bit);
                    cmd.AddParameters(_CreateQuotationCriteria.reason, CommanConstans.reason, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.Package_Type_Id, CommanConstans.Package_Type_Id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.plan_id, CommanConstans.plan_id, SqlDbType.SmallInt);
                    cmd.AddParameters(_CreateQuotationCriteria.floor_id, CommanConstans.floor_id, SqlDbType.SmallInt);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_CreateQuotationCriteria.saleman_commission, CommanConstans.saleman_commission, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.contract_amount, CommanConstans.contract_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.discount, CommanConstans.discount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_CreateQuotationCriteria.version_no, CommanConstans.version_no, SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_Project_Quotation_History);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.project_id);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectQuotation_History, Parameters =" + _CreateQuotationCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _CreateQuotationCriteria = null;
                }
            }
        }
        public SuccessMessage UpsertProjectDetails_History(QuotationUpsertProjectDetails _QuotationUpsertProjectDetails)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_QuotationUpsertProjectDetails.project_det_Id == null || _QuotationUpsertProjectDetails.project_det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_QuotationUpsertProjectDetails.project_det_Id, CommanConstans.project_det_Id, SqlDbType.VarChar);
                    }
                    if (_QuotationUpsertProjectDetails.project_id == null || _QuotationUpsertProjectDetails.project_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_QuotationUpsertProjectDetails.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_QuotationUpsertProjectDetails.item_remarks, CommanConstans.item_remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.BillingUOM.status_lookup_id, CommanConstans.price_type_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Task_Id, CommanConstans.task_id, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Item.item_id, CommanConstans.item_id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Task_Name, CommanConstans.task_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Item.item_description, CommanConstans.item_description, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Category.category_Id, CommanConstans.category_id, SqlDbType.Int);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.UOM.uom_id, CommanConstans.uom_id, SqlDbType.TinyInt);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.price, CommanConstans.price, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.qty, CommanConstans.qty, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.userId, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Category.category_name, CommanConstans.category_name, SqlDbType.NVarChar);
                    cmd.AddParameters(_QuotationUpsertProjectDetails.Highlight, CommanConstans.Highlight, SqlDbType.Bit);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_ProjectDetails_History);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
                        _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
                        _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
                        _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertProjectDetails_History, Parameters =" + _QuotationUpsertProjectDetails.ToString());
                    throw ex;
                }
                finally
                {
                    _QuotationUpsertProjectDetails = null;
                }
            }
        }

        public VariationOrderList GetVODetailsByProjectId(string ProjectId, string VO_Id)
        {
            VariationOrderList _GetVOList = new VariationOrderList();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (VO_Id == null || VO_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(VO_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VO_DetailsByProject_id);
                    while (Ireader.Read())
                    {

                        _GetVOList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                        _GetVOList.vo_number = Ireader.GetString(CommonColumns.VO_NO);
                        _GetVOList.voDate = Ireader.GetString(CommonColumns.vo_date);
                        _GetVOList.salesmen_name = Ireader.GetString(CommonColumns.salesmen);
                        _GetVOList.name1 = Ireader.GetString(CommonColumns.customer);
                        _GetVOList.addressSite = Ireader.GetString(CommonColumns.project_name);
                        _GetVOList.amount = Ireader.GetDecimal(CommonColumns.amount);
                        _GetVOList.status = Ireader.GetString(CommonColumns.status);
                        _GetVOList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);
                        _GetVOList.discount_percentage = Ireader.GetDecimal(CommonColumns.discountpercentage);
                        _GetVOList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetVOList.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                        _GetVOList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);

                        _GetVOList.addition_Amount = Ireader.GetDecimal(CommonColumns.additionAmt);
                        _GetVOList.omission_Amount = Ireader.GetDecimal(CommonColumns.OmissionAmt);

                        _GetVOList.createdBy = Ireader.GetString(CommonColumns.created_by);
                        _GetVOList.modified_by = Ireader.GetString(CommonColumns.modified_by);
                        _GetVOList.created_date = Ireader.GetString(CommonColumns.created_date);
                        _GetVOList.modified_date = Ireader.GetString(CommonColumns.modified_date);
                    }

                }

                return _GetVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVODetailsByProjectId Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                _GetVOList = null;
            }
        }

        public VariationOrderList GetEVODetailsByProjectId(string ProjectId, string EVO_Id)
        {
            VariationOrderList _GetVOList = new VariationOrderList();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (EVO_Id == null || EVO_Id == "")
                    {
                        cmd.AddParameters(new Guid("00000000-0000-0000-0000-000000000000"), CommanConstans.evo_id, SqlDbType.UniqueIdentifier);

                    }
                    else
                    {
                        cmd.AddParameters(new Guid(EVO_Id), CommanConstans.evo_id, SqlDbType.UniqueIdentifier);
                    }
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVO_DetailsByProject_id);
                    while (Ireader.Read())
                    {

                        _GetVOList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                        _GetVOList.vo_number = Ireader.GetString(CommonColumns.VO_NO);
                        _GetVOList.voDate = Ireader.GetString(CommonColumns.vo_date);
                        _GetVOList.salesmen_name = Ireader.GetString(CommonColumns.salesmen);
                        _GetVOList.name1 = Ireader.GetString(CommonColumns.customer);
                        _GetVOList.addressSite = Ireader.GetString(CommonColumns.project_name);
                        _GetVOList.amount = Ireader.GetDecimal(CommonColumns.amount);
                        _GetVOList.status = Ireader.GetString(CommonColumns.status);
                        _GetVOList.status_id = Ireader.GetInt32(CommonColumns.EvoStatus_id);
                        _GetVOList.discount_amount = Ireader.GetDecimal(CommonColumns.discount_amount);
                        _GetVOList.discount_percentage = Ireader.GetDecimal(CommonColumns.discountpercentage);
                        _GetVOList.gst_amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                        _GetVOList.gst_percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                        _GetVOList.total_amount = Ireader.GetDecimal(CommonColumns.total_amount);

                        _GetVOList.addition_Amount = Ireader.GetDecimal(CommonColumns.additionAmt);
                        _GetVOList.omission_Amount = Ireader.GetDecimal(CommonColumns.OmissionAmt);

                        _GetVOList.createdBy = Ireader.GetString(CommonColumns.created_by);
                        _GetVOList.modified_by = Ireader.GetString(CommonColumns.modified_by);
                        _GetVOList.created_date = Ireader.GetString(CommonColumns.created_date);
                        _GetVOList.modified_date = Ireader.GetString(CommonColumns.modified_date);
                    }

                }

                return _GetVOList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetEVODetailsByProjectId Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                _GetVOList = null;
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

        public SuccessMessage UpsertEvoSignature(Signature _Signature, string uid)
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
                    cmd.AddParameters(0, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(0, CommanConstans.SUBSUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(_Signature.FILE_TYPE, CommanConstans.FILE_TYPE, SqlDbType.VarChar);

                    //cmd.AddParameters(_Signature.CustomerImage_PATH, CommanConstans.CUSTOMER_DOCUMENT_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(_Signature.SalesmenImage_PATH, CommanConstans.SalesmenImage_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(0, CommanConstans.DOCUMENT_CONTENT_TYPE_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.DOC_CONFIG_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.COMPANY_ID, SqlDbType.Int);
                    cmd.AddParameters(_Signature.Internal_No, CommanConstans.Internal_evo_num, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.UpsertEvoSignature);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertEvoSignature, Parameters =" + _Signature.ToString());
                    throw ex;
                }
                finally
                {
                    _Signature = null;
                }
            }
        }

        public SuccessMessage UpsertSignatureForProjectBudget(Signature _Signature, string uid, SuccessMessage successMessage,string UserName)
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
                    cmd.AddParameters(0, CommanConstans.SUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(0, CommanConstans.SUBSUBDETAILS_ID, SqlDbType.Int);
                    cmd.AddParameters(_Signature.FILE_TYPE, CommanConstans.FILE_TYPE, SqlDbType.VarChar);

                    //cmd.AddParameters(_Signature.CustomerImage_PATH, CommanConstans.CUSTOMER_DOCUMENT_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(_Signature.SalesmenImage_PATH, CommanConstans.SalesmenImage_PATH, SqlDbType.VarChar);
                    cmd.AddParameters(0, CommanConstans.DOCUMENT_CONTENT_TYPE_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.DOC_CONFIG_ID, SqlDbType.Int);
                    cmd.AddParameters(1, CommanConstans.COMPANY_ID, SqlDbType.Int);
                    cmd.AddParameters(successMessage.Id, CommanConstans.project_budget_details_id, SqlDbType.Int);
                    cmd.AddParameters(UserName, CommanConstans.UserName, SqlDbType.NVarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.UpsertSignatureForProjectBudget);
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

        public SuccessMessage Update_ContractStatus(QuotationStatusCriteria _QuotationStatusCriteria, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_QuotationStatusCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_QuotationStatusCriteria.Status_Id, CommanConstans.Status_Id, SqlDbType.Int);
                    //cmd.AddParameters(_QuotationStatusCriteria.Reason, CommanConstans.sign_path, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_ContractStatus);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_ProjectStatus, Parameters =" + _QuotationStatusCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _QuotationStatusCriteria = null;
                }
            }
        }


        public List<ProjectTasksItemList> GetVOTasksListItem(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItemList> ProjectTasksItemList = new List<ProjectTasksItemList>();
            ProjectTasksItemList _ProjectTasksItemList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectTasksItemCriteria.Vo_Id == null || _ProjectTasksItemCriteria.Vo_Id == "")
                    {

                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {

                        cmd.AddParameters(_ProjectTasksItemCriteria.Vo_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VOTasksList_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItemList();
                        {
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItemList Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }
        public SuccessMessage Validation_VO_Omission(VariationOrder _VariationOrderDetailCriteria, string ProjectId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (_VariationOrderDetailCriteria.vo_det_id == null || _VariationOrderDetailCriteria.vo_det_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_det_id, SqlDbType.VarChar);
                        //cmd.AddParameters(_VariationOrderDetailCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);

                    }
                    else
                    {
                        cmd.AddParameters(_VariationOrderDetailCriteria.vo_det_id, CommanConstans.vo_det_id, SqlDbType.VarChar);
                        //cmd.AddParameters(-(_VariationOrderDetailCriteria.Amount), CommanConstans.amount, SqlDbType.Decimal);

                    }
                    cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Item.item_id, CommanConstans.Item_id, SqlDbType.Int);
                    cmd.AddParameters(_VariationOrderDetailCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Validation_VO_Omission);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Validation_VO_Omission Parameters=vo_det_id" + _VariationOrderDetailCriteria.vo_det_id + " ,Qty" + _VariationOrderDetailCriteria.Qty + " ,ProjectId" + ProjectId);
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _VariationOrderDetailCriteria = null;
                }
            }
        }

        public SuccessMessage Update_VOStatus(VOStatusCriteria _VOStatusCriteria, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_VOStatusCriteria.VO_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    cmd.AddParameters(_VOStatusCriteria.Status_Id, CommanConstans.Status_Id, SqlDbType.Int);
                    //cmd.AddParameters(_QuotationStatusCriteria.Reason, CommanConstans.sign_path, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_VOStatus);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_VOStatus, Parameters =" + _VOStatusCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _VOStatusCriteria = null;
                }
            }
        }

        public SuccessMessage Update_EVOStatus(VOStatusCriteria _VOStatusCriteria, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_VOStatusCriteria.VO_Id, CommanConstans.evo_id, SqlDbType.VarChar);
                    cmd.AddParameters(_VOStatusCriteria.Status_Id, CommanConstans.Status_Id, SqlDbType.Int);
                    //cmd.AddParameters(_QuotationStatusCriteria.Reason, CommanConstans.sign_path, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_EVOStatus);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_VOStatus, Parameters =" + _VOStatusCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _VOStatusCriteria = null;
                }
            }
        }

        public SuccessMessage UpsertVO(VariationOrderCriteria _VariationOrderCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    var VoId = new Guid();
                    var ProjectId = new Guid();
                    Guid.TryParse(_VariationOrderCriteria.vo_id, out VoId);
                    Guid.TryParse(_VariationOrderCriteria.project_id, out ProjectId);
                    cmd.AddParameters(VoId, CommanConstans.vo_id, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(ProjectId, CommanConstans.project_id, SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(_VariationOrderCriteria.contract_amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderCriteria.discount, CommanConstans.discount_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderCriteria.discount_percentage, CommanConstans.discount_percentage, SqlDbType.Float);
                    cmd.AddParameters(_VariationOrderCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_VariationOrderCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_VariationOrderCriteria.Vo_date, CommanConstans.Vo_date, SqlDbType.DateTime);
                    cmd.AddParameters(_VariationOrderCriteria.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(_VariationOrderCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_VO);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.vo_id);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertVO Parameters=" + _VariationOrderCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _VariationOrderCriteria = null;
                }
            }
        }

        public SuccessMessage UpsertEVO(VariationOrderCriteria _EVOCriteria, string uid)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {

                    cmd.AddParameters(_EVOCriteria.vo_id, CommanConstans.evo_id, SqlDbType.VarChar);
                    cmd.AddParameters(_EVOCriteria.project_id, CommanConstans.project_id, SqlDbType.VarChar);
                    cmd.AddParameters(_EVOCriteria.contract_amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.gst_percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.gst_amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.discount, CommanConstans.discount_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.discount_percentage, CommanConstans.discount_percentage, SqlDbType.Float);
                    cmd.AddParameters(_EVOCriteria.total_amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.amountBeforeDiscount, CommanConstans.amountBeforeDiscount, SqlDbType.Decimal);
                    cmd.AddParameters(_EVOCriteria.status_id, CommanConstans.status_id, SqlDbType.Int);
                    cmd.AddParameters(_EVOCriteria.Vo_date, CommanConstans.evo_date, SqlDbType.DateTime);
                    cmd.AddParameters(_EVOCriteria.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(_EVOCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_EVO);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.evo_id);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertVO Parameters=" + _EVOCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _SuccessMessage = null;
                    _EVOCriteria = null;
                }
            }
        }

        public List<ReceiptsList> ReceiptsList(Receipts _Receipts)
        {
            List<ReceiptsList> ReceiptsList = new List<ReceiptsList>();
            ReceiptsList _ReceiptsList;
            AddressDropDown _AddressDropDownList;
            Banks _BanksDropDownList;
            ModeOfPaymentDropDown _ModeOfPaymentDropDownList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(0, CommanConstans.ID, SqlDbType.BigInt);
                    cmd.AddParameters(_Receipts.UserID, CommanConstans.UserID, SqlDbType.VarChar);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branchId, SqlDbType.Int);
                    cmd.AddParameters(_Receipts.FromDate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(_Receipts.ToDate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(_Receipts.ProjectId, CommanConstans.projectid, SqlDbType.BigInt);
                    cmd.AddParameters(1, CommanConstans.startRowIndex, SqlDbType.BigInt);
                    cmd.AddParameters(20, CommanConstans.pageSize, SqlDbType.BigInt);
                    cmd.AddParameters("receipt_date", CommanConstans.ColSort, SqlDbType.VarChar);
                    cmd.AddParameters("Dsc", CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(_Receipts.salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(_Receipts.SearchString, CommanConstans.SearchString, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerPayment);
                    while (Ireader.Read())
                    {
                        _ReceiptsList = new ReceiptsList();
                        {
                            _ReceiptsList.id = Ireader.GetInt32(CommonColumns.id);
                            _ReceiptsList.receipt_date = Ireader.GetDateTime(CommonColumns.receipt_date);
                            //_ReceiptsList.project_name = Ireader.GetString(CommonColumns.project_name);
                            _AddressDropDownList = new AddressDropDown();
                            {
                                _AddressDropDownList.id = Ireader.GetInt32(CommonColumns.id);
                                _AddressDropDownList.AddressSite = Ireader.GetString(CommonColumns.project_name);
                            };
                            _ReceiptsList.project_name = _AddressDropDownList;
                            //_ReceiptsList.mode_of_payment = Ireader.GetString(CommonColumns.mode_of_payment);
                            _ModeOfPaymentDropDownList = new ModeOfPaymentDropDown();
                            {
                                _ModeOfPaymentDropDownList.MOP_id = Ireader.GetInt32(CommonColumns.mode_of_payment_id);
                                _ModeOfPaymentDropDownList.MOP_Description = Ireader.GetString(CommonColumns.mode_of_payment);
                            };
                            _ReceiptsList.mode_of_payment = _ModeOfPaymentDropDownList;
                            // _ReceiptsList.bank_name = Ireader.GetString(CommonColumns.bank_name);
                            _BanksDropDownList = new Banks();
                            {
                                _BanksDropDownList.bank_id = Ireader.GetString(CommonColumns.bank_id);
                                _BanksDropDownList.bank_name = Ireader.GetString(CommonColumns.bank_name);
                            };
                            _ReceiptsList.Banks = _BanksDropDownList;
                            _ReceiptsList.cheque_details = Ireader.GetString(CommonColumns.cheque_details);
                            _ReceiptsList.remarks = Ireader.GetString(CommonColumns.remarks);
                            _ReceiptsList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                            _ReceiptsList.Amount = Ireader.GetDecimal(CommonColumns.amount);
                            _ReceiptsList.Gst_Percentage = Ireader.GetDecimal(CommonColumns.gst_percentage);
                            _ReceiptsList.Gst_Amount = Ireader.GetDecimal(CommonColumns.gst_amount);
                            _ReceiptsList.Total_Amount = Ireader.GetDecimal(CommonColumns.total_amount);

                            //Rk id  receipt_date project_name    mode_of_payment bank_name   
                            //    cheque_details remarks CreatedUpdated bank_id mode_of_payment_id TotalRecords

                        };
                        ReceiptsList.Add(_ReceiptsList);
                    }

                }

                return ReceiptsList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ReceiptsList Receipts=" + _Receipts.ToString());
                throw ex;
            }
            finally
            {
                ReceiptsList = null;
                _Receipts = null;
                _ReceiptsList = null;
            }
        }

        public List<QuotationDetails> Get_Documents(string ProjectId, int ID_TYPE)
        {
            List<QuotationDetails> obj = new List<QuotationDetails>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectId, CommanConstans.SUPER_ID, SqlDbType.VarChar);
                    cmd.AddParameters(ID_TYPE, CommanConstans.ID_TYPE, SqlDbType.SmallInt);
                    cmd.AddParameters(1, CommanConstans.COMPANY_ID, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.get_documents);
                    while (Ireader.Read())
                    {
                        QuotationDetails _GetQuotationDeatils = new QuotationDetails();
                        _GetQuotationDeatils.doc_id = Ireader.GetString(CommonColumns.id);
                        _GetQuotationDeatils.document_path = Ireader.GetString(CommonColumns.document_path);
                        _GetQuotationDeatils.id_type = Ireader.GetString(CommonColumns.Task_Id_Type);
                        // _GetQuotationDeatils.document_name = Ireader.GetString(CommonColumns.document_name);
                        _GetQuotationDeatils.project_number = Ireader.GetString(CommonColumns.project_number);
                        obj.Add(_GetQuotationDeatils);
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Documents Parameters= " + ProjectId);
                throw ex;
            }
            finally
            {
                obj = null;
            }
        }

        public List<QuotationDetails> Get_DocumentsForProjectBudget(int ProjectBudgetDetailsId)
        {
            List<QuotationDetails> obj = new List<QuotationDetails>();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ProjectBudgetDetailsId, CommanConstans.project_budget_details_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Documents_For_Project_Budget);
                    while (Ireader.Read())
                    {
                        QuotationDetails _GetQuotationDeatils = new QuotationDetails();
                        _GetQuotationDeatils.doc_id = Ireader.GetString(CommonColumns.id);
                        _GetQuotationDeatils.document_path = Ireader.GetString(CommonColumns.document_path);
                        _GetQuotationDeatils.id_type = Ireader.GetString(CommonColumns.Task_Id_Type);
                        _GetQuotationDeatils.project_number = Ireader.GetString(CommonColumns.project_number);
                        obj.Add(_GetQuotationDeatils);
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_DocumentsForProjectBudget Parameters= " + ProjectBudgetDetailsId);
                throw ex;
            }
            finally
            {
                obj = null;
            }
        }

        public SuccessMessage GetRowsCount(string ProjectId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_RowsCount);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetRowsCount, ProjectId =" + ProjectId);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage GetAdditionOmissionRowsCount(long ProjectId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_AO_RowsCount);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetAdditionOmissionRowsCount, ProjectId =" + ProjectId);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage GetEVORowsCount(string ProjectId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVO_RowsCount);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetEVORowsCount, ProjectId =" + ProjectId);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        //public List<projects_budget_details> GetBudgetedCostList(int supplier_id, string SearchString)
        //{
        //    List<projects_budget_details> projectsBudgetDetails = new List<projects_budget_details>();
        //    try
        //    {
        //        using (var cmd = new DBSqlCommand())
        //        {
        //            cmd.AddParameters(SessionManagement.SelectedBranchID, CommanConstans.branchId, SqlDbType.Int);
        //            cmd.AddParameters(supplier_id, CommanConstans.SupplierId, SqlDbType.Int);
        //            cmd.AddParameters(SearchString, CommanConstans.SearchString, SqlDbType.VarChar);
        //            IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ProjectList);
        //            while (Ireader.Read())
        //            {
        //                projects_budget_details projectsBudgetDetail = new projects_budget_details();
        //                {
        //                    projectsBudgetDetail.id = Ireader.GetInt32(CommonColumns.id);
        //                    projectsBudgetDetail.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
        //                    projectsBudgetDetail.Suppliers = Ireader.GetString(CommonColumns.Suppliers);
        //                    projectsBudgetDetail.Status = Ireader.GetString(CommonColumns.status);
        //                    projectsBudgetDetail.InvoiceAmount = Ireader.GetInt32(CommonColumns.Amount);
        //                    projectsBudgetDetail.CreatedUpdated = Ireader.GetString(CommonColumns.created_date);
        //                };
        //                projectsBudgetDetails.Add(projectsBudgetDetail);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: GetBudgetedCostList, Parameters= supplier_id" + supplier_id + "SearchString = " + SearchString);
        //    }
        //    finally
        //    {
        //        projectsBudgetDetails = null;
        //    }
        //    return projectsBudgetDetails;
        //}

        public SuccessMessage UpdateProjectStatus(int Id)
        {
            SuccessMessage obj = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommanConstans.ID, SqlDbType.Int);
                    cmd.AddParameters(8, CommanConstans.Status_Id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Update_ProjectBudget);
                    while (Ireader.Read())
                    {
                        obj.Result = Ireader.GetString(CommonColumns.Result);
                        obj.Errormessage = Ireader.GetString(CommonColumns.Errormessage);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpdateProjectStatus");
            }
            finally
            {
                obj = null;
            }
            return obj;
        }
        public SuccessMessage UpsertCustomerPayments(ReceiptsList _UpsertCustomerPaymentsCriteria, string uid)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.id, CommanConstans.ID, SqlDbType.BigInt);
                    //cmd.AddParameters(_UpsertCustomerPaymentsCriteria.ProjectId, CommanConstans.Project_Id, SqlDbType.BigInt);
                    //cmd.AddParameters(_UpsertCustomerPaymentsCriteria.receipt_date, CommanConstans.Receipt_Date, SqlDbType.DateTime);
                    //cmd.AddParameters(_UpsertCustomerPaymentsCriteria.mode_of_payment_id, CommanConstans.mode_of_payment, SqlDbType.Int);
                    //cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Bank_ID, CommanConstans.Bank_ID, SqlDbType.Int);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.project_name.id, CommanConstans.Project_Id, SqlDbType.BigInt);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.receipt_date, CommanConstans.Receipt_Date, SqlDbType.DateTime);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.mode_of_payment.MOP_id, CommanConstans.mode_of_payment, SqlDbType.Int);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Banks.bank_id, CommanConstans.Bank_ID, SqlDbType.Int);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.cheque_details, CommanConstans.cheque_Details, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.remarks, CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Amount, CommanConstans.amount, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.salesmen_id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Gst_Percentage, CommanConstans.gst_percentage, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Gst_Amount, CommanConstans.gst_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_UpsertCustomerPaymentsCriteria.Total_Amount, CommanConstans.total_amount, SqlDbType.Decimal);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Insert_Customer_Payments);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Id = ireader.GetString(CommonColumns.CustomerPaymentId);
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpsertCustomerPayments Parameters=" + _UpsertCustomerPaymentsCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                    _UpsertCustomerPaymentsCriteria = null;
                }
            }
        }
        public List<ModeOfPaymentDropDown> BindMode_of_Payments()
        {
            List<ModeOfPaymentDropDown> _ModeOfPaymentDropDownList = new List<ModeOfPaymentDropDown>();
            ModeOfPaymentDropDown _ModeOfPaymentDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindMode_of_Payments);
                    while (Ireader.Read())
                    {
                        _ModeOfPaymentDropDown = new ModeOfPaymentDropDown();
                        {
                            _ModeOfPaymentDropDown.MOP_id = Ireader.GetInt32(CommonColumns.id);
                            _ModeOfPaymentDropDown.MOP_Description = Ireader.GetString(CommonColumns.mode_of_payment);
                        };

                        _ModeOfPaymentDropDownList.Add(_ModeOfPaymentDropDown);
                    }

                }

                return _ModeOfPaymentDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindMode_of_Payments");
                throw ex;
            }
            finally
            {
                _ModeOfPaymentDropDown = null;
                _ModeOfPaymentDropDownList = null;
            }
        }
        public SuccessMessage Delete_Customer_Payments(string Id)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Id, CommanConstans.ID, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_Customer_Payments);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Delete_Customer_Payments Parameters= " + Id.ToString());
                throw ex;
            }
            finally
            {
                Id = null;
            }
        }

        public List<ProjectsBudget> ProjectsbudgetList(ProjectsBudgetView _ProjectsBudgetView)
        {
            List<ProjectsBudget> ProjectsBudgetList = new List<ProjectsBudget>();
            ProjectsBudget _ProjectsBudgetList;
            SelectListItem GstList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //if (string.IsNullOrEmpty(_ProjectsBudgetView.ProjectId))
                    //{
                    //    cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.Project_Id, SqlDbType.VarChar);
                    //}
                    //else
                    //{
                    //    cmd.AddParameters(_ProjectsBudgetView.ProjectId, CommanConstans.Project_Id, SqlDbType.VarChar);

                    //}
                    cmd.AddParameters(_ProjectsBudgetView.UserID, CommanConstans.UserID, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectsBudgetView.branchId, CommanConstans.branchId, SqlDbType.BigInt);
                    cmd.AddParameters("", CommanConstans.SearchString, SqlDbType.VarChar);
                    // cmd.AddParameters(_ProjectsBudgetView.SearchString, CommanConstans.SearchString, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectsBudgetView.SupplierId, CommanConstans.SupplierId, SqlDbType.Int);
                    cmd.AddParameters(_ProjectsBudgetView.SalesmenId, CommanConstans.SalesMenId, SqlDbType.Int);
                    cmd.AddParameters(_ProjectsBudgetView.fromDate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(_ProjectsBudgetView.endDate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(_ProjectsBudgetView.StatusId, CommanConstans.status, SqlDbType.NVarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Projects_budget_List_New);
                    while (Ireader.Read())
                    {
                        _ProjectsBudgetList = new ProjectsBudget();
                        {//project_budget_details_id  project_budget_id
                            _ProjectsBudgetList.project_budget_details_id = Ireader.GetInt32(CommonColumns.project_budget_details_id);
                            _ProjectsBudgetList.project_budget_id = Ireader.GetInt32(CommonColumns.project_budget_id);
                            _ProjectsBudgetList.project_id = Ireader.GetInt32(CommonColumns.project_id);
                            _ProjectsBudgetList.project_name = Ireader.GetString(CommonColumns.project_name);
                            _ProjectsBudgetList.supplier_id = Ireader.GetInt64(CommonColumns.supplier_id);
                            _ProjectsBudgetList.supplier_names = Ireader.GetString(CommonColumns.supplier_names);
                            _ProjectsBudgetList.status_id = Ireader.GetString(CommonColumns.status_id);
                            _ProjectsBudgetList.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _ProjectsBudgetList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                            _ProjectsBudgetList.Approved_amount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            _ProjectsBudgetList.status_name = Ireader.GetString(CommonColumns.status_name);
                            // _ProjectsBudgetList.remarks = Ireader.GetString(CommonColumns.remarks);
                            _ProjectsBudgetList.InvoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _ProjectsBudgetList.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            GstList = new SelectListItem();
                            {
                                GstList.Text = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                                GstList.Value = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                            }
                            _ProjectsBudgetList.GSTPercent = GstList;//GSTAmount GSTPercent InvoiceAmtWithGST
                            _ProjectsBudgetList.GSTAmount = Ireader.GetDecimal(CommonColumns.GSTAmount);
                            _ProjectsBudgetList.reason = Ireader.GetString(CommonColumns.Reason_Approval);

                        };

                        ProjectsBudgetList.Add(_ProjectsBudgetList);
                    }
                }
                return ProjectsBudgetList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindTasksList");
                throw ex;

            }
            finally
            {
                ProjectsBudgetList = null;
                _ProjectsBudgetList = null;
            }
        }        

        public SuccessMessage Update_ForApproval(ProjectsBudget _StatusCriteria)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_StatusCriteria.project_budget_details_id, CommanConstans.Project_Budget_Detail_Id, SqlDbType.BigInt);
                    cmd.AddParameters(_StatusCriteria.status_id, CommanConstans.Status_Id, SqlDbType.Int);
                    cmd.AddParameters(_StatusCriteria.Approved_amount, CommanConstans.Approved_amount, SqlDbType.Decimal);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_For_Approval);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_ForApproval, Parameters =" + _StatusCriteria.ToString());
                    _successMessage.Result = "0";
                    _successMessage.Errormessage = " Unhandled Exception while Updating the Status";
                    return _successMessage;
                }
                finally
                {
                    _StatusCriteria = null;
                }
            }
        }

        public SuccessMessage Update_ForApprovalWithReason(ProjectsBudget _StatusCriteria)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_StatusCriteria.project_budget_details_id, CommanConstans.Project_Budget_Detail_Id, SqlDbType.BigInt);
                    cmd.AddParameters(_StatusCriteria.status_id, CommanConstans.Status_Id, SqlDbType.Int);
                    cmd.AddParameters(_StatusCriteria.Approved_amount, CommanConstans.Approved_amount, SqlDbType.Decimal);
                    cmd.AddParameters(_StatusCriteria.reason, "reason", SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader("[dbo].[SSP_Update_For_Approval_WithReason]");
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Update_ForApprovalWithReason, Parameters =" + _StatusCriteria.ToString());
                    throw ex;
                }
                finally
                {
                    _StatusCriteria = null;
                }
            }
        }

        public SalesmenAndSupplierDetails GetSalesmenAndSupplierDetailsByProjectBudgetDetailsId(int project_budget_details_id)
        {
            SalesmenAndSupplierDetails _salesmenAndSupplierDetails = new SalesmenAndSupplierDetails();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(project_budget_details_id, CommanConstans.Project_Budget_Detail_Id, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_SalesmenDetailsByProjectBudgetDetailsId);
                    while (ireader.Read())
                    {
                        _salesmenAndSupplierDetails.Id = ireader.GetInt32(CommonColumns.id);
                        _salesmenAndSupplierDetails.SalesmenId = ireader.GetString(CommonColumns.salesmen_id);                        
                        _salesmenAndSupplierDetails.Supplier = new SupplierAddress()
                        {
                            SupplierName = ireader.GetString(CommonColumns.supplier_name),
                            Address1 = ireader.GetString(CommonColumns.Address1),
                            Address2 = ireader.GetString(CommonColumns.Address2),
                            Address3 = ireader.GetString(CommonColumns.Address3),
                            Address4 = ireader.GetString(CommonColumns.Address4),
                            ZipCode = ireader.GetString(CommonColumns.Zip_Code),
                        };

                        ireader.NextResult();
                        while (ireader.Read())
                        {
                            _salesmenAndSupplierDetails.DocPath = ireader.GetString(CommonColumns.document_path);
                            _salesmenAndSupplierDetails.ProjectNumber = ireader.GetString(CommonColumns.project_number);
                        }
                    }
                    return _salesmenAndSupplierDetails;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetSalesmenAndSupplierDetailsByProjectBudgetDetailsId, Parameters =" + project_budget_details_id.ToString());
                    throw ex;
                }
                finally
                {
                    project_budget_details_id = 0;
                }
            }
        }

        //public SuccessMessage AddBudgetDetails(AddBudget _AddBudget)
        //{
        //    SuccessMessage _successMessage = new SuccessMessage();
        //    using (var cmd = new DBSqlCommand())
        //    {
        //        try
        //        {
        //            cmd.AddParameters(_AddBudget.project_budget_details_id, CommanConstans.Project_Budget_Detail_Id, SqlDbType.BigInt);
        //            cmd.AddParameters(_AddBudget.project_budget_id, CommanConstans.project_budget_id, SqlDbType.Int);
        //            cmd.AddParameters(_AddBudget.supplier_id, CommanConstans.supplier_id, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.budget_amount, CommanConstans.budget_amount, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.remarks, CommanConstans.remarks, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.InvoiceNumber, CommanConstans.InvoiceNumber, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.InvoiceAmtWithGST, CommanConstans.InvoiceAmountwithGST, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.GSTPercent, CommanConstans.GSTPercent, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.GSTAmount, CommanConstans.GSTAmount, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.StatusId, CommanConstans.StatusId, SqlDbType.Decimal);
        //            cmd.AddParameters(_AddBudget.SalesMenId, CommanConstans.SalesMenId, SqlDbType.Decimal);
        //            IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Update_For_Approval);
        //            while (ireader.Read())
        //            {
        //                _successMessage.Result = ireader.GetString(CommonColumns.Result);
        //                _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
        //            }
        //            return _successMessage;
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionLog.WriteLog(ex, "Method Name: AddBudgetDetails, Parameters =" + _AddBudget.ToString());
        //            throw ex;
        //        }
        //        finally
        //        {
        //            _AddBudget = null;
        //        }
        //    }
        //}

        public List<SupplierDropDown> BindSupplier()
        {
            List<SupplierDropDown> _SupplierDropDownList = new List<SupplierDropDown>();
            SupplierDropDown _SupplierDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindSupplier);
                    while (Ireader.Read())
                    {
                        _SupplierDropDown = new SupplierDropDown();
                        {
                            _SupplierDropDown.Supplier_id = Ireader.GetInt32(CommonColumns.id);
                            _SupplierDropDown.Supplier_Name = Ireader.GetString(CommonColumns.supplier_name);
                        };

                        _SupplierDropDownList.Add(_SupplierDropDown);
                    }

                }

                return _SupplierDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
                throw ex;
            }
            finally
            {
                _SupplierDropDown = null;
                _SupplierDropDownList = null;
            }
        }

        public List<SupplierDropDown> BindSupplier(string userId)
        {
            List<SupplierDropDown> _SupplierDropDownList = new List<SupplierDropDown>();
            SupplierDropDown _SupplierDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(userId, CommanConstans.userId, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_BindSupplier_By_UserId);
                    while (Ireader.Read())
                    {
                        _SupplierDropDown = new SupplierDropDown();
                        {
                            _SupplierDropDown.Supplier_id = Ireader.GetInt32(CommonColumns.id);
                            _SupplierDropDown.Supplier_Name = Ireader.GetString(CommonColumns.supplier_name);
                        };

                        _SupplierDropDownList.Add(_SupplierDropDown);
                    }

                }

                return _SupplierDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
                throw ex;
            }
            finally
            {
                _SupplierDropDown = null;
                _SupplierDropDownList = null;
            }
        }

        public SuccessMessage AddBudgetDetails(AddBudget _AddBudget, bool IsSupplierCreated)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            var ZipCodeId = _AddBudget.Zip_CodeId;
            var unitCodeId = _AddBudget.Unit_CodeId;
            try
            {

                using (var cmd1 = new DBSqlCommand())
                {

                    cmd1.AddParameters(_AddBudget.project_budget_id, CommanConstans.ID, SqlDbType.BigInt);
                    cmd1.AddParameters(_AddBudget.SupplierId, CommanConstans.SupplierId, SqlDbType.BigInt);
                    cmd1.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    cmd1.AddParameters(_AddBudget.UserId, CommanConstans.user_id, SqlDbType.VarChar);
                    cmd1.AddParameters(1, CommanConstans.IsActive, SqlDbType.Bit);
                    IDataReader ireaderHeader = cmd1.ExecuteDataReader(SqlProcedures.Upsert_Projects_Budget_New);
                    while (ireaderHeader.Read())
                    {
                        _successMessage.Result = ireaderHeader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireaderHeader.GetString(CommonColumns.ErrorMessage);
                        _AddBudget.project_budget_id = ireaderHeader.GetString(CommonColumns.ProjectBudgetId);
                        _successMessage.project_budget_id = ireaderHeader.GetString(CommonColumns.ProjectBudgetId);
                    }
                    ireaderHeader.Close();
                }                
                using (var cmd = new DBSqlCommand())
                {
                    try
                    {
                        if (_successMessage.Result == "1" || _successMessage.Result == "2")
                        {
                            if (_AddBudget.StatusId == null)
                            {
                                if (IsSupplierCreated)
                                    _AddBudget.StatusId = 1;
                                else
                                    _AddBudget.StatusId = 7;
                            }
                            cmd.AddParameters(_AddBudget.project_budget_details_id, CommanConstans.project_budget_details_id, SqlDbType.BigInt);
                            cmd.AddParameters(_AddBudget.project_budget_id, CommanConstans.project_budget_id, SqlDbType.BigInt);
                            cmd.AddParameters(_AddBudget.Address.id, CommanConstans.Project_Id, SqlDbType.BigInt);
                            cmd.AddParameters(_AddBudget.budget_amount, CommanConstans.budget_amount, SqlDbType.Decimal);
                            cmd.AddParameters("", CommanConstans.remarks, SqlDbType.VarChar);
                            cmd.AddParameters(_AddBudget.InvoiceNumber, CommanConstans.InvoiceNumber, SqlDbType.VarChar);
                            cmd.AddParameters(_AddBudget.InvoiceAmtWithGST, CommanConstans.InvoiceAmountwithGST, SqlDbType.Decimal);
                            cmd.AddParameters(_AddBudget.GSTPercent.Value, CommanConstans.GSTPercent, SqlDbType.Decimal);
                            cmd.AddParameters(_AddBudget.GSTAmount, CommanConstans.GSTAmount, SqlDbType.Decimal);
                            cmd.AddParameters(_AddBudget.StatusId, CommanConstans.StatusId, SqlDbType.Int);
                            cmd.AddParameters(_AddBudget.Salesman.id, CommanConstans.SalesMenId, SqlDbType.Int);
                            cmd.AddParameters(_AddBudget.BudgetCostType.BudgetCostType_id, "BudgetCostType_id", SqlDbType.Int);
                            IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Add_Project_Budget_Detail_New);
                            while (ireader.Read())
                            {
                                _successMessage.Result = ireader.GetString(CommonColumns.Result);
                                _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                                _successMessage.project_budget_id = ireader.GetString(CommonColumns.ProjectBudgetId);
                                _successMessage.Id = ireader.GetString(CommonColumns.ProjectBudgetdetailsId);
                            }
                            return _successMessage;
                        }
                        else
                        {
                            return _successMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionLog.WriteLog(ex, "Method Name: AddBudgetDetails, Parameters =" + _AddBudget.ToString());
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: AddBudgetDetails, Parameters =" + _AddBudget.ToString());
                throw ex;
            }
            finally
            {
                _AddBudget = null;
            }

        }
        public SuccessMessage updateBudgetDetails(AddBudget _AddBudget, bool IsSupplierCreated)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            try
            {

                using (var cmd = new DBSqlCommand())
                {
                    if (_AddBudget.StatusId == null)
                    {
                        if (IsSupplierCreated)
                            _AddBudget.StatusId = 1;
                        else
                            _AddBudget.StatusId = 7;
                    }
                    cmd.AddParameters(_AddBudget.project_budget_details_id, CommanConstans.project_budget_details_id, SqlDbType.BigInt);
                    cmd.AddParameters(_AddBudget.project_budget_id, CommanConstans.project_budget_id, SqlDbType.BigInt);
                    cmd.AddParameters(_AddBudget.Address.id, CommanConstans.Project_Id, SqlDbType.BigInt);
                    cmd.AddParameters(_AddBudget.budget_amount, CommanConstans.budget_amount, SqlDbType.Decimal);
                    cmd.AddParameters("", CommanConstans.remarks, SqlDbType.VarChar);
                    cmd.AddParameters(_AddBudget.InvoiceNumber, CommanConstans.InvoiceNumber, SqlDbType.VarChar);
                    cmd.AddParameters(_AddBudget.InvoiceAmtWithGST, CommanConstans.InvoiceAmountwithGST, SqlDbType.Decimal);
                    cmd.AddParameters(_AddBudget.GSTPercent, CommanConstans.GSTPercent, SqlDbType.Decimal);
                    cmd.AddParameters(_AddBudget.GSTAmount, CommanConstans.GSTAmount, SqlDbType.Decimal);
                    cmd.AddParameters(_AddBudget.StatusId, CommanConstans.StatusId, SqlDbType.Int);
                    cmd.AddParameters(_AddBudget.Salesman.id, CommanConstans.SalesMenId, SqlDbType.Int);
                    cmd.AddParameters(_AddBudget.BudgetCostType.BudgetCostType_id, "BudgetCostType_id", SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Add_Project_Budget_Detail_New);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                        _successMessage.Id = ireader.GetString(CommonColumns.ProjectBudgetdetailsId);
                        _successMessage.project_budget_id = ireader.GetString(CommonColumns.ProjectbudgetId);
                    }
                    return _successMessage;

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: updateBudgetDetails, Parameters =" + _AddBudget.ToString());
                throw ex;
            }
            finally
            {
                _AddBudget = null;
            }

        }
        public List<Payment> PaymentList(Paymentview _Paymentview)
        {
            List<Payment> _Payment = new List<Payment>();
            Payment _PaymentList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //cmd.AddParameters(_Paymentview.UserID, CommanConstans.UserID, SqlDbType.VarChar);
                    //cmd.AddParameters(_Paymentview.branchId, CommanConstans.branchId, SqlDbType.BigInt);
                    //cmd.AddParameters("", CommanConstans.SearchString, SqlDbType.VarChar);
                    //// cmd.AddParameters(_ProjectsBudgetView.SearchString, CommanConstans.SearchString, SqlDbType.VarChar);
                    //cmd.AddParameters(_Paymentview.salesMenId, CommanConstans.salesMenId, SqlDbType.Int);
                    //cmd.AddParameters(_Paymentview.FromDate, CommanConstans.fromdate, SqlDbType.DateTime);
                    ////cmd.AddParameters(_ProjectsBudgetView.SalesmenId, CommanConstans.SalesMenId, SqlDbType.Int);
                    ///cmd.AddParameters(0, CommanConstans.ID, SqlDbType.BigInt);//SearchString  ToDate ProjectId SearchString
                    cmd.AddParameters(_Paymentview.UserID, CommanConstans.UserID, SqlDbType.VarChar);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branchId, SqlDbType.Int);
                    cmd.AddParameters(_Paymentview.FromDate, CommanConstans.fromdate, SqlDbType.DateTime);
                    cmd.AddParameters(_Paymentview.ToDate, CommanConstans.todate, SqlDbType.DateTime);
                    cmd.AddParameters(_Paymentview.ProjectId, CommanConstans.projectid, SqlDbType.BigInt);
                    cmd.AddParameters(1, CommanConstans.startRowIndex, SqlDbType.BigInt);
                    cmd.AddParameters(20, CommanConstans.pageSize, SqlDbType.BigInt);
                    cmd.AddParameters("payment_date", CommanConstans.ColSort, SqlDbType.VarChar);
                    cmd.AddParameters("Dsc", CommanConstans.OrderBy, SqlDbType.VarChar);
                    cmd.AddParameters(_Paymentview.salesMenId, CommanConstans.salesMenId, SqlDbType.BigInt);
                    cmd.AddParameters(_Paymentview.SearchString, CommanConstans.SearchString, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Payments);
                    while (Ireader.Read())
                    {
                        _PaymentList = new Payment();
                        {
                            _PaymentList.id = Ireader.GetInt32(CommonColumns.id);

                            _PaymentList.payment_date = Ireader.GetString(CommonColumns.payment_date);
                            _PaymentList.project_name = Ireader.GetString(CommonColumns.project_name);
                            _PaymentList.supplier_names = Ireader.GetString(CommonColumns.supplier_name);
                            _PaymentList.status_id = Ireader.GetString(CommonColumns.status_id);
                            _PaymentList.cheque_number = Ireader.GetDecimal(CommonColumns.cheque_number);
                            _PaymentList.remarks = Ireader.GetString(CommonColumns.remarks);
                            _PaymentList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                        };

                        _Payment.Add(_PaymentList);
                    }
                }
                return _Payment;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindTasksList");
                throw ex;

            }
            finally
            {
                _Paymentview = null;
                _PaymentList = null;
            }
        }

        public List<ProjectsBudget> Get_Project_BudgetById(ProjectsBudgetView _ProjectsBudgetView)
        {
            List<ProjectsBudget> ProjectsBudgetList = new List<ProjectsBudget>();
            ProjectsBudget _ProjectsBudgetList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(_ProjectsBudgetView.project_budget_id, CommanConstans.project_budget_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.get_Project_BudgetById);
                    while (Ireader.Read())
                    {
                        _ProjectsBudgetList = new ProjectsBudget();
                        {
                            _ProjectsBudgetList.project_budget_id = Ireader.GetInt64(CommonColumns.project_budget_id);
                            _ProjectsBudgetList.project_id = Ireader.GetInt64(CommonColumns.project_id);
                            _ProjectsBudgetList.branch_Id = Ireader.GetInt64(CommonColumns.branch_Id);
                            _ProjectsBudgetList.SalesMenId = Ireader.GetInt64(CommonColumns.SalesMenId);
                            _ProjectsBudgetList.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            _ProjectsBudgetList.project_budget_details_id = Ireader.GetInt64(CommonColumns.project_budget_details_id);

                        };

                        ProjectsBudgetList.Add(_ProjectsBudgetList);
                    }
                }
                return ProjectsBudgetList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_BudgetById");
                throw ex;

            }
            finally
            {
                ProjectsBudgetList = null;
                _ProjectsBudgetList = null;
            }
        }

        public List<ProjectsBudget> Get_Project_Budget_DetailsById(ProjectsBudgetView _ProjectsBudgetView)
        {
            List<ProjectsBudget> ProjectsBudgetList = new List<ProjectsBudget>();
            ProjectsBudget _ProjectsBudgetList;
            SupplierDropDown _SupplierList;
            SelectListItem GstList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(_ProjectsBudgetView.project_budget_id, CommanConstans.project_budget_details_id, SqlDbType.Int);
                    cmd.AddParameters(_ProjectsBudgetView.Project_Id, CommanConstans.project_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.get_Project_Budget_DetailsById);
                    while (Ireader.Read())
                    {
                        _ProjectsBudgetList = new ProjectsBudget();
                        {

                            _ProjectsBudgetList.project_budget_details_id = Ireader.GetInt32(CommonColumns.project_budget_details_id);
                            _ProjectsBudgetList.project_budget_id = Ireader.GetInt32(CommonColumns.project_budget_id);

                            _ProjectsBudgetList.project_id = Ireader.GetInt32(CommonColumns.project_id);
                            _ProjectsBudgetList.project_name = Ireader.GetString(CommonColumns.project_name);
                            _SupplierList = new SupplierDropDown();
                            {
                                _SupplierList.Supplier_id = Ireader.GetInt32(CommonColumns.supplier_id);
                                _SupplierList.Supplier_Name = Ireader.GetString(CommonColumns.supplier_names);
                            };
                            _ProjectsBudgetList.supplier = _SupplierList;
                            //_ProjectsBudgetList.supplier_id = Ireader.GetInt64(CommonColumns.supplier_id);
                            _ProjectsBudgetList.InvoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _ProjectsBudgetList.status_id = Ireader.GetString(CommonColumns.status_id);
                            _ProjectsBudgetList.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _ProjectsBudgetList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                            _ProjectsBudgetList.Approved_amount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            _ProjectsBudgetList.status_name = Ireader.GetString(CommonColumns.status_name);
                            _ProjectsBudgetList.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            GstList = new SelectListItem();
                            {
                                GstList.Text = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                                GstList.Value = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                            }
                            _ProjectsBudgetList.GSTPercent = GstList;
                            _ProjectsBudgetList.GSTAmount = Ireader.GetDecimal(CommonColumns.GSTAmount);

                        };

                        ProjectsBudgetList.Add(_ProjectsBudgetList);
                    }
                }
                return ProjectsBudgetList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsById");
                throw ex;

            }
            finally
            {
                ProjectsBudgetList = null;
                _ProjectsBudgetList = null;
            }
        }

        public List<ProjectsBudget> Get_Project_Budget_DetailsByIdForEditBudgetCost(ProjectsBudgetView _ProjectsBudgetView)
        {
            List<ProjectsBudget> ProjectsBudgetList = new List<ProjectsBudget>();
            ProjectsBudget _ProjectsBudgetList;
            SupplierDropDown _SupplierList;
            BudgetCostTypeDropDown _budgetcosttypeList;
            AddressDropDown AddressSites;
            SalesmanDropDown _Salesman;
            SelectListItem GstList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(_ProjectsBudgetView.project_budget_id, CommanConstans.project_budget_id, SqlDbType.Int);
                    cmd.AddParameters(_ProjectsBudgetView.Project_Id, CommanConstans.project_id, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.get_BudgetCost_DetailsById);
                    while (Ireader.Read())
                    {
                        _ProjectsBudgetList = new ProjectsBudget();
                        {
                            _ProjectsBudgetList.id = Ireader.GetInt32(CommonColumns.project_budget_details_id);
                            _ProjectsBudgetList.project_budget_details_id = Ireader.GetInt32(CommonColumns.project_budget_details_id);
                            _ProjectsBudgetList.project_budget_id = Ireader.GetInt32(CommonColumns.project_budget_id);
                            _ProjectsBudgetList.project_id = Ireader.GetInt32(CommonColumns.project_id);
                            AddressSites = new AddressDropDown();
                            {
                                AddressSites.id = Ireader.GetInt32(CommonColumns.project_id);
                                AddressSites.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
                            }
                            _ProjectsBudgetList.Address = AddressSites;
                            //_ProjectsBudgetList.project_id = Ireader.GetInt32(CommonColumns.project_id);
                            //_ProjectsBudgetList.project_name = Ireader.GetString(CommonColumns.project_name);

                            //_ProjectsBudgetList.supplier_id = Ireader.GetInt64(CommonColumns.supplier_id);
                            _ProjectsBudgetList.InvoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _ProjectsBudgetList.status_id = Ireader.GetString(CommonColumns.StatusId);
                            _ProjectsBudgetList.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            //_ProjectsBudgetList.CreatedUpdated = Ireader.GetString(CommonColumns.CreatedUpdated);
                            _ProjectsBudgetList.Approved_amount = Ireader.GetDecimal(CommonColumns.Approved_Amount);
                            //_ProjectsBudgetList.status_name = Ireader.GetString(CommonColumns.status_name);
                            _ProjectsBudgetList.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            GstList = new SelectListItem();
                            {
                                GstList.Text = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                                GstList.Value = Convert.ToString(Ireader.GetDecimal(CommonColumns.GSTPercent));
                            }
                            _ProjectsBudgetList.GSTPercent = GstList;
                            _ProjectsBudgetList.GSTAmount = Ireader.GetDecimal(CommonColumns.GSTAmount);
                            _ProjectsBudgetList.SalesMenId = Ireader.GetInt32(CommonColumns.SalesMenId);
                            _ProjectsBudgetList.supplier_id = _ProjectsBudgetView.SupplierId;
                            _Salesman = new SalesmanDropDown();
                            {
                                _Salesman.id = Ireader.GetInt32(CommonColumns.SalesMenId);
                                _Salesman.salesmen_name = Ireader.GetString(CommonColumns.salesmen_name);
                            };
                            _ProjectsBudgetList.Salesman = _Salesman;
                            _budgetcosttypeList = new BudgetCostTypeDropDown();
                            {
                                _budgetcosttypeList.BudgetCostType_id = Ireader.GetInt32("budgetCostType_id");
                                _budgetcosttypeList.BudgetCostType_type = Ireader.GetString("type");
                            };
                            _ProjectsBudgetList.BudgetCostType = _budgetcosttypeList;
                        };

                        ProjectsBudgetList.Add(_ProjectsBudgetList);
                    }
                }
                return ProjectsBudgetList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsById");
                throw ex;

            }
            finally
            {
                ProjectsBudgetList = null;
                _ProjectsBudgetList = null;
            }
        }

        public List<getBudgetedInvoice_Result> Get_budget_Invoice(int projectId, int supplierId)
        {
            List<getBudgetedInvoice_Result> getBudgetedInvoice_Result = new List<getBudgetedInvoice_Result>();
            getBudgetedInvoice_Result _getBudgetedInvoice_Result;

            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(supplierId, CommanConstans.SupplierId, SqlDbType.Int);
                    cmd.AddParameters(projectId, CommanConstans.projectid, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_getBudgetedInvoice);
                    while (Ireader.Read())
                    {
                        _getBudgetedInvoice_Result = new getBudgetedInvoice_Result();
                        {

                            _getBudgetedInvoice_Result.inv = Ireader.GetInt32(CommonColumns.inv);
                            _getBudgetedInvoice_Result.InvoiceAmtWithGST = Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST);
                            _getBudgetedInvoice_Result.invoiceNumber = Ireader.GetString(CommonColumns.InvoiceNumber);
                            _getBudgetedInvoice_Result.project_id = Ireader.GetInt32(CommonColumns.project_id);
                            _getBudgetedInvoice_Result.budget_amount = Ireader.GetDecimal(CommonColumns.budget_amount);
                            _getBudgetedInvoice_Result.remarks = Ireader.GetString(CommonColumns.remarks);
                            //_getBudgetedInvoice_Result.PaymentAmount = Ireader.GetDecimal(CommonColumns.amo);

                        };

                        getBudgetedInvoice_Result.Add(_getBudgetedInvoice_Result);
                    }
                }
                return getBudgetedInvoice_Result;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsById");
                throw ex;

            }

        }

        public List<Proc_GetPaymentDetail_Result> GetPaymentDetail_Result(int paymentid, int Supplier_id)
        {
            List<Proc_GetPaymentDetail_Result> getPaymentDetailResult = new List<Proc_GetPaymentDetail_Result>();
            Proc_GetPaymentDetail_Result _getPaymentDetailResult;

            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(Supplier_id, CommanConstans.SupplierId, SqlDbType.Int);
                    cmd.AddParameters(paymentid, CommanConstans.PaymentId, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_GetPaymentDetail);
                    while (Ireader.Read())
                    {
                        _getPaymentDetailResult = new Proc_GetPaymentDetail_Result();
                        {

                            _getPaymentDetailResult.id = Ireader.GetInt32(CommonColumns.id);
                            _getPaymentDetailResult.payment_id = Ireader.GetInt32(CommonColumns.Payment_id);
                            _getPaymentDetailResult.invoice_amount = Ireader.GetDecimal(CommonColumns.Invoice_amount);
                            _getPaymentDetailResult.gst_percentage = Ireader.GetDecimal(CommonColumns.Gst_percentage);
                            _getPaymentDetailResult.gst_amount = Ireader.GetDecimal(CommonColumns.Gst_amount);
                            _getPaymentDetailResult.payment_amount = Ireader.GetDecimal(CommonColumns.Payment_amount);
                            _getPaymentDetailResult.supplier_inv_number = Ireader.GetString(CommonColumns.Supplier_inv_number);
                            _getPaymentDetailResult.project_id = Ireader.GetInt64(CommonColumns.Project_id);
                            _getPaymentDetailResult.agreed_amount = Ireader.GetDecimal(CommonColumns.Agreed_amount);
                            _getPaymentDetailResult.message = Ireader.GetString(CommonColumns.Message);
                        };

                        getPaymentDetailResult.Add(_getPaymentDetailResult);
                    }
                }
                return getPaymentDetailResult;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetPaymentDetail_Result");
                throw ex;

            }

        }

        public getBudgetedInvoice_Result getBudgetedCost(int Id)
        {
            getBudgetedInvoice_Result getBudgetedInvoice_Result = new getBudgetedInvoice_Result();
            getBudgetedInvoice_Result _getBudgetedInvoice_Result;

            try
            {
                using (var cmd = new DBSqlCommand())
                {

                    cmd.AddParameters(Id, CommanConstans.id, SqlDbType.Int);
                    //cmd.AddParameters(projectId, CommanConstans.projectid, SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_getBudgetedCostByid);
                    while (Ireader.Read())
                    {
                        _getBudgetedInvoice_Result = new getBudgetedInvoice_Result();
                        {
                            _getBudgetedInvoice_Result.InvoiceAmtWithGST = Convert.ToDecimal( Ireader.GetDecimal(CommonColumns.InvoiceAmtWithGST));
                            _getBudgetedInvoice_Result.budget_amount = Convert.ToDecimal(Ireader.GetDecimal(CommonColumns.budget_amount));
                            _getBudgetedInvoice_Result.ApprovedAmount = Convert.ToDecimal(Ireader.GetDecimal(CommonColumns.Approved_Amount));

                        };

                        getBudgetedInvoice_Result = _getBudgetedInvoice_Result;
                    }
                }
                return getBudgetedInvoice_Result;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Get_Project_Budget_DetailsById");
                throw ex;

            }

        }


        public string GetProjectIdById(string ProjectId)
        {
            string project_id = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.projectid, SqlDbType.BigInt);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.GetProjectIdById);
                    while (ireader.Read())
                    {
                        project_id = ireader.GetString(CommonColumns.project_id);
                    }
                    return project_id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    project_id = string.Empty;
                }
            }
        }

        public string GetProjectNumberIdById(string id, string action)
        {
            string project_number = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(id, CommanConstans.id, SqlDbType.VarChar);
                    cmd.AddParameters(action, CommanConstans.Action, SqlDbType.VarChar);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_ProjectNumberIdById);
                    while (ireader.Read())
                    {
                        project_number = ireader.GetString(CommonColumns.project_number);
                    }
                    return project_number;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    project_number = string.Empty;
                }
            }
        }

        public string GetProjectNumberIdByBudgetId(long id)
        {
            string project_Id = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(id, CommanConstans.id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Get_ProjectNumberIdByBudgetId);
                    while (ireader.Read())
                    {
                        project_Id = ireader.GetString(CommonColumns.project_id);
                    }
                    return project_Id;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    project_Id = string.Empty;
                }
            }
        }

        public string GetFilePathById(Int64 ProjectId)
        {
            string Path = string.Empty;
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.id, SqlDbType.BigInt);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.GetFilePathById);
                    while (ireader.Read())
                    {
                        Path = ireader.GetString(CommonColumns.document_path);
                    }
                    return Path;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Path = string.Empty;
                }
            }
        }
        public SuccessMessage Remove_Project_Budget(string project_budget_id)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(project_budget_id, CommanConstans.project_budget_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Remove_Project_Budget);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Remove_Project_Budget_Detail Parameters= " + project_budget_id.ToString());
                throw ex;
            }
            finally
            {
                project_budget_id = null;
            }
        }

        public SuccessMessage Remove_Project_Budget_Detail(string project_budget_detail_id)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Convert.ToInt32(project_budget_detail_id), CommanConstans.project_budget_details_id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Remove_Project_Budget_Detail);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: Remove_Project_Budget_Detail Parameters= " + project_budget_detail_id.ToString());
                throw ex;
            }
            finally
            {
                project_budget_detail_id = null;
            }
        }
        public SuccessMessage Get_VODetails_RowsCount(string VO_Id)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(VO_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VODetails_RowsCountByVO_id);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Get_VODetails_RowsCount, ProjectId =" + VO_Id);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage Get_EVODetails_RowsCount(string EVO_Id)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(EVO_Id, CommanConstans.evo_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVODetails_RowsCountByEVO_id);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: Get_EVODetails_RowsCount, ProjectId =" + EVO_Id);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage GetDiscountPercentageFromContract(string ProjectId)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.project_Id, SqlDbType.NVarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.GetDiscountPercentageFromContract);
                    while (ireader.Read())
                    {
                        _SuccessMessage.DiscountPercentage = ireader.GetDecimal(CommonColumns.discountpercentage);
                        _SuccessMessage.Result = "1";
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: GetDiscountPercentageFromContract, ProjectId =" + ProjectId);
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public List<VOCategoryDropDown> VOCategoryDropDownList()
        {
            List<VOCategoryDropDown> VOCategoryList = new List<VOCategoryDropDown>();
            VOCategoryDropDown _VOCategoryList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_VOCategoryList);
                    while (Ireader.Read())
                    {
                        _VOCategoryList = new VOCategoryDropDown();
                        {
                            _VOCategoryList.category_Id = Ireader.GetInt32(CommonColumns.PropertyType_Id);
                            _VOCategoryList.category_name = Ireader.GetString(CommonColumns.PropertyType);
                        };

                        VOCategoryList.Add(_VOCategoryList);
                    }

                }

                return VOCategoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _VOCategoryList = null;
            }
        }


        public List<ElectricalItemMapping> GetElectricalItemsDetails(int PropertyType_Id, string projectId, string evo_id = null)
        {
            List<ElectricalItemMapping> ElectricalItemMappingList = new List<ElectricalItemMapping>();
            ElectricalItemMapping _ElectricalItemMappingList;
            ItemDropDown itemDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(projectId, CommanConstans.projectid, SqlDbType.VarChar);
                    cmd.AddParameters(evo_id, CommanConstans.evo_id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_ElectricalItemMappingList);
                    while (Ireader.Read())
                    {
                        _ElectricalItemMappingList = new ElectricalItemMapping();
                        {
                            _ElectricalItemMappingList.ElectricalItemMapping_Id = Ireader.GetInt32(CommonColumns.ElectricalItemMapping_Id);
                            //_ElectricalItemMappingList.Item_Id = Ireader.GetInt32(CommonColumns.item_id);
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ElectricalItemMappingList.Item = itemDropDown;
                            _ElectricalItemMappingList.PropertyType_Id = Ireader.GetInt32(CommonColumns.PropertyType_Id);
                            _ElectricalItemMappingList.Uom_Id = Ireader.GetString(CommonColumns.uom_id);
                            _ElectricalItemMappingList.Cost_Price = Ireader.GetInt32(CommonColumns.price);
                            _ElectricalItemMappingList.Selling_Price = Ireader.GetDecimal(CommonColumns.Selling_Price);
                            _ElectricalItemMappingList.Item_description = Ireader.GetString(CommonColumns.item_description);
                            _ElectricalItemMappingList.Uom_Description = Ireader.GetString(CommonColumns.uom_description);
                            _ElectricalItemMappingList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _ElectricalItemMappingList.Qty = Ireader.GetDecimal(CommonColumns.default_qty);
                            _ElectricalItemMappingList.IsSelected = Ireader.GetBoolean(CommonColumns.IsSelected);
                            _ElectricalItemMappingList.totalamount = Ireader.GetDecimal(CommonColumns.totalamaount);
                            _ElectricalItemMappingList.gstamount = Ireader.GetDecimal(CommonColumns.gstamount);
                            _ElectricalItemMappingList.grandtotal = Ireader.GetDecimal(CommonColumns.GrandTOtal);
                        };

                        ElectricalItemMappingList.Add(_ElectricalItemMappingList);
                    }

                }

                return ElectricalItemMappingList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _ElectricalItemMappingList = null;
            }
        }

        public SuccessMessage GetElectricalItemsDetailsCheckedorUnchecked(string projectId, string SelectedData, string UpdatedData, int propertyType_Id, string uid, string evo_det_id, bool IsCheckboxSelected = false)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (evo_det_id == null || evo_det_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(evo_det_id, CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(projectId, CommanConstans.projectid, SqlDbType.VarChar);
                    cmd.AddParameters(SelectedData, CommanConstans.selectedData, SqlDbType.VarChar);
                    cmd.AddParameters(UpdatedData, CommanConstans.updatedData, SqlDbType.VarChar);
                    cmd.AddParameters(propertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(IsCheckboxSelected, CommanConstans.IsCheckboxSelected, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Evo_CheckedandUnchecked_Details);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                        _SuccessMessage.Evo_Id = ireader.GetString(CommonColumns.evo_id);
                        _SuccessMessage.Evo_Amount = ireader.GetDecimal(CommonColumns.EvoAmount);
                        _SuccessMessage.Evo_GstAmount = ireader.GetDecimal(CommonColumns.Evogstamount);
                        _SuccessMessage.Evo_total_Amount = ireader.GetDecimal(CommonColumns.Evototalamaount);
                        _SuccessMessage.Property_Type = ireader.GetString(CommonColumns.PropertyType);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public SuccessMessage SaveEvoSelectedData(string projectId, string SelectedData, string UpdatedData, int propertyType_Id, string evo_det_id, string uid)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    if (evo_det_id == null || evo_det_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(evo_det_id, CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(projectId, CommanConstans.projectid, SqlDbType.VarChar);
                    cmd.AddParameters(SelectedData, CommanConstans.selectedData, SqlDbType.VarChar);
                    cmd.AddParameters(UpdatedData, CommanConstans.updatedData, SqlDbType.VarChar);
                    //cmd.AddParameters(propertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Upsert_EVo_Details);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: SaveEvoSelectedData");
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage SaveNewMethodEvoSelectedData(List<EVOCriteria> _EVOCriteria, EVOTotalCriteria totalCriterias, string Project_id, int PropertyType_Id, string evo_id, string uid, string evo_det_id)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();

            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    DataTable dt = new DataTable();
                    //Add Columns
                    dt.Columns.Add("Qty");
                    dt.Columns.Add("Id");
                    dt.Columns.Add("ElectricalMapping");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("IsSelected");

                    foreach (var item in _EVOCriteria)
                    {


                        dt.Rows.Add(item.Qty,
                                    item.Id,
                                    item.ElectricalMapping,
                                    item.Price,
                                    item.IsSelected

                                    );

                    }

                    if (evo_det_id == null || evo_det_id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters((evo_det_id), CommanConstans.evo_det_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters((Project_id), CommanConstans.projectid, SqlDbType.VarChar);
                    cmd.AddParameters(dt, CommanConstans.evocriteriaDataTable, SqlDbType.Structured);
                    cmd.AddParameters(PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(uid, CommanConstans.user_id, SqlDbType.VarChar);
                    cmd.AddParameters((evo_id), CommanConstans.evo_id, SqlDbType.VarChar);
                    cmd.AddParameters(totalCriterias.TotalSelectedItemsAmount, CommanConstans.TotalSelectedItemsAmount, SqlDbType.Decimal);
                    cmd.AddParameters(totalCriterias.DiscountPercentage, CommanConstans.DiscountPercentage, SqlDbType.Decimal);
                    cmd.AddParameters(totalCriterias.DiscountAmount, CommanConstans.DiscountAmount, SqlDbType.Decimal);
                    cmd.AddParameters(totalCriterias.TotalAmount, CommanConstans.TotalAmount, SqlDbType.Decimal);
                    cmd.AddParameters(totalCriterias.GstAmount, CommanConstans.GstAmount, SqlDbType.Decimal);
                    cmd.AddParameters(totalCriterias.GrandTotal, CommanConstans.GrandTotal, SqlDbType.Decimal);


                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Usp_Save_Selected_EVo_Details);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);

                        _SuccessMessage.TotalSelectedItemsAmount = ireader.GetDecimal(CommanConstans.TotalSelectedItemsAmount);
                        _SuccessMessage.DiscountPercentage = ireader.GetDecimal(CommanConstans.DiscountPercentage);
                        _SuccessMessage.DiscountAmount = ireader.GetDecimal(CommanConstans.DiscountAmount);
                        _SuccessMessage.TotalAmount = ireader.GetDecimal(CommanConstans.TotalAmount);
                        _SuccessMessage.GstAmount = ireader.GetDecimal(CommanConstans.GstAmount);
                        _SuccessMessage.GrandTotal = ireader.GetDecimal(CommanConstans.GrandTotal);

                        _SuccessMessage.Internal_Evo_Number = ireader.GetString(CommonColumns.evo_id);
                        _SuccessMessage.Evo_Id = ireader.GetString(CommonColumns.evouniquid);
                        _SuccessMessage.Evo_Date = ireader.GetString(CommonColumns.EvoDate);
                        _SuccessMessage.Evo_Status = ireader.GetString(CommonColumns.EvoStatus);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: SaveNewMethodEvoSelectedData");
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }

        public SuccessMessage InsertHeaderNewEvo(string ProjectId, bool isNew = false)
        {
            string result = string.Empty;
            SuccessMessage _SuccessMessage = new SuccessMessage();

            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(ProjectId, CommanConstans.projectid, SqlDbType.VarChar);
                    //cmd.AddParameters(dt, CommanConstans.evocriteriaDataTable, SqlDbType.Structured);
                    // cmd.AddParameters(PropertyType_Id, CommanConstans.PropertyType_Id, SqlDbType.Int);
                    cmd.AddParameters(isNew, CommanConstans.isNew, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Usp_Save_NeW_Evo_Headers);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        _SuccessMessage.Evo_Id = ireader.GetString(CommonColumns.evouniquid);
                        _SuccessMessage.Internal_Evo_Number = ireader.GetString(CommonColumns.evo_id);

                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: SaveEvoSelectedData");
                    throw ex;
                }
                finally
                {
                    result = string.Empty;
                }
            }
        }



        public List<SupplierAddress> GetSupplierAddressById(int supplier_id)
        {
            List<SupplierAddress> salesmenAddresses = new List<SupplierAddress>();
            SupplierAddress _salesmenAddresses;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(supplier_id, CommanConstans.supplier_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Supplier_Address_By_Id);
                    while (Ireader.Read())
                    {
                        _salesmenAddresses = new SupplierAddress();
                        {
                            _salesmenAddresses.Id = Ireader.GetInt32(CommonColumns.id);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Supplier_Name);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Address1);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Address2);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Address3);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Address4);
                            _salesmenAddresses.SupplierName = Ireader.GetString(CommonColumns.Zip_Code);
                        };

                        salesmenAddresses.Add(_salesmenAddresses);
                    }
                }
                return salesmenAddresses;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSupplierAddressById");
                throw ex;
            }
            finally
            {
                salesmenAddresses = null;
                _salesmenAddresses = null;
            }
        }

        public List<UserEmailAddress> GetAdminAndSalesmenEmailAddress(string salesmen_Id,string Project_Id,int Procedure)
        {
            List<UserEmailAddress> EmailAddress = new List<UserEmailAddress>();
            UserEmailAddress _EmailAddress;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(salesmen_Id, CommanConstans.salesmen_id, SqlDbType.NVarChar);
                    cmd.AddParameters(Project_Id, CommanConstans.project_id, SqlDbType.NVarChar);
                    cmd.AddParameters(Procedure, "@Procedure", SqlDbType.Int);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_Salesmen_And_Admin_Email);
                    while (Ireader.Read())
                    {
                        _EmailAddress = new UserEmailAddress();
                        {
                            _EmailAddress.User_Id = Ireader.GetString(CommonColumns.User_Id);
                            _EmailAddress.Role = Ireader.GetString(CommonColumns.Role);
                            _EmailAddress.Email = Ireader.GetString(CommonColumns.Email);
                        };
                        if (_EmailAddress.Email != "")
                            EmailAddress.Add(_EmailAddress);
                    }

                }

                return EmailAddress;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetSupplierAddressById");
                throw ex;
            }
            finally
            {
                EmailAddress = null;
                _EmailAddress = null;
            }
        }

        public List<ProjectTasksItem> GetEVOTasksItem(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<ProjectTasksItem> ProjectTasksItemList = new List<ProjectTasksItem>();
            ProjectTasksItem _ProjectTasksItemList;
            TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    //cmd.AddParameters(_ProjectTasksItemCriteria.record_type, CommanConstans.record_type, SqlDbType.Int);
                    if (_ProjectTasksItemCriteria.Vo_Id == null || _ProjectTasksItemCriteria.Vo_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Vo_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVOTasks_Item);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new ProjectTasksItem();
                        {
                            taskDropDown = new TaskDropDown();
                            {
                                taskDropDown.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                                taskDropDown.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            };
                            _ProjectTasksItemList.Task = taskDropDown;
                            //_ProjectTasksItemList = Ireader.GetString(CommonColumns.Task_Id);
                            //_ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public List<VariationOrder> GetEVOTasksItemDetails(ProjectTasksItemList _ProjectTasksItemCriteria)
        {
            List<VariationOrder> ProjectTasksItemList = new List<VariationOrder>();
            VariationOrder _ProjectTasksItemList;
            CategoryDropDown categoryDropDown;
            ItemDropDown itemDropDown;
            StatusLookup statusLookup; // BillingUOM
            UOMDropDown uOMDropDown;
            //TaskDropDown taskDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    if (_ProjectTasksItemCriteria.Vo_Id == null || _ProjectTasksItemCriteria.Vo_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_id, SqlDbType.VarChar);

                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Vo_Id, CommanConstans.vo_id, SqlDbType.VarChar);
                    }
                    if (_ProjectTasksItemCriteria.Project_Det_Id == null || _ProjectTasksItemCriteria.Project_Det_Id == "")
                    {
                        cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }
                    else
                    {
                        cmd.AddParameters(_ProjectTasksItemCriteria.Project_Det_Id, CommanConstans.vo_det_id, SqlDbType.VarChar);
                    }

                    cmd.AddParameters(_ProjectTasksItemCriteria.project_Id, CommanConstans.project_Id, SqlDbType.VarChar);
                    cmd.AddParameters(_ProjectTasksItemCriteria.Task_Id, CommanConstans.Task_Id, SqlDbType.VarChar);
                    //cmd.AddParameters("00000000-0000-0000-0000-000000000000", CommanConstans.project_det_Id, SqlDbType.VarChar);
                    // cmd.AddParameters(_ProjectTasksItemCriteria.record_type, CommanConstans.record_type, SqlDbType.Int);

                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_EVO_ItemDetails);
                    while (Ireader.Read())
                    {
                        _ProjectTasksItemList = new VariationOrder();
                        {
                            _ProjectTasksItemList.vo_id = Ireader.GetString(CommonColumns.vo_id);
                            _ProjectTasksItemList.vo_det_id = Ireader.GetString(CommonColumns.vo_det_id);
                            categoryDropDown = new CategoryDropDown();
                            {
                                categoryDropDown.category_Id = Ireader.GetInt32(CommonColumns.category_Id);
                                categoryDropDown.category_name = Ireader.GetString(CommonColumns.category_name);
                            };
                            _ProjectTasksItemList.Category = categoryDropDown;
                            itemDropDown = new ItemDropDown();
                            {
                                itemDropDown.item_id = Ireader.GetInt32(CommonColumns.item_id);
                                itemDropDown.item_description = Ireader.GetString(CommonColumns.item_description);
                            };
                            _ProjectTasksItemList.Item = itemDropDown;
                            statusLookup = new StatusLookup();
                            {
                                statusLookup.status_lookup_id = Ireader.GetInt32(CommonColumns.status_lookup_id);
                                statusLookup.description = Ireader.GetString(CommonColumns.description);
                            };
                            _ProjectTasksItemList.BillingUOM = statusLookup;
                            uOMDropDown = new UOMDropDown();
                            {
                                uOMDropDown.uom_id = Ireader.GetInt32(CommonColumns.uom_id);
                                uOMDropDown.uom_description = Ireader.GetString(CommonColumns.Uom);
                            };
                            _ProjectTasksItemList.UOM = uOMDropDown;
                            _ProjectTasksItemList.Task_Id = Ireader.GetString(CommonColumns.Task_Id);
                            _ProjectTasksItemList.Task_Name = Ireader.GetString(CommonColumns.Task_Name);
                            _ProjectTasksItemList.Price = Ireader.GetDecimal(CommonColumns.Price);
                            _ProjectTasksItemList.Qty = Ireader.GetString(CommonColumns.Qty);
                            _ProjectTasksItemList.item_remarks = Ireader.GetString(CommonColumns.item_remarks);
                            _ProjectTasksItemList.Amount = Ireader.GetDecimal(CommonColumns.Amount);
                            _ProjectTasksItemList.record_type = Ireader.GetInt32(CommonColumns.record_type);
                            _ProjectTasksItemList.Highlight = Ireader.GetBoolean(CommonColumns.Highlight);
                            _ProjectTasksItemList.StatusId = Ireader.GetInt32(CommonColumns.status_id);
                        };
                        ProjectTasksItemList.Add(_ProjectTasksItemList);
                    }

                }

                return ProjectTasksItemList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetVOTasksItem Parameters= " + _ProjectTasksItemCriteria.ToString());
                throw ex;
            }
            finally
            {
                ProjectTasksItemList = null;
                _ProjectTasksItemList = null;
            }
        }

        public List<AddressDropDown> BindAddressSiteByStatus(string Project_Id, string Salesmen_Id)
        {
            List<AddressDropDown> BindAddress = new List<AddressDropDown>();
            AddressDropDown _BindAddress;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Project_Id, CommanConstans.Project_Id, SqlDbType.BigInt);
                    cmd.AddParameters(Salesmen_Id, CommanConstans.salesmen_id, SqlDbType.BigInt);
                    cmd.AddParameters(Common.SessionManagement.SelectedBranchID, CommanConstans.branch_id, SqlDbType.BigInt);
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.BindAddressByStatus);
                    while (Ireader.Read())
                    {
                        _BindAddress = new AddressDropDown();
                        {
                            _BindAddress.id = Ireader.GetInt32(CommonColumns.id);
                            _BindAddress.AddressSite = Ireader.GetString(CommonColumns.AddressSite);
                        };

                        BindAddress.Add(_BindAddress);
                    }

                }

                return BindAddress;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindAddressSite");
                throw ex;
            }
            finally
            {
                BindAddress = null;
                _BindAddress = null;
            }
        }

        public List<SourceOfInquiry> SourceOfInquiryList()
        {
            List<SourceOfInquiry> SourceOfInquiryList = new List<SourceOfInquiry>();
            SourceOfInquiry _SourceOfInquiryList;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.Get_SourceOfInquiryList);
                    while (Ireader.Read())
                    {
                        _SourceOfInquiryList = new SourceOfInquiry();
                        {
                            _SourceOfInquiryList.SourceOfInquiry_Id = Ireader.GetInt16(CommonColumns.SourceOfInquiry_Id);
                            _SourceOfInquiryList.SourceOfInquiry_cd = Ireader.GetString(CommonColumns.SourceOfInquiry_cd);
                            _SourceOfInquiryList.SourceOfInquiry_name = Ireader.GetString(CommonColumns.SourceOfInquiry_name);
                            _SourceOfInquiryList.SourceOfInquiry_description = Ireader.GetString(CommonColumns.SourceOfInquiry_description);
                            _SourceOfInquiryList.isactive = Ireader.GetBoolean(CommonColumns.isactive);
                            _SourceOfInquiryList.Seq_No = Ireader.GetInt32("Seq_No");
                        };

                        SourceOfInquiryList.Add(_SourceOfInquiryList);
                    }

                }

                return SourceOfInquiryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _SourceOfInquiryList = null;
            }
        }

        public SuccessMessage CreateSourceOfInquiry(SourceOfInquiry _SourceOfInquiryList, string uid)
        {
            SuccessMessage _successMessage = new SuccessMessage();
            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(_SourceOfInquiryList.SourceOfInquiry_Id, CommanConstans.SourceOfInquiry_Id, SqlDbType.Int);
                    cmd.AddParameters(_SourceOfInquiryList.SourceOfInquiry_cd, CommanConstans.SourceOfInquiry_cd, SqlDbType.VarChar);
                    cmd.AddParameters(_SourceOfInquiryList.SourceOfInquiry_name, CommanConstans.SourceOfInquiry_name, SqlDbType.VarChar);
                    cmd.AddParameters(_SourceOfInquiryList.SourceOfInquiry_description, CommanConstans.SourceOfInquiry_description, SqlDbType.VarChar);
                    cmd.AddParameters(_SourceOfInquiryList.isactive, CommanConstans.isactive, SqlDbType.Bit);
                    cmd.AddParameters(uid, CommanConstans.userId, SqlDbType.VarChar);
                    cmd.AddParameters(_SourceOfInquiryList.Seq_No, "@SeqNo", SqlDbType.Int);

                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Upsert_SourceOfInquiry);
                    while (ireader.Read())
                    {
                        _successMessage.Result = ireader.GetString(CommonColumns.Result);
                        _successMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                    return _successMessage;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _SourceOfInquiryList = null;
                    _successMessage = null;
                }
            }
        }

        public SuccessMessage DeleteSourceOfInquiryBySourceOfInquiryId(string SourceOfInquiry_Id)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(SourceOfInquiry_Id, CommanConstans.SourceOfInquiry_Id, SqlDbType.Int);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Delete_SourceOfInquiryBySourceOfInquiry_Id);
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }

                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: DeleteSourceOfInquiryBySourceOfInquiryId Parameters= " + SourceOfInquiry_Id.ToString());
                throw ex;
            }
            finally
            {
                SourceOfInquiry_Id = null;
            }
        }


        public QuotationImportExcelResult ImportFromExelFile(DataTable dtHeader, DataTable dtDetails)
        {
            try
            {
                QuotationImportExcelResult quotationImportExcelResult = new QuotationImportExcelResult();

                using (SqlConnection conn = new SqlConnection(ConStr))
                {
                    using (SqlCommand cmd = new SqlCommand(SqlProcedures.SSP_Import_Excel_File_Data, conn))
                    {
                        cmd.Parameters.AddWithValue(CommanConstans.dtHeader, dtHeader);
                        cmd.Parameters.AddWithValue(CommanConstans.dtDetails, dtDetails);


                        SqlParameter outStatus = new SqlParameter(CommanConstans.status, SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        SqlParameter outStatusInformation = new SqlParameter(CommanConstans.statusInformation, SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(outStatus);
                        cmd.Parameters.Add(outStatusInformation);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        quotationImportExcelResult.Status = bool.Parse(outStatus.Value.ToString());
                        quotationImportExcelResult.StatusInformation = outStatusInformation.Value.ToString();
                        conn.Close();
                    }
                }
                return quotationImportExcelResult;

                //using (var cmd = new DBSqlCommand())
                //{
                //    cmd.AddParameters(dtHeader, CommanConstans.dtHeader, SqlDbType.Structured);
                //    cmd.AddParameters(dtDetails, CommanConstans.dtDetails, SqlDbType.Structured);
                //    cmd.AddParameters(quotationImportExcelResult.Status, CommanConstans.status, SqlDbType.Bit, ParameterDirection.Output);
                //    cmd.AddParameters(quotationImportExcelResult.StatusInformation, CommanConstans.statusInformation, SqlDbType.NVarChar, ParameterDirection.Output, 1000);

                //    cmd.ExecuteDataReader(SqlProcedures.SSP_Import_Excel_File_Data);
                //}
                //return quotationImportExcelResult;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: ImportFromExelFile Parameters= dtHeader & dtDetails");
                throw ex;
            }
            finally
            {
                dtHeader = null;
                dtDetails = null;
            }
        }

        public SuccessMessage CheckCustomerAndSalesmenDetails(string salesmenName, string customerName)
        {
            try
            {
                SuccessMessage SuccessMessage = new SuccessMessage();
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(salesmenName, CommanConstans.salesmen_name, SqlDbType.VarChar);
                    cmd.AddParameters(customerName, CommanConstans.customer_name, SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Check_Salesmen_Customer_Details);

                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: CheckCustomerAndSalesmenDetails Parameters= salesmenName, customerName");
                throw ex;
            }
            finally
            {
                customerName = null;
            }
        }

        public SuccessMessage UpsertContractSupplierMapping(string suppID, string ProjectID)
        {
            SuccessMessage SuccessMessage = new SuccessMessage();
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(ProjectID), "@Project_Id", SqlDbType.UniqueIdentifier);
                    cmd.AddParameters(suppID, "@Supplier_ID", SqlDbType.VarChar);
                    IDataReader ireader = cmd.ExecuteDataReader("[dbo].[SSP_Upsert_ContractSupplierMapping]");
                    while (ireader.Read())
                    {
                        SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
                        SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
                    }
                }
                return SuccessMessage;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: UpsertContractSupplierMapping Parameters= " + ProjectID.ToString());
                throw ex;
            }
            finally
            {
                ProjectID = null;
                suppID = null;
            }
        }

        public contractSupplierMapping GetContractSupplierMappingForProject(string PrjID)
        {
            contractSupplierMapping csmMappingData = new contractSupplierMapping();
            List<ContractSuppliersMappingData> csMapInfo = new List<ContractSuppliersMappingData>();
            ContractSuppliersMappingData _ContractSuppliersMappingData;
            try
            {
                using (var cmdHeaderInfo = new DBSqlCommand())
                {
                    cmdHeaderInfo.AddParameters(Guid.Parse(PrjID), CommanConstans.project_Id, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmdHeaderInfo.ExecuteDataReader("[dbo].[SSP_Get_ProjectInfoForContractSuppMapping]");
                    while (Ireader.Read())
                    {
                        csmMappingData.project_name = Ireader.GetString("project_name");
                        csmMappingData.project_number = Ireader.GetString("project_number");
                        csmMappingData.customer = Ireader.GetString("customername");
                        csmMappingData.salesmen = Ireader.GetString("salesmenname");
                    }
                }

                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(Guid.Parse(PrjID), CommanConstans.project_Id, SqlDbType.UniqueIdentifier);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_Get_ContractSupplierMappingForProject]");
                    while (Ireader.Read())
                    {
                        _ContractSuppliersMappingData = new ContractSuppliersMappingData();
                        {
                            _ContractSuppliersMappingData.supplierID = Ireader.GetString("Supplier_id");
                            _ContractSuppliersMappingData.suppliername = Ireader.GetString("supplier_name");
                            _ContractSuppliersMappingData.suppaddress = Ireader.GetString("Address1");
                        };

                        csMapInfo.Add(_ContractSuppliersMappingData);
                    }
                }
                csmMappingData._contractSupplierMapping = csMapInfo;


                return csmMappingData;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: GetContractSupplierMappingForProject");
                throw ex;
            }
            finally
            {
                csMapInfo = null;
                _ContractSuppliersMappingData = null;
            }
        }


        public List<UnitCodeDropDown> BindUnitCode(string ZipCodeId)
        {
            List<UnitCodeDropDown> _unitcodeDropDownList = new List<UnitCodeDropDown>();
            UnitCodeDropDown _unitcodeDropDown;     
                        try
            {
                using (var cmd = new DBSqlCommand())
                {
                    cmd.AddParameters(ZipCodeId, CommanConstans.ZipCodeId, SqlDbType.NVarChar);
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_GetUnitCode]");
                    while (Ireader.Read())
                    {
                        _unitcodeDropDown = new UnitCodeDropDown();
                        {
                            _unitcodeDropDown.UnitCode_id = Ireader.GetString("unitcodeID");
                            _unitcodeDropDown.UnitCode_Name = Ireader.GetString("unitcodeName");
                        };                        
                        _unitcodeDropDownList.Add(_unitcodeDropDown);
                    }
                    _unitcodeDropDownList.Insert(0, new UnitCodeDropDown { UnitCode_Name = "Select UnitCode ", UnitCode_id = "" });
                }
                return _unitcodeDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
                throw ex;
            }
            finally
            {
                _unitcodeDropDown = null;
                _unitcodeDropDownList = null;
            }
        }

        public List<ZipCodeDropDown> BindZipCode()
        {
            List<ZipCodeDropDown> _ZipCodeDropDownDropDownList = new List<ZipCodeDropDown>();
            ZipCodeDropDown _ZipCodeDropDownDropDown;
            try
            {

                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_GetZipCode]");
                    while (Ireader.Read())
                    {
                        _ZipCodeDropDownDropDown = new ZipCodeDropDown();
                        {
                            _ZipCodeDropDownDropDown.ZipCode_id = Ireader.GetString("zipcodeID");
                            _ZipCodeDropDownDropDown.ZipCode_Name = Ireader.GetString("zipcodeName");
                        };
                        _ZipCodeDropDownDropDownList.Add(_ZipCodeDropDownDropDown);                        
                    }
                    _ZipCodeDropDownDropDownList.Insert(0, new ZipCodeDropDown{ ZipCode_Name = "Select ZipCode ", ZipCode_id = "" });

                }
                return _ZipCodeDropDownDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
                throw ex;
            }
            finally
            {
                _ZipCodeDropDownDropDown = null;
                _ZipCodeDropDownDropDownList = null;
            }
        }
        //public SuccessMessage UpsertPayments(Paymentview _UpsertPayments)
        //{
        //    SuccessMessage _SuccessMessage = new SuccessMessage();
        //    using (var cmd = new DBSqlCommand())
        //    {
        //        try
        //        {
        //            cmd.AddParameters(_UpsertPayments.id, CommanConstans.id, SqlDbType.Int);
        //            ////cmd.AddParameters(_UpsertPayments.payment_date, CommanConstans.payment_date, SqlDbType.DateTime);
        //            //cmd.AddParameters(_UpsertPayments.supplier_id, CommanConstans.supplier_id, SqlDbType.Int);
        //            //cmd.AddParameters(_UpsertPayments.bank_id, CommanConstans.bank_id, SqlDbType.Int);
        //            //cmd.AddParameters(_UpsertPayments.cheque_number, CommanConstans.cheque_number, SqlDbType.Int);
        //            //cmd.AddParameters(_UpsertPayments.rebate_amount, CommanConstans.rebate_amount, SqlDbType.Decimal);
        //            //cmd.AddParameters(_UpsertPayments.remarks, CommanConstans.remarks, SqlDbType.VarChar);
        //            //cmd.AddParameters(_UpsertPayments.userid, CommanConstans.userid, SqlDbType.VarChar);
        //            //cmd.AddParameters(_UpsertPayments.payment_mode, CommanConstans.payment_mode, SqlDbType.Int);
        //            cmd.AddParameters(_UpsertPayments.isactive, CommanConstans.amount, SqlDbType.Decimal);
        //            cmd.AddParameters(_UpsertPayments.collection_date, CommanConstans.userId, SqlDbType.VarChar);
        //            cmd.AddParameters(_UpsertPayments.Message, CommanConstans.item_remarks, SqlDbType.NVarChar);                   
        //            IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.upsert_packageDetails);                   
        //            while (ireader.Read())
        //            {
        //                _SuccessMessage.Result = ireader.GetString(CommonColumns.Result);
        //                _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.Errormessage);
        //                _SuccessMessage.Amount = ireader.GetDecimal(CommonColumns.TotalAmount);
        //                _SuccessMessage.gst_percentage = ireader.GetDecimal(CommonColumns.gst_percentage);
        //                _SuccessMessage.gst_amount = ireader.GetDecimal(CommonColumns.gst_amount);
        //                _SuccessMessage.SubTotal = ireader.GetDecimal(CommonColumns.Sub_Total);
        //            }
        //            return _SuccessMessage;

        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionLog.WriteLog(ex, "Method Name: UpsertPackageDetails, Parameters =" + _UpsertPayments.ToString());
        //            throw ex;
        //        }
        //        finally
        //        {
        //            _UpsertPayments = null;
        //        }
        //    }
        //}

        //public SuccessMessage SaveUpdateBudgetedCost(ProjectsBudgetViewModel objView)
        //{
        //    SuccessMessage _SuccessMessage = new SuccessMessage();
        //    try
        //    {
        //        foreach (var item in objView.projects_budget_details)
        //        {
        //            using (var cmd = new DBSqlCommand())
        //            {
        //                cmd.AddParameters(0, CommanConstans.project_budget_id, SqlDbType.Int);
        //                cmd.AddParameters(0, CommanConstans.project_budget_details_id, SqlDbType.Int);
        //                cmd.AddParameters(SessionManagement.UserID, CommanConstans.UserID, SqlDbType.VarChar);
        //                cmd.AddParameters(objView.ProjectSalesmen, CommanConstans.SalesMenId, SqlDbType.Int);
        //                cmd.AddParameters(objView.project_id, CommanConstans.project_id, SqlDbType.Int);
        //                cmd.AddParameters(item.supplier_id, CommanConstans.SupplierId, SqlDbType.Int);
        //                cmd.AddParameters(item.InvoiceNumber, CommanConstans.InvoiceNumber, SqlDbType.VarChar);
        //                cmd.AddParameters(item.InvoiceAmountwithGST, CommanConstans.InvoiceAmountwithGST, SqlDbType.Decimal);
        //                cmd.AddParameters(item.GSTpercent, CommanConstans.GSTPercent, SqlDbType.Decimal);
        //                cmd.AddParameters(item.GSTAmount, CommanConstans.GSTAmount, SqlDbType.Decimal);
        //                cmd.AddParameters(item.Amount, CommanConstans.budget_amount, SqlDbType.Decimal);
        //                cmd.AddParameters(7, CommanConstans.StatusId, SqlDbType.Int);
        //                IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerDetailsById);
        //                while (Ireader.Read())
        //                {
        //                    _SuccessMessage.Result = Ireader.GetString(CommonColumns.Result);
        //                    _SuccessMessage.Errormessage = Ireader.GetString(CommonColumns.Errormessage);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}

        //public CustomerDetailsById GetCustomerDetailsById(string Id)
        //{
        //    CustomerDetailsById obj = new CustomerDetailsById();
        //    try
        //    {
        //        using (var cmd = new DBSqlCommand())
        //        {
        //            cmd.AddParameters(Id, CommanConstans.project_id, SqlDbType.VarChar);
        //            IDataReader Ireader = cmd.ExecuteDataReader(SqlProcedures.GetCustomerDetailsById);
        //            while (Ireader.Read())
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: GetCustomerDetailsById Parameters= " + Id);
        //        return obj;
        //    }
        //    finally
        //    {
        //        obj = null;
        //    }
        //}

        //public string SavePath(string Id , string path)
        //{

        //}

        //public string DeletePath(string Id, string path)
        //{

        //}

        //public Dashboard GetDetailsBasedonYear(int Year)
        //{
        //    Dashboard DB = new Dashboard();
        //    DateTime fromdt = new DateTime();
        //    DateTime todt = new DateTime();
        //    switch (Year)
        //    {
        //        case 2021:
        //            fromdt = new DateTime(2021, 01, 01, 00, 00, 00);
        //            todt = DateTime.Now;
        //            break;
        //        case 2020:
        //            fromdt = new DateTime(2020, 01, 01, 00, 00, 00);
        //            todt = new DateTime(2020, 12, 31, 00, 00, 00);
        //            break;
        //        case 2019:
        //            fromdt = new DateTime(2019, 01, 01, 00, 00, 00);
        //            todt = new DateTime(2019, 12, 31, 00, 00, 00);
        //            break;
        //        case 2018:
        //            fromdt = new DateTime(2018, 01, 01, 00, 00, 00);
        //            todt = new DateTime(2018, 12, 31, 00, 00, 00);
        //            break;
        //    }
        //    try
        //    {
        //        using (var cmd = new DBSqlCommand())
        //        {
        //            cmd.AddParameters(fromdt, CommanConstans.fromdate, SqlDbType.DateTime);
        //            cmd.AddParameters(todt, CommanConstans.todate, SqlDbType.DateTime);
        //            IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.RPt_Total);
        //            while (ireader.Read())
        //            {
        //                DB.TotalSales = ireader.GetDecimal(CommonColumns.Subtotal);
        //            }
        //        }
        //        using (var cmd = new DBSqlCommand())
        //        {
        //            cmd.AddParameters(fromdt, CommanConstans.fromdate, SqlDbType.DateTime);
        //            cmd.AddParameters(todt, CommanConstans.todate, SqlDbType.DateTime);
        //            IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.SSP_Rpt_No_OfContracts);
        //            while (ireader.Read())
        //            {
        //                DB.NumberofContracts = ireader.GetInt32(CommonColumns.NoOfContracts);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: GetDetailsBasedonYear, Year =" + Year);
        //        return DB;
        //    }
        //    finally
        //    {
        //        DB = null;
        //    }
        //}

        public SuccessMessage UpdateSupplierRole(string Id, string supplierId)
        {
            SuccessMessage _SuccessMessage = new SuccessMessage();

            using (var cmd = new DBSqlCommand())
            {
                try
                {
                    cmd.AddParameters(Id, CommanConstans.id, SqlDbType.VarChar);
                    cmd.AddParameters(Convert.ToInt64(supplierId), CommanConstans.supplier_id, SqlDbType.BigInt);
                    IDataReader ireader = cmd.ExecuteDataReader(SqlProcedures.Usp_Save_SupplierRole);
                    while (ireader.Read())
                    {
                        _SuccessMessage.Errormessage = ireader.GetString(CommonColumns.ErrorMessage);
                        _SuccessMessage.Result = ireader.GetString(CommonColumns.ReturnCode);
                    }
                    return _SuccessMessage;
                }
                catch (Exception ex)
                {
                    ExceptionLog.WriteLog(ex, "Method Name: UpdateSupplierRole");
                    throw ex;
                }
                finally
                {
                    //result = string.Empty;
                }
            }
        }

        public List<BudgetCostTypeDropDown> BindBudgetCostType()
        {
            List<BudgetCostTypeDropDown> _BudgetCostTypeDropDownList = new List<BudgetCostTypeDropDown>();
            BudgetCostTypeDropDown _BudgetCostTypeDropDown;
            try
            {
                using (var cmd = new DBSqlCommand())
                {
                    IDataReader Ireader = cmd.ExecuteDataReader("[dbo].[SSP_BindBudgetCostType]");
                    while (Ireader.Read())
                    {
                        _BudgetCostTypeDropDown = new BudgetCostTypeDropDown();
                        {
                            _BudgetCostTypeDropDown.BudgetCostType_id = Ireader.GetInt32(CommonColumns.id);
                            _BudgetCostTypeDropDown.BudgetCostType_type = Ireader.GetString("type");
                        };

                        _BudgetCostTypeDropDownList.Add(_BudgetCostTypeDropDown);
                    }

                }

                return _BudgetCostTypeDropDownList;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: BindSupplier");
                throw ex;
            }
            finally
            {
                _BudgetCostTypeDropDown = null;
                _BudgetCostTypeDropDownList = null;
            }
        }
    }
}