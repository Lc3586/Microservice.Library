using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.ConsoleTool
{
    /// <summary>
    /// 点阵图
    /// LCTR 2020-06-11
    /// </summary>
    public class DotPlot
    {
        /// <summary>
        /// 矩阵数据
        /// </summary>
        private byte[][] Matrix { get; set; }

        /// <summary>
        /// 矩阵数据(历史数据)
        /// </summary>
        private byte?[][] Matrix_History { get; set; }

        private DotPlotSetting Setting { get; set; }

        /// <summary>
        /// 点阵图
        /// </summary>
        /// <param name="xLenght">横向长度</param>
        /// <param name="yLength">纵向长度</param>
        /// <param name="colors">颜色</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="title">标题</param>
        /// <param name="dotSize">点位尺寸（使用Unicode字符数）（默认2个）</param>
        /// <param name="showColorsTitle">显示点位颜色的标题</param>
        public DotPlot(int xLenght, int yLength, List<DotColor> colors, byte defaultValue, string title = null, int dotSize = 2, bool showColorsTitle = true)
        {
            Setting = new DotPlotSetting
            {
                XLenght = xLenght,
                YLength = yLength,
                Title = title,
                DotSize = dotSize,
                DefaultValue = defaultValue,
                Colors = colors,
                ColorMapper = colors.ToDictionary(k => k.Value, v => v.Color),
                ShowColorsTitle = showColorsTitle
            };

            Init();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="left">光标左侧位置(默认使用控制台当前值)</param>
        /// <param name="top">光标顶部位置(默认使用控制台当前值)</param>
        public DotPlot Show(int? left = null, int? top = null)
        {
            lock (Lock.LockObject)
            {
                var bufferSizeWidth = Setting.XLenght * Setting.DotSize;
                var bufferSizeHeight = Setting.YLength;
                Console.SetBufferSize(bufferSizeWidth < Console.BufferWidth ? Console.BufferWidth : bufferSizeWidth, bufferSizeHeight < Console.BufferHeight ? Console.BufferHeight : bufferSizeHeight);

                Setting.Left = left ?? Console.CursorLeft;
                Setting.Top = top ?? Console.CursorTop;

                if (!string.IsNullOrEmpty(Setting.Title))
                    ShowTitle();

                if (Setting.ShowColorsTitle)
                    ShowColorsTitle();
            }

            Draw();
            return this;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="x">横轴位置(索引)</param>
        /// <param name="y">纵轴位置(索引)</param>
        /// <param name="value">值</param>
        /// <param name="update">刷新</param>
        /// <returns></returns>
        public DotPlot SetValue(int x, int y, byte value, bool update = true)
        {
            Matrix[y][x] = value;

            if (update)
                return Update();

            return this;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <returns></returns>
        public DotPlot Update()
        {
            Draw();
            return this;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Matrix = new byte[Setting.YLength][];
            Matrix_History = new byte?[Setting.YLength][];
            for (int i = 0; i < Matrix.Length; i++)
            {
                Matrix[i] = new byte[Setting.XLenght];
                Matrix_History[i] = new byte?[Setting.XLenght];
                for (int j = 0; j < Matrix[i].Length; j++)
                {
                    Matrix[i][j] = Setting.DefaultValue;
                }
            }
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        private void ShowTitle()
        {
            Console.SetCursorPosition(Setting.Left, Setting.Top);
            if (Setting.Title.Length < Setting.XLenght * Setting.DotSize)
                Console.Write(new string(' ', (Setting.XLenght * Setting.DotSize - Setting.Title.Length) / 2));
            Console.Write(Setting.Title);
            Setting.Top += 2;
        }

        /// <summary>
        /// 显示颜色标题
        /// </summary>
        private void ShowColorsTitle()
        {
            Console.SetCursorPosition(Setting.Left, Setting.Top);
            var margin = Setting.Colors.Sum(c => Setting.DotSize + 2 + c.Title.Length);
            if (margin < Setting.XLenght * Setting.DotSize)
                Console.Write(new string(' ', (Setting.XLenght * Setting.DotSize - margin) / 2));
            var backgroundColor = Console.BackgroundColor;
            Setting.Colors.ForEach(c =>
            {
                Console.BackgroundColor = c.Color;
                Console.Write(new string(' ', Setting.DotSize));
                Console.BackgroundColor = backgroundColor;
                Console.Write($" {c.Title} ");

            });
            Console.BackgroundColor = backgroundColor;
            Setting.Top += 2;
        }

        /// <summary>
        /// 绘制
        /// </summary>
        private void Draw()
        {
            lock (Lock.LockObject)
            {
                var backgroundColor = Console.BackgroundColor;
                var backgroundColorLast = backgroundColor;
                for (int i = 0; i < Matrix.Length; i++)
                {
                    Console.SetCursorPosition(Setting.Left, Setting.Top + i);

                    for (int j = 0; j < Matrix[i].Length; j++)
                    {
                        if (Matrix[i][j] != Matrix_History[i][j])
                        {
                            var color = Setting.ColorMapper[Matrix[i][j]];
                            if (backgroundColorLast != color)
                            {
                                Console.BackgroundColor = color;
                                backgroundColorLast = color;
                            }
                            Console.Write(new string(' ', Setting.DotSize));
                            Matrix_History[i][j] = Matrix[i][j];
                        }
                        else
                            Console.SetCursorPosition(Setting.Left + (j + 1) * Setting.DotSize, Setting.Top + i);
                    }
                }

                Console.BackgroundColor = backgroundColor;

                Console.SetCursorPosition(0, Setting.Top + Setting.YLength + 1);
            }
        }

        /// <summary>
        /// 设置
        /// </summary>
        private class DotPlotSetting
        {
            /// <summary>
            /// 光标左侧位置
            /// </summary>
            public int Left { get; set; }

            /// <summary>
            /// 光标顶部位置
            /// </summary>
            public int Top { get; set; }

            /// <summary>
            /// 横向长度
            /// </summary>
            public int XLenght { get; set; }

            /// <summary>
            /// 纵向长度
            /// </summary>
            public int YLength { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 点位尺寸（使用Unicode字符数）
            /// </summary>
            public int DotSize { get; set; }

            /// <summary>
            /// 默认值
            /// </summary>
            public byte DefaultValue { get; set; }

            /// <summary>
            /// 点位颜色集合
            /// </summary>
            public List<DotColor> Colors { get; set; }

            /// <summary>
            /// 颜色值映射字典
            /// </summary>
            public Dictionary<byte, ConsoleColor> ColorMapper { get; set; }

            /// <summary>
            /// 显示颜色标题
            /// </summary>
            public bool ShowColorsTitle { get; set; }
        }

        /// <summary>
        /// 点位颜色
        /// </summary>
        public class DotColor
        {
            /// <summary>
            /// 值
            /// </summary>
            public byte Value { get; set; }

            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 颜色
            /// </summary>
            public ConsoleColor Color { get; set; }
        }
    }
}
