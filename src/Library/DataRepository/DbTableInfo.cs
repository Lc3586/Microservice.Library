using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Microservice.Library.DataRepository
{

    /// <summary>
    /// 表结构
    /// </summary>
    public sealed class DbTableInfo
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        #region 特殊属性

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        #endregion

        /// <summary>
        /// 表的架构
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// 表的记录数
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 含有主键
        /// </summary>
        public bool HasPrimaryKey { get; set; }

        /// <summary>
        /// 主键名称
        /// <para>;号拼接</para>
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// 备注与说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 表字段结构集合
        /// </summary>
        public List<DbColumn> Column { get; set; }
    }

    /// <summary>
    /// 表字段结构
    /// </summary>
    public sealed class DbColumn
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据库类型对应的C#类型
        /// </summary>
        public Type CSharpType { get; private set; }

        /// <summary>
        /// 数据库类型对应的C#类型名称
        /// </summary>
        public string CSharpTypeName { get; private set; }

        /// <summary>
        /// 为数值
        /// </summary>
        public bool IsNumber { get; private set; }

        /// <summary>
        /// 为日期时间
        /// </summary>
        public bool IsDateTime { get; private set; }

        /// <summary>
        /// 为选项
        /// </summary>
        public bool HasOption { get; private set; }

        /// <summary>
        /// 范围最小值
        /// </summary>
        public string Range_Min { get; private set; }

        /// <summary>
        /// 范围最大值
        /// </summary>
        public string Range_Max { get; private set; }

        /// <summary>
        /// 选项
        /// </summary>
        public Dictionary<string, string> Options { get; private set; }

        /// <summary>
        /// 主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// 外键关系表集合
        /// </summary>
        public string FKTableNames { get; set; }

        /// <summary>
        /// 字节长度
        /// </summary>
        public int ByteLength { get; set; }

        /// <summary>
        /// 字符长度
        /// </summary>
        public int CharLength { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// 小数位
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// 自增列
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        /// 可为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 可为空类型
        /// </summary>
        public bool IsNullableType { get; private set; }

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 其他说明
        /// </summary>
        public List<string> Description_Other { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="errorUseDefault">异常时使用默认值</param>
        /// <returns></returns>
        public bool Init(DatabaseType dbType, bool errorUseDefault = true)
        {
            try
            {

                #region 数据类型解析

                CSharpTypeName = DbType2CSharpType.GetCSharpTypeName(dbType, DataType);
                CSharpType = DbType2CSharpType.GetCSharpType(dbType, DataType);

                IsNullableType = IsNullable && CSharpType != typeof(string);

                IsNumber = ",sbyte,short,int,double,float,decimal,".IndexOf("," + CSharpTypeName + ",") > -1 && Precision > 0;

                if (IsNumber)
                {
                    string range_max = string.Empty;
                    for (int i = 0; i < Precision; i++)
                    {
                        range_max += "9";
                    }
                    if (Scale > 0)
                    {
                        range_max += ".";
                        for (int i = 0; i < Scale; i++)
                        {
                            range_max += "9";
                        }
                    }
                    var max = CSharpType.GetField("MaxValue").GetRawConstantValue();
                    var range_max_out = max;
                    var args_max = new object[] { range_max, range_max_out };
                    if ((bool)CSharpType.GetMethod("TryParse", new[] { typeof(string), CSharpType.MakeByRefType() }).Invoke(null, args_max))
                        range_max_out = (Comparer.Default.Compare(max, args_max[1]) >= 0 ? args_max[1] : max).ToString();
                    Range_Max = range_max_out.ToString();

                    Range_Min = CSharpType.GetField("MinValue").GetRawConstantValue().ToString();
                }

                IsDateTime = ",DateTime,TimeSpan,".IndexOf("," + CSharpTypeName + ",") > -1;

                #endregion

                #region 注释信息解析

                //注释中   {}为选项内容 ()[]为其他说明
                DisplayName = Regex.Replace(Comment, @"[\{\[](.*?)[\]\}]", "");

                Description_Other = Regex.Matches(Comment, @"[\{\[](.*?)[\]\}]").Cast<Match>().Where(o => o.Success).Select(o => o.Value.TrimStart(new char[] { '{', '[' }).TrimEnd(new char[] { '}', ']' })).ToList();

                var _options = new Regex(@"\{(.*?)\}").Match(Comment);

                if (_options.Success)
                    Options = _options.Value.TrimStart('{').TrimEnd('}').Split(',').ToDictionary(k => k.Split(':')[0], v => v.Split(':')[1]);

                //if (Options != null && Options.Count > 0)
                //{
                //    string result = "{0}";
                //    foreach (var item in Options)
                //    {
                //        result = string.Format(result, $"({ Name} == { item.Key} ? \"{item.Value}\" : {{0}})");
                //    }
                //    OptionOutput = string.Format(result, "\"\"");
                //}

                HasOption = Options != null && Options.Count > 0;

                #endregion

                return true;
            }
            catch (Exception)
            {
                if (errorUseDefault)
                    Default();
                return false;
            }

            //默认值
            void Default()
            {
                DisplayName = Comment;
                IsNumber = false;
                IsDateTime = false;
            }
        }
    }
}
