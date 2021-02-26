using Microservice.Library.Extension;
using SoapCore;
using SoapCore.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Api.Filter.Soap
{
    public class MessageFilter : IMessageFilter
    {
        public void OnRequestExecuting(Message message)
        {

        }

        public void OnResponseExecuting(Message message)
        {

        }
    }
}
