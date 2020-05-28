using System;
using System.Collections.Generic;
using System.Linq;
using Library.Configuration;
using Library.Extension;
using Library.SuperSocket.Extension;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// JT协议
    /// </summary>
    public class JTProtocol
    {
        /// <summary>
        /// 获取JT协议
        /// </summary>
        /// <param name="path">配置文件路径</param>
        /// <param name="name">协议名称</param>
        /// <returns></returns>
        public static JTProtocol Get(string path, JTProtocolName name)
        {
            var result = new ConfigHelper(path).GetModel<JTProtocol>(name.ToString());
            if (result.Structures.Any_Ex())
            {
                //排序
                result.Structures = result.Structures.OrderBy(o => o.Value.Order).ToDictionary(k => k.Key, v =>
                {
                    var value = v.Value;
                    value.Internal = value.Internal?.ToDictionary(ik => ik.Key, iv => iv.Value?.OrderBy(ivv => ivv.Value.Order).ToDictionary(ivk => ivk.Key, ivv => ivv.Value));
                    return value;
                });
            }
            result.Name = name;
            return result;
        }

        /// <summary>
        /// 协议名称
        /// </summary>
        public JTProtocolName Name { get; set; }

        /// <summary>
        /// 数据流是否遵循大端
        /// </summary>
        public bool BigEndian { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// 帧头
        /// </summary>
        public FlagValue HeadFlag { get; set; }

        /// <summary>
        /// 帧头
        /// </summary>
        public byte[] HeadFlagValue => HeadFlag.GetBytes(this);

        /// <summary>
        /// 帧尾
        /// </summary>
        public FlagValue EndFlag { get; set; }

        /// <summary>
        /// 帧尾
        /// </summary>
        public byte[] EndFlagValue => EndFlag.GetBytes(this);

        /// <summary>
        /// 转义集合
        /// </summary>
        public Dictionary<string, string[]> Escapes { get; set; }

        /// <summary>
        /// 数据映射集合
        /// </summary>
        public Dictionary<string, List<Matches>> DataMappings { get; set; }

        /// <summary>
        /// 内部结构实体映射集合
        /// </summary>
        public Dictionary<string, InternalEntitysMapping> InternalEntitysMappings { get; set; }

        /// <summary>
        /// CRC数据校验配置
        /// </summary>
        public CrcCcittConfig CrcCcitt { get; set; }

        /// <summary>
        /// 加密配置
        /// </summary>
        public EncryptConfig Encrypt { get; set; }

        /// <summary>
        /// 结构集合
        /// </summary>
        public Dictionary<string, Structure> Structures { get; set; }

        /// <summary>
        /// 结构信息
        /// </summary>
        public class Structure
        {
            /// <summary>
            /// 排序值
            /// </summary>
            public int Order { get; set; }

            /// <summary>
            /// 属性
            /// </summary>
            public string Property { get; set; }

            /// <summary>
            /// 说明
            /// </summary>
            public string Explain { get; set; }

            /// <summary>
            /// 长度
            /// </summary>
            public int? Length { get; set; }

            /// <summary>
            /// 结构类型
            /// </summary>
            public StructureType StructureType { get; set; } = StructureType.normal;

            /// <summary>
            /// 解码类型
            /// </summary>
            public CodeInfo CodeInfo { get; set; }

            /// <summary>
            /// 动态计算表达式
            /// </summary>
            public string CompileExpression { get; set; }

            /// <summary>
            /// 数据映射
            /// </summary>
            public string DataMapping { get; set; }

            /// <summary>
            /// 内部结构的Key来自于哪个属性
            /// 为null时默认取第一个
            /// </summary>
            public InternalProperty InternalPropertyForKey { get; set; }

            /// <summary>
            /// 内部结构
            /// </summary>
            public Dictionary<string, Dictionary<string, Structure>> Internal { get; set; }

            /// <summary>
            /// 附加信息
            /// </summary>
            public Additional Additional { get; set; }

            /// <summary>
            /// 是否需要计算
            /// </summary>
            /// <returns></returns>
            public bool NeedCompile => !string.IsNullOrEmpty(CompileExpression);

            /// <summary>
            /// 是否需要映射
            /// </summary>
            /// <returns></returns>
            public bool NeedMapping => !string.IsNullOrEmpty(DataMapping);
        }

        /// <summary>
        /// 帧值
        /// </summary>
        public class FlagValue
        {
            /// <summary>
            /// 解码类型
            /// </summary>
            public CodeInfo CodeInfo { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            private byte[] Bytes { get; set; }

            /// <summary>
            /// 获取Byte值数组
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes(JTProtocol jTProtocol)
            {
                if (Bytes != null)
                    return Bytes;
                Bytes = jTProtocol.Encode(Value, CodeInfo);
                return Bytes;
            }
        }

        /// <summary>
        /// 内部结构的Key所属的属性信息
        /// </summary>
        public class InternalProperty
        {
            /// <summary>
            /// 属性
            /// </summary>
            public string Property { get; set; }

            /// <summary>
            /// 编码
            /// </summary>
            public CodeInfo Encode { get; set; }

            /// <summary>
            /// 解码
            /// </summary>
            public CodeInfo Decode { get; set; }
        }

        /// <summary>
        /// 转换
        /// </summary>
        public class Additional
        {
            /// <summary>
            /// 长度
            /// </summary>
            public int Length { get; set; }

            /// <summary>
            /// 解码类型
            /// </summary>
            public CodeInfo CodeInfo { get; set; }

            /// <summary>
            /// 转换
            /// </summary>
            public Dictionary<string, Structure> Switch { get; set; }
        }

        /// <summary>
        /// CRC数据校验配置
        /// </summary>
        public class CrcCcittConfig
        {
            /// <summary>
            /// 初始值
            /// </summary>
            public CrcLength CrcLength { get; set; }

            /// <summary>
            /// 初始值
            /// </summary>
            public InitialCrcValue InitialCrcValue { get; set; }

            /// <summary>
            /// 需要跳过部分的长度[头，尾]
            /// </summary>
            public int[] Skip { get; set; }

            /// <summary>
            /// 取值长度(负数为从后往前取)
            /// </summary>
            public int Take { get; set; }

            /// <summary>
            /// 属性
            /// </summary>
            public string Property { get; set; }
        }

        /// <summary>
        /// 加密配置
        /// </summary>
        public class EncryptConfig
        {
            /// <summary>
            /// 
            /// </summary>
            public UInt32 M1 { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public UInt32 IA1 { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public UInt32 IC1 { get; set; }

            /// <summary>
            /// 目标集合
            /// </summary>
            public Dictionary<string, EncryptProperty> Targets { get; set; }
        }

        /// <summary>
        /// 加密标识和秘钥来自来自于哪些属性
        /// </summary>
        public class EncryptProperty
        {
            /// <summary>
            /// 加密标识
            /// </summary>
            public string Flag { get; set; }

            /// <summary>
            /// 秘钥
            /// </summary>
            public string Key { get; set; }
        }

        /// <summary>
        /// 匹配项
        /// </summary>
        public class Matches
        {
            /// <summary>
            /// 匹配
            /// </summary>
            public string Matching { get; set; }

            /// <summary>
            /// 值
            /// </summary>
            public string Value { get; set; }
        }

        /// <summary>
        /// 内部结构实体映射
        /// </summary>
        public class InternalEntitysMapping
        {
            /// <summary>
            /// 命名空间
            /// </summary>
            public string Assembly { get; set; }

            /// <summary>
            /// 类名
            /// </summary>
            public string TypeName { get; set; }
        }
    }
}
