using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WhatsAppApi;

namespace PMS.Common
{
    public static class WhatsAppService
    {
        public static string SendWhatsAppSMS(string userMobileList, string content)
        {
            try
            {
                string accountSid = "AC970d36936a0fea975ea17997cfcee5a1";
                string authToken = "05d81ded38428f372a30b080ca618f2b";

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                TwilioClient.Init(accountSid, authToken);
                var message = MessageResource.Create(
                 from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                 body: content,
                 to: new PhoneNumber("whatsapp:" + userMobileList)
             ) ;             
                Console.WriteLine(message.AccountSid);
                return "The whatsapp sms has been sent succussfully.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        

    }
}