using System;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

namespace PMS.Common
{
    public static class Constants
    {
        public static Int32 timeout;
        public static Int32 Timecountdown;
        public static string ApplicationName = "PMS";
        public static string DefaultMailPassword;
        public static string DefaultCCMail;
        public static string DefaultBccMail;
        public static string DefaultMailSMTPServer;
        public static int DefaultMailPortNo;
        public static bool DefaultMailEnableSSl;
        public static string DefaultMailUserName;
        public static string PhysicalPath;
        public static string LicenseEnabled;
        public static string DefaultFromMail;
        public static string DefaultDisplayName;
        public static string DefaultToMail;
        public static string SessionTimeOut;
        public static string PhysicalPathService;
        public static string SQLiteDBFilePath;
        public static string DocumentFolder;
        public static string GETDocuments;
        public static string ProjURL;
        public static string BuildVersion;
        public static bool WriteDebugLog;
        public static string CompanyLogo;
        public static Boolean SendMailForExcep;
        public static string XsltConnectionString;
        public static object XsltMailXSLTSFC;
        public static string DebugVersion;

        public static decimal DefaultBranchGST = GSTList.S;

        public static decimal DefaultZeroGstBranchGST = GSTList.Zero;

        public static int DefaultCountry = 1;

        public static string AdditionalDescription
        {
            get;
            set;
        }


        public static class GetTransaction
        {
            public static int Quotation = 1;
            public static int Contract = 2;
            public static int VO = 3;
            public static int EVO = 4;
            public static int CreateBudgetCost = 5;
            public static int VerifyBudgetCost = 6;
            public static int ApproveBudgetCost = 7;
        }

        public class VoTypeList
        {
            public static int Addition = 1;
            public static int Omission = 2;
            public static int Discount = 3;
            public static int Electrical = 4;
        }
        public static class UserError
        {
            public static bool IsUserCreated;
            public static string UserCreationerror;
        }
        public static class GetDocumentIdType 
        {
            public static int Contract = 6;
            public static int AdditionsOmissions = 7;
            public static int Master = 8;
        }

        public static class GSTList
        {
            public static int Zero = 0;
            public static int STD = 1;
            public static int MFT = 2;
            public static int Z = 3;
            public static int O = 4;
            public static int E = 5;
            public static int S = 7;
            public static int NA = 9;
        }
    
        public static class MailSubject
        {
            public static string ForgotUserNameOrPassword = "Forgot UserName Or Password";
            public static string ContactUs = "Contact Us";
        }

        public static class ExcelSheetName
        {
            public static string Role = "Role";
            public static string Customer = "Customer";
            public static string Demographic = "Demographic";
            public static string Lookup = "Lookup";
            public static string LookupDetail = "LookupDetail";
            public static string Port = "Port";
            public static string Terminal = "Terminal";
            public static string CheckList = "CheckList";
            public static string JobCrewDetail = "JobCrewDetail";
            public static string UOM = "UOM";
            public static string Supplier = "Supplier";
            public static string Surveyor = "Surveyor";
            public static string Vessel = "Vessel";
            public static string JobCrewPersonalDetails = "JobCrewPersonalDetails";
            public static string WaterCleaning = "WaterCleaning";
            public static string Consignment = "Consignment";
            public static string SpecialCase = "SpecialCase";
            public static string Currency = "Currency";
            public static string Document = "Document";
            public static string JobDocument = "JobDocument";
            public static string OtherService = "OtherService";
            public static string Agent = "Agent";
            public static string BoardingOfficer = "BoardingOfficer";
            public static string AnchorageLocation = "AnchorageLocation";
            public static string JobPilotBoarding = "JobPilotBoarding";
            public static string JobBoardingOfficer = "JobBoardingOfficer";
            public static string Charge = "Charge";
            public static string GLCode = "GLCode";
            public static string Launch = "Launch";
            public static string Category = "Category";
            public static string Task = "Task";
            public static string Pilot = "Pilot";
            public static string DAILY_SCHEDULAR = "DAILY_SCHEDULAR";
            public static string DOCUMENT_CONFIG = "DOCUMENT_CONFIG";
            public static string Is_CheckList_Autogenerated = "Is_CheckList_Autogenerated";
            public static string TASK_SCHEDULAR = "TASK_SCHEDULAR";
        }

