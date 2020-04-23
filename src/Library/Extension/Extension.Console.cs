using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Extension
{
    public static partial class Extension
    {
        /// <summary>
        /// 控制台读取用户输入的密码
        /// </summary>
        /// <param name="tps">提示</param>
        /// <param name="line">是否整行</param>
        /// <param name="maxLength">最大长度（默认不限制）</param>
        /// <param name="hiddenSymbol">输入时显示的字符</param>
        /// <returns></returns>
        public static string ReadPassword(string tps = null, bool line = true, int maxLength = -1, char hiddenSymbol = '*')
        {
            if (line)
                Console.Write("\n");
            if (!tps.IsNullOrEmpty())
                Console.Write(tps);
            var password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length == 0)
                        continue;
                    password = password.Substring(0, password.Length - 1);
                    Console.Write('\u0008');
                    Console.Write(' ');
                    Console.Write('\u0008');
                }
                else if (key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar.ToString();
                    Console.Write(hiddenSymbol);
                }
                else
                    break;
            } while (maxLength == -1 || password.Length < maxLength);
            return password;
        }
    }
}
