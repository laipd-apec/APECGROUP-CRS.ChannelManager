using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class TANotifycationTelegramDto
    {
        public class TANotifycationTelegramRequestDto
        {
            public int HotelId { get; set; }
            public string BookingNo { get; set; }
            public string BookingRef { get; set; }
            public string ChannelName { get; set; }
            public string HotelName { get; set; }
            public string PaymentStatus { get; set; }
            public string RoomTypeName { get; set; }
            public int TotalCustomer { get; set; }
            public int TotalAdult { get; set; }
            public int TotalChildren { get; set; }
            public string CustomerName { get; set; }
            public string Phone { get; set; }
            public DateTime BookingDate { get; set; }
            public DateTime ArrivalDate { get; set; }
            public DateTime DepartureDate { get; set; }
            public decimal TotalAmount { get; set; }
            public string UrlLink { get; set; }
            public string? Remark { get; set; }
        } 
    }
}
