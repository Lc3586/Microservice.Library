﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetExceptionAllMsg(this Exception ex)
        {
            var message = ex?.Message;
            if (ex.InnerException != null)
                message += $" {ex.InnerException.GetExceptionAllMsg()}";
            return message;
        }

        /// <summary>
        /// 获取异常位置
        /// </summary>
        /// <param name="e">异常</param>
        /// <returns></returns>
        public static string GetExceptionAddr(this Exception e)
        {
            StringBuilder excAddrBuilder = new StringBuilder();
            e?.StackTrace?.Split("\r\n".ToArray())?.ToList()?.ForEach(item =>
            {
                if (item.Contains("行号") || item.Contains("line"))
                    excAddrBuilder.Append($"    {item}\r\n");
            });

            string addr = excAddrBuilder.ToString();

            return string.IsNullOrEmpty(addr) ? "    无" : addr;
        }

        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="ex">捕捉的异常</param>
        /// <param name="level">内部异常层级</param>
        /// <returns></returns>
        public static string ExceptionToString(this Exception ex, int level)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"\r\n{level}层错误:  " +
                           $"\r\n\t消息:    " +
                           $"\r\n\t\t{ex?.Message}  " +
                           $"\r\n\t数据:    " +
                           $"\r\n\t\t{ex?.Data?.ToJson()}  " +
                           //$"\r\n\t实体验证错误:    " +
                           //$"\r\n\t\t{ex.GetEntityErrors()}  " +
                           $"\r\n\t位置:" +
                           $"\r\n\t\t{ex.GetExceptionAddr()}");
            if (ex.InnerException != null)
            {
                builder.Append(ex.InnerException.ExceptionToString(level + 1));
            }

            return builder.ToString();
        }

        ///// <summary>
        ///// 捕获实体异常
        ///// </summary>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //public static string GetEntityErrors(this Exception ex)
        //{
        //    if (ex.GetType().ToString() == "System.Data.Entity.Validation.DbEntityValidationException")
        //        return string.Join("\r\n\t\t", ((EntityException)ex).EntityValidationErrors.SelectMany(e => e.ValidationErrors).Select(e => e.ErrorMessage));
        //    else
        //        return "\r\n\t\t无";
        //}

        /// <summary>
        /// 获取异常消息
        /// </summary>
        /// <param name="ex">捕捉的异常</param>
        /// <returns></returns>
        public static string ExceptionToString(this Exception ex)
        {
            return ex.ExceptionToString(1);
        }
    }
}
