using Benchmark.Dotnet.Tests;
using BenchmarkDotNet.Running;
using System;

namespace Benchmark.Dotnet
{
    /// <summary>
    /// 性能测试
    /// https://benchmarkdotnet.org/articles/overview.html
    /// 测试结果见："运行目录\BenchmarkDotNet.Artifacts\" 文件夹
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {

            //(new MessageQueueTest()).ZeromqTest();
            // p:BuildInParallel = false
            //BenchmarkRunner.Run<StringTest>(new AllowNonOptimized());
            //BenchmarkRunner.Run<SingleItemTest>(new AllowNonOptimized());
            //BenchmarkRunner.Run<IteratorTest>(new AllowNonOptimized());
            //BenchmarkRunner.Run<MultiThreadTest>(new AllowNonOptimized());
            //BenchmarkRunner.Run<ObjectMapperTest>(new AllowNonOptimized());
             BenchmarkRunner.Run<MessageQueueTest>(new AllowNonOptimized()); 

            Console.Read();
        }
    }







}
