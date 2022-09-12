using Microsoft.AspNet.Identity;
using PMS.Common;
using PMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using static PMS.Common.CommonFunction;

namespace PMS.Controllers
{
    [Authorize]
    [BranchFilter]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReceiptReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    // branchList = Common.CommonFunction.BranchList();
                    objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
                }
                else if(User.IsInRole("Salesman"))
                {
                    if(!SessionManagement.IsBranchAdmin)
                        objView.ProjectList = CommonFunction.UserSalesMenProjectList(uid);
                    else
                        objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");

                }
                else
                {
                    // branchList = Common.CommonFunction.UserBranchList(uid);
                    objView.ProjectList = CommonFunction.UserSalesMenProjectList(uid);
                    //objView.ProjectList = CommonFunction.
                }
                objView.Uid = uid;

                //branchList.Insert(0, new SelectListItem { Text = "Select Branch", Value = "0" });
                //objView.BranchList = branchList;


                DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //var endDate = startDate.AddMonths(1).AddDays(-1);
                // var startDate = new DateTime(now.Year, (now.Month - 5), 1);
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

                objView.BankList = CommonFunction.BankList();
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise(objView.Uid);
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();

                if (User.IsInRole("Salesman"))
                {
                    if (SessionManagement.IsBranchAdmin)
                        objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                    else
                        objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                }

                ExportReceiptReport(objView, true);

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ReceiptReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public void ExportReceiptReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_ReceiptReport_Result> reportData = new List<Database.SSP_ReceiptReport_Result>();
            StringBuilder str = new StringBuilder("");
            List<SelectListItem> ModeofPayment = Common.CommonFunction.ModeofPaymentList();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                else if(User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalesmenId = CommonFunction.SalesmenIDByUserID(uid);
                    else
                        objView.SalesmenId = 0;
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_ReceiptReport(uid, Common.SessionManagement.SelectedBranchID, objView.from_date, objView.to_date, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
                }
                string projectName = "";
                string salesmanname = "";
                string bankname = "";
                str.Append("<table width='100%' border='0' class='table table-striped'>");
                if (isGridData == false)
                {
                    if (objView.ProjectId > 0)
                    {
                        projectName = CommonFunction.UserProjectList(Convert.ToString("00000000-0000-0000-0000-000000000000")).Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }
                    if (objView.SalesmenId > 0)
                    {
                        salesmanname = CommonFunction.SalesmenList().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }
                    if (objView.BankId > 0)
                    {
                        bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='7'><b>Customer Payment Report</b></th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");

                    if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                    {
                        str.Append("<tr>");
                    }

                    if (Convert.ToString(bankname) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
                    }
                    if (Convert.ToString(projectName) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Project: </b> " + projectName + "</td>");
                    }
                    if (Convert.ToString(salesmanname) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Salesmen: </b> " + salesmanname + "</td>");
                    }


                    if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                    {
                        str.Append("<td></td>");
                        str.Append("</tr>");
                        str.Append("<tr><td colspan='7'></td></tr>");
                    }
                }

                str.Append("<tr>");
                str.Append("<th>Date</th><th>Cheque No.</th><th style='text-align:right;'>Amount</th><th>Job Site Location</th><th>Tel</th><th>Client</th><th>Salesmen</th>");
                str.Append("</tr>");

                string tele = "";
                foreach (var items in reportData)
                {
                    if (items.phone != null && items.mobile != null)
                    {
                        tele = items.phone + " / " + items.mobile;
                    }
                    else if (items.phone != null)
                    {
                        tele = items.phone;
                    }
                    else
                    {
                        tele = items.mobile;
                    }

                    str.Append("<tr>");
                    str.Append("<td>" + items.receipt_date + "</td><td>" + items.cheque_details + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.amount)) + "</td><td>" + items.branch_name + "</td>");
                    str.Append("<td>" + tele + "</td><td>" + items.name1 + "</td><td>" + items.salesmen_name + "</td>");
                    str.Append("</tr>");
                }

                foreach (var items in ModeofPayment)
                {
                    if (Convert.ToInt32(items.Value) > 0)
                    {
                        
                            double nets = Convert.ToDouble(reportData.Where(o => o.mode_of_payment_id == Convert.ToInt32(items.Value)).Select(o => o.amount).Sum());
                            double gst = (nets * 0.07);

                            str.Append("<tr>");
                            str.Append("<td colspan='2'><strong>" + items.Text + "</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(nets)) + "</strong></td><td></td><td></td><td><strong>GST</strong></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(gst)) + "</td>");
                            str.Append("</tr>");
                      
                    }
                }
                double total = Convert.ToDouble(reportData.Select(o => o.amount).Sum());
                double totalgst = (total * 0.07);

                str.Append("<tr>");
                str.Append("<td colspan='2'><strong>Total</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(total)) + "</strong></td><td></td><td></td><td><strong>Total GST</strong></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(totalgst)) + "</td>");
                str.Append("</tr>");

                str.Append("</table>");
                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("ReceiptReport_" + objView.ReportMonth.ToString() + "_" + objView.ReportYear.ToString() + ".xls", str.ToString());

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportReceiptReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
                str = null;
            }
        }

        public ActionResult SaleSummaryReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            // List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    //   branchList = Common.CommonFunction.BranchList();
                }
                else
                {
                    //  branchList = Common.CommonFunction.UserBranchList(uid);
                }
                objView.Uid = uid;

                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                //objView.BranchList = branchList;

                objView.YearList = Common.CommonFunction.YearList();

                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                }
                ExportSaleSummaryReport(objView, true);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SaleSummaryReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }

        public void ExportSaleSummaryReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_SaleSummeryReport_Result> reportData = new List<Database.SSP_SaleSummeryReport_Result>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.Uid = uid;
                    else
                        uid = "";
                }
                else
                    objView.Uid = uid;

                objView.YearList = Common.CommonFunction.YearList();

                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                }

                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_SaleSummeryReport(uid, SessionManagement.SelectedBranchID, objView.ReportYear).ToList();
                }

                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");

                if (isGridData == false)
                {
                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='14'><b>Sales Summary Report</b></th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='14'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='14'><b>Year: </b> " + objView.ReportYear.ToString() + "</th>");
                    str.Append("</tr>");
                }

                str.Append("<tr>");
                str.Append("<th>Sales " + objView.ReportYear.ToString() + "</th><th style='text-align:right;'>Jan</th><th style='text-align:right;'>Feb</th><th style='text-align:right;'>Mar</th><th style='text-align:right;'>April</th><th style='text-align:right;'>May</th><th style='text-align:right;'>June</th><th style='text-align:right;'>July</th><th style='text-align:right;'>Aug</th><th style='text-align:right;'>Sep</th><th style='text-align:right;'>Oct</th><th style='text-align:right;'>Nov</th><th style='text-align:right;'>Dec</th><th style='text-align:right;'>Total</th>");
                str.Append("</tr>");

                double total = 0;
                double grandTotal = 0;
                foreach (var items in reportData)
                {
                    total = Convert.ToDouble(items.jan) + Convert.ToDouble(items.feb) + Convert.ToDouble(items.mar) + Convert.ToDouble(items.apr) + Convert.ToDouble(items.may) + Convert.ToDouble(items.jun) + Convert.ToDouble(items.jul) + Convert.ToDouble(items.aug) + Convert.ToDouble(items.sep) + Convert.ToDouble(items.oct) + Convert.ToDouble(items.nov) + Convert.ToDouble(items.dec);
                    grandTotal = grandTotal + total;

                    str.Append("<tr>");
                    str.Append("<td>" + items.salesmen_name + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jan)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.feb)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.mar)) + "</td>");
                    str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.apr)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.may)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jun)) + "</td>");
                    str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.jul)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.aug)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.sep)) + "</td>");
                    str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.oct)) + "</td><td align='right'>$ " + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.nov)) + "</td><td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.dec)) + "</td>");
                    str.Append("<td align='right'>$" + Common.CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(total)) + "</td>");
                    str.Append("</tr>");
                }

                str.Append("<tr>");
                str.Append("<td>Total</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jan).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.feb).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.mar).Sum())) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.apr).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.may).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jun).Sum())) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.jul).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.aug).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.sep).Sum())) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.oct).Sum())) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.nov).Sum())) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Select(o => o.dec).Sum())) + "</td>");
                str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(grandTotal)) + "</td>");
                str.Append("</tr>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();

                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("Sales_Summary_" + objView.ReportYear.ToString() + ".xls", str.ToString());

                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportReceiptReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
            }
        }

        public ActionResult PaymentReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    //  objView.ProjectList = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    // objView.ProjectList = CommonFunction.UserProjectList(uid);
                }
                objView.Uid = uid;


                DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, now.Month, 1);
                //var endDate = startDate.AddMonths(1).AddDays(-1);

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
                objView.BankList = CommonFunction.BankList();
                //   objView.SalenmenList = CommonFunction.UserSalesmenList(objView.Uid);
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise(objView.Uid);
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();

                if (User.IsInRole("Salesman"))
                {
                    objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                }
                //List<SelectListItem> branchList = new List<SelectListItem>();
                //if (uid == "")
                //{
                //    branchList = Common.CommonFunction.BranchList();
                //}
                //else
                //{
                //    branchList = Common.CommonFunction.UserBranchList(uid);
                //}
                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                //objView.BranchList = branchList;


                //objView.BranchId = 0;


                ExportPaymentReport(objView, true);


                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: PaymentReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }

        public void ExportPaymentReport(ReportViewModel objView, bool isGridData = false)
        {
            List<Database.SSP_PaymentReport_Result> reportData = new List<Database.SSP_PaymentReport_Result>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalesmenId = CommonFunction.SalesmenIDByUserID(uid);
                    else
                        objView.SalesmenId = 0;
                }
                string projectName = "";
                string salesmanname = "";
                string bankname = ""; 
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_PaymentReport(uid, objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.BankId, objView.ProjectId, objView.SalesmenId).ToList();
                }

                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");

                if (isGridData == false)
                {

                    if (objView.ProjectId > 0)
                    {
                        projectName = CommonFunction.UserProjectList("00000000-0000-0000-0000-000000000000").Where(o => o.Value == objView.ProjectId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }
                    if (objView.SalesmenId > 0)
                    {
                        salesmanname = CommonFunction.SalesmenList().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }
                    if (objView.BankId > 0)
                    {
                        bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='7'><b>Supplier Payment Report</b></th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;'  colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");

                    if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                    {
                        str.Append("<tr>");
                    }

                    if (Convert.ToString(bankname) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Bank: </b> " + bankname + "</td>");
                    }
                    if (Convert.ToString(projectName) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Project: </b> " + projectName + "</td>");
                    }
                    if (Convert.ToString(salesmanname) != "")
                    {
                        str.Append("<td colspan='2' style='text-align:left;'><b>Salesmen: </b> " + salesmanname + "</td>");
                    }

                    if (Convert.ToString(projectName) != "" || Convert.ToString(salesmanname) != "" || Convert.ToString(bankname) != "")
                    {
                        str.Append("<td></td>");
                        str.Append("</tr>");
                        str.Append("<tr><td></td></tr>");
                    }
                }
                str.Append("<tr>");
                str.Append("<th>Payment Date</th><th >Supplier</th><th style='text-align:right;'>Capital</td><th style='text-align:right;'>Rebate</th><th>Bank</th><th>Cheque#</th><th style='text-align:right;'>Amount Paid</th>");
                str.Append("</tr>");

                foreach (var items in reportData)
                {
                    string payment_date = items.payment_date;

                    string supplier_name = Convert.ToString(items.supplier_name);
                    //double invoice_amount = Convert.ToDouble(items.invoice_amount);
                    double rebate_amount = Convert.ToDouble(items.rebate_amount);
                    string bank = Convert.ToString(items.bank);
                    string cheque_number = Convert.ToString(items.cheque_number);
                    double payment_amount = Convert.ToDouble(items.payment_amount);


                    str.Append("<tr>");
                    str.Append("<td>" + payment_date + "</td><td>" + supplier_name + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payment_amount)) + "</td><td align='right'>$ " + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(rebate_amount)) + "</td><td>" + bank + "</td>");
                    str.Append("<td>" + cheque_number + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(payment_amount) - Convert.ToDecimal(rebate_amount)) + "</td>");
                    str.Append("</tr>");
                }

                double capital_amount = Convert.ToDouble(reportData.Select(o => o.payment_amount).Sum());
                double dis_received = Convert.ToDouble(reportData.Select(o => o.rebate_amount).Sum());
                double amount_paid = capital_amount - dis_received;

                double total_amount_report = (amount_paid + dis_received);


                str.Append("<tr>");
                str.Append("<td></td><td></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(capital_amount)) + "</strong></td><td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(dis_received)) + "</strong></td><td></td><td></td> <td align='right'><strong>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(amount_paid)) + "</strong></td>");
                str.Append("</tr>");
                str.Append("</table>");
                if (isGridData == true)
                {
                    objView.GridData = str.ToString();

                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("PaymentReport_" + Convert.ToDateTime(objView.from_date).ToString("ddMMyyyy") + "_" + Convert.ToDateTime(objView.to_date).ToString("ddMMyyyy") + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportReceiptReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
            }
        }


        public ActionResult IndividualSaleReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> salesmenList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                    //salesmenList = Common.CommonFunction.SalesmenList();
                }
                else
                {
                    // salesmenList = Common.CommonFunction.UserSalesmenList(uid);
                }
                //  salesmenList = Common.CommonFunction.UserSalesmenList("");
                objView.Uid = uid;
                // objView.SalenmenList = salesmenList;
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise("");
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                // objView.YearList = Common.CommonFunction.YearList();
                if (User.IsInRole("Salesman"))
                {
                    objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                }
                objView.ProjectStatusList = CommonFunction.DDLProjectStatusList();
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
                ExportIndividualSaleReport(objView, true);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: IndividualSaleReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                salesmenList = null;
            }
        }

        public void ExportIndividualSaleReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalesmenId = CommonFunction.SalesmenIDByUserID(uid);
                    else
                        uid = "";
                }
                List<Database.SSP_SaleIndividualReport_Result> report_items = new List<Database.SSP_SaleIndividualReport_Result>();
                report_items = Common.CommonFunction.GetSaleIndividualReport_Result(uid, objView.SalesmenId, objView.from_date, objView.to_date, objView.ProjectStatus, SessionManagement.SelectedBranchID);

                //using (Database.PMSEntities objDB = new Database.PMSEntities())
                //{
                    //report_items = objDB.SSP_SaleIndividualReport(uid, objView.SalesmenId, objView.from_date, objView.to_date, objView.ProjectStatus).ToList();
                //}
                string prevMonth = "";
                string salesmen = "";
                int count = 0;
                Int32 totalRec = 0;
                Int32 colspan = 5;

                if (isGridData)
                {
                    colspan = 6;
                }

                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");

                if (isGridData == false)
                {
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='5'><b>Individual Sale Report</b></th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='5'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='5'><b>Year: </b>" + objView.ReportYear.ToString() + "</th>");
                    str.Append("</tr>");
                }

                if (report_items != null)
                {
                    if (report_items.Count > 0)
                    {
                        salesmen = report_items[0].salesmen_name;
                        str.Append("<tr>");
                        str.Append("<td colspan='" + colspan + "'><b>ID: </b>" + salesmen + "</td>");
                        str.Append("</tr>");
                    }
                }

                foreach (var items in report_items)
                {
                    count = count + 1;
                    if (prevMonth != items.receipt_date)
                    {
                        str.Append("<tr>");
                        str.Append("<td colspan='" + colspan + "'></td>");
                        str.Append("</tr>");

                        str.Append("<tr>");
                        str.Append("<td style='background-color:#C0C0C0;'>No</td><td style='background-color:#C0C0C0;'>" + items.receipt_date + "</td><td style='background-color:#C0C0C0;'>Date of Contract</td><td style='background-color:#C0C0C0;'>Contract No</td><td style='background-color:#C0C0C0; text-align:right;'>Sales Amt</td><td style='background-color:#C0C0C0; text-align:right;'>Remarks</td>");
                        if (isGridData)
                        {
                            str.Append("<td style='background-color:#C0C0C0; width:5%'></td>");
                        }
                        str.Append("</tr>");
                        prevMonth = items.receipt_date;
                        count = 1;
                        totalRec = report_items.Where(o => o.receipt_date == prevMonth).Select(o => o.id).Count();
                    }

                    str.Append("<tr>");
                    str.Append("<td>" + count + "</td><td>" + items.project_name + "</td>");
                    str.Append("<td>" + items.contract_date + "</td><td>" + items.project_number + "</td>");
                    str.Append("<td align='right'>$" + items.total_amount + "</td>");
                    str.Append("<td>" + items.remarks + "</td>");
                    if (isGridData)
                    {
                        str.Append("<td style='text-align:center;'><a title='Project Costing Report' class='btn-large' href='/Report/ProjectCostingReport?ProjectId=" + items.id + "' ><span class='glyphicon glyphicon-usd'></span></a>");

                        //str.Append("&nbsp; <a title='View Document' target='_blank' class='btn-large' onclick='LoadDocumentsByProject(" + items.id + ");' href='javascript:void(0)' ><i class='fa fa-file' aria-hidden='true'></i></a>");
                        str.Append("&nbsp; <a title='View Document' class='btn-large' onclick='LoadDocumentsByProject(" + items.id + ");' href='javascript:void(0)' ><i class='fa fa-file' aria-hidden='true'></i></a>");

                        //if (!string.IsNullOrEmpty(items.document_name))
                        //{
                        //    str.Append("&nbsp; <a title='View Document' target='_blank' class='btn-large' href='/Content/ContractDucuments/" + items.document_name + "' ><i class='fa fa-file' aria-hidden='true'></i></a>");
                        //}

                        str.Append("</td>");
                    }
                    str.Append("</tr>");

                    if (count == totalRec)
                    {
                        str.Append("<tr>");
                        str.Append("<td></td><td></td>");
                        str.Append("<td></td><td>Total</td>");
                        str.Append("<td align='right'>$" + report_items.Where(o => o.receipt_date == prevMonth).Select(o => o.total_amount).Sum() + "</td>");
                        if (isGridData)
                        {
                            str.Append("<td></td>");
                        }
                        str.Append("</tr>");
                    }
                }

                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();

                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("SaleReport_" + objView.ReportYear.ToString() + "_" + salesmen.Replace(" ", "") + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportIndividualSaleReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
            }
        }

        public ActionResult ProjectListingReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();

            DateTime now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            //var endDate = startDate.AddMonths(1).AddDays(-1);

            //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
            var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
            var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
            try
            {
                if (objView.from_date == null)
                {
                    objView.from_date = CurrentstartDate.AddMonths(-5);
                }
                if (objView.to_date == null)
                {
                    objView.to_date = endDate;
                }

                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }

                // salesmenList = CommonFunction.UserSalesmenList("");
                objView.Uid = uid;
                // branchList.Insert(0, new SelectListItem { Text = "Select Branch", Value = "0" });

                // objView.BranchList = branchList;
                // objView.SalenmenList = salesmenList;
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise("");
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                //objView.YearList = Common.CommonFunction.YearList();

                //if (objView.ReportYear == 0)
                //{
                //    objView.ReportYear = DateTime.Now.Year;
                //}
                ExportProjectListingReport(objView, true);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectListingReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                salesmenList = null;
            }
        }

        public void ExportProjectListingReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_ProjectsBySalesman_Result> report_items = new List<Database.SSP_ProjectsBySalesman_Result>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    report_items = objDB.SSP_ProjectsBySalesman(objView.from_date, objView.to_date, uid, objView.SalesmenId, SessionManagement.SelectedBranchID).ToList();
                }

                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");

                if (isGridData == false)
                {
                    string salesmanname = "";
                    if (objView.SalesmenId > 0)
                    {
                        salesmanname = CommonFunction.SalesmenList().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }

                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='5'><b>Project Listing Report</b></th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='5'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='1'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='4'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");
                    if (salesmanname != "")
                    {
                        str.Append("<tr>");
                        str.Append("<th style='text-align:left;'  colspan='5'><b>Salesmen: </b>" + salesmanname + "</th>");
                        str.Append("</tr>");
                    }
                }

                str.Append("<tr>");
                str.Append("<th>Project Name</th><th>Date of Contract</th>");
                str.Append("<th>Date of Project Closed</th><th>Salesmen</th>");
                str.Append("<th>Branch</th>");
                str.Append("<th>Project Cost</th>");
                str.Append("</tr>");

                foreach (var items in report_items)
                {
                    str.Append("<tr>");
                    str.Append("<td>" + items.project_name + "</td><td>" + items.contract_date + "</td>");
                    str.Append("<td>" + items.closing_date + "</td><td>" + items.salesmen_name + "</td>");
                    str.Append("<td>" + items.branch_name + "</td>");
                    str.Append("<td>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.project_contract_amount)) + "</td>");

                    str.Append("</tr>");
                }

                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();

                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("ProjectListingReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportProjectListingReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                report_items = null;
            }
        }

        public ActionResult ProjectSummaryReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //  List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                // salesmenList = CommonFunction.UserSalesmenList("");
                objView.Uid = uid;
                //objView.SalenmenList = salesmenList;
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise("");
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                objView.ProjectStatusList = CommonFunction.DDLProjectStatusList();
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                    else
                        objView.SalenmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);

                }
                DateTime now = DateTime.Now;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                if (objView.from_date == null)
                {
                    objView.from_date = startDate;
                }

                if (objView.to_date == null)
                {
                    objView.to_date = endDate;
                }

                ExportProjectSummeryReport(objView, true);

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectSummaryReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                salesmenList = null;
            }
        }

        public void ExportProjectSummeryReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_ProjectSummeryReport_Result> reportData = new List<Database.SSP_ProjectSummeryReport_Result>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalesmenId = CommonFunction.SalesmenIDByUserID(uid);
                    else
                        objView.SalesmenId = 0;
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_ProjectSummeryReport(uid, objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.SalesmenId, objView.ProjectStatus).ToList();
                    reportData = reportData.OrderBy(o => o.contract_date).ThenBy(o => o.contract_date).ToList();
                }
                StringBuilder str = new StringBuilder("");
                string salesmanname = "";
                str.Append("<table width='100%' class='table table-striped'>");
                if (isGridData == false)
                {
                    if (objView.SalesmenId > 0)
                    {
                        salesmanname = CommonFunction.SalesmenList().Where(o => o.Value == objView.SalesmenId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }

                    str.Append("<tr>");
                    //str.Append("<th style='text-align:left;' colspan='7'><b>Weeklly Project report</b></th>");
                    str.Append("<th style='text-align:left;' colspan='7'><b>Project Summary Report</b></th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;'  colspan='7'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='5'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");

                    if (Convert.ToString(salesmanname) != "")
                    {
                        str.Append("<tr>");
                        str.Append("<td style='text-align:left;' colspan='7'><b>Salesmen: </b> " + salesmanname + "</td>");
                        str.Append("</tr>");
                    }
                }
                int i = 0;
                int yer = 0;
                StringBuilder innerStr = new StringBuilder("");
                foreach (var groups in reportData.GroupBy(o => o.contract_date))
                {
                    foreach (var items in groups)
                    {
                        if (yer != items.contract_date)
                        {
                            yer = items.contract_date ?? 0;
                            i = 0;
                            str.Append("<tr>");
                            str.Append("<td style='text-align:left;' colspan='7'></td>");
                            str.Append("</tr>");

                            str.Append("<tr><th>No.</th><th>Job Site " + items.contract_date + "</th><th>Salesmen</th><th>Date of Collect Payment</th><th style='text-align:right;'>Budgeted Cost</th><th style='text-align:right;'>Contract Value</th><th style='text-align:right;'>Progress Claim</th><th style='text-align:right;'>Balance Amt</th><th style='text-align:right;'>Costing Amt</th><th style='text-align:right;'>PC-BC</th><th>Remarks</th>");

                            if (isGridData == false)
                            {
                                str.Append("<th>Remarks</th>");
                            }

                            str.Append("</tr>");
                        }

                        i = i + 1;

                        str.Append("<tr>");
                        str.Append("<td>" + i + "</td><td>" + items.project_name + "</td><td>" + items.salesmen_name + "</td><td>" + items.receipt_date + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(items.budgeted_cost) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(items.contract_value) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(items.progress_claim) + "</td>");
                        str.Append("<td align='right'>$" + (items.contract_value - items.progress_claim) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(items.costing_amt)) + "</td>");
                        //if(items.pc_bc )


                        //str.Append("<td align='right'>$" + items.pc_bc + "</td>");
                        if (items.pc_bc < 0)
                        {
                            str.Append("<td align='right' style='color:red;'>$" + items.pc_bc + "</td>");

                        }
                        else
                        {
                            str.Append("<td align='right'>$" + items.pc_bc + "</td>");
                        }
                        str.Append("<td>" + items.remarks + "</td>");

                        if (isGridData == false)
                        {
                            str.Append("<td>Remarks</td>");
                        }

                        str.Append("</tr>");


                        //str.Append("<tr>");
                        //str.Append("<td></td><td></td><td></td><td>$" + CommonFunction.ConvertAmountoDecimal(reportData.Sum(o => o.contract_value).ToString()) + "</td><td>$" + CommonFunction.ConvertAmountoDecimal(reportData.Sum(o => o.progress_claim).ToString()) + "</td>");
                        //str.Append("<td>$" + CommonFunction.ConvertAmountoDecimal((reportData.Sum(o => o.contract_value) - reportData.Sum(o => o.progress_claim)).ToString()) + "</td><td>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(reportData.Sum(o => o.costing_amt)).ToString()) + "</td>");
                        //str.Append("</tr>");

                    }

                    str.Append("<tr>");
                    str.Append("<td></td><td></td><td></td><td></td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(groups.Sum(o => o.budgeted_cost)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(groups.Sum(o => o.contract_value)) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(groups.Sum(o => o.progress_claim)) + "</td>");
                    str.Append("<td align='right'>$" + CommonFunction.ConvertAmountoDecimal((groups.Sum(o => o.contract_value) - groups.Sum(o => o.progress_claim))) + "</td><td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(groups.Sum(o => o.costing_amt))) + "</td> <td align='right'>$" + CommonFunction.ConvertAmountoDecimal(Convert.ToDecimal(groups.Sum(o => o.pc_bc))) + "</td>");

                    if (isGridData == false)
                    {
                        str.Append("<td></td>");
                    }
                    str.Append("</tr>");
                }
                str.Append("</table>");
                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("ProjectSummaryReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportProjectSummeryReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
            }
        }

        [ActionName("ProjectbyAddress")]
        public ActionResult ProjectStatusbyBranchReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                objView.Uid = uid;
                objView.YearList = Common.CommonFunction.YearList();
                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                }
                //List<SelectListItem> branchList = new List<SelectListItem>();
                //if (uid == "")
                //{
                //    branchList = Common.CommonFunction.BranchList();
                //}
                //else
                //{
                //    branchList = Common.CommonFunction.UserBranchList(uid);
                //}
                //branchList.Insert(0, new SelectListItem { Text = "Please Select", Value = "0" });
                //objView.BranchList = branchList;
                //objView.BranchId = 0;
                ExportProjectStatusbyBranchReport(objView, true);
                return View("ProjectStatusbyBranchReport", objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectStatusbyBranchReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
            }
        }

        public void ExportProjectStatusbyBranchReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_ProjectListingStatusByBranch_Result> reportData = new List<Database.SSP_ProjectListingStatusByBranch_Result>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_ProjectListingStatusByBranch(uid, objView.ReportYear, SessionManagement.SelectedBranchID).ToList();
                }
                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");
                if (isGridData == false)
                {
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='8'><b>Project By Address</b></th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='8'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='8'><b>Year: </b>" + objView.ReportYear.ToString() + "</th>");
                    str.Append("</tr>");
                }
                str.Append("<tr><td><b>Updated: " + Convert.ToDateTime(DateTime.Now).ToString("MM/dd/yyyy").ToString() + "</b></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
                str.Append("<tr>");
                //string headerstyle = "style='background-color:#bfbfbf'";
                string FirstCharStyle = "style='background-color:#f2dcdb'";

                str.Append("<th>Job Site " + objView.ReportYear.ToString() + "</th><th>Contact No.</th><th>Customer</th><th>ID In-Charge</th><th>Date of Contract</th><th>Tiles</th><th>M. Bldg Products</th><th>Project Closed</th>");
                str.Append("</tr>");

                string first_char = "";
                foreach (var items in reportData)
                {
                    string job_sites = Convert.ToString(items.job_sites);

                    string contact_no = Convert.ToString(items.contact_no);
                    string customer = Convert.ToString(items.customer);
                    string id_in_charge = Convert.ToString(items.id_in_charge);
                    string contract_date = Convert.ToString(items.contract_date);
                    string tiles = Convert.ToString(items.tiles);
                    string m_bldg_products = Convert.ToString(items.m_bldg_products);
                    string project_closed = Convert.ToString(items.project_closed);
                    if (job_sites.ToString().Length > 0)
                    {
                        if (job_sites.Substring(0, 1).ToString().ToUpper() != first_char.ToString())
                        {
                            first_char = job_sites.Substring(0, 1).ToString().ToUpper();
                            str.Append("<tr><td " + FirstCharStyle + "><b>" + job_sites.Substring(0, 1).ToString().ToUpper() + "</b></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td><td " + FirstCharStyle + "></td></tr>");
                        }
                    }

                    str.Append("<tr>");
                    str.Append("<td>" + items.project_name + "</td><td>" + contact_no + "</td><td>" + customer + "</td><td>" + id_in_charge + "</td><td>" + contract_date + "</td>");
                    str.Append("<td>" + tiles + "</td><td>" + m_bldg_products + "</td><td>" + project_closed + "</td>");
                    str.Append("</tr>");
                    if (job_sites.ToString().Length > 0)
                    {
                        if (job_sites.Substring(0, 1).ToString().ToUpper() != first_char.ToString())
                        {
                            str.Append("<tr><td></td></tr><tr><td></td></tr><tr><td></td></tr><tr><td></td></tr>");
                        }
                    }
                }
                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();

                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("ProjectStatusbyBranchReport_" + objView.ReportYear.ToString() + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportProjectStatusbyBranchReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
            }
        }


        public ActionResult ProjectCostingReport(ReportViewModel objView, string ProjectId)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "00000000-0000-0000-0000-000000000000";
                }
                else if(User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalesmenId = Common.CommonFunction.SalesmenIDByUserID(uid);
                    else
                        objView.SalesmenId = 0;
                }
                else
                {
                    uid = "00000000-0000-0000-0000-000000000000";
                }

                objView.Uid = uid;
                objView.YearList = CommonFunction.YearList();
                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                }
                // objView.SalenmenList = CommonFunction.UserSalesmenList("");
                objView.SalenmenList = CommonFunction.SalesmenListStatusWise("");
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.SalenmenList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                    else
                        objView.SalenmenList = CommonFunction.SalesmenListByBranch(SessionManagement.SelectedBranchID);
                }
                //objView.ProjectList = CommonFunction.UserProjectList(Convert.ToString(SessionManagement.UserID));
                if (objView.ProjectId > 0)
                {
                    objView.SalesmenId = CommonFunction.GetSalesmanByProjectId(objView.ProjectId);
                    ExportProjectCostingReport(objView, "frm", true);
                }
                objView.ProjectList = CommonFunction.UserProjectListByYear(objView.Uid, objView.ReportYear, objView.SalesmenId);
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ProjectCostingReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                branchList = null;
            }
        }

        public class PaymentCls
        {
            public DateTime? Date { get; set; }
            //public string Description { get; set; }
            public string SupplierName { get; set; }
            public string InvNo { get; set; }
            public decimal? NonGst { get; set; }
            public decimal? Gst { get; set; }
        }

        public List<Database.additions_omissions> GetVODetails(int prjID)
        {
            List<Database.additions_omissions> getVOData = new List<Database.additions_omissions>();
            getVOData = CommonFunction.GetVODataForProjectCostingReport(prjID);
            return getVOData;
        }

        public List<GetPaymentDetails> GetPaymentDetails(int prjID)
        {
            List<GetPaymentDetails> getPaymentData = new List<GetPaymentDetails>();
            getPaymentData = CommonFunction.GetPaymentDataProjectCostingReport(prjID);
            return getPaymentData;
        }

        public List<Database.additions_omissions> GetEVODetails(int prjID)
        {
            List<Database.additions_omissions> getVOData = new List<Database.additions_omissions>();
            getVOData = CommonFunction.GetEVODataForProjectCostingReport(prjID);
            return getVOData;
        }

        public ActionResult ExportProjectCostingReport(ReportViewModel objView, string btntype, bool isGridData = false)
        {
            StringBuilder str = new StringBuilder("");

            Database.SSP_ProjectCostingReport_Result costingResult = new Database.SSP_ProjectCostingReport_Result();
            List<Database.receipt> receiptResult = new List<Database.receipt>();
            List<GetPaymentDetails> paymentResult = new List<GetPaymentDetails>();
            List<Database.additions_omissions> VOResult = new List<Database.additions_omissions>();
            List<Database.additions_omissions> additionResult = new List<Database.additions_omissions>();
            List<Database.additions_omissions> EVOResult = new List<Database.additions_omissions>();
            List<Database.discount> discountResult = new List<Database.discount>();
            try
            {
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    costingResult = objDB.SSP_ProjectCostingReport(objView.ProjectId).SingleOrDefault();
                    paymentResult = GetPaymentDetails(objView.ProjectId);

                    //paymentResult = (from p in objDB.payments
                    //                 join c in objDB.payment_detail on p.id equals c.payment_id
                    //                 join s in objDB.suppliers on p.supplier_id equals s.id
                    //                 where c.project_id == objView.ProjectId
                    //                 //where p.project_id == objView.ProjectId
                    //                 orderby p.payment_date
                    //                 select new PaymentCls
                    //                 {
                    //                     Date = p.payment_date,
                    //                    // Description = p.remarks,
                    //                        SupplierName=s.supplier_name, 
                    //                     InvNo = c.supplier_inv_number,
                    //                     NonGst = c.invoice_amount,
                    //                     Gst = c.payment_amount
                    //                 }
                    //             ).ToList();

                    receiptResult = objDB.receipts.Where(o => o.project_id == objView.ProjectId).ToList();
                    additionResult = objDB.additions_omissions.Where(o => o.project_id == objView.ProjectId).ToList();
                    VOResult = GetVODetails(objView.ProjectId);
                    EVOResult = GetEVODetails(objView.ProjectId);
                    discountResult = objDB.discounts.Where(o => o.project_id == objView.ProjectId).ToList();
                }
                str.Append("<table class='table'>");
                str.Append("<tr>");
                str.Append("<td colspan='9' style='text-align:center;'><strong>Project Costing</strong></td>");
                if (isGridData)
                {
                    str.Append("<td  colspan='2' style='text-align:right;'><button type='button'  class='btn btn-success pull-right' onclick='LoadDocumentsByProject(" + objView.ProjectId + ");' style='padding-top: 2px!Important;padding-bottom: 2px!Important;'>View Documents</button></td>");
                }
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<th style='text-align:left;' colspan='11'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                str.Append("</tr>");
                str.Append("<tr>");

                str.Append("<td><strong>Date:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.contract_date + "</td>");
                str.Append("<td></td>");
                str.Append("<td><strong>Project No:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.project_number + "</td>");
                str.Append("</tr>");
                str.Append("<tr>");
                str.Append("<td><strong>Sales Person:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.salesmen_name + "</td>");
                str.Append("<td></td>");
                str.Append("<td><strong>Email:</></td>");
                str.Append("<td colspan='4'>" + costingResult.email + "</td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td><strong>Customer:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.customer_name + "</td>");
                str.Append("<td></td>");
                str.Append("<td><strong>NRIC No:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.nric1 + "</td>");
                str.Append("</tr>");
                str.Append("<tr>");

                if (costingResult.Name2 != "")
                {
                    str.Append("<tr>");
                    str.Append("<td><strong>Customer:</strong></td>");
                    str.Append("<td colspan='4'>" + costingResult.Name2 + "</td>");
                    str.Append("<td></td>");
                    str.Append("<td><strong>NRIC No:</strong></td>");
                    str.Append("<td colspan='4'>" + costingResult.Nric2 + "</td>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                }
                objView.ReportYear = Convert.ToDateTime(costingResult.contract_date).Year;

                str.Append("<td><strong>Address:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.address + "</td>");
                str.Append("<td></td>");
                str.Append("<td><strong>Contact No:</strong></td>");
                str.Append("<td colspan='4'>" + costingResult.contact_no + "</td>");
                str.Append("</tr>");

                //str.Append("<tr>");
                //str.Append("<td colspan='2'><strong>Start Of  Work:</strong></td>");
                //str.Append("<td colspan='3'><strong>HDB Permit Date:</strong></td>");
                //str.Append("<td></td>");
                //str.Append("<td colspan='5'><strong>Completion:</strong></td>");
                //str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td colspan='5' style='vertical-align:top;'>");
                str.Append("<table class='table'>");
                str.Append("<tr style='background-color:#d2d2d2;'>");
                str.Append("<th>Date</th>");
                str.Append("<th>Supplier Name</th>");
                str.Append("<th>INV No</th>");
                str.Append("<th style='text-align:right;'>Budgeted Cost</th>");
                str.Append("<th style='text-align:right;'>Non GST Supplier</th>");
                str.Append("<th style='text-align:right;'>GST Supplier</th>");
                str.Append("<th style='text-align:right;'>Preview </th>");
                str.Append("</tr>");

                decimal totalBudgetAmount = 0;
                // List<Int64> chkDuplicateSupplierId = new List<Int64>();

                foreach (var payments in paymentResult)
                {
                    str.Append("<tr>");
                    if (Convert.ToDateTime(payments.Date).ToString("dd/MM/yyyy") == "01/01/1900")
                    {
                        str.Append("<td></td>");
                    }
                    else
                    {
                        str.Append("<td>" + Convert.ToDateTime(payments.Date).ToString("dd/MM/yyyy") + "</td>");
                    }
                    str.Append("<td>" + Convert.ToString(payments.SupplierName) + "</td>");
                    str.Append("<td>" + payments.InvNo + "</td>");
                    // if (chkDuplicateSupplierId.Contains(payments.SupplierId) == false)
                    // {
                    // chkDuplicateSupplierId.Add(payments.SupplierId);
                    totalBudgetAmount = totalBudgetAmount + Convert.ToDecimal(payments.BudgetAmount);
                    //}
                    //Icon ic = new Icon("icon.ico");
                    str.Append("<td align='right'>$" + payments.BudgetAmount + "</td>");
                    str.Append("<td align='right'>$" + payments.NonGst + "</td>");
                    str.Append("<td align='right'>$" + payments.Gst + "</td>");
                    str.Append("<td align='Center'><a title='Project Costing Report' class='btn-large' href='/Report/ProjectCostingReport?ProjectId=" + payments.projectId + "' ><span class='glyphicon glyphicon-usd'></span></a>");
                    str.Append("&nbsp; <a title='Preview Document' class='btn-large' onclick='InvoiceFilePreviewInReport(" + payments.projectId + ","+payments.ProjectBudgetdetailsId+");'  href='javascript:void(0)' ><i class='fa fa-file' aria-hidden='true'></i></a>");
                    str.Append("</td>");
                    str.Append("</tr>");
                    objView.ProjectBudget_DetailId = payments.ProjectBudgetdetailsId;
                }


                decimal non_gst_supplier_amt = Convert.ToDecimal(paymentResult.Sum(o => o.NonGst));
                decimal gst_supplier_amt = Convert.ToDecimal(paymentResult.Sum(o => o.Gst));

                string CompareBudgetStyle = "";
                if (totalBudgetAmount < gst_supplier_amt)
                {
                    CompareBudgetStyle = ";color:red!Important;";
                }


                str.Append("<tr>");
                str.Append("<td colspan='3' style='background-color:#efefef;'><strong>Total Costing</strong></td>");
                str.Append("<td style='background-color:#efefef;" + CompareBudgetStyle + "' align='right'><strong>$" + totalBudgetAmount.ToString() + "</strong></td>");
                str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + non_gst_supplier_amt.ToString() + "</strong></td>");
                str.Append("<td style='background-color:#efefef;' align='right'><strong>$" + gst_supplier_amt.ToString() + "</strong></td>");
                str.Append("</tr>");

                str.Append("</table>");
                str.Append("</td>");

                str.Append("<td></td>");

                str.Append("<td colspan='5' style='vertical-align:top;'>");
                str.Append("<table class='table'>");
                str.Append("<tr style='background-color:#d2d2d2;'>");
                str.Append("<th>Cheque Number</th>");
                str.Append("<th style='text-align:right;'>Payment Collected</th>");
                str.Append("<th style='text-align:right;'>Non GST Payment</th>");
                str.Append("<th style='text-align:right;'>GST Payment</th>");
                str.Append("<th>Date Payment</th>");
                str.Append("</tr>");

                foreach (var recepts in receiptResult)
                {
                    str.Append("<td>" + recepts.cheque_details + "</td>");
                    str.Append("<td>" + recepts.remarks + "</td>");
                    if (recepts.amount < 0)
                    {
                        str.Append("<td align='right' style='color:red;'>$" + recepts.amount + "</td>");
                    }
                    else
                    {
                        str.Append("<td align='right'>$" + recepts.amount + "</td>");
                    }

                    if (recepts.total_amount < 0)
                    {
                        str.Append("<td align='right' style='color:red;'>$" + recepts.total_amount + "</td>");
                    }
                    else
                    {
                        str.Append("<td align='right'>$" + recepts.total_amount + "</td>");
                    }

                    str.Append("<td>" + Convert.ToDateTime(recepts.receipt_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");
                }

                str.Append("<tr style='background-color:#D9EDF7;'>");
                str.Append("<td colspan='2' class='info'>Total Amount Collected</td>");
                str.Append("<td class='info' align='right'>$" + receiptResult.Sum(o => o.amount) + "</td>");
                str.Append("<td class='info' align='right'>$" + receiptResult.Sum(o => o.total_amount) + "</td>");
                str.Append("<td class='info'></td>");
                str.Append("</tr>");

                str.Append("<tr><td colspan='5'></td></tr>");

                str.Append("<tr style='background-color:#d2d2d2;'>");
                str.Append("<th colspan='2'>Contract</th>");
                str.Append("<th style='text-align:right;'>Non GST</th>");
                str.Append("<th style='text-align:right;'>w/ GST</th>");
                str.Append("<th style='text-align:right;'>GST</th>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td colspan='2'>1st Contract Amount (Project)</td>");
                str.Append("<td align='right'>$" + costingResult.contract_amount + "</td>");
                str.Append("<td align='right'>$" + costingResult.total_amount + "</td>");
                str.Append("<td align='right'>$" + costingResult.gst_amount + "</td>");
                str.Append("</tr>");
                decimal totalNonGSTAddition = Convert.ToDecimal(costingResult.contract_amount);
                decimal totalGSTAddition = Convert.ToDecimal(costingResult.total_amount);
                decimal totalGSTPer = Convert.ToDecimal(costingResult.gst_amount);

                foreach (var additions in VOResult)
                {
                    str.Append("<tr>");
                    if (additions.record_type == 1)
                    {
                        totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                        totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                        totalGSTPer = totalGSTPer + Convert.ToDecimal(additions.gst_amount);

                        str.Append("<td colspan='2'>");
                        str.Append("VO Additional");
                    }
                    else
                    {
                        if(additions.amount < 0)
                            totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                        else
                            totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(additions.amount);
                        if(additions.total_amount < 0)
                            totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                        else
                            totalGSTAddition = totalGSTAddition - Convert.ToDecimal(additions.total_amount);
                        totalGSTPer = totalGSTPer - Convert.ToDecimal(additions.gst_amount);
                        str.Append("<td colspan='2' style='color:red;'>");
                        str.Append("VO Omission");
                    }

                    str.Append("</td>");
                    str.Append("<td align='right'>$" + additions.amount + "</td>");
                    str.Append("<td align='right'>$" + additions.total_amount + "</td>");
                    str.Append("<td align='right'>$" + additions.gst_amount + "</td>");
                    str.Append("</tr>");
                }

                foreach (var additionsEVO in EVOResult)
                {
                    additionsEVO.record_type = 1; // this needs to be changes once record type is implemented
                    str.Append("<tr>");
                    totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additionsEVO.amount);
                    totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additionsEVO.total_amount);
                    totalGSTPer = totalGSTPer + Convert.ToDecimal(additionsEVO.gst_amount);

                    if (additionsEVO.amount > 0) //additionsEVO.record_type == 1
                    {
                        //totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additionsEVO.amount);
                        //totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additionsEVO.total_amount);
                        //totalGSTPer = totalGSTPer + Convert.ToDecimal(additionsEVO.gst_amount);

                        str.Append("<td colspan='2'>");
                        str.Append("EVO Additional");
                    }
                    else
                    {
                        //totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(additionsEVO.amount);
                        //totalGSTAddition = totalGSTAddition - Convert.ToDecimal(additionsEVO.total_amount);
                        //totalGSTPer = totalGSTPer - Convert.ToDecimal(additionsEVO.gst_amount);
                        str.Append("<td colspan='2' style='color:red;'>");
                        str.Append("EVO Omission");
                    }

                    str.Append("</td>");
                    str.Append("<td align='right'>$" + additionsEVO.amount + "</td>");
                    str.Append("<td align='right'>$" + additionsEVO.total_amount + "</td>");
                    str.Append("<td align='right'>$" + additionsEVO.gst_amount + "</td>");
                    str.Append("</tr>");
                }


                foreach (var additions in additionResult)
                {
                    str.Append("<tr>");                   
                    if (additions.record_type == 1)
                    {
                        totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                        totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                        totalGSTPer = totalGSTPer + Convert.ToDecimal(additions.gst_amount);

                        str.Append("<td colspan='2'>");
                        str.Append("Additional");
                    }
                    else if (additions.record_type == 4)
                    {
                        totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                        totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                        totalGSTPer = totalGSTPer + Convert.ToDecimal(additions.gst_amount);

                        str.Append("<td colspan='2'>");
                        str.Append("Electrical");
                    }
                    else
                    {
                        if (additions.amount < 0)
                            totalNonGSTAddition = totalNonGSTAddition + Convert.ToDecimal(additions.amount);
                        else
                            totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(additions.amount);
                        if (additions.total_amount < 0)
                            totalGSTAddition = totalGSTAddition + Convert.ToDecimal(additions.total_amount);
                        else
                            totalGSTAddition = totalGSTAddition - Convert.ToDecimal(additions.total_amount);
                        totalGSTPer = totalGSTPer - Convert.ToDecimal(additions.gst_amount);
                        str.Append("<td colspan='2' style='color:red;'>");
                        str.Append("Omission");
                    }

                    str.Append("</td>");
                    if (additions.record_type == Common.Constants.VoTypeList.Omission)
                    {
                        str.Append("<td align='right' style='color:red;'>$" + additions.amount + "</td>");
                        str.Append("<td align='right' style='color:red;'>$" + additions.total_amount + "</td>");
                        str.Append("<td align='right' style='color:red;'>$" + additions.gst_amount + "</td>");

                    }
                    else
                    {
                        str.Append("<td align='right'>$" + additions.amount + "</td>");
                        str.Append("<td align='right'>$" + additions.total_amount + "</td>");
                        str.Append("<td align='right'>$" + additions.gst_amount + "</td>");
                    }
                    
                    str.Append("</tr>");
                }

                foreach (var discounts in discountResult)
                {
                    str.Append("<tr>");


                    totalNonGSTAddition = totalNonGSTAddition - Convert.ToDecimal(discounts.amount);
                    totalGSTAddition = totalGSTAddition - Convert.ToDecimal(discounts.total_amount);
                    totalGSTPer = totalGSTPer - Convert.ToDecimal(discounts.gst_amount);
                    str.Append("<td colspan='2' style='color:blue;'>");
                    str.Append("Discount");

                    str.Append("</td>");
                    str.Append("<td align='right'>$" + discounts.amount + "</td>");
                    str.Append("<td align='right'>$" + discounts.total_amount + "</td>");
                    str.Append("<td align='right'>$" + discounts.gst_amount + "</td>");
                    str.Append("</tr>");
                }
                str.Append("<tr style='background-color:#D9EDF7;'>");
                str.Append("<td colspan='2' class='info'>Total Contract Amount</td>");
                str.Append("<td class='info' align='right'>$" + totalNonGSTAddition + "</td>");
                str.Append("<td class='info' align='right'>$" + totalGSTAddition + "</td>");
                str.Append("<td class='info' align='right'>$" + totalGSTPer + "</td>");
                str.Append("</tr>");

                str.Append("<tr><td colspan='5'></td></tr>");

                str.Append("<tr style='background-color:#D9EDF7;'><td colspan='2'>Balance Amount Due</td><td align='right' style='color:red;'>$" + (totalNonGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.amount))) + "</td><td align='right' style='color:red;'>$" + (totalGSTAddition - Convert.ToDecimal(receiptResult.Sum(o => o.total_amount))) + "</td><td align='right'></td></tr>");

                str.Append("<tr><td colspan='5'></td></tr>");

                str.Append("<tr>");
                str.Append("<td colspan='2' style='background-color:#efefef;'><strong>Profit & Loss</strong></td>");
                str.Append("<td style='background-color:#efefef; text-align:right;'><strong>Non GST</strong></td>");
                str.Append("<td style='background-color:#efefef; text-align:right;'><strong>w/ GST</strong></td>");
                str.Append("<td></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td colspan='2'>Final Amount Collected</td>");
                str.Append("<td align='right'>$" + receiptResult.Sum(o => o.amount) + "</td>");
                str.Append("<td align='right'>$" + receiptResult.Sum(o => o.total_amount) + "</td>");
                str.Append("<td></td>");
                str.Append("</tr>");

                str.Append("<tr>");
                str.Append("<td colspan='2'>Total Costing</td>");
                str.Append("<td align='right'>$" + paymentResult.Sum(o => o.NonGst) + "</td>");
                str.Append("<td align='right'>$" + paymentResult.Sum(o => o.Gst) + "</td>");
                str.Append("<td></td>");
                str.Append("</tr>");

                decimal costingNonGST = Convert.ToDecimal(paymentResult.Sum(o => o.NonGst));
                decimal costingWithGST = Convert.ToDecimal(paymentResult.Sum(o => o.Gst));

                decimal totalFinalAmtNonGST = Convert.ToDecimal(receiptResult.Sum(o => o.amount));
                decimal totalFinalAmtWithGST = Convert.ToDecimal(receiptResult.Sum(o => o.total_amount));

                str.Append("<tr>");
                str.Append("<td colspan='2'>Total Profit</td>");
                str.Append("<td align='right'>$" + (totalFinalAmtNonGST - costingNonGST) + "</td>");
                str.Append("<td align='right'>$" + (totalFinalAmtWithGST - costingWithGST) + "</td>");
                str.Append("<td></td>");
                str.Append("</tr>");

                decimal salesmenComm = 0;
                if (costingResult.saleman_commission > 0)
                {
                    salesmenComm = Math.Round((Convert.ToDecimal((totalFinalAmtNonGST - costingNonGST) * costingResult.saleman_commission) / 100), 2);
                }
                str.Append("<tr>");
                str.Append("<td colspan='2'>Sales Commission (%)</td>");
                str.Append("<td align='right'>$" + salesmenComm + "</td>");
                str.Append("<td></td>");
                str.Append("<td></td>");
                str.Append("</tr>");
                decimal profit = 100;
                if (totalFinalAmtNonGST > 0)
                {
                    profit = Math.Round(((totalFinalAmtNonGST - costingNonGST) / totalFinalAmtNonGST) * 100, 2);
                }

                str.Append("<tr>");
                str.Append("<td colspan='2'>Profit Margin (%)</td>");
                str.Append("<td align='right'>" + profit + "%</td>");
                str.Append("<td></td>");
                str.Append("<td></td>");
                str.Append("</tr>");

                str.Append("</table>");
                str.Append("</td>");
                str.Append("</tr>");
                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                    return null;
                }
                else
                {
                    objView.GridData = "";
                    if (btntype == "xls")
                    {
                        GenerateReport("ProjectCostingReport_" + objView.ProjectId + ".xls", str.ToString());
                        return null;
                    }
                    else if (btntype == "pdf")
                    {
                        return GeneratePDFReport("ProjectCostingReport_" + objView.ProjectId + ".pdf", str.ToString());
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportProjectCostingReport, Parameter : objView={objView} , btntype={btntype} ,  isGridData={isGridData}");
                return null;
            }
            finally
            {
                costingResult = null;
                receiptResult = null;
                paymentResult = null;
                VOResult = null;
                discountResult = null;
            }
        }

        public ActionResult GeneratePDFReport(string reportName, string reportData)
        {
            try
            {
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                int webPageWidth = 1150;
                int webPageHeight = 0;
                string pdf_page_size = "A4";
                SelectPdf.PdfPageSize pageSize = (SelectPdf.PdfPageSize)Enum.Parse(typeof(SelectPdf.PdfPageSize), pdf_page_size, true);
                string pdf_orientation = "Landscape";
                SelectPdf.PdfPageOrientation pdfOrientation = (SelectPdf.PdfPageOrientation)Enum.Parse(typeof(SelectPdf.PdfPageOrientation), pdf_orientation, true);
                // set converter options
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = webPageHeight;
                converter.Options.MarginTop = 5;
                converter.Options.MarginLeft = 20;
                converter.Options.MarginRight = 5;
                converter.Options.MarginBottom = 5;

                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(reportData);
                //  MemoryStream stream = new MemoryStream();

                //  // Response.ContentType = "application/pdf";
                //  // Response.AddHeader("content-disposition", "attachment;filename=" + reportName);
                //  // Response.Cache.SetCacheability(HttpCacheability.NoCache);

                //  doc.Save(stream);
                //  //doc.Save(Response, false, reportName);

                // // Response.Write(doc);
                //  doc.Close();            
                ////  Response.End();

                // save pdf document
                byte[] pdf = doc.Save();

                // close pdf document
                doc.Close();

                // return resulted pdf document
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = reportName;
                return fileResult;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GeneratePDFReport, Parameter : reportName={reportName} , reportData={reportData}");
                return null;
            }
            finally
            {
            }
        }

        public ActionResult BankChequeDetails(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            //  List<SelectListItem> branchList = new List<SelectListItem>();
            List<SelectListItem> salesmenList = new List<SelectListItem>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                salesmenList = SalesmenListByBranch(SessionManagement.SelectedBranchID);
                objView.Uid = uid;
                objView.SalenmenList = salesmenList;
                objView.BankList = CommonFunction.BankList();

                if (objView.from_date == null)
                {
                    objView.from_date = DateTime.Now;
                }

                if (objView.to_date == null)
                {
                    objView.to_date = DateTime.Now;
                }

                ExportBankChequeDetails(objView, true);

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: BankChequeDetails, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                salesmenList = null;
            }
        }

        public void ExportBankChequeDetails(ReportViewModel objView, bool isGridData = false)
        {
            List<Database.SSP_BankChequeReport_Result> report_items = new List<Database.SSP_BankChequeReport_Result>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    report_items = objDB.SSP_BankChequeReport(objView.from_date, objView.to_date, SessionManagement.SelectedBranchID, objView.BankId, Convert.ToInt32(objView.CheckNoFrom), Convert.ToInt32(objView.CheckNoTo)).ToList();
                }

                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped'>");

                if (isGridData == false)
                {
                    string bankname = "";
                    if (objView.BankId > 0)
                    {
                        bankname = CommonFunction.BankList().Where(o => o.Value == objView.BankId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }

                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='4'><b>Cheque Running Number</b></th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='4'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");
                    if (bankname != "")
                    {
                        str.Append("<tr>");
                        str.Append("<th style='text-align:left;'  colspan='4'><b>Bank: </b>" + bankname + "</th>");
                        str.Append("</tr>");
                    }

                    if (objView.CheckNoFrom != null)
                    {
                        str.Append("<tr>");
                        str.Append("<th style='text-align:left;'  colspan='2'><b>Cheque No From: </b>" + objView.CheckNoFrom + "</th>");
                        str.Append("<th style='text-align:left;'  colspan='2'><b>To: </b>" + objView.CheckNoTo + "</th>");
                        str.Append("</tr>");
                    }
                }

                str.Append("<tr>");
                str.Append("<th>Date</th><th>Chq Number</th>");
                str.Append("<th>Suppliers</th><th style='text-align:right;'>Amt Payable</th>");
                str.Append("</tr>");

                foreach (var items in report_items)
                {
                    str.Append("<tr>");
                    str.Append("<td>" + items.payment_date + "</td><td>" + items.cheque_number + "</td>");
                    str.Append("<td>" + items.supplier_name + "</td><td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.PaymentAmount - items.rebate_amount) + "</td>");
                    str.Append("</tr>");
                }

                str.Append("<tr>");
                str.Append("<td style='text-align:right;'  colspan='4'><b>$" + CommonFunction.ConvertAmountoDecimal(report_items.Sum(o => o.PaymentAmount) - report_items.Sum(o => o.rebate_amount)) + "</b></td>");
                str.Append("</tr>");

                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("BankChequeDetails.xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportBankChequeDetails, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                report_items = null;
            }
        }


        public ActionResult SupplierReport(ReportViewModel objView)
        {
            List<SelectListItem> supplierList = new List<SelectListItem>();
            string uid = User.Identity.GetUserId();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "00000000-0000-0000-0000-000000000000";
                }
                else if (User.IsInRole("Salesman"))
                {
                    if (!SessionManagement.IsBranchAdmin)
                        objView.Uid = uid;
                    else
                        objView.Uid = "00000000-0000-0000-0000-000000000000";
                }
                else
                    objView.Uid = uid;

                objView.YearList = CommonFunction.YearList();
                objView.AlphabetList = CommonFunction.AlphabetList();

                supplierList = CommonFunction.UserSupplierList(uid);

                if (objView.Alphabet != "0" && objView.Alphabet != null)
                {
                    if (objView.Alphabet == "1")
                    {
                        supplierList = supplierList.Where(o => (o.Text.StartsWith("0") || o.Text.StartsWith("1") || o.Text.StartsWith("2")
                        || o.Text.StartsWith("3") || o.Text.StartsWith("4") || o.Text.StartsWith("5") || o.Text.StartsWith("6") || o.Text.StartsWith("7")
                        || o.Text.StartsWith("8") || o.Text.StartsWith("9"))).ToList();


                    }
                    else
                    {
                        supplierList = supplierList.Where(o => (o.Text.StartsWith(objView.Alphabet))).ToList();
                    }
                    supplierList.Insert(0, new SelectListItem { Text = "Select Supplier", Value = "0" });
                }

                objView.SupplierList = supplierList;

                if (objView.ReportYear == 0)
                {
                    objView.ReportYear = DateTime.Now.Year;
                    objView.Alphabet = "0";
                }

                if (objView.SupplierId == 0 && (objView.Alphabet == "0" || objView.Alphabet == null) && (objView.InvoicesNo == "" || objView.InvoicesNo == null))
                {

                }
                else
                {
                    ExportSupplierReport(objView, true);
                }

                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: SupplierReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                supplierList = null;
            }
        }

        public void ExportSupplierReport(ReportViewModel objView, bool isGridData = false)
        {
            string uid = User.Identity.GetUserId();
            List<Database.SSP_SupplierReport_Result> report_items = new List<Database.SSP_SupplierReport_Result>();
            try
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    uid = "";
                }
                if (User.IsInRole("Salesman"))
                {
                    if (SessionManagement.IsBranchAdmin)
                        uid = "";
                }
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    report_items = objDB.SSP_SupplierReport(uid, objView.ReportYear, SessionManagement.SelectedBranchID, objView.SupplierId, objView.Alphabet, objView.InvoicesNo).ToList();
                }
                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' class='table table-striped table-condensed'>");
                string suppliername = "";
                if (isGridData == false)
                {
                    //suppliername = CommonFunction.UserSupplierList("").Where(o => o.Value == objView.SupplierId.ToString()).Select(o => o.Text).SingleOrDefault();

                    //str.Append("<tr>");
                    //str.Append("<th style='text-align:left;' colspan='9'><b>Supplier Report</b></th>");
                    //str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;'  colspan='9'><b>Branch: </b>" + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");
                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='9'><b>Year:</b> " + objView.ReportYear + "</td>");
                    str.Append("</tr>");

                }

                Int32 rowSpan = 1;
                Int32 flag = 1;
                decimal amtPaid = 0;
                decimal rebtAmt = 0;
                string projectName = "";
                string salesmenName = "";
                decimal totalInvAmt = 0;
                decimal totalGstAmt = 0;
                decimal totalInvAfterGst = 0;
                decimal totalRebate = 0;
                decimal totalPaid = 0;
                foreach (var supp in report_items.GroupBy(o => o.supplier_id))
                {
                    suppliername = report_items.Where(o => o.supplier_id == supp.Key).Select(o => o.supplier_name).FirstOrDefault();

                    str.Append("<tr>");
                    str.Append("<td style='text-align:left; font-size:18px;' colspan='11'><b>Supplier: " + suppliername + "</b></td>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th>Invoices No</th><th style='text-align:right;'>Invoice Amt</th><th style='text-align:right;'>GST Amt</th><th style='text-align:right;'>Invoice Amt After GST</th>");
                    str.Append("<th style='text-align:right; color:red;'>Rebate Amt</th><th style='text-align:right; color:red;'>Amount Paid</th>");
                    str.Append("<th>Project</th><th>Salesperson</th>");
                    str.Append("<th>Mode Of Payment</th><th style='text-align:left;'>Paid on</th>");
                    str.Append("<th>Branch</th>");
                    str.Append("</tr>");

                    foreach (var repMonth in report_items.Where(o => o.supplier_id == supp.Key).GroupBy(o => o.payment_month))
                    {
                        rowSpan = 1;
                        flag = 1;
                        str.Append("<tr>");
                        str.Append("<td colspan='11' style='text-align:center; color:red;'><b>" + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(repMonth.Key)) + " " + objView.ReportYear + "</b></td>");
                        str.Append("</tr>");

                        decimal? MonthInv = 0;
                        decimal? MonthGst = 0;
                        decimal? MonthInvAfterGst = 0;
                        decimal? MonthRebate = 0;
                        decimal? MonthAmtPaid = 0;

                        MonthInv = report_items.Where(o => o.payment_month == repMonth.Key && o.supplier_id == supp.Key).Sum(o => o.invoice_amount);
                        MonthGst = report_items.Where(o => o.payment_month == repMonth.Key && o.supplier_id == supp.Key).Sum(o => o.gst_amount);
                        MonthInvAfterGst = report_items.Where(o => o.payment_month == repMonth.Key && o.supplier_id == supp.Key).Sum(o => o.payment_amount);
                        List<Database.SSP_SupplierReport_Result> _items = new List<Database.SSP_SupplierReport_Result>();
                        string paymentDate = string.Empty;
                        long paymentId = 0;
                        List<long> lstMonthPaymentIds = new List<long>();
                        //bool singleRecord = false;
                        var MonthlyData = report_items.Where(o => o.payment_month == repMonth.Key && o.supplier_id == supp.Key).OrderBy(x => x.payment_id).ToList();
                        foreach (var items in MonthlyData)
                        {

                            if (!lstMonthPaymentIds.Contains(items.payment_id) && rowSpan == 1)
                            {
                                lstMonthPaymentIds.Add(items.payment_id);
                                paymentDate = items.payment_date;
                                paymentId = items.payment_id;
                                _items = MonthlyData.Where(o => o.payment_id == items.payment_id && o.payment_date == items.payment_date).ToList();
                                projectName = string.Join(",", _items.Select(o => o.project_name).Distinct().ToArray());
                                salesmenName = string.Join(",", _items.Select(o => o.salesmen_name).Distinct().ToArray());
                                amtPaid = Convert.ToDecimal(_items.Sum(o => o.payment_amount));
                                rebtAmt = Convert.ToDecimal(items.rebate_amount);
                                amtPaid = amtPaid - rebtAmt;
                                rowSpan = Convert.ToInt32(_items.Count());
                                flag = rowSpan;

                                totalRebate += Convert.ToDecimal(rebtAmt);
                                totalPaid += Convert.ToDecimal(amtPaid);

                                MonthRebate = MonthRebate + rebtAmt;
                                MonthAmtPaid = MonthAmtPaid + amtPaid;
                                //if (rowSpan == 1)
                                //{
                                //    singleRecord = true;
                                //}
                            }
                            //if (rowSpan != 1 || singleRecord)
                            //{
                            totalInvAmt += Convert.ToDecimal(items.invoice_amount);
                            totalGstAmt += Convert.ToDecimal(items.gst_amount);
                            totalInvAfterGst += Convert.ToDecimal(items.payment_amount);

                            str.Append("<tr>");
                            str.Append("<td>" + items.supplier_inv_number + "</td><td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.invoice_amount) + "</td>");
                            str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.gst_amount) + "</td>");
                            str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(items.payment_amount) + "</td>");
                            if (rowSpan > 1 && flag == rowSpan)
                            {
                                str.Append("<td style='text-align:right; vertical-align:middle; color:red;' rowspan=" + rowSpan + ">$" + CommonFunction.ConvertAmountoDecimal(rebtAmt) + "</td><td style='text-align:right; vertical-align:middle; color:red;' rowspan=" + rowSpan + ">$" + CommonFunction.ConvertAmountoDecimal(amtPaid) + "</td>");
                            }
                            else if (flag == rowSpan)
                            {
                                str.Append("<td style='text-align:right; color:red;'>$" + CommonFunction.ConvertAmountoDecimal(rebtAmt) + "</td><td style='text-align:right; color:red;'>$" + CommonFunction.ConvertAmountoDecimal(amtPaid) + "</td>");
                            }
                            str.Append("<td>" + items.project_name + "</td>");
                            str.Append("<td>" + items.salesmen_name + "</td>");
                            if (rowSpan > 1 && flag == rowSpan)
                            {
                                //str.Append("<td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + projectName + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + salesmenName + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.payment_date + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.branch_name + rowSpan + "</td>");
                                str.Append("<td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.payment_date + "</td><td style='vertical-align:middle;' rowspan=" + rowSpan + ">" + items.branch_name + "</td>");
                            }
                            else if (flag == rowSpan)
                            {
                                //str.Append("<td>" + projectName + "</td><td>" + salesmenName + "</td><td>" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td>" + items.payment_date + "</td><td>" + items.branch_name + rowSpan + "</td>");
                                str.Append("<td>" + items.mode_of_payment + " " + items.bank + " " + items.cheque_number + "</td><td>" + items.payment_date + "</td><td>" + items.branch_name + "</td>");
                            }

                            str.Append("</tr>");

                            flag = flag - 1;
                            if (flag == 0)
                            {
                                rowSpan = 1;
                                flag = 1;
                            }
                            //}
                        }


                        str.Append("<tr><td>Monthly Total</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(MonthInv) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(MonthGst) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(MonthInvAfterGst) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(MonthRebate) + "</td>");
                        str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(MonthAmtPaid) + "</td>");
                        str.Append("<td></td><td></td><td></td><td></td><td></td></tr>");

                    }

                }

                if (report_items != null && report_items.Count > 0)
                {
                    str.Append("<tr><td>Total</td>");
                    str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalInvAmt) + "</td>");
                    str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalGstAmt) + "</td>");
                    str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalInvAfterGst) + "</td>");
                    str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalRebate) + "</td>");
                    str.Append("<td style='text-align:right;'>$" + CommonFunction.ConvertAmountoDecimal(totalPaid) + "</td>");
                    str.Append("<td></td><td></td><td></td><td></td><td></td></tr>");
                }

                str.Append("</table>");

                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("Supplier_Report" + suppliername.Replace(" ", "_") + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportSupplierReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                report_items = null;
            }
        }


        public ActionResult LoanReport(ReportViewModel objView)
        {
            string uid = User.Identity.GetUserId();
            List<SelectListItem> branchList = new List<SelectListItem>();
            try
            {
                objView.Uid = uid;
                //if(objView.SearchSalesmenStatus==null)
                //{
                //    objView.Uid = "";
                //}
                objView.SalesmenAndUserList = CommonFunction.GetSalesmenAndUser();
                // objView.SalenmenList = CommonFunction.SalesmenListStatusWise(objView.Uid);
                objView.SalesmenStatusList = CommonFunction.SalesmenStatusList();
                if (User.IsInRole("Salesman"))
                {
                    objView.SalesmenAndUserList = CommonFunction.GetSalesmenIdByUserId(objView.Uid);
                }
                DateTime now = DateTime.Now;
                //var startDate = new DateTime(now.Year, 1, 1);
                //var endDate = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);

                //var startDate = new DateTime(now.Year, (now.Month - 5), 1);
                var CurrentstartDate = new DateTime(now.Year, now.Month, 1);
                var endDate = CurrentstartDate.AddMonths(1).AddDays(-1);
                if (objView.from_date == null)
                {
                    objView.from_date = CurrentstartDate.AddMonths(-5);
                }
                if (objView.to_date == null)
                {
                    objView.to_date = endDate;
                }

                if (User.IsInRole("Salesman"))
                {
                    objView.SalesmenOrUserId = CommonFunction.SalesmenIDByUserID(uid).ToString();
                }

                if (objView.SalesmenOrUserId != "" && objView.SalesmenOrUserId != null)
                {
                    ExportLoanReport(objView, true);
                }
                return View(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: LoanReport, Parameter : objView={objView}");
                return null;
            }
            finally
            {
                branchList = null;
            }
        }

        public void ExportLoanReport(ReportViewModel objView, bool isGridData = false)
        {
            Int32 empID = 0;
            Int32 reType = 2;
            string[] arr = new string[objView.SalesmenOrUserId.ToString().Split('_').Count() + 1]; //= objView.SalesmenOrUserId.ToString().Split('_');
            if(objView.SalesmenOrUserId.ToString().Contains('_'))
            {
                arr = objView.SalesmenOrUserId.ToString().Split('_');
            }
            else
            {
                arr[0] = arr[1] = objView.SalesmenOrUserId.ToString();
                //arr[1] = objView.SalesmenOrUserId.ToString();
            }
            List<Database.SSP_LoanReport_Result> reportData = new List<Database.SSP_LoanReport_Result>();
            try
            {
                if (arr[0].ToString() == "U")
                {
                    reType = 1;
                }
                empID = Convert.ToInt32(arr[1]);
                using (Database.PMSEntities objDB = new Database.PMSEntities())
                {
                    reportData = objDB.SSP_LoanReport(empID, reType, objView.from_date, objView.to_date).ToList();
                }
                string Name = "";
                StringBuilder str = new StringBuilder("");
                str.Append("<table width='100%' border='0' class='table table-striped'>");

                if (isGridData == false)
                {
                    if (objView.SalesmenOrUserId != "0")
                    {
                        Name = CommonFunction.GetSalesmenAndUser().Where(o => o.Value == objView.SalesmenOrUserId.ToString()).Select(o => o.Text).SingleOrDefault();
                    }

                    str.Append("<tr>");
                    str.Append("<th style='text-align:left;' colspan='6'><b>Employee Loan Report</b></th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<th  style='text-align:left;' colspan='6'><b>Branch: </b> " + SessionManagement.SelectedBranchName + "</th>");
                    str.Append("</tr>");

                    str.Append("<tr>");
                    str.Append("<td style='text-align:left;' colspan='2'><b>From:</b> " + Convert.ToDateTime(objView.from_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("<td style='text-align:left;' colspan='4'><b>To:</b> " + Convert.ToDateTime(objView.to_date).ToString("dd/MM/yyyy") + "</td>");
                    str.Append("</tr>");

                    if (Name != "")
                    {
                        str.Append("<tr>");
                        str.Append("<td colspan='6' style='text-align:left;'><b>Employee: </b> " + Name + "</td>");
                        str.Append("</tr>");
                    }
                }

                str.Append("<tr>");
                str.Append("<th>Date</th><th>Purpose</th>");
                str.Append("<th style='text-align:right;'>Closed File</th>");
                str.Append("<th style='text-align:right;'>Loan/Payouts</th>");
                str.Append("<th>Mode of Payment</th><th style='text-align:right;'>Total Balance</th>");
                str.Append("</tr>");

                //foreach(var items in reportData)
                // bool chk = true;
                for (int i = 0; i < reportData.Count; i++)
                {
                    str.Append("<tr>");
                    str.Append("<td>" + reportData[i].LoanDate.ToString("dd/MM/yyyy") + "</td><td>" + reportData[i].purpose + "</td>");
                    str.Append("<td style='text-align:right;'>");
                    if (reportData[i].rec_type == 2)
                    {
                        str.Append("$" + reportData[i].amount);
                    }
                    else
                    {
                        str.Append("-");
                    }
                    str.Append("</td>");
                    str.Append("<td style='text-align:right;'>");
                    if (reportData[i].rec_type == 1)
                    {
                        str.Append("$" + reportData[i].amount);
                    }
                    else
                    {
                        str.Append("-");
                    }
                    str.Append("</td>");
                    str.Append("<td>" + reportData[i].mode_of_payment + "");

                    if (reportData[i].bank_name != "")
                    {
                        str.Append(" " + reportData[i].bank_name);
                    }
                    if (reportData[i].cheque_number > 0)
                    {
                        str.Append(" " + reportData[i].cheque_number);
                    }

                    str.Append("</td><td style='text-align:right;'>");
                    str.Append("$" + reportData[i].TotalBalance + "");
                    //if (chk)
                    //{
                    //    if (reportData[i].TotalBalance == reportData[i].amount && reportData[i].rec_type == 1)
                    //    {
                    //        str.Append("-");
                    //    }
                    //    else
                    //    {
                    //        str.Append("$" + reportData[i].TotalBalance + "");
                    //    }
                    //}
                    //else
                    //{
                    //    str.Append("$" + reportData[i - 1].TotalBalance + "");
                    //}

                    str.Append("</td>");

                    str.Append("</tr>");
                    // chk = false;
                }

                str.Append("</table>");
                if (isGridData == true)
                {
                    objView.GridData = str.ToString();
                }
                else
                {
                    objView.GridData = "";
                    GenerateReport("LoanReport_" + Convert.ToDateTime(objView.from_date).Year.ToString() + ".xls", str.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: ExportLoanReport, Parameter : objView={objView} , isGridData={isGridData}");
            }
            finally
            {
                reportData = null;
            }

        }

        public void GenerateReport(string reportName, string reportData)
        {
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + reportName);
            Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
            Response.Write("<head>");
            Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write("<!--[if gte mso 9]><xml>");
            Response.Write("<x:ExcelWorkbook>");
            Response.Write("<x:ExcelWorksheets>");
            Response.Write("<x:ExcelWorksheet>");
            Response.Write("<x:Name>Report Data</x:Name>");
            Response.Write("<x:WorksheetOptions>");
            Response.Write("<x:Print>");
            Response.Write("<x:ValidPrinterInfo/>");
            Response.Write("</x:Print>");
            Response.Write("</x:WorksheetOptions>");
            Response.Write("</x:ExcelWorksheet>");
            Response.Write("</x:ExcelWorksheets>");
            Response.Write("</x:ExcelWorkbook>");
            Response.Write("</xml>");
            Response.Write("<![endif]--> ");
            Response.Write(reportData);
            Response.Write("</head>");
            Response.Flush();
        }

        [HttpPost]
        public ActionResult GetSalesmen(string salesmenStatus)
        {
            //  var ddlModeldatasurce=  CommonFunction.SalesmenListStatusWise(id);
            ProjectAdditionsViewModel objView = new ProjectAdditionsViewModel();
            try
            {
                //cc
                objView.SalesmenList = CommonFunction.SalesmenListStatusWise(salesmenStatus);

                return Json(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmen, Parameter : salesmenStatus={salesmenStatus}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }
        [HttpPost]
        public ActionResult GetSalesmenforLoanReport(string salesmenStatus)
        {
            //  var ddlModeldatasurce=  CommonFunction.SalesmenListStatusWise(id);
            ReportViewModel objView = new ReportViewModel();
            try
            {
                //cc
                objView.SalesmenAndUserList = CommonFunction.GetSalesmenAndUser(salesmenStatus);
                return Json(objView);
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, $"Method Name: GetSalesmenforLoanReport, Parameter : salesmenStatus={salesmenStatus}");
                return null;
            }
            finally
            {
                objView = null;
            }
        }
    }
}