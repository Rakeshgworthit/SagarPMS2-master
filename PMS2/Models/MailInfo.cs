using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMS.Models
{
    public class EMailInfo : IDisposable
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

        public HttpPostedFileBase Attachment { get; set; }

        public string FileName { get; set; }

        public string AttachmentPath { get; set; }

        public string UserName { get; set; }

        public string CCMail { get; set; }

        public string BCCMail { get; set; }

        public string BillNo { get; set; }

        public string Status { get; set; }

        public byte[] FileInBytes { get;
            set;
        }
        public string AttachmentName
        {
            get;
            set;
        }
    }
}