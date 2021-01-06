using Model.Example.SoapDTO;
using SoapServices.Example;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Api.Services.Example
{
    public class SampleService : ISampleService
    {
        public string Ping(string s)
        {
            Console.WriteLine("Exec ping method");
            return s;
        }

        public Response PingComplexModel(Input inputModel)
        {
            Console.WriteLine("Input data. IntProperty: {0}, StringProperty: {1}", inputModel.IntProperty, inputModel.StringProperty);

            return new Response
            {
                FloatProperty = float.MaxValue / 2,
                StringProperty = inputModel.StringProperty,
                ListProperty = inputModel.ListProperty,
                DateTimeOffsetProperty = inputModel.DateTimeOffsetProperty
            };
        }

        public void VoidMethod(out string s)
        {
            s = "Value from server";
        }

        public Task<int> AsyncMethod()
        {
            return Task.Run(() => 42);
        }

        public int? NullableMethod(bool? arg)
        {
            return null;
        }

        public void XmlMethod(XElement xml)
        {
            Console.WriteLine(xml.ToString());
        }
    }
}
