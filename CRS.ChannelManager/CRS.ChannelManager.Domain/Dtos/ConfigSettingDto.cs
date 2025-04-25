using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class ConfigSettingDto
    {
        public class kafkaConsumerConfig
        {
            public string BootstrapServers { get; set; }
            public string ClientId { get; set; }
            public string GroupId { get; set; }
            public List<string> Topics { get; set; }
        }

        public class kafkaProducerConfig
        {
            public string BootstrapServers { get; set; }
            public string ClientId { get; set; }
            public string GroupId { get; set; }
            //public List<string> Topics { get; set; }
        }

        public class InventoryConfig
        {
            public string Domain { get; set; }

            public string IpServer { get; set; }

            public InventoryRouter Router { get; set; }
        }

        public class InventoryRouter
        {
            public string GetByRoomType { get; set; }
            public string GetByRoomTypes { get; set; }
            public string GetRoomAvailable { get; set; }
            public string SourceAvailableHotel { get; set; }
        }

        public class PermissionConfig
        {
            public string Domain { get; set; } = string.Empty;
            public string IpServer { get; set; } = string.Empty;
            public string ApiCreateAction { get; set; } = string.Empty;
            public string ApiCheckAction { get; set; } = string.Empty;
            public string ApiConfirmAccount { get; set; } = string.Empty;
            public string ApiCreateSendMail { get; set; } = string.Empty;
            public string ApiUserInfo { get; set; } = string.Empty;
            public string ApiUserNotify { get; set; } = string.Empty;
            public string ApiTANotification { get; set; } = string.Empty;
            public string ApiUserHotel { get; set; } = string.Empty;
        }

        public class EvoucherConfig
        {
            public string Domain { get; set; }

            public string IpServer { get; set; }

            public string Token { get; set; }

            public string AppName { get; set; }

            public string TranNo { get; set; }

            public EvoucherRouter Router { get; set; }
        }

        public class EvoucherRouter
        {
            public string DetailVoucher { get; set; }
            public string ListDetailVoucher { get; set; }
        }

        public class ImportExeclConfig
        {
            public string FileExtension { get; set; }
            public string LimitMemoryRoomSharing { get; set; }
        }

        public class OpenBakingConfig
        {
            public string Domain { get; set; }
            public string IpServer { get; set; }
            public string AccountNumber { get; set; }
            public AuthenConfig AuthenConfig { get; set; }
            public OpenBakingRouter Router { get; set; }
        }

        public class AuthenConfig
        {
            public string serviceId { get; set; }
            public string code { get; set; }
            public string signature { get; set; }
            public string keyencrypt { get; set; }
            public string keydecrypt { get; set; }
            public string ownerkeysecure { get; set; }
            public string customerkeysecure { get; set; }
            public string source { get; set; }
        }

        public class OpenBakingRouter
        {
            public string GenQR { get; set; }
        }

        public class SSOConfig
        {
            public string Domain { get; set; } = string.Empty;
            public string UrlApiConnect { get; set; } = string.Empty;
            public string UrlApiLogout { get; set; } = string.Empty;
            public string UrlApiAccount { get; set; } = string.Empty;
            public string UrlApiRegister { get; set; } = string.Empty;
            public string ApiUserInfo { get; set; } = string.Empty;
            public string ApiDeleteUser { get; set; } = string.Empty;
            public GrantTypeConfig GrantTypeConfig { get; set; }

        }

        public class GrantTypeConfig
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string ClientId { get; set; } = string.Empty;
            public string ClientSecret { get; set; } = string.Empty;
            public string GrantType { get; set; } = string.Empty;
        }

        public class WebSocketConfig
        {
            public string Domain { get; set; }
            public string IpServer { get; set; }
            public WebSocketRouter Router { get; set; }
        }

        public class WebSocketRouter
        {
            public string Connect { get; set; }
        }

        public class CMSPortalConfig
        {
            public string Domain { get; set; }

            public string IpServer { get; set; }

            public CMSPortalRouter Router { get; set; }
        }

        public class CMSPortalRouter
        {
            public string ViewBooking { get; set; }
        }

        public class LotusAPIConfig
        {
            public string Domain { get; set; }

            public string IpServer { get; set; }

            public LotusAPIRouter Router { get; set; }
        }

        public class LotusAPIRouter
        {
            public string CreateReservation { get; set; }
        }

        public class IZOTAWebHookConfig
        {
            public string Domain { get; set; }

            public string IpServer { get; set; }

            public IZOTAWebHookRouter Router { get; set; }
        }

        public class IZOTAWebHookRouter
        {
            public string Callback { get; set; }
        }
    }
}
