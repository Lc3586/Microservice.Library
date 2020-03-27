using Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace T4CAGC.Models
{
    /// <summary>
    /// 生成配置
    /// </summary>
    public class GenerateConfig
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "";

        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 输出路径
        /// </summary>
        public List<string> OutputPath { get; set; }

        /// <summary>
        /// 文件输出政策
        /// </summary>
        public string outputFile { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType? DbType_Type { get { var result = DatabaseType.MySql; if (!Enum.TryParse(DbType, out result)) return null; return result; } }

        /// <summary>
        /// 数据库连接语句
        /// </summary>
        public string DbConnection { get; set; }

        /// <summary>
        /// 目标数据库
        /// </summary>
        public List<string> Database { get; set; }

        /// <summary>
        /// 指定表
        /// </summary>
        public List<string> Table { get; set; }

        /// <summary>
        /// 忽略表
        /// </summary>
        public List<string> TableIgnore { get; set; }

        /// <summary>
        /// 类注释
        /// </summary>
        public List<string> ClassComments { get; set; }

        /// <summary>
        /// 类属性
        /// </summary>
        public List<string> ClassAttribute { get; set; }

        /// <summary>
        /// 类修饰符
        /// </summary>
        public List<string> ClassModifier { get; set; }

        /// <summary>
        /// 类继承
        /// </summary>
        public List<string> ClassInheritance { get; set; }

        /// <summary>
        /// 类默认方法
        /// </summary>
        public List<string> ClassDefaultFunction { get; set; }

        /// <summary>
        /// 成员注释
        /// </summary>
        public List<string> MemberComments { get; set; }

        /// <summary>
        /// 成员属性
        /// </summary>
        public List<string> MemberAttribute { get; set; }

        /// <summary>
        /// 成员修饰符
        /// </summary>
        public List<string> MemberModifier { get; set; }

        /// <summary>
        /// 使用数据库注释作为成员显示名称
        /// </summary>
        public bool MemberDisplay { get; set; }

        /// <summary>
        /// 成员数据验证策略
        /// </summary>
        public List<string> MemberVerify { get; set; }

        /// <summary>
        /// 成员默认值
        /// </summary>
        public List<string> MemberDefaultValue { get; set; }

        /// <summary>
        /// 成员附加策略
        /// </summary>
        public List<string> MemberAdd { get; set; }
    }
}
