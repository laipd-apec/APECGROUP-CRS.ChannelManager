using Core.Helpper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace CRS.ChannelManager.Library.Helpper
{
    public class JsonExpressionParser
    {
        public const string StringStr = "string";
        public const string BooleanStr = "boolean";
        public const string Number = "number";
        public const string DateTime = "datetime";
        public const string Date = "date";
        public const string Int32 = "int32";
        public const string NotSet = "null";

        public const string And = "and";
        public const string Or = "or";

        public const string Any = "any";
        public const string First = "first";
        public const string Last = "last";

        public const string In = "in";
        public const string ContainArray = "containarray";
        public const string NotIn = "notin";
        public const string Equal = "=";
        public const string EqualCountArray = "equalcountarray";
        public const string NotEqual = "!=";
        public const string GreaterThan = ">";
        public const string LessThan = "<";
        public const string GreaterThanOrEqual = ">=";
        public const string LessThanOrEqual = "<=";
        public const string StartWith = "s_";
        public const string EndWith = "_s";
        public const string Contains = "like";
        public const string NotContains = "notlike";

        private const char Separator = '.';


        private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(
                        BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Contains)
                            && m.GetParameters().Length == 2);
        private readonly MethodInfo MethodIntersect = typeof(Enumerable).GetMethods(
                        BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Intersect)
                            && m.GetParameters().Length == 2);
        private readonly MethodInfo MethodCount = typeof(Enumerable).GetMethods(
                        BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.Count)
                            && m.GetParameters().Length == 1);
        private readonly MethodInfo Method2Array = typeof(Enumerable).GetMethods(
                        BindingFlags.Static | BindingFlags.Public)
                        .Single(m => m.Name == nameof(Enumerable.ToArray)
                            && m.GetParameters().Length == 1);

        public class EntityBaseComparer<T> : IEqualityComparer<T> where T : Base.EntityBase
        {
            public bool Equals(string x, string y)
            {
                return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            }

            public bool Equals(T? x, T? y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(string obj)
            {
                return obj?.GetHashCode() ?? 0;
            }

            public int GetHashCode([DisallowNull] T obj)
            {
                return obj?.GetHashCode() ?? 0;
            }
        }

        private static Expression getPropertyExpresion(Expression expression, string[] propertiesTree, int count, ref int currentLevel)
        {
            if (currentLevel < count)
            {
                expression = Expression.Property(expression, propertiesTree[currentLevel]);
                currentLevel++;
                if (expression.Type.GetInterfaces().Contains(typeof(System.Collections.IEnumerable)))
                {
                    return expression;
                }
                return getPropertyExpresion(expression, propertiesTree, count, ref currentLevel);
            }
            return expression;
        }


        private delegate Expression Binder(Expression left, Expression right);

        private object ToObjectValue(JsonElement value, string type)
        {
            object val;
            switch (type)
            {
                case Date:
                case DateTime:
                    val = value.GetDateTime();
                    break;
                case BooleanStr:
                    val = value.GetString() == "true" ? true : false;
                    break;
                case Number:
                    val = value.GetDecimal();
                    break;
                case Int32:
                    val = value.GetInt32();
                    break;
                case NotSet:
                    val = null;
                    break;
                default:
                    val = (object)value.GetString();
                    break;
            }
            return val;
        }

        private Expression ParseTree<T>(
            JsonElement condition,
            ParameterExpression parm)
        {
            try
            {
                Expression left = null;
                var gate = condition.GetProperty(nameof(condition)).GetString().ToLower();

                JsonElement rules = condition.GetProperty(nameof(rules));

                Binder binder = gate == And ? (Binder)Expression.And : Expression.Or;

                Expression bind(Expression left, Expression right) =>
                    left == null ? right : binder(left, right);

                foreach (var rule in rules.EnumerateArray())
                {
                    if (rule.TryGetProperty(nameof(condition), out JsonElement check))
                    {
                        var right = ParseTree<T>(rule, parm);
                        left = bind(left, right);
                        continue;
                    }
                    string @operator;
                    JsonElement operatorProperty = rule.GetProperty(nameof(@operator));
                    @operator = operatorProperty.ValueKind == JsonValueKind.String ? operatorProperty.GetString() : string.Empty;
                    string type = rule.GetProperty(nameof(type)).GetString();
                    string field = rule.GetProperty(nameof(field)).GetString();

                    JsonElement value = rule.GetProperty(nameof(value));
                    string[] fields = field.Split(Separator);
                    int currentLevel = 0;
                    Expression property = field.Contains(Separator) &&
                        fields.Length > 1 ?
                        getPropertyExpresion(parm, fields, fields.Length, ref currentLevel) :
                        Expression.Property(parm, field);
                    bool isObjectComparer = false;
                    if (@operator == Any || (isObjectComparer = operatorProperty.ValueKind == JsonValueKind.Object))
                    {
                        string method = @operator, comparer = Equal;
                        if (isObjectComparer && operatorProperty.TryGetProperty("method", out JsonElement methodElement) &&
                            operatorProperty.TryGetProperty("comparer", out JsonElement comparerElement))
                        {
                            method = methodElement.GetString();
                            comparer = comparerElement.GetString();
                        }
                        switch (method)
                        {
                            case Any:
                                method = nameof(Any); break;
                            case First:
                                method = nameof(First); break;
                            case Last:
                                method = nameof(Last); break;
                            default:
                                break;
                        }
                        if (property.Type.GetInterfaces().Contains(typeof(System.Collections.IEnumerable)))
                        {
                            var lambdaParameter = Expression.Parameter(property.Type.GetGenericArguments().First(), "item");


                            var anyMethod = typeof(Enumerable).GetMethods()
                                .First(m => m.Name == method && m.GetParameters().Length == 2)
                                .MakeGenericMethod(property.Type.GetGenericArguments().First());

                            var anyLeftCompare = getPropertyExpresion(lambdaParameter, fields, fields.Length, ref currentLevel);
                            var convertedValue = Expression.Constant(ToObjectValue(value, type));

                            var equalityExpression = ProduceExpression(comparer, anyLeftCompare, convertedValue, ToObjectValue(value, type));
                            // Expression.Equal(anyLeftCompare, convertedValue);

                            var lambda = Expression.Lambda(equalityExpression, lambdaParameter);

                            var anyExpression = Expression.Call(anyMethod, property, lambda);
                            left = bind(left, anyExpression);
                        }
                    }
                    else if (@operator == In)
                    {
                        var contains = MethodContains.MakeGenericMethod(typeof(string));
                        object val = value.EnumerateArray().Select(e => e.GetString()).ToList();
                        var right = Expression.Call(contains,Expression.Constant(val),property);
                        left = bind(left, right);
                    }
                    else if (@operator == NotIn)
                    {
                        var contains = MethodContains.MakeGenericMethod(typeof(string));
                        object val = value.EnumerateArray().Select(e => e.GetString()).ToList();
                        var right = Expression.Not(Expression.Call(contains,Expression.Constant(val),property));
                        left = bind(left, right);
                    }
                    else if (@operator == ContainArray)
                    {
                        //var intersect = MethodIntersect.MakeGenericMethod(typeof(string));


                        //var val = BuildSplitExpression(strValue);

                        //var str2ArrayProperty = BuildSplitExpression(property);

                        //var intersectArray = Expression.Call(
                        //    intersect,
                        //    str2ArrayProperty,
                        //    val);
                        //// Call Enumerable.Count method with the property as argument
                        //var countMethod = MethodCount.MakeGenericMethod(typeof(string));
                        //var intersectEnumerable = Expression.Call(Method2Array.MakeGenericMethod(typeof(string)), intersectArray);
                        //var countCall = Expression.Call(countMethod, intersectEnumerable);
                        string strValue = (string)ToObjectValue(value, type);
                        int count = strValue.Split(",").Count();
                        var dbContainArray = typeof(SqlFunctionHelper).GetMethod("ContainArray", BindingFlags.Static | BindingFlags.Public);
                        property = Expression.Call(null, dbContainArray, property, Expression.Constant(strValue));
                        var right = Expression.Equal(property, Expression.Constant(0));
                        left = bind(left, right);
                    }
                    else
                    {
                        object val = ToObjectValue(value, type);
                        Expression toCompare;
                        if (type == Date)
                        {
                            var date2Number = typeof(DateHelper).GetMethod("DateToNumber", BindingFlags.Static | BindingFlags.Public);
                            var dbDate2Number = typeof(SqlFunctionHelper).GetMethod("DateToNumber", BindingFlags.Static | BindingFlags.Public);
                            toCompare = Expression.Call(null, date2Number, Expression.Constant(val));
                            property = Expression.Convert(property, (dbDate2Number.GetParameters()[0]).ParameterType);
                            property = Expression.Call(null, dbDate2Number, property);
                        }
                        else if(@operator == EqualCountArray)
                        {
                            toCompare = Expression.Convert(Expression.Constant(val), typeof(Int32));
                        }
                        else
                        {
                            toCompare = Expression.Convert(Expression.Constant(val), property.Type);
                        }
                        dynamic right = ProduceExpression(@operator, property, toCompare, val);
                        left = bind(left, right);
                    }
                }

                return left;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Expression ProduceExpression(string @operator, Expression property, Expression toCompare, object val)
        {
            dynamic right = null;
            switch (@operator)
            {
                case GreaterThan:
                    right = Expression.GreaterThan(property, toCompare);
                    break;
                case LessThan:
                    right = Expression.LessThan(property, toCompare);
                    break;
                case GreaterThanOrEqual:
                    right = Expression.GreaterThanOrEqual(property, toCompare);
                    break;
                case LessThanOrEqual:
                    right = Expression.LessThanOrEqual(property, toCompare);
                    break;
                case NotEqual:
                    right = Expression.NotEqual(property, toCompare);
                    break;
                case Contains:
                    // var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var ftsConvertMethod = typeof(StringHelper).GetMethod("ConvertToFts", BindingFlags.Static | BindingFlags.Public);
                    var dbFtsConvertMethod = typeof(SqlFunctionHelper).GetMethod("ConvertToFts", BindingFlags.Static | BindingFlags.Public);
                    string textSearch = val.ToString();
                    if (string.IsNullOrEmpty(textSearch))
                    {
                        right = Expression.Call(property, containsMethod, Expression.Constant(textSearch, typeof(string)));
                        break;
                    }
                    var searchExpression = Expression.Call(null, ftsConvertMethod, Expression.Constant(textSearch));
                    var xExpression = Expression.Call(null, dbFtsConvertMethod, property);
                    right = Expression.Call(xExpression, containsMethod, searchExpression);
                    break;
                case NotContains:
                    // var stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                    var toLowerMethodNotLike = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var containsMethodNotLike = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    var ftsConvertMethodNotLike = typeof(StringHelper).GetMethod("ConvertToFts", BindingFlags.Static | BindingFlags.Public);
                    var dbFtsConvertMethodNotLike = typeof(SqlFunctionHelper).GetMethod("ConvertToFts", BindingFlags.Static | BindingFlags.Public);
                    string textSearchNotLike = val.ToString();
                    if (string.IsNullOrEmpty(textSearchNotLike))
                    {
                        right = Expression.Call(property, containsMethodNotLike, Expression.Constant(textSearchNotLike, typeof(string)));
                        break;
                    }
                    var searchExpressionNotLike = Expression.Call(null, ftsConvertMethodNotLike, Expression.Constant(textSearchNotLike));
                    var xExpressionNotLike = Expression.Call(null, dbFtsConvertMethodNotLike, property);
                    right = Expression.Call(xExpressionNotLike, containsMethodNotLike, searchExpressionNotLike);
                    right = Expression.Not(right);
                    break;
                case StartWith:
                    var startWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });
                    right = Expression.Call(property, startWithMethod, Expression.Constant(val, typeof(string)));
                    break;
                case EndWith:
                    var endWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });
                    right = Expression.Call(property, endWithMethod, Expression.Constant(val, typeof(string)));
                    break;
                case EqualCountArray:
                    property = Expression.Call(MethodCount.MakeGenericMethod(typeof(object)), property);
                    goto default;
                default:
                    right = Expression.Equal(property, toCompare);
                    break;
            }
            return right;
        }

        public Expression<Func<T, bool>> ParseExpressionOf<T>(JsonDocument doc)
        {
            var itemExpression = Expression.Parameter(typeof(T));
            var conditions = ParseTree<T>(doc.RootElement, itemExpression);
            if (conditions.CanReduce)
            {
                conditions = conditions.ReduceAndCheck();
            }

            var query = Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
            return query;
        }

        public Expression<Func<T, bool>> ParsePredicateOf<T>(JsonDocument doc)
        {
            var query = ParseExpressionOf<T>(doc);
            return query;
        }

        private static Expression BuildSplitExpression(string input)
        {
            // Define parameter for the input string
            var inputParam = Expression.Constant(input);

            // Call string.Split method with the input string as argument
            var splitMethod = typeof(string).GetMethod("Split", new[] { typeof(char[]) });
            var separator = Expression.Constant(new[] { ',' }); // Split by space
            var splitCall = Expression.Call(inputParam, splitMethod, separator);

            return splitCall;
        }

        private static Expression BuildSplitExpression(Expression input)
        {
            // Call string.Split method with the input string as argument
            var splitMethod = typeof(string).GetMethod("Split", new[] { typeof(char[]) });
            var separator = Expression.Constant(new[] { ',' }); // Split by space
            var splitCall = Expression.Call(input, splitMethod, separator);

            // Create lambda expression
            return splitCall;
        }

        private static Expression BuildIntersectExpression(int[] array1, Expression array2Param)
        {
            // Define parameters for the arrays
            var array1Param = Expression.Constant(array1);

            // Call Enumerable.Intersect method with the arrays as arguments
            var intersectMethod = typeof(Enumerable).GetMethod("Intersect", new[] { typeof(IEnumerable<int>), typeof(IEnumerable<int>) });
            var intersectCall = Expression.Call(intersectMethod, array1Param, array2Param);

            return intersectCall;
        }
    }
}
