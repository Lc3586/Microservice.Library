using Flee.PublicTypes;
using Library.Extension;
using Library.SuperSocket.Extension;
using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;

namespace Library.SuperSocket.JTProtocol
{
    /// <summary>
    /// JT协议编码器
    /// </summary>
    public class JTEncoder : IPackageEncoder<MessagePackageInfo>
    {
        static JTEncoder()
        {
            ExpressionContext = new ExpressionContext();
            ExpressionContext.Imports.AddType(typeof(Math));
            Msg_SN = 1;
        }

        public JTEncoder(JTProtocol jTProtocol)
        {
            JTProtocol = jTProtocol;
            Encoding = Encoding.GetEncoding(JTProtocol.Encoding);
            if (CrcCcitt == null || CrcCcitt.initialCrcValue != JTProtocol.CrcCcitt.InitialCrcValue)
                CrcCcitt = new CrcCcitt(JTProtocol.CrcCcitt.CrcLength, JTProtocol.CrcCcitt.InitialCrcValue);
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
        private static ExpressionContext ExpressionContext;

        /// <summary>
        /// CRC数据校验
        /// </summary>
        private static CrcCcitt CrcCcitt;

        /// <summary>
        /// 报文序列号
        /// </summary>
        private static UInt32 Msg_SN { get; set; }

        /// <summary>
        /// 消息包编码
        /// </summary>
        /// <param name="writer">流数据写入器</param>
        /// <param name="pack">消息包</param>
        /// <returns></returns>
        public int Encode(IBufferWriter<byte> writer, MessagePackageInfo pack)
        {
            if (pack == null)
                throw new Exception("消息包为空");
            if (!JTProtocol.Structures.Any_Ex())
                throw new Exception($"{JTProtocol.Name}协议配置错误");

            Pack = pack;

            MessagePackageToBuffer();

            //帧头
            writer.Write(Pack.HeadFlag);

            //消息头 + 消息体
            writer.Write(JTProtocol.Escape(Pack.Buffer));

            //CRC校验码
            writer.Write(JTProtocol.Encode(Pack.CRCCode, new CodeInfo() { CodeType = CodeType.uint16 }));

            //帧尾
            writer.Write(Pack.EndFlag);

            return Pack.Buffer.Length + 4;
        }

        /// <summary>
        /// 消息包转流数据
        /// </summary>
        private void MessagePackageToBuffer()
        {
            try
            {
                Pack.HeadFlag = JTProtocol.HeadFlagValue;
                Pack.EndFlag = JTProtocol.EndFlagValue;
                //先处理消息体再处理消息头
                MessageBodyToBuffer();
                MessageHeaderToBuffer();
                Pack.Buffer = Pack.MessageHeader.Buffer.Concat(Pack.MessageBody.Buffer).ToArray();
                Pack.CRCCode = CrcCcitt.ComputeChecksumBytes(Pack.Buffer);
            }
            catch (Exception ex)
            {
                throw new Exception("消息包转流数据 : 错误", ex);
            }
        }

        /// <summary>
        /// 消息头转流数据
        /// </summary>
        private void MessageHeaderToBuffer()
        {
            InitMessageHeader();
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms, Encoding);
                if (!JTProtocol.Structures.ContainsKey("MessageHeader"))
                    throw new Exception("消息头转流数据 : 协议中未找到消息头结构信息");
                writer.Write(AnalysisStructure(Pack, JTProtocol.Structures["MessageHeader"]));
                Pack.MessageHeader.Buffer = ms.ToArray();
            }
        }

        /// <summary>
        /// 获取报文序列号
        /// </summary>
        /// <returns></returns>
        private UInt32 GetMsgSN()
        {
            return Msg_SN++;
        }

