using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace PMS.Common
{
    public static class Mailing
    {
        public static bool SendMail(MailInfo ObjMailInfo)
        {
            bool EnableSSL = Constants.DefaultMailEnableSSl;
            string Host = Constants.DefaultMailSMTPServer;
            string UserName = Constants.DefaultMailUserName;
            string Password = Constants.DefaultMailPassword;
            int PortNo = Constants.DefaultMailPortNo;
            string CCMail = Constants.DefaultCCMail;
            string BccMail = Constants.DefaultBccMail;
            try
            {
                using (SmtpClient SMTPClient = new SmtpClient())
                {
                    SMTPClient.Host = Host;
                    SMTPClient.Port = PortNo;
                    SMTPClient.Credentials = new NetworkCredential(UserName, Password);
                    SMTPClient.EnableSsl = EnableSSL;

                    using (MailMessage Message = new MailMessage())
                    {
                        Message.From = new MailAddress(ObjMailInfo.FromMail, ObjMailInfo.DisplayName);
                        Message.To.Add(ObjMailInfo.ToMail);
                        Message.Subject = ObjMailInfo.Subject;
                        Message.Body = ObjMailInfo.Body;
                        Message.IsBodyHtml = true;
                        if (!string.IsNullOrEmpty(CCMail))
                        {
                            Message.CC.Add(CCMail);
                        }
                        if (!string.IsNullOrEmpty(BccMail))
                        {
                            Message.Bcc.Add(BccMail);
                        }

                        SMTPClient.Send(Message);
                    }
                }
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            finally
            {
                Host = null;
                UserName = null;
                Password = null;
                CCMail = null;
                BccMail = null;
                ObjMailInfo = null;
            }
        }

        public static void ExceptionMail(string ExceptionMessage, string projname)
        {
            String DefaultExceptionEmailFrom = Constants.DefaultFromMail;
            String DefaultExceptionEmailTo = Constants.DefaultFromMail;
            String DefaultExceptionExceptionCC1 = Constants.DefaultCCMail;
            String DefaultExceptionExceptionCC2 = Constants.DefaultBccMail;

            bool EnableSSL = Constants.DefaultMailEnableSSl;
            string Host = Constants.DefaultMailSMTPServer;
            string UserName = Constants.DefaultMailUserName;
            string Password = Constants.DefaultMailPassword;
            int PortNo = Constants.DefaultMailPortNo;

            try
            {
                if ((DefaultExceptionEmailFrom != null && DefaultExceptionEmailFrom.Trim() != String.Empty) &&
                    (DefaultExceptionEmailTo != null && DefaultExceptionEmailTo.Trim() != String.Empty))
                {
                    using (MailMessage mailmessage = new MailMessage())
                    {
                        mailmessage.From = new MailAddress(DefaultExceptionEmailFrom, Constants.DefaultMailUserName,
                                                           Encoding.UTF8);
                        mailmessage.To.Add(DefaultExceptionEmailTo);
                        if (DefaultExceptionExceptionCC1 != null && DefaultExceptionExceptionCC1.Trim() != String.Empty)
                        {
                            mailmessage.CC.Add(DefaultExceptionExceptionCC1);
                        }
                        if (DefaultExceptionExceptionCC2 != null && DefaultExceptionExceptionCC2.Trim() != String.Empty)
                        {
                            mailmessage.CC.Add(DefaultExceptionExceptionCC2);
                        }
                        projname = projname == "" ? "AGMS Exceptions" : "Mobile Primex Exceptions";
                        mailmessage.Subject = String.Format("{0} {1} {2} {3}", projname, Environment.MachineName,
                                                            Constants.DefaultDisplayName, Constants.BuildVersion);

                        mailmessage.SubjectEncoding = Encoding.UTF8;

                        mailmessage.Body = ExceptionMessage;

                        mailmessage.IsBodyHtml = false;

                        mailmessage.BodyEncoding = Encoding.UTF8;
                        mailmessage.Priority = MailPriority.High;
                        using (SmtpClient smtpClient = new SmtpClient())
                        {
                            smtpClient.Host = Host;
                            smtpClient.Credentials = new NetworkCredential(UserName, Password);
                            smtpClient.Port = PortNo; //25;
                            smtpClient.EnableSsl = EnableSSL;
                            smtpClient.Send(mailmessage);
                        }
                    }
                }
            }
            catch (Exception )
            {
                //ExceptionLog.WriteLog(, $"Method :ExceptionMail  , Parameter: ExceptionMessage={ExceptionMessage},projname={projname}");
            }
            finally
            {
                DefaultExceptionEmailFrom = null;
                DefaultExceptionEmailTo = null;
                DefaultExceptionExceptionCC1 = null;
                DefaultExceptionExceptionCC2 = null;
            }
        }

        //TO PREPARE MAIL BODY FOR FORGOT USER NAME OR PASSWORD
        public static string ForgotUserNameOrPasswordMailBody(string TemplateString, string UserName, string Password)
        {
            try
            {
                if (!string.IsNullOrEmpty(TemplateString))
                {
                    TemplateString = TemplateString.Replace("#UserName#", UserName);
                    TemplateString = TemplateString.Replace("#Password#", Password);
                }
                return TemplateString;
            }
            catch (Exception )
            {
                //ExceptionLog.WriteLog(, $"Method :ForgotUserNameOrPasswordMailBody  , Parameter: TemplateString={TemplateString},UserName={UserName},Password={Password}");
                return null;
            }
            finally
            {
                TemplateString = null;
                UserName = null;
                Password = null;
            }
        }

        //TO PREPARE MAIL BODY FOR FORGOT USER NAME OR PASSWORD
        public static string PasswordMailBody(string TemplateString, string UserName, string Password, string Link)
        {
            try
            {
                if (!string.IsNullOrEmpty(TemplateString))
                {
                    TemplateString = TemplateString.Replace("#UserName#", UserName);
                    TemplateString = TemplateString.Replace("#Password#", Password);
                    TemplateString = TemplateString.Replace("#Link#", Link);
                }
                return TemplateString;
            }
            catch (Exception )
            {
                //ExceptionLog.WriteLog(, $"Method : PasswordMailBody , Parameter: TemplateString={TemplateString},  UserName={UserName},  Password={Password},  Link={Link}");
                return null;
            }
            finally
            {
                TemplateString = null;
                UserName = null;
                Password = null;
            }
        }

        //TO PREPARE MAIL BODY FOR CONTACT US
        public static string ContactUsMailBody(string TemplateString, ContactUs ObjContactUs)
        {
            try
            {
                if (!string.IsNullOrEmpty(TemplateString))
                {
                    TemplateString = TemplateString.Replace("#Name#", ObjContactUs.Name);
                    TemplateString = TemplateString.Replace("#MobileNumber#", ObjContactUs.MobileNumber);
                    TemplateString = TemplateString.Replace("#Email#", ObjContactUs.Email);
                    TemplateString = TemplateString.Replace("#Subject#", ObjContactUs.Subject);
                    TemplateString = TemplateString.Replace("#Message#", ObjContactUs.Message);
                }
                return TemplateString;
            }
            catch (Exception )
            {
                //ExceptionLog.WriteLog(, $"Method :ContactUsMailBody  , Parameter: TemplateString={TemplateString}, ContactUs= {ObjContactUs.ToString()}");
                return null;
            }
            finally
            {
                TemplateString = null;
                ObjContactUs = null;
            }
        }
    }

    public class MailInfo : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public string DisplayName { get; set; }

        public string FromMail { get; set; }

        public string ToMail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(DisplayName)}={DisplayName}, {nameof(FromMail)}={FromMail}, {nameof(ToMail)}={ToMail}, {nameof(Subject)}={Subject}, {nameof(Body)}={Body}}}";
        }
    }

    public class ContactUs : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(MobileNumber)}={MobileNumber}, {nameof(Email)}={Email}, {nameof(Subject)}={Subject}, {nameof(Message)}={Message}}}";
        }
    }
}