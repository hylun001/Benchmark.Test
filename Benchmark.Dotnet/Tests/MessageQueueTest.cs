using BenchmarkDotNet.Attributes;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmark.Dotnet.Tests
{

    [Config(typeof(AllowNonOptimized))]
    [MemoryDiagnoser]
    public class MessageQueueTest
    {
        private readonly int _messageSize = 1000;

        public MessageQueueTest()
        {
            Task.Factory.StartNew(() =>
            {
                MessageQueue.OnProcessQueue += (msg) =>
                {
                    //if (msg == "999")
                    //{
                    //    Console.WriteLine("server recv1 {0}", msg);
                    //}
                };

                var responseSocket = new ResponseSocket("tcp://*:5560");
                {
                    while (true)
                    {
                        var message = responseSocket.ReceiveFrameString();
                        //if (message == "999")
                        //{
                        //    Console.WriteLine("server recv2 {0}", message);
                        //}

                        responseSocket.SendFrame("world");
                    }
                }


            });
        }

        [Benchmark]
        public void ZeromqTest()
        {

            using (var requestSocket = new RequestSocket("tcp://localhost:5560"))
            {
                for (int i = 0; i < _messageSize; i++)
                {
                    requestSocket.SendFrame($"{i.ToString()}");
                    requestSocket.ReceiveFrameString();
                }
            }

        }


        [Benchmark]
        public void CustomMqTest()
        {
            for (int i = 0; i < _messageSize; i++)
            {
                MessageQueue.Add($"{i.ToString()}");
            }
        }
    }


    /// <summary>
    /// 消息队列缓存类
    /// </summary>
    public sealed class MessageQueue
    {
        static MessageQueue()
        {
            msgQueue = new ConcurrentQueue<string>();
            Task.Factory.StartNew(() =>
            {
                OnRouteQueueProcess();

            });
        }

        /// <summary>
        /// 实时视频请求消息队列
        /// </summary>
        public static ConcurrentQueue<string> msgQueue { get; set; }


        private static readonly object objLock = new object();
        /// <summary>
        /// 处理单个任务
        /// </summary>
        public static Action<string> OnProcessQueue;
        /// <summary>
        /// 
        /// </summary>
        private static void OnRouteQueueProcess()
        {
            string temp = null;
            while (true)
            {
                //判断消息队列是否为空
                if (!msgQueue.IsEmpty)
                {

                    if (msgQueue.TryDequeue(out temp))
                    {
                        OnProcessQueue(temp);
                    }
                }

            }
        }

        /// <summary>
        /// 总共数量
        /// </summary>
        public static int Count
        {
            get { return msgQueue.Count; }
        }

        /// <summary>
        /// 添加到队列结尾
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msgContext"></param>
        public static void Add(string msgContext)
        {
            msgQueue.Enqueue(msgContext);

        }

    }
}
