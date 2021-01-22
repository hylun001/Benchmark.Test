using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Benchmark.Dotnet
{
    /// <summary>
    ///  字符串拼接 性能对比   
    /// Method | Count |      Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
    /// ------ | ----- | ----- ---:| ---------:| ---------:| -------:| ------:| -----:| ---------:|
    ///  StringAdd | 100 | 14.893 us | 0.3994 us | 1.1461 us | 39.0015 | 1.1292 | - | 238.99 KB |
    /// StringBuilder | 100 | 1.547 us | 0.0384 us | 0.1070 us | 2.1744 | 0.1068 | - | 13.34 KB |
    /// StringFormat | 100 | 27.682 us | 0.5442 us | 0.9387 us | 39.0015 | 1.1292 | - | 239.06 KB |
    /// StringNew | 100 | 12.846 us | 0.2538 us | 0.2374 us | 39.0015 | 1.1292 | - | 238.99 KB |
    /// </summary>
    ///加上内存分析
    [MemoryDiagnoser]
    public class StringTest
    {
        [Params(100)]
        public int Count { get; set; }

        private static readonly string _Word = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff");

        /// <summary>
        /// Bad
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string StringAdd()
        {
            string result = string.Empty;
            for (int i = 0; i < Count; i++)
            {
                result += _Word;
            }

            return result;
        }

        /// <summary>
        /// Better
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string StringBuilder()
        {
            StringBuilder strBuf = new StringBuilder();
            for (int i = 0; i < Count; i++)
            {
                strBuf.Append(_Word);
            }

            return strBuf.ToString();
        }

        /// <summary>
        /// Good
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string StringFormat()
        {
            string result = string.Empty;
            for (int i = 0; i < Count; i++)
            {
                result = string.Format("{0}{1}", result, _Word);
            }

            return result;
        }

        /// <summary>
        /// Good
        /// </summary>
        /// <returns></returns>
        [Benchmark]
        public string StringNew()
        {
            string result = string.Empty;
            for (int i = 0; i < Count; i++)
            {
                result = $"{result}{_Word}";
            }

            return result;
        }

    }
}
