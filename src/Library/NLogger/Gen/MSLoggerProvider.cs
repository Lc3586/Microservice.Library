using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;

namespace Library.NLogger.Gen
{
    /// <summary>
    /// MS日志组件构造器
    /// </summary>
    /// <remarks>必须先注册<see cref="Library.NLogger.Gen.INLoggerProvider"/>, 使用<see cref="Microsoft.Extensions.DependencyInjection.NLoggerServiceCollectionExtensions"/>方法</remarks>
    public class MSLoggerProvider : ILoggerProvider
    {
        public MSLoggerProvider(INLoggerProvider nLoggerProvider)
        {
            NLoggerProvider = nLoggerProvider;
        }

        readonly INLoggerProvider NLoggerProvider;

        public ILogger CreateLogger(string categoryName)
        {
            return new MSLogger(NLoggerProvider, categoryName);
        }

        bool isDisposed;

        IntPtr nativeResource = Marshal.AllocHGlobal(100);

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }

            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~MSLoggerProvider()
        {
            Dispose(false);
        }
    }
}
