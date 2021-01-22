using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace Benchmark.Dotnet
{
    /// <summary>
    /// Single 与 First 性能对比
    ///          Method |          Mean |       Error |      StdDev |
    ///---------------- |--------------:|------------:|------------:|
    /// SingleOrDefault | 12,871.542 us | 189.7420 us | 158.4431 us |
    ///  FirstOrDefault |      1.390 us |   0.0265 us |   0.0490 us |
    /// </summary>
    /// 设置导出文件格式
    [AsciiDocExporter, CsvExporter, RPlotExporter]
    ///设置同时运行的框架 <TargetFrameworks>net5.0;net472;netcoreapp3.1</TargetFrameworks> 与 SimpleJob一起使用
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp31)]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net472)]
    ///加上内存分析
    [MemoryDiagnoser]
    public class SingleItemTest
    {

        private readonly int _haystackSize = 1000000;
        private readonly List<string> _haystack = new List<string>();
        private readonly string _needle = "needle";

        //[Params("needle100","abcd12345")]
        [Params("needle100")]
        public string Key { get; set; }

        public SingleItemTest()
        {
            for (int i = 0; i < _haystackSize; i++)
            {
                _haystack.Add($"{_needle}{i.ToString()}");
            }
        }


        [Benchmark]
        public string SingleOrDefault()
        {
            return _haystack.SingleOrDefault(x => x == Key);
        }

        /// <summary>
        /// Better
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string FirstOrDefault()
        {
            return _haystack.FirstOrDefault(x => x == Key);
        }

    }
}
