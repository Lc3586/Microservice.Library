using Library.Extention;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Models
{
    /// <summary>
    /// 命令行配置
    /// </summary>
    public class CommandConfig
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 模块配置信息
        /// </summary>
        public List<Modular> Modulars { get; set; } = new List<Modular>();

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 模块配置
    /// </summary>
    public class Modular
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件路径
        /// <para>相对路径</para>
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 排序值
        /// <para>排序规则：升序</para>
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 方法配置信息
        /// </summary>
        public List<Method> Methods { get; set; } = new List<Method>();

        /// <summary>
        /// 参数配置信息
        /// </summary>
        public List<Arg> Args { get; set; } = new List<Arg>();

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 方法配置
    /// </summary>
    public class Method
    {
        /// <summary>
        /// 名称
        /// <para>命令行匹配用的名称</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所在类名称
        /// <para>包含命名空间</para>
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 明确的名称
        /// <para>代码中的名称</para>
        /// </summary>
        public string SpecifiedName { get; set; }

        /// <summary>
        /// 静态
        /// </summary>
        public bool Static { get; set; }

        /// <summary>
        /// 实例化时调用构造函数时携带配置作为参数
        /// <para>启用时，目标类必须同时具有无参构造函数</para>
        /// </summary>
        public bool IConfig { get; set; }

        /// <summary>
        /// 异步
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// 参数数组转模型
        /// <para>使用完整名称匹配（忽略大小写）</para>
        /// </summary>
        public Arg2Model Arg2Model { get; set; }

        /// <summary>
        /// 排序值
        /// <para>排序规则：升序</para>
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 参数转模型
    /// <para>使用模型传参至目标方法</para>
    /// </summary>
    public class Arg2Model
    {
        /// <summary>
        /// 模型
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 所在类名称
        /// <para>包含命名空间</para>
        /// </summary>
        public string TypeName { get; set; }
    }

    /// <summary>
    /// 参数配置
    /// </summary>
    public class Arg
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属方法名称
        /// <para>命令行匹配用的名称</para>
        /// <para>多个使用【，】拼接</para>
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 排序值
        /// <para>排序规则：升序</para>
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 参数类型
        /// <para>可用的值：</para>
        /// <para>MultipleValue 可多个,例如 --param=A --param=B --param=C</para>
        /// <para>SingleValue 单个，例如 -param=A</para>
        /// <para>SingleOrNoValue 单个，可选，例如 --param 或 --param=A</para>
        /// <para>NoValue 单个，无值，例如 --noexit</para>
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public CommandOptionType Type_enum { get { var _Type = CommandOptionType.SingleOrNoValue; Enum.TryParse(Type, out _Type); return _Type; } }

        /// <summary>
        /// 数据类型
        /// <para>必须是C#基础类型，例如 System.String</para>
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType_Type { get { return System.Type.GetType(DataType); } }

        /// <summary>
        /// 默认值
        /// <para>参数未赋值时使用此值</para>
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 参数配置
    /// <para>中转操作</para>
    /// </summary>
    public class Arg_internal
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 所属方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 所属方法
        /// <para>匹配时使用</para>
        /// </summary>
        public string Method_match { get { return $",{Method},"; } }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
