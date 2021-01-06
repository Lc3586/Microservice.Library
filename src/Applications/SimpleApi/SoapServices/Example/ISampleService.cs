using Model.Example.SoapDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SoapServices.Example
{
    [ServiceContract]
    public interface ISampleService
    {
        [OperationContract]
        string Ping(string s);

        [OperationContract]
        Response PingComplexModel(Input inputModel);

        [OperationContract]
        void VoidMethod(out string s);

        [OperationContract]
        Task<int> AsyncMethod();

        [OperationContract]
        int? NullableMethod(bool? arg);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
    }
}
