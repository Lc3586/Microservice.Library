using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.ConsoleTool
{
    /// <summary>
    /// 控制台进度条
    /// <!-- LCTR 2019-08-06 新增允许同时存在多个进度条 -->
    /// <!-- LCTR 2020-06-02 新增进度条显示失败效果 -->
    /// <!-- 参考资料：https://www.cnblogs.com/zhanghuabin/p/5310680.html -->
    /// </summary>
    public class ProgressBar
    {

        /// <summary>
        /// 光标的列位置。将从 0 开始从左到右对列进行编号。
        /// </summary>
        private int Left { get; set; }

        /// <summary>
        /// 光标的行位置。从上到下，从 0 开始为行编号。
        /// </summary>
        private int[] Top { get; set; }

        /// <summary>
        /// 进度条宽度。
        /// </summary>
        private int Width { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        private decimal Ratio { get; set; }

        /// <summary>
        /// 进度条当前值。
        /// </summary>
        private List<byte?[]> Value { get; set; }

        /// <summary>
        /// 进度条当前信息。
        /// </summary>
        private string[] Msg { get; set; }

        /// <summary>
        /// 进度条类型
        /// </summary>
        private ProgressBarType progressBarType { get; set; }


        private ConsoleColor colorBack;
        private ConsoleColor colorFore;

        private object lockobj = null;

        /// <summary>
        /// 控制台进度条
        /// </summary>
        public ProgressBar() : this(Console.CursorLeft, Console.CursorTop)
        {

        }

        /// <summary>
        /// 控制台进度条
        /// </summary>
        /// <param name="left">光标左侧位置</param>
        /// <param name="top">光标顶部位置</param>
        /// <param name="count">进度条数量</param>
        /// <param name="width">进度条宽度（单位为字符的宽度）</param>
        /// <param name="ProgressBarType">进度条类型</param>
        public ProgressBar(int left, int top, int count = 1, int width = 50, ProgressBarType ProgressBarType = ProgressBarType.Multicolor)
        {
            lockobj = new object();
            progressBarType = ProgressBarType;
            Left = left;
            Top = new int[count];
            Width = width;
            Ratio = 100 / width;
            Value = new List<byte?[]>();
            Msg = new string[count];

            for (int i = 0; i < count; i++)
            {
                Value.Add(new byte?[100]);
                Top[i] = top + i * 2;
            }

            InitAll(count);
        }

        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="count">数量</param>
        private void InitAll(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Init(i);
            }
        }

        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="index">索引</param>
        private void Init(int index)
        {
            for (int i = 0; i < Value[index].Length; i++)
            {
                Value[index][i] = null;
            }

            // 清空显示区域；
            Console.SetCursorPosition(Left, Top[index]);
            for (int j = Left; ++j < Console.WindowWidth;) { Console.Write(" "); }
            if (progressBarType == ProgressBarType.Multicolor)
            {
                // 绘制进度条背景； 
                colorBack = Console.BackgroundColor;
                Console.SetCursorPosition(Left, Top[index]);
                Console.BackgroundColor = ConsoleColor.DarkGray;
                for (int j = 0; ++j <= Width;) { Console.Write(" "); }
                Console.BackgroundColor = colorBack;
            }
            else
            {
                // 绘制进度条背景；    
                Console.SetCursorPosition(Left, Top[index]);
                Console.Write("[");
                Console.SetCursorPosition(Left + Width - 1, Top[index]);
                Console.Write("]");
            }
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(double value, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(double value, string text, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(float value, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(float value, string text, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(decimal value, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(decimal value, string text, int index = 0)
        {
            Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(int value, int index = 0)
        {
            Draw(true, 1, value, null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Normal(int value, string text, int index = 0)
        {
            Draw(true, 1, value, text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(double value, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(double value, string text, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(float value, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(float value, string text, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(decimal value, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(decimal value, string text, int index = 0)
        {
            Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(int value, int index = 0)
        {
            Draw(false, 1, value, null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Successes(int value, string text, int index = 0)
        {
            Draw(false, 1, value, text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(double value, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(double value, string text, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(float value, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(float value, string text, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(decimal value, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(decimal value, string text, int index = 0)
        {
            Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(int value, int index = 0)
        {
            Draw(false, 2, value, null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Warned(int value, string text, int index = 0)
        {
            Draw(false, 2, value, text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(double value, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(double value, string text, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(float value, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(float value, string text, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(decimal value, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(decimal value, string text, int index = 0)
        {
            Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(int value, int index = 0)
        {
            Draw(false, 0, value, null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Failures(int value, string text, int index = 0)
        {
            Draw(false, 0, value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(double value, int index = 0)
        {
            Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(double value, string text, int index = 0)
        {
            Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(float value, int index = 0)
        {
            Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(float value, string text, int index = 0)
        {
            Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(decimal value, int index = 0)
        {
            Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(decimal value, string text, int index = 0)
        {
            Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(int value, int index = 0)
        {
            Draw(false, null, -value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void RollBack(int value, string text, int index = 0)
        {
            Draw(false, null, -value, text, index);
        }

        /// <summary>
        /// 获取宽度
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private int GetWidth(int length)
        {
            return (int)Math.Round(Math.Abs(length) / Ratio);
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="reFill">重新填充</param>
        /// <param name="state">状态(0:失败,1:成功,2:警告)</param>
        /// <param name="length">长度</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public void Draw(bool reFill, byte? state, int length, string text, int index)
        {
            lock (lockobj)
            {
                var lastDrawIndex = reFill ? 0 : Array.IndexOf(Value[index], null);

                if (lastDrawIndex != -1 || length < 0)
                {
                    if (reFill)
                        Init(index);

                    if (lastDrawIndex == -1)
                        lastDrawIndex = Value[index].Length - 1;

                    for (int i = length < 0 ? lastDrawIndex + length : lastDrawIndex; i < (length < 0 ? lastDrawIndex : length); i++)
                    {
                        Value[index][i] = state;
                    }

                    if (length > 0)
                        length -= lastDrawIndex;
                    else
                        lastDrawIndex += length;

                    // 保存背景色与前景色；
                    colorBack = Console.BackgroundColor;
                    colorFore = Console.ForegroundColor;

                    char c;

                    switch (progressBarType)
                    {
                        case ProgressBarType.Multicolor:
                            // 绘制进度条进度                
                            Console.SetCursorPosition(Left + GetWidth(lastDrawIndex), Top[index]);
                            switch (state)
                            {
                                case 1:
                                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                                    break;
                                case 0:
                                    Console.BackgroundColor = ConsoleColor.DarkRed;
                                    break;
                                case 2:
                                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                                    break;
                                case null:
                                default:
                                    Console.BackgroundColor = ConsoleColor.DarkGray;
                                    break;
                            }
                            c = ' ';
                            break;
                        default:
                        case ProgressBarType.Character:
                            Console.SetCursorPosition(Left + lastDrawIndex + 1, Top[index]);
                            c = '*';
                            break;
                    }

                    Console.Write(new string(c, GetWidth(length)));
                    Console.BackgroundColor = colorBack;

                    //for (int i = lastDrawIndex, j = 0; i < Value[index].Length - length; i++)
                    //{
                    //    if (Value[index][i] == Value[index][i++])
                    //    {
                    //        j++;
                    //        continue;
                    //    }

                    //    char c;

                    //    switch (progressBarType)
                    //    {
                    //        case ProgressBarType.Multicolor:
                    //            // 绘制进度条进度                
                    //            Console.SetCursorPosition(Left + i - j, Top[index]);
                    //            switch (Value[index][i])
                    //            {
                    //                case true:
                    //                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    //                    break;
                    //                case false:
                    //                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    //                    break;
                    //                case null:
                    //                default:
                    //                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    //                    break;
                    //            }
                    //            c = ' ';
                    //            break;
                    //        default:
                    //        case ProgressBarType.Character:
                    //            Console.SetCursorPosition(Left + i - j + 1, Top[index]);
                    //            c = '*';
                    //            break;
                    //    }

                    //    Console.Write(new string(c, (int)Math.Round(j / (100.0 / Width))));
                    //    j = 0;
                    //}

                    //Console.BackgroundColor = colorBack;
                }

                if (Msg[index] == null || Msg[index] != text)
                {
                    Msg[index] = string.IsNullOrWhiteSpace(text) ? $"{Value[index]}%" : text;

                    // 显示信息                      
                    if (progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(Left + Width + 1, Top[index]);
                    Console.Write(Msg[index]);
                    if (progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = colorFore;

                }

                //恢复光标到正常位置,恢复颜色
                for (int i = 0; i < Value.Count; i++)
                {
                    if (Value[i].Last() != null)
                        break;
                    if (i == Value.Count - 1)
                    {
                        Console.SetCursorPosition(0, Top[i] + 1);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
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
