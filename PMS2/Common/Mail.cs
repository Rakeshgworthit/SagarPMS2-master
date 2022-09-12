using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.IO;
using PMS.Models;

namespace PMS.Common
{
    public class Mail
    {
        public static string Result = "Mail Sent Successful";

        public static bool SendMail(EMailInfo ObjMailInfo)
        {
            string Host = ConfigurationManager.AppSettings["DefaultMailSMTPServer"];
            string UserName = ConfigurationManager.AppSettings["DefaultMailUserName"];
            string Password = ConfigurationManager.AppSettings["DefaultMailPassword"];
            int PortNo = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultMailPortNo"]);
            bool EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["DefaultMailEnableSSl"]);
            string CCMail = ConfigurationManager.AppSettings["DefaultCCMail"];
            string BccMail = ConfigurationManager.AppSettings["DefaultBccMail"];
            string FromMail = ConfigurationManager.AppSettings["DefaultFromMail"];
            string DisplayName = ConfigurationManager.AppSettings["DefaultDisplayName"];
            string ProxyAddress = ConfigurationManager.AppSettings["ProxyAddress"];
            int ProxyPortNo = Convert.ToInt32(ConfigurationManager.AppSettings["ProxyPortNo"]);
            string IsProxy = ConfigurationManager.AppSettings["IsProxy"];
            //string DocumentFolderPath = ConfigurationManager.AppSettings["BillSavePath"].ToString();
            //string AttachmentPath = DocumentFolderPath + ObjMailInfo.FileName;
            string AttachmentPath = ObjMailInfo.AttachmentPath;
            string AttachmentName = ObjMailInfo.AttachmentName;
            byte[] bytes = ObjMailInfo.FileInBytes;
            try
            {
                SmtpClient SMTPClient;
                if (string.IsNullOrEmpty(IsProxy))
                {
                    SMTPClient = new SmtpClient();
                }
                else
                {
                    SMTPClient = new SmtpClient(ProxyAddress, ProxyPortNo);
                }
                SMTPClient.Host = Host;
                SMTPClient.Port = PortNo;
                NetworkCredential NetworkCred = new NetworkCredential(UserName, Password);
                SMTPClient.EnableSsl = EnableSSL;
                SMTPClient.UseDefaultCredentials = true;
                SMTPClient.Credentials = NetworkCred;                

                using (MailMessage Message = new MailMessage())
                {
                    Message.From = new MailAddress(FromMail, DisplayName, Encoding.UTF8);
                    //Message.From = new MailAddress(ObjMailInfo.DisplayName, ObjMailInfo.FromMail);
                    // Message.To.Add(ObjMailInfo.ToMail);
                    Message.Subject = ObjMailInfo.Subject;
                    Message.Body = ObjMailInfo.Body;
                    Message.IsBodyHtml = Convert.ToBoolean(ConfigurationManager.AppSettings["IsHTML"]);                   
                    if (ObjMailInfo.ToMail != null && ObjMailInfo.ToMail.Trim() != String.Empty)
                    {
                        if (ObjMailInfo.ToMail.Contains(','))
                        {
                            foreach (var item in ObjMailInfo.ToMail.Split(','))
                            {
                                Message.To.Add(item);
                            }
                        }
                        else
                        {
                            Message.To.Add(ObjMailInfo.ToMail);
                        }
                    }

                    if (ObjMailInfo.CCMail != null && ObjMailInfo.CCMail.Trim() != String.Empty)
                    {
                        if (ObjMailInfo.CCMail.Contains(','))
                        {
                            foreach (var item in ObjMailInfo.CCMail.Split(','))
                            {
                                Message.CC.Add(item);
                            }
                        }
                        else
                        {
                            Message.CC.Add(ObjMailInfo.CCMail);
                        }
                    }

                    if (ObjMailInfo.BCCMail != null && ObjMailInfo.BCCMail.Trim() != String.Empty)
                    {
                        if (ObjMailInfo.BCCMail.Contains(','))
                        {
                            foreach (var item in ObjMailInfo.BCCMail.Split(','))
                            {
                                Message.Bcc.Add(item);
                            }
                        }
                        else
                        {
                            Message.Bcc.Add(ObjMailInfo.BCCMail);
                        }
                    }

                    if (ObjMailInfo.Attachment != null)
                    {
                        string FileName = Path.GetFileName(ObjMailInfo.Attachment.FileName);
                        Message.Attachments.Add(new Attachment(ObjMailInfo.Attachment.InputStream, FileName));
                    }
                    else if (!string.IsNullOrEmpty(AttachmentPath) && File.Exists(AttachmentPath))
                    {
                        Message.Attachments.Add(new Attachment(new StreamReader(AttachmentPath).BaseStream, ObjMailInfo.FileName));
                    }
                    if (bytes != null)
                    {
                        Message.Attachments.Add(new Attachment(new MemoryStream(bytes), AttachmentName));
                    }
                    SMTPClient.Send(Message);
                }
                ObjMailInfo.Status = "Mail Sent Successfully.";
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLog.WriteLog(ex, "Method Name: SendMail");
                ObjMailInfo.Status = ex.Message;
                return false;
            }
            finally
            {
                // SaveSentMails(ObjMailInfo);
                Host = null;
                UserName = null;
                Password = null;
                CCMail = null;
                BccMail = null;
                ObjMailInfo = null;
            }
        }

        //public static string SignUpMailBody(string TemplateString, string LogIn, string URL)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(TemplateString))
        //        {
        //            TemplateString = TemplateString.Replace("#LogInId#", LogIn);
        //            TemplateString = TemplateString.Replace("#URL#", URL);
        //        }
        //        return TemplateString;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLog.WriteLog(ex, "Method Name: SignUpMailBody");
        //        return TemplateString;
        //    }
        //    finally
        //    {
        //        TemplateString = null;
        //    }
        //}
    }
}