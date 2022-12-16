using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Library.ConsoleTool
{
    /// <summary>
    /// 拓展方法
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// 控制台延时输出
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="color">颜色(默认白色)</param>
        /// <param name="tag">标签</param>
        /// <param name="line">是否整行(默认true)</param>
        /// <param name="blankline">空行数</param>
        /// <param name="delay">延时(-1为随机延时(20到300毫秒之间))</param>
        /// <returns></returns>
        public static async Task ConsoleWriteAsync(this object data, int delay = -1, ConsoleColor color = ConsoleColor.White, string tag = null, bool line = true, int blankline = 0)
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
            if (delay <= 0 && delay != -1)
                Console.Write(data);
            else
            {
                Random random = delay == -1 ? new Random() : null;
                foreach (var item in data.ToString())
                {
                    Console.Write(item);
                    await Task.Delay(delay == -1 ? random.Next(20, 300) : delay);
                }
            }
            Console.ForegroundColor = _color;
            while (blankline > 0)
            {
                Console.Write("\n");
                blankline--;
            }
        }

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
        /// <param name="aotuConfirm">自动确认（输入内容达到最大长度时无需再按回车键确认）</param>
        /// <param name="hiddenSymbol">输入时用来隐藏内容的字符</param>
        /// <returns></returns>
        public static string ReadInput(string tps = null, bool line = true, string defaultValue = null, int maxLength = -1, bool aotuConfirm = false, char? hiddenSymbol = null)
        {
            if (line)
                Console.Write("\n");
            if (!string.IsNullOrEmpty(tps))
                Console.Write(tps);
            var input = string.Empty;
            int currentIndex = defaultValue == null ? -1 : defaultValue.Length - 1;
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
                    if (currentIndex == -1)
                        continue;

                    var length = input.GetCharLength(currentIndex);
                    var lastInput = input.SubstringZero(currentIndex + 1, input.Length - 1 - currentIndex);
                    input = $"{input.SubstringZero(0, currentIndex)}{lastInput}";
                    Console.Write($"{new string('\u0008', length)}{new string(' ', length)}{new string('\u0008', length)}{(hiddenSymbol.HasValue ? new string(hiddenSymbol.Value, lastInput.Length) : lastInput)}{new string(' ', length)}{new string('\u0008', GetCharLength(lastInput) + length)}");
                    currentIndex--;
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (currentIndex == -1)
                        continue;

                    var length = input.GetCharLength(currentIndex);
                    Console.Write(new string('\u0008', length));

                    currentIndex--;
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (currentIndex + 1 >= input.Length)
                        continue;

                    var length = input.GetCharLength(currentIndex + 1);
                    Console.SetCursorPosition(Console.CursorLeft + length, Console.CursorTop);

                    currentIndex++;
                }
                else if (key.Key == ConsoleKey.Delete)
                {
                    if (currentIndex == -1 || currentIndex + 1 >= input.Length)
                        continue;

                    var length = input.GetCharLength(currentIndex + 1);
                    var lastInput = input.SubstringZero(currentIndex + 2, input.Length - 2 - currentIndex);
                    input = $"{input.SubstringZero(0, currentIndex + 1)}{lastInput}";
                    Console.Write($"{(hiddenSymbol.HasValue ? new string(hiddenSymbol.Value, lastInput.Length) : lastInput)}{new string(' ', length)}{new string('\u0008', GetCharLength(lastInput) + length)}");
                }
                else if (key.Key == ConsoleKey.Enter)
                    break;
                else
                {
                    if (maxLength != -1 && currentIndex + 1 >= maxLength)
                        continue;

                    var lastInput = input.SubstringZero(currentIndex + 1, input.Length - 1 - currentIndex);
                    input = $"{input.SubstringZero(0, currentIndex + 1)}{key.KeyChar}{lastInput}";
                    Console.Write($"{(hiddenSymbol.HasValue ? hiddenSymbol.Value : key.KeyChar)}{(hiddenSymbol.HasValue ? new string(hiddenSymbol.Value, lastInput.Length) : lastInput)}{new string('\u0008', GetCharLength(lastInput))}");
                    currentIndex++;
                }
            } while (maxLength == -1 || !aotuConfirm || currentIndex + 1 < maxLength);
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
        /// <param name="aotuConfirm">自动确认（输入内容达到最大长度时无需再按回车键确认）</param>
        /// <param name="hiddenSymbol">输入时用来隐藏内容的字符</param>
        /// <returns></returns>
        public static string ReadPassword(string tps = null, bool line = true, int maxLength = -1, bool aotuConfirm = false, char hiddenSymbol = '*')
        {
            return ReadInput(tps, line, null, maxLength, aotuConfirm, hiddenSymbol);
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string SubstringZero(this string input, int startIndex, int length)
        {
            return length == 0 || startIndex >= input.Length ? string.Empty : input.Substring(startIndex, length);
        }

        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public static int GetCharLength(this string input, int index)
        {
            return Encoding.ASCII.GetBytes(new char[] { input[index] })[0] == 63 ? 2 : 1;
        }

        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <returns></returns>
        public static int GetCharLength(this string input)
        {
            return Encoding.ASCII.GetBytes(input).Sum(c => c == 63 ? 2 : 1);
        }
    }
}
