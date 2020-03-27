using System;
using System.Collections.Generic;
using System.Text;

namespace Library.ConsoleTool
{
    /***********************************************************************
    * 文 件 名：ProgressBar.cs
    * CopyRight(C) 2016-2020 中国XX工程技术有限公司
    * 文件编号：201603230001
    * 创 建 人：张华斌
    * 创建日期：2016-03-23
    * 修 改 人：
    * 修改日期：
    * 描    述：控制台进度条实现类
    * <!--LCTR 2019-08-06 新增允许同时存在多个进度条-->
    ***********************************************************************/
    public class ProgressBar
    {

        /// <summary>
        /// 光标的列位置。将从 0 开始从左到右对列进行编号。
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// 光标的行位置。从上到下，从 0 开始为行编号。
        /// </summary>
        public int[] Top { get; set; }

        /// <summary>
        /// 进度条宽度。
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 进度条当前值。
        /// </summary>
        public int[] Value { get; set; }
        /// <summary>
        /// 进度条当前信息。
        /// </summary>
        public string[] Msg { get; set; }
        /// <summary>
        /// 进度条类型
        /// </summary>
        public ProgressBarType progressBarType { get; set; }


        private ConsoleColor colorBack;
        private ConsoleColor colorFore;

        private object lockobj = null;

        public ProgressBar() : this(Console.CursorLeft, Console.CursorTop)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="count"></param>
        /// <param name="width"></param>
        /// <param name="ProgressBarType"></param>
        public ProgressBar(int left, int top, int count = 1, int width = 50, ProgressBarType ProgressBarType = ProgressBarType.Multicolor)
        {
            lockobj = new object();

            this.Left = left;
            this.Top = new int[count];
            this.Width = width;
            this.progressBarType = ProgressBarType;
            this.Value = new int[count];
            this.Msg = new string[count];

            for (int i = 0; i < count; i++)
            {
                Top[i] = top + i * 2;
                // 清空显示区域；
                Console.SetCursorPosition(Left, Top[i]);
                for (int j = left; ++j < Console.WindowWidth;) { Console.Write(" "); }
                if (this.progressBarType == ProgressBarType.Multicolor)
                {
                    // 绘制进度条背景； 
                    colorBack = Console.BackgroundColor;
                    Console.SetCursorPosition(Left, Top[i]);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    for (int j = 0; ++j <= width;) { Console.Write(" "); }
                    Console.BackgroundColor = colorBack;
                }
                else
                {
                    // 绘制进度条背景；    
                    Console.SetCursorPosition(left, Top[i]);
                    Console.Write("[");
                    Console.SetCursorPosition(left + width - 1, Top[i]);
                    Console.Write("]");
                }
            }
        }

        public int Dispaly(decimal value, int index = 0)
        {
            return Dispaly((int)value, null, index);
        }

        public int Dispaly(decimal value, string msg, int index = 0)
        {
            return Dispaly((int)value, msg, index);
        }

        public int Dispaly(int value, int index = 0)
        {
            return Dispaly(value, null, index);
        }

        public int Dispaly(int value, string msg, int index)
        {
            lock (lockobj)
            {
                if (this.Value[index] != value)
                {
                    this.Value[index] = value;

                    if (this.progressBarType == ProgressBarType.Multicolor)
                    {
                        // 保存背景色与前景色；
                        colorBack = Console.BackgroundColor;
                        colorFore = Console.ForegroundColor;
                        // 绘制进度条进度                
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.SetCursorPosition(this.Left, this.Top[index]);
                        Console.Write(new string(' ', (int)Math.Round(this.Value[index] / (100.0 / this.Width))));
                        Console.BackgroundColor = colorBack;
                    }
                    else
                    {
                        // 绘制进度条进度      
                        Console.SetCursorPosition(this.Left + 1, this.Top[index]);
                        Console.Write(new string('*', (int)Math.Round(this.Value[index] / (100.0 / (this.Width - 2)))));
                    }
                }

                if (this.Msg[index] == null || this.Msg[index] != msg)
                {
                    this.Msg[index] = string.IsNullOrWhiteSpace(msg) ? $"{this.Value[index]}%" : msg;

                    // 显示信息                      
                    if (this.progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(this.Left + this.Width + 1, this.Top[index]);
                    Console.Write(this.Msg[index]);
                    if (this.progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = colorFore;

                }

                //恢复光标到正常位置,恢复颜色
                for (int i = 0; i < this.Value.Length; i++)
                {
                    if (this.Value[i] != 100)
                        break;
                    if (i == this.Value.Length - 1)
                    {
                        Console.SetCursorPosition(0, this.Top[i] + 1);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// 进度条类型
        /// </summary>
        public enum ProgressBarType
        {
            /// <summary>
            /// 字符
            /// </summary>
            Character,
            /// <summary>
            /// 彩色
            /// </summary>
            Multicolor
        }
    }
}
