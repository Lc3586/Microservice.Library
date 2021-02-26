using Flee.PublicTypes;
using Microservice.Library.Extension;
using Microservice.Library.SuperSocket.Extension;
using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microservice.Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// JT协议解码器
    /// </summary>
    public class JTDecoder : IPackageDecoder<MessagePackageInfo>
    {
        public JTDecoder(JTProtocol jTProtocol)
        {
            JTProtocol = jTProtocol;
            Encoding = Encoding.GetEncoding(JTProtocol.Encoding);
            ExpressionContext = new ExpressionContext();
            ExpressionContext.Imports.AddType(typeof(Math));
            if (Crc16Ccitt == null || Crc16Ccitt.initialCrcValue != JTProtocol.CrcCcitt.InitialCrcValue)
                Crc16Ccitt = new CrcCcitt(JTProtocol.CrcCcitt.CrcLength, JTProtocol.CrcCcitt.InitialCrcValue);
        }

        /// <summary>
        /// JT协议
        /// </summary>
        public JTProtocol JTProtocol { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 消息包
        /// </summary>
        private MessagePackageInfo Pack { get; set; }

        /// <summary>
        /// 计算表达式上下文
        /// </summary>
        private ExpressionContext ExpressionContext;

        /// <summary>
        /// CRC数据校验
        /// </summary>
        private CrcCcitt Crc16Ccitt;

        /// <summary>
        /// 流数据解码
        /// </summary>
        /// <param name="buffer">流数据</param>
        /// <param name="context"></param>
        /// <returns>消息包</returns>
        public MessagePackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            if (buffer.IsEmpty)
                throw new Exception("流数据为空");
            if (!JTProtocol.Structures.Any_Ex())
                throw new Exception($"{JTProtocol.Name}协议配置错误");

            Pack = new MessagePackageInfo(JTProtocol.UnEscape(buffer.ToArray()));

            Pack.HeadFlag = JTProtocol.HeadFlagValue;
            Pack.EndFlag = JTProtocol.EndFlagValue;

            if (!JTProtocol.CheckCRCCode(Pack, Crc16Ccitt))
                throw new Exception("CRC校验失败");

            var offset = 0;

            JTProtocol.Structures.ForEach(s => AnalysisStructure(Pack, s.Value, ref offset));

            Pack.Success = true;
            return Pack;
        }

        /// <summary>
        /// 分析结构
        /// </summary>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        /// <param name="offset">偏移量</param>
        private void AnalysisStructure(object obj, JTProtocol.Structure structure, ref int offset)
        {
            try
            {
                object value;

                Decrypt(Pack.Buffer, structure, offset);

                switch (structure.StructureType)
                {
                    case StructureType.@internal:
                        value = AnalysisInternalStructure(structure, ref offset);
                        break;
                    case StructureType.additional:
                        AnalysisAdditionalStructure(obj, structure, ref offset);
                        return;
                    case StructureType.@normal:
                    default:
                        value = AnalysisNormalStructure(structure, ref offset);
                        break;
                }

                if (!obj.ContainsProperty(structure.Property))
                    throw new Exception($"[{nameof(obj)}]不包含属性[{structure.Property}]");

                obj.SetValueToProperty(structure.Property, value);
            }
            catch (Exception ex)
            {
                throw new Exception($"分析结构 : 错误,Structure : [{structure.Property}]", ex);
            }
        }

        /// <summary>
        /// 分析普通结构
        /// </summary>
        /// <param name="structure">结构</param>
        /// <param name="offset">偏移量</param>
        /// <returns>获得的数据</returns>
        private object AnalysisNormalStructure(JTProtocol.Structure structure, ref int offset)
        {
            object value;
            //取值
            var bytes = new byte[structure.Length.Value];
            Buffer.BlockCopy(
                Pack.Buffer,
                offset,
                bytes,
                0,
                structure.Length.Value);

            //解码
            value = JTProtocol.Decode(bytes, structure.CodeInfo);

            //动态计算
            if (structure.NeedCompile)
                value = CompileDynamic(structure, value);

            offset += structure.Length.Value;
            return value;
        }

        /// <summary>
        /// 分析内部结构
        /// </summary>
        /// <param name="structure">结构</param>
        /// <param name="offset">偏移量</param>
        /// <returns>内部结构映射的实例</returns>
        private object AnalysisInternalStructure(JTProtocol.Structure structure, ref int offset)
        {
            try
            {
                object value;
                var InternalKey = string.Empty;
                if (structure.InternalPropertyForKey.IsNullOrEmpty())
                    InternalKey = structure.Internal.First().Key;
                else
                {
                    var PropertyValue = Pack.GetValueFromProperty(structure.InternalPropertyForKey.Property);
                    if (structure.InternalPropertyForKey.Encode != null)
                        PropertyValue = JTProtocol.Encode(PropertyValue, structure.InternalPropertyForKey.Encode);
                    if (structure.InternalPropertyForKey.Decode != null)
                        PropertyValue = JTProtocol.Decode((byte[])PropertyValue, structure.InternalPropertyForKey.Decode);
                    InternalKey = (string)PropertyValue;
                }

                if (!JTProtocol.InternalEntitysMappings.ContainsKey(InternalKey))
                    throw new Exception($"内部结构不存在[InternalKey : {InternalKey}]");

                //实例化内部结构的映射的实体类
                var InternalEntityMapping = JTProtocol.InternalEntitysMappings[InternalKey];
                var InternalEntityAssembly = InternalEntityMapping.Assembly.IsNullOrEmpty() ?
                    Assembly.GetExecutingAssembly() :
                    Assembly.Load(InternalEntityMapping.Assembly);

                value = InternalEntityAssembly.CreateInstance(InternalEntityMapping.TypeName, true);

                //递归处理内部结构
                var InternalOffset = offset;
                structure.Internal[InternalKey].ForEach(s => AnalysisStructure(value, s.Value, ref InternalOffset));
                offset = InternalOffset;

                return value;
            }
            catch (Exception ex)
            {
                throw new Exception($"分析内部结构 : 错误,Structure : [{structure.Property}]", ex);
            }
        }

        /// <summary>
        /// 分析附加信息
        /// </summary>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        /// <param name="offset">偏移量</param>
        private void AnalysisAdditionalStructure(object obj, JTProtocol.Structure structure, ref int offset)
        {
            do
            {
                var bytes = new byte[structure.Additional.Length];
                Buffer.BlockCopy(
                    Pack.Buffer,
                    offset,
                    bytes,
                    0,
                    structure.Additional.Length);

                var key = (string)JTProtocol.Decode(bytes, structure.Additional.CodeInfo);
                if (!structure.Additional.Switch.ContainsKey(key))
                    break;

                offset += structure.Additional.Length;

                Buffer.BlockCopy(
                    Pack.Buffer,
                    offset,
                    bytes,
                    0,
                    1);

                var length = (byte)JTProtocol.Decode(bytes, new CodeInfo() { CodeType = CodeType.@byte });

                offset++;

                var additionalStructure = structure.Additional.Switch[key];
                additionalStructure.Length = length;

                AnalysisStructure(obj, additionalStructure, ref offset);
            } while (offset < Pack.Buffer.Length);
        }

        /// <summary>
        /// 计算动态表达式
        /// </summary>
        /// <param name="structure"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        private object CompileDynamic(JTProtocol.Structure structure, object value, Type valueType = null, Type resultType = null)
        {
            ExpressionContext.Variables.Clear();
            ExpressionContext.Variables[structure.Property] = value;
            if (valueType != null)
                ExpressionContext.Variables.DefineVariable(structure.Property, valueType);
            if (resultType != null)
                ExpressionContext.Options.ResultType = resultType;
            return ExpressionContext.CompileDynamic(structure.CompileExpression).Evaluate();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="structure">结构</param>
        /// <param name="offset">偏移量</param>
        /// <returns></returns>
        private void Decrypt(byte[] buffer, JTProtocol.Structure structure, int offset)
        {
            try
            {
                if (JTProtocol.Encrypt.Targets?.ContainsKey(structure.Property) != true)
                    return;

                var encryptProperty = JTProtocol.Encrypt.Targets[structure.Property];
                var Flag = (bool)Pack.GetValueFromProperty(encryptProperty.Flag);
                if (!Flag)
                    return;

                var Length = buffer.Length - offset - JTProtocol.Structures.Where(o => o.Value.Order > structure.Order).Sum(o => o.Value.Length.Value);
                var Key = (UInt32)Pack.GetValueFromProperty(encryptProperty.Key);
                using (MemoryStream ms_encrypt = new MemoryStream())
                {
                    var Writer = new BinaryWriter(ms_encrypt, Encoding);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        var Reader = new BinaryReader(ms, Encoding);

                        //获取加密的数据
                        ms.Seek(offset, SeekOrigin.Begin);

                        //解密
                        while (ms.Position < Length)
                        {
                            var Char = Reader.ReadChar();

                            //将传输的数据与伪随机码按字节进行异或运算
                            Key = JTProtocol.Encrypt.IA1 * (Key % JTProtocol.Encrypt.M1) + JTProtocol.Encrypt.IC1;
                            Char ^= (Char)((Key << 20) & 0xff);

                            //写入解密的数据
                            Writer.Write(Char);
                        }
                    }
                    buffer = ms_encrypt.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"解密 : 错误,Structure : [{structure.Property}]", ex);
            }
        }
    }
}