        public static class Tables
        {
            public static string JO_LAUNCH_SERVICES = "JO_LAUNCH_SERVICES";
            public static string JO_MEDICAL_ASSISTANCE = "JO_MEDICAL_ASSISTANCE";
            public static string JO_FRESH_WATER_SUPPLY = "JO_FRESH_WATER_SUPPLY";
            public static string JO_CREW_PERSONAL_DETAILS = "JO_CREW_PERSONAL_DETAILS";
        }
        
        public static class AutoFilterColumns
        {
            public static string LAUNCH_SERIVICE_NAME = "LAUNCH_SERIVICE_NAME";
            public static string NATIONALITY = "NATIONALITY";
            public static string RANK = "RANK";
            public static string CLINIC_NAME = "CLINIC_NAME"; 
             public static string DOCTOR_NAME = "DOCTOR_NAME";
            public static string REASON = "REASON";
            public static string ANNEXURE = "ANNEXURE";
            public static string REMARKS = "REMARKS";
            public static string BOAT_OPERATORS = "BOAT_OPERATORS";
            public static string BARGE_OPERATOR_NAME = "BARGE_OPERATOR_NAME";
            public static string FLIGHT_DETAILS = "FLIGHT_DETAILS";
            public static string HOTEL = "HOTEL";
            public static string GENDER = "GENDER";
            public static string MEDICAL = "MEDICAL";
            public static string TICKET = "TICKET";
            public static string DESCRIPTION = "DESCRIPTION";
        }

        //public static class Tables
        //{
        //    public static string JO_MEDICAL_ASSISTANCE = "JO_MEDICAL_ASSISTANCE";
        //}

        //public static class AutoFilterColumns
        //{
        //    public static string NATIONALITY = "NATIONALITY";
        //}

        public static void BindingConfigvalues()
        {
            timeout = timeoutConfig;
            Timecountdown = TimecountdownConfig;
            DefaultMailPassword = DefaultMailPasswordConfig;
            DefaultCCMail = DefaultCCMailConfig;
            DefaultBccMail = DefaultBccMailConfig;
            DefaultMailSMTPServer = DefaultMailSMTPServerConfig;
            DefaultMailPortNo = DefaultMailPortNoConfig;
            DefaultMailEnableSSl = DefaultMailEnableSSlConfig;
            DefaultMailUserName = DefaultMailUserNameConfig;
            PhysicalPath = PhysicalPathConfig;
            LicenseEnabled = LicenseEnabledConfig;
            DefaultFromMail = DefaultFromMailConfig;
            DefaultDisplayName = DefaultDisplayNameConfig;
            DefaultToMail = DefaultToMailConfig;
            SessionTimeOut = SessionTimeOutConfig;
            PhysicalPathService = PhysicalPathServiceConfig;
            XsltConnectionString = pcfgConnectionString;
            XsltMailXSLTSFC = pcfgMailXSLTSFC;
            SQLiteDBFilePath = SQLiteDBFilePathConfig;
            DocumentFolder = DocumentFolderConfig;
            GETDocuments = GETDocumentsConfig;
            ProjURL = ProjURLConfig;
            BuildVersion = BuildVersionConfig;
            WriteDebugLog = WriteDebugLogConfig;
            CompanyLogo = CompanyLogoConfig;
            SendMailForExcep = pcfgSendMailForExcep;
            DebugVersion = pcfgDebugVersion;



        }
        private static string pcfgDebugVersion
        {
            get
            {
                try
                {
                    FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo
                                   (Assembly.GetExecutingAssembly().Location);
                     return fv.FileVersion.ToString();
                    }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        private static string DefaultMailPasswordConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultMailPassword"];
                }
                catch (Exception)
                {
                    return "Paresh-2011"; ;
                }
            }
        }

