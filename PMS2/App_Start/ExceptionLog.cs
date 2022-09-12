using Microsoft.Practices.EnterpriseLibrary.Logging;
using PMS.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

internal static class ExceptionLog
{
    private static readonly object dummy = new object();
    private static LogEntry glog;
    private static string gmpath;
    private static string gUserHostAddress;
    private static Uri Url;
    private static string gException = string.Empty;

    public static void WriteLog(Exception ex)
    {
        var log = new LogEntry();
        try
        {
            log.TimeStamp = DateTime.Now;
            log.Message = ex.ToString();
            log.Severity = TraceEventType.Information;
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            WriteLog(log, "");
            if (Constants.SendMailForExcep)
                SendMailThread(log.Message);
        }
        catch (Exception)
        {
            Logger.Write(log.Message += HttpContext.Current.Request.RawUrl + Environment.NewLine + ex.ToString());
        }
        finally
        {
            log = null;
        }
    }

    public static string get_log_file_path()
    {
        DateTime now = DateTime.Now;
        string now_string = (now.Year).ToString() + "_" + (now.Month).ToString("0#") + "_" + (now.Day).ToString("0#");

        string path = Constants.PhysicalPath + "\\" + "LogFiles\\" + now_string + ".txt";

        return path;
    }

    public static void WriteLog(Exception ex, string RequestURL)
    {
            var log = new LogEntry();
        try
        {
            log.TimeStamp = DateTime.Now;
            log.Message = RequestURL + Environment.NewLine + ex.ToString();
            log.Severity = TraceEventType.Information;
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            WriteLog(log, "");
            if (Constants.SendMailForExcep)
                SendMailThread(log.Message);
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
        }
    }

    public static void WriteLog(Exception ex, string SPName, List<object> lstParamsList)
    {
        var str = new StringBuilder();
        string Msg = string.Empty;
        var log = new LogEntry();
        try
        {
            log.TimeStamp = DateTime.Now;
            log.Message = ex.ToString();
            log.Message += Environment.NewLine;
            log.Message += "Stored Procedure :" + SPName;
            log.Message += Environment.NewLine;
            log.Message += "Parameters :";
            foreach (object strParam in lstParamsList)
            {
                log.Message += Convert.ToString(strParam) + ",";
            }
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            WriteLog(log, "");
            str.Append(log.Message);
            if (Constants.SendMailForExcep)
                SendMailThread(log.Message);
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
            str = null;
        }
    }

    public static void WriteLog(Exception ex, string SPName, object[] lstParams)
    {
        var str = new StringBuilder();
        string Msg = string.Empty;
        var log = new LogEntry();
        try
        {
            log.TimeStamp = DateTime.Now;
            log.Message = ex.ToString();
            log.Message += Environment.NewLine;
            log.Message += "Stored Procedure :" + SPName;
            log.Message += Environment.NewLine;
            log.Message += "Parameters :";
            foreach (object strParam in lstParams)
            {
                log.Message += Convert.ToString(strParam) + ",";
            }
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            WriteLog(log, "");
            str.Append(log.Message);
            if (Constants.SendMailForExcep)
                SendMailThread(log.Message);
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
            str = null;
        }
    }

