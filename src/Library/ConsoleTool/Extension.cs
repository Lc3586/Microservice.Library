using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ConsoleTool
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 控制台输出
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="color">颜色(默认白色)</param>
        /// <param name="tag">标签</param>
        /// <param name="line">是否整行(默认true)</param>
        /// <param name="blankline">空行数</param>
        /// <returns></returns>
        public static void ConsoleWrite(this object data, ConsoleColor color = ConsoleColor.White, string tag = null, bool line = true, int blankline = 0)
        {
            ConsoleColor _color = Console.ForegroundColor;
            if (line)
                Console.Write("\n");
            if (!string.IsNullOrEmpty(tag))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(tag + " : ");
            }
            Console.ForegroundColor = color;
            Console.Write(data);
            Console.ForegroundColor = _color;
            while (blankline > 0)
            {
                Console.Write("\n");
                blankline--;
            }
        }

        /// <summary>
        /// 控制台读取用户输入的信息
        /// </summary>
        /// <param name="tps">提示</param>
        /// <param name="line">是否整行</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="maxLength">最大长度（默认不限制）</param>
        /// <param name="hiddenSymbol">输入时用来隐藏内容的字符</param>
        /// <returns></returns>
        public static string ReadInput(string tps = null, bool line = true, string defaultValue = null, int maxLength = -1, char? hiddenSymbol = null)
        {
            if (line)
                Console.Write("\n");
            if (!string.IsNullOrEmpty(tps))
                Console.Write(tps);
            var input = "";
            ConsoleKeyInfo key;

            if (!string.IsNullOrEmpty(defaultValue))
            {
                input = defaultValue;
                foreach (var o in defaultValue)
                {
                    Console.Write(hiddenSymbol.HasValue ? hiddenSymbol.Value : o);
                }
            }

            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length == 0)
                        continue;
                    input = input.Substring(0, input.Length - 1);
                    Console.Write('\u0008');
                    Console.Write(' ');
                    Console.Write('\u0008');
                }
                else if (key.Key != ConsoleKey.Enter)
                {
                    input += key.KeyChar.ToString();
                    Console.Write(hiddenSymbol.HasValue ? hiddenSymbol.Value : key.KeyChar);
                }
                else
                    break;
            } while (maxLength == -1 || input.Length < maxLength);
            if (line)
                Console.Write("\n");
            return input;
        }

        /// <summary>
        /// 控制台读取用户输入的密码
        /// </summary>
        /// <param name="tps">提示</param>
        /// <param name="line">是否整行</param>
        /// <param name="maxLength">最大长度（默认不限制）</param>
        /// <param name="hiddenSymbol">输入时用来隐藏内容的字符</param>
        /// <returns></returns>
        public static string ReadPassword(string tps = null, bool line = true, int maxLength = -1, char hiddenSymbol = '*')
        {
            return ReadInput(tps, line, null, maxLength, hiddenSymbol);
        }
    }
}