        private static string DefaultCCMailConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultCCMail"];
                }
                catch (Exception)
                {
                    return "info@softpal.com";
                }
            }
        }
        public static string pcfgConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    //Convert.ToString(ConfigurationManager.AppSettings["con1"]);
                }
                catch (Exception )
                {
                    //ExceptionLog.WriteLog(ex, "Parameters: pcfgConnectionString");
                    return "";

                }
            }
        }
        public static string pcfgMailXSLTSFC
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MailXSLTSFC"].ToString();
                    //Convert.ToString(ConfigurationManager.AppSettings["con1"]);
                }
                catch (Exception )
                {
                    //ExceptionLog.WriteLog(ex, "Parameters: pcfgConnectionString");
                    return "";
                }
            }
        }
        public static Int32 TimecountdownConfig
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["Timecountdown"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static Int32 timeoutConfig
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["timeout"]);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        private static string DefaultBccMailConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultBccMail"];
                }
                catch (Exception)
                {
                    return "info@softpal.com";
                }
            }
        }

        private static string DefaultMailSMTPServerConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultMailSMTPServer"];
                }
                catch (Exception)
                {
                    return ""; // "info@softpal.com";
                }
            }
        }

        private static int DefaultMailPortNoConfig
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultMailPortNo"]);
                }
                catch (Exception)
                {
                    return 0; //
                }
            }
        }

        private static bool DefaultMailEnableSSlConfig
        {
            get
            {
                try
                {
                    string DefaultMailEnableSSl = ConfigurationManager.AppSettings["DefaultMailEnableSSl"];
                    if (string.Compare(DefaultMailEnableSSl.ToLower(), "true") == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static string DefaultMailUserNameConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultMailUserName"];
                }
                catch (Exception)
                {
                    return ""; // "info@softpal.com";
                }
            }
        }

        private static string PhysicalPathConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["PhysicalPath"];
                }
                catch (Exception)
                {
                    return ""; // "info@softpal.com";
                }
            }
        }

        private static string LicenseEnabledConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["LicenseEnabled"];
                }
                catch (Exception)
                {
                    return ""; // "info@softpal.com";
                }
            }
        }

        private static string DefaultFromMailConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultFromMail"];
                }
                catch (Exception)
                {
                    return "info@softpal.com";
                }
            }
        }

        private static string DefaultDisplayNameConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultDisplayName"];
                }
                catch (Exception)
                {
                    return "Paresh";
                }
            }
        }

        private static string DefaultToMailConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DefaultToMail"];
                }
                catch (Exception)
                {
                    return "info@softpal.com";
                }
            }
        }

        private static string SessionTimeOutConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SessionTimeOut"];
                }
                catch (Exception)
                {
                    return "2000";
                }
            }
        }

        private static string PhysicalPathServiceConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["PhysicalPathService"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string SQLiteDBFilePathConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SQLiteDBFilePath"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string DocumentFolderConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DocumentFolder"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string GETDocumentsConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["GETDocuments"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string ProjURLConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ProjURL"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string BuildVersionConfig
        {
            get
            {
                try
                {
                    //return ConfigurationManager.AppSettings["BuildVersion"];
                    FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo
                                (Assembly.GetExecutingAssembly().Location);
                    return fv.FileVersion.ToString();
                    
                }
                catch (Exception)
                {
                    return "Build - Ver 01.000.173, 19th Aug 2017 20:30";
                }
            }
        }

        private static bool WriteDebugLogConfig
        {
            get
            {
                try
                {
                    string WriteDebugLog = ConfigurationManager.AppSettings["WriteDebugLog"];
                    if (string.Compare(WriteDebugLog.ToLower(), "true") == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private static string CompanyLogoConfig
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["CompanyLogo"];
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static bool pcfgSendMailForExcep
        {
            get
            {
                try
                {
                    string SendMailForExcep = ConfigurationManager.AppSettings["SendMailForExcep"];
                    if (string.Compare(SendMailForExcep.ToLower(), "true") == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
       
    }
}