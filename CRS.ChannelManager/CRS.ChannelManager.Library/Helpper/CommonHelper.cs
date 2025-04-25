using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Helpper
{
    public class CommonHelper
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            // Use reflection to get the PropertyInfo for the specified property name
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                // Use the PropertyInfo to get the value of the property
                object value = propertyInfo.GetValue(obj);
                return value;
            }
            else
            {
                // Property not found
                throw new ArgumentException($"Property '{propertyName}' not found in type '{obj.GetType().Name}'.");
            }
        }
        public static object GetDbSet(DbContext dbContext, Type entityType)
        {
            // Use reflection to call the generic Set method with the specified entity type
            MethodInfo setMethod = typeof(DbContext).GetMethod("Set");
            MethodInfo genericSetMethod = setMethod.MakeGenericMethod(entityType);
            object dbSet = genericSetMethod.Invoke(dbContext, null);
            return dbSet;
        }
        public static object GetFirstArrayContainsElementType(object value, Type childEntityType)
        {
            PropertyInfo[] properties = value.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = CommonHelper.GetPropertyValue(value, property.Name);
                if (propertyValue is IEnumerable array &&
                    CommonHelper.GetElementType(array).Equals(childEntityType))
                {
                    return propertyValue;
                }
            }
            return null;
        }
        public static Type GetElementType(IEnumerable enumerable)
        {
            Type type = enumerable.GetType();

            // If the IEnumerable is generic, get the type of its elements
            if (type.IsGenericType)
            {
                Type[] genericArguments = type.GetGenericArguments();
                return genericArguments.Length > 0 ? genericArguments[0] : null;
            }

            // If the IEnumerable is non-generic, try to get the type of its elements by iterating through them
            foreach (object element in enumerable)
            {
                if (element != null)
                {
                    return element.GetType();
                }
            }

            return null; // If the IEnumerable is empty or has null elements
        }
        public static void UpdatePropertyValue(object obj, string propertyName, object newValue)
        {
            // Use reflection to get the PropertyInfo for the specified property name
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                // Convert the new value to the property type and update the property
                propertyInfo.SetValue(obj, newValue);
            }
            else
            {
                // Property not found
                throw new ArgumentException($"Property '{propertyName}' not found in type '{obj.GetType().Name}'.");
            }
        }
        public static object ExecuteMethod(object obj, string methodName, object[] parameters)
        {
            // Use reflection to get the MethodInfo for the specified method name
            var methodInfos = obj.GetType().GetMethods().Where(m => m.Name.Equals(methodName));

            MethodInfo methodInfo = null;
            bool isMatch = false;
            foreach (var method in methodInfos)
            {
                var parameterInfos = method.GetParameters();
                if (parameterInfos.Length == parameters.Length)
                {
                    isMatch = parameterInfos.Length == 0;
                    if (parameterInfos.Length != 0)
                    {
                        for (int i = 0; i < parameterInfos.Length; i++)
                        {
                            if (!parameterInfos[i].ParameterType.Equals(parameters[i].GetType()))
                            {
                                break;
                            }
                            isMatch = i == parameterInfos.Length - 1;
                        }
                    }
                    if (isMatch)
                    {
                        methodInfo = method;
                        break;
                    }
                }
            }

            if (methodInfo != null)
            {
                // Execute the method dynamically
                object result = methodInfo.Invoke(obj, parameters);
                return result;
            }
            else
            {
                // Method not found
                throw new ArgumentException($"Method '{methodName}' not found in type '{obj.GetType().Name}'.");
            }
        }
    }
}
