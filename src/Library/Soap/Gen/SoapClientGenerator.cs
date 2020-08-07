using Library.Soap.Application;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace Library.Soap.Gen
{
    /// <summary>
    /// Soap构造器
    /// </summary>
    public class SoapClientGenerator : ISoapClientProvider
    {
        private readonly SaopClientGeneratorOptions _options;

        private readonly ConcurrentDictionary<Type, object> Clients;

        private readonly ConcurrentDictionary<string, Type> NameTypeMapping;

        public SoapClientGenerator(SaopClientGeneratorOptions options)
        {
            _options = options ?? new SaopClientGeneratorOptions();
            Clients = new ConcurrentDictionary<Type, object>();
            NameTypeMapping = new ConcurrentDictionary<string, Type>();
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            var channelFactoryType = typeof(ChannelFactory<>);

            _options.SoapClients.ForEach(client =>
            {
                var binding = new BasicHttpBinding();
                var endpoint = new EndpointAddress(new Uri(client.Uri));

                var channelType = Assembly
                                        .Load(client.ServiceType.Assembly)
                                        .GetType(client.ServiceType.Type, true, true);

                var _channelFactoryType = channelFactoryType.MakeGenericType(channelType);
                var channelFactory = Activator.CreateInstance(_channelFactoryType, new object[] { binding, endpoint });

                var channel = _channelFactoryType.InvokeMember("CreateChannel", BindingFlags.Default | BindingFlags.InvokeMethod, null, channelFactory, null);

                Clients.AddOrUpdate(channelType, channel, (type, oldClient) => channel);
                NameTypeMapping.AddOrUpdate(client.Name, channelType, (name, oldType) => channelType);
            });
        }


        public object GetClient(string name)
        {
            if (!NameTypeMapping.ContainsKey(name))
                throw new SoapException($"名称 {name} 没有对应的类型.");

            return GetClient(NameTypeMapping[name]);
        }


        public object GetClient(Type type)
        {
            if (!Clients.ContainsKey(type))
                throw new SoapException($"类型 {type.FullName} 没有对应的客户端.");

            return Clients[type];
        }


        public TChannel GetClient<TChannel>() where TChannel : class
        {
            return GetClient(typeof(TChannel)) as TChannel;
        }


        public IEnumerable<(string Name, Type Type, object Client)> GetClients()
        {
            return NameTypeMapping.Select(mapping => (mapping.Key, mapping.Value, Clients[mapping.Value]));
        }
    }
}
