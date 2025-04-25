using HandlebarsDotNet;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CRS.ChannelManager.Library.BaseDto.UtilsDto;

namespace CRS.ChannelManager.Library.Utils
{
    public class SmsUtils
    {
        public static async Task<SMSSendResponse> SendSMS(SMSConfig config, SMSInfo sms)
        {
            try
            {
                var client = new RestClient(config.ApiUrl);
                var request = new RestRequest(string.Empty, Method.Post);
                request.AddHeader("Content-Type", "application/json");

                var jsonData = JsonConvert.DeserializeObject(sms.JsonData);
                var messageTemplate = Handlebars.Compile(sms.Message);
                string message = messageTemplate(jsonData);
                if (message.Length > config.MaxLength)
                {
                    return new SMSSendResponse
                    {
                        status = "Error",
                        data = $"Message length must be less than {config.MaxLength}"
                    };
                }

                SMSSendInfo smsSendInfo = new SMSSendInfo
                {
                    phoneNumber = sms.Phone,
                    message = message,
                    sourceType = config.APECType
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(smsSendInfo), ParameterType.RequestBody);
                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful)
                {
                    SMSSendResponse rs = JsonConvert.DeserializeObject<SMSSendResponse>(response.Content);
                    return rs;
                }
                else
                {
                    return new SMSSendResponse
                    {
                        status = "Error",
                        data = response.Content
                    };
                }
            }
            catch (Exception ex)
            {
                return new SMSSendResponse
                {
                    status = "Error",
                    data = ex.Message
                };
            }
        }

    }
}
