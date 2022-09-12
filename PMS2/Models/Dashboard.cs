using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class Dashboard
    {
        public decimal TotalSales { get; set; }
        public int NoOfContracts { get; set; }
        public decimal CurrentMonthSales { get; set; }
        public int CurrentMonthContracts { get; set; }
        //public decimal AverageSales { get; set; }
        //public decimal LargestSales { get; set; }
        //public decimal CurrentDay { get; set; }
       
        //public decimal CompletedProjects { get; set; }
        //public decimal TotalPurchases { get; set; }
        //public decimal AveragePurchases { get; set; }
        //public decimal LargestPurchases { get; set; }
       
        //public int NumberofPurchases { get; set; }
    }

    public class Top10Sites
    {
        public string Site { get; set; }
        public decimal Amount { get; set; }
    }

    public class Top10SitesOwing
    {
        public string Site { get; set; }
        public decimal Amount { get; set; }
    }

    public class Top10SitesCustOwing
    {
        public string Site { get; set; }
        public decimal Amount { get; set; }
    }

    public class Top10OpenProjects
    {
        public string SalesMan { get; set; }
        public int PendingTotal { get; set; }
    }

    public class Top10Salesman
    {
        public string Salesman { get; set; }
        public decimal Amount { get; set; }
    }
    public class Top10Customer
    {
        public string name1 { get; set; }
        public decimal Amount { get; set; }
    }
    public class Top10Projects
    {
        public string project_name { get; set; }
        public decimal Amount { get; set; }
    }
    public class Top10Supplier
    {
        public string supplier_name { get; set; }
        public decimal Amount { get; set; }
    }

    public class GetTopTenLoan
    {
        public string Address { get; set; }
        public decimal Comission { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal loanpaid { get; set; }
        public decimal repaidloan { get; set; }

    }
    public class Top10SalesmanOwing
    {
        public string Salesman { get; set; }
        public decimal Amount { get; set; }
    }

    public class ProjectsSummaryReport
    {
        public int SNo { get; set; }
        public string Address { get; set; }
        public string SalesMan { get; set; }
        public DateTime Date { get; set; }
        public decimal BudgetedCost { get; set; }
        public decimal ContractValue { get; set; }
        public decimal ProgressClaim { get; set; }
        public int BalanceAmount { get; set; }
        public decimal CostingAmount { get; set; }
        public decimal Pcbc { get; set; }
    }
}