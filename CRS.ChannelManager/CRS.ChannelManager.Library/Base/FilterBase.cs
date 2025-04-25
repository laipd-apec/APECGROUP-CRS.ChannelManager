using Core.Helpper;
using CRS.ChannelManager.Library.BaseEnum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Base
{
    public class FilterBase
    {

        public const string ConditionAnd = "And";
        public const string ConditionOr = "Or";

        public const string OperatorEqual = "==";
        public const string OperatorNotEqual = "!=";
        public const string OperatorGreaterThan = ">";
        public const string OperatorGreaterThanOrEqual = ">=";
        public const string OperatorLessThan = "<";
        public const string OperatorLessThanOrEqual = "<=";
        public const string OperatorContains = "Contains";
        public const string OperatorIn = "In";
        public const string OperatorNotIn = "NotIn";
        public const string OperatorStartsWith = "StartsWith";
        public const string OperatorEndsWith = "EndsWith";
        public const string OperatorLike = "Like";

        public class MoreFilterGroup
        {
            public List<FilterGroup> GroupFilters { get; set; }

            public string Condition { get; set; }

            public Expression<Func<T, bool>> BuildGroupExpression<T>() where T : class
            {
                if (!GroupFilters.Any()) throw new InvalidOperationException("No filters provided.");

                var parameter = Expression.Parameter(typeof(T), "g");
                List<Expression> lstCombinedExpression = new List<Expression>();
                Expression combinedExpressionGroup = null;
                foreach (var filter in GroupFilters)
                {
                    Expression combinedExpression = null;
                    if (filter != null)
                    {
                        foreach (var item in filter.Filters)
                        {
                            var expression = item.BuildExpression<T>();
                            if (expression != null)
                            {
                                var invokedExpr = Expression.Invoke(expression, parameter);

                                if (combinedExpression == null)
                                {
                                    combinedExpression = invokedExpr;
                                }
                                else
                                {
                                    combinedExpression = filter.Condition.Equals(ConditionAnd, StringComparison.OrdinalIgnoreCase)
                                        ? Expression.AndAlso(combinedExpression, invokedExpr)
                                        : Expression.OrElse(combinedExpression, invokedExpr);
                                }
                            }
                        }
                    }
                    if (combinedExpression != null)
                    {
                        lstCombinedExpression.Add(combinedExpression);
                    }
                }
                if (lstCombinedExpression.Any())
                {
                    foreach (var item in lstCombinedExpression)
                    {
                        if (combinedExpressionGroup == null)
                        {
                            combinedExpressionGroup = item;
                        }
                        else
                        {
                            combinedExpressionGroup = Condition.Equals(ConditionAnd, StringComparison.OrdinalIgnoreCase)
                                                  ? Expression.AndAlso(combinedExpressionGroup, item)
                                                  : Expression.OrElse(combinedExpressionGroup, item);
                        }
                    }
                }

                if (combinedExpressionGroup == null)
                {
                    combinedExpressionGroup = Expression.Constant(true);
                }
                return Expression.Lambda<Func<T, bool>>(combinedExpressionGroup, parameter);
            }
        }

        public class FilterGroup
        {
            public List<Filter> Filters { get; set; }

            public string Condition { get; set; }

            public Expression<Func<T, bool>> BuildGroupExpression<T>() where T : class
            {
                if (!Filters.Any()) throw new InvalidOperationException("No filters provided.");

                var parameter = Expression.Parameter(typeof(T), "x");
                Expression combinedExpression = null;

                foreach (var filter in Filters)
                {
                    var expression = filter.BuildExpression<T>();
                    if (expression != null)
                    {
                        var invokedExpr = Expression.Invoke(expression, parameter);
                        if (combinedExpression == null)
                        {
                            combinedExpression = invokedExpr;
                        }
                        else
                        {
                            combinedExpression = Condition.Equals(ConditionAnd, StringComparison.OrdinalIgnoreCase)
                                ? Expression.AndAlso(combinedExpression, invokedExpr)
                                : Expression.OrElse(combinedExpression, invokedExpr);
                        }
                    }
                }

                if (combinedExpression == null)
                {
                    combinedExpression = Expression.Constant(true);
                }

                return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
            }
        }

        public class Filter
        {
            public object PropertyValue { get; set; }
            public object ObjValue
            {
                get
                {
                    try
                    {
                        var json = JsonDocument.Parse(JsonSerializer.Serialize(PropertyValue));
                        var root = json.RootElement;
                        object valueJson = string.Empty;
                        switch (root.ValueKind)
                        {
                            case JsonValueKind.Undefined:
                                return valueJson;

                            case JsonValueKind.Object:
                                //Value = root.EnumerateObject();
                                return root.EnumerateObject();

                            case JsonValueKind.Array:
                                List<object> arrayValues = new List<object>();
                                foreach (JsonElement arrayElement in root.EnumerateArray())
                                {
                                    switch (arrayElement.ValueKind)
                                    {
                                        case JsonValueKind.String:
                                            arrayValues.Add((arrayElement.GetString() ?? string.Empty).ToUpper());
                                            break;
                                        case JsonValueKind.Number:
                                            arrayElement.TryGetInt32(out int intEValue);
                                            arrayValues.Add(intEValue);
                                            break;
                                    }
                                }
                                //Value = arrayValues;
                                return arrayValues;

                            case JsonValueKind.String:
                                //Value = root.GetString();
                                if (PropertyType == typeof(DateTime).Name)
                                {
                                    return root.GetDateTime();
                                }
                                return (root.GetString() ?? string.Empty).ToUpper();

                            case JsonValueKind.Number:
                                root.TryGetInt32(out int intValue);
                                //Value = intValue;
                                return intValue;

                            case JsonValueKind.True:
                                return true;

                            case JsonValueKind.False:
                                return false;

                            case JsonValueKind.Null:
                                return valueJson;

                            default:
                                return valueJson;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Value = string.Empty;
                        return string.Empty;
                    }
                }
            }
            public string PropertyName { get; set; }
            public string? PropertyType { get; set; }
            private object Value { get { return ObjValue; } }
            public string Operator { get; set; }

            public Expression<Func<T, bool>> BuildExpression<T>() where T : class
            {
                if (ObjValue == null || string.IsNullOrEmpty(ObjValue.ToString()))
                {
                    return null;
                }

                ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
                MemberExpression property = Expression.Property(parameter, PropertyName.Split('.')[0]);

                if (property.Type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(property.Type))
                {
                    return BuildNestedListExpression<T>(parameter, property);
                }
                if (property.Type.BaseType == typeof(Library.Base.EntityBase))
                {
                    // Truy cập thuộc tính con từ object
                    string[] propertyNames = PropertyName.Split('.');
                    MemberExpression nestedProperty = property;
                    for (int i = 1; i < propertyNames.Length; i++)
                    {
                        nestedProperty = Expression.Property(nestedProperty, propertyNames[i]);
                    }
                    ConstantExpression value;
                    if (Nullable.GetUnderlyingType(nestedProperty.Type) != null)
                    {
                        if (Value.GetType().Name == typeof(IEnumerable<>).Name || Value.GetType().Name == typeof(List<>).Name || Value.GetType().Name == typeof(ICollection<>).Name)
                        {
                            var valueList = (List<object>)Value;
                            List<ConstantExpression> constantValues = new List<ConstantExpression> { };
                            // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                            if (Nullable.GetUnderlyingType(nestedProperty.Type) != null)
                            {
                                var targetType = Nullable.GetUnderlyingType(nestedProperty.Type);
                                constantValues = valueList.Select(val => Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType ?? nestedProperty.Type), val))).ToList();
                            }
                            else
                            {
                                constantValues = valueList.Select(val => Expression.Constant(Convert.ChangeType(val, nestedProperty.Type))).ToList();
                            }
                            List<Expression> expressionList = new List<Expression>();
                            foreach (var item in constantValues)
                            {
                                var expressionChild = GetExpression(nestedProperty, item);
                                expressionList.Add(expressionChild);
                            }
                            if (!expressionList.Any())
                            {
                                return null;
                            }
                            var body = expressionList.Aggregate((e, next) => Operator == OperatorNotIn ? Expression.AndAlso(e, next) : Expression.OrElse(e, next));
                            return Expression.Lambda<Func<T, bool>>(body, parameter);
                        }
                        else
                        {
                            // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                            var targetType = Nullable.GetUnderlyingType(nestedProperty.Type);
                            var valueNew = Convert.ChangeType(Value, targetType);
                            if (targetType == typeof(DateTime))
                            {
                                value = Expression.Constant(valueNew);
                            }
                            else
                            {
                                value = Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType), Value));
                            }
                        }
                    }
                    else
                    {
                        // Chuyển đổi giá trị cần so sánh sang kiểu của thuộc tính con
                        value = Expression.Constant(Convert.ChangeType(Value, nestedProperty.Type));
                    }
                    Expression expression = GetExpression(nestedProperty, value);
                    return Expression.Lambda<Func<T, bool>>(expression, parameter);
                }
                else
                {
                    if (Value.GetType().Name == typeof(IEnumerable<>).Name || Value.GetType().Name == typeof(List<>).Name || Value.GetType().Name == typeof(ICollection<>).Name)
                    {
                        var valueList = (List<object>)Value;
                        List<ConstantExpression> constantValues = new List<ConstantExpression> { };
                        // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                        if (Nullable.GetUnderlyingType(property.Type) != null)
                        {
                            var targetType = Nullable.GetUnderlyingType(property.Type);
                            constantValues = valueList.Select(val => Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType ?? property.Type), val))).ToList();
                        }
                        else
                        {
                            constantValues = valueList.Select(val => Expression.Constant(Convert.ChangeType(val, property.Type))).ToList();
                        }
                        List<Expression> expressionList = new List<Expression>();
                        foreach (var item in constantValues)
                        {
                            var expressionChild = GetExpression(property, item);
                            expressionList.Add(expressionChild);
                        }
                        if (!expressionList.Any())
                        {
                            return null;
                        }
                        var body = expressionList.Aggregate((e, next) => Operator == OperatorNotIn ? Expression.AndAlso(e, next) : Expression.OrElse(e, next));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);
                    }
                    else
                    {
                        Expression expression;
                        ConstantExpression value;
                        if (Nullable.GetUnderlyingType(property.Type) != null)
                        {
                            // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                            var targetType = Nullable.GetUnderlyingType(property.Type);
                            var valueNew = Convert.ChangeType(Value, targetType);
                            if (targetType == typeof(DateTime))
                            {
                                value = Expression.Constant(valueNew);
                            }
                            else
                            {
                                value = Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType), Value));
                            }
                        }
                        else
                        {
                            // Chuyển đổi giá trị cần so sánh sang kiểu của thuộc tính con
                            value = Expression.Constant(Convert.ChangeType(Value, (property.Type)));
                        }
                        expression = GetExpression(property, value);
                        return Expression.Lambda<Func<T, bool>>(expression, parameter);
                    }
                }
            }

            private Expression<Func<T, bool>> BuildNestedListExpression<T>(ParameterExpression parameter, MemberExpression property) where T : class
            {
                Type elementType = property.Type.GetGenericArguments()[0];
                var innerParameter = Expression.Parameter(elementType, "i");

                var nestedProperty = Expression.Property(innerParameter, PropertyName.Split('.')[1]);
                ConstantExpression value;
                if (Value.GetType().Name == typeof(IEnumerable<>).Name || Value.GetType().Name == typeof(List<>).Name || Value.GetType().Name == typeof(ICollection<>).Name)
                {
                    var valueList = (List<object>)Value;
                    ConstantExpression[]? constantValues;
                    IEnumerable<BinaryExpression>? containsExpressions;
                    if (Nullable.GetUnderlyingType(nestedProperty.Type) != null)
                    {
                        var targetType = Nullable.GetUnderlyingType(nestedProperty.Type);
                        constantValues = valueList.Select(val => Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType), val))).ToArray();
                        containsExpressions = constantValues.Select(value => Expression.Equal(nestedProperty, value));
                    }
                    else
                    {
                        constantValues = valueList.Select(val => Expression.Constant(Convert.ChangeType(val, nestedProperty.Type))).ToArray();
                        containsExpressions = constantValues.Select(value => Expression.Equal(nestedProperty, value));
                    }
                    var innerExpression = containsExpressions.Aggregate<Expression>((accumulate, current) => Expression.OrElse(accumulate, current));
                    var innerLambda = Expression.Lambda(innerExpression, innerParameter);
                    MethodInfo anyMethod;
                    Expression call;
                    anyMethod = typeof(Enumerable).GetMethods().First(m => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(elementType);
                    call = Expression.Call(anyMethod, property, innerLambda);
                    if (Operator.Equals(OperatorNotIn, StringComparison.OrdinalIgnoreCase))
                    {
                        call = Expression.Not(call);
                    }
                    return Expression.Lambda<Func<T, bool>>(call, parameter);
                }
                else
                {
                    if (Nullable.GetUnderlyingType(nestedProperty.Type) != null)
                    {
                        // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                        var targetType = Nullable.GetUnderlyingType(nestedProperty.Type);
                        value = Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType), Value));
                    }
                    else
                    {
                        if (nestedProperty.Type.BaseType == typeof(Library.Base.EntityBase))
                        {
                            // Tạo biểu thức cho Any
                            //var anyMethodChild = typeof(Enumerable).GetMethods()
                            //    .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
                            //    .MakeGenericMethod(nestedProperty.Type);

                            //// Tạo biểu thức cho Child
                            //var childProperty = Expression.Property(property, "RoomType");

                            // Truy cập thuộc tính con từ object
                            int indexStart = 2;
                            int count = PropertyName.Split('.').Length - indexStart;
                            string[] propertyNames = PropertyName.Split('.').Skip(indexStart).Take(count).ToArray();
                            MemberExpression nestedPropertyChild = nestedProperty;
                            for (int i = 0; i < propertyNames.Length; i++)
                            {
                                nestedPropertyChild = Expression.Property(nestedPropertyChild, propertyNames[i]);
                            }
                            if (Nullable.GetUnderlyingType(nestedProperty.Type) != null)
                            {
                                // Nếu là nullable, chuyển đổi sang kiểu không nullable trước, sau đó tạo nullable
                                var targetType = Nullable.GetUnderlyingType(nestedPropertyChild.Type);
                                var valueNew = Convert.ChangeType(Value, targetType);
                                if (targetType == typeof(DateTime))
                                {
                                    value = Expression.Constant(valueNew);
                                }
                                else
                                {
                                    value = Expression.Constant(Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(targetType), Value));
                                }
                            }
                            else
                            {
                                // Chuyển đổi giá trị cần so sánh sang kiểu của thuộc tính con
                                value = Expression.Constant(Convert.ChangeType(Value, nestedPropertyChild.Type));
                            }
                            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                            var containsMethod = typeof(List<string>).GetMethod("Any", new[] { typeof(string) });
                            var equalsExpression = Expression.Call(nestedPropertyChild, equalsMethod, value);
                            var innerExpressionChild = GetExpression(nestedPropertyChild, value);
                            var innerLambdaChild = Expression.Lambda(innerExpressionChild, innerParameter);
                            var anyMethodChild = typeof(Enumerable).GetMethods().First(m => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(elementType); ;
                            var callChild = Expression.Call(anyMethodChild, property, innerLambdaChild);
                            return Expression.Lambda<Func<T, bool>>(callChild, parameter);
                        }
                        else
                        {
                            value = Expression.Constant(Convert.ChangeType(Value, nestedProperty.Type));
                        }
                    }
                    var innerExpression = GetExpression(nestedProperty, value);
                    var innerLambda = Expression.Lambda(innerExpression, innerParameter);
                    var anyMethod = typeof(Enumerable).GetMethods().First(m => m.Name == "Any" && m.GetParameters().Length == 2).MakeGenericMethod(elementType);
                    var call = Expression.Call(anyMethod, property, innerLambda);
                    return Expression.Lambda<Func<T, bool>>(call, parameter);
                }
            }

            private void GetDateExpression(ref Expression property, ref ConstantExpression value)
            {
                var propertyDate = Expression.Property(property, "Date");
                var valueDate = Expression.Constant(((DateTime)value.Value).Date);
                if (!string.IsNullOrEmpty(PropertyType) && PropertyType.Equals("date", StringComparison.OrdinalIgnoreCase))
                {
                    property = propertyDate;
                    //Operator = OperatorContains;
                }
                //switch (Operator)
                //{
                //    case "==": return Expression.Equal(propertyDate, valueDate);
                //    case "!=": return Expression.NotEqual(propertyDate, valueDate);
                //    case ">": return Expression.GreaterThan(propertyDate, valueDate);
                //    case ">=": return Expression.GreaterThanOrEqual(propertyDate, valueDate);
                //    case "<": return Expression.LessThan(propertyDate, valueDate);
                //    case "<=": return Expression.LessThanOrEqual(propertyDate, valueDate);
                //    default: throw new NotSupportedException($"Operator '{Operator}' is not supported.");
                //}
            }

            private Expression GetExpression(Expression property, ConstantExpression value)
            {
                if (property.Type == typeof(DateTime))
                {
                    GetDateExpression(ref property, ref value);
                }
                MethodInfo? unaccentMethod = null;

                if (property.Type == typeof(string))
                {
                    property = Expression.Call(property, "ToUpper", null);
                }
                if ((Nullable.GetUnderlyingType(property.Type) != null))
                {
                    property = Expression.Convert(property, value.Type);
                }
                switch (Operator)
                {
                    case OperatorEqual: return Expression.Equal(property, value);
                    case OperatorNotEqual: return Expression.NotEqual(property, value);
                    case OperatorGreaterThan: return Expression.GreaterThan(property, value);
                    case OperatorGreaterThanOrEqual: return Expression.GreaterThanOrEqual(property, value);
                    case OperatorLessThan: return Expression.LessThan(property, value);
                    case OperatorLessThanOrEqual: return Expression.LessThanOrEqual(property, value);
                    case OperatorContains: return Expression.Call(property, typeof(string).GetMethod("Contains", new[] { typeof(string) }), value);
                    case OperatorNotIn: return Expression.NotEqual(property, value);
                    case OperatorIn: return Expression.Equal(property, value);
                    case OperatorStartsWith: return Expression.Call(property, typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), value);
                    case OperatorEndsWith: return Expression.Call(property, typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), value);
                    default: throw new NotSupportedException($"Operator '{Operator}' is not supported.");
                }
            }

        }

        public static class FilterService
        {
            public static IQueryable<T> GetEntities<T>(IQueryable<T> query, Expression<Func<T, bool>> filter = null) where T : class
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return query;
            }

            public static IQueryable<T> GetEntities<T>(IQueryable<T> query, List<FilterGroup> filterGroups) where T : class
            {
                if (filterGroups != null && filterGroups.Any())
                {
                    foreach (var item in filterGroups)
                    {
                        query = query.Where(item.BuildGroupExpression<T>());
                    }
                }
                return query;
            }

            public static IQueryable<T> GetEntities<T>(IQueryable<T> query, MoreFilterGroup moreFilterGroups) where T : class
            {
                if (moreFilterGroups != null && moreFilterGroups.GroupFilters.Any())
                {
                    query = query.Where(moreFilterGroups.BuildGroupExpression<T>());
                }
                return query;
            }

            public static IQueryable<T> GetFilteredEntities<T>(IQueryable<T> query, List<FilterGroup> filterGroups) where T : class
            {

                if (filterGroups != null && filterGroups.Any())
                {
                    //foreach (var item in filterGroups)
                    //{
                    //    foreach (var itemFilter in item.Filters)
                    //    {
                    //        var setValue = itemFilter.ObjValue;
                    //    }
                    //}
                    var mainExpression = GetMainExpression<T>(filterGroups[0]);
                    foreach (var filterGroup in filterGroups.Skip(1))
                    {
                        var subExpression = GetMainExpression<T>(filterGroup);
                        mainExpression = CombineExpressions(filterGroup.Condition, mainExpression, subExpression);
                    }

                    query = query.Where(mainExpression);
                }

                return query;
            }

            private static Expression<Func<T, bool>> GetMainExpression<T>(FilterGroup filterGroup) where T : class
            {
                var filterExpressions = filterGroup.Filters.Where(filter => filter.BuildExpression<T>() != null).Select(filter => filter.BuildExpression<T>()).ToList();
                var mainExpression = filterExpressions[0];

                foreach (var expression in filterExpressions.Skip(1))
                {
                    mainExpression = CombineExpressions(filterGroup.Condition, mainExpression, expression);
                }

                return mainExpression;
            }

            private static Expression<Func<T, bool>> CombineExpressions<T>(string op, Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
            {
                var paramExpr = Expression.Parameter(typeof(T), "x");
                var expr1Body = new ReplaceExpressionVisitor(expression1.Parameters[0], paramExpr).Visit(expression1.Body);
                var expr2Body = new ReplaceExpressionVisitor(expression2.Parameters[0], paramExpr).Visit(expression2.Body);

                if (op.Equals(ConditionAnd, StringComparison.OrdinalIgnoreCase))
                    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1Body, expr2Body), paramExpr);
                else
                    return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1Body, expr2Body), paramExpr);
            }

            private class ReplaceExpressionVisitor : ExpressionVisitor
            {
                private readonly Expression _oldValue;
                private readonly Expression _newValue;

                public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
                {
                    _oldValue = oldValue;
                    _newValue = newValue;
                }

                public override Expression Visit(Expression node)
                {
                    return node == _oldValue ? _newValue : base.Visit(node);
                }
            }
        }

    }

}
