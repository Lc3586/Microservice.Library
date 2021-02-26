using System;
using System.Linq.Expressions;

namespace Microservice.Library.LinqTool
{
    /// <summary>
    /// 创建条件
    /// </summary>
    public static class Where
    {
        /// <summary>
        /// 创建初始条件为True的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return x => true;
        }

        /// <summary>
        /// 创建初始条件为True的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, bool> TrueFunc<T>()
        {
            return x => true;
        }

        /// <summary>
        /// 创建初始条件为False的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return x => false;
        }

        /// <summary>
        /// 创建初始条件为False的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Func<T, bool> FalseFunc<T>()
        {
            return x => false;
        }
    }
}