    public static void WriteLog(Exception ex, DbCommand dbCommand, string exLayer)
    {
        var log = new LogEntry();
        try
        {
            string Msg = string.Empty;
            log.TimeStamp = DateTime.Now;
            log.Message = ex.ToString();
            if (exLayer == "DAL")
            {
                string strParams = ".......\n";
                strParams += "Exception Layer : " + "DAL" + Environment.NewLine;
                strParams += "SP Name " + dbCommand.CommandText;
                foreach (DbParameter param in dbCommand.Parameters)
                {
                    try
                    {
                        if (param.DbType.ToString() == "Byte")
                        {
                            int val = (int)param.Value;
                            strParams += Environment.NewLine + param.ParameterName + " = " + val + ",";
                        }
                        else if (param.DbType.ToString() == "String" || param.DbType.ToString() == "DateTime")
                            strParams += Environment.NewLine + param.ParameterName + " = " + "'" + param.Value + "',";
                        else
                            strParams += Environment.NewLine + param.ParameterName + " = " + param.Value + ",";
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                log.Message += strParams;
                if (Constants.SendMailForExcep)
                    SendMailThread(log.Message);
                if (ex.InnerException != null)
                {
                    log.Message += "\n" + "Inner Exception :  ";
                    log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
                }
                log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
                WriteLog(log, "");
            }
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
        }
    }

    public static void WriteLog(LogEntry log, string mpath)
    {
        gmpath = mpath;
        glog = log;
        if (HttpContext.Current != null)
        {
            gUserHostAddress = HttpContext.Current.Request.UserHostAddress;
            Url = HttpContext.Current.Request.Url;
        }
        else
        {
            gUserHostAddress = string.Empty;
            Url = null;
        }
        clsAThread aThread = null;
        try
        {
            aThread = new clsThread();
            aThread.AssignThread(WriteLogThread);
            aThread.WorkStart();
            while (aThread.IsAlive())
            {
            }
            aThread.WorkStop();
        }
        catch (Exception ex)
        {
            WriteLog(ex);
        }
        finally
        {
            aThread = null;
        }
    }

    public static void WriteLogThread()
    {
        LogEntry log = glog;
        string mpath = gmpath;

        string path = get_log_file_path();
        lock (dummy)
        {
            StreamWriter swLogWriter = File.AppendText(path);
            swLogWriter.Write(DateTime.Now + "        ");

            swLogWriter.Write("Debug Version:AGMS");
            swLogWriter.WriteLine("User IP Address:" + gUserHostAddress);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");
            swLogWriter.WriteLine(log.Severity);
            if (mpath != "")
                swLogWriter.WriteLine(mpath);
            swLogWriter.WriteLine("URL :" + Url);
            swLogWriter.WriteLine(log.Message);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");
            swLogWriter.Flush();
            swLogWriter.Close();
        }
    }

    public static void WriteDebugLog(String Parameters, string Response)
    {
        if (Constants.WriteDebugLog) //We are setting in Web.Config whether to write Debug log or not
        {
            var log = new LogEntry();
            DateTime now = DateTime.Now;
            string now_string =
                (now.Year).ToString()
                + "_" +
                (now.Month).ToString("0#")
                + "_" +
                (now.Day).ToString("0#");

            string path = Constants.PhysicalPath
                          + "\\"
                          + "LogFiles\\"
                          + now_string + "_Debuglog"
                          + ".txt";
            try
            {
                log.TimeStamp = DateTime.Now;
                log.Message = "Parameters:-" + Parameters + "     \n Response of this Service:" + Response;
                log.Severity = TraceEventType.Information;
                lock (dummy)
                {
                    StreamWriter swLogWriter = File.AppendText(path);
                    swLogWriter.Write(DateTime.Now + "        ");

                    swLogWriter.Write("Debug Version:AGMS        ");
                    swLogWriter.WriteLine("User IP Address: ");
                    swLogWriter.WriteLine(
                        "--------------------------------------------------------------------------------");
                    swLogWriter.WriteLine(log.Severity);
                    swLogWriter.WriteLine("URL :" + Url);
                    swLogWriter.WriteLine(log.Message);
                    swLogWriter.WriteLine(
                        "--------------------------------------------------------------------------------");
                    swLogWriter.Flush();
                    swLogWriter.Close();
                }
            }
            catch
            {
            }
            finally
            {
                log = null;
                path = null;
            }
        }
    }

    public static void WriteDebugLogWithResponse(string Parameters, object _Object)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        String Response = string.Empty;

        try
        {
            Response = jss.Serialize(_Object);
            ExceptionLog.WriteDebugLog(Parameters, Response);
        }
        finally
        {
            jss = null;
            Response = null;
            Parameters = null;
            _Object = null;
        }
    }

    public static void WriteRunQueryLog(string log)
    {
        string path = get_log_file_path();
        lock (dummy)
        {
            StreamWriter swLogWriter = File.AppendText(path);
            swLogWriter.Write(DateTime.Now + "        ");

            swLogWriter.Write("Debug Version:eZTrack          ");
            swLogWriter.WriteLine("User IP Address:" + HttpContext.Current.Request.UserHostAddress);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");

            swLogWriter.WriteLine("URL :" + HttpContext.Current.Request.Url);
            swLogWriter.WriteLine(log);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");
            swLogWriter.Flush();
            swLogWriter.Close();
        }
    }

    public static void CreateLOgFileForMailSending(Exception ex, string Operation)
    {
        string path;
        string FileName = string.Empty;
        StringBuilder Heading;
        string Data = string.Empty;
        string Version = string.Empty;

        try
        {
            Heading = new StringBuilder();
            path = Constants.PhysicalPath;
            FileName = "MailSendingError.log";
            Version = "AGMS";
            StreamWriter LogFile = File.AppendText(path);
            if (FileName != string.Empty)
            {
                LogFile.WriteLine(
                    String.Format(
                        "Operation Date: {0:dd-MM-yyyy}    Operation Time: {0:HH:mm:ss UTC}    Version: {1}",
                        DateTime.Now, Version));
                LogFile.WriteLine();
                LogFile.WriteLine(String.Format("Operation: {0}", Operation));
                LogFile.WriteLine(String.Format("Operation Status Message: {0}", ex));
                LogFile.WriteLine(String.Format("Operation Status Details: {0}",
                                                ex.ToString().Replace("Softpal", "I3CE")));
                LogFile.WriteLine();
                LogFile.WriteLine("--------------------------------------------------------------");
                LogFile.WriteLine();
                LogFile.Flush();
                LogFile.Close();
            }
            else
            {
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            path = null;
            Heading = null;
            Data = null;
        }
    }

    public static void SmsStausLog(Exception ex, string RequestURL)
    {
        var log = new LogEntry();
        try
        {
            if (ex.GetType() != typeof(ThreadAbortException))
            {
                log.TimeStamp = DateTime.Now;
                log.Message = RequestURL + Environment.NewLine + ex.ToString();
                log.Severity = TraceEventType.Information;

                WriteLog(log, "");
            }
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
        }
    }

    public static void MobileWritelog(params string[] list)
    {
        LogEntry log = glog;
        string mpath = gmpath;

        string path = get_Mobile_log_file_path();
        lock (dummy)
        {
            StreamWriter swLogWriter = File.AppendText(path);
            swLogWriter.Write(DateTime.Now + "        ");

            swLogWriter.Write("AGMSMobileException");
            swLogWriter.Write("  PNMobileOSType:" + list[2] + " ");
            swLogWriter.WriteLine("User IP Address:" + gUserHostAddress);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");
            swLogWriter.WriteLine(list[1]);
            if (mpath != "")
                swLogWriter.WriteLine(mpath);
            swLogWriter.WriteLine("URL :" + Url);
            swLogWriter.WriteLine(list[0]);
            swLogWriter.WriteLine("--------------------------------------------------------------------------------");
            swLogWriter.Flush();
            swLogWriter.Close();
        }
    }

    public static string get_Mobile_log_file_path()
    {
        DateTime now = DateTime.Now;
        string now_string =
            (now.Year).ToString()
            + "_" +
            (now.Month).ToString("0#")
            + "_" +
            (now.Day).ToString("0#");

        string path = Constants.PhysicalPath
                      + "\\"
                      + "LogFiles\\MobileLog_"
                      + now_string
                      + ".txt";

        return path;
    }

    public static void EmailLog(Exception ex)
    {
        var log = new LogEntry();
        try
        {
            log.TimeStamp = DateTime.Now;
            log.Message = ex.ToString();
            log.Severity = TraceEventType.Information;
            if (ex.InnerException != null)
            {
                log.Message += "\n" + "Inner Exception :  ";
                log.Message += "\n" + String.Concat(ex.InnerException.StackTrace, ex.InnerException.Message);
            }
            log.Message += "\n" + "Get Base Exception  :  " + "\n" + ex.GetBaseException().ToString();
            WriteLog(log, "");
        }
        catch (Exception)
        {
            WriteLog(ex);
        }
        finally
        {
            log = null;
        }
    }

    public static void SendMailThread(string exception)
    {
        clsAThread aThread = null;
        string excepmess;
        try
        {
            excepmess = "User IP Address: " + gUserHostAddress + Environment.NewLine + exception;
            gException = excepmess;
            aThread = new clsThread();
            aThread.AssignThread(DoWorkMail);
            aThread.WorkStart();
            while (aThread.IsAlive())
            {
            }
            aThread.WorkStop();
        }
        catch (Exception ex)
        {
            WriteLog(ex);
        }
        finally
        {
            aThread = null;
            excepmess = null;
        }
    }

    public static void DoWorkMail()
    {
        Mailing.ExceptionMail(gException, "");
    }
}

internal abstract class clsAThread
{
    protected Thread tr;

    public void AssignThread(ThreadStart trStrat)
    {
        tr = new Thread(trStrat);
    }

    public void AssignThread(ParameterizedThreadStart trStrat)
    {
        tr = new Thread(trStrat);
    }

    public abstract void WorkStart();

    public abstract void WorkStart(object param);

    public abstract void WorkComplete();

    public abstract void WorkStop();

    public abstract bool IsAlive();
}

internal class clsThread : clsAThread
{
    #region Methods workstart, Workcomplete, workstop

    public override void WorkStart()
    {
        try
        {
            if (tr.ThreadState.ToString() != "Stopped")
            {
                tr.IsBackground = false;
                tr.Start();
            }
        }
        catch (Exception ex)
        {
            ExceptionLog.WriteLog(ex);
        }
    }

    public override void WorkStart(object param)
    {
        try
        {
            tr.IsBackground = false;
            tr.Start(param);
        }
        catch (Exception ex)
        {
            ExceptionLog.WriteLog(ex);
        }
    }

    public override void WorkComplete()
    {
        try
        {
        }
        catch (Exception ex)
        {
            ExceptionLog.WriteLog(ex);
        }
    }

    public override void WorkStop()
    {
        try
        {
            tr.Abort();
        }
        catch (Exception ex)
        {
            ExceptionLog.WriteLog(ex);
        }
    }

    public override bool IsAlive()
    {
        try
        {
            return tr.IsAlive;
        }
        catch (Exception ex)
        {
            ExceptionLog.WriteLog(ex);
            return false;
        }
    }

    #endregion Methods workstart, Workcomplete, workstop
}