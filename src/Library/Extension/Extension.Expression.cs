﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microservice.Library.Extension
{
    public static partial class Extension
    {
        #region 拓展BuildExtendSelectExpre方法

        /// <summary>
        /// 组合继承属性选择表达式树,无拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, TResult>> BuildExtendSelectExpre<TBase, TResult>(this Expression<Func<TBase, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,1个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, TResult>> BuildExtendSelectExpre<TBase, T1, TResult>(this Expression<Func<TBase, T1, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,2个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, TResult>> BuildExtendSelectExpre<TBase, T1, T2, TResult>(this Expression<Func<TBase, T1, T2, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,3个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, TResult>(this Expression<Func<TBase, T1, T2, T3, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,4个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,5个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,6个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,7个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,8个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="T8">拓展类型8</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,9个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="T8">拓展类型8</typeparam>
        /// <typeparam name="T9">拓展类型9</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression) where TResult : TBase
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(expression);
        }

        #endregion

        #region 拓展And和Or方法

        /// <summary>
        /// 连接表达式与运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新的表达式</param>
        /// <param name="type">连接类型</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Concat<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another, ConcatType type)
        {
            //创建新参数
            var newParameter = Expression.Parameter(typeof(T), "parameter");

            var parameterReplacer = new ParameterReplaceVisitor(newParameter);
            var left = parameterReplacer.Visit(one.Body);
            var right = parameterReplacer.Visit(another.Body);
            BinaryExpression body;

            switch (type)
            {
                case ConcatType.And:
                    body = Expression.And(left, right);
                    break;
                case ConcatType.AndAlso:
                    body = Expression.AndAlso(left, right);
                    break;
                case ConcatType.Or:
                    body = Expression.Or(left, right);
                    break;
                case ConcatType.OrElse:
                    body = Expression.OrElse(left, right);
                    break;
                default:
                    throw new ApplicationException($"未知的连接类型 {type}");
            }

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        /// <summary>
        /// 连接表达式与运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新的表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            return one.Concat(another, ConcatType.And);
        }

        /// <summary>
        /// 连接表达式与运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新的表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            return one.Concat(another, ConcatType.AndAlso);
        }

        /// <summary>
        /// 连接表达式或运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            return one.Concat(another, ConcatType.Or);
        }

        /// <summary>
        /// 连接表达式或运算
        /// </summary>
        /// <typeparam name="T">参数</typeparam>
        /// <param name="one">原表达式</param>
        /// <param name="another">新表达式</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            return one.Concat(another, ConcatType.OrElse);
        }

        #endregion

        #region 拓展Expression的Invoke方法
        public static IEnumerable<TSource> NewIfEmpty<TSource>(this IEnumerable<TSource> source) where TSource : new()
        {
            return source.Select(o =>
            {
                if (o == null)
                    return new TSource();
                return o;
            });
        }

        public static TResult Invoke<TResult>(this Expression<Func<TResult>> expression)
        {
            return expression.Compile().Invoke();
        }

        public static TResult Invoke<T1, TResult>(this Expression<Func<T1, TResult>> expression, T1 arg1)
        {
            return expression.Compile().Invoke(arg1);
        }

        public static TResult Invoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expression, T1 arg1, T2 arg2)
        {
            return expression.Compile().Invoke(arg1, arg2);
        }

        public static TResult Invoke<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expression, T1 arg1, T2 arg2, T3 arg3)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3);
        }

        public static TResult Invoke<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, TResult>(this Expression<Func<T1, T2, T3, T4, T5, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static TResult Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> expression, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return expression.Compile().Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        #endregion

        /// <summary>
        /// 获取表达式中的固定值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static object GetConstantValue(this Expression expression)
        {
            var visitor = new GetConstantValueVisitor();
            visitor.Visit(expression);
            return visitor.ConstantValue;
        }

        public static object GetMemberValue(this Expression expression)
        {
            var visitor = new GetMemberValueVisitor();
            visitor.Visit(expression);
            return visitor.Value;
        }

        #region 私有成员

        private static Expression<TDelegate> GetExtendSelectExpre<TBase, TResult, TDelegate>(Expression<TDelegate> expression)
        {
            NewExpression newBody = Expression.New(typeof(TResult));
            MemberInitExpression oldExpression = (MemberInitExpression)expression.Body;

            ParameterExpression[] oldParamters = expression.Parameters.ToArray();
            List<string> existsProperties = new List<string>();
            oldExpression.Bindings.ForEach(aBinding =>
            {
                existsProperties.Add(aBinding.Member.Name);
            });

            List<MemberBinding> newBindings = new List<MemberBinding>();
            typeof(TResult).GetProperties().Where(x => !existsProperties.Contains(x.Name)).ForEach(aProperty =>
            {
                if (typeof(TBase).GetMembers().Any(x => x.Name == aProperty.Name))
                {
                    MemberInfo newMember = typeof(TBase).GetMember(aProperty.Name)[0];
                    MemberBinding newMemberBinding = Expression.Bind(newMember, Expression.Property(oldParamters[0], aProperty.Name));
                    newBindings.Add(newMemberBinding);
                }
            });

            newBindings.AddRange(oldExpression.Bindings);

            var body = Expression.MemberInit(newBody, newBindings.ToArray());
            var resExpression = Expression.Lambda<TDelegate>(body, oldParamters);

            return resExpression;
        }

        #endregion
    }

    /// <summary>
    /// 继承ExpressionVisitor类，实现参数替换统一
    /// </summary>
    class ParameterReplaceVisitor : ExpressionVisitor
    {
        public ParameterReplaceVisitor(ParameterExpression paramExpr)
        {
            _parameter = paramExpr;
        }

        //新的表达式参数
        private ParameterExpression _parameter { get; set; }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (p.Type == _parameter.Type)
                return _parameter;
            else
                return p;
        }
    }

    class GetConstantValueVisitor : ExpressionVisitor
    {
        public object ConstantValue { get; set; }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            ConstantValue = node.Value;

            return base.VisitConstant(node);
        }
    }

    class GetMemberValueVisitor : ExpressionVisitor
    {
        public object Value { get; set; }
        protected override Expression VisitMember(MemberExpression node)
        {
            //静态成员
            if (node.Expression == null)
            {
                if (node.Member.MemberType == MemberTypes.Property)
                    Value = node.Type.GetProperty(node.Member.Name, BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                else if (node.Member.MemberType == MemberTypes.Field)
                    Value = node.Type.GetField(node.Member.Name, BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
            }
            else//对象成员
            {
                var obj = (node.Expression as ConstantExpression).Value;
                if (obj.ContainsField(node.Member.Name))
                    Value = obj.GetGetFieldValue(node.Member.Name);
                else if (obj.ContainsProperty(node.Member.Name))
                    Value = obj.GetPropertyValue(node.Member.Name);
            }

            return base.VisitMember(node);
        }
    }

    /// <summary>
    /// 连接类型
    /// </summary>
    public enum ConcatType
    {
        And,
        AndAlso,
        Or,
        OrElse
    }
}