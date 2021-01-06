using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

/*
 * 示例Soap服务业务模型
*/
namespace Model.Example.SoapDTO
{
    /// <summary>
    /// 输出
    /// </summary>
    [DataContract]
    public class Response
    {

        [DataMember]
        public float FloatProperty { get; set; }

        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public List<string> ListProperty { get; set; }

        [DataMember]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [DataMember]
        public SoapTestEnum TestEnum { get; set; }
    }

    /// <summary>
    /// 输入
    /// </summary>
    [DataContract]
    public class Input
    {
        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public int IntProperty { get; set; }

        [DataMember]
        public List<string> ListProperty { get; set; }

        [DataMember]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [DataMember]
        public List<Object> ComplexListProperty { get; set; }
    }

    /// <summary>
    /// 对象
    /// </summary>
    [DataContract]
    public class Object
    {
        [DataMember]
        public string StringProperty { get; set; }

        [DataMember]
        public int IntProperty { get; set; }
    }
}
