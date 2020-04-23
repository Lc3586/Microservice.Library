using Library.Extension;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Log
{
    public static class NLogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 关闭日志
        /// </summary>
        public static bool Off = false;

        #region 常规

        public static void I(string msg)
        {
            if (Off) return;

            Logger.Info(msg);
        }

        public static void I(string Title, string msg)
        {
            if (Off) return;

            Logger.Info(Title + "\r\n" + msg);
        }

        public static void I(System.Exception ex)
        {
            if (Off) return;

            Logger.Info(ex.GetExceptionAllMsg());
        }

        public static void I(string Title, System.Exception ex)
        {
            if (Off) return;

            Logger.Info(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 调试

        public static void D(string msg)
        {
            if (Off) return;

            Logger.Debug(msg);
        }

        public static void D(string Title, string msg)
        {
            if (Off) return;

            Logger.Debug(Title + "\r\n" + msg);
        }

        public static void D(System.Exception ex)
        {
            if (Off) return;

            Logger.Debug(ex.GetExceptionAllMsg());
        }

        public static void D(string Title, System.Exception ex)
        {
            if (Off) return;

            Logger.Debug(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 警告

        public static void W(string msg)
        {
            if (Off) return;

            Logger.Warn(msg);
        }

        public static void W(string Title, string msg)
        {
            if (Off) return;

            Logger.Warn(Title + "\r\n" + msg);
        }

        public static void W(System.Exception ex)
        {
            if (Off) return;

            Logger.Warn(ex.GetExceptionAllMsg());
        }

        public static void W(string Title, System.Exception ex)
        {
            if (Off) return;

            Logger.Warn(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 错误

        public static void E(string msg)
        {
            if (Off) return;

            Logger.Error(msg);
        }

        public static void E(string Title, string msg)
        {
            if (Off) return;

            Logger.Error(Title + "\r\n" + msg);
        }

        public static void E(System.Exception ex)
        {
            if (Off) return;

            Logger.Error(ex.GetExceptionAllMsg());
        }

        public static void E(string Title, System.Exception ex)
        {
            if (Off) return;

            Logger.Error(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

        #region 灾难

        public static void F(string msg)
        {
            if (Off) return;

            Logger.Fatal(msg);
        }

        public static void F(string Title, string msg)
        {
            if (Off) return;

            Logger.Fatal(Title + "\r\n" + msg);
        }

        public static void F(System.Exception ex)
        {
            if (Off) return;

            Logger.Fatal(ex.GetExceptionAllMsg());
        }

        public static void F(string Title, System.Exception ex)
        {
            if (Off) return;

            Logger.Fatal(Title + "\r\n" + ex.GetExceptionAllMsg());
        }

        #endregion

    }
}
