using System.Diagnostics;

namespace Microservice.Library.HardwareInfo
{
    /// <summary>
    /// https://blog.csdn.net/weixin_43145361/article/details/90939154
    /// </summary>
    public static class CPU
    {
        private static PerformanceCounter[] Counters = null;
        private static object Lock = new { };

        public static double[] CPUUsageInfo()
        {
            lock (Lock)
            {
                if (Counters == null)
                {
                    Counters = new PerformanceCounter[System.Environment.ProcessorCount];
                    for (int i = 0; i < Counters.Length; i++)
                    {
                        Counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                    }
                }
            }

            var usageInfo = new double[Counters.Length];
            for (int i = 0; i < Counters.Length; i++)
                usageInfo[i] = Counters[i].NextValue();
            return usageInfo;
        }
    }
}
