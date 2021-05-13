using Mindmagma.Curses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microservice.Library.ConsoleTool
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
        /// 总数
        /// </summary>
        private int Count { get; set; }

        /// <summary>
        /// 光标的列位置。将从 0 开始从左到右对列进行编号。
        /// </summary>
        private int? Left { get; set; }

        /// <summary>
        /// 光标的行位置。从上到下，从 0 开始为行编号。
        /// </summary>
        private int?[] Top { get; set; }

        /// <summary>
        /// 进度条宽度。
        /// </summary>
        private int Width { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        private decimal Ratio { get; set; }

        /// <summary>
        /// 初始化状态
        /// </summary>
        private bool[] InitFlag { get; set; }

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

        private void AutoResize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                NCurses.ResizeTerminal(Width < NCurses.Columns ? NCurses.Columns : Width, NCurses.Lines);
            else
                Console.SetBufferSize(Width < Console.BufferWidth ? Console.BufferWidth : Width, Console.BufferHeight);
        }

        private void Move(int x, int y)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                NCurses.Move(y, x);
            else
                Console.SetCursorPosition(x, y);
        }

        //private int X()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        //    {
        //        NCurses.GetMouse(out MouseEvent mouseEvent);
        //        return mouseEvent.x;
        //    }
        //    else
        //        return Console.CursorLeft;
        //}

        /// <summary>
        /// 控制台进度条
        /// </summary>
        /// <param name="count">进度条数量</param>
        /// <param name="width">进度条宽度（单位为字符的宽度）</param>
        /// <param name="ProgressBarType">进度条类型</param>
        /// <param name="init">初始化进度条</param>
        /// <param name="left">光标左侧位置(默认使用控制台当前的值)</param>
        /// <param name="top">光标顶部位置(默认使用控制台当前的值)</param>
        public ProgressBar(int count = 1, int width = 50, ProgressBarType ProgressBarType = ProgressBarType.Multicolor, bool init = false, int? left = null, int? top = null)
        {
            Count = count;
            progressBarType = ProgressBarType;
            if (left.HasValue)
                Left = left.Value;
            Top = new int?[count];
            Width = width;
            Ratio = 100 / width;
            InitFlag = new bool[count];
            Value = new List<byte?[]>();
            Msg = new string[count];

            for (int i = 0; i < count; i++)
            {
                InitFlag[i] = false;
                Value.Add(new byte?[100]);
                if (top.HasValue)
                    Top[i] = top.Value + i * 2;
            }

            AutoResize();

            if (init)
                InitAll();
        }

        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="left">光标左侧位置(默认使用实例化时的值)</param>
        /// <param name="top">光标顶部位置(默认使用实例化时的值)</param>
        public ProgressBar InitAll(int? left = null, int? top = null)
        {
            for (int i = 0; i < Count; i++)
                Init(i, left, top);
            return this;
        }

        /// <summary>
        /// 初始化进度条
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="left">光标左侧位置(默认使用实例化时的值)</param>
        /// <param name="top">光标顶部位置(默认使用实例化时的值)</param>
        public ProgressBar Init(int index, int? left = null, int? top = null)
        {
            if (left.HasValue)
                Left = left.Value;
            else if (Left == null)
                Left = Console.CursorLeft;

            if (top.HasValue)
                Top[index] = top.Value + index * 2;
            else if (Top[index] == null)
                Top[index] = (index == 0 ? Console.CursorTop : Top[0]) + index * 2;

            for (int i = 0; i < 100; i++)
            {
                Value[index][i] = null;
            }

            lock (Lock.LockObject)
            {
                // 清空显示区域；
                Move(Left.Value, Top[index].Value);
                for (int j = Left.Value; ++j < Console.WindowWidth;) { Console.Write(" "); }
                if (progressBarType == ProgressBarType.Multicolor)
                {
                    // 绘制进度条背景； 
                    colorBack = Console.BackgroundColor;
                    Move(Left.Value, Top[index].Value);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    for (int j = 0; ++j <= Width;) { Console.Write(" "); }
                    Console.BackgroundColor = colorBack;
                }
                else
                {
                    // 绘制进度条背景；
                    Move(Left.Value, Top[index].Value);
                    Console.Write($"[{new string(' ', Width)}]");
                    //Move(Left.Value + Width - 1, Top[index].Value);
                    //Console.Write("]");
                }

                ReSetCursor(index);

                InitFlag[index] = true;

                return this;
            }
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(double value, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(double value, string text, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(float value, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(float value, string text, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(decimal value, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(decimal value, string text, int index = 0)
        {
            return Draw(true, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(int value, int index = 0)
        {
            return Draw(true, 1, value, null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Normal(int value, string text, int index = 0)
        {
            return Draw(true, 1, value, text, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar NormalAppend(int value, int index = 0)
        {
            return Draw(true, 1, value + CurrentValue(index), null, index);
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar NormalAppend(int value, string text, int index = 0)
        {
            return Draw(true, 1, value + CurrentValue(index), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(double value, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(double value, string text, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(float value, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(float value, string text, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(decimal value, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(decimal value, string text, int index = 0)
        {
            return Draw(false, 1, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(int value, int index = 0)
        {
            return Draw(false, 1, value, null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Successes(int value, string text, int index = 0)
        {
            return Draw(false, 1, value, text, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar SuccessesAppend(int value, int index = 0)
        {
            return Draw(false, 1, value + CurrentValue(index), null, index);
        }

        /// <summary>
        /// 成功的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar SuccessesAppend(int value, string text, int index = 0)
        {
            return Draw(false, 1, value + CurrentValue(index), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(double value, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(double value, string text, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(float value, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(float value, string text, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(decimal value, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(decimal value, string text, int index = 0)
        {
            return Draw(false, 2, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(int value, int index = 0)
        {
            return Draw(false, 2, value, null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Warned(int value, string text, int index = 0)
        {
            return Draw(false, 2, value, text, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar WarnedAppend(int value, int index = 0)
        {
            return Draw(false, 1, value + CurrentValue(index), null, index);
        }

        /// <summary>
        /// 警告的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar WarnedAppend(int value, string text, int index = 0)
        {
            return Draw(false, 1, value + CurrentValue(index), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(double value, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(double value, string text, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(float value, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(float value, string text, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(decimal value, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(decimal value, string text, int index = 0)
        {
            return Draw(false, 0, Convert.ToInt32(value * 100), text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(int value, int index = 0)
        {
            return Draw(false, 0, value, null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(百分比)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar Failures(int value, string text, int index = 0)
        {
            return Draw(false, 0, value, text, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar FailuresAppend(int value, int index = 0)
        {
            return Draw(false, 0, value + CurrentValue(index), null, index);
        }

        /// <summary>
        /// 失败的进度
        /// </summary>
        /// <param name="value">进度值(增量值)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar FailuresAppend(int value, string text, int index = 0)
        {
            return Draw(false, 0, value + CurrentValue(index), text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(double value, int index = 0)
        {
            return Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(double value, string text, int index = 0)
        {
            return Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(float value, int index = 0)
        {
            return Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(float value, string text, int index = 0)
        {
            return Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(decimal value, int index = 0)
        {
            return Draw(false, null, -(int)value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(decimal value, string text, int index = 0)
        {
            return Draw(false, null, -(int)value, text, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(int value, int index = 0)
        {
            return Draw(false, null, -value, null, index);
        }

        /// <summary>
        /// 回滚进度
        /// </summary>
        /// <param name="value">回滚进度值(分子)</param>
        /// <param name="text">文本信息（显示在进度条右侧）</param>
        /// <param name="index">进度条索引（当有多个进度条时）</param>
        /// <returns></returns>
        public ProgressBar RollBack(int value, string text, int index = 0)
        {
            return Draw(false, null, -value, text, index);
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
        /// 当前进度值
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        private int CurrentValue(int index)
        {
            var value = Array.IndexOf(Value[index], null);
            return value == -1 ? 100 : value;
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
        public ProgressBar Draw(bool reFill, byte? state, int length, string text, int index)
        {
            if (!InitFlag[index])
                Init(index);

            lock (Lock.LockObject)
            {
                //保存原始位置
                var oLeft = Console.CursorLeft;
                var oTop = Console.CursorTop;

                var lastDrawIndex = reFill ? 0 : Array.IndexOf(Value[index], null);

                if (lastDrawIndex != -1 || (length < 0 && lastDrawIndex > 0))
                {
                    if (lastDrawIndex + length < 0)
                        length = lastDrawIndex;
                    if (length > 100)
                        length = 100;

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

                    var width = GetWidth(length);

                    // 保存背景色与前景色；
                    colorBack = Console.BackgroundColor;
                    colorFore = Console.ForegroundColor;

                    switch (progressBarType)
                    {
                        case ProgressBarType.Multicolor:
                            // 绘制进度条进度
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
                            Move(Left.Value + GetWidth(lastDrawIndex), Top[index].Value);
                            Console.Write(new string(' ', width));
                            Console.BackgroundColor = colorBack;
                            break;
                        default:
                        case ProgressBarType.Character:
                            Move(Left.Value + lastDrawIndex, Top[index].Value);
                            Console.Write($"[{new string('*', width)}{new string(' ', Width - width)}]");
                            break;
                    }
                }

                if (Msg[index] == null || (text != null && Msg[index] != text) || Msg[index].Last() == '%')
                {
                    if (Msg[index] != null)
                    {
                        //清空之前的内容
                        Move(Left.Value + Width + 2, Top[index].Value);
                        Console.Write(new string(' ', Msg[index].Length));
                    }

                    Msg[index] = string.IsNullOrWhiteSpace(text) ? $"{Value[index].Count(i => i != null)}%" : text;

                    // 显示信息                      
                    if (progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = ConsoleColor.White;
                    Move(Left.Value + Width + 2, Top[index].Value);
                    Console.Write(Msg[index]);
                    if (progressBarType == ProgressBarType.Multicolor)
                        Console.ForegroundColor = colorFore;
                }

                //恢复原始位置
                Move(oLeft, oTop);
                //ReSetCursor(index);
            }
            return this;
        }

        /// <summary>
        /// 恢复光标到正常位置,恢复颜色
        /// </summary>
        /// <param name="index"></param>
        private void ReSetCursor(int index)
        {
            Move(0, Top[index].Value + (Count - index) * 2);
            Console.ForegroundColor = ConsoleColor.White;
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
