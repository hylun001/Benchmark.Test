using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Text;

namespace Benchmark.Dotnet
{
    /// <summary>
    ///   Method |     Mean |     Error |    StdDev 
    ///----------- |---------:|----------:|----------:
    ///        For | 6.793 ms | 0.1348 ms | 0.2177 ms 
    ///    Foreach | 4.191 ms | 0.0583 ms | 0.0517 ms 
    /// TPLForeach |       NA |        NA |        NA 
    /// </summary>
    [Config(typeof(AllowNonOptimized))]
    [MemoryDiagnoser]
    public class IteratorTest
    {
        private readonly List<string> _list = new List<string>();

        public IteratorTest()
        {
            for (int i = 0; i < 100000; i++)
            {
                _list.Add(i.ToString());
            }
        }

        [Benchmark]
        public void For()
        {
            StringBuilder strBuf = new StringBuilder();
            int count = _list.Count;
            for (int i = 0; i < count; i++)
            {
                strBuf.AppendFormat(_list[i]);
            }
        }
        /// <summary>
        /// Better
        /// </summary>
        [Benchmark]
        public void Foreach()
        {
            StringBuilder strBuf = new StringBuilder();
            _list.ForEach(item =>
            {
                strBuf.AppendFormat(item);
            });
        }
    }
}
