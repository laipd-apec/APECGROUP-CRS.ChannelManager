using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CRS.ChannelManager.Library.Base;
using HandlebarsDotNet;
using CRS.ChannelManager.Library.BaseDto;

namespace CRS.ChannelManager.Library.Utils
{
    public static class MailUtils
    {

        public static MailRecord SendMail(UtilsDto.EmailInfo email, UtilsDto.EmailConfigInfo emailConfig, SendCompletedEventHandler SendCompletedCallback)
        {
            try
            {
                var smtp = new SmtpClient();
                smtp.Host = emailConfig.SmtpHost;
                smtp.Port = emailConfig.SmtpPort;
                smtp.EnableSsl = emailConfig.SmtpSsl == "Y";

                string userState = email.EmailId.ToString();
                var emailData = JsonConvert.DeserializeObject(email.Data);

                if (!string.IsNullOrEmpty(emailConfig.SmtpUsername) && !string.IsNullOrEmpty(emailConfig.SmtpPassword))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailConfig.SmtpUsername, emailConfig.SmtpPassword);
                }

                var subjectTemplate = Handlebars.Compile(email.Subject);
                var mailMessage = new MailMessage
                {
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.UTF8,
                    Subject = subjectTemplate(emailData),
                    From = new MailAddress(email.EmailFrom, email.EmailName)
                };

                mailMessage.To.Add(Regex.Replace(email.EmailTo, "[,; ]+", ","));
                if (!string.IsNullOrEmpty(email.EmailCC))
                    mailMessage.CC.Add(Regex.Replace(email.EmailCC, "[,; ]+", ","));

                if (!string.IsNullOrEmpty(email.EmailBCC))
                    mailMessage.Bcc.Add(Regex.Replace(email.EmailBCC, "[,; ]+", ","));

                mailMessage.IsBodyHtml = true;
                mailMessage.Body = email.Content;

                if (!string.IsNullOrEmpty(email.Data))
                {
                    var contentTemplate = Handlebars.Compile(email.Content);
                    var result = contentTemplate(emailData);
                    mailMessage.Body = result;
                }

                smtp.SendCompleted += SendCompletedCallback;
                smtp.SendAsync(mailMessage, userState);
                return new MailRecord(smtp, mailMessage);
            }
            catch (DomainExceptionBase ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
        }

        public static async Task<UtilsDto.SMSSendResponse> SendEmailByAPI(UtilsDto.EmailInfo email, UtilsDto.EmailConfigInfo emailConfig)
        {
            try
            {
                var client = new RestClient(emailConfig.ApiUrl);
                var request = new RestRequest(string.Empty, Method.Post);
                request.AddHeader("Content-Type", "application/json");

                UtilsDto.EmailMessageInfo messageInfo = new UtilsDto.EmailMessageInfo();
                messageInfo.key = emailConfig.SmtpPassword;
                UtilsDto.Message msg = new UtilsDto.Message();
                var emailData = JsonConvert.DeserializeObject(email.Data);
                var subjectTemplate = Handlebars.Compile(email.Subject);
                var contentTemplate = Handlebars.Compile(email.Content);
                msg.html = contentTemplate(emailData);
                msg.subject = subjectTemplate(emailData);
                msg.from_email = emailConfig.SmtpEmail;
                msg.from_name = email.EmailName;
                msg.to = new List<UtilsDto.To>();
                msg.to.Add(new UtilsDto.To
                {
                    email = email.EmailTo,
                    type = "to"
                });
                if (!string.IsNullOrEmpty(email.EmailCC))
                {
                    msg.to.Add(new UtilsDto.To
                    {
                        email = email.EmailCC,
                        type = "cc"
                    });
                }
                if (!string.IsNullOrEmpty(email.EmailBCC))
                {
                    msg.to.Add(new UtilsDto.To
                    {
                        email = email.EmailBCC,
                        type = "bcc"
                    });
                }
                if (email.Attachment != null && email.Attachment.Any())
                {
                    msg.attachments = new List<object>();
                    foreach (var item in email.Attachment)
                    {
                        msg.attachments.Add(item);
                    }
                }
                msg.subaccount = emailConfig.SubAccount;

                messageInfo.message = msg;
                messageInfo.async = true;

                string body = JsonConvert.SerializeObject(messageInfo);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    return new UtilsDto.SMSSendResponse
                    {
                        status = "Ok",
                        data = response.Content,
                        statusCode = (int)response.StatusCode
                    };
                }
                else
                {
                    return new UtilsDto.SMSSendResponse
                    {
                        status = "Error",
                        data = response.Content,
                        statusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                throw new DomainExceptionBase(ex.Message);
            }
        }


        /// <summary>
        /// record to keep track of clients and messages that need to be disposed
        /// </summary>
        public class MailRecord
        {
            public SmtpClient client;
            public MailMessage message;

            public MailRecord(SmtpClient cl, MailMessage mm)
            {
                client = cl;
                message = mm;
            }
        }


    }
}
