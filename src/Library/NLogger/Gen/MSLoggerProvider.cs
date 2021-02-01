using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;

namespace Library.NLogger.Gen
{
    /// <summary>
    /// MS日志组件构造器
    /// </summary>
    public class MSLoggerProvider : ILoggerProvider
    {
        public MSLoggerProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        readonly IServiceProvider ServiceProvider;

        public ILogger CreateLogger(string categoryName)
        {
            return new MSLogger(ServiceProvider, categoryName);
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
