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
    /// kafka生产者类
    /// </summary>
    /// <typeparam name="TKey">标识</typeparam>
    /// <typeparam name="TValue">消息值</typeparam>
    public class KafkaProducer<TKey, TValue> where TKey : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config">配置</param>
        public KafkaProducer(ProducerConfig config)
        {
            Producer = new ProducerBuilder<TKey, TValue>(config)
             .SetErrorHandler((producer, error) =>
             {
                 Logger.Log(
                     NLog.LogLevel.Trace,
                     LogType.系统跟踪,
                     $"Kafka, Name[{producer.Name}]: producer exception.",
                     null,
                     new ApplicationException($"Code: {error.Code}, Reason: {error.Reason}."));
             })
             .Build();

            CTS = new Dictionary<object, CancellationTokenSource>();

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
        /// 生产者
        /// </summary>
        private readonly IProducer<TKey, TValue> Producer;

        /// <summary>
        /// 取消生产令牌集合
        /// </summary>
        private readonly Dictionary<object, CancellationTokenSource> CTS;

        /// <summary>
        /// 生产
        /// </summary>
        /// <param name="topic">订阅</param>
        /// <param name="key">标识</param>
        /// <param name="messageValue">消息值</param>
        /// <param name="timeout">超时时间（默认10秒）</param>
        public async Task Produce(string topic, object key, TValue messageValue, TimeSpan timeout = default)
        {
            if (messageValue == null)
                throw new ApplicationException("消息不能为空");

            if (CTS.ContainsKey(key))
                throw new ApplicationException("消息不可重复");

            CTS.Add(key, new CancellationTokenSource());

            await Producer.ProduceAsync(topic, typeof(TKey) == typeof(Null) ? new Message<TKey, TValue>() { Value = messageValue } : new Message<TKey, TValue>() { Key = key as TKey, Value = messageValue }, CTS[key].Token)
                 .ContinueWith(task =>
                 {
                     CTS.Remove(key);

                     if (task.IsFaulted)
                         Logger.Log(
                             NLog.LogLevel.Error,
                             LogType.系统异常,
                             $"Kafka, Topic[{topic}]: produce exception.",
                             null,
                             task.Exception);
                     else
                         Logger.Log(
                             NLog.LogLevel.Trace,
                             LogType.系统跟踪,
                             $"Kafka, Topic[{task.Result.Topic}] produce message, \r\n" +
                             $"Status: {task.Result.Status}, \r\n" +
                             $"Partition: {task.Result.Partition}, \r\n" +
                             $"Offset : {task.Result.Offset}, \r\n" +
                             $"Key: {task.Result.Message.Key}, \r\n" +
                             $"Value: {task.Result.Message.Value}, \r\n" +
                             $"Time: {task.Result.Timestamp}",
                             null);
                 });

            if (timeout == default)
                timeout = TimeSpan.FromSeconds(10);

            Producer.Flush(timeout);
        }
    }
}
