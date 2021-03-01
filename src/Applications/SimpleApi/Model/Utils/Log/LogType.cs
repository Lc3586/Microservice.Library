﻿namespace Model.Utils.Log
{
    /// <summary>
    /// 日志类型
    /// </summary>
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public struct LogType
#pragma warning restore CA1815 // Override equals and operator equals on value types
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>名称</returns>
        public static string GetName(byte value)
        {
            return value switch
            {
                系统跟踪 => "系统跟踪",
                调试日志 => "调试日志",
                警告信息 => "警告信息",
                系统异常 => "系统异常",
                _ => null,
            };
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>值</returns>
        public static byte? GetValue(string name)
        {
            return name switch
            {
                "系统跟踪" => 系统跟踪,
                "调试日志" => 调试日志,
                "警告信息" => 警告信息,
                "系统异常" => 系统异常,
                _ => null,
            };
        }

        public const byte 系统跟踪 = 0x00;

        public const byte 调试日志 = 0x01;

        public const byte 警告信息 = 0x02;

        public const byte 系统异常 = 0x03;
    }
}
