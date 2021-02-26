using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Library.ConsoleTool
{
    /// <summary>
    /// 锁
    /// </summary>
    internal static class Lock
    {
        public static object LockObject = new object();
    }
}
