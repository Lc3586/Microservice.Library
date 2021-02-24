using Business.Utils.Log;
using Confluent.Kafka;
using Model.Utils.Log;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Business.Utils.Kafka
{
    /// <summary>
    /// kafka消费者类
    /// </summary>
    /// <typeparam name="TKey">标识</typeparam>
    /// <typeparam name="TValue">消息值</typeparam>
    public class KafkaConsume<TKey, TValue>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">配置</param>
        public KafkaConsume(ConsumerConfig config)
        {
            Consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetErrorHandler((consumer, error) =>
                {
                    Logger.Log(
                        NLog.LogLevel.Trace,
                        LogType.系统跟踪,
                        $"Kafka, MemberId[{consumer.MemberId}]: consumer exception.",
                        null,
                        new ApplicationException($"Code: {error.Code}, Reason: {error.Reason}."));
                })
                .Build();

            CTS = new Dictionary<string, CancellationTokenSource>();

            //控制台关闭时取消全部订阅
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;
                foreach (var cts in CTS)
                {
                    cts.Value.Cancel();
                }
            };
        }

        /// <summary>
        /// 消费者
        /// </summary>
        private readonly IConsumer<TKey, TValue> Consumer;

        /// <summary>
        /// 取消订阅令牌集合
        /// </summary>
        private readonly Dictionary<string, CancellationTokenSource> CTS;

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <remarks>
        /// <para>参数1: 订阅</para>
        /// <para>参数2: 消息</para>
        /// </remarks>
        public Action<string, Message<TKey, TValue>> ReceiveMessage;

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="topics">订阅</param>
        public KafkaConsume<TKey, TValue> Subscribe(params string[] topics)
        {

            foreach (var topic in topics)
            {
                if (!CTS.ContainsKey(topic))
                    CTS.Add(topic, new CancellationTokenSource());

                Task.Run(() =>
                {
                    Consume(topic);
                });
            }

            Consumer.Subscribe(topics);

            return this;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="topics">订阅</param>
        public KafkaConsume<TKey, TValue> Cancle(params string[] topics)
        {
            foreach (var topic in topics)
            {
                if (!CTS.ContainsKey(topic))
                    continue;
                CTS[topic].Cancel();
                CTS.Remove(topic);
            }

            return this;
        }

        /// <summary>
        /// 消费
        /// </summary>
        /// <param name="topic">订阅</param>
        private void Consume(string topic)
        {
            while (CTS.ContainsKey(topic))
            {
                try
                {
                    var result = Consumer.Consume(CTS[topic].Token);
                    ReceiveMessage?.Invoke(topic, result.Message);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    Logger.Log(
                        NLog.LogLevel.Error,
                        LogType.系统异常,
                        $"Kafka, Topic[{topic}]: consume exception.",
                        null,
                        ex);
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }
    }
}