        /// <summary>
        /// 初始化消息头
        /// </summary>
        public void InitMessageHeader()
        {
            try
            {
                if (Pack.MessageHeader == null)
                    Pack.MessageHeader = new MessageHeader();
                if (Pack.MessageBody?.Buffer == null)
                    throw new Exception("消息体为空");
                Pack.MessageHeader.MsgLength = 1 + 22 + (UInt32)Pack.MessageBody.Buffer.Length + 2 + 1;
                Pack.MessageHeader.Msg_SN = GetMsgSN();

                //协议号
                if (!JTProtocol.Structures.ContainsKey("MessageBody"))
                    throw new Exception($"消息体结构不存在[MessageBody]");

                var TypeName = Pack.MessageBody.GetType().FullName;
                if (!JTProtocol.InternalEntitysMappings.Any(o => o.Value.TypeName == TypeName))
                    throw new Exception($"消息体结构不存在[TypeName : {TypeName}]");

                var InternalEntitysMapping = JTProtocol.InternalEntitysMappings.First(o => o.Value.TypeName == TypeName);

                var Structure = JTProtocol.Structures["MessageBody"];
                object PropertyValue = InternalEntitysMapping.Key;
                if (Structure.InternalPropertyForKey.Encode != null)
                    PropertyValue = JTProtocol.Encode(PropertyValue, Structure.InternalPropertyForKey.Decode);
                if (Structure.InternalPropertyForKey.Encode != null)
                    PropertyValue = JTProtocol.Decode((byte[])PropertyValue, Structure.InternalPropertyForKey.Encode);
                Pack.SetValueToProperty(Structure.InternalPropertyForKey.Property, PropertyValue);

                ////秘钥
                //JTProtocol.Encrypt?.Targets?.Values.ForEach(o =>
                //{
                //    if (!(bool)Pack.GetValueFromProperty(o.Flag))
                //        return;
                //    //生成Key
                //    var Key_bytes = new byte[4];
                //    new Random().NextBytes(Key_bytes);
                //    var Key = BitConverter.ToUInt32(Key_bytes);
                //    Pack.SetValueToProperty(o.Key, Key);
                //});
            }
            catch (Exception ex)
            {

                throw new Exception("构建消息头 : 错误", ex); ;
            }
        }

