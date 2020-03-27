using System.Diagnostics;

namespace Library.HardwareInfo
{
    /// <summary>
    /// https://blog.csdn.net/weixin_43145361/article/details/90939154
    /// </summary>
    public static class CPU
    {
        public static double[] UsageInfo = null;

        private static PerformanceCounter[] counters = null;

        public static double[] CPUUsageInfo()
        {
            if (counters == null)
            {
                counters = new PerformanceCounter[System.Environment.ProcessorCount];
                for (int i = 0; i < counters.Length; i++)
                {
                    counters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                }
            }
            UsageInfo = new double[counters.Length];
            for (int i = 0; i < counters.Length; i++)
                UsageInfo[i] = counters[i].NextValue();
            return UsageInfo;
        }
    }
}
