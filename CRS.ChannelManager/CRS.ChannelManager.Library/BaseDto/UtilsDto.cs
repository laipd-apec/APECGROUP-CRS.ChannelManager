using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class UtilsDto
    {
        public class EmailInfo
        {
            public long EmailId { get; set; }

            public string EmailName { get; set; } = string.Empty;

            public string EmailFrom { get; set; } = string.Empty;

            public string EmailTo { get; set; } = string.Empty;

            public string EmailCC { get; set; } = string.Empty;

            public string EmailBCC { get; set; } = string.Empty;

            public string Subject { get; set; } = string.Empty;

            public string Content { get; set; } = string.Empty;

            public string Status { get; set; } = string.Empty;

            public string Data { get; set; } = string.Empty;

            public List<object>? Attachment { get; set; }
        }

        public class EmailAttachmentInfo
        {
            public long Id { get; set; }

            public long EmailId { get; set; }

            public string FileName { get; set; }

            public long FileSize { get; set; }

            public string FilePath { get; set; }

            public Byte[] FileContent { get; set; }

            public string IsReport { get; set; }

            public string ReportModuleId { get; set; }

            public string ReportSubmodule { get; set; }

            public string ReportParams { get; set; }

            public string SessionKey { get; set; }

        }

        public class EmailConfigInfo
        {
            public string SmtpEmail { get; set; }

            public string SmtpHost { get; set; }

            public int SmtpPort { get; set; }

            public string SmtpSsl { get; set; }

            public string SmtpUsername { get; set; }

            public string SmtpPassword { get; set; }

            public int BatchSize { get; set; }

            public string SubAccount { get; set; }

            public string ApiUrl { get; set; }

        }

        public class SMSInfo
        {
            public int Id { get; set; }

            public string Phone { get; set; }

            public string Message { get; set; } = string.Empty;

            public string JsonData { get; set; } = string.Empty;

            public string Provider { get; set; } = string.Empty;

            public int Status { get; set; }

            public DateTime CreatedDate { get; set; }

            public string CreatedBy { get; set; }

            public DateTime SentDate { get; set; }

            public string ResponseMessage { get; set; }
        }

        public class SMSConfig
        {
            public string ApiUrl { get; set; }

            public int BatchSize { get; set; }

            public int MaxLength { get; set; }

            public string APECType { get; set; }
            public string DefaultOTP { get; set; }
        }

        public class SMSSendInfo
        {
            public string phoneNumber { get; set; }
            public string sourceType { get; set; }
            public string message { get; set; }
        }

        public class SMSSendResponse
        {
            public string status { get; set; }
            public int? statusCode { get; set; }
            public dynamic data { get; set; }
        }

        public class EmailMessageInfo
        {
            public string key { get; set; }
            public Message message { get; set; }
            public bool async { get; set; }
            public string ip_pool { get; set; }
            public string send_at { get; set; }
        }

        public class Headers
        {
        }

        public class Message
        {
            public string html { get; set; }
            public string text { get; set; }
            public string subject { get; set; }
            public string from_email { get; set; }
            public string from_name { get; set; }
            public List<To> to { get; set; }
            public Headers headers { get; set; }
            public bool important { get; set; }
            public bool track_opens { get; set; }
            public bool track_clicks { get; set; }
            public bool auto_text { get; set; }
            public bool auto_html { get; set; }
            public bool inline_css { get; set; }
            public bool url_strip_qs { get; set; }
            public bool preserve_recipients { get; set; }
            public bool view_content_link { get; set; }
            public string bcc_address { get; set; }
            public string tracking_domain { get; set; }
            public string signing_domain { get; set; }
            public string return_path_domain { get; set; }
            public bool merge { get; set; }
            public string merge_language { get; set; }
            public List<object> global_merge_vars { get; set; }
            public List<object> merge_vars { get; set; }
            public List<object> tags { get; set; }
            public string subaccount { get; set; }
            public List<object> google_analytics_domains { get; set; }
            public string google_analytics_campaign { get; set; }
            public Metadata metadata { get; set; }
            public List<object> recipient_metadata { get; set; }
            public List<object> attachments { get; set; }
            public List<object> images { get; set; }
        }

        public class Metadata
        {
            public string website { get; set; }
        }

        public class To
        {
            public string email { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }

    }
}
