using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.BaseDto;
using Newtonsoft.Json.Linq;

namespace CRS.ChannelManager.Library.Utils
{
    public static class EnumExtensions
    {
        // Note that we never need to expire these cache items, so we just use ConcurrentDictionary rather than MemoryCache
        private static readonly ConcurrentDictionary<string, string> RawValueCache = new ConcurrentDictionary<string, string>();

        public static string GetDescription(this Enum value)
        {
            var key = $"{value.GetType().FullName}.{value}";

            var rawValue = RawValueCache.GetOrAdd(key, x =>
            {
                var name = (DescriptionAttribute[])value
                    .GetType()
                    .GetTypeInfo()
                    .GetField(value.ToString())
                    .GetCustomAttributes(typeof(DescriptionAttribute), false);

                return name.Length > 0 ? name[0].Description : value.ToString();
            });

            return rawValue;
        }

        private static EnumMemberAttribute GetEnumMemberAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<EnumMemberAttribute>();
        }

        public static string ToEnumMemberString(this Enum enu)
        {
            var attr = GetEnumMemberAttribute(enu);
            return attr != null ? attr.Value : enu.ToString();
        }

        public static IEnumerable<EnumDataSourceDto> EnumToList(Type typeEnum)
        {
            List<EnumDataSourceDto> result = new List<EnumDataSourceDto>();
            foreach (Enum iEnumItem in Enum.GetValues(typeEnum))
            {
                result.Add(new EnumDataSourceDto
                {
                    Value = iEnumItem.ToEnumMemberString(),
                    Text = GetDisplayName(typeEnum, iEnumItem.ToString())
                });
            }
            return result;
        }

        public static List<string> ValueToList(Type typeEnum)
        {
            List<string> result = new List<string>();
            foreach (object iEnumItem in Enum.GetValues(typeEnum))
            {
                result.Add(iEnumItem.ToString() ?? string.Empty);
            }
            return result;
        }

        public static string ValueToString(Type typeEnum)
        {
            List<string> result = new List<string>();
            foreach (object iEnumItem in Enum.GetValues(typeEnum))
            {
                result.Add(iEnumItem.ToString() ?? string.Empty);
            }
            if (!result.Any())
            {
                return string.Empty;
            }
            return string.Join(",", result);
        }

        public static string GetDisplayName(Type enumType, string enumValue)
        {
            MemberInfo memInfo = enumType.GetMember(enumValue)[0];
            var attrs = memInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            return ((DisplayAttribute)attrs[0]).GetName();
        }


    }
}
