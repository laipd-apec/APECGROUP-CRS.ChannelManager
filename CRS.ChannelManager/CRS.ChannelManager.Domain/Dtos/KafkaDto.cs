using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Domain.Dtos
{
    public class KafkaDto
    {
        public class KafkaRequestDto<T>
        {
            public KafkaRequestDto()
            {

            }

            public KafkaRequestDto(string action, T? data)
            {
                Action = action;
                Data = data;
            }

            // cấu trúc theo kiểu {name-object}.items.{actiontype}
            public string? Action { get; set; }
            public T? Data { get; set; }
        }

        public class KafkaResponseDto
        {
            // cấu trúc theo kiểu {name-object}.items.{actiontype}
            public string? Action { get; set; }

            public object? Data { get; set; }

            public string ActionType
            {
                get
                {
                    if (!string.IsNullOrEmpty(Action))
                    {
                        return Action.Split(".").Last();
                    }
                    return string.Empty;
                }
            }

            public string NameObject
            {
                get
                {
                    if (!string.IsNullOrEmpty(Action))
                    {
                        return Action.Split(".").First().Replace("_", string.Empty);
                    }
                    return string.Empty;
                }
            }
        }

        public class KafkaResponseDto<T>
        {
            // cấu trúc theo kiểu {name-object}.items.{actiontype}
            public string? Action { get; set; }

            public T? Data { get; set; }

            public string ActionType
            {
                get
                {
                    if (!string.IsNullOrEmpty(Action))
                    {
                        return Action.Split(".").Last();
                    }
                    return string.Empty;
                }
            }

            public string NameObject
            {
                get
                {
                    if (!string.IsNullOrEmpty(Action))
                    {
                        return Action.Split(".").First();
                    }
                    return string.Empty;
                }
            }
        }
    }
}
