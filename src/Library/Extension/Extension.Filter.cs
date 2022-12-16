using Microservice.Library.Extension.Model;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Microservice.Library.Extension
{
    /// <summary>
    /// 筛选扩展方法
    /// </summary>
    public static partial class Extension
    {
        /// <summary>
        /// 获取linq表达式
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="filter">筛选</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetLinqWhere<T>(this Filter filter)
        {
            Expression left = Expression.Constant(1);
            BinaryExpression binaryExpression = Expression.Equal(left, left);
            Type type = typeof(T);
            ParameterExpression param = Expression.Parameter(type, "p");

            if (filter == null || (filter.Param == null && filter.Group == null))
                goto end;

            if (filter.Group != null)
            {

            }

            if (filter.Param != null)
            {

            }



            end:
            return Expression.Lambda<Func<T, bool>>(binaryExpression, param);
        }

        private static Expression GetLambda<T>(this FilterParam filterParam, Type type, ParameterExpression param)
        {
            PropertyInfo prop = type.GetProperty(filterParam.Field);
            if (prop == null)
                return null;
            var propType = param.Type.GetRuntimeProperty(prop.Name).PropertyType;
            filterParam.Value = filterParam.Value.ChangeType_ByConvert(propType);


            Expression lambda = null;
            Expression left = Expression.Property(param, typeof(T).GetProperty(prop.Name));
            Expression right = propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>) ? (Expression)Expression.Convert(Expression.Constant(filterParam.Value), propType) : Expression.Constant(filterParam.Value);

            switch (filterParam.Compare)
            {
                case CompareType.IN:
                    lambda = Expression.Call(left, typeof(string).GetMethod("Contains"), right);
                    break;
                case CompareType.BIN:
                    lambda = Expression.Call(right, typeof(string).GetMethod("Contains"), left);
                    break;
                case CompareType.SIN:
                    break;
                case CompareType.NSIN:
                    break;
                case CompareType.EQ:
                    lambda = Expression.Equal(left, right);
                    break;
                case CompareType.NE:
                    lambda = Expression.NotEqual(left, right);
                    break;
                case CompareType.LE:
                    lambda = Expression.LessThanOrEqual(left, right);
                    break;
                case CompareType.LT:
                    lambda = Expression.LessThan(left, right);
                    break;
                case CompareType.GE:
                    lambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case CompareType.GT:
                    lambda = Expression.GreaterThan(left, right);
                    break;
                default:
                    lambda = Expression.Equal(left, right);
                    break;
            }
            Expression left1 = Expression.Constant(1);
            return lambda;
        }
    }
}