        /// <summary>
        /// 消息体转流数据
        /// </summary>
        private void MessageBodyToBuffer()
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms, Encoding);
                if (!JTProtocol.Structures.ContainsKey("MessageBody"))
                    throw new Exception("消息体转流数据 : 协议中未找到消息体结构信息");
                writer.Write(AnalysisStructure(Pack, JTProtocol.Structures["MessageBody"]));
                Pack.MessageBody.Buffer = ms.ToArray();
            }
        }

        /// <summary>
        /// 分析结构
        /// </summary>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        private byte[] AnalysisStructure(object obj, JTProtocol.Structure structure)
        {
            try
            {
                if (structure.StructureType != StructureType.additional && !obj.ContainsProperty(structure.Property))
                    throw new Exception($"[{nameof(obj)}]不包含属性[{structure.Property}]");

                byte[] value;

                switch (structure.StructureType)
                {
                    case StructureType.@internal:
                        value = AnalysisInternalStructure(obj, structure);
                        break;
                    case StructureType.additional:
                        value = AnalysisAdditionalStructure(obj, structure);
                        break;
                    case StructureType.@normal:
                    default:
                        value = AnalysisNormalStructure(obj, structure);
                        break;
                }

                Encrypt(value, structure);

                return value;
            }
            catch (Exception ex)
            {
                throw new Exception($"分析结构 : 错误,Structure : [{structure.Property}]", ex);
            }
        }

        /// <summary>
        /// 分析普通结构
        /// </summary>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        /// <returns>获得的数据</returns>
        private byte[] AnalysisNormalStructure(object obj, JTProtocol.Structure structure)
        {
            return JTProtocol.Encode(obj.GetValueFromProperty(structure.Property), structure.CodeInfo).PadRight(structure.Length.HasValue ? structure.Length.Value : 0);
        }

        /// <summary>
        /// 分析内部结构
        /// </summary>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        /// <returns>内部结构映射的实例</returns>
        private byte[] AnalysisInternalStructure(object obj, JTProtocol.Structure structure)
        {
            try
            {
                var InternalEntity = obj.GetValueFromProperty(structure.Property);

                var TypeName = InternalEntity.GetType().FullName;
                if (!JTProtocol.InternalEntitysMappings.Any(o => o.Value.TypeName == TypeName))
                    throw new Exception($"内部结构不存在[TypeName : {TypeName}]");

                var InternalEntitysMapping = JTProtocol.InternalEntitysMappings.First(o => o.Value.TypeName == TypeName);

                if (!structure.InternalPropertyForKey.IsNullOrEmpty())
                {
                    var PropertyValue = Pack.GetValueFromProperty(structure.InternalPropertyForKey.Property);
                    if (PropertyValue == null)
                    {
                        PropertyValue = InternalEntitysMapping.Key;
                        if (structure.InternalPropertyForKey.Encode != null)
                            PropertyValue = JTProtocol.Encode(PropertyValue, structure.InternalPropertyForKey.Decode);
                        if (structure.InternalPropertyForKey.Decode != null)
                            PropertyValue = JTProtocol.Decode((byte[])PropertyValue, structure.InternalPropertyForKey.Encode);
                        Pack.SetValueToProperty(structure.InternalPropertyForKey.Property, PropertyValue);
                    }
                }

                using (var ms = new MemoryStream())
                {
                    var writer = new BinaryWriter(ms, Encoding);
                    //处理内部结构
                    structure.Internal[InternalEntitysMapping.Key].ForEach(s => writer.Write(AnalysisStructure(InternalEntity, s.Value)));
                    return ms.ToArray();
                }
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
        private byte[] AnalysisAdditionalStructure(object obj, JTProtocol.Structure structure)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new BinaryWriter(ms, Encoding);
                foreach (var item in structure.Additional.Switch)
                {
                    if (!obj.ContainsProperty(item.Value.Property))
                        continue;

                    if (obj.GetValueFromProperty(item.Value.Property) == null)
                        continue;

                    var value = AnalysisStructure(obj, item.Value);

                    writer.Write(JTProtocol.Encode(item.Key, structure.Additional.CodeInfo));
                    writer.Write(JTProtocol.Encode(Convert.ToByte(value.Length), new CodeInfo() { CodeType = CodeType.@byte }));
                    writer.Write(value);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="buffer">数据</param>
        /// <param name="obj">当前实例</param>
        /// <param name="structure">结构</param>
        /// <returns></returns>
        private void Encrypt(byte[] buffer, JTProtocol.Structure structure)
        {
            try
            {
                if (JTProtocol.Encrypt.Targets?.ContainsKey(structure.Property) != true)
                    return;

                var encryptProperty = JTProtocol.Encrypt.Targets[structure.Property];
                if (!(bool)Pack.GetValueFromProperty(encryptProperty.Flag))
                    return;

                //生成Key
                var Key_bytes = new byte[4];
                new Random().NextBytes(Key_bytes);
                var Key = BitConverter.ToUInt32(Key_bytes);

                //写入Key
                Pack.SetValueToProperty(encryptProperty.Key, Key);

                //var Key = (UInt32)Pack.GetValueFromProperty(encryptProperty.Key);

                using (MemoryStream ms_encrypt = new MemoryStream())
                {
                    var Writer = new BinaryWriter(ms_encrypt, Encoding);
                    using (MemoryStream ms = new MemoryStream(buffer))
                    {
                        var Reader = new BinaryReader(ms, Encoding);

                        while (ms.Position < ms.Length)
                        {
                            var Char = Reader.ReadChar();

                            //将待传输的数据与伪随机码按字节进行异或运算
                            Key = JTProtocol.Encrypt.IA1 * (Key % JTProtocol.Encrypt.M1) + JTProtocol.Encrypt.IC1;
                            Char ^= (Char)((Key >> 20) & 0xff);

                            //写入加密的数据
                            Writer.Write(Char);
                        }
                    }
                    buffer = ms_encrypt.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加密 : 错误,Structure : [{structure.Property}]", ex);
            }
        }
    }
}
