using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Mapping
{
    public static class JsonToEntityMapper
    {
        public static T MapJsonToEntity<T>(string json) where T : new()
        {
            T entity = new T();
            var jObject = JObject.Parse(json);

            foreach (var property in typeof(T).GetProperties())
            {
                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                if (columnAttribute != null)
                {
                    var columnName = columnAttribute.Name;
                    if (columnName != null)
                    {
                        var value = jObject[columnName];
                        if (value != null)
                        {
                            try
                            {
                                var convertedValue = value.ToObject(property.PropertyType);
                                // Kiểm tra kiểu của convertedValue có tương thích với property.PropertyType không
                                if (convertedValue != null && property.PropertyType.IsAssignableFrom(convertedValue.GetType()))
                                {
                                    property.SetValue(entity, convertedValue); // Set value nếu kiểu khớp
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log hoặc xử lý nếu có lỗi khi chuyển đổi kiểu dữ liệu
                                Console.WriteLine($"Lỗi khi chuyển đổi giá trị cho thuộc tính {property.Name}: {ex.Message}");
                            }
                        }
                    }
                }
            }
            return entity;
        }

        public static string GetValueFromJson<T>(string json, string propertyName) where T : new()
        {
            string value = string.Empty;
            var jObject = JObject.Parse(json);
            if (jObject != null)
            {
                if (jObject.TryGetValue(propertyName, out JToken valueJson))
                {
                    value = valueJson.ToString();
                }
                else
                {
                    value = string.Empty;
                }
            }
            return value;
        }
    }
}
