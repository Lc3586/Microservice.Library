using Library.Extension;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Log
{
    public class NLogHelper
    {
        public NLogHelper()
        {

        }

        public NLogHelper(string loggerName)
        {
            LoggerName = loggerName;
        }

        public Logger GetLogger()
        {
            return LoggerName.IsNullOrEmpty() ? Logger : LogManager.GetLogger(LoggerName);
        }

        private string LoggerName { get; set; }

        private static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 关闭日志
        /// </summary>
        public static bool Off = false;

        #region 普通

        public void I(string msg)
        {
            if (Off) return;

            GetLogger().Info(msg);
        }

        public void I(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Info(Title + "\r\n" + msg);
        }

        public void I(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Info(ex.GetExceptionAllMsg());
        }

        public void I(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Info(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 调试

        public void D(string msg)
        {
            if (Off) return;

            GetLogger().Debug(msg);
        }

        public void D(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Debug(Title + "\r\n" + msg);
        }

        public void D(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Debug(ex.GetExceptionAllMsg());
        }

        public void D(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Debug(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 追踪

        public void T(string msg)
        {
            if (Off) return;

            GetLogger().Trace(msg);
        }

        public void T(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Trace(Title + "\r\n" + msg);
        }

        public void T(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Trace(ex.GetExceptionAllMsg());
        }

        public void T(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Trace(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 警告

        public void W(string msg)
        {
            if (Off) return;

            GetLogger().Warn(msg);
        }

        public void W(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Warn(Title + "\r\n" + msg);
        }

        public void W(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Warn(ex.GetExceptionAllMsg());
        }

        public void W(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Warn(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 错误

        public void E(string msg)
        {
            if (Off) return;

            GetLogger().Error(msg);
        }

        public void E(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Error(Title + "\r\n" + msg);
        }

        public void E(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Error(ex.GetExceptionAllMsg());
        }

        public void E(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Error(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 灾难

        public void F(string msg)
        {
            if (Off) return;

            GetLogger().Fatal(msg);
        }

        public void F(string Title, string msg)
        {
            if (Off) return;

            GetLogger().Fatal(Title + "\r\n" + msg);
        }

        public void F(System.Exception ex)
        {
            if (Off) return;

            GetLogger().Fatal(ex.GetExceptionAllMsg());
        }

        public void F(string Title, System.Exception ex)
        {
            if (Off) return;

            GetLogger().Fatal(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

    }
}
