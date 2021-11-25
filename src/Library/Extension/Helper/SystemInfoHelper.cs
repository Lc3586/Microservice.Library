using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Microservice.Library.Extension.Helper
{
    /// <summary>
    /// 系统信息帮助类
    /// </summary>
    public static class SystemInfoHelper
    {
        /// <summary>
        /// 当前操作系统平台
        /// </summary>
        public static OSPlatform CurrentOS
        {
            get
            {
                if (_CurrentOS != null)
                    return _CurrentOS.Value;

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    _CurrentOS = OSPlatform.Windows;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    _CurrentOS = OSPlatform.Linux;
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    _CurrentOS = OSPlatform.OSX;
                else
                    throw new ApplicationException("无法获取当前的操作系统平台.");

                return _CurrentOS.Value;
            }
        }

        /// <summary>
        /// 当前操作系统平台
        /// </summary>
        private static OSPlatform? _CurrentOS { get; set; }
    }
}
